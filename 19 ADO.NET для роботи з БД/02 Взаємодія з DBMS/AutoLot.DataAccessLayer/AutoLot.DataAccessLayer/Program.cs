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