# Обробка численних винятків

## Не зловленний спеціальний виняток.

В блоці try можуть виникнути числені винятки. Припустимо ми створили власний виняток і превірили шо він перехоплюється, а потім вирішили поставити ше одне обмеженя на швидкість.

```cs
    class Car_v1
    {

        public const int MAXSPEED = 140;
        private bool _carIsDead;

        public Car_v1(string name, int currentSpeed)
        {
            Name = name;
            CurrentSpeed = currentSpeed;
        }
        public Car_v1() { }

        public string Name { get; set; } = "";
        public int CurrentSpeed { get; set; }


        public void Accelerate(int delta)
        {

            if (delta < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(delta), "Acceleration must be greater than zero.");
            }


            if (_carIsDead)
            {
                Console.WriteLine($"{Name} is out of order ...");
            }
            else
            {
                CurrentSpeed += delta;
                if (CurrentSpeed > MAXSPEED)
                {
                    int tempCurrentSpeed = CurrentSpeed;
                    CurrentSpeed = 0;
                    _carIsDead = true;
                    throw new CarIsDead_v1_Exception("Speed too high.",tempCurrentSpeed,$"{Name} broke down.");

                }
                Console.WriteLine($"Current speed {Name}:{CurrentSpeed}");
            }
        }
    }

    public class CarIsDead_v1_Exception : ApplicationException
    {
        public CarIsDead_v1_Exception()
        {
        }

        public CarIsDead_v1_Exception(string? message) : base(message)
        {
        }

        public CarIsDead_v1_Exception(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CarIsDead_v1_Exception(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CarIsDead_v1_Exception(string? cause, int speed,  string? message) : base(message)
        {
            Cause = cause;
            Speed = speed;
        }

        public string? Cause { get; }
        public int Speed { get; }
    }
```
```cs
ExplorationUncaughtException();
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
```
```
Unhandled exception. System.ArgumentOutOfRangeException: Acceleration must be greater than zero. (Parameter 'delta')
   at MultipleExceptions.Car_v1.Accelerate(Int32 delta) in D:\MyWork\CS-Step-by-Step\08 Обробка винятк?в\Exeptions\MultipleExceptions\Classes_v1.cs:line 32
   at Program.<<Main>$>g__Exploration|0_0() in D:\MyWork\CS-Step-by-Step\08 Обробка винятк?в\Exeptions\MultipleExceptions\Program.cs:line 11
   at Program.<Main>$(String[] args) in D:\MyWork\CS-Step-by-Step\08 Обробка винятк?в\Exeptions\MultipleExceptions\Program.cs:line 3
```
Тут використали визначений систений клас ArgumentOutOfRangeException при передачі неправіилної зміни швидкості. Зауважте шо в цому класі є констуктор який приймає неправільний параметр а потім message. Використаня в цьому випадку nameof безпечний варіант отриманя назви парметру.
В цьому випадку програма викидує виняток і закінчує роботу програми з помилкою. Блок catch можна повторити аби реагувати на різні винятки.

```cs
ExplorationPairExceptions();
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
```
```
Case 1

Message:        Acceleration must be greater than zero. (Parameter 'delta')
Parameter name: delta

Case 2

Current speed Nissan Leaf:65
Current speed Nissan Leaf:95
Current speed Nissan Leaf:125
Message:        Nissan Leaf broke down.
Cause:  Speed too high.
Speed:  155
```
Тепер різні catch реагують на винятки яки ми викидаємо в класі. 

## Бішле винятків чим очікується.

Але блок try може мати багату логіку. Тоді можуть відбутись інші випадки.

