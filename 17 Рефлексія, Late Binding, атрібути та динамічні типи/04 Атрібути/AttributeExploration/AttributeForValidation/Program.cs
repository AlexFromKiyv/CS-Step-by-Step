using AttributeForValidation;

static bool AgeValidationForWarrior(Warrior warrior)
{
    Type typeWarrior = warrior.GetType();

    var attributes = typeWarrior.GetCustomAttributes(false);

    foreach (Attribute attribute in attributes)
    {
        if (attribute is AgeValidationAttribute ageValidationAttribute)
        {
            return ageValidationAttribute.From <= warrior.Age && warrior.Age < ageValidationAttribute.To;
        }
    }
    return true;
}

static void IsAsWarriorAgeAppropriate(Warrior warrior)
{
    string result = AgeValidationForWarrior(warrior) ? "is" : "is not";

    Console.WriteLine($"{warrior.Name} {warrior.Age} as a warrior {result} age appropriate.");
}

void WarriorValidation()
{
    Warrior max = new("Max", 58);

    IsAsWarriorAgeAppropriate(max);

    Warrior julia = new("Julia", 23);

    IsAsWarriorAgeAppropriate(julia);
}
WarriorValidation();