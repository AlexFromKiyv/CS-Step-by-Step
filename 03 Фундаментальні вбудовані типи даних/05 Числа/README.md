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

## Створення

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

## Діапазони типів
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
Треба зазначити що для типів  int, double для відповідних літералів не треба суфіксів. Для long потрібен L, для float - F, для decimal - M

В просторі імен System.Numerics; існуе типи для наукових розрахунків BigInteger який не обмежений.
```cs

UsingBigInteger();

static void UsingBigInteger()
{
    BigInteger myBigInt_1;
    myBigInt_1= BigInteger.Parse("1111111111111111111111111111111111111111111111111111111");
    Console.WriteLine(myBigInt_1);

    BigInteger myBigInt_2;
    myBigInt_2 = BigInteger.Parse("2222222222222222222222222222222222222222222222222222222");
    Console.WriteLine(myBigInt_2);

    Console.WriteLine(myBigInt_1*myBigInt_2);

    Console.WriteLine($"Is ValueType?:{myBigInt_1 is ValueType}");
}

``` 
## Використання var

```cs
UsingVar();
static void UsingVar()
{
    var myInt = 100;
    var myLong = 100L;
    var myDouble = 100.00;
    var myFloat = 100.00F;
    var myDecimal = 100.00M;

    Console.WriteLine($"{myInt} : {myInt.GetType()}");
    Console.WriteLine($"{myLong} : {myLong.GetType()}");
    Console.WriteLine($"{myDouble} : {myDouble.GetType()}");
    Console.WriteLine($"{myFloat} : {myFloat.GetType()}");
    Console.WriteLine($"{myDecimal} : {myDecimal.GetType()}");
}
```
## Операціїї

## Особливості унарної операції ++
```
IncremetAndAssign();
void IncremetAndAssign()
{
    int x;
    int y;

    x = 1;
    y = x++;
    Console.WriteLine($"x = 1;");
    Console.WriteLine($"y = x++;");
    Console.WriteLine($"x:{x}  y:{y}");
    Console.WriteLine();
    Console.WriteLine("Good practice:");
    x = 1;
    x++;
    y = x;
    Console.WriteLine($"x = 1;");
    Console.WriteLine("x++;");
    Console.WriteLine($"y = x;");
    Console.WriteLine($"x:{x}  y:{y}");
}
```

```
x = 1;
y = x++;
x:2  y:1

Good practice:
x = 1;
x++;
y = x;
x:2  y:2
```
## Бінарні операції.
```cs
BinaryOperations();
void BinaryOperations()
{
    int a = 11;
    int b = 3;

    Console.WriteLine($"{a} + {b}={a + b}");
    Console.WriteLine($"{a} - {b}={a - b}");
    Console.WriteLine($"{a} * {b}={a * b}");
    Console.WriteLine($"{a} / {b}={a / b}");
    Console.WriteLine($"{a} % {b}={a % b}");
}
```

```
11 + 3=14
11 - 3=8
11 * 3=33
11 / 3=3
11 % 3=2
```

## Метод Parse, TryParse

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

Але рядок може не відповідає типу. Для того щоб в консоль не викинувся виняток існуе метод TryParse

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

## Перетворення типів. 

```cs
ImplicitlyCastShortToInt();

static void ImplicitlyCastShortToInt()
{
    Console.WriteLine(short.MaxValue);
    Console.WriteLine(int.MaxValue);

    short myShort = 100, myOtherShort;
    Console.WriteLine();

    int myInt = myShort;
    Console.WriteLine(myInt);

    myInt = Square(myShort);

    Console.WriteLine(myInt);

    // Cannot implicitly convert
    // myShort = myInt;
    // myOtherShort = Square(1);

    static int Square(int a)
    {
        return a*a;
    }
}
```
Оскілкі любе значення short без проблем може бути доповнене до int в процесі виконання відбуваеться неявне претвореня short в int. Прицьому дані претворюються безопасно без втрати. Можна сказати short підмножина int. Також при передачі методу якій очікує int змінної типа short також відбувается неявне претворення і помилки не виникає. Кажуть змінна myShort розширюється до int. В звороньому напрямку претворення неявно не выдбудеться ы компылятор сповыстить про це. Хоча 1 можно зберігти в типі short тип int неавно не перетворюеться(не звужуеться). Розмір int виходить за межи (overflow) short. Компілятор не пропускає всі неявні звужуючи перетворення. 

## Переповнення

Компілятору можно вказати явно про звуження типу але при цьому можна втратити данні.

```cs
ExplicitlyCastIntToShort();
static void ExplicitlyCastIntToShort()
{
    Console.WriteLine(short.MaxValue);
    Console.WriteLine();

    short myShort;
    int myInt = 10_000;

    myShort = (short)myInt;
    Console.WriteLine($"{myShort} = {myInt}");

    myInt = 32_770;
    myShort = (short)myInt;
    Console.WriteLine($"{myShort} = {myInt}");
}
```
Код компилюється виконуеться але данні стають некоректними. Коли скорочується місце де зберігаються дані частина данних відсікається. При цьому нема сповіщень про якісь помилки. Якшо примусово компілятору вказуеться операцію приведеня треба додадково потурбуватися що вона пройде без втрати данних.

Оскільки при операції приведеня процесс може пройти як успішно так і з помилкою надаеться можливість контролювати цей процес як на рівні додадку так і на рівні частини коду.

```cs

UsingChacked();

static void UsingChacked()
{
    int myInt = 10_000;
    short myShort;
    try
    {
        checked
        {
            myShort = (short)myInt; // or checked((short)myInt)
        }
        Console.WriteLine($"{myShort} = {myInt}");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    myInt = 32_770;
    try
    {
        myShort = checked((short)myInt);
        Console.WriteLine($"{myShort} = {myInt}");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
```