```cs
ExplorationThreeExceptionsBad();
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
```
```
Current speed Nissan Leaf:65
Unhandled exception. System.DivideByZeroException: Attempted to divide by zero.
   at Program.<<Main>$>g__ExplorationThreeExceptionsBad|0_2() in D:\MyWork\CS-Step-by-Step\08 Обробка винятк?в\Exeptions\MultipleExceptions\Program.cs:line 80
   at Program.<Main>$(String[] args) in D:\MyWork\CS-Step-by-Step\08 Обробка винятк?в\Exeptions\MultipleExceptions\Program.cs:line 68
```
Тут програма вивалюється з помилкою бо нема блока catch якій зловить всі інші помилки. 
```cs
ExplorationThreeExceptionsGood();
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
``` 
```
Current speed Nissan Leaf:65


Attention! There is a problem!

 Message: Attempted to divide by zero.
 Is System:True
   at Program.<<Main>$>g__ExplorationThreeExceptionsGood|0_3() in D:\MyWork\CS-Step-by-Step\08 Обробка винятк?в\Exeptions\MultipleExceptions\Program.cs:line 109
```
Таким чином потрібен блок який зловить неспецифічні винятки і таким чином буде корректно оброблений.

## Порядок catch.

Порядок в якому ловится виняток має значення.

```cs
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
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
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
```
В цьому випадку компілятор покаже помилку оскільки більш базовий виняток не дасть обробити більш конкретні випадки. Перший блок може обробити всі помилки похідні від Exception і які мають звязок "is-a". Таким чином ви отримаєте тилки message без додадкових даних по проблемі. Тобто чим базовіший виняток тим він повинен стояти нижче.
Останій блок бажано робити інформативним з усіма можливими данними.

## Загальний оператор catch

```cs

ExplorationGenericCatch();
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
```
```
Something bad happened.
Work after try.
```

Catch шо не отримує об'єкту Exception спрацовує коли в try виникає будь-який виняток. Вона буде корисна коли не хочеться ділитись додадковою інформацією і треба видадт загальне зауваження видавати загальне завуваженя для всіх помилок.

## Повторний викид винятку.

```cs
ExplorationRethrowingException();
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
```
```
Message:Nissan Leaf broke down.
Cause:  Speed too high.
Speed:  141


Unhandled exception. MultipleExceptions.CarIsDead_v1_Exception: Nissan Leaf broke down.
   at MultipleExceptions.Car_v1.Accelerate(Int32 delta) in D:\MyWork\CS-Step-by-Step\08 Обробка винятк?в\Exeptions\MultipleExceptions\Classes_v1.cs:line 48
   at Program.<<Main>$>g__ExplorationRethrowingExceptions|0_6() in D:\MyWork\CS-Step-by-Step\08 Обробка винятк?в\Exeptions\MultipleExceptions\Program.cs:line 193
   at Program.<Main>$(String[] args) in D:\MyWork\CS-Step-by-Step\08 Обробка винятк?в\Exeptions\MultipleExceptions\Program.cs:line 187
```
Коли блок catch може лише частвково обробити помилку в блоці можна знову викинути виняток. Середовище виконання знову отримує виняток і закінчує роботу прорами з помилкою.
Тобто це обробка винятку трохи краще ніж це б зробило середовище.

## Inner Exceptions

Під час обробки винятку коли ви обробляєте дані зверетраючись до ресурсів може виникнути інший виняток.

```cs
ExplorationUnhandledInnerException();
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
```
```
Unhandled exception. System.IO.FileNotFoundException: Could not find file 'D:\carError.txt'.
File name: 'D:\carError.txt'
   at Microsoft.Win32.SafeHandles.SafeFileHandle.CreateFile(String fullPath, FileMode mode, FileAccess access, FileShare share, FileOptions options)
   at Microsoft.Win32.SafeHandles.SafeFileHandle.Open(String fullPath, FileMode mode, FileAccess access, FileShare share, FileOptions options, Int64 preallocationSize, Nullable`1 unixCreateMode)
   at System.IO.Strategies.OSFileStreamStrategy..ctor(String path, FileMode mode, FileAccess access, FileShare share, FileOptions options, Int64 preallocationSize, Nullable`1 unixCreateMode)
   at System.IO.Strategies.FileStreamHelpers.ChooseStrategyCore(String path, FileMode mode, FileAccess access, FileShare share, FileOptions options, Int64 preallocationSize, Nullable`1 unixCreateMode)
   at System.IO.File.Open(String path, FileMode mode)
   at Program.<<Main>$>g__ExplorationUnhandledInnerException|0_7() in D:\MyWork\CS-Step-by-Step\08 Обробка винятк?в\Exeptions\MultipleExceptions\Program.cs:line 223
   at Program.<Main>$(String[] args) in D:\MyWork\CS-Step-by-Step\08 Обробка винятк?в\Exeptions\MultipleExceptions\Program.cs:line 212
