using AccessModifiers;

//LivingOrganism livingOrganism = new(); // LivingOrganism.LivingOrganism() inaccesible

void UsingAnimal()
{
    Animal animal = new();

    // animal.weight //no access
}



void UsingDog()
{
    Dog snupy = new("mutt");
    Console.WriteLine(snupy.Breed);
}

