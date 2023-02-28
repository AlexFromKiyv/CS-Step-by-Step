using SimpleClass;

//UsingClassBike();
void UsingClassBike()
{
    Bike bike;
   // bike.StateToConsol(); don't work
    bike = new();
    bike.StateToConsol();

    bike.ownerName = "Mark";
    bike.currentSpeed = 5;
    bike.StateToConsol();


    for (int i = 0; i < 5; i++)
    {
        bike.SpeedUp(1);
        bike.StateToConsol();
    }
}

//UsingVariousConstructors();
void UsingVariousConstructors()
{
    Bike bike = new();
    bike.StateToConsol();

    Bike veraOnBike = new("Vera");
    veraOnBike.StateToConsol();

    Bike maxBike = new("Max", 15);
    maxBike.StateToConsol();
}


//IfNoUseThis();
void IfNoUseThis()
{
    Bike bike = new();
    bike.SetOwnerName("David");
    bike.StateToConsol();
}

//MethodWithThis();
void MethodWithThis()
{
    Bike bike = new();
    bike.SetOwnerNameWithThis("David");
    bike.StateToConsol();
}

//UsingChainOfConstructors();
void UsingChainOfConstructors()
{
    Bus25 bus25 = new(30);
    bus25.StateToConsol();
}

UsingConstructorWithOpionalParameter();

void UsingConstructorWithOpionalParameter()
{
    Bus bus_1 = new();
    bus_1.ToConsol();
    
    Bus bus_2 = new(15);
    bus_2.ToConsol();

    Bus bus_3 = new(driverName:"Mark");
    bus_3.ToConsol();

    Bus bus_4 = new(35, "Jack");
    bus_4.ToConsol();

    Bus bus_5 = new(default, default);
    bus_5.ToConsol();
}