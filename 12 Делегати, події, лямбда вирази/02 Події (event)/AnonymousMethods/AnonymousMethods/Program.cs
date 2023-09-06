
using AnonymousMethods;
using System.Runtime.CompilerServices;

void SimpleUseEvents()
{
    Something something = new("Bird", 0);
    something.ToConsole();

    something.SpeedUp += new EventHandler<SomethingEventArgs>(Something_ChangeSpeed);
    something.SpeedDown += Something_ChangeSpeed;

    ConsoleKey consoleKey = ConsoleKey.Home;
    while (consoleKey != ConsoleKey.End)
    {
        consoleKey = Console.ReadKey().Key;

        switch (consoleKey)
        {
            case ConsoleKey.UpArrow:
                something.ChangeSpeed(1);
                something.ToConsole();
                break;
            case ConsoleKey.DownArrow:
                something.ChangeSpeed(-1);
                something.ToConsole();
                break;
            default:
                something.ToConsole();
                break;
        }
    }

    void Something_ChangeSpeed(object? sender, SomethingEventArgs e)
    {
        if (sender is Something something)
        {
            Console.WriteLine($"Event on {something.Name} : {e.message}");
        }
    }
}

//SimpleUseEvents();

void UseAnonymousMethods()
{
    Car car = new("VW Golf", 160, 143);

    car.AboutToBlow += delegate
    {
        Console.WriteLine("Hey! Going too fast!");
    };

    car.AboutToBlow += delegate (object? sender, CarEventArgs e)
    {
        if (sender is Car c)
        {
            Console.WriteLine($"Critical message from engine {c.Name} : {e.message}");
        }

    };

    car.Exploded += delegate (object? sender, CarEventArgs e)
    {
        if (sender is Car c)
        {
            Console.WriteLine($"Fatal message from engine {c.Name} : {e.message}");
        }

    };

    for (int i = 0; i < 8; i++)
    {
        car.Accelerate(3);
    }
}

//UseAnonymousMethods();


void AccessLocalVariables()
{
    int aboutToBlowCounter = 0;

    Car car = new("VW e-up", 130, 110);

    car.AboutToBlow += delegate
    {
        aboutToBlowCounter++;
        Console.WriteLine("Hey! Going too fast!");
    };

    for (int i = 0; i < 8; i++)
    {
        car.Accelerate(3);
    }

    Console.WriteLine(aboutToBlowCounter);
}

AccessLocalVariables();