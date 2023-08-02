using ExtensionMethods;
using System.Runtime.InteropServices;

void InvokeExtentionMethod()
{
    int myInt = 123;
    myInt.DisplayDefiningAssembly();

    Console.WriteLine(myInt.ReverseDigits());

    string? myString = "Hi girl";

    myString.DisplayDefiningAssembly();

    Console.WriteLine(myString.ReverseChars());

    // myString.ReverseDigits(); have no for string

    System.Data.DataSet ds = new();
    ds.DisplayDefiningAssembly();
}

//InvokeExtentionMethod();

void ExtentionForInterface()
{
    string[] strings = { "Hi", "girl", "!", "How", "are", "you", "?" };

    strings.Print();

    List<int> ints = new() { 1, 2, 3, };

    ints.Print();
}

//ExtentionForInterface();

void GetEnumeratorAsExtention()
{
    Car[] cars = 
    {
        new("VW Beetle",30),
        new("VW Golf",40),
        new("VW Passat",35)
    };

    Garage garage = new(cars);

    foreach (var item in garage)
    {
        Console.WriteLine(item);
    }

}

GetEnumeratorAsExtention();