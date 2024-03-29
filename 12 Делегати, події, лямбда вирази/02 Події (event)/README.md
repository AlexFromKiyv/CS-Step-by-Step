# 02 Події (event)

Делегати є цікавими конструкціями, оскільки дозволяють об'єктам в пам`яті вести двусторонню взаємодію. Однак робота безпосередньо з делегатими може приветси до створення шаблонного коду(визначення делегата, визначення похідної, створення регеструючих та знімаючих з регістрації методів для забеспечення інкапсуляції то що). Більше того, коли ви використовуєте власний делегат як механізм зворотнього виклику, якшо ви не визначите зміні члени цього типу як приватні, зовнішній код може матиме прямий доступ до об'єктів делегату. Тоді він може перепризначити зміну новому об'єкту делегата(фактично видаливши поточний список), і, що ще гірше, зовнішній код може безпосередьно викликати список викликів делегата.

Розглянемо клас
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

        //Define delegate type
        public delegate void CarEngineHandler(string messageForCaller);

        //Now variable for delegate is public !!! 
        public CarEngineHandler ListOfHandlers;


        public void Accelerate(int delta)
        {
            if (_isDead)
            {
                ListOfHandlers?.Invoke("Sorry, this car is dead!");
            }
            else
            {
                CurrentSpeed += delta;


                if (CurrentSpeed > MaxSpeed)
                {
                    _isDead = true;
                    ListOfHandlers?.Invoke("Car dead!");
                }
                else
                {
                    Console.WriteLine($"Current speed {Name}: {CurrentSpeed}");
                }

                if ((MaxSpeed - CurrentSpeed) < 10 && !_isDead)
                {
                    ListOfHandlers?.Invoke($"Careful buddy! Gonna blow! Current speed:{CurrentSpeed}");
                }
            }
        }
    }
```
Змінна делегату як член класу  визначена як public. Тоб то немає інкапсуляції за допомогою спеціальних методів регістрації. 

```cs
void PublicDelegateMemeber()
{
    Car car = new Car("VW Transporter",160,130);

    car.ListOfHandlers = KindHandler;
    car.Accelerate(8);
    car.Accelerate(8);
    car.Accelerate(8);
    car.Accelerate(8);

    Console.WriteLine("\nThe end of a work the kind hundler.\n");

    car = new Car("VW Transporter", 160, 130);
    car.ListOfHandlers = KindHandler;
    car.Accelerate(8);

    car.ListOfHandlers = EvilHandler;
    car.Accelerate(8);
    car.Accelerate(8);
    car.Accelerate(8);
    car.ListOfHandlers.Invoke("");
    car.ListOfHandlers.Invoke("");




    static void KindHandler(string message)
    {
        Console.WriteLine($"Kind handler says:{message}");
    }

    static void EvilHandler(string _)
    {
        Console.WriteLine("Evil handler says:I crecked you!");
    }

}

PublicDelegateMemeber();
```
```
Current speed VW Transporter: 138
Current speed VW Transporter: 146
Current speed VW Transporter: 154
Kind handler says:Careful buddy! Gonna blow! Current speed:154
Kind handler says:Car dead!

The end of a work the kind hundler.

Current speed VW Transporter: 138
Current speed VW Transporter: 146
Current speed VW Transporter: 154
Evil handler says:I crecked you!
Evil handler says:I crecked you!
Evil handler says:I crecked you!
```
Оскільки член класу загальнодоступний, зовнішній код може отримати прямий доступ і перепризначити метод виконання та викликати делегат колизавгодно.
Викриття публічних членів порушує інкапсуляцію, шо може викликати не тільки проблему підтримки коду а також збільшує ризик зламу коду. Очевидно, ви не хочете зовнішньому коду давати можливість маніпулювати змінною делегата. Враховуючи це загальноприйнятою практикою є встановленя ці змінни приватними.

## event

Ключеве слово event дозволяє не створювати змінну член та методи для додавання або видалення в список виклику делегата. Коли компілятор виконує процес з event вам автоматично пердостаняються методи регістрації та зняття з регістрації, а також необхідні змінні-члени класу ваших типів делегатів. Ці змінні-члени завжди декларуються як приватні, і, отже, не піддаються безпосередньому впливу об'єкта шо викликає подію. Ключеве слово event можна використовувати для спрощення того, як спеціальний клас надсилає сповіщеня зовнішнім об'єктам.
Визначеня події відбувається в два єтапи:
    1. Визначаеться тип делегата або використовується існуючий чи вбудований. Методи такого типу будуть зберігатись при виклику.
    2. За допомогою ключового слова event оголошується подія у термінах відповідного типу делегата.

CarEvents\Car.cs
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

        public void ToConsole()
        {
            Console.WriteLine($"\tCar:{Name} Max speed:{MaxSpeed}");
            Console.WriteLine($"\tCurrent speed:{CurrentSpeed}");
        }

        //Define delegate type
        public delegate void CarEngineHandler(string? messageForCaller);

        // This car can send these events
        public event CarEngineHandler AboutToBlow;
        public event CarEngineHandler Exploded;

        //This is a method of changing the current speed
        public void Accelerate(int delta)
        {
            if (_isDead)
            {
                Exploded?.Invoke("Sorry, this car is dead!");
            }
            else
            {
                CurrentSpeed += delta;

                if (CurrentSpeed > MaxSpeed)
                {
                    _isDead = true;
                    CurrentSpeed = 0;
                    Exploded?.Invoke("Car dead!");
                }
                else
                {
                    Console.WriteLine($"\tCurrent speed {Name}: {CurrentSpeed}");
                }

                if ((MaxSpeed - CurrentSpeed) < 10 && !_isDead)
                {
                    AboutToBlow?.Invoke($"Careful buddy! Gonna blow! Current speed:{CurrentSpeed}");
                }
            }
        }
    }
```
В цьому варіанті класу Car визначен делегат та дві події з двигуном. Ці події ассосціюються з одним і тим типом делегата CarEngineHandler. Відправити подію викликаючому коду просто вказавши ім'я події та передавши параметри які визначені пов'язаним делегатом. Визиваючий код повинен зареєструвати як мінімум один метод який буде включений в список визову. Щоб не виникло винятку, об'єкт подію перевіряеться на null перед самим викликом Invoke.
Завдяки такому визначенню подій в класі не потрібно визначати метод регестрації та визначати змінну делегата.
Як видно події потрідні шоб не таратити час на шаблоний код використаня делегатів. 
Коли клас має події їх може використовувати зовнішій код.

