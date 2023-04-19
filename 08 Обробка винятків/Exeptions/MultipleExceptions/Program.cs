using MultipleExceptions;

//ExplorationUncaughtException();
void ExplorationUncaughtException()
{
    Car_v1 car = new("Nissan Leaf", 75);
    try
    {
            car.Accelerate(-20);
    }
    catch (CarIsDead_v1_Exception e)
    {
        Console.WriteLine($"\nMessage:\t{e.Message}");
        Console.WriteLine($"Cause:\t{e.Cause}");
        Console.WriteLine($"Speed:\t{e.Speed}");
    }
}

//ExplorationPairExceptions();
void ExplorationPairExceptions()
{
    //For ArgumentOutOfRangeException
    Console.WriteLine("\nCase 1\n");

    Car_v1 car = new("Nissan Leaf", 75);
    try
    {
        car.Accelerate(-20);
    }
    catch (ArgumentOutOfRangeException e)
    {
        Console.WriteLine($"Message:\t{e.Message}");
        Console.WriteLine($"Parameter name:\t{e.ParamName}");
    }
    catch (CarIsDead_v1_Exception e)
    {
        Console.WriteLine($"Message:\t{e.Message}");
        Console.WriteLine($"Cause:\t{e.Cause}");
        Console.WriteLine($"Speed:\t{e.Speed}");
    }

    //For CarIsDead_v1_Exception
    Console.WriteLine("\nCase 2\n");

    car.CurrentSpeed = 35;

    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(30);
        }
    }
    catch (ArgumentOutOfRangeException e)
    {
        Console.WriteLine($"Message:\t{e.Message}");
        Console.WriteLine($"Parameter name:\t{e.ParamName}");
    }
    catch (CarIsDead_v1_Exception e)
    {
        Console.WriteLine($"Message:\t{e.Message}");
        Console.WriteLine($"Cause:\t{e.Cause}");
        Console.WriteLine($"Speed:\t{e.Speed}");
    }

}

//ExplorationThreeExceptionsBad();
void ExplorationThreeExceptionsBad()
{
    Car_v1 car = new("Nissan Leaf", 35);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(30);
            
            int speed = 0;

            speed = car.CurrentSpeed / speed;
        }
    }

    catch (ArgumentOutOfRangeException e)
    {
        Console.WriteLine($"Message:\t{e.Message}");
        Console.WriteLine($"Parameter name:\t{e.ParamName}");
    }
    catch (CarIsDead_v1_Exception e)
    {
        Console.WriteLine($"Message:\t{e.Message}");
        Console.WriteLine($"Cause:\t{e.Cause}");
        Console.WriteLine($"Speed:\t{e.Speed}");
    }
}

//ExplorationThreeExceptionsGood();
void ExplorationThreeExceptionsGood()
{
    Car_v1 car = new("Nissan Leaf", 35);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(30);

            int speed = 0;

            speed = car.CurrentSpeed / speed;
        }
    }

    catch (ArgumentOutOfRangeException e)
    {
        Console.WriteLine($"Message:\t{e.Message}");
        Console.WriteLine($"Parameter name:\t{e.ParamName}");
    }
    catch (CarIsDead_v1_Exception e)
    {
        Console.WriteLine($"Message:\t{e.Message}");
        Console.WriteLine($"Cause:\t{e.Cause}");
        Console.WriteLine($"Speed:\t{e.Speed}");
    }
    catch (Exception e)
    {
        Console.WriteLine();

        string stringForShow = "\n" +
            $"Attention! There is a problem!\n\n" +
            $" Message: {e.Message}\n" +
            $" Is System:{e is SystemException}\n" +
            e.StackTrace;

        Console.WriteLine(stringForShow);
    }

}

void ExplorationCatchOrder()
{
    Car_v1 car = new("Nissan Leaf", 35);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(30);
        }
    }
    //catch (Exception e) // It don't work. Put it down.
    //{
    //    Console.WriteLine(e.Message);
    //}
    catch (ArgumentOutOfRangeException e)
    {
        Console.WriteLine($"Message:\t{e.Message}");
        Console.WriteLine($"Parameter name:\t{e.ParamName}");
    }
    catch (CarIsDead_v1_Exception e)
    {
        Console.WriteLine($"Message:\t{e.Message}");
        Console.WriteLine($"Cause:\t{e.Cause}");
        Console.WriteLine($"Speed:\t{e.Speed}");
    }
}

//ExplorationGenericCatch();
void ExplorationGenericCatch()
{
    Car_v1 car = new("Nissan Leaf", 135);

    try
    {
        int speed = 0;

        speed = car.CurrentSpeed / speed;

        car.Accelerate(50);
 
    }
    catch 
    {
        Console.WriteLine("Something bad happened.");
    }
    Console.WriteLine("Work after try.");
}

