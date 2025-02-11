using CustomException;

void CatchingExceptions1()
{
    Car car = new Car("Zippy", 20);
    car.CrankTunes(true);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(10);
        }
    }
    catch (CarIsDeadException1 ex)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine(ex.ErrorTimeStamp);
        Console.WriteLine(ex.CauseOfError);
    }

}
//CatchingExceptions1();

void CatchingExceptions2()
{
    Car car = new Car("Zippy", 70);
    car.CrankTunes(true);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(10);
        }
    }
    catch (CarIsDeadException2 ex)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine(ex.ErrorTimeStamp);
        Console.WriteLine(ex.CauseOfError);
    }
}
//CatchingExceptions2();


void CatchingExceptions3()
{
    Car car = new Car("Zippy", 80);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(10);
        }
    }
    catch (CarIsDeadException3 ex)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine(ex.ErrorTimeStamp);
        Console.WriteLine(ex.CauseOfError);
    }
}
//CatchingExceptions3();

