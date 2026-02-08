namespace AttributeForValidation;

[AgeValidation(25, 60)]
internal class Warrior
{
    public string Name { get; set; }
    public int Age { get; set; }

    public Warrior(string name, int age)
    {
        Name = name;
        Age = age;
    }
}