```cs
void useEvents()
{
    Car car = new Car("BMW i3",150,125);
    car.ToConsole();

    ConsoleKey consoleKey = ConsoleKey.Home;
    while (consoleKey != ConsoleKey.End)
    {
        consoleKey = Console.ReadKey().Key;
 
        switch (consoleKey)
        {
            case ConsoleKey.UpArrow:
                car.Accelerate(3);
                break;
            case ConsoleKey.DownArrow:
                car.Accelerate(-3);
                break;
            case ConsoleKey.Insert:
                car.AboutToBlow += PrintCriticalMessage;
                Console.WriteLine("Handler for event AboutToBlow added.");
                car.Exploded += Console.WriteLine;
                Console.WriteLine("Handler for event Exploded added.");
                break;
            default:
                car.ToConsole();
                break;
        }
    }

    void PrintCriticalMessage(string? message)
    {
        Console.WriteLine($"Critical message from engine: {message}");
    }
}
useEvents();
```
```
        Car:BMW i3 Max speed:150
        Current speed:125
        Current speed BMW i3: 128
        Current speed BMW i3: 131
        Current speed BMW i3: 134
Handler for event AboutToBlow added.
Handler for event Exploded added.
        Current speed BMW i3: 137
        Current speed BMW i3: 140
        Current speed BMW i3: 143
Critical message from engine: Careful buddy! Gonna blow! Current speed:143
        Current speed BMW i3: 146
Critical message from engine: Careful buddy! Gonna blow! Current speed:146
        Current speed BMW i3: 149
Critical message from engine: Careful buddy! Gonna blow! Current speed:149
Car dead!
Sorry, this car is dead!
Sorry, this car is dead!
        Car:BMW i3 Max speed:150
        Current speed:0

```
В подіях спрощена регестрація методів-обробників визиваючого кода. Замість використання допоміжних методів регистарція використовуються оператори += , -= . 
Як приклад: 
```cs
Car.CarEngineHandler handler = new Car.CarEngineHandler(CarExplodedEventHandler);
myCar.Exploded += handler;
myCar.Exploded -= handler;
myCar.Exploded += handler;
``` 
Також можна використовувати синтаксис "method group conversion"

```cs
Car.CarEngineHandler handler = CarExplodedEventHandler;
myCar.Exploded += handler;
```
Все це аби меньше витрачати часу на шаблоні речі. Зверніть увагу шо делегат в цьому прикладі має сігнатуру для якою підходить метод Console.WriteLine, тому його можна використати в якості обробника.

## Спрошення регістрації обробника події за допомогою Visual Stidio.

Visual Studio може полегшити процес створеня обробника події.

Наприклад у нас є метод в якому не встановлени обробники подій.

```cs
void SimplifyingCodingHandler()
{
    Car car = new("Nissan Leaf", 150, 100);

    car.Accelerate(5);
    car.Accelerate(15);
    car.Accelerate(20);
    car.Accelerate(25);
}


SimplifyingCodingHandler();
```
Коли ви пробуєте реєструвати подію car.Exploded += Visual Studio пропонує вам додати код відповідно до делегату нажавши TAB. Після чого додається назва мтода і генерується метод "заглушка".

```cs
void SimplifyingCodingHandler()
{
    Car car = new("Nissan Leaf", 150, 100);

    car.Exploded += Car_Exploded;

    car.Accelerate(5);
    car.Accelerate(15);
    car.Accelerate(20);
    car.Accelerate(25);
}

void Car_Exploded(string? messageForCaller)
{
    throw new NotImplementedException();
}

SimplifyingCodingHandler();
```
Зверніть увагу код-заглушка має правільний формат який відповідає визначенню делегата. 
IntelliSense доступний для всіх подій .Net, ваших спеціальних подій та подій базової бібіліотеки класів. Ця функйія дозволяє єкономити час. Це позбавляє потребит шукати дані про делегат та формат цільового методу делегата.