При використані checked перевіряеться чи не було переповнення і видаеться сповіщеня шо було переповнення.  

## Перевірка переповнення 

Якшо в проекті багато мість для перевірки можна ваказати перевірку всього проекту.
1. Правий клік на проекті
2. Properties
3. В рядку пошуку введіть overflow
4. Поставити флаг Check for arithmeric overflow

Цe добавляе в файл проекту

```xml
  <PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Debug|AnyCPU'">
    <CheckForOverflowUnderflow>True</CheckForOverflowUnderflow>
  </PropertyGroup>

  <PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Release|AnyCPU'">
    <CheckForOverflowUnderflow>True</CheckForOverflowUnderflow>
  </PropertyGroup>
```
Включивши перевірку проекту ви первіряете весь код. Деяки нагружени ділянки можна послабити за допомогою ключового слова unchecked тим самим підвищити продуктивність.

## Як вибрати потрібний тип.

Зазвичай для цілих чисел вибирають int, але можна вибрати враховуючи діапазон.
```cs
RangeOfWhole();

void RangeOfWhole()
{

    Console.WriteLine($"sbyte  {sbyte.MinValue}  {sbyte.MaxValue} byte:{sizeof(sbyte)}");
    Console.WriteLine($"byte   {byte.MinValue}   {byte.MaxValue} byte:{sizeof(byte)}");
    Console.WriteLine($"short  {short.MinValue}  {short.MaxValue} byte:{sizeof(short)} ");
    Console.WriteLine($"ushort {ushort.MinValue} {ushort.MaxValue} byte:{sizeof(ushort)}");
    Console.WriteLine($"int    {int.MinValue}    {int.MaxValue} byte:{sizeof(int)}");
    Console.WriteLine($"uint   {uint.MinValue}   {uint.MaxValue} byte:{sizeof(uint)}");
    Console.WriteLine($"long   {long.MinValue}   {long.MaxValue} byte:{sizeof(long)}");
    Console.WriteLine($"ulong   {ulong.MinValue}   {ulong.MaxValue} byte:{sizeof(ulong)}");
}

```
```
sbyte  -128  127 byte:1
byte   0   255 byte:1
short  -32768  32767 byte:2
ushort 0 65535 byte:2
int    -2147483648    2147483647 byte:4
uint   0   4294967295 byte:4
long   -9223372036854775808   9223372036854775807 byte:8
ulong   0   18446744073709551615 byte:8
```
Дивлячись на діапазон зрозуміло чому кілкість мість в вагоні не обов'язково зберігати в типі int.

При зберіганні реальних чисел вони перетворюються в бінарну систему обчисленяя і тому 
в випадку з float або double не повністю точно співпадають с аналогом в 10 системі счислення. Тому треба звертати увагу як буде використовуватися змінна і наскільки важлива точність розрахунків.

Розглянемо особливості типу double i decimal.

Діапазони типів різні.
```cs
RangeOfDoubleAndDecimal();
void RangeOfDoubleAndDecimal()
{
    Console.WriteLine($"double   {double.MinValue}   {double.MaxValue} byte:{sizeof(double)}");
    Console.WriteLine($"decimal   {decimal.MinValue}   {decimal.MaxValue} byte:{sizeof(decimal)}");
}
```
```
double   -1,7976931348623157E+308   1,7976931348623157E+308 byte:8
decimal   -79228162514264337593543950335   79228162514264337593543950335 byte:16
```
Хоча double займає меньше місця і має більший діапазон але в деяких випадках не відповідає вимогам точності.
```cs
DoubleOrDecimal();
void DoubleOrDecimal()
{
    double doubleA = 0.3;
    double doubleB = 0.2;
    Console.WriteLine($"DoubleA:{doubleA} DoubleB:{doubleB}");
    Console.WriteLine($"DoubleA - DoubleB = 0.1? :{(doubleA - doubleB) == 0.1 }");
    Console.WriteLine($"DoubleA - DoubleB :{doubleA-doubleB}");
    Console.WriteLine($"DoubleA - DoubleB -0.1 :{doubleA - doubleB - 0.1}");

    Console.WriteLine();

    decimal decimalA = 0.3M;
    decimal decimalB = 0.2M;
    Console.WriteLine($"DecimalA:{decimalA} DecimalB:{decimalB} ");
    Console.WriteLine($"DecimalA - DecimalB = 0.1 ?:{(decimalA - decimalB) == 0.1M}");
    Console.WriteLine($"DecimalA - DecimalB:{decimalA - decimalB}");
}
```
```
DoubleA:0,3 DoubleB:0,2
DoubleA - DoubleB = 0.1? :False
DoubleA - DoubleB :0,09999999999999998
DoubleA - DoubleB -0.1 :-2,7755575615628914E-17

DecimalA:0,3 DecimalB:0,2
DecimalA - DecimalB = 0.1 ?:True
DecimalA - DecimalB:0,1
```
Як видно з прикладу розахунки з використанням double не дають точного співпадіння.
Таким чином тип double можна використовувати коли з зміною можна використовувати порівняння на > ,< але точне співпадіння не є критерієм принятя рішення. Наприклад вимірювання ваги людини і порівнювання з витривалістю велосипеда не потребують точного співпадіння. Тобто double не гаратує роботу == оскілки результат обчислень може відрізнятися на дуже маленьке значення. В інших випадках він може зберегти память.

Використовуйте decimal для обліку грошей, інжинерних обчислень і всюду де потребується точність обчислень.
