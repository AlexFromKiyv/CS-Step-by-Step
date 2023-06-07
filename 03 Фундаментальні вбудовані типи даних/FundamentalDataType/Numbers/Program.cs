using System.Numerics;
//ExplorationOfNumbers_1();
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

//ExplorationOfNumbers_2();
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



//ExplorationOfNumbers_3();
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

//UsingVar();
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

//IncremetAndAssign();
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

//BinaryOperations();
void BinaryOperations()
{
    int a = 11;
    int b = 3;

    Console.WriteLine($"{a} + {b}={a + b}");
    Console.WriteLine($"{a} - {b}={a - b}");
    Console.WriteLine($"{a} * {b}={a * b}");
    Console.WriteLine($"{a} / {b}={a / b}");
    Console.WriteLine($"{a} % {b}={a % b}");

    double c = 11.0;
    Console.WriteLine($"{c} / {b}={c/b}");

}

//AssignmentOperators();
void AssignmentOperators()
{
    int a = 2;
    Console.WriteLine(a);
    
    a += 2;
    Console.WriteLine(a);

    a -= 2;
    Console.WriteLine(a);

    a *= 2;
    Console.WriteLine(a);

    a /= 2;
    Console.WriteLine(a);
}


//UsingParse();
static void UsingParse()
{
    int myInt = int.Parse("100");
    double myDouble = double.Parse("100,23");
    decimal myDecimal = decimal.Parse("1001000000100,293");

    Console.WriteLine($"{myInt} {myInt.GetType()}");
    Console.WriteLine($"{myDouble} {myDouble.GetType()}");
    Console.WriteLine($"{myDecimal} {myDecimal.GetType()}");
}

//UsingTryParse();
static void UsingTryParse()
{
    string myString = "83 kg";
    bool resultParsing = int.TryParse(myString, out int myInt1);

    Console.WriteLine($"Was parsing \"{myString}\" well?:{resultParsing} {myInt1}");


    myString = "83";
    resultParsing = int.TryParse(myString, out int myInt2);

    Console.WriteLine($"Was parsing \"{myString}\" well?:{resultParsing} {myInt2}");
}


//UsingBigInteger();

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


//ImplicitlyCastShortToInt();
static void ImplicitlyCastShortToInt()
{
    Console.WriteLine(short.MaxValue);
    Console.WriteLine(int.MaxValue);
    Console.WriteLine();

    short myShort = 100, myOtherShort;

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

//ExplicitlyCastIntToShort();
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

//UsingChacked();

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

//RangeOfWhole();

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


//RangeOfDoubleAndDecimal();
void RangeOfDoubleAndDecimal()
{
    Console.WriteLine($"double   {double.MinValue}   {double.MaxValue} byte:{sizeof(double)}");
    Console.WriteLine($"decimal   {decimal.MinValue}   {decimal.MaxValue} byte:{sizeof(decimal)}");
}



//DoubleOrDecimal();
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

//RoundingRuleDefault();

void RoundingRuleDefault()
{
    Console.WriteLine(Math.Round(3.5));
    Console.WriteLine(Math.Round(4.5));
}

RoundingRuleAwayFromZero();

void RoundingRuleAwayFromZero()
{
    Console.WriteLine(Math.Round(3.5,0,MidpointRounding.AwayFromZero));
    Console.WriteLine(Math.Round(4.5,0, MidpointRounding.AwayFromZero));
}