```
Тут в блоці catch спроба відкрити неіснуючого файлу.(Доступ до типу FileStream забезпечують glodal using). Як видно з прикладу виняток не оброблюється. Краший варіант не накладати на блок catch забагато роботи, а виділити в окрему функцію. Як поступити з винятком який виник при обробці винятку.

```cs
ExplorationHandledInnerException();
void ExplorationHandledInnerException()
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
            Console.WriteLine( iE.Message );
        }

        Console.WriteLine($"\nMessage:{e.Message}");
        Console.WriteLine($"Cause:\t{e.Cause}");
        Console.WriteLine($"Speed:\t{e.Speed}\n");

        Console.WriteLine($"Inner Exeption is null:\t{e.InnerException is null}");
    }
}
```
```
Unhandled exception. MultipleExceptions.CarIsDead_v1_Exception: Nissan Leaf broke down.
 ---> System.IO.FileNotFoundException: Could not find file 'D:\carError.txt'.
File name: 'D:\carError.txt'
   at Microsoft.Win32.SafeHandles.SafeFileHandle.CreateFile(String fullPath, FileMode mode, FileAccess access, FileShare share, FileOptions options)
   at Microsoft.Win32.SafeHandles.SafeFileHandle.Open(String fullPath, FileMode mode, FileAccess access, FileShare share, FileOptions options, Int64 preallocationSize, Nullable`1 unixCreateMode)
   at System.IO.Strategies.OSFileStreamStrategy..ctor(String path, FileMode mode, FileAccess access, FileShare share, FileOptions options, Int64 preallocationSize, Nullable`1 unixCreateMode)
   at System.IO.Strategies.FileStreamHelpers.ChooseStrategyCore(String path, FileMode mode, FileAccess access, FileShare share, FileOptions options, Int64 preallocationSize, Nullable`1 unixCreateMode)
   at System.IO.File.Open(String path, FileMode mode)
   at Program.<<Main>$>g__ExplorationWriteIntoInnerException|0_8() in D:\MyWork\CS-Step-by-Step\08 Обробка винятк?в\Exeptions\MultipleExceptions\Program.cs:line 244
   --- End of inner exception stack trace ---
   at Program.<<Main>$>g__ExplorationWriteIntoInnerException|0_8() in D:\MyWork\CS-Step-by-Step\08 Обробка винятк?в\Exeptions\MultipleExceptions\Program.cs:line 248
   at Program.<Main>$(String[] args) in D:\MyWork\CS-Step-by-Step\08 Обробка винятк?в\Exeptions\MultipleExceptions\Program.cs:line 231
```
Тут не получилось коректно обробити виняток. Але видно шо обробка винятку ініїцювала новий. Краши практики кажуть шо треба використати властивість InnerException того ж типу винятку.
```cs
ExplorationWriteIntoInnerException();
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
        Console.WriteLine(e.InnerException?.Message);
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
```
```
Nissan Leaf broke down.
Speed too high.
141

InnerException:Could not find file 'D:\carError.txt'.
```
Помістити об'ект винятку в InnerException це кращий спосиб задокументувати шо відбувся виняток в обробці винятку. Звернфть увагу шо для цього використовувався відповідний конструктор. Після створення обї'кта винятку ми перекидаєм його в стек викликів. В цьому випадку програма буде оброблювати всі винятки коректо у випадку існувані чи не існування файла.

## finally

Код try/catch може мати також необовьязковий блок finally. Цей блок виконується завжди незалежно від того що відбуваеться в try.

```cs
    class Radio
    {
        public void Switch(bool on)
        {
            Console.WriteLine(on ? "Jamming ..." : "Quiet time...");
        }
    }

    class Car_v2
    {
        public string Name { get; set; } = "";
        public int CurrentSpeed { get; set; } 
        public int MaximumSpeed { get; }

        private readonly Radio radio = new Radio();

        private bool _carIsDead;

        public Car_v2()
        {
        }

        public Car_v2(string name, int currentSpeed, int maximumSpeed)
        {
            Name = name;
            CurrentSpeed = currentSpeed;
            MaximumSpeed = maximumSpeed;
        }

        public void RadioSwitch(bool state)
        {
            radio.Switch(state);
        }

        public void Accelerate(int delta)
        {

            if (delta < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(delta), "Acceleration must be greater than zero.");
            }


            if (_carIsDead)
            {
                Console.WriteLine($"{Name} is out of order ...");
            }
            else
            {
                CurrentSpeed += delta;
                if (CurrentSpeed > MaximumSpeed)
                {
                    int tempCurrentSpeed = CurrentSpeed;
                    CurrentSpeed = 0;
                    _carIsDead = true;
                    throw new CarIsDead_v2_Exception("Speed too high.",DateTime.Now, tempCurrentSpeed, $"{Name} broke down.");

                }
                Console.WriteLine($"Current speed {Name}:{CurrentSpeed}");
            }
        }
    }


    public class CarIsDead_v2_Exception : ApplicationException
    {
        public CarIsDead_v2_Exception()
        {
        }
        public CarIsDead_v2_Exception(string? cause, DateTime time,int speed) : this(cause,time,speed,string.Empty)
        {
        }
        public CarIsDead_v2_Exception(string? cause, DateTime time, int speed, string? message) : this(cause,time,speed,message,null)
        {
            Cause = cause;
            Speed = speed;
        }

        public CarIsDead_v2_Exception(string? cause, DateTime time, int speed, string? message, Exception? innerException) : base(message, innerException)
        {
            Cause = cause;
            Speed = speed;
            ErrorTimeStamp = time;
        }

        public string? Cause { get; }
        public int Speed { get; }
        public DateTime ErrorTimeStamp { get; set; }
    }
```
```cs
ExplorationFinally();
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
```
```
Jamming ...
Current speed Nissan Leaf:110
Current speed Nissan Leaf:130

Message:        Nissan Leaf broke down.
Cause:  Speed too high.
Speed:  150
Time:   19.04.2023 11:38:46

Quiet time...
```
Тут при виконані прискорень вимикаеться радіо. Вимикання буде виконуватися в будьякому випадку чи буде рух чи буде поломка. У більш реальному сценарії, коли вам потрібно позбутися об’єктів, закрити файл або від’єднатися від бази даних (чи що завгодно), це забезпечує блок finally для належного очищення.

## catch ... when ...

На процес коли спрацьовує catch можна поставити додадкові умови.

```cs
ExplorationCathWhen();
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
```
```
Jamming ...
Current speed Nissan Leaf:110
Current speed Nissan Leaf:130

Message:        Nissan Leaf broke down.
Cause:  Speed too high.
Speed:  150
Time:   19.04.2023 11:47:44

Quiet time...

```
Умова в when накладає "фільтр" на обробку винятків. Якшо запустити цей код не в середу то відповідний блок catch не зпрацює і не буде видно деталей помилки.
В реальності цю можливість можна використати для більш детального вивчення проблеми.

## Де ловити винятки.

Методи можуть викликать инщі методи які в свою чергу можуть звертатися до бібліoтеки і визивати інші методи. Цепочка визовів зберігаеться в стеку.
```cs
ExplorationCallStack();
void CallStack()
{
    Method_1();

    void Method_1()
    {
        Console.WriteLine("Method_1");
        Method_2();
    }

    void Method_2()
    {
        Console.WriteLine("Method_2");
        MethodFromLibrary();
    }

    void MethodFromLibrary()
    {
        Console.WriteLine("MethodFromLibrary");
        PrivateMethodFromLibrary();
    }

    void PrivateMethodFromLibrary()
    {
        Console.WriteLine("PrivateMethodFromLibrary");
        int world = int.Parse("World");
    }
}
```
```
Method_1
Method_2
MethodFromLibrary
PrivateMethodFromLibrary
Unhandled exception. System.FormatException: The input string 'World' was not in a correct format.
   at System.Number.ThrowOverflowOrFormatException(ParsingStatus status, ReadOnlySpan`1 value, TypeCode type)
   at System.Int32.Parse(String s)
   at Program.<<Main>$>g__PrivateMethodFromLibrary|0_17() in D:\MyWork\CS-Step-by-Step\08 Обробка винятк?в\Exeptions\MultipleExceptions\Program.cs:line 396
   at Program.<<Main>$>g__MethodFromLibrary|0_16() in D:\MyWork\CS-Step-by-Step\08 Обробка винятк?в\Exeptions\MultipleExceptions\Program.cs:line 390
   at Program.<<Main>$>g__Method_2|0_15() in D:\MyWork\CS-Step-by-Step\08 Обробка винятк?в\Exeptions\MultipleExceptions\Program.cs:line 384
   at Program.<<Main>$>g__Method_1|0_14() in D:\MyWork\CS-Step-by-Step\08 Обробка винятк?в\Exeptions\MultipleExceptions\Program.cs:line 378
   at Program.<<Main>$>g__ExplorationCallStack|0_12() in D:\MyWork\CS-Step-by-Step\08 Обробка винятк?в\Exeptions\MultipleExceptions\Program.cs:line 372
   at Program.<Main>$(String[] args) in D:\MyWork\CS-Step-by-Step\08 Обробка винятк?в\Exeptions\MultipleExceptions\Program.cs:line 369 
