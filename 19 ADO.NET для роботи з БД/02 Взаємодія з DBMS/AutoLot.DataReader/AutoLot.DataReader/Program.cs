using Microsoft.Data.SqlClient;

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
UseDataReader();


static void ShowConnectionStatus(SqlConnection connection)
{
    Console.WriteLine("\n\tInfo about your connection\n");
    Console.WriteLine($"Database location : {connection.DataSource}");
    Console.WriteLine($"Database : {connection.Database}");
    Console.WriteLine($"Timeout : {connection.ConnectionTimeout}");
    Console.WriteLine($"State : {connection.State}");
    Console.WriteLine();
}

