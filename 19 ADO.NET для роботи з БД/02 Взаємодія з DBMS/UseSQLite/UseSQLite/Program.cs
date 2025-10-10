using Microsoft.Data.Sqlite;

string path = "DataSource=D:\\Temp\\Team.db";

// This method create DB, add Table "People" with data and show it.
void InitializationDb()
{
    ConnectionToDb();
    if (!TableExists("People"))
    {
        AddTable();
    }
    else
    {
        ClearTableRecords("People");
    }
    AddPerson();
    AddPeople();
    ReadFromDb();
}
InitializationDb();

void ConnectionToDb()
{
    using SqliteConnection connection = new(path);
    connection.Open();
    Console.Write(connection.Database); Console.Write("\t");
    Console.Write(connection.State); Console.Write("\t");
    Console.Write(connection.DataSource); Console.Write("\t");
    Console.Write(connection.ServerVersion); Console.Write("\t");
    Console.WriteLine(connection.DefaultTimeout);
    connection.Close();
}
//ConnectionToDb();

void AddTable()
{
    if (TableExists("People"))
    {
        return;
    }

    using SqliteConnection connection = new(path);
    connection.Open();

    SqliteCommand command = new SqliteCommand();
    command.Connection = connection;
    command.CommandText = "CREATE TABLE People(Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, Name TEXT NOT NULL, Age INTEGER NOT NULL)";
    command.ExecuteNonQuery();

    connection.Close();
}
//AddTable();

bool TableExists(string tableName)
{
    using SqliteConnection connection = new(path);
    connection.Open();
    
    var command = connection.CreateCommand();
        command.CommandText =@"SELECT name FROM sqlite_master WHERE type='table' AND name=$tableName";
        command.Parameters.AddWithValue("$tableName", tableName);

    using var reader = command.ExecuteReader();
    
    return reader.HasRows; // If a row is returned, the table exists
}

void AddPerson()
{
    using SqliteConnection connection = new(path);
    connection.Open();

    SqliteCommand command = new SqliteCommand();
    command.Connection = connection;
    command.CommandText = "INSERT INTO People (Name, Age) VALUES ('Julia', 36)";
    int number = command.ExecuteNonQuery();

    connection.Close();

    Console.WriteLine($"Rows have been added to the People table: {number}");
}
//AddPerson();

void AddPeople()
{
    using SqliteConnection connection = new(path);

    string sql = "INSERT INTO People (Name, Age) VALUES ('Alex', 34), ('Olga', 28)";

    connection.Open();

    SqliteCommand command = new SqliteCommand(sql, connection);
    int number = command.ExecuteNonQuery();

    connection.Close();

    Console.WriteLine($"Rows have been added to the People table: {number}");
}
//AddPeople();

void ReadFromDb()
{
    using SqliteConnection connection = new(path);

    connection.Open();

    string sql = "SELECT * FROM People";

    SqliteCommand command = new(sql, connection);

    using SqliteDataReader reader = command.ExecuteReader();

    Console.WriteLine($"{reader.GetName(0)}\t{reader.GetName(1)}\t{reader.GetName(2)}");

    reader.Read();

    Console.WriteLine($"{reader.GetValue(0)}\t{reader.GetValue(1)}\t{reader.GetValue(2)}");

    while (reader.Read())
    {
        Console.WriteLine($"{reader["Id"]}\t{reader["Name"]}\t{reader["Age"]}");
    }

    connection.Close();
}
//ReadFromDb();

void ClearTableRecords(string tableName)
{
    using SqliteConnection connection = new(path);

    connection.Open();

    var commandText = $"DELETE FROM {tableName};";

    using var command = new SqliteCommand(commandText, connection);
    command.ExecuteNonQuery();

    command.CommandText = $"UPDATE sqlite_sequence SET seq = 0 WHERE name = 'People';";
    command.ExecuteNonQuery();
}

void UpdatePerson()
{
    using SqliteConnection connection = new(path);

    connection.Open();

    string sql = "UPDATE People SET Age=14 WHERE Name='Olga'";
    SqliteCommand command = new SqliteCommand(sql, connection);
    int number = command.ExecuteNonQuery();

    connection.Close();

    Console.WriteLine($"Rows have been updated to the People table: {number}");
    ReadFromDb();
}
//UpdatePerson();

void DeletePerson()
{
    using SqliteConnection connection = new(path);

    connection.Open();

    string sql = "DELETE  FROM People WHERE Age < 16";
    SqliteCommand command = new SqliteCommand(sql, connection);
    int number = command.ExecuteNonQuery();

    connection.Close();

    Console.WriteLine($"Rows have been deleted to the Person table: {number}");
    ReadFromDb();
}
//DeletePerson();

void SomeDataChanges()
{
    using SqliteConnection connection = new(path);

    connection.Open();

    SqliteCommand command = new SqliteCommand();
    command.Connection = connection;

    command.CommandText = "INSERT INTO People (Name, Age) VALUES ('Sara', 34), ('John', 28)";
    int number = command.ExecuteNonQuery();
    Console.WriteLine($"Rows have been added to the People table: {number}");

    command.CommandText = "UPDATE People SET Age=44 WHERE Name='Sara'";
    number = command.ExecuteNonQuery();
    Console.WriteLine($"Rows have been updated to the People table: {number}");

    command.CommandText = "DELETE FROM People WHERE Name='John'";
    number = command.ExecuteNonQuery();
    Console.WriteLine($"Rows have been deleted to the People table: {number}");

    ReadFromDb();

    connection.Close();
}
//SomeDataChanges();



void CommandWithParameters()
{
    using SqliteConnection connection = new(path);

    connection.Open();

    string sql = "INSERT INTO People (Name, Age) VALUES (@name, @age)";

    SqliteCommand command = new(sql, connection);

    SqliteParameter parameterName = new("@name", SqliteType.Text, 50);
    parameterName.Value = "Tomy";
    command.Parameters.Add( parameterName );

    SqliteParameter parameterAge = new("@age", 37);
    command.Parameters.Add(parameterAge);

    int number = command.ExecuteNonQuery();
    Console.WriteLine($"Rows have been added to the People table: {number}");

    connection.Close();

    ReadFromDb();
}
//CommandWithParameters();

void RetrieveScalarValue()
{
    using SqliteConnection connection = new(path);

    connection.Open();

    ReadFromDb(); Console.WriteLine();

    string sql = "SELECT AVG(Age) FROM People";
    SqliteCommand command = new(sql, connection);
    object? avgAge = command.ExecuteScalar();
    Console.WriteLine(avgAge);

    command.CommandText = "SELECT COUNT(*) FROM People";
    object? countUsers = command.ExecuteScalar();
    Console.WriteLine(countUsers);

    connection.Close();
}
//RetrieveScalarValue();
