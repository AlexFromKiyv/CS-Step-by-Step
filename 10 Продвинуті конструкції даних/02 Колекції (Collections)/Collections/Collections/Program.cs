using Collections;
using System.Collections;
using System.Collections.Specialized;
using System.Net.Http.Headers;

void UsingArray()
{
    string[] strings = { "First", "Second", "Third" };

    ArrayToConsole(strings);

    Array.Reverse(strings);

    ArrayToConsole(strings);


    void ArrayToConsole(string[] array)
    {
        Console.WriteLine();
        Console.WriteLine($"Type:{array}"  );
        Console.WriteLine($"Length:{array.Length}");
        foreach (string item in array)
        {
            Console.WriteLine("\t"+item);
        }
    }
}

//UsingArray();

void ArrayHasFixedSize()
{
    string[] strings = { "First", "Second", "Third" };

    strings[4] ="1"; 
    //System.IndexOutOfRangeException:Index was outside the bounds of the array.
    strings[5] ="2";

    Console.WriteLine(strings[5]);
}

//ArrayHasFixedSize();


void UsingArrayList()
{
    ArrayList arrayList = new() {"First","Second","Third","4","5"};
    arrayList.Add(4);
    arrayList.Add(5);
    arrayList.Remove("4");
    arrayList.RemoveAt(3);
    ArrayListToConsole(arrayList);

    Console.WriteLine();
    arrayList.Clear();

    string[] stringArray = { "First", "Second", "Third", "4", "5" };
    arrayList.AddRange(stringArray);
    arrayList.Add(6);
    arrayList.AddRange(new string[] { "7", "Julia"});
    DateTime dateTime = DateTime.Now;
    arrayList.Add(dateTime.DayOfWeek);

    ArrayListToConsole(arrayList);

    arrayList.RemoveRange(0, 5);

    ArrayListToConsole(arrayList);


    void ArrayListToConsole(ArrayList arrayList)
    {
        Console.WriteLine($"Count:{arrayList.Count}");
        Console.WriteLine($"Capacity:{arrayList.Capacity}");
        foreach (var item in arrayList)
        {
            ObjectToConsole(item);
        }
    }

    void ObjectToConsole(object obj)
    {
        Console.WriteLine($"\t{obj}\t{obj.GetType()}");
    }
}

//UsingArrayList();

void BoxingAndUnboxing()
{
    int valueTypeVariable = 10;
    Console.WriteLine(valueTypeVariable.GetType());

    //boxing
    object referenceToValueTypeVariable = valueTypeVariable;
    Console.WriteLine(referenceToValueTypeVariable.GetType());
    
    //unboxing
    int unboxedInt = (int)referenceToValueTypeVariable;
    Console.WriteLine(unboxedInt.GetType());

    try
    {
        //do not work
        long unboxingLong = (long)referenceToValueTypeVariable; 
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

//BoxingAndUnboxing();

void UsingSystemObjectInArrayList()
{
    //boxing
    ArrayList myInts = new();
    myInts.Add(10);
    myInts.Add(20);
    myInts.Add(30);

    //unboxing
    int? i = (int?) myInts[0];

    Console.WriteLine(i);
}

//UsingSystemObjectInArrayList();

void NoSafetyUsingArrayList()
{
    ArrayList myArray = new();

    myArray.Add(1);
    myArray.Add(true);
    myArray.Add(new OperatingSystem(PlatformID.Unix,new Version()));
    myArray.Add(new DateTime());

    foreach (int item in myArray)
    {
        int i = item;
    }
}

//NoSafetyUsingArrayList();


void UsePersonCollection()
{
    
    PersonCollectiom personages = new PersonCollectiom();
    personages.Add(new("Lara", "Croft", 50));
    personages.Add(new("Slerlock", "Holmes", 40));
    personages.Add(new("Sara", "Connor", 35));
    personages.Add(new("Tony", "Stark", 40));
    personages.Add(new("Захар", "Беркут", 40));

    // personages.Add((new DateTime()); // cannot convert System.DateTime to Collections.Person

    foreach (Person itemPerson in personages)
    {
        Console.WriteLine(itemPerson);
    }
}

//UsePersonCollection();


void UseGenericList()
{
    List<Person> personages = new();
    personages.Add(new("Lara", "Croft", 50));
    personages.Add(new("Slerlock", "Holmes", 40));
    personages.Add(new("Sara", "Connor", 35));
    personages.Add(new("Tony", "Stark", 40));
    personages.Add(new("Захар", "Беркут", 40));

    // personages.Add((new DateTime()); // cannot convert System.DateTime to Collections.Person

    Console.WriteLine(personages[1]);

    Console.WriteLine();

    List<int> ints = new();

    ints.Add(10);
    ints.Add(20);
    ints.Add(30);

    Console.WriteLine(ints[1] + ints[2]);
}

UseGenericList();
