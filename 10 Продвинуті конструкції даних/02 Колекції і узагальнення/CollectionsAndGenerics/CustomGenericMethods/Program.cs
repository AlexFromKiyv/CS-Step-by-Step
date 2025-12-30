
using CustomGenericMethods;

void SwapingIntAndString()
{
    // Swap 2 ints.
    int a = 10, b = 90;
    Console.WriteLine($"Before swap: {a} {b}");
    SwapFunctions.Swap<int>(ref a, ref b);
    Console.WriteLine($"After swap: {a} {b}");
    Console.WriteLine();

    // Swap 2 strings.
    string s1 = "Hello", s2 = "There";
    Console.WriteLine($"Before swap: {s1} {s2}");
    SwapFunctions.Swap<string>(ref s1, ref s2);
    Console.WriteLine($"After swap: {s1} {s2}");
    Console.WriteLine();
}
//SwapingIntAndString();

void SwapingBoolean()
{
    // Compiler will infer System.Boolean.
    bool b1 = true, b2 = false;
    Console.WriteLine($"Before swap: {b1}, {b2}");
    SwapFunctions.Swap(ref b1, ref b2);
    Console.WriteLine($"After swap: {b1}, {b2}");
    Console.WriteLine();
}
//SwapingBoolean();

static void DisplayBaseClass<T>()
{
    // BaseType is a method used in reflection,
    // which will be examined in Chapter 17
    Console.WriteLine($"Base class of {typeof(T)} is: {typeof(T).BaseType}.");
}

// Must supply type parameter if
// the method does not take params.
DisplayBaseClass<int>();
DisplayBaseClass<string>();
// Compiler error! No params? Must supply placeholder!
//DisplayBaseClass();
