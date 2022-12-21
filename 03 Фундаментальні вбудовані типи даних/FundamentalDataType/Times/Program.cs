ExplorationOfDateTime();
static void ExplorationOfDateTime()
{
    DateTime myDate = DateTime.Now;
    Console.WriteLine($"Is value type?: {myDate is ValueType}");
    Console.WriteLine($"Now {myDate}");
    Console.WriteLine($"It {myDate.DayOfWeek}");
    
    DateTime  myDateAdd30Days = myDate.AddDays( 30 );
    Console.WriteLine($"After 30 days:{myDateAdd30Days}");
    
    DateTime myDateAddMonth = myDate.AddMonths( 1 );
    Console.WriteLine($"After month:{myDateAddMonth}");

    Console.WriteLine($"ToLongDateString: {myDate.ToLongDateString()}");

    myDate = new DateTime(1914, 07, 14);
    Console.WriteLine($"{myDate.Day} {myDate.Month} {myDate.Year} {myDate.DayOfWeek}");
}