```
Методи при визові по черзі попадають в стек і коли виникає виняток можна побачити в зворотн'му порядку як виникала проблема. 

Перехопити виняток можна або в місці виникнення або више по стеку. Рішеня де перехопити виняток може спростити і стандартізувати код.

Іноді бібліотека в якій може виникнути виняток не має досатьно даних шоб корректно її описати. Тоді вона може прехопити виняток зробити запис про нього  та далі викинути виняток шоб він обробився више в стеку.

```cs
CallStackWithThrow();
void CallStackWithThrow()
{
    Method_1();


    void Method_1()
    {
        Console.WriteLine("Method_1");
        Method_2();
    }

    void Method_2()
    {
        Console.WriteLine("Method_2");
        try
        {
            MethodFromLibrary();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exeption here: {ex.Message}");
            throw ex;
        }

    }

    void MethodFromLibrary()
    {
        Console.WriteLine("MethodFromLibrary");
        PrivateMethodFromLibrary();
    }

    void PrivateMethodFromLibrary()
    {
        Console.WriteLine("PrivateMethodFromLibrary");
        int world = int.Parse("World");
    }
}
```
Як ви бачите якшо таким чином перхопити виняток то залишаеться частина визовів стеку. Іноді це пагана практика і щоб залишити повну інформацію замість throw ex екуба визивати throw;

Таким чином слід перехоплювати виняток коли вас є повний обсях даних шо відбулося. Якшо не можете в повній мірі обробити виняток вам слід дозволити винятку пройти через стек викликів на вищий рівень.