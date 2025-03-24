
using CarEvents;
using System;
using System.Diagnostics.Tracing;

//Methods
static void CarAboutToBlow(string message)
{
    Console.WriteLine(message);
}
static void CarIsAlmostDoomed(string message)
{
    Console.WriteLine($"=> Critical Message from Car: {message}");
}
static void CarExploded(string message)
{
    Console.WriteLine(message.ToUpper());
}

static void UsingEvents()
{
    Car car1 = new("SlugBug",100,10);

    car1.AboutToBlow += CarIsAlmostDoomed;
    //car1.AboutToBlow += CarAboutToBlow;

    Car.CarEngineHandler d = CarExploded;
    car1.Exploded += d;

    Console.WriteLine("***** Speeding up *****");
    for (int i = 0; i < 6; i++)
    {
        car1.Accelerate(20);
    }

    // Remove CarExploded method
    // from invocation list.
    car1.Exploded -= d;

    Console.WriteLine("***** Speeding up *****");
    for (int i = 0; i < 6; i++)
    {
        car1.Accelerate(20);
    }
}
//UsingEvents();

static void HookIntoEvents()
{
    Car car = new("Max", 200, 40);
    car.AboutToBlow += Car_AboutToBlow;
}

static void Car_AboutToBlow(string msgForCaller)
{
    throw new NotImplementedException();
}

static void UsingCarEventArgs()
{
    Car1 car = new("SlugBug", 100, 10);

    car.AboutToBlow += Car1_AboutToBlow;
    car.Exploded += Car1_Exploded;

    Console.WriteLine("***** Speeding up *****");
    for (int i = 0; i < 6; i++)
    {
        car.Accelerate(20);
    }
}
//UsingCarEventArgs();

static void Car1_AboutToBlow(object sender, CarEventArgs eventArgs)
{
    // Just to be safe, perform a
    // runtime check before casting.
    if (sender is Car1 car)
    {
        Console.WriteLine($"Critical Message from {car.PetName} :{eventArgs.message}");
    }
}

static void Car1_Exploded(object sender, CarEventArgs eventArgs)
{
    if (sender is Car1 car)
    {
        Console.WriteLine(eventArgs.message.ToUpper());
    }
}

static void UsingEventHandler()
{
    Car2 car = new("SlugBug", 100, 10);

    car.AboutToBlow += Car2_AboutToBlow;
    car.Exploded += Car2_Exploded;

    Console.WriteLine("***** Speeding up *****");
    for (int i = 0; i < 6; i++)
    {
        car.Accelerate(20);
    }
}
UsingEventHandler();

static void Car2_AboutToBlow(object? sender, CarEventArgs e)
{
    if (sender is Car2 car)
    {
        Console.WriteLine($"Critical Message from {car.PetName} :{e.message}");
    }
}
static void Car2_Exploded(object? sender, CarEventArgs e)
{
    if (sender is Car2 car)
    {
        Console.WriteLine(e.message.ToUpper());
    }
}
