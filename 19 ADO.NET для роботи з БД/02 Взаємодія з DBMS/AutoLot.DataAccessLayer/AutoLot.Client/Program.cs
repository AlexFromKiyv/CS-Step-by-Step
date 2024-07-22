using AutoLot.DataAccessLayer.BulkImport;
using AutoLot.DataAccessLayer.DataOperations;
using AutoLot.DataAccessLayer.Models;



static void Run()
{
    InventoryDal inventoryDal = new();
    Console.WriteLine("\t\tAll list");
    List<CarViewModel> cars = inventoryDal.GetAllInventory();
    ViewListOfCar(cars);
    Console.WriteLine("\n\n");

    
    int firstId = cars.OrderBy(c => c.Make).Select(r => r.Id).First();
    CarViewModel car = inventoryDal.GetCar(firstId);
    Console.WriteLine("\t\tFinding first car by Make");
    Console.WriteLine("Id\tMake\tColor\tPet Name");
    Console.WriteLine($"{car.Id}\t{car.Make}\t{car.Color}\t{car.PetName}");
    Console.WriteLine("\n\n");

    Console.WriteLine("\t\tInsert");
    Car newCar = new() { Color = "Red", MakeId = 5, PetName = "Cher" };
    inventoryDal.InsertCar(newCar);
    ViewListOfCar(inventoryDal.GetAllInventory());
    Console.WriteLine("\n\n");

    Console.WriteLine("\t\tDelete");
    int lastId = inventoryDal.GetAllInventory().Max(c => c.Id);
    Console.WriteLine($"Last ID {lastId}");
    inventoryDal.DeleteCar(lastId);
    ViewListOfCar(inventoryDal.GetAllInventory());
    Console.WriteLine("\n\n");


    Console.WriteLine("\t\tUpdate");
    inventoryDal.Update(13, "Shmapik");
    ViewListOfCar(inventoryDal.GetAllInventory());
    Console.WriteLine("\n\n");


    Console.WriteLine("\t\tDelete with SqlException");
    inventoryDal.DeleteCar(5);
    ViewListOfCar(inventoryDal.GetAllInventory());
}
//Run();

static void ViewListOfCar(List<CarViewModel> cars)
{
    Console.WriteLine("Id\tMake\tColor\tPet Name");
    foreach (var item in cars)
    {
        Console.WriteLine($"{item.Id}\t{item.Make}\t{item.Color}\t{item.PetName}");
    }
}

void DoBulkCopy()
{
    InventoryDal inventoryDal = new();
    Console.WriteLine("\t\tBefore bulk copy");
    ViewListOfCar(inventoryDal.GetAllInventory());
    Console.WriteLine("\n\n");

    var cars = new List<Car>
    {
        new Car() {Color = "Blue", MakeId = 4, PetName = "MyCar1"},
        new Car() {Color = "Red", MakeId = 3, PetName = "MyCar2"},
        new Car() {Color = "White", MakeId = 1, PetName = "MyCar3"},
        new Car() {Color = "Yellow", MakeId = 2, PetName = "MyCar4"}
    };

    ProcessBulkImport.ExecuteBulkImport(cars, "Inventory");

    Console.WriteLine("\t\tAfter bulk copy");
    ViewListOfCar(inventoryDal.GetAllInventory());
}
DoBulkCopy();
