# Числа

System.Int32, System.Double, System.Decimal и т.д системні класи для роботи з числами.
Розглянемо як їx можна викорустовувати для роботи з числами. Додамо проект Numbers з методом.
```cs
static void ExplorationOfNumbers_1()
{
    var number1 = 100;
    Console.WriteLine($"{number1.GetType()} : {number1}");

    var number2 = 100000000000000;
    Console.WriteLine($"{number2.GetType()} : {number2}");

    var number3 = 100.1;
    Console.WriteLine($"{number3.GetType()} : {number3}");

    var number4 = 0.00000000000001;
    Console.WriteLine($"{number4.GetType()} : {number4}");

    var number5 = 7/3;
    Console.WriteLine($"{number5.GetType()} : {number5}");

    double number6 = 7 / 3;
    Console.WriteLine($"{number6.GetType()} : {number6}");

    double number7 = 7 / (double) 3;
    Console.WriteLine($"{number7.GetType()} : {number7}");

    var number8 = 7 / 3.0;
    Console.WriteLine($"{number8.GetType()} : {number8}");
}
```
Ви бачите типи які були використані під час виконання для відповідних літералів. Але можливо стиль за допомогою якого визначені ці змвні не дасть користі ні вам ні тому хто буде розбиратись з вашим кодом. При можливості замість ключового слова var краше вказувати конкретний тип.

```cs
ExplorationOfNumbers_2();
static void ExplorationOfNumbers_2()
{
    int number1 = 100;
    Console.WriteLine($"{number1.GetType()} : {number1}");

    long number2 = 100000000000000;
    Console.WriteLine($"{number2.GetType()} : {number2}");

    double number3 = 100.1;
    Console.WriteLine($"{number3.GetType()} : {number3}");

    double number4 = 0.00000000000001;
    Console.WriteLine($"{number4.GetType()} : {number4}");

    double a = 7;
    double b = 3;

    double number5 = a / b; 
    Console.WriteLine($"{number5.GetType()} : {number5}");
}
```
Кожен числовий тип має свої межи. Подивимось які.

```cs
ExplorationOfNumbers_3();
static void ExplorationOfNumbers_3()
{
    Console.WriteLine("int --------------");
    int i = 1_000;
    Console.WriteLine(i);
    Console.WriteLine(i.GetType());
    Console.WriteLine("Min: " + int.MinValue);
    Console.WriteLine("Max: " + int.MaxValue);

    Console.WriteLine("long --------------");
    long l = 100_000_000_000L;
    Console.WriteLine(l);
    Console.WriteLine(l.GetType());
    Console.WriteLine("Min: " + long.MinValue);
    Console.WriteLine("Max: " + long.MaxValue);

    Console.WriteLine("float -------------");
    float f = 100_000.12345F;
    Console.WriteLine(f);
    Console.WriteLine(f.GetType());
    Console.WriteLine("Min: " + float.MinValue);
    Console.WriteLine("Max: " + float.MaxValue);

    Console.WriteLine("double -------------");
    double d = 100_000.12345;
    Console.WriteLine(d);
    Console.WriteLine(d.GetType());
    Console.WriteLine("Min: " + double.MinValue);
    Console.WriteLine("Max: " + double.MaxValue);
    Console.WriteLine(double.PositiveInfinity);
    Console.WriteLine(double.NegativeInfinity);


    Console.WriteLine("decimal -------------");
    decimal m = 100_000.12345M;
    Console.WriteLine(m);
    Console.WriteLine(m.GetType());
    Console.WriteLine("Min: " + decimal.MinValue);
    Console.WriteLine("Max: " + decimal.MaxValue);

}
```
Треба зазначити що для типів <em> int, double </em> для відповідних літералів не треба суфіксів. Для <em>long</em> потрібен L, для <em>float</em> - F, для <em>decimal</em> - M

# Метод Parse, TryParse

З строки можно отримати змінну типу.

```cs
UsingParse();
static void UsingParse()
{
    int myInt = int.Parse("100");
    double myDouble = double.Parse("100,23");
    decimal myDecimal = decimal.Parse("1001000000100,293");

    Console.WriteLine($"{myInt} {myInt.GetType()}");
    Console.WriteLine($"{myDouble} {myDouble.GetType()}");
    Console.WriteLine($"{myDecimal} {myDecimal.GetType()}");
}
```

Але рядок може не відповідає типу. У такому разі існуе метод TryParse

```cs
UsingTryParse();
static void UsingTryParse()
{
    string myString = "83 kg";

    bool resultParsing = int.TryParse(myString, out int myInt1);

    Console.WriteLine($"Was parsing \"{myString}\" well?:{resultParsing} {myInt1}");


    myString = "83";

    resultParsing = int.TryParse(myString, out int myInt2);

    Console.WriteLine($"Was parsing \"{myString}\" well?:{resultParsing} {myInt2}");
}
```







