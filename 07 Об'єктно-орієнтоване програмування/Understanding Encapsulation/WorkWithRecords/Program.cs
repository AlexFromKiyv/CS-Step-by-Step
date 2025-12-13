using WorkWithRecords;

void UsingClassCar()
{
    //Use object initialization
    Car car1 = new Car
    {
        Make = "Honda",
        Model = "Pilot",
        Color = "Blue"
    };
    DisplayCarState(car1);

    //Use the custom constructor
    Car car2 = new Car("Honda", "Pilot", "Blue");
    DisplayCarState(car2);

    Console.WriteLine($"Cars are the same? {car1.Equals(car2)}");
    Console.WriteLine($"Cars are the same reference? {ReferenceEquals(car1,car2)}");

    //Compile error if property is changed
    //car1.Color = "Red";

    void DisplayCarState(Car car)
    {
        Console.WriteLine($"{car.Make} {car.Model} {car.Color}");
    }

}
//UsingClassCar();

void UsingCarRecord()
{
    //Use object initialization
    CarRecord carRecord1 = new CarRecord
    {
        Make = "Honda",
        Model = "Pilot",
        Color = "Blue"
    };
    DisplayCarRecordState(carRecord1);

    //Use the custom constructor
    CarRecord carRecord2 = new CarRecord("Honda", "Pilot", "Blue");
    DisplayCarRecordState(carRecord2);

    Console.WriteLine();

    Console.WriteLine(carRecord1.ToString());
    Console.WriteLine(carRecord2.ToString());
 
    void DisplayCarRecordState(CarRecord carRecord)
    {
        Console.WriteLine($"{carRecord.Make} {carRecord.Model} {carRecord.Color}");
    }
}
//UsingCarRecord();

void UsingCarRecord1()
{
    //Don't work
    //CarRecord1 carRecord1 = new CarRecord1
    //{
    //    Make = "Honda",
    //    Model = "Pilot",
    //    Color = "Blue"
    //};

    //Use the internal constructor
    CarRecord1 carRecord1 = new CarRecord1("Honda", "Pilot", "Blue");
    Console.WriteLine(carRecord1.ToString());
}
//UsingCarRecord1();

void DecondtructRecord()
{
    CarRecord1 carRecord1 = new CarRecord1("Honda", "Pilot", "Blue");
    carRecord1.Deconstruct(out string make, out string model, out string color);

    Console.WriteLine(make+"\t"+model+"\t"+color);

    var (p1, p2, p3) = carRecord1;
    Console.WriteLine(p1 + "\t" + p2 + "\t" + p3);

}
//DecondtructRecord();

void ValueEqualityWithRecord()
{
    CarRecord carRecord1 = new CarRecord("Honda", "Pilot", "Blue");
    CarRecord carRecord2 = new CarRecord("Honda", "Pilot", "Blue");

    Console.WriteLine($"CarRecords are the same? {carRecord1.Equals(carRecord2)}");
    Console.WriteLine($"CarRecords are the same reference? " +
        $"{ReferenceEquals(carRecord1,carRecord2)}");

    Console.WriteLine($"CarRecords are the same?{carRecord1 == carRecord2}");
    Console.WriteLine($"CarRecords are not the same?{carRecord1 != carRecord2}");
}
//ValueEqualityWithRecord();

void CopyingRecordUsingWithExpressions()
{
    CarRecord carRecord1 = new CarRecord("Honda", "Pilot", "Blue");
    CarRecord carRecord2 = carRecord1;

    Console.WriteLine($"CarRecords are the same? {carRecord1.Equals(carRecord2)}");
    Console.WriteLine($"CarRecords are the same reference? " +
        $"{ReferenceEquals(carRecord1, carRecord2)}");

    Console.WriteLine();

    CarRecord carRecord3 = carRecord1 with { Model = "Odyssey" };
    Console.WriteLine(carRecord3);
    Console.WriteLine($"CarRecords are the same? {carRecord1.Equals(carRecord3)}");
    Console.WriteLine($"CarRecords are the same reference? " +
        $"{ReferenceEquals(carRecord1, carRecord3)}");

}
//CopyingRecordUsingWithExpressions();