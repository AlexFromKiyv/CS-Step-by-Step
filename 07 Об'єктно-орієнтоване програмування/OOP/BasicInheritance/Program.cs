using BasicInheritance;
using System.Net.Http.Headers;

//ExamineParentClass();
void ExamineParentClass()
{
    Car car = new Car();

    car.Speed = 50;
    Console.WriteLine($"Car with a speed {car.Speed}");

    car.Speed = 80;
    Console.WriteLine($"Car with a speed {car.Speed}");

    Car redCar = new Car(120);

    redCar.Speed = 70;
    Console.WriteLine($"Car with a speed {redCar.Speed}");

    redCar.Speed = 150;
    Console.WriteLine($"Car with a speed {redCar.Speed}");

}
ExamineInheritancedClass();
void ExamineInheritancedClass()
{
    MiniVan van = new();
    Console.WriteLine(van.Speed);

    MiniVan van1 = new();
    van1.Speed = 120;
    Console.WriteLine(van1.Speed);

    MiniVan van2 = new() { Speed = 100 };
    Console.WriteLine(van2.Speed);

    //MiniVan van3 = new(100) //   doesn`t contain constructor

    Console.WriteLine(van2 is MiniVan);
    Console.WriteLine(van2 is Car);
}


