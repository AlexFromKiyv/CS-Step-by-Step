//Methods
static void DisplayMessage(string message,ConsoleColor color, int printCount)
{
    ConsoleColor previousColor = Console.ForegroundColor;
    
    Console.ForegroundColor = color;
    for (int i = 0; i < printCount; i++)
    {
        Console.WriteLine(message);
    }

    Console.ForegroundColor = previousColor;
}

static int Add(int x, int y)
{
    return x + y;
}

static string SumToString(int x, int y)
{
    return (x + y).ToString();
}

static void UsingActionDelegate()
{
    // Use the Action<> delegate to point to DisplayMessage.
    Action<string, ConsoleColor, int> actionTarget = DisplayMessage;
    actionTarget("Hi", ConsoleColor.Yellow, 3);
}
//UsingActionDelegate();

static void UsingFuncDelegate()
{
    Func<int, int, int> funcSumInt = Add;
    Console.WriteLine(funcSumInt(1,1));

    Func<int, int, string> funcSumString = SumToString;
    Console.WriteLine(funcSumString(1, 2) is String);
}
UsingFuncDelegate();