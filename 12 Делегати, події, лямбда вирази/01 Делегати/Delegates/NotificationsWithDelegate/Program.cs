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
	carRed.RegisterCarEngineHandler(OnCarEngineEvent);

    for (int i = 0; i < 10; i++)
    {
		carRed.Accelerate(3);
	}

	void OnCarEngineEvent(string message)
	{
		Console.WriteLine($"  Message from car engine: {message}");
	}
}
UseDelegateInfrastructure();