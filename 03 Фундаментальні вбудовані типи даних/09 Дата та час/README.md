# Дата та час

Структури System.DateTime, System.TimeSpan, допомогає працювати з змінними часу. Додамо проект Times.

 ```cs
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
ExplorationOfDateTime();
```
```
Is value type?: True
Now 09.12.2025 10:26:03
It Tuesday
After 30 days:08.01.2026 10:26:03
After month:09.01.2026 10:26:03
ToLongDateString: 9 грудня 2025 р.
14 7 1914 Tuesday

```

Крім того існують стрктури DateOnly, TimeOnly яку корисні при роботы с типами SQL Server Date / Time.

```cs
static void ExplorationOfDateOnlyTimeOnly()
{
    DateOnly myDate = new DateOnly(1945, 6, 22);
    Console.WriteLine(myDate);

    TimeOnly myTime = new TimeOnly(1, 30, 10);
    Console.WriteLine(myTime);
}
ExplorationOfDateOnlyTimeOnly();
```
```
22.06.1945
1:30
```
Структура TimeSpan дозволяє працювати з проміжком часу

```cs
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
ExplorationOfTimeSpan();

```
```
05:00:00
9:00
14:00
00:05:00
```

 