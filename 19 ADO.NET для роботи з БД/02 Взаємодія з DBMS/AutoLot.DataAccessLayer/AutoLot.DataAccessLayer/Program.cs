using AutoLot.DataAccessLayer.DataOperations;

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
Test_Simple_DeleteCar();
