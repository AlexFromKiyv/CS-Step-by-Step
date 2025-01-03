# Системні та власно створені винятки.

# Системні винятки.

В бібліотеці базових класів є багато похідних класів від System.Exception на різні випадки. Наприклад в namespace System є ArgumentOutOfRangeException, IndexOutOfRangeException, StackOverflowException. Інші простори імен визначають винятки, які повязані з йх спеціфікою. Наприклад, System.IO визначає винятки на основі вводу/виводу, System.Data визначає винятки, орієнтовані на базу даних, і так далі. 
Винятки які генерує система в процесі роботи називають системними. Таки винятки вважають невиправними або фатальними. Системні винятки походять від класу System.SystemException який є похідним від System.Exception.
Цей клас потрібен для роботи середовища. Коли виникає системний виняток то самє середовище виконяння є ініціатором, а не код програми що виконується. Рішення YourOwnExceptions

YourOwnExceptions\Program.cs
```cs
void ExplorationSystemExceptions()
{
    NullReferenceException exception = new();

    Console.WriteLine( exception is SystemException);
}
ExplorationSystemExceptions();
```
```
True
```
Чи є виняток системни провірити не складно просто використайте is SystemException. Таким чином системні винятки це винятки яки попереджують про те шо система не може щось виконати.

# Власно створені винятки.

## Створення та перехоплення власного винятку. 

Для створення винятків для власного коду було створено окремий клас System.ApplicationException який походить від System.Exeption. Головна мета цього класа віділити системні винятки від ваших власних. 
Хоча завжди можна створити загальний виняток за допомогою Exception, іноді користно створити строго типізовані винтки для конкрентого випадку які характеризують унікальну проблему.
Припустимо треба створити виняток на випадок перевищеня швидкості.
YourOwnExceptions\Classes_v1.cs
```cs
namespace YourOwnExceptions;

public class CarIsDead_v1_Exception : ApplicationException
{
    private string? _messageDetails;
    public string? CauseOfError { get; }
    public int Speed { get; }

    public CarIsDead_v1_Exception()
    {
    }
    public CarIsDead_v1_Exception(string? message, string? cause,int speed)
    {
        _messageDetails = message;
        CauseOfError = cause;
        Speed = speed;
    }
    public override string Message => $"Car error message:\t{_messageDetails}";
}

public class Car_v1
{
    public const int MAXSPEED = 140;
    public string Name { get; set; } = "";
    public int CurrentSpeed { get; set; }

    private bool _carIsDead;

    public Car_v1(string name, int currentSpeed)
    {
        Name = name;
        CurrentSpeed = currentSpeed;
    }
    public Car_v1()
    {
    }
    public void Accelerate(int delta)
    {
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
                throw new CarIsDead_v1_Exception($"{Name} has overheated!", "Speed too high.", tempCurrentSpeed);
            }
            Console.WriteLine($"Current speed {Name}:{CurrentSpeed}");
        }
    }
}
```
```cs
void ExplorationCarIsDead_v1_Exception()
{
    Car_v1 car = new("Nissan Leaf", 35);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(20);
        }
    }
    catch (CarIsDead_v1_Exception e)
    {
        Console.WriteLine($"\n{e.Message}");
        Console.WriteLine($"Cause:\t{e.CauseOfError}");
        Console.WriteLine($"Speed:\t{e.Speed}");
    }
}
ExplorationCarIsDead_v1_Exception();
```
```
Current speed Nissan Leaf:55
Current speed Nissan Leaf:75
Current speed Nissan Leaf:95
Current speed Nissan Leaf:115
Current speed Nissan Leaf:135

Car error message:      Nissan Leaf has overheated!
Cause:  Speed too high.
Speed:  155
```
За домовленністю назви класу закінчуються Exception. 
Оскільки об'єкти такіх класів часто передаються поза збірки клас як правило роблять public.  
Як і в будь-якому класі в спеціальному класі винятку можна створити будь яку кількість члені, які можна потім використати в catsh. Також можна первизначити віртуальні члени.
Тут клас підтримує приватне поле (_messageDetails) яке мітсить дані що до поточного винятку і яке можна встановити за допомогою конструктора. 
Використання throw для спеціального винятку аналогічно Exception за різницею того що використовується інший конструктор. 
На стороні методу де випробується виконання, блок catch потрібно вказати тип якій прехоплюеться і  використовувати розширені можливости цього типу.

## Використовуваня базової реалізації для винятків.

Попередній клас винятку перевизначив властивість і додав додадкові данні. Перевизначення властивості в такий спосіб не дуже корисне тому що шапку повідомленя напевно краще використовувати в мість отриманя. Додадкові дані маєть користь при вивченя винятку. Отже створити власний виняток можна інакше.

