using Microsoft.Data.SqlClient;
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

    ShowConnectionStatus(connection);

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
CreatingCommandObjects();