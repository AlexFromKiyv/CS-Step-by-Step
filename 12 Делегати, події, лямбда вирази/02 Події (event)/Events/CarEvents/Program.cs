
using CarEvents;

void useEvents()
{
    Car car = new Car("BMW i3",150,125);
    car.ToConsole();

    ConsoleKey consoleKey = ConsoleKey.Home;
    while (consoleKey != ConsoleKey.End)
    {
        consoleKey = Console.ReadKey().Key;
 
        switch (consoleKey)
        {
            case ConsoleKey.UpArrow:
                car.Accelerate(3);
                break;
            case ConsoleKey.DownArrow:
                car.Accelerate(-3);
                break;
            case ConsoleKey.Insert:
                car.AboutToBlow += PrintCriticalMessage;
                Console.WriteLine("Handler for event AboutToBlow added.");
                car.Exploded += Console.WriteLine;
                Console.WriteLine("Handler for event Exploded added.");
                break;
            default:
                car.ToConsole();
                break;
        }
    }

    void PrintCriticalMessage(string? message)
    {
        Console.WriteLine($"Critical message from engine: {message}");
    }
}

//useEvents();

void SimplifyingCodingHandler()
{
    Car car = new("Nissan Leaf", 150, 100);

    car.Exploded += Car_Exploded;

    car.Accelerate(5);
    car.Accelerate(15);
    car.Accelerate(20);
    car.Accelerate(25);
}

void Car_Exploded(string? messageForCaller)
{
    throw new NotImplementedException();
}

//SimplifyingCodingHandler();

