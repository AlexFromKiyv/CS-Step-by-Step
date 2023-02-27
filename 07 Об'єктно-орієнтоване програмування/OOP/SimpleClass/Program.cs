using SimpleClass;

UsingClassBike();
void UsingClassBike()
{
    Bike bike = new();

    bike.ownerName = "Mark";
    bike.currentSpeed = 5;

    for (int i = 0; i < 5; i++)
    {
        bike.SpeedUp(1);
        bike.StateToConsol();
    }
}