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

