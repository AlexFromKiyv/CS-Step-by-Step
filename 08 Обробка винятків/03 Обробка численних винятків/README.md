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

## Загальний оператор catsh

