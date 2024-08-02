using AutoLot.DataAccessLayer.DataOperations;
using AutoLot.DataAccessLayer.BulkImport;
using System.Data.Common;

static void TestGetAllInvertory()
{
    InventoryDal inventoryDal = new InventoryDal();

    var inventory = inventoryDal.GetAllInventory();

    foreach (var item in inventory)
    {
        Console.WriteLine($"{item.Id}\t{item.Make}\t{item.Color}\t{item.PetName}");
    }
}
//TestGetAllInvertory();

static void Test_VerySimple_GetCar()
{
    InventoryDal inventoryDal = new InventoryDal();
    CarViewModel car = inventoryDal.VerySimple_GetCar(7);
    Console.WriteLine($"{car.Id}\t{car.Make}\t{car.Color}\t{car.PetName}");
}
//Test_VerySimple_GetCar();

static void Test_VerySimple_InsertCar()
{
    InventoryDal inventoryDal = new InventoryDal();
    inventoryDal.VerySimple_InsertCar("Green", 1, "Ella");
    
    TestGetAllInvertory();
}
//Test_VerySimple_InsertCar();

static void Test_Simple_InsertCar()
{
    InventoryDal inventoryDal = new InventoryDal();
    Car car = new() { Color = "Gray", MakeId = 1, PetName = "Elektric" };
    inventoryDal.Simple_InsertCar(car);

    TestGetAllInvertory();
}
//Test_Simple_InsertCar();

static void Test_Simple_DeleteCar()
{
    InventoryDal inventoryDal = new InventoryDal();
    inventoryDal.Simple_DeleteCar(11);
    TestGetAllInvertory();

    Console.WriteLine();

    inventoryDal.Simple_DeleteCar(5);

    Console.WriteLine();

    TestGetAllInvertory();
}
//Test_Simple_DeleteCar();

static void Test_Simple_Update()
{
    TestGetAllInvertory();
    Console.WriteLine();

    InventoryDal inventoryDal = new InventoryDal();
    inventoryDal.Simple_Update(12, "Electra");

    TestGetAllInvertory();
}
//Test_Simple_Update();

static void Test_GetCar()
{
    InventoryDal inventoryDal = new InventoryDal();
    CarViewModel car = inventoryDal.GetCar(7);
    Console.WriteLine($"{car.Id}\t{car.Make}\t{car.Color}\t{car.PetName}");
}
//Test_GetCar();

static void Test_DeleteCar()
{
    TestGetAllInvertory();
    Console.WriteLine();

    InventoryDal inventoryDal = new InventoryDal();
    inventoryDal.Simple_DeleteCar(12);

    TestGetAllInvertory();

}
//Test_DeleteCar();

static void Test_Update()
{
    TestGetAllInvertory();
    Console.WriteLine();

    InventoryDal inventoryDal = new InventoryDal();
    inventoryDal.Update(10, "Electra");

    TestGetAllInvertory();
}
//Test_Update();

static void Test_InsertCar()
{
    InventoryDal inventoryDal = new InventoryDal();
    Car car = new() { Color = "White", MakeId = 2, PetName = "Lapik" };
    inventoryDal.InsertCar(car);

    TestGetAllInvertory();
}
//Test_InsertCar();


static void Test_LookUpPetName()
{
    TestGetAllInvertory();
    Console.WriteLine();

    InventoryDal inventoryDal = new InventoryDal();

    Console.WriteLine(inventoryDal.LookUpPetName(5));
    Console.WriteLine(inventoryDal.LookUpPetName(155));

}
//Test_LookUpPetName();

static void Test_ProcessCreditRisk()
{
    InventoryDal inventoryDal = new InventoryDal();

    inventoryDal.GetAllCustomer(); Console.WriteLine();
    inventoryDal.GetAllCreditRisks(); Console.WriteLine();

    Console.WriteLine("Run process with transaction.");

    inventoryDal.ProcessCreditRisk(false, 1);
    inventoryDal.ProcessCreditRisk(true, 3);

    inventoryDal.GetAllCustomer(); Console.WriteLine();
    inventoryDal.GetAllCreditRisks(); Console.WriteLine();

}
//Test_ProcessCreditRisk();


