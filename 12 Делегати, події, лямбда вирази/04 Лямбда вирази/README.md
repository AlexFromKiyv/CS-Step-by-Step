# Лямбда вирази

За допомогою анонімних методів події можна обробляти "inline" (в одному рядку), без неодхідності створювати окремий метод. Ці методи відповідають делегату і попадають в список на виконання при необхідності використаня події.
Лямбда вирази це способ лаконічного та стислого створеня анонімного методу і тим самим ще більше спрощення роботи з подіями.

Перед розлядом лямда виразів розглянемо делегат який будемо використовувати з простору System.

```cs
    //
    // Summary:
    //     Represents the method that defines a set of criteria and determines whether the
    //     specified object meets those criteria.
    //
    // Parameters:
    //   obj:
    //     The object to compare against the criteria defined within the method represented
    //     by this delegate.
    //
    // Type parameters:
    //   T:
    //     The type of the object to compare.
    //
    // Returns:
    //     true if obj meets the criteria defined within the method represented by this
    //     delegate; otherwise, false.
    public delegate bool Predicate<in T>(T obj);
```
Як видно з опису цей делегат представляє метод який визначає набір крітеріїв, і визначає чи вказаний об'єкт відповідає їм.

Цей делегат використовується в методі FindAll() загального класу List<T>.
```cs
public List<T> FindAll(Predicate<T> match)
{
    ...
}
``` 
Метод дозволяє отримати підмножину сипску яка задоволняє крітерію. Цей метод повертає новий список List<T> який представляє цю підмножину. Параметром є загальний делегат. Делегат може вказувати на будь-який метод, який отримує екземпляр об'єкту і повертає bool.  
Коли визивається метод FindAll, кожний елемент списку передається методу на який вказує об'єкт Predicate<T>. В залаежності від того шо повертає метод визначається чи додавати єлемент в новий сиписок.

Розглянемо використаня Predicate<T> без лямбда-виразу. 

```cs
void ExampleWithoutLambdaExpression()
{
    List<int> ints = new List<int>();
    ints.AddRange(new int[] {1,12,3,2,4,29,48,4,5});

    Predicate<int> predicate = IsEvenNumber;

    List<int> evenNumbers = ints.FindAll(predicate);

    foreach (var item in evenNumbers)
    {
        Console.Write($"{item}\t");
    }

    static bool IsEvenNumber(int x)
    {
        return (x%2) == 0;
    }
}

ExampleWithoutLambdaExpression();
```
```
12      2       4       48      4
```
В прикладі визначено окремий метод який використовується для внесеня в сиписок виконання делегата. Він необїідний тільки для функції FindAll.

Можна використати анонімний метод.

```cs
void ExampleWithAnonymousMethod()
{
    List<int> ints = new List<int>();
    ints.AddRange(new int[] { 1, 12, 3, 2, 4, 29, 48, 4, 5 });

    Predicate<int> predicate = delegate(int x) { return x % 2 == 0; } ;

    List<int> evenNumbers = ints.FindAll(predicate);

    foreach (var item in evenNumbers)
    {
        Console.Write($"{item}\t");
    }

}

ExampleWithAnonymousMethod();
```
```
12      2       4       48      4
```
Використавни анонімний метод ми значно очишуємо код. Код можна ще скоротити.

```cs
void ExampleWithAnonymousMethod2()
{
    List<int> ints = new List<int>();
    ints.AddRange(new int[] { 1, 12, 3, 2, 4, 29, 48, 4, 5 });

    List<int> evenNumbers = ints.FindAll(delegate (int x) { return x % 2 == 0; });

    foreach (var item in evenNumbers)
    {
        Console.Write($"{item}\t");
    }

}

ExampleWithAnonymousMethod2();
```
Таким чином не створюється окремий метод і не створюється окремий об'єкт Predicate<T>.

Лямбда-вирази дозволяють ше більше спростири визначення методу який потребує делегат.

```cs
void UseLambdaExpression()
{
    List<int> ints = new List<int>();
    ints.AddRange(new int[] { 1, 12, 3, 2, 4, 29, 48, 4, 5 });

    List<int> evenNumbers = ints.FindAll( x => x % 2 == 0 );

    foreach (var item in evenNumbers)
    {
        Console.Write($"{item}\t");
    }
}

UseLambdaExpression();
```
```
12      2       4       48      4
```
У цьому випадку використовіється лямбда-вираз.
```cs
x => x % 2 == 0 
```
Лямбда вираз можна використовувати всюди де б ви використовували б анонімний метод або строго типізований делегат. У цьому випадку компілятор перетовре лямбда вираз, використовуючи визначення Predicate<T>, в анаонімний метод:
```cs
delegate (int x) { return x % 2 == 0; }
```

## Визначення лямбда виразів.

Лямбда-вираз можна зрозуміти наступним чином.

```cs
ArgumentsToProcess => StatementsToProcessThem

```
Спочатку іде список параметрів, далі символ =>, а потім набор операторів яки обробляють аргументи.  

