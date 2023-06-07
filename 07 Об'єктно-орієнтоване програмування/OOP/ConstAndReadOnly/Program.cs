
using ConstAndReadOnly;

UsingConstants();
void UsingConstants()
{
    //BodyMassIndex.LESS_THEN_NORM = 17; // don't work for const
    Console.WriteLine(BodyMassIndex.OVER_THEN_NORM);
}

UsingReadOnly();
void UsingReadOnly()
{
    BodyMassIndex_v1 bodyMassIndex_V1 = new BodyMassIndex_v1();

    //bodyMassIndex_V1.LESS_THEN_NORM = 18; don't work for read-only
    Console.WriteLine(bodyMassIndex_V1.LESS_THEN_NORM);

}