## Шаблон подій з використанням EventArgs. 

Використаня подій в базових бібліотеках має свій шаблон. Це рекомендований шаблон для подій від Microsoft. Бібіліотеки базових класів використовують події в яких базовий делегат має перший параметр System.Object, а другий параметр нашадок System.EventArgs.
Аргумент System.Object пердставляє посилання на об'єкт, який надіслав подію(наприклад екземпляр Car). Другий параметр представляє інформацію про поточну подію. Базовий клас представляє подію, яка не надсилає жодної спеціальної додадкової інформації.
```cs
    //
    // Summary:
    //     Represents the base class for classes that contain event data, and provides a
    //     value to use for events that do not include event data.
    public class EventArgs
    {
        //
        // Summary:
        //     Provides a value to use with events that do not have event data.
        public static readonly EventArgs Empty;

        //
        // Summary:
        //     Initializes a new instance of the System.EventArgs class.
        public EventArgs();
    }
``` 
Для простих подій можна відправити безпосередньо екземпляр класу EventArgs, тобто без додадкової інформації про подію. Якшо ви хочете передати спеціальні дані, слід створити підходящий клас що походить від EventArgs

UseEventArgs\CarEventArgs.cs
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
Маючи цей клас можна оновити клас Car який визначає делегат.

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

        //Define delegate type
        public delegate void CarEngineHandler(object sender, CarEventArgs e);


        // This car can send these events
        public event CarEngineHandler AboutToBlow;
        public event CarEngineHandler Exploded;

        //This is a method of changing the current speed
        public void Accelerate(int delta)
        {
            if (_isDead)
            {
                Exploded?.Invoke(this,new CarEventArgs("Sorry, this car is dead!"));
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
В класі оновлено визначення делегата та метод який використовує події. Передається посилання на поточний об'єкт за допомогою this та посилання на екземпляр класу CarEventArgs з відповідним значенням для конструктора.
При таких змінах визиваючий код теж повитен мати методи які відповідають делегату.
```cs
void UseEventPattern()
{
    Car car = new("Volkswagen Käfer", 105, 83);
    car.AboutToBlow += Car_AboutToBlow;
    car.Exploded += Car_Exploded;

    car.Accelerate(5);
    car.Accelerate(5);
    car.Accelerate(5);
    car.Accelerate(5);
    car.Accelerate(5);
    car.Accelerate(5);
}

void Car_Exploded(object sender, CarEventArgs e)
{
    if (sender is Car car)
    {
        Console.WriteLine($"{car.Name} : {e.message}");
    }
}
void Car_AboutToBlow(object sender, CarEventArgs e)
{
    if (sender is Car car)
    {
        Console.WriteLine($"Critical message from {car.Name} : {e.message}"  );
    }
}

UseEventPattern();
```
```
        Current speed Volkswagen Kafer: 88
        Current speed Volkswagen Kafer: 93
        Current speed Volkswagen Kafer: 98
Critical message from Volkswagen Kafer : Careful buddy! Gonna blow! Current speed:98
        Current speed Volkswagen Kafer: 103
Critical message from Volkswagen Kafer : Careful buddy! Gonna blow! Current speed:103
Volkswagen Kafer : Car dead!
Volkswagen Kafer : Sorry, this car is dead!
```
Зовнішній код отримує посилання на відправника та об'єкт події. Цього достатньо шоб отримпати повну інформацію про подію.

## Узагальнений делегат EventHandler<T>.

В поппередньому прикладі визначення делегато мало чіткий шаблон перший параметр System.Object, другий екземпляр типу похідного від EventArgs.
Оскільки визначення делегата шаблонне то немає сенсу вигадувати і постійно визначати нові делегати типу:
```cs
        public delegate void CarEngineHandler(object sender, CarEventArgs e);
```
Можна використовувати загальний вбудований делегат:

```cs
public delegate void EventHandler<TEventArgs>(object? sender, TEventArgs e);
```
В нашому випадку TEventArgs це CarEventArgs. Тому в класі Car можна зробити такі зміни

```cs
    internal class Car
    {

        ...

        // This car can send these events
        public event EventHandler<CarEventArgs> AboutToBlow;
        public event EventHandler<CarEventArgs> Exploded;

        ...

    }
```
При цьому не треба визначати делегата. В методах визиваючого коду тілки треба вказати перший параметр як object?.

```cs
void Car_Exploded(object? sender, CarEventArgs e)
{
    if (sender is Car car)
    {
        Console.WriteLine($"{car.Name} : {e.message}");
    }
}
void Car_AboutToBlow(object? sender, CarEventArgs e)
{
    if (sender is Car car)
    {
        Console.WriteLine($"Critical message from {car.Name} : {e.message}");
    }
}
```
