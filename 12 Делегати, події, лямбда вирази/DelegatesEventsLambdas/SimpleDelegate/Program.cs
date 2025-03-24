
using SimpleDelegate;

static void UsingDelegate()
{
    // Create a BinaryOp delegate object that
    // 'points to' SimpleMath.Add().
    BinaryOp myAdd = new BinaryOp(SimpleMath.Add);

    // Invoke Add() method indirectly using delegate object.
    Console.WriteLine(myAdd(10,10));
    Console.WriteLine(myAdd.Invoke(10,20));

    // Compile Error
    //BinaryOp sQ = new BinaryOp(SimpleMath.SquareNumber);
}
//UsingDelegate();

static void DisplayDelegateInfo(Delegate delObj)
{
    // Print the names of each member in the
    // delegate's invocation list.
    foreach (Delegate @delegate in delObj.GetInvocationList())
    {
        Console.WriteLine($"Method Name: {@delegate.Method}");
        Console.WriteLine($"Type Name: {@delegate.Target}");
    }
}
static void InvestigatingADelegateObject()
{
    BinaryOp bOp = new(SimpleMath.Add);
    DisplayDelegateInfo(bOp);

    SimpleMath simpleMath = new();
    BinaryOp bop1 = new(simpleMath.otherAdd);
    DisplayDelegateInfo(bop1);
}
InvestigatingADelegateObject();



// Additional type definitions must be placed at the end of the
// top-level statements

// This delegate can point to any method,
// taking two integers and returning an integer.
public delegate int BinaryOp(int x, int y);

