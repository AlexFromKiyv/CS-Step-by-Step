// Methods
static void StringTarget(string arg)
{
    Console.WriteLine($"arg in uppercase is: {arg.ToUpper()}");
}
static void IntTarget(int arg)
{
    Console.WriteLine($"++arg is: {++arg}");
}

// Using the generic delegate.

// Register targets.

MyGenericDelegate<string> strTarget = new(StringTarget);
strTarget("hi everyone");

MyGenericDelegate<int> intTarget = new(IntTarget);
intTarget(50);


// This generic delegate can represent any method
// returning void and taking a single parameter of type T.
public delegate void MyGenericDelegate<T>(T arg);

