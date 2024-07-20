using AutoLot.DataAccessLayer.DataOperations;
using AutoLot.DataAccessLayer.BulkImport;

static void TestGetAllInvertory()
{
    InventoryDal inventoryDal = new InventoryDal();

    var inventory = inventoryDal.GetAllInvertory();

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
Test_MyDataReader();