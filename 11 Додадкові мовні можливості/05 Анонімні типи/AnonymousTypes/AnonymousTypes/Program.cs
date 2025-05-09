
static void BuildAnonymousType(string make, string color, int speed)
{
    // Build anonymous type using incoming args.
    var car = new { Make = make, Color = color, Speed = speed };

    // Note you can now use this type to get the property data!
    Console.WriteLine($"You have a {car.Color} {car.Make} going {car.Speed} MPH");
    Console.WriteLine();
    // Anonymous types have custom implementations of each virtual
    // method of System.Object. For example:
    Console.WriteLine($"ToString() == {car.ToString()}");
}

static void DefiningAnonymousType()
{

    BuildAnonymousType("Ford", "Red", 20);

    // Make an anonymous type representing a car.
    var myCar = new { Color = "Bright Pink", Make = "Saab", CurrentSpeed = 55 };
    // Now show the color and make.
    Console.WriteLine($"My car is a {myCar.Color} {myCar.Make}.");

}
//DefiningAnonymousType();

static void ReflectOverAnonymousType(object obj)
{
    Console.WriteLine($"obj is an instance of: {obj.GetType().Name}");
    Console.WriteLine($"Base class of {obj.GetType().Name} is {obj.GetType().BaseType}");
    Console.WriteLine($"obj.ToString() == {obj.ToString()}");
    Console.WriteLine($"obj.GetHashCode() == {obj.GetHashCode()}");
    Console.WriteLine();
}

static void ReflectCar()
{
    // Make an anonymous type representing a car.
    var myCar = new
    {
        Color = "Bright Pink",
        Make = "Saab",
        CurrentSpeed = 55
    };

    ReflectOverAnonymousType(myCar);
}
//ReflectCar();

static void EqualityTest()
{
    // Make 2 anonymous classes with identical name/value pairs.
    var firstCar = new { Color = "Bright Pink", Make = "Saab", CurrentSpeed = 55 };
    var secondCar = new { Color = "Bright Pink", Make = "Saab", CurrentSpeed = 55 };

    // Are they considered equal when using Equals()?
    if (firstCar.Equals(secondCar))
    {
        Console.WriteLine("Same anonymous object!");
    }
    else
    {
        Console.WriteLine("Not the same anonymous object!");
    }

    // Are they considered equal when using ==?
    if (firstCar == secondCar)
    {
        Console.WriteLine("Same anonymous object!");
    }
    else
    {
        Console.WriteLine("Not the same anonymous object!");
    }

    // Are these objects the same underlying type?
    if (firstCar.GetType().Name == secondCar.GetType().Name)
    {
        Console.WriteLine("We are both the same type!");
    }
    else
    {
        Console.WriteLine("We are different types!");
    }

    // Show all the details.
    Console.WriteLine();
    ReflectOverAnonymousType(firstCar);
    ReflectOverAnonymousType(secondCar);
}
//EqualityTest();

static void AnonymousTypesContainingAnonymousTypes()
{
    // Make an anonymous type that is composed of another.
    var purchaseItem = new
    {
        TimeBought = DateTime.Now,
        ItemBought = new { Color = "Red", Make = "Saab", CurrentSpeed = 55 },
        Price = 34.000
    };

    ReflectOverAnonymousType(purchaseItem);
    ReflectOverAnonymousType(purchaseItem.ItemBought);
}
AnonymousTypesContainingAnonymousTypes();