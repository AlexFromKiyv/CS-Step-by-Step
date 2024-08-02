using Microsoft.Data.Sqlite;

void ConnectionToDb()
{
    using SqliteConnection connection = new("DataSource=D:\\Temp\\users.db");
    connection.Open();
    Console.WriteLine(connection.State);
    Console.WriteLine(connection.Database);
    Console.WriteLine(connection.DataSource);
    Console.WriteLine(connection.ServerVersion);
    Console.WriteLine(connection.DefaultTimeout);
    connection.Close();
}
//ConnectionToDb();

void AddTable()
{
    using SqliteConnection connection = new("DataSource=D:\\Temp\\users.db");
    connection.Open();

    SqliteCommand command = new SqliteCommand();
    command.Connection = connection;
    command.CommandText = "CREATE TABLE Users(_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, Name TEXT NOT NULL, Age INTEGER NOT NULL)";
    command.ExecuteNonQuery();

    connection.Close();
}
//AddTable();

void AddUser()
{
    using SqliteConnection connection = new("DataSource=D:\\Temp\\users.db");
    connection.Open();

    SqliteCommand command = new SqliteCommand();
    command.Connection = connection;
    command.CommandText = "INSERT INTO Users (Name, Age) VALUES ('Julia', 36)";
    int number = command.ExecuteNonQuery();

    connection.Close();

    Console.WriteLine($"Rows have been added to the Users table: {number}");

}
//AddUser();

void AddUsers()
{
    using SqliteConnection connection = new("DataSource=D:\\Temp\\users.db");

    string sql = "INSERT INTO Users (Name, Age) VALUES ('Alex', 34), ('Olga', 28)";

    connection.Open();

    SqliteCommand command = new SqliteCommand(sql, connection);
    int number = command.ExecuteNonQuery();

    connection.Close();

    Console.WriteLine($"Rows have been added to the Users table: {number}");
}
//AddUsers();

void UpdateUser()
{
    using SqliteConnection connection = new("DataSource=D:\\Temp\\users.db");

    string sql = "UPDATE Users SET Age=14 WHERE Name='Olga'";

    connection.Open();

    SqliteCommand command = new SqliteCommand(sql, connection);

    int number = command.ExecuteNonQuery();

    connection.Close();

    Console.WriteLine($"Rows have been updated to the Users table: {number}");
}
//UpdateUser();

void DeleteUser()
{
    using SqliteConnection connection = new("DataSource=D:\\Temp\\users.db");

    string sql = "DELETE  FROM Users WHERE Age < 16";

    connection.Open();

    SqliteCommand command = new SqliteCommand(sql, connection);

    int number = command.ExecuteNonQuery();

    connection.Close();

    Console.WriteLine($"Rows have been deleted to the Users table: {number}");
}
//DeleteUser();

void SomeDataChanges()
{
    using SqliteConnection connection = new("DataSource=D:\\Temp\\users.db");

    connection.Open();

    SqliteCommand command = new SqliteCommand();
    command.Connection = connection;

    command.CommandText = "INSERT INTO Users (Name, Age) VALUES ('Sara', 34), ('John', 28)";
    int number = command.ExecuteNonQuery();
    Console.WriteLine($"Rows have been added to the Users table: {number}");

    command.CommandText = "UPDATE Users SET Age=44 WHERE Name='Sara'";
    number = command.ExecuteNonQuery();
    Console.WriteLine($"Rows have been updated to the Users table: {number}");

    command.CommandText = "DELETE FROM Users WHERE Name='John'";
    number = command.ExecuteNonQuery();
    Console.WriteLine($"Rows have been deleted to the Users table: {number}");

    connection.Close();

}
//SomeDataChanges();

void ReadFromDb()
{
    using SqliteConnection connection = new("DataSource=D:\\Temp\\users.db");

    connection.Open();

    string sql = "SELECT * FROM Users";

    SqliteCommand command = new(sql, connection);

    using SqliteDataReader reader = command.ExecuteReader();

    Console.WriteLine($"{reader.GetName(0)}\t{reader.GetName(1)}\t{reader.GetName(2)}");

    reader.Read();

    Console.WriteLine($"{reader.GetValue(0)}\t{reader.GetValue(1)}\t{reader.GetValue(2)}");


    while (reader.Read())
    {
        Console.WriteLine($"{reader["_id"]}\t{reader["Name"]}\t{reader["Age"]}");
    }

    connection.Close();
}
//ReadFromDb();

void CommandWithParameters()
{
    using SqliteConnection connection = new("DataSource=D:\\Temp\\users.db");

    connection.Open();

    string sql = "INSERT INTO Users (Name, Age) VALUES (@name, @age)";

    SqliteCommand command = new(sql, connection);

    SqliteParameter parameterName = new("@name", SqliteType.Text, 50);
    parameterName.Value = "Tomy";
    command.Parameters.Add( parameterName );

    SqliteParameter parameterAge = new("@age", 37);
    command.Parameters.Add(parameterAge);

    int number = command.ExecuteNonQuery();
    Console.WriteLine($"Rows have been added to the Users table: {number}");

    connection.Close();

    ReadFromDb();
}
//CommandWithParameters();

void RetrieveScalarValue()
{
    using SqliteConnection connection = new("DataSource=D:\\Temp\\users.db");

    connection.Open();

    string sql = "SELECT AVG(Age) FROM Users";
    SqliteCommand command = new(sql, connection);
    object? avgAge = command.ExecuteScalar();
    Console.WriteLine(avgAge);

    command.CommandText = "SELECT COUNT(*) FROM Users";
    object? countUsers = command.ExecuteScalar();
    Console.WriteLine(countUsers);

    connection.Close();
}
RetrieveScalarValue();