В виразі 
```cs
x => x % 2 == 0 
``` 
х це список параметрів, x % 2 == 0  оператори обробки.

Параметри виразу можуть бути неявно або явно типізовані. Якшо контекст дозволяє компілятор смостійно визначає тип параметрів. В попередньму випадку оскільки список List<int> тому і Predicate<int> i параметр x int. В такому випадку ми можемо визначити праметри неявно. Але ми можемо вказати це явно
```cs
(int x) => x % 2 == 0 
```
Коли є один неявно типізований параметр дужки можна опустити в списку параметрів.
```cs
x => x % 2 == 0
```
Якшо ви хочете біти послідовними що до використання лямбда-параметрів ви завжди можете обготрути параметри в дужки.
```cs
(x) => ( x % 2 ) == 0

```
Можна зробити вираз ще зрозумілішим при розборі.
```cs
(x) => (( x % 2 ) == 0)
```
В решті решт ви, а можливо  ще хтось подивиться в код і швідкість розуміння, що відбувається, залежить як ви це оформили.

## Обробка аргументів

Попередній приклад лямбда-виразу визначав один оператор, який повертав Boolean. Але метод на який вказує делегат може бути складнішим і виконувати декілька інструкцій. З цієї причини дозволено створювати вирази з кількома операторами які облогнутиі як блок коду.
```cs
void LambdaWithMultipleStataments()
{
    List<int> ints = new List<int>();
    ints.AddRange(new int[] { 1, 12, 3, 2, 4, 29, 48, 4, 5 });

    List<int> evenNumbers = ints.FindAll((x) =>
    {
        bool isEvent = ((x % 2) == 0);
        Console.WriteLine( $"Is {x} ever :{isEvent}");
        return isEvent;
    });

    foreach (var item in evenNumbers)
    {
        Console.Write($"{item}\t");
    }
}

LambdaWithMultipleStataments();
``` 
```
Is 1 ever :False
Is 12 ever :True
Is 3 ever :False
Is 2 ever :True
Is 4 ever :True
Is 29 ever :False
Is 48 ever :True
Is 4 ever :True
Is 5 ever :False
12      2       4       48      4
```
Зверніть шо дужки додають читабільносі коду. 

## Лябда-вирази з різною кількістю параметрів.

Лямбда-вирази можуть обробляти декілька параметрів або жодного.

Наприклад ми маємо клас.

```cs
    internal class SimpleMath
    {
        public delegate void MathMessage(string message, int result);
        private MathMessage? _mmDelegate;
        public void SetMathMessageHandler(MathMessage target)
        {
            _mmDelegate = target;
        }

        public void Add(int x, int y)
        {
            _mmDelegate?.Invoke("Adding has complited!", x + y);
        }
    }
```
Зверніть увагу що ціловий метод делегата очікує два параметрами.В такому разі лямбда-вираз може виглядадти так.
```cs
void LambdaWithMiltipeParameters()
{
    SimpleMath simpleMath = new();

    simpleMath.SetMathMessageHandler((string message, int result) =>
    {
        Console.WriteLine($"Message:{message}");
        Console.WriteLine($"Result:{result}");
    });

    simpleMath.Add(1, 2);
}

LambdaWithMiltipeParameters();
```
```
Message:Adding has complited!
Result:3
```
В цьому випадку можна спростити вираз оскільки компілятор сам може визначити типи параметрів.
```cs
void MoreSimpleLambdaWithMiltipeParameters()
{
    SimpleMath simpleMath = new();

    simpleMath.SetMathMessageHandler( (message, result) => 
    Console.WriteLine($"Message:{message}\nResult:{result}")
    );

    simpleMath.Add(1, 2);
}

MoreSimpleLambdaWithMiltipeParameters();
```
```
Message:Adding has complited!
Result:3
```
В більшості випадків компілятор здатний виявити тип необхідних паратетрів, тому неявна типізація досить компактний і зручний спосіб побудови виразу.

Лямбда-вираз може бути без параметрів
```cs
    internal class Call
    {
        public delegate string VerySimpleDelegate();
        private VerySimpleDelegate? _vsDelegate;
        public void SetVSDelegate(VerySimpleDelegate vsDelegate)
        {
            _vsDelegate = vsDelegate;
        }

        public void SaySomething()
        {
            Console.WriteLine(_vsDelegate?.Invoke());
        }
    }
```
```cs
void LambdaWithoutParameters()
{
    Call call = new();

    call.SetVSDelegate(() => "Hi girl!");

    call.SaySomething();
}

LambdaWithoutParameters();
```
```
Hi girl!
```
Коли ви взаємодієте з делегатом який зовсім не приймає параметрів тоді лямбда вираз починається з () => . 

## static лямбда-вирази.

