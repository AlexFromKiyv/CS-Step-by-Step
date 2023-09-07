# Анонімні методи

Коли ми хочемо використати події ми виконуємо декілька визначень нових типів.
Створюємо клас похідний від EventArgs.
```cs
    internal class SomethingEventArgs : EventArgs
    {
        public readonly string message;
        public SomethingEventArgs(string message)
        {
            this.message = message;
        }
    }
``` 
В класі визначаємо події та коли вони відбуваються.

```cs
        //State 
        public string? Name { get; set; }
        public int Speed { get; set; }
        public void ToConsole() => Console.WriteLine($"\tName: {Name}\tSpeed: {Speed}");

        public Something(string name, int speed)
        {
            Name = name;
            Speed = speed;
        }

        public Something()
        {
        }

        //Events
        public event EventHandler<SomethingEventArgs>? SpeedUp;
        public event EventHandler<SomethingEventArgs>? SpeedDown;
    
        public void ChangeSpeed(int speed)
        {
            Speed += speed;

            if (speed > 0)
            {
                SpeedUp?.Invoke(this, new SomethingEventArgs("Speed increased"));
            }
            else if(speed < 0) 
            {
                SpeedDown?.Invoke(this, new SomethingEventArgs("Speed dropped"));
            }
        }
    }
```
В коді який використовує клас визначаємо методи які прослуховують події.

```cs
void SimpleUseEvents()
{
    Something something = new("Bird", 0);
    something.ToConsole();

    something.SpeedUp += new EventHandler<SomethingEventArgs>(Something_ChangeSpeed);
    something.SpeedDown += Something_ChangeSpeed;

    ConsoleKey consoleKey = ConsoleKey.Home;
    while (consoleKey != ConsoleKey.End)
    {
        consoleKey = Console.ReadKey().Key;

        switch (consoleKey)
        {
            case ConsoleKey.UpArrow:
                something.ChangeSpeed(1);
                something.ToConsole();
                break;
            case ConsoleKey.DownArrow:
                something.ChangeSpeed(-1);
                something.ToConsole();
                break;
            default:
                something.ToConsole();
                break;
        }
    }

    void Something_ChangeSpeed(object? sender, SomethingEventArgs e)
    {
        if (sender is Something something)
        {
            Console.WriteLine($"Event on {something.Name} : {e.message}");
        }
    }
}

SimpleUseEvents();
```
Коли код, що визиває об'єкт, хоче прослуховувати вхідні події він повинен визначити спеціальні методи яки відповідають сігнатурі делегата. Ці методи рідко де використовуються в іншій частині програми окрім як делегату шо викиликає його.
Крім того трохи накладно визначати окремий метод для конкретного одного випадку.
Є можливість зв'язати подію безросередньо з блоком коду під час регістрації обробника події. Формально це називається анонімні методи. 
Нехай ми наступні класи.
```cs
    public class CarEventArgs : EventArgs
    {
        public readonly string message;
        public CarEventArgs(string message)
        {
            this.message = message;
        }
    }
```
```cs
    internal class Car
    {
        // State data
        public string Name { get; set; } = string.Empty;
        public int MaxSpeed { get; set; } = 100;
        public int CurrentSpeed { get; set; }

        private bool _isDead;

        //Constructors
        public Car(string name, int maxSpeed, int currentSpeed)
        {
            Name = name;
            MaxSpeed = maxSpeed;
            CurrentSpeed = currentSpeed;
        }
        public Car()
        {
        }

        // This car can send these events
        public event EventHandler<CarEventArgs>? AboutToBlow;
        public event EventHandler<CarEventArgs>? Exploded;

        //This is a method of changing the current speed
        public void Accelerate(int delta)
        {
            if (_isDead)
            {
                Exploded?.Invoke(this, new CarEventArgs("Sorry, this car is dead!"));
            }
            else
            {
                CurrentSpeed += delta;

                if (CurrentSpeed > MaxSpeed)
                {
                    _isDead = true;
                    CurrentSpeed = 0;
                    Exploded?.Invoke(this, new CarEventArgs("Car dead!"));
                }
                else
                {
                    Console.WriteLine($"\tCurrent speed {Name}: {CurrentSpeed}");
                }

                if ((MaxSpeed - CurrentSpeed) < 10 && !_isDead)
                {
                    AboutToBlow?.Invoke(this, new CarEventArgs($"Careful buddy! Gonna blow! Current speed:{CurrentSpeed}"));
                }
            }
        }
    }
```
Використаємо ці класи.
```cs
void UseAnonymousMethods()
{
    Car car = new("VW Golf", 160, 143);

    car.AboutToBlow += delegate
    {
        Console.WriteLine("Hey! Going too fast!");
    };

    car.AboutToBlow += delegate (object? sender, CarEventArgs e)
    {
        if (sender is Car c)
        {
            Console.WriteLine($"Critical message from engine {c.Name} : {e.message}");
        }

    };

    car.Exploded += delegate (object? sender, CarEventArgs e)
    {
        if (sender is Car c)
        {
            Console.WriteLine($"Fatal message from engine {c.Name} : {e.message}");
        }

    };

    for (int i = 0; i < 8; i++)
    {
        car.Accelerate(3);
    }
}

UseAnonymousMethods();
```
```
        Current speed VW Golf: 146
        Current speed VW Golf: 149
        Current speed VW Golf: 152
Hey! Going too fast!
Critical message from engine VW Golf : Careful buddy! Gonna blow! Current speed:152
        Current speed VW Golf: 155
Hey! Going too fast!
Critical message from engine VW Golf : Careful buddy! Gonna blow! Current speed:155
        Current speed VW Golf: 158
Hey! Going too fast!
Critical message from engine VW Golf : Careful buddy! Gonna blow! Current speed:158
Fatal message from engine VW Golf : Car dead!
Fatal message from engine VW Golf : Sorry, this car is dead!
Fatal message from engine VW Golf : Sorry, this car is dead!
```
Події надіслані Car обрабляються не окремо визначеними методами а анонімними методами. Взерніть увагу шо визнечення делегата закінчуеться ; . 
Таким чином не турбуючись про створення і назву окремого обробника ви вказуєте обробку тут і зараз за допомогою += . Оскільки ці методи обробки не мають ім'я вони називаються анонімні. Схему використання анонімних методів можна приставити в псевдокоді.
```cs
 SomeType obj = new();
obj.SomeEvent += delegate (optionallySpecifiedDelegateArgs)
{ /* statements */ };
```
Зверніть увагу шо для першого обробника події не вказано параметрів методу та не вкзано () . Строго кажучи не обов'язково отримувати аргументи відправлені певною подією. Однак, якшо ви хочите використати вхідні аргументи щоб визнати деталі події ви повині вказати параметри протиповані типом делегата.

