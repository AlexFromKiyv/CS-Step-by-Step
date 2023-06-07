using MyBusLibrary;
using MyLibrary;

UsingClassCar();
void UsingClassCar()
{
    Car car = new Car("Nissan", "Leaf", 2005);
    car.ToConsole();
}

UsingClassBus();
void UsingClassBus()
{
    Bus bus = new();
    bus.ToConsole();
}
