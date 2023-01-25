//AssignNull();

static void AssignNull()
{
    //int age = null; //not-nullable //Cannot convert null to int because it is not-nullable value type.

    int? age; //nullable
    bool? married; //nullable

    age = GetAgeFromDB();
    married = GetMarriedFromDB();
    
    Console.WriteLine($"{age} {married}");


    static bool? GetMarriedFromDB()
    {
        return null;
    }

    static int? GetAgeFromDB()
    {
        return null;
    }
}

//UsingSystemNullable();
static void UsingSystemNullable()
{

    Nullable<bool> merried = null; 
    Nullable<int> age = null;
    Console.WriteLine("merried = null, age = null");
    Console.WriteLine($"merried.HasValue : {merried.HasValue}");
    Console.WriteLine($"age.GetValueOrDefault() : {age.GetValueOrDefault()}");
    Console.WriteLine($"merried is ValueType : {merried is ValueType}");
    Console.WriteLine($"age is ValueType : {age is ValueType}");

    merried = true;
    age = 25;

    Console.WriteLine("\nAfter: merried = true, age=25");
    Console.WriteLine($"merried.HasValue : {merried.HasValue}");
    Console.WriteLine($"age.GetValueOrDefault() : {age.GetValueOrDefault()}");
    Console.WriteLine($"merried is ValueType : {merried is ValueType}");
    Console.WriteLine($"age is ValueType : {age is ValueType}");
    Console.WriteLine($"merried.Value : {merried.Value}");
    Console.WriteLine($"age.Value : {age.Value}");


    //Nullable<Person> person = null; // Person must be non-nullable value type.
}

UsingNullables();
static void UsingNullables()
{
    UserDatabaseSimulator user = new UserDatabaseSimulator(1, "Julia");

    int? age = user.GetAge();

    if (age.HasValue)
    {
        Console.WriteLine(GetUserInfo(user)+age.ToString());   
    }
    else
    {
        Console.WriteLine(GetUserInfo(user)+"age is undefined");     
    }

    bool? merried = user.GetMerried();

    if (merried != null)
    {
        Console.WriteLine(GetUserInfo(user) + $"Merried:{merried.Value}");
    }
    else
    {
        Console.WriteLine(GetUserInfo(user) + "merried is undefined");
    }
    static string GetUserInfo(UserDatabaseSimulator user) => $"{user.id} {user.name} ";
}

class Person
{
    public string Name { get; set; }
}

class UserDatabaseSimulator
{
    public int id;
    public string name; 
    public bool? merried = true;
    public int? age = null;

    public UserDatabaseSimulator(int id, string name)
    {
        this.id = id;
        this.name = name;
    }

    public bool? GetMerried()
    {
        return merried;
    }

    public int? GetAge()
    {
        return age;
    }
}