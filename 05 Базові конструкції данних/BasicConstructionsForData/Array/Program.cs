//UsingSimpleArray();

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

//ArrayInitialization();

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

//UsingVarForArray();

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


//UsingObjectForArray();
static void UsingObjectForArray()
{
    object[] myArray = new object[] { 1, "Hi", true };

    foreach (var item in myArray)
    {
        Console.WriteLine(item);
    }
}


//UsingRectangularArray();
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


//UsingJaggedArray();

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

