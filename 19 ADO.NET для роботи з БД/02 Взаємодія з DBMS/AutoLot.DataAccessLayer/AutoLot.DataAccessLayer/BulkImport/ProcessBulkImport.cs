using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.DataAccessLayer.BulkImport;

public static class ProcessBulkImport
{
    private const string ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=AutoLot";
    private static SqlConnection? _sqlConnection = null;

    private static void OpenConnection()
    {
        _sqlConnection = new()
        {
            ConnectionString = ConnectionString,
        };
        _sqlConnection.Open();
    }

    private static void CloseConnection() 
    {
        if(_sqlConnection?.State != ConnectionState.Closed)
        {
            _sqlConnection?.Close();
        }
    }

    public static void ExecuteBulkImport<T>(IEnumerable<T> records, string tableName)
    {
        OpenConnection();
        
        using SqlConnection connection = _sqlConnection!;
        SqlBulkCopy bulkCopy = new(connection)
        {
            DestinationTableName = tableName,
        };

        var dataReader = new MyDataReader<T>(records.ToList(), _sqlConnection, "dbo", tableName);

        try
        {
            bulkCopy.WriteToServer(dataReader);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            CloseConnection();
        } 
    }
}

