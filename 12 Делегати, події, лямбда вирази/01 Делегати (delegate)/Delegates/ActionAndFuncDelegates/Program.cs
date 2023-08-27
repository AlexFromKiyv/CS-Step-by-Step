using System.Threading.Channels;

void UseActionDelegate()
{
    Action<string, ConsoleColor, int> printToConsole = new(MessageToConsole);

    printToConsole("Hi!", ConsoleColor.Yellow, 3);


    void MessageToConsole(string message,ConsoleColor txtColor, int printCount)
    {
        ConsoleColor previousColor = Console.ForegroundColor;
        Console.ForegroundColor = txtColor;

        for (int i = 0; i < printCount; i++)
        {
            Console.WriteLine(message);
        }

        Console.ForegroundColor = previousColor;
    }
}

//UseActionDelegate();


void UseFuncDelegate()
{
    Func<int, int, int> biIntOp = Add;
    int result = biIntOp(5,5);
    Console.WriteLine(result);

    biIntOp += Subtract;
    foreach (var item in biIntOp.GetInvocationList())
    {
        Console.WriteLine(biIntOp.Method);
    }
    result = biIntOp.Invoke(5,5);
    Console.WriteLine(result);

    Func<string?> getNumberAsString = InputString;
    Console.WriteLine(getNumberAsString());

    // Local function for delegates
    static int Add(int x,int y) =>  x + y;

    static int Subtract(int x, int y) => x - y;

    static string? InputString()
    {
        Console.Write("Input whole number:");
        string? input = Console.ReadLine();

        return int.TryParse(input,out int _) ? input : null;
    } 
}
UseFuncDelegate();