// Data for Test_MyDataReader
List<Car> cars = new()
{
    new Car() {Color = "Blue", MakeId = 2, PetName = "Snuppy1"},
    new Car() {Color = "White", MakeId = 1, PetName = "Snuppy2"},
    new Car() {Color = "Red", MakeId = 4, PetName = "Snuppy3"},
    new Car() {Color = "Yellow", MakeId = 1, PetName = "Snuppy4"},
}; 

void Test_MyDataReader()
{
    string connectionString = "Data Source=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=AutoLot";

    var connection = new SqlConnection { ConnectionString = connectionString };

    connection.Open();

    MyDataReader<Car> myDataReader = new(cars, connection, "dbo", "Inventory");

    Console.WriteLine("FildCount:"+myDataReader.FieldCount);

    Console.WriteLine("Id\tMakeId\tColor\tPetName\tTimeStep");

    while (myDataReader.Read())
    {
        Console.WriteLine(
            myDataReader.GetValue(0) + "\t" +
            myDataReader.GetValue(1) + "\t" +
            myDataReader.GetValue(2) + "\t" +
            myDataReader.GetValue(3) + "\t" +
            myDataReader.GetValue(4));
    }
    connection.Close();
}
//Test_MyDataReader();

void UsingDataSetForReadData()
{
    string connectionString = "Data Source=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=AutoLot";

    string sql = "Select * From Customers";

    var connection = new SqlConnection { ConnectionString = connectionString };

    SqlDataAdapter adapter = new(sql, connection);
    DataSet dataSet = new();

    adapter.Fill(dataSet);

    PrintDataSetForCustomers(dataSet);

}
//UsingDataSetForReadData();

void PrintDataSetForCustomers(DataSet dataSet)
{
    foreach (DataTable dataTable in dataSet.Tables)
    {
        foreach (DataColumn dataColumn in dataTable.Columns)
        {
            Console.Write($"{dataColumn.ColumnName}\t");
        }
        Console.WriteLine();

        foreach (DataRow dataRow in dataTable.Rows)
        {
            Console.WriteLine($"{dataRow[0]}\t{dataRow[1]}\t{dataRow[2]}\t{BitConverter.ToUInt64((byte[])dataRow[3], 0)}");
        }
    }
}



void ChangeDataSetAndUpdateDB()
{
    string connectionString = "Data Source=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=AutoLot";

    string sql = "Select * From Customers";

    var connection = new SqlConnection { ConnectionString = connectionString };

    SqlDataAdapter adapter = new(sql, connection);
    DataSet dataSet = new();

    adapter.Fill(dataSet);

    DataTable dataTableCustomers = dataSet.Tables[0];

    Console.WriteLine($"Number of customers:{dataTableCustomers.Rows.Count}");

    //Add row
    DataRow newRowCustomer = dataTableCustomers.NewRow();
    newRowCustomer["FirstName"] = "Tomy";
    newRowCustomer["LastName"] = "Stark";
    newRowCustomer["Timestamp"] = BitConverter.GetBytes(DateTime.Now.ToBinary());
    dataTableCustomers.Rows.Add(newRowCustomer);

    //Change item in row 
    Console.WriteLine("Rows[4][1]"+dataTableCustomers.Rows[4][1]);
    dataTableCustomers.Rows[4][1] = "Jack";

    Console.WriteLine("\nDataSet in Memory");
    PrintDataSetForCustomers(dataSet);

    Console.WriteLine();
    //Update DB
    SqlCommandBuilder sqlCommandBuilder = new(adapter);
    Console.WriteLine(sqlCommandBuilder.GetUpdateCommand().CommandText);
    Console.WriteLine(sqlCommandBuilder.GetInsertCommand().CommandText);
    Console.WriteLine(sqlCommandBuilder.GetDeleteCommand().CommandText);

    adapter.Update(dataSet);
    //For one table
    //adapter.Update(dataTableCustomers);

}
//ChangeDataSetAndUpdateDB();
//Console.WriteLine();
//UsingDataSetForReadData();