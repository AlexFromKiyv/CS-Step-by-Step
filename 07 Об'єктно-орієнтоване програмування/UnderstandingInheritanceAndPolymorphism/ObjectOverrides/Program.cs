
using ObjectOverrides;
using System.Runtime.CompilerServices;

void InteractionWithMembersOfSystemObject()
{
    Person person1 = new Person();

    // Use inherited members of System.Object.
    Console.WriteLine($"ToString()\t: {person1.ToString()}");
    Console.WriteLine($"GetHashCode()\t: {person1.GetHashCode()}");
    Console.WriteLine($"GetType() : {person1.GetType()}");

    // Make some other references to person1.

    Person person2 = person1;
    object objectPerson = person2;

    // Are the references pointing to the same object in memory?
    if (objectPerson.Equals(person1) && person1.Equals(objectPerson) )
    {
        Console.WriteLine("The variables are pointing to the same object in memory.");
        Console.WriteLine(person1.GetHashCode());
        Console.WriteLine(person2.GetHashCode());
        Console.WriteLine(objectPerson.GetHashCode());
    }
}
//InteractionWithMembersOfSystemObject();


void OverrideToString()
{
    Person1 person = new Person1("John","Connor",16);

    Console.WriteLine(person);
}
//OverrideToString();

void OverrideEquals()
{
    Person1 person1 = new Person1("John", "Connor", 16);

    Person1 person2 = new Person1("John", "Connor", 16);

    Console.WriteLine(ReferenceEquals(person1,person2));

    Console.WriteLine(person1.Equals(person2));
}
//OverrideEquals();

void OverrideGetHashCode()
{
    //string string1 = "Hello";
    //string string2 = "Hello";
    //string string3 = "hello";

    //Console.WriteLine(string1.GetHashCode());
    //Console.WriteLine(string2.GetHashCode());
    //Console.WriteLine(string3.GetHashCode());

    Person2 person1 = new Person2("John", "Connor", 32, "1234-23-45");
    Person2 person2 = new Person2("Sara", "Connor", 32, "1234-23-45");

    Console.WriteLine(person1.GetHashCode() == person2.GetHashCode() );
    Console.WriteLine(person1.Equals(person2));
}
//OverrideGetHashCode();

void StaticMemebersSystemObject()
{
    Person1 person1 = new("Tomy", "Stark", 40);
    Person1 person2 = new("Tomy", "Stark", 40);

    Console.WriteLine(Equals(person1,person2));
    Console.WriteLine(ReferenceEquals(person1,person2));
}
StaticMemebersSystemObject();

