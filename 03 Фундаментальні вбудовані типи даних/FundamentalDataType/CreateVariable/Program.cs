//

//CreateVarable();

using System.Xml.Linq;

static void CreateVarable()
{
    string name = "";
    decimal price = new decimal();
    bool electric = false, forChildren = false;
    double weight = default, height=default;

    name = "ВЕЛОСИПЕД 20 DOROZHNIK ONYX 2022";
    weight = 14.23;
    price = 12853; 
    
    Console.WriteLine($"{name} {price} {weight} ");
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

ImplicitDeclarations();
static void ImplicitDeclarations()
{
    var code = "025441";
    var name = "Bicycle";
    var weight = 14.23;
    var inStock = true;

    Console.WriteLine($"{code} {name} {weight} {inStock}");
    Console.WriteLine($"{code.GetType()} {name.GetType()} {weight.GetType()} {inStock.GetType()}");
}


