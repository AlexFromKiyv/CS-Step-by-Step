using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.DataAccessLayer.DataOperations;

public class InventoryDal : IDisposable
{
    // Variables
    private readonly string _connectionString;
    private SqlConnection? _sqlConnection = null;


    // Constructors
    public InventoryDal(string connectionString)
    {
        _connectionString = connectionString;
    }
    public InventoryDal() : this("Data Source=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=AutoLot")
    {
    }

    // Connection
    private void OpenConnection()
    {
        _sqlConnection = new SqlConnection { ConnectionString = _connectionString };
        _sqlConnection.Open();
    }

    private void CloseConnection() 
    { 
        if (_sqlConnection?.State != ConnectionState.Closed)
        {
            _sqlConnection?.Close();
        } 
    }

    // Implementation the disposable pattern
    bool _disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) 
        {
            return;
        }
        if (disposing) 
        { 
            _sqlConnection?.Dispose();
        }
        _disposed = true;  
    }

    public void Dispose() 
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
