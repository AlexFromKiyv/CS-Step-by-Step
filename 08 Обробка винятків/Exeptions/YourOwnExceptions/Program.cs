
using YourOwnExceptions;

//ExplorationSystemExceptions();
void ExplorationSystemExceptions()
{
    NullReferenceException exception = new();

    Console.WriteLine( exception is SystemException);
}

//ExplorationCarIsDead_v1_Exception();
void ExplorationCarIsDead_v1_Exception()
{
    Car_v1 car = new("Nissan Leaf", 35);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(20);
        }

    }
    catch (CarIsDead_v1_Exception e)
    {
        Console.WriteLine($"\n{e.Message}");
        Console.WriteLine($"Cause:\t{e.CauseOfError}");
        Console.WriteLine($"Speed:\t{e.Speed}");
    }
}

//ExplorationCarIsDead_v2_Exception();
void ExplorationCarIsDead_v2_Exception()
{
    Car_v2 car = new("Nissan Leaf", 35);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(20);
        }

    }
    catch (CarIsDead_v2_Exception e)
    {
        Console.WriteLine($"\nMessage:\t{e.Message}");
        Console.WriteLine($"Cause:\t{e.CauseOfError}");
        Console.WriteLine($"Speed:\t{e.Speed}");
    }
}

ExplorationCarIsDead_v3_Exception();
void ExplorationCarIsDead_v3_Exception()
{
    Car_v3 car = new("Nissan Leaf", 35);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(20);
        }

    }
    catch (CarIsDead_v3_Exception e)
    {
        Console.WriteLine($"\nMessage:\t{e.Message}");
        Console.WriteLine($"Cause:\t{e.CauseOfError}");
        Console.WriteLine($"Speed:\t{e.Speed}");
    }
}