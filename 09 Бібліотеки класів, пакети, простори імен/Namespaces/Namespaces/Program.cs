
using Account;
using Vehicle.Cars;
using Env = System.Environment;

//ToConnectNamespace();
void ToConnectNamespace()
{
    Person person = new("Viktory");
    person.ToConsole();

    Car car = new Car();
    car.ToConsole();
}

UsingAlias();
void UsingAlias()
{
    Console.WriteLine(Env.OSVersion);
    Console.WriteLine(Env.Version);
    Console.WriteLine(Env.ProcessorCount);
 }