## Доступ до локальних змінних.

Анонімні методи мають доступ до локальних зміних в методі в якому вони визначені. 

```cs
void AccessLocalVariables()
{
    int aboutToBlowCounter = 0;

    Car car = new("VW e-up", 130, 110);

    car.AboutToBlow += delegate
    {
        aboutToBlowCounter++;
        Console.WriteLine("Hey! Going too fast!");
    };

    for (int i = 0; i < 8; i++)
    {
        car.Accelerate(3);
    }

    Console.WriteLine(aboutToBlowCounter);
}

AccessLocalVariables();
```
```
        Current speed VW e-up: 113
        Current speed VW e-up: 116
        Current speed VW e-up: 119
        Current speed VW e-up: 122
Hey! Going too fast!
        Current speed VW e-up: 125
Hey! Going too fast!
        Current speed VW e-up: 128
Hey! Going too fast!
3
```
Зміні шо знаходяцьа поза анонімного метода називають зовнішніми. Тут є деяки обмеженя:

- Анонімні методи не мають доступу до ref та out параметрів методу в якому вони визначені.
- Не можуть мати зміну з назвою яка є в методі в якому вони визначені.
- Анонімний метод може отримати доступ до змінних екземпляра (або статичних змінних, у відповідних випадках) у зовнішній області класу.
- Анонімний метод може оголошувати локальні змінні з тим самим іменем, що й зовнішні змінні-члени класу (локальні змінні мають окрему область і приховують зовнішні змінні-члени класу).

## static анонімні методи.

В попередьному прикладі метод використовував зовнішню змінну. Але це пагана практика оскільки порушується інкапсуляція і може викликати небажані побічні ефекти в програмі.
Локалні функції можна ізолювати від коду зробивши їх статичними 

```cs

void IsolationLocalFunction()
{

    Console.WriteLine(AddWrapperWithStatic(1,2));

    int AddWrapperWithStatic(int x, int y)
    {
        //Do some validation here
        return Add(x, y);
   
        static int Add(int x, int y)
        {
            return x + y;
        }
    }
}

IsolationLocalFunction();
```
Цю можливість, ізоляції коду ,можна використати для анонімних методів вказавши їх як статік. Тоді зовнішні змінні стануть недоступні.
```cs
void StaticAnonymousMethods()
{
    int aboutToBlowCounter = 0;

    Car car = new("VW e-up", 130, 110);

    // Now it is static
    car.AboutToBlow += static delegate
    {
        //aboutToBlowCounter++; //A static anonymous function cannot contain а reference  
        Console.WriteLine("Hey! Going too fast!");
    };

    for (int i = 0; i < 8; i++)
    {
        car.Accelerate(3);
    }
}
```
При визначені методу як static компілятор покаже недоступність змінної.

## Відкідання параметрів в анонімних методах.

В методі можна ігнорувати вхідні аргуметни.

```cs
void DiscardsMethodParameters()
{
    Console.WriteLine(ReturnResult(10));
    Console.WriteLine(ReturnResult(20));

    string ReturnResult(int _)
    {
        return "Hi";
    }
}
DiscardsMethodParameters();
```
```
Hi
Hi
```
Це саме можна використати для анонімних методів.
```cs
void DiscardInAnonymousMethod()
{
    Func<int, string> sayHi = delegate (int _) { return "Hi"; };
    
    Console.WriteLine(sayHi(3));
}

DiscardInAnonymousMethod();
```
```
Hi
```

