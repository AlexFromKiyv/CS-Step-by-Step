using ForEachWithExtensionMethods;

static void UsingGatage()
{
    Garage carLot = new Garage();

    // Hand over each car in the collection?
    foreach (Car c in carLot)
    {
        Console.WriteLine($"{c.PetName} is going {c.CurrentSpeed} MPH");
    }
}
UsingGatage();
