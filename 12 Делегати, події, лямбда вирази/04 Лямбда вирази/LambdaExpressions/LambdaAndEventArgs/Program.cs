
using LambdaAndEventArgs;
using System.Threading.Channels;

void UseEventWithoutLambda()
{
    Car car = new("Volkswagen Käfer", 105, 83);
    
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

    for (int i = 0; i < 5; i++)
    {
        car.Accelerate(5);
    }

}

//UseEventWithoutLambda();

void UseEventWithLambda()
{
    Car car = new("Volkswagen Käfer", 105, 83);

    car.AboutToBlow += (sender, e) => 
    Console.WriteLine($"Critical message from engine {sender} : {e.message}");
    
    car.Exploded += (sender, e) =>
    Console.WriteLine($"Fatal message from engine {sender} : {e.message}");

    for (int i = 0; i < 5; i++)
    {
        car.Accelerate(5);
    }

}

UseEventWithLambda();