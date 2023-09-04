
using UseEventArgs;

void UseEventPattern()
{
    Car car = new("Volkswagen Käfer", 105, 83);
    car.AboutToBlow += Car_AboutToBlow;
    car.Exploded += Car_Exploded;

    car.Accelerate(5);
    car.Accelerate(5);
    car.Accelerate(5);
    car.Accelerate(5);
    car.Accelerate(5);
    car.Accelerate(5);
}

void Car_Exploded(object sender, CarEventArgs e)
{
    if (sender is Car car)
    {
        Console.WriteLine($"{car.Name} : {e.message}");
    }
}
void Car_AboutToBlow(object sender, CarEventArgs e)
{
    if (sender is Car car)
    {
        Console.WriteLine($"Critical message from {car.Name} : {e.message}"  );
    }
}

UseEventPattern();

