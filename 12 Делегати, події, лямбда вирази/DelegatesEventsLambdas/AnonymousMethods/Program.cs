
using AnonymousMethods;

static void UsingAnonimousMethods()
{
    Car car = new("SlugBug",100,10);

    car.AboutToBlow += delegate
    {
        Console.WriteLine("Eek! Going too fast!");
    };

    car.AboutToBlow += delegate(object sender, CarEventArgs e)
    {
        Console.WriteLine($"Message from Car{e.message}");
    }!;

    car.Exploded += delegate(object sender, CarEventArgs e)
    {
        Console.WriteLine(e.message.ToUpper());
    }!;

    Console.WriteLine("***** Speeding up *****");
    for (int i = 0; i < 6; i++)
    {
        car.Accelerate(20);
    }
}
//UsingAnonimousMethods();

static void AccessingLocalVariables()
{
    int aboutToBlowCounter = 0;

    Car car = new("SlugBug", 100, 10);

    car.AboutToBlow += delegate
    {
        //aboutToBlowCounter++;
        Console.WriteLine("Eek! Going too fast!");
    };

    car.AboutToBlow += delegate (object sender, CarEventArgs e)
    {
        aboutToBlowCounter++;
        Console.WriteLine($"Message from Car{e.message}");
    }!;

    car.Exploded += delegate (object sender, CarEventArgs e)
    {
        Console.WriteLine(e.message.ToUpper());
    }!;

    Console.WriteLine("***** Speeding up *****");
    for (int i = 0; i < 6; i++)
    {
        car.Accelerate(20);
    }

    Console.WriteLine($"AboutToBlow event was fired {aboutToBlowCounter} times.");
}
//AccessingLocalVariables();

static int AddWrapperWithStatic(int x, int y)
{
    //Do some validation here
    return Add(x, y);

    static int Add(int x, int y)
    {
        return x + y;
    }
}
//Console.WriteLine(AddWrapperWithStatic(1,1));

static void UsingStatic()
{
    int aboutToBlowCounter = 0;

    Car car = new("SlugBug", 100, 10);

    car.AboutToBlow += static delegate
    {
        //aboutToBlowCounter++;
        Console.WriteLine("Eek! Going too fast!");
    };

    Console.WriteLine("***** Speeding up *****");
    for (int i = 0; i < 6; i++)
    {
        car.Accelerate(20);
    }
}
//UsingStatic();

Func<int, int> constant = delegate (int _) { return 42; };
Console.WriteLine(constant(4));