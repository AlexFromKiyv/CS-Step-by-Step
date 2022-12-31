//UsingFor();
static void UsingFor()
{
	int length = 10;
	for (int i = 0; i < length; i++)
	{
		Console.WriteLine(i);
	}
}

UsingForeach();

static void UsingForeach()
{
	string[] myTools = { "Апценьки", "Молоток", "Вивертка" };

	foreach(string tool in myTools)
	{
		Console.WriteLine(tool);
	}
}