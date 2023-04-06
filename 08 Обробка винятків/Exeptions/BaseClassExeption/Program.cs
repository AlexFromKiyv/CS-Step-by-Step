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

ExplorationThrow();
void ExplorationThrow()
{
    Car_v2 car = new("Nissan Leaf", 35);

    for (int i = 0; i < 10; i++)
    {
        car.Accelerate(20);
    }
}