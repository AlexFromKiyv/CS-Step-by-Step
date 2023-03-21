
using RecordInheritance;
using System.Drawing;
using System.Reflection;

//ExploreRecordInheritance();
void ExploreRecordInheritance()
{
    MiniVan miniVan = new("Mersedes", "Vito", "White", 3);
    Console.WriteLine(miniVan);

    Console.WriteLine(miniVan is Car);

    Car car = miniVan;
    Console.WriteLine(car);
}

//ExplorePositionRecordInheritance();
void ExplorePositionRecordInheritance()
{
    MiniVan_v1 miniVan = new MiniVan_v1("Mercedes", "Vito", "Black", 4);
    Console.WriteLine(miniVan);

    Car_v1 car = new MiniVan_v1("VW", "Transporter", "White", 4);
    Console.WriteLine(car);
    Console.WriteLine(car is Car_v1);
    Console.WriteLine(car is MiniVan_v1);

    Car_v1 car1 = car with { Manufacturer = "VW", Model = "Transporter", Color = "Black" };
    Console.WriteLine(car1);
    Console.WriteLine(car1 is Car_v1);
    Console.WriteLine(car1 is MiniVan_v1);
}

//ExploreEqualityRecordInheritance();
void ExploreEqualityRecordInheritance()
{
    MotorCycle motorCycle = new("Harley", "Low Rider");
    MotorCycle motorCycle1 = new("Harley", "Low Rider");
    Console.WriteLine(motorCycle);
    Console.WriteLine(motorCycle1);
    Console.WriteLine(motorCycle == motorCycle1);

    Scooter scooter = new("Harley", "Low Rider");
    Console.WriteLine(motorCycle);
    Console.WriteLine(scooter);
    Console.WriteLine(motorCycle == scooter);

    MotorCycle motorCycle2 = new Scooter("Harley", "Low Rider");
    Console.WriteLine(motorCycle);
    Console.WriteLine(motorCycle2);
    Console.WriteLine(motorCycle == motorCycle2);

    Console.WriteLine(motorCycle.GetType());
    Console.WriteLine(motorCycle2.GetType());
}

ExploreDeconstructorRecordInheritance();
void ExploreDeconstructorRecordInheritance()
{
    MotorCycle motorCycle = new FancyScooter("Harley", "Low Rider", "Red");

    var (manufacturer1, model1) = motorCycle;
    Console.WriteLine($"{manufacturer1} {model1}");

    //var (manufacturer2, model2, color2 ) = motorCycle; // don't work
    var (manufacturer2, model2, color2) = (FancyScooter)motorCycle;
    Console.WriteLine($"{manufacturer2} {model2} {color2}");

}
