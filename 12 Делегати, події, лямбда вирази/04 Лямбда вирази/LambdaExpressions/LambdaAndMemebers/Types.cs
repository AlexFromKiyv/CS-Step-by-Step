using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaAndMemebers;

static class SimpleMath
{
    public static int Multiplying(int x, int y)
    {
        return x * y;
    }
    public static void PrintMultiplying(int x, int y)
    {
        Console.WriteLine(x * y);
    }
}

static class MoreSimpleMath
{
    public static int Multiplying(int x, int y) =>  x * y;
    
    public static void PrintMultiplying(int x, int y) => Console.WriteLine( x * y );
}

class Car
{
    public string? Name { get; set; }
    
    private int _maxSpeed;
    public int MaxSpeed => _maxSpeed;
    public Car()
    {
    }
    public Car(string? name, int maxSpeed)
    {
        Name = name;
        _maxSpeed = maxSpeed;
    }
}
