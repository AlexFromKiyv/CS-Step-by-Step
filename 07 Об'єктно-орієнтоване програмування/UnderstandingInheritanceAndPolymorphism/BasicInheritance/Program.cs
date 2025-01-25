using BasicInheritance;

void UsingClassCar1()
{
    // Make a Car object, set max speed and current speed.
    Car1 car = new Car1(80) { Speed = 50};
    // Print current speed.
    Console.WriteLine($"My car is going {car.Speed} MPH");
}
//UsingClassCar1();

void UsingMiniVan1()
{
    // Don't work
    //MiniVan1 miniVan = new(100) { Speed = 50 };

    // Now make a MiniVan object.
    MiniVan1 miniVan = new() { Speed = 50 };
    Console.WriteLine($"My van is going {miniVan.Speed}");

    MiniVan1 miniVan1 = new();
    miniVan1.Speed = 30;
    Console.WriteLine($"My van is going {miniVan.Speed}");

    // Error! Can't access private members! 
    //miniVan1._speed = 70;

}
UsingMiniVan1();
