
using ConstAndReadOnly;

UsingConstants();
void UsingConstants()
{
    //BodyMassIndex.LESS_THEN_NORM = 17; // don't work for const
    Console.WriteLine(BodyMassIndex.OVER_THEN_NORM);
}

