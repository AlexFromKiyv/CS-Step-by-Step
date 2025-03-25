# Анонімні методи

Як ви бачили, коли викликаючий код хоче прослухати вхідні події, він повинен визначити спеціальний метод у класі (або структурі), який відповідає підпису пов’язаного делегату. Ось приклад:

```cs
SomeType t = new();
// Assume 'SomeDelegate' can point to methods taking no
// args and returning void.
t.SomeEvent += new SomeDelegate(MyEventHandler);

// Typically only called by the SomeDelegate object.
static void MyEventHandler()
{
  // Do something when event is fired.
}
```
Однак якщо ви подумаєте про це, такі методи, як MyEventHandler(), рідко призначені для виклику будь-якою частиною програми, окрім делегату, що викликає. Що стосується продуктивності, то вручну визначати окремий метод, який буде викликатися об’єктом-делегатом, є дещо клопітним (хоча ні в якому разі не заважає).
Щоб вирішити цю проблему, можна зв’язати подію безпосередньо з блоком операторів коду під час реєстрації події. Формально такий код називається анонімним методом. Щоб проілюструвати синтаксис, спочатку створіть нову консольну програму під назвою AnonymousMethods. Додайте класи.

```cs

namespace AnonymousMethods;

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
namespace AnonymousMethods;

public class Car
{
    public int CurrentSpeed { get; set; }
    public int MaxSpeed { get; set; }
    public string PetName { get; set; } = string.Empty;

    private bool _carIsDead;

    public Car() { MaxSpeed = 100; }
    public Car(string name, int maxSp, int currSp)
    {
        CurrentSpeed = currSp;
        MaxSpeed = maxSp;
        PetName = name;
    }

    public event EventHandler<CarEventArgs>? Exploded;
    public event EventHandler<CarEventArgs>? AboutToBlow;

    public void Accelerate(int delta)
    {
        // If the car is dead, fire Exploded event.
        if (_carIsDead)
        {
            Exploded?.Invoke(this, new CarEventArgs("Sorry, this car is dead..."));
        }
        else
        {
            CurrentSpeed += delta;

            // Check speed
            if (CurrentSpeed >= MaxSpeed)
            {
                _carIsDead = true;
            }
            else
            {
                Console.WriteLine($"CurrentSpeed = {CurrentSpeed}");
            }

            //Almost dead
            if ((MaxSpeed - CurrentSpeed) <= 10)
            {
                AboutToBlow?.Invoke(this, new CarEventArgs("Careful buddy! Gonna blow!"));
            }
        }
    }
}
```
Оновіть код файлу Program.cs відповідно до наведеного нижче, який обробляє події, надіслані з класу Car, використовуючи анонімні методи, а не конкретно названі обробники подій:

```cs

using AnonymousMethods;

static void UsingAnonimousMethods()
{
    Car car = new("SlugBug",100,10);

    car.AboutToBlow += delegate
    {
        Console.WriteLine("Eek! Going too fast!");
    };

    car.AboutToBlow += delegate(object sender, CarEventArgs e)
    {
        Console.WriteLine($"Message from Car{e.message}");
    }!;

    car.Exploded += delegate(object sender, CarEventArgs e)
    {
        Console.WriteLine(e.message.ToUpper());
    }!;

    Console.WriteLine("***** Speeding up *****");
    for (int i = 0; i < 6; i++)
    {
        car.Accelerate(20);
    }
}
UsingAnonimousMethods();
```
```
***** Speeding up *****
CurrentSpeed = 30
CurrentSpeed = 50
CurrentSpeed = 70
CurrentSpeed = 90
Eek! Going too fast!
Message from CarCareful buddy! Gonna blow!
Eek! Going too fast!
Message from CarCareful buddy! Gonna blow!
SORRY, THIS CAR IS DEAD...
```

Кінцева фігурна дужка анонімного методу повинна закінчуватися крапкою з комою.
Знову ж таки, зауважте, що код виклику більше не потребує визначення конкретних статичних обробників подій, таких як CarAboutToBlow() або CarExploded().
Швидше, безіменні (або анонімні) методи визначаються вбудовано в той час, коли абонент обробляє подію за допомогою синтаксису +=. Базовий синтаксис анонімного методу відповідає такому псевдокоду:

```cs
SomeType t = new SomeType();
t.SomeEvent += delegate (optionallySpecifiedDelegateArgs)
{ /* statements */ };
```
Під час обробки першої події AboutToBlow у попередньому зразку коду зауважте, що ви не вказуєте аргументи, передані делегатом. Строго кажучи, вам не потрібно отримувати вхідні аргументи, надіслані певною подією. Однак, якщо ви хочете використовувати можливі вхідні аргументи, вам потрібно буде вказати параметри, прототиповані типом делегату (як показано у другій обробці подій AboutToBlow і Exploded). Ось приклад:

