void UsingArray1()
{
    string[] strArray = { "First", "Second", "Third" };

    // Show number of items in array using Length property.
    Console.WriteLine($"This array has {strArray.Length} items.");
    Console.WriteLine();
    
    // Display contents using enumerator.
    foreach (string s in strArray)
    {
        Console.WriteLine($"Array Entry: {s}");
    }
    Console.WriteLine();

    // Reverse the array and print again.
    Array.Reverse(strArray);
    foreach (string s in strArray)
    {
        Console.WriteLine($"Array Entry: {s}");
    }

}
//UsingArray1();

void ArrayHasFixedSize()
{
    string[] strArray = { "First", "Second", "Third" };

    strArray[3] = "Hi";
}
//ArrayHasFixedSize();

void SpecifyingTypeParametersForGenericMembers()
{
    int[] myInts = { 10, 4, 2, 33, 93 };
    // Specify the placeholder to the generic
    // Sort<>() method.
    Array.Sort<int>(myInts);
    foreach (int i in myInts)
    {
        Console.WriteLine(i);
    }
}
//SpecifyingTypeParametersForGenericMembers();