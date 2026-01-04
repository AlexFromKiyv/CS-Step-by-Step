
using Indexers;
using System.Data;

static void UsingArray()
{
    string[] args = Environment.GetCommandLineArgs();
    // Loop over incoming command-line arguments
    // using index operator.
    for (int i = 0; i < args.Length; i++)
    {
        Console.WriteLine($"Args: {args[i]}");
    }
    Console.WriteLine();

    // Declare an array of local integers.
    int[] myInts = { 10, 9, 100, 432, 9874 };
    // Use the index operator to access each element.
    for (int j = 0; j < myInts.Length; j++)
    {
        Console.WriteLine($"Index {j}  = {myInts[j]}");
    }
}
//UsingArray();


static void UsingPersonCollection()
{
    PersonCollection myPeople = new PersonCollection();
    // Add objects with indexer syntax.
    myPeople[0] = new Person ("Homer", "Simpson", 40);
    myPeople[1] = new Person("Marge", "Simpson", 38);
    myPeople[2] = new Person("Lisa", "Simpson", 9);
    myPeople[3] = new Person("Bart", "Simpson", 7);
    myPeople[4] = new Person("Maggie", "Simpson", 2);

    // Now obtain and display each item using indexer.
    for (int i = 0; i < myPeople.Count; i++)
    {
        Console.WriteLine($"{i}\t{myPeople[i]}");
    }
    Console.WriteLine($"\nCount of people :{myPeople.Count}");
}
//UsingPersonCollection();

static void UsingGenericListOfPeople()
{
    List<Person> myPeople = new List<Person>();
    myPeople.Add(new Person("Lisa", "Simpson", 9));
    myPeople.Add(new Person("Bart", "Simpson", 7));

    // Change first person with indexer.
    myPeople[0] = new Person("Maggie", "Simpson", 2);

    // Now obtain and display each item using indexer.
    for (int i = 0; i < myPeople.Count; i++)
    {
        Console.WriteLine($"{i}\t{myPeople[i]}");
    }
}
//UsingGenericListOfPeople();

static void UsingPersonCollectionStringIndexer()
{
    PersonCollectionStringIndexer people = new();
    people["Homer"] = new Person("Homer", "Simpson", 40);
    people["Marge"] = new Person("Marge", "Simpson", 38);

    Console.WriteLine(people["Homer"]);

    foreach (var person in people)
    {
        Console.WriteLine(person);
    }

}
//UsingPersonCollectionStringIndexer();

static void MultiIndexerWithDataTable()
{
    // Make a simple DataTable with 3 columns.
    DataTable myTable = new DataTable();
    myTable.Columns.Add(new DataColumn("FirstName"));
    myTable.Columns.Add(new DataColumn("LastName"));
    myTable.Columns.Add(new DataColumn("Age"));
    // Now add a row to the table.
    myTable.Rows.Add("Mel", "Appleby", 60);
    // Use multidimension indexer to get details of first row.
    Console.WriteLine($"First Name: {myTable.Rows[0][0]}");
    Console.WriteLine($"Last Name: {myTable.Rows[0][1]}");
    Console.WriteLine($"Age : {myTable.Rows[0][2]}");
}
//MultiIndexerWithDataTable();