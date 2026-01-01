using System.Collections;

void UsingArrayList()
{
    ArrayList strArray = new ArrayList();
    strArray.AddRange(new string[] { "First", "Second", "Third" });

    // Show number of items in ArrayList.
    Console.WriteLine($"This collection has {strArray.Count} items.");
    Console.WriteLine();

    // Add a new item and display current count.
    strArray.Add("Fourth!");
    Console.WriteLine($"This collection has {strArray.Count} items.");
    Console.WriteLine();

    strArray.RemoveAt(1);
    Console.WriteLine("I removed one item.");
    Console.WriteLine();

    // Display contents.
    foreach (string s in strArray)
    {
        Console.WriteLine($"Entry: {s}");
    }
}
UsingArrayList();
