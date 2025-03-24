// Methods 
using PublicDelegateProblem;

static void CallWhenExploded(string message)
{
    Console.WriteLine(message);
}
static void CallHereToo(String message)
{
    Console.WriteLine(message);
}

// Using
static void MakeProblem()
{
    Car car = new();
    // We have direct access to the delegate!
    car.ListOfHandler = CallWhenExploded;
    car.Accelerate(10);

    // We can now assign to a whole new object...
    // confusing at best.
    car.ListOfHandler = CallHereToo;
    car.Accelerate(10);

    // The caller can also directly invoke the delegate!
    car.ListOfHandler.Invoke("hee, hee, hee...");
}
MakeProblem();

