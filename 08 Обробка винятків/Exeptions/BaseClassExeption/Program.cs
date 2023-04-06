using BaseClassExeption;

//ExplorationTheOccurationOfAnException();
void ExplorationTheOccurationOfAnException()
{
    Car_v1 car = new("Nissan Leaf", 35);

	for (int i = 0; i < 10; i++)
	{
		car.Accelerate(20);
	}

}

//ExplorationThrow();
void ExplorationThrow()
{
    Car_v2 car = new("Nissan Leaf", 35);

    for (int i = 0; i < 10; i++)
    {
        car.Accelerate(20);
    }
}

ExplorationTryCatch();
void ExplorationTryCatch()
{
    Car_v2 car = new("Nissan Leaf", 35);

    Console.WriteLine("---The begin of try---\n");
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(20);
        }

    }
    catch (Exception e)
    {
        Console.WriteLine(  );

        string stringForShow = "\n" +
            $"Attention! Problem occured!\n\n" +
            $" Method: {e.TargetSite}\n" +
            $" Message: {e.Message}\n" +
            $" Source: {e.Source}\n";

        Console.WriteLine(stringForShow);
    }
    Console.WriteLine("---The end of try---");
}