Так само як і для методів для лямбда-виразів доступні зовнішні змінні.
```cs
void LambdaAndOuterVariables()
{
    int outerVariable = 0;

    Func<int, int, int> sum = (x, y) =>
    {
        outerVariable++;
        return x + y;
    };

    sum(1, 2);
    sum(1, 2);

    Console.WriteLine(outerVariable);
}

LambdaAndOuterVariables();
```
```
2
```
Перед лямбда-виразом можна використати ключове слово static.
```cs
void StaticLambda()
{
    int outerVariable = 0;

    Func<int, int, int> sum = static (x, y) =>
    {
        // A compilation error occurs here.
        // Cannot a reference to ...
        //outerVariable++;
        return x + y;
    };

    sum(1, 2);
    sum(1, 2);

    Console.WriteLine(outerVariable);
}
```
При визначені лямбда-виразу як статичний зовнішні змінні стають недоступними.

## Відкидання параметрів для лямбда-виразів.

```cs
void DiscardsWithLambda()
{
    int counter = 0;
    Func<int, bool> tik = (_) =>
    {
        counter++;
        return true;
    };

    Console.WriteLine(tik(1));
    Console.WriteLine(tik(2));
    Console.WriteLine(counter);
}

DiscardsWithLambda();
```
```
True
True
2
```
Якшо вхідні параметри не мають значеня їх можна відкинути в лямбда-виразі.

## Лямбда-вирази для EventArgs.

В базових бібліотеках класів широко використовується шаблон подій з використанням класу EventArgs. Створимо лямбда-вирази для таких випадків.

Нехай ми маємо наступні класи.

LambdaAndEventArgs\

```cs
    class CarEventArgs : EventArgs
    {
        public readonly string message;
        public CarEventArgs(string message)
        {
            this.message = message;
        }
    }
```
```cs
    class Car
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

        public override string? ToString() => Name;

        // This car can send these events
        public event EventHandler<CarEventArgs> AboutToBlow;
        public event EventHandler<CarEventArgs> Exploded;

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
В класі Car має дві події які мають тип EventHandler<CarEventArgs>, сігнатура якого наступна.
```cs
public delegate void EventHandler<CarEventArgs>(object? sender, CarEventArgs e);
```
Без використання лямбда-виразу використати класи можна було наступним чином.

```cs
void UseEventWithoutLambda()
{
    Car car = new("Volkswagen Käfer", 105, 83);
    
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

    for (int i = 0; i < 5; i++)
    {
        car.Accelerate(5);
    }

}

UseEventWithoutLambda();
```
Використаємо лямбда-вирази.

```cs
void UseEventWithLambda()
{
    Car car = new("Volkswagen Käfer", 105, 83);

    car.AboutToBlow += (sender, e) => 
    Console.WriteLine($"Critical message from engine {sender} : {e.message}");
    
    car.Exploded += (sender, e) =>
    Console.WriteLine($"Fatal message from engine {sender} : {e.message}");

    for (int i = 0; i < 5; i++)
    {
        car.Accelerate(5);
    }

}

UseEventWithLambda();
```
Вся причина використаня лямб-да виразів в чикому і стислому способі визначення анонімних методів. Таким чином спорщується робота з делегатами.

## Лямбда-вирази як вираз з тілом в якості члена класу.

Лямбда-вирази можна використовувати для спрощення визначеня членів класу.
Наприклад маємо клас.
```cs
    static class SimpleMath
    {
        public static int Multiplying(int x, int y)
        {
            return x * y;
        }
        public static void PrintMultiplying(int x, int y)
        {
            Console.WriteLine(x * y);
        }
    }
```
Використовуючи лямбда вирази його можна визначити так.
```cs
    static class MoreSimpleMath
    {
        public static int Multiplying(int x, int y) =>  x * y;
        
        public static void PrintMultiplying(int x, int y) => Console.WriteLine( x * y );
    }
```

Якшо є метод або властивіст, які складається з одного або кількох дій, немає необхідності визначати окремий блок з return, а можна використати => . Це називають "expression-bodied member." (вираз з тілом). Цей синтаксис можна використовувати для конструкторів, фіналізаторів та для акссерорів та властивостей.
Також такий синтаксис можна використовувати будь-де і він не прив'язаний до делегатів.

```cs
void UseLambda()
{
    PrintSquared(5);

    void PrintSquared(int x) => MoreSimpleMath.PrintMultiplying(x,x); 
}

UseLambda();
```
```
25
```
Можна використати лямбда для визначення властивості.
```cs
class Car
{
    public string? Name { get; set; }
    
    private int _maxSpeed;
    public int MaxSpeed => _maxSpeed;
    public Car()
    {
    }
    public Car(string? name, int maxSpeed)
    {
        Name = name;
        _maxSpeed = maxSpeed;
    }
}
```
```cs
void UseLambdaAsAccessor()
{
    Car car = new("VW Golf", 160);

    Console.WriteLine(car.MaxSpeed);
}

UseLambdaAsAccessor();
```
```
160
```
При необхідности лямбда-вираз завжди можна розкласти в метод і навпаки.

```cs
ArgumentsToProcess => StatementsToProcessThem

//or
ArgumentsToProcess => 
{
    StatementsToProcessThem
}

//to
MethodName(ArgumentsToProcess)
{
 StatementsToProcessThem
}
```

Лямбда-вирази значною мірою використвуються в LINQ щоб спростити кодування.

