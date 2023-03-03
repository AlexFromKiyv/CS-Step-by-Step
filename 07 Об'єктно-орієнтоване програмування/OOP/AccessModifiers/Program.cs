using AccessModifiers;

//LivingOrganism livingOrganism = new(); // LivingOrganism.LivingOrganism() inaccesible

Animal animal = new();

// animal.weight //no access

Dog snupy = new("mutt");
Console.WriteLine(snupy.Breed);