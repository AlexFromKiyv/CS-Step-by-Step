using PublicDelegateProblem;

void PublicDelegateMemeber()
{
    Car car = new Car("VW Transporter",160,130);

    car.ListOfHandlers = KindHandler;
    car.Accelerate(8);
    car.Accelerate(8);
    car.Accelerate(8);
    car.Accelerate(8);

    Console.WriteLine("\nThe end of a work the kind hundler.\n");

    car = new Car("VW Transporter", 160, 130);
    car.ListOfHandlers = KindHandler;
    car.Accelerate(8);

    car.ListOfHandlers = EvilHandler;
    car.Accelerate(8);
    car.Accelerate(8);
    car.Accelerate(8);
    car.ListOfHandlers.Invoke("");
    car.ListOfHandlers.Invoke("");




    static void KindHandler(string message)
    {
        Console.WriteLine($"Kind handler says:{message}");
    }

    static void EvilHandler(string _)
    {
        Console.WriteLine("Evil handler says:I crecked you!");
    }

}

PublicDelegateMemeber();
