using CustomEnumeratorWithYield;

void ShowGarage()
{
    Garage garage = new Garage();

    foreach (Car car in garage)
    {
        Console.WriteLine($"{car.PetName} is going {car.CurrentSpeed} MPH");
    }
}
//ShowGarage();

void GuardClausesWithLocalFunctions()
{
    Garage garage = new();
    System.Collections.IEnumerator enumerator = garage.GetEnumerator();
}
//GuardClausesWithLocalFunctions();

void GuardClausesWithLocalFunctions1()
{
    Garage garage = new Garage();
    try
    {
        //Error at this time
        var enumerator = garage.GetEnumerator();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception occurred on GetEnumerator");
        Console.WriteLine(ex.Message);
    }
}
//GuardClausesWithLocalFunctions1();

void NamedIterator()
{
    Garage garage = new Garage();
    // Get items using GetEnumerator()
    foreach (Car car in garage)
    {
        Console.WriteLine($"{car.PetName} is going {car.CurrentSpeed} MPH");
    }
    Console.WriteLine();

    // Get items (in reverse!) using named iterator.
    foreach (Car car in garage.GetTheCars(true))
    {
        Console.WriteLine($"{car.PetName} is going {car.CurrentSpeed} MPH");
    }
    Console.WriteLine();

    foreach (Car car in garage.GetTheCars(false))
    {
        Console.WriteLine($"{car.PetName} is going {car.CurrentSpeed} MPH");
    }
}
//NamedIterator();