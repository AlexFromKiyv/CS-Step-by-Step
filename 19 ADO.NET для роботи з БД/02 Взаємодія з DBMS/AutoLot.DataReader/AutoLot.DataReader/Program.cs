using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml.Serialization;

static void UseDataReader()
{
    using SqlConnection connection = new();

    connection.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=AutoLot";
    connection.Open();

    ShowConnectionStatus(connection);

    string sql = @"Select i.id, m.Name as Make, i.Color, i.Petname
                   FROM Inventory i
                   INNER JOIN Makes m on m.Id = i.MakeId";

    SqlCommand myCommand = new(sql, connection);

    using SqlDataReader myDataReader = myCommand.ExecuteReader();

    while (myDataReader.Read())
    {
        Console.WriteLine($"{myDataReader["Make"]} {myDataReader["Color"]} {myDataReader["PetName"]}");
    }

}
//UseDataReader();


static void ShowConnectionStatus(SqlConnection connection)
{
    Console.WriteLine("\n\tInfo about your connection\n");
    Console.WriteLine($"Database location : {connection.DataSource}");
    Console.WriteLine($"Database : {connection.Database}");
    Console.WriteLine($"Timeout : {connection.ConnectionTimeout}");
    Console.WriteLine($"State : {connection.State}");
    Console.WriteLine();
}

static void UsingSqlConnectionStringBuilder()
{
    using SqlConnection connection = new();

    var connectionStringBuilder = new SqlConnectionStringBuilder()
    {
        InitialCatalog = "AutoLot",
        DataSource = "(localdb)\\mssqllocaldb",
        IntegratedSecurity = true,
        ConnectTimeout = 30,
        Encrypt = false,
    };

    Console.WriteLine(connectionStringBuilder.ConnectionString);

    connection.ConnectionString = connectionStringBuilder.ConnectionString;
    connection.Open();

    ShowConnectionStatus(connection);

}
//UsingSqlConnectionStringBuilder();


static void CreatingCommandObjects()
{
 
    using SqlConnection connection = new();

    connection.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=AutoLot";
    connection.Open();

    // Create command object via ctor args.
    string sql1 = @"Select i.id, m.Name as Make, i.Color, i.Petname
                   FROM Inventory i
                   INNER JOIN Makes m on m.Id = i.MakeId";
    SqlCommand myCommand1 = new(sql1, connection);

    // Create another command object via properties.
    SqlCommand myCommand2 = new();
    myCommand2.Connection = connection;
    myCommand2.CommandText = "Select m.id, m.Name from Makes m";


}
//CreatingCommandObjects();

static void ObtainDataReader()
{
    using SqlConnection connection = new();

    connection.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=AutoLot";
    connection.Open();

    SqlCommand myCommand = new();
    myCommand.Connection = connection;
    myCommand.CommandText = "Select m.id, m.Name from Makes m";

    using SqlDataReader dataReader = myCommand.ExecuteReader();

    dataReader.Read();
    Console.WriteLine($"{dataReader["id"]} {dataReader["Name"]}");

    Console.WriteLine();
    while (dataReader.Read())
    {
        Console.WriteLine($"{dataReader["id"]} {dataReader["Name"]}");
    }
    dataReader.Close();

    Console.WriteLine();

    string sql1 = @"Select i.id, m.Name as Make, i.Color, i.Petname
                   FROM Inventory i
                   INNER JOIN Makes m on m.Id = i.MakeId";
    SqlCommand myCommand1 = new(sql1, connection);

    using SqlDataReader dataReader1 = myCommand1.ExecuteReader();

    while (dataReader1.Read())
    {
        for (int i = 0; i < dataReader1.FieldCount; i++)
        {
            Console.Write($"{dataReader1.GetName(i)} = {dataReader1.GetValue(i)}\t");
        }
        Console.WriteLine();
    }
}
//ObtainDataReader();

static void MultipleResultSetsWithDataReader()
{
    using SqlConnection connection = new();

    connection.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=AutoLot";
    connection.Open();

    SqlCommand myCommand = new();
    myCommand.Connection = connection;
    myCommand.CommandText = "Select m.id, m.Name from Makes m; Select * from Customers";

    using SqlDataReader dataReader = myCommand.ExecuteReader();
    do
    {
        while (dataReader.Read())
        {
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                Console.Write($"{dataReader.GetName(i)} = {dataReader.GetValue(i)}\t");
            }
            Console.WriteLine();
        }
    } while (dataReader.NextResult());
}
//MultipleResultSetsWithDataReader();

static async Task ReadDataFromDBAsync()
{
    string connectionString = "Data Source=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=AutoLot";

    using SqlConnection connection = new(connectionString);

    string sql = "Select * From Customers";

    await connection.OpenAsync();

    SqlCommand command = new(sql, connection);

    SqlDataReader reader = await command.ExecuteReaderAsync();

    Console.WriteLine($"{reader.GetName(0)}\t{reader.GetName(1)}\t{reader.GetName(2)}");

    while (await reader.ReadAsync())
    {
        Console.WriteLine($"{reader.GetInt32("Id")}\t{reader.GetString("FirstName")}\t{reader.GetString("LastName")}");
    }
}
await ReadDataFromDBAsync();
