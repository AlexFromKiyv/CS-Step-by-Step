
using LambdaAndMemebers;

void UseLambda()
{
    PrintSquared(5);

    void PrintSquared(int x) => MoreSimpleMath.PrintMultiplying(x,x); 
}

//UseLambda();

void UseLambdaAsAccessor()
{
    Car car = new("VW Golf", 160);

    Console.WriteLine(car.MaxSpeed);
}

UseLambdaAsAccessor();