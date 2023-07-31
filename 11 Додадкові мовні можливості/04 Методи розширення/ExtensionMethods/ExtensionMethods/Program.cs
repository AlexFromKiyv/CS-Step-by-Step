using ExtensionMethods;

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

InvokeExtentionMethod();