
using GenericCollections;
using Generics;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

void SpecifyingGenericParameters()
{
    List<Car> cars = new()
    {
        new Car(1,"VW", "Beetle", 2020),
        new Car(2,"VW", "E-Buster", 2025)
    };

    Car car = cars[0];
    car.Year = 2016;

    foreach (Car? item in cars)
    {
        item.ToConsole();
    }
}

//SpecifyingGenericParameters();

void UseGenericMemeber()
{
    int[] ints = { 31, 22, 13, 4, 25 };

    Array.Sort<int>(ints);

    foreach (int item in ints)
    {
        Console.Write(item + "\t");
    }
}

//UseGenericMemeber();

void UsingGeneticInterface()
{
    List<Car_v1?> cars = new()
    {
        new Car_v1(3,"VW","New Beetle",1998),
        new Car_v1(1,"VW", "Käfer", 1938),
        null,
        new Car_v1(2,"VW", "Golf", 1974),
        null
    };

    Console.WriteLine("\n Before sorting.");
    PrintListCar(cars);

    cars.Sort();

    Console.WriteLine("\n After sorting.");
    PrintListCar(cars);

    void PrintListCar(List<Car_v1?> list)
    {
        foreach (var item in list)
        {
            if (item is null)
            {
                Console.WriteLine();
            }
            else
            {
                item.ToConsole();
            }
        }
    }
}

//UsingGeneticInterface();

void InitialiazationCollection()
{
    int[] simpleArray = { 1, 2, 3 };
    PrintCollection(simpleArray);

    List<int> simpleList = new() { 1, 2, 3 };
    PrintCollection(simpleList);

    List<Point> listOfPoint = new()
    {
        new(1,2),
        new(2,3),
        new(3,4)
    };
    PrintCollection(listOfPoint);

    List<Rectangle> listOfRectangle = new()
    {
        new(){Height =90,Width =40, Location = new(1,2) },
        new(){Height =100,Width =50, Location = new(2,4) },

    };
    PrintCollection(listOfRectangle);

    void PrintCollection(ICollection collection)
    {
        Console.WriteLine(collection);
        foreach (var item in collection)
        {
            Console.WriteLine(item);
        }
        Console.WriteLine();
    }
}

//InitialiazationCollection();

void CollectionToConsole(ICollection collection)
{
    Console.WriteLine(collection);
    Console.WriteLine($"Count:{collection.Count}\n");
    // Enumerate over collection ICollection : IEnumerable
    int index = 0;
    foreach (var item in collection)
    {
        Console.Write($"\t{index}.\t");
        Console.WriteLine(item);
        index++;
    }
}

void ListToConsole<T>(List<T> list)
{
    Console.WriteLine();
    Console.WriteLine(list);
    Console.WriteLine($"Count:{list.Count}");
    Console.WriteLine($"Capacity:{list.Capacity}\n"  );
    // Enumerate over collection ICollection : IEnumerable
    int index = 0;
    foreach (var item in list)
    {
        Console.Write($"\t{index}.\t");
        Console.WriteLine(item);
        index++;
    }
}


void UseGenericList()
{
    // Make a List of personages
    List<Person> personages = new()
    {
        new("Tomy","Stark",40),
        new("Sara","Connor",30),
        new("Sherlock","Holms",50),
    };
    // Print out
    ListToConsole(personages);

    //Add
    Person rembo = new("John", "Rembo", 30);
    personages.Add(rembo);

    // Insert new item
    Person bond = new("James", "Bond", 40);
    personages.Insert(2,bond);

    ListToConsole(personages);

    //Remove
    personages.Remove(bond);
    personages.Remove(rembo);

    ListToConsole(personages);

    // To array 
    Person[] arrayPersonages = personages.ToArray();

    //Array : ICollection
    CollectionToConsole(arrayPersonages);

    
}
//UseGenericList();

void UseGenericStack()
{
    Stack<Person> personages = new();

    personages.Push(new("Tomy", "Stark", 40));
    CollectionToConsole(personages);

    personages.Push(new("Sara", "Connor", 30));
    CollectionToConsole(personages);

    personages.Push(new("John", "Rembo", 30));
    CollectionToConsole(personages);


    Person person = personages.Peek();
    Console.WriteLine($"\n{person}\n");
    CollectionToConsole(personages);

    person = personages.Pop();
    Console.WriteLine($"\n{person}\n");
    CollectionToConsole(personages);

    Console.WriteLine($"\n{personages.Pop()}\n");
    Console.WriteLine($"\n{personages.Pop()}\n");
    Console.WriteLine($"\n{personages.Pop()}");
}

