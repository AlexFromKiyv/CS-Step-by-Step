using MyBusLibrary;
using MyLibrary;


void UsingClassCar()
{
    Car car = new Car("Nissan", "Leaf", 2005);
    car.ToConsole();
}
//UsingClassCar();
void UsingClassBus()
{
    Bus bus = new();
    bus.ToConsole();
}
UsingClassBus();