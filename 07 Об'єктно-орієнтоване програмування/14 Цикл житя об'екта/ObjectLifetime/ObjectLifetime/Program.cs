
using ObjectLifetime;


ExplorationClassObjectReference();
void ExplorationClassObjectReference()
{
    int max = 115;    

    Car myCar = new Car("Volkswagen Käfer", max, 30);

    Console.WriteLine(myCar.ToString());
}
