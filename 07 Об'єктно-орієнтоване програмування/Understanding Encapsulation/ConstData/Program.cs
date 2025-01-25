
using ConstData;

void UsingConstant()
{
    Console.WriteLine($"The value of PI is:{MyMathClass.PI}");
    // Error! Can't change a constant!
    // MyMathClass.PI = 3.1444;
}
//UsingConstant();

void LocalConstStringVariable()
{
    // A local constant data point can be directly accessed.
    const string fixedStr = "Fixed string Data";
    Console.WriteLine(fixedStr);
    // Error!
    // fixedStr = 'This will not work!';
}
//LocalConstStringVariable();

void ConstInterpolationString()
{
    const string string1 = "Merry";
    const string string2 = "Christmas";
    const string result = $"{string1} {string2}";
    Console.WriteLine(result);
}
ConstInterpolationString();
