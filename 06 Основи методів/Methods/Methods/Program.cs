//SimpleMethod();
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Threading.Tasks.Dataflow;

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
    PrintQuadrate(1);

    static void PrintQuadrate(double length)
    {
        Console.WriteLine(Quadrate());

        double Quadrate()
        {
            length += 1;
            return length * length;
        }
    }
}

//StaticLocalFunction();
static void StaticLocalFunction()
{
    PrintQuadrate(1);

    static void PrintQuadrate(double length)
    {
        Console.WriteLine(Quadrate(length));

        static double Quadrate(double l) => l * l; 
  
    }
}

//ValueTypeWithoutModifier();

static void ValueTypeWithoutModifier()
{
    int length = 2;

    Console.WriteLine(Quadrate(length));

    static int Quadrate(int l)
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

    Quadrate(enterlength, out int quadrate);

    static void Quadrate(int length, out int result)
    {
        result = length * length;
    }


    Console.WriteLine($"{enterlength} * {enterlength} = {quadrate}");


    int newQuadrate;

    newQuadrate = 10;

    Quadrate(enterlength, out newQuadrate);

    Console.WriteLine(newQuadrate);


}

//UsingOutModifier_2();
static void UsingOutModifier_2()
{
    int enterlength = 10;

    static void QuadrateAndVolume(int length,out bool isPositive , out int Quadrate, out int volume)
    {
        isPositive = length > 0;
        Quadrate = length * length;
        volume = length * length * length;
    }

    QuadrateAndVolume(enterlength, out bool isPositive, out int Quadrate, out int volume);

    Console.WriteLine($"{enterlength} isPositive:{isPositive} Quadrate:{Quadrate}, volume:{volume}");

    QuadrateAndVolume(5, out _, out _, out int newVolume);

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

//UsingOptionalPatameters();
static void UsingOptionalPatameters()
{
    Console.WriteLine(GetStringTemperature(20));
    Console.WriteLine(GetStringTemperature(68,"F"));

    static string GetStringTemperature(double temperature,string scale = "C")
    {
        return temperature.ToString() + "°" + scale;
    }

    //static string GetStringTemperatureWithDateTime(double temperature, 
    //    string scale = "C", 
    //    DateTime dateTime = DateTime.Now) // it don't work
    //{
    //    return temperature.ToString() + "°" + scale + dateTime.ToString() ;
    //}

}


//UsingNamedParameters();
static void UsingNamedParameters()
{

    Volume(length: 1, height: 3, width: 2);


    static void Volume(int length, int width , int height)
    {
        Console.WriteLine($"Lenght:{length} Width:{width} Height:{height}" );
        Console.WriteLine(length*width*height);    
    }

  
    Console.WriteLine(GetStringTemperature(temperature:20));
  
    static string GetStringTemperature(string scale = "C",double temperature = 0)
    {
        return temperature.ToString() + "°" + scale;
    }
}

//UsingOverload();
static void UsingOverload()
{
    int myInt = 10;
    double myDouble = 5.23;
    decimal myDecimal = 1000.2356M;
    float myFloat = 100.12F;
    long myLong = 100000000L;

    Console.WriteLine(Quadrate.GetQuadrate(myInt));
    Console.WriteLine(Quadrate.GetQuadrate(myDouble));
    Console.WriteLine(Quadrate.GetQuadrate(myDouble,2));
    Console.WriteLine(Quadrate.GetQuadrate(myDecimal));
    Console.WriteLine(Quadrate.GetQuadrate(myDecimal,2));
    Console.WriteLine(Quadrate.GetQuadrate(myFloat));
    Console.WriteLine(Quadrate.GetQuadrate(myLong)); 

}


CheckParameterForNull();

static void CheckParameterForNull()
{
    //SendMessageBad(null);
    //SendMessageLargeCheck(null);
    SendMessageShortSheck(null);


    static void SendMessageBad(string message)
    {
        Console.WriteLine(message.Length);
    }

    static void SendMessageLargeCheck(string message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(message);
        }
        Console.WriteLine("Send:"+message);
    }

    static void SendMessageShortSheck(string message)
    {
        ArgumentNullException.ThrowIfNull(message);
        Console.WriteLine("Send:" + message);

    }
}





static class Quadrate
{
    internal static string GetQuadrate(int lenght)
    {
        Console.WriteLine("I choose method 1");
        return (lenght * lenght).ToString();
    }
    internal static string GetQuadrate(double lenght)
    {
        Console.WriteLine("I choose method 2");
        return (lenght * lenght).ToString();
    }
    internal static string GetQuadrate(decimal lenght)
    {
        Console.WriteLine("I choose method 3");
        return (lenght * lenght).ToString();
    }
    internal static string GetQuadrate(double lenght, int accuracy = 2)
    {
        Console.WriteLine("I choose method 4");

        return double.Round(lenght * lenght, accuracy).ToString();
    }
    internal static string GetQuadrate(decimal lenght, int accuracy = 2)
    {
        Console.WriteLine("I choose method 5");
        return decimal.Round(lenght * lenght, accuracy).ToString();
    }

    internal static string GetQuadrate(long lenght)
    {
        Console.WriteLine("I choose method 6");
        return GetQuadrate((decimal)lenght);
    }

}