//ExplorationRethrowingException();
void ExplorationRethrowingException()
{
    Car_v1 car = new("Nissan Leaf", 130);
    try
    {
        car.Accelerate(11);

        int speed = 0;
        speed = car.CurrentSpeed / speed;
    }

    catch (CarIsDead_v1_Exception e)
    {
        Console.WriteLine($"\nMessage:{e.Message}");
        Console.WriteLine($"Cause:\t{e.Cause}");
        Console.WriteLine($"Speed:\t{e.Speed}\n\n");
        throw;
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}

//ExplorationUnhandledInnerException();
void ExplorationUnhandledInnerException()
{
    Car_v1 car = new("Nissan Leaf", 130);
    try
    {
        car.Accelerate(11);
    }

    catch (CarIsDead_v1_Exception e)
    {
        FileStream fileStream = File.Open(@"D:\carError.txt", FileMode.Open);

        Console.WriteLine($"\nMessage:{e.Message}");
        Console.WriteLine($"Cause:\t{e.Cause}");
        Console.WriteLine($"Speed:\t{e.Speed}\n\n");
    }
}

//ExplorationAttemptHandledInnerException();
void ExplorationAttemptHandledInnerException()
{
    Car_v1 car = new("Nissan Leaf", 130);
    try
    {
        car.Accelerate(11);
    }

    catch (CarIsDead_v1_Exception e)
    {
        try
        {
            FileStream fileStream = File.Open(@"D:\carError.txt", FileMode.Open);
        }
        catch (Exception iE)
        {
            Console.WriteLine(iE.Message);
        }

        Console.WriteLine($"\nMessage:{e.Message}");
        Console.WriteLine($"Cause:\t{e.Cause}");
        Console.WriteLine($"Speed:\t{e.Speed}\n");

        Console.WriteLine($"Inner Exeption is null:\t{e.InnerException is null}");
    }
}



//ExplorationWriteIntoInnerException();
void ExplorationWriteIntoInnerException()
{
 
    Car_v1 car = new("Nissan Leaf", 130);

    try
    {
        MyAccelerate(11, car);
    }
    catch (CarIsDead_v1_Exception e)
    {
        Console.WriteLine(e.Message);
        Console.WriteLine(e.Cause);
        Console.WriteLine(e.Speed);
        Console.WriteLine("");
        Console.WriteLine($"InnerException:{e.InnerException?.Message}");
    }
    
 void MyAccelerate(int delta, Car_v1 car)
    {
        try
        {
            car.Accelerate(delta);
        }
        catch (CarIsDead_v1_Exception e)
        {
            try
            {
                FileStream fileStream = File.Open(@"D:\carError.txt", FileMode.Open);
            }
            catch (FileNotFoundException e1)
            {
                throw new CarIsDead_v1_Exception(e.Cause, e.Speed, e.Message, e1);
            }
            Console.WriteLine($"\nMessage:{e.Message}");
            Console.WriteLine($"Cause:\t{e.Cause}");
            Console.WriteLine($"Speed:\t{e.Speed}\n");
            Console.WriteLine($"InnerException is null:\t{e.InnerException is null}");
        }
    }
}


//ExplorationFinally();
void ExplorationFinally()
{

    Car_v2 car = new("Nissan Leaf", 90, 140);
    car.RadioSwitch(true);
    try
    {
        car.Accelerate(20);
        car.Accelerate(20);
        car.Accelerate(20);
    }
    catch (CarIsDead_v2_Exception e)
    {
        Console.WriteLine();
        Console.WriteLine($"Message:\t{e.Message}");
        Console.WriteLine($"Cause:\t{e.Cause}");
        Console.WriteLine($"Speed:\t{e.Speed}");
        Console.WriteLine($"Time:\t{e.ErrorTimeStamp}");
        Console.WriteLine();
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
    finally
    {
        car.RadioSwitch(false);
    }        

}

//ExplorationCathWhen();
void ExplorationCathWhen()
{

    Car_v2 car = new("Nissan Leaf", 90, 140);
    car.RadioSwitch(true);
    try
    {
        car.Accelerate(20);
        car.Accelerate(20);
        car.Accelerate(20);
    }
    catch (CarIsDead_v2_Exception e) when(e.ErrorTimeStamp.DayOfWeek == DayOfWeek.Wednesday)
    {
        Console.WriteLine();
        Console.WriteLine($"Message:\t{e.Message}");
        Console.WriteLine($"Cause:\t{e.Cause}");
        Console.WriteLine($"Speed:\t{e.Speed}");
        Console.WriteLine($"Time:\t{e.ErrorTimeStamp}");
        Console.WriteLine();
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
    finally
    {
        car.RadioSwitch(false);
    }
}


UsingDebugging();
void UsingDebugging()
{
    Car_v2 car = new("Nissan Leaf", 110, 140);
    car.Accelerate(20);
    car.Accelerate(20);
}