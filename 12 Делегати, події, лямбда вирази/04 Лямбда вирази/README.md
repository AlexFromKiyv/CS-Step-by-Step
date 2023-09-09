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



