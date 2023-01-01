//UsingFor();
static void UsingFor()
{
	int length = 10;
	for (int i = 0; i < length; i++)
	{
		Console.WriteLine(i);
	}
}

//UsingForeach();

static void UsingForeach()
{
	string[] myTools = { "Pliers", "Hammer", "Screwdriver", "Wrench" };

	foreach(string tool in myTools)
	{
		Console.WriteLine(tool);
	}

	int[] myWeights = { 75, 78, 82, 80, 79, 75 };

	foreach (int weight in myWeights)
	{
		Console.WriteLine(weight);
	}
}

UsinForeachWithVar();
static void UsinForeachWithVar()
{
    int[] temperaturs = { 5, 12, 4, 15, 10, 8, 17 };

    var normal = from t in temperaturs where t > 10 select t;

    foreach (var item in normal)
    {
        Console.WriteLine(item);
    }
}