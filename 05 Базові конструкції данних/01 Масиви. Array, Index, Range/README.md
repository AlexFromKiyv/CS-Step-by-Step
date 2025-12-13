#  Масиви. Array, Index, Range

Додамо рішеня BasicConstructionsForData з проектом Array

# Array

## Створення

Масив це набір єлементів одного типу з доступом через числові індекси.

```cs
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
UsingSimpleArray();
```
```
1
2
3
0
0
0
0
0
0
0
0
0
0
```
Треба зауважити new int[3] означає що загальна кількість єлементів 3. Індексація починаеться з 0. Коли ви задекларували масив память виділяеться і він заповнюється значеннями default. В цьому прикладі 0-ми. 

## Ініціалізація

Масив можна задекларувати і одразу задати значення єлементам. 

```cs
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
ArrayInitialization();
```
```
System.String[]
3
good < better < best
System.Int32[]
3
10, 15, 20
```

При ініціалізації не потрібно вказувати розмір массиву. Також можна опустити new type[]

## Неявна типізація

```cs
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
UsingVarForArray();
```
```
System.Int32[]
System.String[]
System.Double[]
```
Тип визначае компілятор і його можна побачити у редакторі.

```cs
static void UsingObjectForArray()
{
    object[] myArray = new object[] { 1, "Hi", true };

    foreach (var item in myArray)
    {
        Console.WriteLine(item);
    }
}
UsingObjectForArray();
```
```
1
Hi
True
```
Оскільки object є тип від якого походять інщі базові типи це працює.

## Багатовимірні масиви

```cs
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
UsingRectangularArray();
```
```
0 1 2 3
1 2 3 4
2 3 4 5
```

```cs
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
UsingJaggedArray();
```
```
0 0
0 0 0
0 0 0 0
```

## Основні можливості классу System.Array

```cs
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
ArrayFunctionality();
```
```
good better best
1
3
best better good
best better
10 9 8
10 8 9

```

System.Array має багато статичних методів для роботи з масивами. Як бачите сортування залежить від типу.

## Index, Rang 

Представляє індекс в послідовності.

```cs
static void UsingIndex()
{
    int[] myArray = new int[] { 10, 20, 30 };

    for (int i = 0; i < myArray.Length; i++)
    {
        Index index= i;
        Console.Write(myArray[index].ToString() + " ");
    }
    Console.WriteLine();

    for (int i = 1; i < myArray.Length+1; i++)
    {
        Index index = ^i;
        Console.Write(myArray[index].ToString() + " ");
    }
}
UsingIndex();
```
```
10 20 30
30 20 10
```

```cs
static void UsingRang()
{
    int[] myArray = new int[] { 10, 20, 30 };
    
    foreach (var item in myArray[0..2])
    {
        Console.Write(item.ToString() + " ");
    }
    Console.WriteLine();

    Range range = 0..2;

    foreach (var item in myArray[range])
    {
        Console.Write(item.ToString() + " ");
    }
    Console.WriteLine();

    Index index1 = 0;
    Index index2 = 2;
    Range range1 = index1..index2;

    foreach (var item in myArray[range1])
    {
        Console.Write(item.ToString() + " ");
    }
    Console.WriteLine();

    foreach (var item in myArray[..])
    {
        Console.Write(item.ToString() + " ");
    }
    Console.WriteLine();

    foreach (var item in myArray[0..2])
    {
        Console.Write(item.ToString() + " ");
    }
    Console.WriteLine();

    foreach (var item in myArray[0..])
    {
        Console.Write(item.ToString() + " ");
    }
    Console.WriteLine();

    foreach (var item in myArray[..1])
    {
        Console.Write(item.ToString() + " ");
    }
    Console.WriteLine();

    int myInt = myArray.ElementAt(^1);
    Console.WriteLine(myInt);
}
UsingRang();
```
```
10 20
10 20
10 20
10 20 30
10 20
10 20 30
10
30
```