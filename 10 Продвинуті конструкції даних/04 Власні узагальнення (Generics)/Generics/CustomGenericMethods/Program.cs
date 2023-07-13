using CustomGenericMethods;
using System.Drawing;

void UseGenericMethod()
{
    int a = 35, b = 50;
    Console.WriteLine($"{a} {b}");
    SwapFunctions.Swap<int>(ref a,ref b);
    Console.WriteLine($"{a} {b}\n");

    string vechile1 = "Bus", vechile2 = "eCar";
    Console.WriteLine($"{vechile1} {vechile2}");
    SwapFunctions.Swap<string>(ref vechile1,ref vechile2);
    Console.WriteLine($"{vechile1} {vechile2}\n");

    Person person1= new Person(1,"Alex"), person2 = new(2,"Ira");
    Console.WriteLine($"{person1} {person2}");
    SwapFunctions.Swap<Person>(ref person1,ref person2);
    Console.WriteLine($"{person1} {person2}");

}

//UseGenericMethod();

void UseGenericMethodWithoutTypeParameter()
{
    bool run = true, stop = false;

    SwapFunctions.Swap(ref run,ref stop);
}

//UseGenericMethodWithoutTypeParameter();

void UseGenericStruct()
{
    Point<int> point = new(1, 2);
    Console.WriteLine(point);

    Point<double> point1 = new(1.01, 2.02);
    Console.WriteLine(point1);

    Point<string> point2 = new("one", "two");
    Console.WriteLine(point2);

    point1.Reset();
    Console.WriteLine(point1);

    point2.Reset();
    Console.WriteLine(point2);

    Point<long> point3 = default;
    Console.WriteLine(point3);
}

UseGenericStruct();