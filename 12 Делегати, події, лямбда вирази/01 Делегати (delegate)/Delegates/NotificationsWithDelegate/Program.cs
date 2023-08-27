using NotificationsWithDelegate;

void UseDelegateInfrastructure()
{
    Car carGrey = new("VW E-Golf Grey",150,130);

	for (int i = 0; i < 10; i++)
	{
		carGrey.Accelerate(3);
	}

    Console.WriteLine("\n\n");

    Car carRed = new("VW E-Golf Red", 150, 130);
	carRed.RegisterCarEngineHandler(new Car.CarEngineHandler(OnCarEngineEvent));

    for (int i = 0; i < 10; i++)
    {
		carRed.Accelerate(3);
	}

	void OnCarEngineEvent(string message)
	{
		Console.WriteLine($"  Message from car engine: {message}");
	}
}
//UseDelegateInfrastructure();

void UseMulticasting()
{
    Car car = new("VW E-Golf", 150, 133);
    car.RegisterCarEngineHandlers(new Car.CarEngineHandler(OnCarEngineEvent1));
    car.RegisterCarEngineHandlers(new Car.CarEngineHandler(OnCarEngineEvent2));

    for (int i = 0; i < 7; i++)
    {
        car.Accelerate(3);
    }

    Console.WriteLine("\n\n");

    Car car1 = new("Mercedes W123", 150, 133);


    Car.CarEngineHandler handler = new Car.CarEngineHandler(OnCarEngineEvent1);
    car1.RegisterCarEngineHandlers(handler);
    car1.RegisterCarEngineHandlers(new Car.CarEngineHandler(OnCarEngineEvent2));
    car1.UnRegisterCarEngineHandlers(handler);

    for (int i = 0; i < 7; i++)
    {
        car1.Accelerate(3);
    }


    void OnCarEngineEvent1(string message)
    {
        Console.WriteLine($"\t{message}");
    }
    void OnCarEngineEvent2(string message)
    {
        Console.WriteLine($"\t{message.ToUpper()}");
    }

}

//UseMulticasting();

void UseMethodGroupConversion()
{
    Car car = new("BMW i3", 180, 160);

    car.RegisterCarEngineHandler(OnCarEngineEvent);
    car.UnRegisterCarEngineHandlers(OnCarEngineEvent);
    car.RegisterCarEngineHandlers(OnCarEngineEvent);

    for (int i = 0; i < 7; i++)
    {
        car.Accelerate(3);
    }

    void OnCarEngineEvent(string message)
    {
        Console.WriteLine($"\t{message.ToUpper()}");
    }
}

//UseMethodGroupConversion();
