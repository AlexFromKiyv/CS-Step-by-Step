
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

//AccessLocalVariables();

void IsolationLocalFunction()
{

    Console.WriteLine(AddWrapperWithStatic(1,2));

    int AddWrapperWithStatic(int x, int y)
    {
        //Do some validation here
        return Add(x, y);
   
        static int Add(int x, int y)
        {
            return x + y;
        }
    }
}

//IsolationLocalFunction();


void StaticAnonymousMethods()
{
    int aboutToBlowCounter = 0;

    Car car = new("VW e-up", 130, 110);

    // Now it is static
    car.AboutToBlow += static delegate
    {
        //aboutToBlowCounter++; //A static anonymous function cannot contain а reference  
        Console.WriteLine("Hey! Going too fast!");
    };

    for (int i = 0; i < 8; i++)
    {
        car.Accelerate(3);
    }
}

void DiscardsMethodParameters()
{
    Console.WriteLine(ReturnResult(10));

    string ReturnResult(int _)
    {
        return "Hi";
    }
}
//DiscardsMethodParameters();

void DiscardInAnonymousMethod()
{
    Func<int, string> sayHi = delegate (int _) { return "Hi"; };
    
    Console.WriteLine(sayHi(3));
}

DiscardInAnonymousMethod();