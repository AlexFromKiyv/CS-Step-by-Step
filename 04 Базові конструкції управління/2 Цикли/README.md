Додамо в рішення проект Loops

# For

```cs
UsingFor();
static void UsingFor()
{
	int length = 10;
	for (int i = 0; i < length; i++)
	{
		Console.WriteLine(i);
	}

	//Console.WriteLine(i); //does not exit in context
}
```
Якщо відомо яку фіксовану кількість ітерацій треба зробити for швидкий і простий варіант.

# Foreach

```cs
UsingForeach();

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
```
Цей варіанти підходить коли вам треба в колекції пройтись по всім єлементам один за одоним без всаких трюків. Коллекцією може бути клас який реалізовує інтерфейс IEnumerable (перелічуваний).

```cs
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
```
В циклі можна використовувати неявну типізацію єлементів.


# While

```cs
UsingWhile();
static void UsingWhile()
{
	string enteredString = "";

	while (enteredString.ToLower() != "y")
	{
		Console.Write("Do you want to exit ? (Y/N):");
		enteredString = Console.ReadLine();
		Console.Clear();

    }
}
```

Цикл while корсиний коли треба виконати блок коду поки не буде виконана умова. Для того щоб цикл колись закінчився треба переконатися що умова рано чи пізно можлива.

```cs
UsingDoWhile();
static void UsingDoWhile()
{
    string enteredString = "";

	do
	{
        Console.Clear();
        Console.Write("Do you want to exit ? (Y/N):");
        enteredString = Console.ReadLine();
    } while (enteredString.ToLower() != "y");
	
}
```
Особливости циклу do/while то що тіло циклу виконуяться перш ніж перевіряеться умова. Тобто тіло хочаб один раз буде виконано.

