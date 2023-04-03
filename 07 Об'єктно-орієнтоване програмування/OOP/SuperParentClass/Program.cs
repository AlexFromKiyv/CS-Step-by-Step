using SuperParentClass;

//ExploreSystemObject();
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

//ExploreOverridingToString();
void ExploreOverridingToString()
{
    Person_v1 person_1 = new Person_v1("Mark", "Twain", 49);
    Console.WriteLine(person_1);

    Person_v1 person_2 = new Person_v1();
    Console.WriteLine(person_2);

}

//ExploreOverridingEquals();
void ExploreOverridingEquals()
{
    Person_v2 person_1 = new();
    Person_v2 person_2 = new();

    Console.WriteLine(person_1.Equals(person_2));

    Person_v2 person_3 = new("Elvis", "Presley", 35);
    Person_v2 person_4 = new("Elvis", "Presley", 35);

    Console.WriteLine(person_3.Equals(person_4));
    Console.WriteLine(person_3.GetHashCode());
    Console.WriteLine(person_4.GetHashCode());


    Person_v2 person_5 = new("Elvis", "Presley", 36);
    Console.WriteLine(person_3.Equals(person_5));
}

//ExploreOverridingEqualsWithToSting();
void ExploreOverridingEqualsWithToSting()
{
    Person_v3 person_1 = new();
    Person_v3 person_2 = new();

    Console.WriteLine(person_1.Equals(person_2));

    Person_v3 person_3 = new("Elvis", "Presley", 35);
    Person_v3 person_4 = new("Elvis", "Presley", 35);

    Console.WriteLine(person_3.Equals(person_4));
    Console.WriteLine(person_3.GetHashCode());
    Console.WriteLine(person_4.GetHashCode());


    Person_v3 person_5 = new("Elvis", "Presley", 36);
    Console.WriteLine(person_3.Equals(person_5));
}

//ExploreHashCode();
void ExploreHashCode()
{
    string string1 = "Hi girl";
    string string2 = "Hi girl";
    string string3 = "нi girl";

    Console.WriteLine(string1.GetHashCode());
    Console.WriteLine(string2.GetHashCode());
    Console.WriteLine(string3.GetHashCode());
}

//GenerateHashCodeWithStringProperty();
void GenerateHashCodeWithStringProperty()
{
    Console.WriteLine("1234567".GetHashCode());

    Person_v4 person_1 = new("Mark", "Twain", 40, "1234567");
    Console.WriteLine(person_1.GetHashCode());

    Person_v4 person_2 = new("Mark", "Twain", 40, "1234567");
    Console.WriteLine(person_2.GetHashCode());

    Person_v4 person_3 = new("Elwis", "Presley", 40, "1234567");
    Console.WriteLine(person_3 .GetHashCode());

    Person_v4 person_4 = new("Elwis", "Presley", 40, "1234566");
    Console.WriteLine(person_4.GetHashCode());

}

//GenerateHashCodeWithHashCode();
void GenerateHashCodeWithHashCode()
{
    Console.WriteLine(HashCode.Combine(1, "Mark", "Twain", 40));

    Person_v5 person_1 = new(1,"Mark", "Twain", 40);
    Console.WriteLine(person_1.GetHashCode());

    Person_v5 person_2 = new(2,"Mark", "Twain", 40);
    Console.WriteLine(person_2.GetHashCode());

    Person_v5 person_3 = new(3,"Elwis", "Presley",40);
    Console.WriteLine(person_3.GetHashCode());

    Person_v5 person_4 = new(4,"Elwis", "Presley", 40);
    Console.WriteLine(person_4.GetHashCode());

}

ExploreStaticMemeberObject();
void ExploreStaticMemeberObject()
{
    Person_v5 person_1 = new();
    Person_v5 person_2 = new();

    Console.WriteLine(object.Equals(person_1,person_2));
    Console.WriteLine(ReferenceEquals(person_1,person_2));

    Person_v5 person_3 = new(1,"Leonardo","da Vinci",35);
    Person_v5 person_4 = new(1, "Leonardo", "da Vinci", 35);

    Console.WriteLine(object.Equals(person_3, person_4));
    Console.WriteLine(ReferenceEquals(person_3, person_4));
}