```cs
    car.Exploded += delegate(object sender, CarEventArgs e)
    {
        Console.WriteLine(e.message.ToUpper());
    }!;
```

## Доступ до локальних змінних

Анонімні методи цікаві тим, що вони можуть отримати доступ до локальних змінних методу, який їх визначає. Формально кажучи, такі змінні називаються зовнішніми змінними анонімного методу. Необхідно вказати на наступні важливі моменти щодо взаємодії між анонімною областю методу та областю визначення методу:

1. Анонімний метод не може отримати доступ до параметрів ref або out методу визначення.
2. Анонімний метод не може мати локальну змінну з тим самим іменем, що й локальна змінна у зовнішньому методі.
3. Анонімний метод може отримати доступ до змінних екземпляра (або статичних змінних, у відповідних випадках) у зовнішній області класу.
4. Анонімний метод може оголошувати локальні змінні з тим самим іменем, що й зовнішні змінні-члени класу (локальні змінні мають окрему область і приховують зовнішні змінні-члени класу).

Припустімо, що ваші оператори верхнього рівня визначають локальне ціле число з назвою aboutToBlowCounter. У анонімних методах, які обробляють подію AboutToBlow, ви збільшите цей лічильник на одиницю та роздрукуєте підрахунок до завершення операторів.

```cs
static void AccessingLocalVariables()
{
    int aboutToBlowCounter = 0;

    Car car = new("SlugBug", 100, 10);

    car.AboutToBlow += delegate
    {
        aboutToBlowCounter++;
        Console.WriteLine("Eek! Going too fast!");
    };

    car.AboutToBlow += delegate (object sender, CarEventArgs e)
    {
        aboutToBlowCounter++;
        Console.WriteLine($"Message from Car{e.message}");
    }!;

    car.Exploded += delegate (object sender, CarEventArgs e)
    {
        Console.WriteLine(e.message.ToUpper());
    }!;

    Console.WriteLine("***** Speeding up *****");
    for (int i = 0; i < 6; i++)
    {
        car.Accelerate(20);
    }

    Console.WriteLine($"AboutToBlow event was fired {aboutToBlowCounter} times.");
}
AccessingLocalVariables();
```
```
***** Speeding up *****
CurrentSpeed = 30
CurrentSpeed = 50
CurrentSpeed = 70
CurrentSpeed = 90
Eek! Going too fast!
Message from CarCareful buddy! Gonna blow!
Eek! Going too fast!
Message from CarCareful buddy! Gonna blow!
SORRY, THIS CAR IS DEAD...
AboutToBlow event was fired 4 times.
```
Після запуску цього оновленого коду ви побачите остаточний звіт Console.WriteLine() про те, що подія AboutToBlow була викликана двічі. Зрозуміло що можна виправити ситуацію залишивши інкремент тільки в одному обробнику.

## Використання static з анонімними методами

Попередній приклад продемонстрував анонімні методи, які взаємодіють зі змінними, оголошеними поза межами самого методу. Хоча це може бути те, що потрібно, це порушує інкапсуляцію та може викликати небажані побічні ефекти у вашій програмі. Хоча це може бути те, що ви збираєтеся, це порушує інкапсуляцію та може викликати небажані побічні ефекти у вашій програмі. Згадайте, що локальні функції можна ізолювати від коду, що містить, встановивши їх як статичні, як у наступному прикладі:

```cs
static int AddWrapperWithStatic(int x, int y)
{
    //Do some validation here
    return Add(x, y);

    static int Add(int x, int y)
    {
        return x + y;
    }
}
Console.WriteLine(AddWrapperWithStatic(1,1));
```
```
2
```
Анонімні методи також можна позначити як статичні, щоб зберегти інкапсуляцію та гарантувати, що метод не може внести жодних побічних ефектів у код в якому він містиця. Наприклад, перегляньте оновлений анонімний метод тут:

```cs

    car.AboutToBlow += static delegate
    {
        //This causes a compile error because it is marked static
        aboutToBlowCounter++;
        Console.WriteLine("Eek! Going too fast!");
    };
```
Попередній код не буде скомпільовано через анонімні методи, які намагаються отримати доступ до змінної, оголошеної поза її областю. Щоб видалити помилку компіляції, закоментуйте (або видаліть) рядок, який оновлює лічильник.

## Відкидання за допомогою анонімних методів

Ви можете використовувати відкидання як вхідні параметри для анонімних методів.

```cs
Func<int, int> constant = delegate (int _) { return 42; };
Console.WriteLine(constant(4));
```
```
42
```
