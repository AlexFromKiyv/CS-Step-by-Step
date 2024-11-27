using BaseClassExeption;
using System.Collections;

void ExplorationTheOccurationOfAnException()
{
    Car_v1 car = new("Nissan Leaf", 35);

    for (int i = 0; i < 10; i++)
    {
        car.Accelerate(20);
    }

}
//ExplorationTheOccurationOfAnException();

void ExplorationThrow()
{
    Car_v2 car = new("Nissan Leaf", 35);

    for (int i = 0; i < 10; i++)
    {
        car.Accelerate(20);
    }
}
//ExplorationThrow();
void ExplorationTryCatch()
{
    Car_v2 car = new("Nissan Leaf", 35);

    Console.WriteLine("---The begin of try---\n");
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(20);
        }

    }
    catch (Exception e)
    {
        string stringForShow = "\n\n" +
            $"Attention! Problem occured!\n\n" +
            $" Method: {e.TargetSite}\n" +
            $" Message: {e.Message}\n" +
            $" Source: {e.Source}\n";
        Console.WriteLine(stringForShow);
    }
    Console.WriteLine("---The end of try---");
}
//ExplorationTryCatch();

void ExplorationExceptionMemberTargetSite()
{
    Car_v2 car = new("Nissan Leaf", 35);

    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(20);
        }

    }
    catch (Exception e)
    {
        string stringForShow = "\n\n" +
            $" Member Name: {e.TargetSite}\n" +
            $" Class defining member: {e.TargetSite?.DeclaringType}\n" +
            $" Memeber Type: {e.TargetSite?.MemberType}\n";
        Console.WriteLine(stringForShow);
    }
}
//ExplorationExceptionMemberTargetSite();

void ExplorationExceptionMemberStackTrace()
{
    Car_v2 car = new("Nissan Leaf", 35);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(20);
        }
    }
    catch (Exception e)
    {
        string stringForShow = "\n\n" +
            $" Stack: {e.StackTrace}\n";
        Console.WriteLine(stringForShow);
    }
}
//ExplorationExceptionMemberStackTrace();

void ExplorationExceptionMemberHelpLink()
{
    Car_v3 car = new("Nissan Leaf", 35);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(20);
        }
    }
    catch (Exception e)
    {
        string stringForShow = "\n\n" +
            $" Help link: {e.HelpLink}\n";
        Console.WriteLine(stringForShow);
    }
}
//ExplorationExceptionMemberHelpLink();

void ExplorationExceptionMemberData()
{
    Car_v4 car = new("Nissan Leaf", 35);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(20);
        }
    }
    catch (Exception e)
    {
        Console.WriteLine();

        string stringForShow = "\nProblem:\n";
        foreach (DictionaryEntry item in e.Data)
        {
            stringForShow += $"{item.Key} : {item.Value}\n";
        }
        Console.WriteLine(stringForShow);
    }
}
//ExplorationExceptionMemberData();

void UsingIf()
{
    while (true)
    {
        Console.Write("Enter whole number:");
        PrintIncrement(Console.ReadLine());
    }

    void PrintIncrement(string? enteredString)
    {
        if (int.TryParse(enteredString, out int number))
        {
            number++;
            Console.WriteLine(number);
        }
        else
        {
            Console.WriteLine("You entered not whole number!");
        }
    }
}
UsingIf();