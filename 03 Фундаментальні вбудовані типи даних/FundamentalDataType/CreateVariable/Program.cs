//

CreateVarable();

static void CreateVarable()
{
    string name = "";
    decimal price = new decimal();
    bool electric = false, forChildren = false;
    double weight = default, height=default;

    name = "ВЕЛОСИПЕД 20 DOROZHNIK ONYX 2022";
    weight = 14.23;
    price = 12853; 
    
    Console.WriteLine($"{nameof(name)}: {name}\n{nameof(price)}: {price}\n{nameof(weight)}: {weight} ");
}

//UsingNew();
static void UsingNew()
{
    int myInt = new int();
    double myDouble = new();
    bool myBool = new();
    DateTime myDateTime = new();

    Console.WriteLine("By default:");
    Console.WriteLine($"int:{myInt}");
    Console.WriteLine($"double:{myDouble}");
    Console.WriteLine($"bool:{myBool}");
    Console.WriteLine($"DataTime:{myDateTime}");
}

//ImplicitDeclarations();
static void ImplicitDeclarations()
{
    var code = "025441";
    var name = "Bicycle";
    var weight = 14.23;
    var inStock = true;
   
    //var something; //must be initialized


    Console.WriteLine($"{code} {name} {weight} {inStock}");
    Console.WriteLine($"{code.GetType()} {name.GetType()} {weight.GetType()} {inStock.GetType()}");
}

//Console.WriteLine(GetVarString("Hi girl"));

static string GetVarString(string enterString)
{
    var myString =  enterString+"!";

    //myString = 45;// Cannot implicitly convert int to string

    return myString;
}

 static void ReadebleCode()
{
    var sum = GetSum(10,15);

    var squareSum = sum * sum;

    var power = 12;

    var result = squareSum * power;

    static double GetSum(int a,int b)
    {
        return a + b;
    }
}

//UsinVarForLinq();

static void UsinVarForLinq()
{
    int[] temperaturs = { 5, 12, 4, 15, 10, 8, 17 };

    var normal = from t in temperaturs where t > 10 select t;

    foreach (var item in normal)
    {
        Console.WriteLine(item);
    }

    Console.WriteLine(normal.GetType().Name);
}
