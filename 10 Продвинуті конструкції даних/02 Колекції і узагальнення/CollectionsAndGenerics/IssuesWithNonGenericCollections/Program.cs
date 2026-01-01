using IssuesWithNonGenericCollections;
using System.Collections;

static void SimpleBoxUnboxOperation()
{
    // Make a ValueType (int) variable.
    int myInt = 25;
    // Box the int into an object reference.
    object boxedInt = myInt;
    // Unbox in the wrong data type to trigger
    // runtime exception.
    try
    {
        long unboxedLong = (long)boxedInt;
    }
    catch (InvalidCastException ex)
    {
        Console.WriteLine(ex.Message);
    }
}
//SimpleBoxUnboxOperation();

static void WorkWithArrayList()
{
    // Value types are automatically boxed when
    // passed to a method requesting an object.
    ArrayList myInts = new ArrayList();
    myInts.Add(10);
    myInts.Add(20);
    myInts.Add(35);

    // Unboxing occurs when an object is converted back to
    // stack-based data.
    int i = (int)myInts[0];
    // Now it is reboxed, as WriteLine() requires object types!
    Console.WriteLine($"Value of your int: {i}");
}
//WorkWithArrayList();

static void UsePersonCollection()
{
    PersonCollection myPeople = new PersonCollection();
    myPeople.AddPerson(new Person("Homer", "Simpson", 40));
    myPeople.AddPerson(new Person("Marge", "Simpson", 38));
    myPeople.AddPerson(new Person("Lisa", "Simpson", 9));
    myPeople.AddPerson(new Person("Bart", "Simpson", 7));
    myPeople.AddPerson(new Person("Maggie", "Simpson", 2));

    // This would be a compile-time error!
    // myPeople.AddPerson(new Car());

    Console.WriteLine(myPeople.GetPerson(1));
    Console.WriteLine();

    foreach (Person p in myPeople)
    {
        Console.WriteLine(p);
    }
}
//UsePersonCollection();

static void UseGenericList()
{
    // This List<> can hold only Person objects.
    List<Person> morePeople = new List<Person>();
    morePeople.Add(new Person("Frank", "Black", 50));
    Console.WriteLine(morePeople[0]);

    // This List<> can hold only integers.
    List<int> moreInts = new List<int>();
    moreInts.Add(10);
    moreInts.Add(2);
    int sum = moreInts[0] + moreInts[1];
    Console.WriteLine(sum);

    // Compile-time error! Can't add Person object
    // to a list of ints!
    //moreInts.Add(new Person());
}
//UseGenericList();