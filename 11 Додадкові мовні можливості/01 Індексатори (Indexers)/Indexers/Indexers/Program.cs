using Indexers;

void WorkWithArray()
{
    string[] girls = { "Vera", "Olga", "Viktory" };

    girls[1] = "Ira";

    Console.WriteLine(girls[2]); 
    Console.WriteLine();

    for (int i = 0; i < girls.Length; i++)
    {
        Console.WriteLine($"{i}. {girls[i]}");
    }

    //Unhandled exception. System.IndexOutOfRangeException
    //girls[4] = "Nikita";
}

//WorkWithArray();


void UseSimpleIndexer()
{
    PersonCollection_v1 people = new();

    people[0] = new Person("John");
    people[0] = new Person("John");
    people[1] = new Person("Sara");

    people[2] = people[1];
    people[1] = new Person("Tony");

    for (int i = 0; i < 5; i++)
    {
        Console.WriteLine(people[i]);
    }
    //Unhandled exception. System.IndexOutOfRangeException
    //people[101] = new Person("");
}

//UseSimpleIndexer();

void UseGenericTypeIndexer()
{
    List<Person> people = new()
    {
        new("Tony"),
        new("Sara"),
        new("Sherlock"),
    };


    people[2] = people[0];

    people[0] = new Person("Jhon");

    for (int i = 0; i < people.Count; i++)
    {
        Console.WriteLine(people[i]);
    }

    //Unhandled exception. System.IndexOutOfRangeException
    //people[3] = new("Someone");
}

//UseGenericTypeIndexer();

void UseStringIndexer()
{
    PersonCollectionWithStringIndex personage = new();

    personage["John"] = new("John Connor");
    personage["Terminator"] = new("T-800");
    personage["IronMan"] = new("Tony Stark");

    Console.WriteLine(personage["Terminator"]);
    Console.WriteLine();

    personage["Terminator"] = new("T-1000");

    foreach (KeyValuePair<string, Person> item in personage)
    {
        Console.WriteLine($"Index:{item.Key}\tItem:{item.Value}");
    }
}

//UseStringIndexer();

void UseIndexerInterface()
{
    List<string> strings = new() { "in", "in front", "next to", "under", "on" };

    SomeStrings words = new(strings);

    for (int i = 0; i < words.Count; i++)
    {
        Console.WriteLine(words[i]);
    }
}

UseIndexerInterface();
