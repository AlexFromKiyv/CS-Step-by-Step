using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
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


    // Methods of data selection
    public List<CarViewModel> GetAllInvertory()
    {
        List<CarViewModel> invertory = new();

        OpenConnection();

        string sql = 
            @"SELECT i.Id, i.Color, i.PetName,m.Name as Make 
              FROM Inventory i 
              INNER JOIN Makes m on m.Id = i.MakeId";
        using SqlCommand command = new(sql, _sqlConnection);

        SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
        while (dataReader.Read())
        {
            invertory.Add(new CarViewModel
            {
                Id = dataReader.GetInt32("Id"),
                Color = dataReader.GetString("Color"),
                Make = dataReader.GetString("Make"),
                PetName = dataReader.GetString("PetName")
            });
        }
        dataReader.Close();

        return invertory;
    }

    public CarViewModel VerySimple_GetCar(int id)
    {
        OpenConnection();
       
        CarViewModel car = new();

        //This should use parameters for security reasons
        string sql =
            $@"SELECT i.Id, i.Color, i.PetName,m.Name as Make 
               FROM Inventory i 
               INNER JOIN Makes m on m.Id = i.MakeId
               WHERE i.Id = {id}";

        using SqlCommand command = new(sql, _sqlConnection);

        SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);

        dataReader.Read();
        car = new CarViewModel
        {
            Id = dataReader.GetInt32("Id"),
            Color = dataReader.GetString("Color"),
            Make = dataReader.GetString("Make"),
            PetName = dataReader.GetString("PetName")
        };
        dataReader.Close();
        return car;
    }


    // Method for insert 
    public void VerySimple_InsertCar(string color, int makeId, string petName)
    {
        OpenConnection();

        string sql = $"Insert Into Inventory (MakeId, Color, PetName) Values ('{makeId}', '{color}', '{petName}')";
        
        using SqlCommand command = new(sql, _sqlConnection);

        command.ExecuteNonQuery();
        
        CloseConnection();
    }

    public void Simple_InsertCar(Car car)
    {
        OpenConnection();

        string sql = $"Insert Into Inventory (MakeId, Color, PetName) Values ('{car.MakeId}', '{car.Color}', '{car.PetName}')";

        using SqlCommand command = new(sql, _sqlConnection);

        command.ExecuteNonQuery();

        CloseConnection();
    }


    // Methods for deletion

    public void Simple_DeleteCar(int id)
    {
        OpenConnection();

        string sql = $"Delete from Inventory where Id = '{id}' ";
        using SqlCommand command = new(sql, _sqlConnection);

        try
        {
            command.ExecuteNonQuery();
        }
        catch (SqlException sqlEx)
        {
            Console.WriteLine("Exception in DB:" + sqlEx.Message);
        }
        catch (Exception ex)  {
            Console.WriteLine(ex.Message);
        }
        
        CloseConnection();
    }

}