```cs
    class Car_v2
    {
        public const int MAXSPEED = 140;
        public string Name { get; set; } = "";
        public int CurrentSpeed { get; set; }

        private bool _carIsDead;

        public Car_v2(string name, int currentSpeed)
        {
            Name = name;
            CurrentSpeed = currentSpeed;
        }
        public Car_v2()
        {
        }

        public void Accelerate(int delta)
        {

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
                    throw new CarIsDead_v2_Exception("Speed too high.", tempCurrentSpeed, $"{Name} has overheated!");

                }
                Console.WriteLine($"Current speed {Name}:{CurrentSpeed}");
            }
        }
    }

    public class CarIsDead_v2_Exception : ApplicationException
    {
        public string? CauseOfError { get; }
        public int Speed { get; }
        public CarIsDead_v2_Exception(string? cause, int speed, string message) :base(message)
        {
            CauseOfError = cause;
            Speed = speed;
        }
    }
```
```cs
void ExplorationCarIsDead_v2_Exception()
{
    Car_v2 car = new("Nissan Leaf", 35);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(20);
        }
    }
    catch (CarIsDead_v2_Exception e)
    {
        Console.WriteLine($"\nMessage:\t{e.Message}");
        Console.WriteLine($"Cause:\t{e.CauseOfError}");
        Console.WriteLine($"Speed:\t{e.Speed}");
    }
}
ExplorationCarIsDead_v2_Exception();
```
```
Current speed Nissan Leaf:55
Current speed Nissan Leaf:75
Current speed Nissan Leaf:95
Current speed Nissan Leaf:115
Current speed Nissan Leaf:135

Message:        Nissan Leaf has overheated!
Cause:  Speed too high.
Speed:  155
```
Мабудь не завжди треба робити додадкове прикрашання аби залишити ланцюжок успадкування. В цьму прикладі використовується батьківський конструктор і цей клас можна далі взяти за базовий. При первищені скорості може зламатися або двигун або ходова або ще щось. 
В данному випадку сама назва класу є частиною відповіді що не так, а реалізація як у базовому класі без перевизначень. В багатьох випадках такий шаблон достатній, адже тип винятку чітко визначає природу. Клієнт може дати різну логіку при обробці різних типів винятків.

## Як краше робити клас винятку.

Аби провільно побудувати власний клас винтку треба додримуватися таких правил:
- За базовий брати клас Exception/ApplicationException.
- Визначити конструктор за замовчуванням.
- Визначити конструктор який встановлює успадковану властивість Message.
- Визначити конструктор для innerException

Аби створити всі конструктори бистріше CTRL+. > Generete All

```cs
    class Car_v3    
    {
        public const int MAXSPEED = 140;
        public string Name { get; set; } = "";
        public int CurrentSpeed { get; set; }

        private bool _carIsDead;

        public Car_v3(string name, int currentSpeed)
        {
            Name = name;
            CurrentSpeed = currentSpeed;
        }
        public Car_v3()
        {
        }

        public void Accelerate(int delta)
        {

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
                    throw new CarIsDead_v3_Exception("Speed too high.", tempCurrentSpeed, $"{Name} has overheated!");

                }
                Console.WriteLine($"Current speed {Name}:{CurrentSpeed}");
            }
        }
    }

    public class CarIsDead_v3_Exception : ApplicationException
    {
        public CarIsDead_v3_Exception()
        {
        }

        public CarIsDead_v3_Exception(string? message) : base(message)
        {
        }

        public CarIsDead_v3_Exception(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CarIsDead_v3_Exception(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CarIsDead_v3_Exception(string? causeOfError, int speed, string message) :base(message) 
        {
            CauseOfError = causeOfError;
            Speed = speed;
        }

        public CarIsDead_v3_Exception( string? causeOfError, int speed, string? message, Exception? innerException) : base(message, innerException)
        {
            CauseOfError = causeOfError;
            Speed = speed;
        }

        public string? CauseOfError { get; }
        public int Speed { get; }
    }
```
```cs
void ExplorationCarIsDead_v3_Exception()
{
    Car_v3 car = new("Nissan Leaf", 35);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(20);
        }

    }
    catch (CarIsDead_v3_Exception e)
    {
        Console.WriteLine($"\nMessage:\t{e.Message}");
        Console.WriteLine($"Cause:\t{e.CauseOfError}");
        Console.WriteLine($"Speed:\t{e.Speed}");
    }
}
ExplorationCarIsDead_v3_Exception();
```
```
Current speed Nissan Leaf:55
Current speed Nissan Leaf:75
Current speed Nissan Leaf:95
Current speed Nissan Leaf:115
Current speed Nissan Leaf:135

Message:        Nissan Leaf has overheated!
Cause:  Speed too high.
Speed:  155
```
Таким чином буде повністю збережений ланцюг успадкування.

## Коли власні винятки зайві. 

В .Net є багато визначених класів які можна використовувати для викиду винятків.
Коли ви первіряєте параметри функцій можна використати вбудовані класи.

```cs
void UsingBuilInExceptions()
{
    // account = null
    try
    {
        DoSomething(null, 12);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }

    // sum < 0
    try
    {
        DoSomething("3234 2345", -10);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }

    void DoSomething(string? account, decimal sum)
    {
        if (account is null)
        {
            throw new ArgumentNullException(paramName: nameof(account)); // Here use built in exception 
        }
        // or   ArgumentException.ThrowIfNullOrEmpty(account); // Here use built in exception
        if (sum < 0)
        {
            throw new ArgumentException(message: "The sum must be greater than zero."); // Here use built in exception
        }
    }
}
UsingBuilInExceptions();
```
```
Value cannot be null. (Parameter 'account')
The sum must be greater than zero.
```
Перш ніж створювати власний виняток спробуйте подивиться чи немає вбудованового. 
