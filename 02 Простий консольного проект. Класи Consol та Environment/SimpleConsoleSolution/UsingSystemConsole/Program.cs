UsingConsoleForInputOutputString();
static void UsingConsoleForInputOutputString()
{
    // Input string
 
    Console.Write("Enter name:");
    string? name = Console.ReadLine();
    Console.Write("What do you like?:");
    string? interests = Console.ReadLine();

    Console.Clear();

    //Output string

    Console.WriteLine("Hi {0} ! You like {1}", name, interests);
    Console.WriteLine($"Hi {name} ! You like {interests}");
}

//UsingConsoleColor();
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
