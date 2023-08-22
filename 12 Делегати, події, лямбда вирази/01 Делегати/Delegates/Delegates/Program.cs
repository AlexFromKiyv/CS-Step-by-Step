using Delegates;
void UseSimpleDelegate()
{
    // Create a BinaryIntOp delegate object that
    // 'points to' SimpleMath.Add().
    BinaryIntOp biIntOp = new(SimpleMath.Add);

    // Invoke Add() method indirectly using delegate object.
    // Invoke() is really called here!
    int result = biIntOp(5, 5);
    Console.WriteLine(result);

    Console.WriteLine(biIntOp.Invoke(10,10));
}

UseSimpleDelegate();


void DelegateIsTypeSafe()
{
    BinaryIntOp intOp1 = new(SimpleMath.Subtract);

    Console.WriteLine(intOp1(10,20));
    
    //BinaryIntOp intOp2 = new(SimpleMath.Square); // Square is method that have diferent signateure.
}




//...

// Additional type definitions must be placed at the end of the
// top-level statements

// This delegate can point to any method,
// taking two integers and returning an integer.
public delegate int BinaryIntOp(int x, int y);