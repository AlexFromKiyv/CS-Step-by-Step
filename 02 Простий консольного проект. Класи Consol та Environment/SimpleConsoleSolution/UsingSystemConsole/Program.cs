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

    Console.WriteLine("Hi {0}! {0} like {1}.", name, interests);
    Console.WriteLine("Hi {1}! {1} like {0}.", interests, name );
    Console.WriteLine($"Hi {name}! {name} like {interests}.");
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

UsingNumericalFormatingForConsole();

static void UsingNumericalFormatingForConsole()
{
    Console.WriteLine($"For money C: {10000.00023:C}"); // гривня покаже ?
    Console.WriteLine($"For decimal D: {10023:D}");
    Console.WriteLine($"For decimal D9: {10023:D7}");
    Console.WriteLine($"For exponencial format E: {0.000025:E}");
    Console.WriteLine($"For fixed-poind format F: {0.025:F}");
    Console.WriteLine($"For fixed-poind format F3: {0.025:F3}");
    Console.WriteLine($"For fixed and exponencial format G: {1000000:G}");
    Console.WriteLine($"For fixed and exponencial format G: {0.00025:G}");
    Console.WriteLine($"For numerical format N: {0.025:N}");
    Console.WriteLine($"For numerical format N: {1000000:N}");
    Console.WriteLine($"Hexadecimal format X:{1000000:X}");
}