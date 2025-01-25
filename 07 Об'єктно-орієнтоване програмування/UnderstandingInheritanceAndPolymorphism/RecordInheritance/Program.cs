using RecordInheritance;

void ImplicitCastsRecords()
{
    Car1 c = new Car1("Honda", "Pilot", "Blue");
    MiniVan1 m = new MiniVan1("Honda", "Pilot", "Blue", 10);
    Console.WriteLine($"Checking MiniVan is-a Car:{m is Car1}");
}
//ImplicitCastsRecords();

void UsingPosotionalRecord()
{
    MiniVan2 vito = new MiniVan2("Mercedes", "Vito 110", "DarkGrey", 18);
    Console.WriteLine(vito);

    FancyScooter scooter = new FancyScooter("Honda", "Reveals","Red-Blue");
    Console.WriteLine(scooter);
}
//UsingPosotionalRecord();

void NondestructiveMutationWithInheritedRecord()
{
    MotorCycle motorCycle1 = new FancyScooter("Harley", "Lowrider", "Gold");
    Console.WriteLine(motorCycle1);
    Console.WriteLine($"motorCycle1 is FancyScooter :{motorCycle1 is FancyScooter}");

    MotorCycle motorCycle2 = motorCycle1 with { Model = "Low Rider S" };
    Console.WriteLine(motorCycle2);
    Console.WriteLine($"motorCycle2 is FancyScooter :{motorCycle2 is FancyScooter}");
}
//NondestructiveMutationWithInheritedRecord();

void EqualityWithInheritedRecord()
{
    MotorCycle motorCycle = new MotorCycle("Harley", "Low Rider");
    MotorCycle scooter = new Scooter("Harley", "Low Rider");
    Console.WriteLine($"MotorCycle and Scooter are equal: {Equals(motorCycle,scooter)}");
    Console.WriteLine(motorCycle);
    Console.WriteLine(scooter);
}
//EqualityWithInheritedRecord();

void DeconstructorBehaviorWithInheritedRecord()
{
    MotorCycle motorCycle = new FancyScooter("Harley", "Low rider", "Gold");
    var (make1, model1) = motorCycle;
    Console.WriteLine(make1+"\t"+model1);

    // You need to cast the variable to the derived type
    var (make2, model2,color2) = (FancyScooter)motorCycle;
    Console.WriteLine(make2 + "\t" + model2+"\t"+color2);
}
//DeconstructorBehaviorWithInheritedRecord();

