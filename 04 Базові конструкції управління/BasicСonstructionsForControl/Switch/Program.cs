
//SimpleSwitch();
static void SimpleSwitch()
{
    Console.WriteLine("Choose: ");
    Console.WriteLine("1. tea");
    Console.WriteLine("2. coffee");
    Console.WriteLine("3. water");
    Console.Write("Enter your varient:");
    
    int.TryParse(Console.ReadLine(), out int enteredVariant); Console.Clear();

    string result;
    switch (enteredVariant)
    {
        case 1: result = "I'm making tea";
            break;
        case 2:
            result = "I'm making coffee";
            break;
        case 3:
            result = "I'm pouring water";
            break;
        default: result = "Invalid input";
            break;
    }
    Console.WriteLine(result);
}


SimpleSwitchWithString();
static void SimpleSwitchWithString()
{
    Console.WriteLine("Choose: ");
    Console.WriteLine("tea");
    Console.WriteLine("coffee");
    Console.WriteLine("water");
    Console.Write("Enter your varient:");

    string enteredVariant = (Console.ReadLine()).ToLower(); Console.Clear();

    string result;
    switch (enteredVariant)
    {
        case "tea":
            result = "I'm making tea";
            break;
        case "coffee":
            result = "I'm making coffee";
            break;
        case "water":
            result = "I'm pouring water";
            break;
        default:
            result = "Invalid input";
            break;
    }
    Console.WriteLine(result);
}


