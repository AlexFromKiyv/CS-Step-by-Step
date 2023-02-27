using SimpleClass;

UsingClassBike();
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


