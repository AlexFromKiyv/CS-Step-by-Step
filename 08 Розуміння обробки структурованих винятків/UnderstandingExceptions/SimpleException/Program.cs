using SimpleException;
using System.Collections;


void ToDo()
{
    Car car = new();
    car.CrankTunes(false);
}
//ToDo();

void TheSimplestPossibleExample()
{
    Car car = new Car("Zippy",20);
    car.CrankTunes(true);
    for (int i = 0; i < 10; i++)
    {
        car.Accelerate(10);
    }
}
//TheSimplestPossibleExample();


void ThrowingAGeneralException()
{
    Car1 car = new Car1("Zippy", 20);
    car.CrankTunes(true);
    for (int i = 0; i < 10; i++)
    {
        car.Accelerate(10);
    }
}
//ThrowingAGeneralException();

void CatchingExceptions()
{
    Car1 car = new Car1("Zippy", 20);
    car.CrankTunes(true);
    try
    {
        // Speed up past the car's max speed to
        // trigger the exception.
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(10);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("\n\t Exceptions");
        Console.WriteLine($"TargetSite: {ex.TargetSite}");
        Console.WriteLine($"Message: {ex.Message}");
        Console.WriteLine($"Source: {ex.Source}");
        //throw;
    }
    Console.WriteLine("\tOut of try/catch");
}
//CatchingExceptions();

void TheTargetSiteProperty()
{
    Car1 car = new Car1("Zippy", 80);
    try
    {
        // Speed up past the car's max speed to
        // trigger the exception.
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(10);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("\n\t Exceptions");
        Console.WriteLine($"TargetSite: {ex.TargetSite}");
        Console.WriteLine($"TargetSite.DeclaringType: {ex.TargetSite!.DeclaringType}");
        Console.WriteLine($"TargetSite.MemberType: {ex.TargetSite.MemberType}");
    }
}
//TheTargetSiteProperty();

void TheStackTraceProperty()
{
    Car1 car = new Car1("Zippy", 80);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(10);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("\n\t Exceptions");
        Console.WriteLine($"StackTrace: {ex.StackTrace}");
    }
}
//TheStackTraceProperty();

void TheHelpLinkProperty()
{
    Car1 car = new Car1("Zippy", 80);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(10);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("\n\t Exceptions");
        Console.WriteLine($"HelpLink: {ex.HelpLink}");
    }
}
//TheHelpLinkProperty();

void TheDataProperty()
{
    Car1 car = new Car1("Zippy", 80);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(10);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("\n\t Exceptions");
        Console.WriteLine("\nCustom Data:");
        foreach (DictionaryEntry de in ex.Data)
        {
            Console.WriteLine($"{de.Key} {de.Value}");
        }
    }
}
TheDataProperty();

void SystemLevelExceptions()
{
    // True! NullReferenceException is-a SystemException.
    NullReferenceException nullRefEx = new NullReferenceException();
    Console.WriteLine($"NullReferenceException is-a SystemException? : " +
        $"{nullRefEx is SystemException}");
}
//SystemLevelExceptions();