using System.Data.Common;
using System.Data.Odbc;
#if PC
using System.Data.OleDb;
#endif
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using DataProviderFactory;

static void Run()
{
    PrintOutSimpleList();
}
Run();

static void PrintOutSimpleList()
{
    var (provider, connectionString) = GetProviderFromConfiguration();

    DbProviderFactory factory = GetDbProviderFactory(provider);

    // Get the connection object
    using DbConnection? connection = factory.CreateConnection();
    Console.WriteLine($"Your connection object is : {connection?.GetType().Name}");

    if (connection == null) return;

    // Opening a connection to the database
    connection.ConnectionString = connectionString;
    connection.Open();

    // Make command object
    DbCommand? command = factory.CreateCommand();
    Console.WriteLine($"Your command object is a : {command?.GetType().Name}");

    if (command == null) return;

    command.Connection = connection;
    command.CommandText =
        "Select i.Id, m.Name From Inventory " +
        "i inner join Makes m on m.Id = i.MakeId";

    // Make data reader
    using DbDataReader reader = command.ExecuteReader();
    Console.WriteLine($"Your data reader object is a : {reader.GetType().Name}");

    Console.WriteLine("\n\t\tInventory\n");
    //Print out data
    while (reader.Read())
    {
        Console.WriteLine($"\t{reader["Id"]}\t{reader["Name"]} ");
    }
}


static (DataProviderEnum Provider, string? ConnectionString) GetProviderFromConfiguration()
{
    IConfiguration config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", true, true)
        .Build();

    string? providerName = config["ProviderName"];

    if (Enum.TryParse<DataProviderEnum>(providerName, out DataProviderEnum provider))
    {
        return (provider, config[$"{providerName}:ConnectionString"]);
    }
    throw new Exception("Invalid data provider value supplied.");
}

static DbProviderFactory GetDbProviderFactory(DataProviderEnum provider)
	=> provider switch
	{
		DataProviderEnum.SqlServer => SqlClientFactory.Instance,
		DataProviderEnum.Odbc => OdbcFactory.Instance,
#if PC
        DataProviderEnum.OleDb => OleDbFactory.Instance,
#endif
		_ => SqlClientFactory.Instance

    };

