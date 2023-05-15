
using ObjectLifetime;


ExplorationClassObjectReference();
void ExplorationClassObjectReference()
{
    CreateOldCar();

    void CreateOldCar()
    {

        int max = 250;

        Car myCar = new Car("Volkswagen Käfer", 115, 30);

        Console.WriteLine(myCar.ToString());
        Console.WriteLine(max/ myCar.MaxSpeed);

    }
}
