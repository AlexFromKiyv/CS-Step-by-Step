
using SimpleClassExample;

static void SimpleUsingObject1()
{
    // Allocate and configure a Car object.
    Car car = new();
    car.petName = "Henry";
    car.currSpeed = 10;

    // Speed up the car a few times and print out the
    // new state.
    for (int i = 0; i <= 10; i++)
    {
        car.SpeedUp(5);
        car.PrintState();
    }
}
//SimpleUsingObject1();



static void AboutNew()
{
    // Compiler error! Forgot to use 'new' to create object!
    //Car myCar;
    //myCar.petName = 'Fred';

    Car myCar = new();
    myCar.petName = "Lastivka";

    Car yourCar;
    yourCar = new();
    yourCar.PrintState();
}

static void DefaultConstructor()
{
    // Invoking the default constructor.
    Car car = new();
    car.PrintState();
}
//DefaultConstructor();

static void UsingConstructors()
{
    // Make a Car called No name going 0 MPH.
    Car chuck = new Car();
    chuck.PrintState();
    // Make a Car called Mary going 0 MPH.
    Car mary = new Car("Mary");
    mary.PrintState();
    // Make a Car called Daisy going 75 MPH.
    Car daisy = new Car("Daisy", 75);
    daisy.PrintState();

    // Use out parameter
    Car fastic = new Car("Fastic", 120, out bool danger);
    Console.WriteLine(danger);

    // Default constructor
    Motorcycle motorcycle = new();
    motorcycle.Drive();

    Motorcycle motorcycle1 = new(100);
    motorcycle1.Drive();
}
//UsingConstructors();

static void AboutThis()
{
    Motorcycle motorcycle = new();
    motorcycle.SetName("Julia");
    Console.WriteLine($"Biker:{motorcycle.driverName}");
}
//AboutThis();

static void ObservingConstructorFlow()
{
    Motorcycle4 motorcycle = new(7);
    motorcycle.SetName("John");
    motorcycle.Drive();
}
//ObservingConstructorFlow();

static void ConstructorWithOptionalArguments()
{
    Motorcycle5 motorcycle1 = new();
    motorcycle1.Drive();

    Motorcycle5 motorcycle2 = new(3);
    motorcycle2.Drive();

    Motorcycle5 motorcycle3 = new(driverName:"Jack");
    motorcycle3.Drive();

    Motorcycle5 motorcycle4 = new(7,"Max");
    motorcycle4.Drive();

}
ConstructorWithOptionalArguments();