//UseGenericStack();

void UseGenericStackWithCheck()
{
    Person[] persons = 
    {
        new("Tomy", "Stark", 40),
        new("Sara", "Connor", 30),
        new("John", "Rembo", 30)
    };

    Stack<Person> personages = new(persons);
    CollectionToConsole(personages);

    Console.WriteLine();

    while (personages.TryPop(out Person? person))
    {
        Console.WriteLine(person);
    }
}

//UseGenericStackWithCheck();

void UseGenericQueue()
{
    Queue<Person> sitizens = new();

    // queuing
    sitizens.Enqueue(new("Julia", "Firstenko", 25));
    sitizens.Enqueue(new("Evgeniy", "Secandenko", 28));
    sitizens.Enqueue(new("Nikolaj","Thirdenko",42));
    sitizens.Enqueue(new("Pavel", "Fourtinenko", 32));

    CollectionToConsole(sitizens);

    // peek first 
    Person person = sitizens.Peek();
    Console.WriteLine($"\nWork Peek: {person}\n"  );

    CollectionToConsole(sitizens);
    
    // dequing
    person = sitizens.Dequeue();
    Console.WriteLine($"\nWork Dequeue: {person}\n");

    CollectionToConsole(sitizens);
    
    Console.WriteLine();

    while (sitizens.TryDequeue(out Person? sitizen))
    {
        Console.WriteLine(sitizen);
    }

}

//UseGenericQueue();

void UseGenericPriorityQueue()
{
    PriorityQueue<Person, int> personages = new();

    personages.Enqueue(new("John", "Rembo", 30), 2);
    personages.Enqueue(new("John", "Connor", 15), 1);
    personages.Enqueue(new("Tomy", "Stark", 35), 3);
    personages.Enqueue(new("Sara", "Connor", 35), 2);
    personages.Enqueue(new("T", "800", 75),2);

    while(personages.Count > 0)
    {
        Console.WriteLine(personages.Dequeue());
    }
}

//UseGenericPriorityQueue();

void UseGenericSortedSet()
{
    SortedSet<Person> personages = new(new PersonComparerByAge())
    {
        new("Tomy","Stark",40),
        new("Sara","Connor",30),
        new("Sherlock","Holms",50),
    };

    CollectionToConsole(personages);

    //Add
    Person johnConnor = new("John", "Connor", 15);
    bool result = personages.Add(johnConnor);
    Console.WriteLine($"\nAdded {johnConnor} successfully? : {result}");
    CollectionToConsole(personages);

    //Add
    Person rembo = new("John", "Rembo", 30);
    result = personages.Add(rembo);
    Console.WriteLine($"\nAdded {rembo} successfully? : {result}");
    CollectionToConsole(personages);

    //Remove
    Person saraConnor = new("Sara", "Connor", 30);
    result = personages.Remove(saraConnor);
    Console.WriteLine($"\nRemoved {saraConnor} successfully? : {result}");
    CollectionToConsole(personages);

}

//UseGenericSortedSet();

void UseGenericDictionary()
{

    //Initialization
    Dictionary<string, Person> personages = new() 
    {
        {"IronMan", new("Tony", "Stark", 35) },
        {"SpiderMan", new("Peter", "Parker", 25)}
    };
    CollectionToConsole(personages);
    
    //Get
    Console.WriteLine($"\n{personages["IronMan"]}\n");

    //Add
    Person rembo = new("John", "Rembo", 30);
    personages.Add("Rembo",rembo);
    CollectionToConsole(personages);

    //TryAdd
    Person _007 = new("James", "Bond", 35);
    bool result = personages.TryAdd("007",_007);
    Console.WriteLine($"\nAdded {_007} successfully? : {result}");
    CollectionToConsole(personages);

    //Remove
    personages.Remove("SpiderMan");
    CollectionToConsole(personages);
}

//UseGenericDictionary();

void UseOtherInizialaserGenericDictionary()
{
    Dictionary<string, Person> personages = new()
    {
        ["IronMan"] = new("Tony", "Stark", 35) ,
        ["Rembo"] = new("John", "Rembo", 30),
        ["SpiderMan"] = new("Peter", "Parker", 25)
    };
    CollectionToConsole(personages);
}

UseOtherInizialaserGenericDictionary();