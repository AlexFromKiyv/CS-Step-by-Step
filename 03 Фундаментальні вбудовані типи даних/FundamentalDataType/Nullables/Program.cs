//CrashWithNull();
static void CrashWithNull()
{

    Person girl = new(); 

    AgePlusOne(girl);//Ok 
    girl.Display();  //Ok  Name: Age:1 IsNull:False

    Person boy = null; //War: Converting null to no-nullable type. // Start problem
    AgePlusOne(boy); //War: may be null here  // Сontinuation of the problem

    Person maneger = new();
    maneger.Name = null; //War: Converting null to no-nullable type. // Start problem
    maneger.Name.ToUpper();//War: may be null here  // Exeption thrown Name == null

    static void AgePlusOne(Person p)
    {
        p.Age++; // Exeption thrown if p == null
    }
}



//AssignNull();

static void AssignNull()
{
    //int weight = null; // don't work //Cannot convert null to int because it is not-nullable value type.
    //string title = null; // work, but has warning

    string title = "User:";

    int? age;      //nullable
    bool? married; //

    age = null;     // no problem
    married = null; //


    age = GetAgeFromDB();
    married = GetMarriedFromDB();
    
    Console.WriteLine($"{title} {age} {married}");


    static bool? GetMarriedFromDB()
    {
        return null;
    }

    static int? GetAgeFromDB()
    {
        return null;
    }
}

//StructSystemNullable();
static void StructSystemNullable()
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

//UsingNullablesValueType();
static void UsingNullablesValueType()
{
    
    UserDatabaseSimulator girlJulia = new UserDatabaseSimulator(1, "Julia");
    GetUserInfo(girlJulia);

    UserDatabaseSimulator girlHanna = new UserDatabaseSimulator(2, "Hanna",true,35);
    GetUserInfo(girlHanna);

    UserDatabaseSimulator boyAlex = new UserDatabaseSimulator(3, "Alex",null, 30);
    GetUserInfo(boyAlex);

    UserDatabaseSimulator boyJohn = new UserDatabaseSimulator(4, "Jhon",true);
    GetUserInfo(boyJohn);



    void GetUserInfo(UserDatabaseSimulator user)
    {

        string result = $"Id:{user.id} Name:{user.name} ";

        bool? merried = user.GetMerried();


        if (merried.HasValue)
        {
            result +=$"Merried:{merried.Value} ";
        }
        else
        {
            result += "Merried: undefined ";
        }

        int? age = user.GetAge();

        if (age != null)
        {
           result +=$"Age:{age}";
        }
        else
        {
            result += "Age: undefined";
        }

        Console.WriteLine(result);
    }
}

//UsingNullableReferenceType();
static void UsingNullableReferenceType()
{

    Person? girl;

    girl = GetPersonFromDb(IsItDefinet: true);
    GetPersonData(girl);

    girl = GetPersonFromDb(IsItDefinet: false);
    GetPersonData(girl);


    static void GetPersonData(Person? person)
    {
        if(person != null)
        {
            person.Display();
        }
        else
        {
            Console.WriteLine("Person undefined");
        }
        
    }

    static Person? GetPersonFromDb(bool IsItDefinet)
    {
        return IsItDefinet ? new Person("Someone",30) : null;
    }

}


//UsingNullCoalescing();
static void UsingNullCoalescing()
{
    UserDatabaseSimulator girl = new UserDatabaseSimulator(1, "Julia");
    Console.WriteLine($"Is age null: {girl.age == null}");
    int? girlAge;

    //Code without ??
    girlAge = girl.age;
    if (!girl.age.HasValue)
    {
        girlAge = 35;
    }
    Console.WriteLine(girlAge);


    //With operator ??
    girlAge = girl.age ?? 35;
    Console.WriteLine(girlAge);


    //Operator ??=
    girlAge ??= 85;
    Console.WriteLine(girlAge);
}


//UsingNullConditional();

static void UsingNullConditional()
{
    //Exapmle1

    ArrayLength(null);
    ArrayLength(new string[] { "good", "better", "best" });

    static void ArrayLength(string[]? args)
    {
        //Without operator ?
        if (args != null)
        {
            Console.WriteLine(args.Length);
        }
        else
        {
            Console.WriteLine(0);
        }

        //With operator ?
        Console.WriteLine(args?.Length ?? 0);

    }


    //Example2

    Person? boy;

    boy = null;
    Action(boy);

    boy = new Person("John",30);
    Action(boy);    


    static void Action(Person? person)
    {
        //Without operator ?
        if (person != null)
        {
            Console.WriteLine(person.Name);
        }

        //With operator ?
        Console.WriteLine(person?.Name);

        person?.Display();
    }
}

ValidationOfFunctionArguments();
void ValidationOfFunctionArguments()
{
    try
    {
        //AddSum1(null);
        AddSum2(null);

    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }


    // Variant 1
    void AddSum1(string account, decimal sum = 0)
    {
        if (account is null)
        {
            throw new ArgumentNullException(nameof(account));
        }
        // the rest code
    }

    // Variant 2
    void AddSum2(string account, decimal sum = 0)
    {
        ArgumentException.ThrowIfNullOrEmpty(account);
        // the rest code

    }
}



// Classes
class Person
{
    public string? Name { get; set; }
    public int Age { get; set; }

    public Person()
    {
    }

    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public void Display() => Console.WriteLine($"Name:{Name} Age:{Age}");
}


class UserDatabaseSimulator
{
    public int id;
    public string name; 
    public bool? merried;
    public int? age;

    public UserDatabaseSimulator(int id, string name, bool? merried = null, int? age=null)
    {
        this.id = id;
        this.name = name;
        this.merried = merried;
        this.age = age;
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
