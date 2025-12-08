Console.Write("Enter your name:");
string? name = Console.ReadLine();
Console.Clear();
Console.WriteLine("Today is " + DateTime.Now.DayOfWeek);
Console.WriteLine($"Have a nice day {name}");
