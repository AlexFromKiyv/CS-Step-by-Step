using SystemObject;

ExploreSystemObject();
void ExploreSystemObject()
{

    Person person1 = new Person();

    Console.WriteLine($"ToString:\t{person1.ToString()}");
    Console.WriteLine($"GetHashCode:\t{person1.GetHashCode()}");
    Console.WriteLine($"GetType:\t{person1.GetType()}");

    // new reference to person1
    Person person2 = person1;

    // new reference to person1
    object obj = person2;

    Console.WriteLine(obj.Equals(person1));
    Console.WriteLine(obj.Equals(person2));
    Console.WriteLine(person1.Equals(obj));
    Console.WriteLine(person2.Equals(obj));




}
