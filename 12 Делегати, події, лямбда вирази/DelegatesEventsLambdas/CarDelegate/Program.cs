
using CarDelegate;

static void DelegatesAsEventEnablers()
{
    // First, make a Car object.
    Car car = new Car("SlugBug",100,10);

    // Now, tell the car which method to call
    // when it wants to send us messages.
    car.RegisterWithCarEngine(new Car.CarEngineHandler(OnCarEngineEvent));
    //car.RegisterWithCarEngine(new Car.CarEngineHandler(OnCarEngineEventToUpper));

    Car.CarEngineHandler handler2 = OnCarEngineEventToUpper;
    car.RegisterWithCarEngine(handler2);
    car.UnRegisterWithCarEngine(handler2);

    // Speed up (this will trigger the events).
    Console.WriteLine("***** Speeding up *****");
    for (int i = 0; i < 6; i++)
    {
        car.Accelerate(20);
    }
}
//DelegatesAsEventEnablers();
static void OnCarEngineEvent(string message)
{
    Console.WriteLine("\n*** Message From Car Object ***");
    Console.WriteLine($"=> {message}");
    Console.WriteLine("********************\n");
}

static void OnCarEngineEventToUpper(string message)
{
    Console.WriteLine($"=> {message.ToUpper()}\n");
}

static void DelegatesAsEventEnablers1()
{

    Car car = new Car("SlugBug", 100, 10);

    car.RegisterWithCarEngine(OnCarEngineEvent);

    car.RegisterWithCarEngine(OnCarEngineEventToUpper);
    car.UnRegisterWithCarEngine(OnCarEngineEventToUpper);

    Console.WriteLine("***** Speeding up *****");
    for (int i = 0; i < 6; i++)
    {
        car.Accelerate(20);
    }
}
DelegatesAsEventEnablers1();