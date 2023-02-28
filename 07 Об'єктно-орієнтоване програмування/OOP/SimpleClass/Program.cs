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

MethodWithThis();
void MethodWithThis()
{
    Bike bike = new();
    bike.SetOwnerNameWithThis("David");
    bike.StateToConsol();
}