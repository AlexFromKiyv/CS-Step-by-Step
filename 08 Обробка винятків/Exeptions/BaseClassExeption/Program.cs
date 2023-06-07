using BaseClassExeption;
using System.Collections;

//ExplorationTheOccurationOfAnException();
void ExplorationTheOccurationOfAnException()
{
    Car_v1 car = new("Nissan Leaf", 35);

	for (int i = 0; i < 10; i++)
	{
		car.Accelerate(20);
	}

}

//ExplorationThrow();
void ExplorationThrow()
{
    Car_v2 car = new("Nissan Leaf", 35);

    for (int i = 0; i < 10; i++)
    {
        car.Accelerate(20);
    }
}

//ExplorationTryCatch();
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
        Console.WriteLine(  );

        string stringForShow = "\n" +
            $"Attention! There is a problem!\n\n" +
            $" Method: {e.TargetSite}\n" +
            $" Message: {e.Message}\n" +
            $" Source: {e.Source}\n";

        Console.WriteLine(stringForShow);
    }
    Console.WriteLine("---The end of try---");
}

//ExplorationExceptionMemberTargetSite();
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
        Console.WriteLine();

        string stringForShow = "\n" +
            $" Member Name: {e.TargetSite}\n" +
            $" Class defining member: {e.TargetSite?.DeclaringType}\n" +
            $" Memeber Type: {e.TargetSite?.MemberType}\n";

        Console.WriteLine(stringForShow);
    }
}

//ExplorationExceptionMemberStackTrace();
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
        Console.WriteLine();

        string stringForShow = "\n" +
            $" Stack: {e.StackTrace}\n";

        Console.WriteLine(stringForShow);
    }
}


//ExplorationExceptionMemberHelpLink();
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
        Console.WriteLine();

        string stringForShow = "\n" +
            $" Help link: {e.HelpLink}\n";

        Console.WriteLine(stringForShow);
    }
}

//ExplorationExceptionMemberData();
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

        string stringForShow = "\nAttention! There is a problem!\n\n";
        foreach (DictionaryEntry item in e.Data)
        {
            stringForShow += $"{item.Key} : {item.Value}\n";
        }
       
        Console.WriteLine(stringForShow);
    }
}

//UsingIf();
void UsingIf()
{

    while (true)
    {
        Console.Write("Enter whole number:");
        PrintIncrement(Console.ReadLine());
    }


    void PrintIncrement(string? enteredString)
    {
        if(int.TryParse(enteredString,out int number))
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