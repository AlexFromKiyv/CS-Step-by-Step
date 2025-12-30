
using GenericCollections;

static void UseGenericList()
{
    // Make a List of Person objects, filled with
    // collection/object init syntax.
    List<Person> people = new List<Person>()
    {
        new Person {FirstName= "Homer", LastName="Simpson", Age=47},
        new Person {FirstName= "Marge", LastName="Simpson", Age=45},
        new Person {FirstName= "Lisa", LastName="Simpson", Age=9},
        new Person {FirstName= "Bart", LastName="Simpson", Age=8}
    };

    // Print out # of items in List.
    Console.WriteLine($"Items in list: {people.Count}");
    
    // Enumerate over list.
    foreach (Person p in people)
    {
        Console.WriteLine(p);
    }
    
    // Insert a new person.
    Console.WriteLine("\tInserting new person.");
    people.Insert(2, new Person { FirstName = "Maggie", LastName = "Simpson", Age = 2 });
    Console.WriteLine($"Items in list: {people.Count}");

    // Copy data into a new array.
    Person[] arrayOfPeople = people.ToArray();
    foreach (Person p in arrayOfPeople)
    {
        Console.WriteLine($"First Names: {p.FirstName}");
    }
}
//UseGenericList();

static void UseGenericStack()
{
    Stack<Person> stackOfPeople = new Stack<Person>();
    stackOfPeople.Push(new Person { FirstName = "Homer", LastName = "Simpson", Age = 47 });
    stackOfPeople.Push(new Person { FirstName = "Marge", LastName = "Simpson", Age = 45 });
    stackOfPeople.Push(new Person { FirstName = "Lisa", LastName = "Simpson", Age = 9 });

    foreach (var p in stackOfPeople)
    {
        Console.WriteLine(p);
    }
    Console.WriteLine();

    // Now look at the top item, pop it, and look again.
    Console.WriteLine($"\nFirst person is: {stackOfPeople.Peek()}");
    Console.WriteLine($"Popped off {stackOfPeople.Pop()}");
    Console.WriteLine($"\nFirst person is: {stackOfPeople.Peek()}");
    Console.WriteLine($"Popped off {stackOfPeople.Pop()}");
    Console.WriteLine($"\nFirst person is: {stackOfPeople.Peek()}");
    Console.WriteLine($"Popped off {stackOfPeople.Pop()}");

    try
    {
        Console.WriteLine($"\nFirst person is: {stackOfPeople.Peek()}");
        Console.WriteLine($"Popped off {stackOfPeople.Pop()}");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine("\nError! {0}", ex.Message);
    }
}
//UseGenericStack();

static void UseGenericQueue()
{
    // Make a Queue with three people.
    Queue<Person> peopleQ = new();
    peopleQ.Enqueue(new Person { FirstName = "Homer", LastName = "Simpson", Age = 47 });
    peopleQ.Enqueue(new Person { FirstName = "Marge", LastName = "Simpson", Age = 45 });
    peopleQ.Enqueue(new Person { FirstName = "Lisa", LastName = "Simpson", Age = 9 });

    foreach (var p in peopleQ)
    {
        Console.WriteLine(p);
    }
    Console.WriteLine();

    // Peek at first person in Q.
    Console.WriteLine($"{peopleQ.Peek().FirstName} is first in line!");

    // Remove each person from Q.
    GetCoffee(peopleQ.Dequeue());
    GetCoffee(peopleQ.Dequeue());
    GetCoffee(peopleQ.Dequeue());
    // Try to de-Q again?
    try
    {
        GetCoffee(peopleQ.Dequeue());
    }
    catch (InvalidOperationException e)
    {
        Console.WriteLine("\nError! {0}", e.Message);
    }

    //Local helper function
    static void GetCoffee(Person p)
    {
        Console.WriteLine($"\n{p.FirstName} got coffee!");
    }
}
//UseGenericQueue();

static void UsePriorityQueue()
{
    PriorityQueue<Person, int> peopleQ = new();
    peopleQ.Enqueue(new Person { FirstName = "Lisa", LastName = "Simpson", Age = 9 }, 3);
    peopleQ.Enqueue(new Person { FirstName = "Homer", LastName = "Simpson", Age = 47 }, 1);
    peopleQ.Enqueue(new Person { FirstName = "Marge", LastName = "Simpson", Age = 45 }, 3);
    peopleQ.Enqueue(new Person { FirstName = "Bart", LastName = "Simpson", Age = 12 }, 2);

    while (peopleQ.Count > 0)
    {
        Console.WriteLine(peopleQ.Dequeue().FirstName); 
        Console.WriteLine(peopleQ.Dequeue().FirstName); 
        Console.WriteLine(peopleQ.Dequeue().FirstName); 
        Console.WriteLine(peopleQ.Dequeue().FirstName); 
    }
}
//UsePriorityQueue();

static void UseSortedSet()
{
    // Make some people with different ages.
    SortedSet<Person> sortSetOfPeople = new SortedSet<Person>(new PeopleByAgeComparer())
    {
        new Person {FirstName= "Homer", LastName="Simpson", Age=47},
        new Person {FirstName= "Marge", LastName="Simpson", Age=45},
        new Person {FirstName= "Lisa", LastName="Simpson", Age=9},
        new Person {FirstName= "Bart", LastName="Simpson", Age=8}
    };

    // Note the items are sorted by age!
    foreach (var p in sortSetOfPeople)
    {
        Console.WriteLine(p);
    }
    Console.WriteLine();

    // Add a few new people, with various ages.
    sortSetOfPeople.Add(new Person { FirstName = "Saku", LastName = "Jones", Age = 1 });
    sortSetOfPeople.Add(new Person { FirstName = "Mikko", LastName = "Jones", Age = 32 });

    foreach (var p in sortSetOfPeople)
    {
        Console.WriteLine(p);
    }
    Console.WriteLine();
}
//UseSortedSet();

static void UseDictionary()
{
    // Populate using Add() method
    Dictionary<string, Person> peopleA = new Dictionary<string, Person>();
    peopleA.Add("Homer", new Person { FirstName = "Homer", LastName = "Simpson", Age = 47 });
    peopleA.Add("Marge", new Person { FirstName = "Marge", LastName = "Simpson", Age = 45 });
    peopleA.Add("Lisa", new Person { FirstName = "Lisa", LastName = "Simpson", Age = 9 });

    // Get Homer.
    Person homer = peopleA["Homer"];
    Console.WriteLine(homer);

    // Populate with initialization syntax.
    Dictionary<string, Person> peopleB = new Dictionary<string, Person>()
    {
        { "Homer", new Person { FirstName = "Homer", LastName = "Simpson", Age = 47 } },
        { "Marge", new Person { FirstName = "Marge", LastName = "Simpson", Age = 45 } },
        { "Lisa", new Person { FirstName = "Lisa", LastName = "Simpson", Age = 9 } }
    };
    // Populate with dictionary initialization syntax.    
    Dictionary<string, Person> peopleC = new Dictionary<string, Person>()
    {
        ["Homer"] = new Person { FirstName = "Homer", LastName = "Simpson", Age = 47 },
        ["Marge"] = new Person { FirstName = "Marge", LastName = "Simpson", Age = 45 },
        ["Lisa"] = new Person { FirstName = "Lisa", LastName = "Simpson", Age = 9 }
    };
    // Get Lisa.
    Person lisa = peopleB["Lisa"];
    Console.WriteLine(lisa);
}
//UseDictionary();