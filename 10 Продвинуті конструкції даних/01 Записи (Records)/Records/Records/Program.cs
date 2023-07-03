
using Records;

//UsingCar();
void UsingCar()
{
    Car car1 = new Car("VW", "Polo", "Red");
    Car car2 = new()
    {
        Manufacturer = "VW",
        Model = "Polo",
        Color = "Red"
    };

    Console.WriteLine(car1.GetType());
    Console.WriteLine(car1);

    Console.WriteLine( car1 == car2 );
    Console.WriteLine(ReferenceEquals(car1,car2));
}

//UsingCarRecord_v1();
void UsingCarRecord_v1()
{
    CarRecord_v1 car1 = new("VW","Polo","Red");
    CarRecord_v1 car2 = new()
    {
        Manufacturer = "VW",
        Model = "Polo",
        Color = "Red"
    };

    Console.WriteLine(car1.GetType());
    Console.WriteLine(car1);
    Console.WriteLine( car1 == car2 );
    Console.WriteLine(ReferenceEquals(car1, car2));
}

//UsingCarRecord_v2();
void UsingCarRecord_v2()
{
    CarRecord_v2 car1 = new("VW", "Polo", "Red");
    CarRecord_v2 car2 = new CarRecord_v2("VW", "Polo", "Red");

   // car1.Manufacturer = "Volks Wagen"; // don't work  immutable


    Console.WriteLine(car1.GetType());
    Console.WriteLine(car1);
    Console.WriteLine(car1 == car2);
    Console.WriteLine(car1.Equals(car2));
    Console.WriteLine(ReferenceEquals(car1, car2));
}

//UsingDeconstuct();
void UsingDeconstuct()
{
    CarRecord_v1 car1 = new("VW", "Polo", "Red");
    
    // car1.Deconstruct( It's no here this method

    CarRecord_v2 car2 = new("VW", "Polo", "Red");

    string color;

    car2.Deconstruct(out string manufacturer, out string model, out color);


    Console.WriteLine($"{manufacturer} {model} {color}");

    var (manufacturer_1, model_1, color_1) = car2;

    Console.WriteLine($"{manufacturer_1} {model_1} {color_1}");

}

//AssigningRecord();
void AssigningRecord()
{
    CarRecord carRecord_1 = new CarRecord("VW", "Polo", "Red");
    CarRecord carRecord_2 = carRecord_1;

    Console.WriteLine(carRecord_2);
    Console.WriteLine(carRecord_1.Equals(carRecord_2));
    Console.WriteLine(ReferenceEquals(carRecord_1,carRecord_2));
}

//CopyingRecord();
void CopyingRecord()
{
    CarRecord carRecord = new("VW", "Polo", "Not known");

    CarRecord carRecord1 = carRecord with { Color = "White" };
    CarRecord carRecord2 = carRecord with { Color = "Red" };


    Console.WriteLine(carRecord1);
    Console.WriteLine(carRecord2);

    Console.WriteLine(carRecord.Equals(carRecord1));
    Console.WriteLine(ReferenceEquals(carRecord,carRecord1));

}

//UsingRecordStruct();
void UsingRecordStruct()
{
    Point_v1 point1 = new(1, 1);
    Console.WriteLine(point1);
    
    point1.X = 2;
    point1.Y = 2;
    Console.WriteLine(point1);

    Point_v2 point2 = new();
    point2.ToConsole();

    Point_v2 point3 = point2 with { Y = 1 };
    point3.ToConsole();

}

//UsingReadonlyRecordStruct();
void UsingReadonlyRecordStruct()
{
    Point_v3 point1 = new(1, 1);
    Console.WriteLine(point1);

    //point1.X = 2; don't work

    Point_v4 point2 = new(2,2);
    //point2.X = 3; don't work
    point2.ToConsole();
 
}
UsingDecontrictRecordStruct();

void UsingDecontrictRecordStruct()
{
    Point_v1 point = new(1, 2);
    var (x, y) = point;
    Console.WriteLine(x.GetType());
    Console.WriteLine(x);

    point.Deconstruct(out int x1,out int y1);
    Console.WriteLine(x1);
}