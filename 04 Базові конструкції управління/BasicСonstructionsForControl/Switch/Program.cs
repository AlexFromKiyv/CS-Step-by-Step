
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


//SimpleSwitchWithString();
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

//SwitchWithEnum_1();
static void SwitchWithEnum_1()
{
    Console.Write("Enter day of the week:");

    DayOfWeek dayOfWeek;

    try
    {
        dayOfWeek = (DayOfWeek) Enum.Parse(typeof(DayOfWeek),Console.ReadLine());
    }
    catch (Exception)
    {
        Console.WriteLine("Bad input!");
        return;
    }
    Console.Clear();

    string result;

    switch (dayOfWeek)
    {
        case DayOfWeek.Sunday: result = "Long walk.";
            break;
        case DayOfWeek.Monday: 
            result = "Morning exercise only";
            break;
        case DayOfWeek.Tuesday: 
            result = "Run";
            break;
        case DayOfWeek.Wednesday: 
            result = "Morning exercise only.";
            break;
        case DayOfWeek.Thursday: 
            result = "Run";
            break;
        case DayOfWeek.Friday:
            result = "Morning exercise only.";
            break;
        case DayOfWeek.Saturday:
            result = "Long walk.";
            break;
        default:
            result = "Bad input!";
            break;
    }
    Console.WriteLine(result);
}

SwitchWithEnum_2();
static void SwitchWithEnum_2()
{
    DayOfWeek dayOfWeek = DayOfWeek.Saturday;
   
    string result;

    switch (dayOfWeek)
    {
        case DayOfWeek.Saturday:
        case DayOfWeek.Sunday:
            result = "On weekend long walk.";
            break;
        case DayOfWeek.Monday:
            result = "Morning exercise only";
            break;
        case DayOfWeek.Tuesday:
            result = "Run";
            break;
        case DayOfWeek.Wednesday:
            result = "Morning exercise only.";
            break;
        case DayOfWeek.Thursday:
            result = "Run";
            break;
        case DayOfWeek.Friday:
            result = "Morning exercise only.";
            break;
        default:
            result = "Bad input!";
            break;
    }
    Console.WriteLine(result);
}
