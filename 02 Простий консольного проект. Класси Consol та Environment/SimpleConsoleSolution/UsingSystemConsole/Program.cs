//UsingConsoleForInputOutputString();
static void UsingConsoleForInputOutputString()
{
    // Input string
    Console.Write("Enter name:");
    string? name = Console.ReadLine();
    Console.Write("What do you like?:");
    string? interests = Console.ReadLine();

    Console.Clear();

    //Output string
    Console.WriteLine("Hi {0} !", name);
    Console.Write($"You like {interests}.");
    Console.WriteLine(" Ok!");
}

UsingConsoleColor();
static void UsingConsoleColor()
{
    ConsoleColor beginColor = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Red;
    for (int i = 0; i < 20; i++)
    {
        Console.WriteLine("DANGER!!!");
    }
    Console.Beep();
    Console.ForegroundColor = beginColor;
}
