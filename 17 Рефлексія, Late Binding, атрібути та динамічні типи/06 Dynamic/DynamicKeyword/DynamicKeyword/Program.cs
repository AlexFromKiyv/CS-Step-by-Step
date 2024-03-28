using DynamicKeyword;

static void InvestigationImplicitlyTypedValue()
{
    var list = new List<string> { "20" };

    //list = "10"; //Cannot implicitly convert type "string" to ... 
}


static void UseObjectVariable()
{
    // Create a new instance of the Person class
    // and assign it to a variable of type System.Object
    object p = new Person() { FirstName = "Antoni", LastName = "Gaudi" };

    // Must cast object as Person to gain access
    // to the Person properties.
    Console.WriteLine($"{((Person)p).FirstName} {((Person)p).LastName}");
}
//UseObjectVariable();

static void TreeString()
{
    var str1 = "Go";
    object str2 = "to";
    dynamic str3 = "Home";

    Console.WriteLine(str1.GetType());
    Console.WriteLine(str2.GetType());
    Console.WriteLine(str3.GetType());
}
//TreeString();

static void UseDynamic()
{
    dynamic val;
    //object val; 
        
    val = "Hi";
    Console.WriteLine(val+" "+val.GetType());

    val = false;
    Console.WriteLine(val + " " + val.GetType());

    val = new List<int> { 1, 2, 3 };
    Console.WriteLine(val.GetType());

}
//UseDynamic();

static void UseMemeberOfDynamicVariable()
{
    dynamic str = "hi";
    Console.WriteLine(str.ToUpper());

    try
    {
        Console.WriteLine(str.DoIt());
        Console.WriteLine(str.toupper());
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);    
    }
}
//UseMemeberOfDynamicVariable();

static void UseVeryDynamic()
{
    VeryDynamic veryDynamic = new();

    dynamic result =  veryDynamic.Method(10);
    Write(result);

    result = veryDynamic.Method("10");
    Write(result);

    static void Write(dynamic value)
    {
        Console.WriteLine(value + " " + value.GetType());
    }
}
UseVeryDynamic();