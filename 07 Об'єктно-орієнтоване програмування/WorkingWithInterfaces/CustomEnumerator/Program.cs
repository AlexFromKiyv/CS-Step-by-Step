
using CustomEnumerator;

void UsingForeach()
{
    // Iterate over an array of items.
    int[] myArrayOfInts = { 10, 20, 30, 40 };
    foreach (int i in myArrayOfInts)
    {
        Console.WriteLine(i);
    }
}
//UsingForeach();

void ShowGarage()
{
    Garage garage = new Garage();

    foreach (Car car in garage)
    {
        Console.WriteLine($"{car.PetName} is going {car.CurrentSpeed} MPH");
    }

    Console.WriteLine( );

    //System.Collections.IEnumerator enumerator = garage.GetEnumerator();

    //enumerator.MoveNext();
    //Console.WriteLine(((Car)enumerator.Current).PetName);
   
}
ShowGarage();
