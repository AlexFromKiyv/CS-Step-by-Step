using ProcessMultipleExceptions;

void CatchingExceptions4()
{
    Car car = new Car("Rusty", 90);
    try
    {
        // Trip Arg out of range exception.
        car.Accelerate(-10);
    }
    catch (CarIsDeadException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch(ArgumentOutOfRangeException ex)  
    {
        Console.WriteLine(ex.Message);
    }
}
//CatchingExceptions4();

void CatchingExceptions5()
{
    Car car = new Car("Rusty", 90);
    try
    {
        // Trip Arg out of range exception.
        car.Accelerate(-10);
    }
    //catch(Exception ex)
    //{
    //    Console.WriteLine(ex.Message);
    //}   
    catch (CarIsDeadException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine(ex.Message);
    }
}
//CatchingExceptions5();

void CatchingExceptions6()
{
    Car car = new Car("Rusty", 70);
    try
    {
        car.Accelerate(50);
    }
    catch (CarIsDeadException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine(ex.Message);
    }
    // This will catch any other exception
    // beyond CarIsDeadException or
    // ArgumentOutOfRangeException.
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
//CatchingExceptions6();


void CatchingExceptions7()
{
    Car car = new Car("Rusty", 70);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(10);
        }
    }
    catch 
    {
        Console.WriteLine("Something bad happened...");
    }
}
//CatchingExceptions7();


void CatchingExceptions8()
{
    Car car = new Car("Rusty", 70);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(10);
        }
    }
    catch(CarIsDeadException ex)
    {
        Console.WriteLine(ex.Message);
        // Do any partial processing of this error and pass the buck.
        throw;
    }
    catch
    {
        Console.WriteLine("Something bad happened...");
    }
}
//CatchingExceptions8();

void CatchingExceptions9()
{
    Car car = new Car("Rusty", 70);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(10);
        }
    }
    catch (CarIsDeadException ex)
    {
        Console.WriteLine(ex.Message);
        FileStream fileStream = File.Open(@"D:\carError.txt", FileMode.Open);
    }
    catch (Exception ex) 
    {
        Console.WriteLine(ex.Message);     
    }     
 
}
//CatchingExceptions9();
void CatchingExceptions10()
{
    Car car = new Car("Rusty", 70);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(10);
        }
    }
    catch (CarIsDeadException ex1)
    {
        Console.WriteLine(ex1.Message);
        try
        {
            FileStream fileStream = File.Open(@"D:\carError.txt", FileMode.Open);
        }
        catch (Exception ex2)
        {
            //This causes a compile error-InnerException is read only
            //ex1.InnerException = ex2;

            // Throw an exception that records the new exception,
            // as well as the message of the first exception.
            throw new CarIsDeadException(ex1.Message, ex1.ErrorTimeStamp, ex1.CauseOfError,
                ex2);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
//CatchingExceptions10();

void CatchingExceptions11()
{
    try
    {
        CatchingExceptions10();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex?.InnerException?.Message);
    }
}
//CatchingExceptions11();


void BlockFinally()
{
    Car car = new Car("Rusty", 70);
    car.CrankTunes(true);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(10);
        }
    }
    catch(CarIsDeadException ex)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine(ex.CauseOfError);
        FileStream fileStream = File.Open(@"D:\carError.txt", FileMode.Open);
    }
    catch(Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    finally
    {
        // This will always occur. Exception or not.
        car.CrankTunes(false);
    }
}
//BlockFinally();

void ExceptionFilters()
{
    Car car = new Car("Rusty", 70);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(10);
        }
    }
    catch (CarIsDeadException ex) when (ex.ErrorTimeStamp.DayOfWeek != DayOfWeek.Friday)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine(ex.CauseOfError);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
//ExceptionFilters();

void DebuggingUnhandledExceptions()
{
    Car car = new Car("Rusty", 70);
    car.Accelerate(50);
}
//DebuggingUnhandledExceptions();