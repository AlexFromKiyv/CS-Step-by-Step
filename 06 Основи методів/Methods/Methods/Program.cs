//SimpleMethod();
using System.Reflection.Metadata.Ecma335;

static void SimpleMethod()
{
    double height = 176;

    double result = MaxGoodWeight(height);

    Console.WriteLine(result);


    static double MaxGoodWeight(double height)
    {
        return (height / 100) * (height / 100) * 24.9;
    }
}


//SimpleMethodWithLambda();

static void SimpleMethodWithLambda()
{

    Console.WriteLine(MaxGoodWeight(176));

    static double MaxGoodWeight(double height) => (height / 100) * (height / 100) * 24.9;
}

//SimpleMethodWithValidation();
static void SimpleMethodWithValidation()
{
    for (int height = 164; height < 192; height+=2)
    {
        Console.WriteLine(MaxGoodWeightWithValidation(height));
    }
   
    //Console.WriteLine(MaxGoodWeight(320)); it do not work

    static string MaxGoodWeightWithValidation(double height)
    {

        if (height > 130 && height < 280 )
        {
            return $"Max good weight for {height} cm is {MaxGoodWeight(height)} "; 
        }
        else
        {
            return $"Bad input height {height} ";
        }

        // Local function
        static double MaxGoodWeight(double height)
        {
            return (height / 100) * (height / 100) * 24.9;
        }  
        
    }
}

//BadNoStaticLocalFunction();

static void BadNoStaticLocalFunction()
{
    PrintRectangle(1);

    static void PrintRectangle(double length)
    {
        Console.WriteLine(Rectangle());

        double Rectangle()
        {
            length += 1;
            return length * length;
        }
    }
}

//StaticLocalFunction();
static void StaticLocalFunction()
{
    PrintRectangle(1);

    static void PrintRectangle(double length)
    {
        Console.WriteLine(Rectangle(length));

        static double Rectangle(double l) => l * l; 
  
    }
}

//ValueTypeWithoutModifier();

static void ValueTypeWithoutModifier()
{
    int length = 2;

    Console.WriteLine(Rectangle(length));

    static int Rectangle(int l)
    {
        Console.WriteLine(l is ValueType);
       
        int result = l * l;
        
        return result;
    }
}

//UsingOutModifier_1();
static void UsingOutModifier_1()
{
    int enterlength = 10;

    Rectangle(enterlength, out int rectangle);

    static void Rectangle(int length, out int result)
    {
        result = length * length;
    }


    Console.WriteLine($"{enterlength} * {enterlength} = {rectangle}");


    int newRectangle;

    newRectangle = 10;

    Rectangle(enterlength, out newRectangle);

    Console.WriteLine(newRectangle);


}

//UsingOutModifier_2();
static void UsingOutModifier_2()
{
    int enterlength = 10;

    static void RectangleAndVolume(int length,out bool isPositive , out int rectangle, out int volume)
    {
        isPositive = length > 0;
        rectangle = length * length;
        volume = length * length * length;
    }

    RectangleAndVolume(enterlength, out bool isPositive, out int rectangle, out int volume);

    Console.WriteLine($"{enterlength} isPositive:{isPositive} rectangle:{rectangle}, volume:{volume}");

    RectangleAndVolume(5, out _, out _, out int newVolume);

    Console.WriteLine(newVolume);

}

//UsingRefModifier();

static void UsingRefModifier()
{
    int x = 5;
    int y = 8;

    Console.WriteLine($"Before:  x:{x} y:{y}");

    SwapInt(ref x,ref y);

    Console.WriteLine($"After:   x:{x} y:{y}");

    SwapInt(ref x, ref y);

    Console.WriteLine($"After:   x:{x} y:{y}");

    static void SwapInt(ref int a, ref int b)
    {
        if (a < b)
        {   int t = b;
            b = a;
            a = t;
        }
    }

    string str1 = "Bye";
    string str2 = "Hi";

    Console.WriteLine("Before: " + str1 + " " + str2);

    SwapStr(ref str1, ref str2);
    
    Console.WriteLine("After: " + str1 + " " + str2);

    static void SwapStr(ref string a, ref string b)
    {
        string stringTemp = b;
        b = a;
        a = stringTemp;
    }
}



//UsingInModifier();

static void UsingInModifier()
{
    string greeting = "Welcome to paradise!";

    Console.WriteLine(greeting is ValueType); //False

    Console.WriteLine($"Before:{greeting}");
    ChangeWay(greeting);
    Console.WriteLine($"After:{greeting}");

    static void ChangeWay(string greetingString)
    {
        greetingString = "Welcom to hell!";
    }

    Console.WriteLine($"Before:{greeting}");
    ChangeWayWithIn(greeting);
    Console.WriteLine($"After:{greeting}");


    static void ChangeWayWithIn(in string greetingStreeng)
    {
        //greetingStreeng = "Welcom to paradise!"; //it don't work
        // using greetingString 
        Console.WriteLine(greetingStreeng.Length);
    }
}

//UsingParamsModifier();

static void UsingParamsModifier()
{
    Console.WriteLine(GetSum());

    Console.WriteLine(GetSum(1));

    Console.WriteLine(GetSum(1,2,3,4));

    double d = 7.34; 

    Console.WriteLine(GetSum(1.2,3.3,4.5,d));

    double[] myDoubleArray = new double[] {4,5,6.7};

    Console.WriteLine(GetSum(myDoubleArray));


    static double GetSum(params double[] values)
    {
        double sum = 0;

        if (values.Length > 0)
        {
            for (int i = 0; i < values.Length; i++)
            {
                sum += values[i];
            }
        }
        return sum;
    }
}

UsingOptionalPatameters();
static void UsingOptionalPatameters()
{
    Console.WriteLine(GetStringTemperature(20));
    Console.WriteLine(GetStringTemperature(68,"F"));

    static string GetStringTemperature(double temperature,string scale = "C")
    {
        return temperature.ToString() + "°" + scale;
    }
}