//ExplorationOfDateTime();
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

//ExplorationOfDateOnlyTimeOnly();
static void ExplorationOfDateOnlyTimeOnly()
{
    DateOnly myDate = new DateOnly(1945, 6, 22);
    Console.WriteLine(myDate);

    TimeOnly myTime = new TimeOnly(4, 00, 10);
    Console.WriteLine(myTime);
}


ExplorationOfTimeSpan();

static void ExplorationOfTimeSpan()
{
    TimeSpan myTimeSpan = new TimeSpan(5, 0, 0);
    Console.WriteLine(myTimeSpan);

    TimeOnly myTime = new TimeOnly(9, 0, 0);
    Console.WriteLine(myTime);

    Console.WriteLine(myTime.Add(myTimeSpan));

    TimeSpan myShortTimeSpan = myTimeSpan.Subtract(new TimeSpan(4, 55, 0));
    Console.WriteLine(myShortTimeSpan);
}