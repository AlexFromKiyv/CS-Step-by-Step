# Array

## Створення

Масив це набір єлементів одного типу з доступом через числові індекси.

```cs

UsingSimpleArray();

static void UsingSimpleArray()
{
    int[] myIntArray = new int[3]; // declaring

    myIntArray[0] = 1;
    myIntArray[1] = 2;
    myIntArray[2] = 3;

    for (int i = 0; i < 3; i++)
    {
        Console.WriteLine(myIntArray[i]);
    }

    //myIntArray[3] = 4; //do not work. Out of index
    int[] myNewArray = new int[10];

    foreach (int item in myNewArray)
    {
        Console.WriteLine(item);
    }
}
```
Треба зауважити new int[3] означає що загальна кількість єлементів 3. Індексація починаеться з 0. Коли ви задекларували масив память виділяеться і він заповнюється значеннями default. В цьому прикладі 0-ми. 

## Ініціалізація

Масив можна задекларувати і одразу задати значення єлементам. 

```cs

ArrayInitialization();

static void ArrayInitialization()
{
    string[] myStringArray = new string[] { "good", "better", "best" };
    bool[] myBoolArray = { true, false, false, true };
    int[] myIntArray = { 10, 15, 20 };

    Console.WriteLine(myStringArray);
    Console.WriteLine(myStringArray.Length);
    Console.WriteLine(string.Join(" < ",myStringArray));

    Console.WriteLine(myIntArray);
    Console.WriteLine(myIntArray.Length);
    Console.WriteLine(string.Join(", ", myIntArray));
}
```
При ініціалізації не потрібно вказувати розмір массиву. Також можна опустити new type[]

## Неявна типізація

```cs
UsingVarForArray();

static void UsingVarForArray()
{
    var myIntArray = new[] { 1, 2, 3 };
    var myDoubleArray = new[] { 1, 2.1, 3 };
    var myString = new[] { "low", "normal", "high" };

    //var badArray = new[] { 1, "Hi", true }; It do not work

    Console.WriteLine(myIntArray);
    Console.WriteLine(myString);
    Console.WriteLine(myDoubleArray);
}

```
Тип визначае компілятор і його можна побачити у редакторі.

```cs
UsingObjectForArray();
static void UsingObjectForArray()
{
    object[] myArray = new object[] { 1, "Hi", true };

    foreach (var item in myArray)
    {
        Console.WriteLine(item);
    }
}
```
Оскільки object є тип від якого походять інщі базові вдудовагі типи це працює.

## Багатовимірні масиви

```cs
UsingRectangularArray();
static void UsingRectangularArray()
{
    int[,] myArray = new int[3, 4];

    for (int i = 0; i < 3; i++)
    {
        for (int j = 0; j < 4; j++)
        {
            myArray[i, j] = i + j;
            Console.Write(myArray[i,j]+" ");
        }
        Console.WriteLine();
    }
}
```

```cs
UsingJaggedArray();

static void UsingJaggedArray()
{
    int[][] myArray = new int[3][];

    for (int i = 0; i < myArray.Length; i++)
    {
        myArray[i] = new int[i + 2];
    }

    for (int i = 0; i < myArray.Length; i++)
    {
        for (int j = 0; j < myArray[i].Length; j++)
        {
            Console.Write(myArray[i][j]+" ");
        }
        Console.WriteLine();
    }
}
```
## Основні можливості классу System.Array

```cs
ArrayFunctionality();

static void ArrayFunctionality()
{
    string[] myArray = new string[] { "good", "better", "best" };

    PrintArray(myArray);

    Console.WriteLine(myArray.Rank); 

    Console.WriteLine(myArray.Length);

    Array.Reverse(myArray);

    PrintArray(myArray);

    Array.Clear(myArray,2,1);

    PrintArray(myArray);

    Array.Clear(myArray);

    PopulateArray(myArray);

    PrintArray(myArray);

    Array.Sort(myArray);

    PrintArray(myArray);


    static void PrintArray(string[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            Console.Write(array[i] + " ");
        }
        Console.WriteLine();

    }

    static void PopulateArray(string[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = (10-i).ToString();
        }
    }

}
```
System.Array має багато статичних методів для роботи з масивами. Як бачите сортування залежить від типу.