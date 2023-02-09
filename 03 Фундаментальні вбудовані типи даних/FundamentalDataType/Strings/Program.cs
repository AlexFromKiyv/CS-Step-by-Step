//ExplorationOfStrings_1();

using System.Text;

static void ExplorationOfStrings_1()
{
    string myString = "Hi girl!";

    Console.WriteLine(myString);
    Console.WriteLine($"String is ValueType: {myString is ValueType}");
    Console.WriteLine($"Length:{myString.Length}");
    Console.WriteLine($"Compare with \"HI girl!\":{string.Compare(myString,"HI girl!")}");
    Console.WriteLine($"Contains \"girl\":{myString.Contains("girl")}");
    Console.WriteLine($"To uppper:{myString.ToUpper()}");
    Console.WriteLine($"Replace \" girl!\":{myString.Replace(" girl!"," !")}");
    Console.WriteLine(myString);
}

//Interpolation();
static void Interpolation()
{
    string name = "Julia";
    int weight = 65;

    string myString = string.Format("Name:{0} Weight:{1}",name,weight);
    Console.WriteLine(myString);

    myString = $"Name:{name} Weight:{weight}";
    Console.WriteLine(myString);

    myString = $"Name:{name.ToUpper()} Weight:{weight+=3}";
    Console.WriteLine(myString);
}

//Concatination();
static void Concatination()
{
    string myString1 = "Hi";
    string myString2 = "everybody";
    string myString3 = myString1 + " " + myString2;
    myString3 += "!";
    Console.WriteLine(myString3);
}

//Escapes();
static void Escapes()
{
    Console.WriteLine("Code\tName\tPrice");
    Console.WriteLine("D:\\Documents\\template.doc");
    Console.WriteLine("Text\n\n");
    Console.WriteLine("Text{0}{0}",Environment.NewLine);
    Console.WriteLine("\"New text\"");
    Console.WriteLine("\a");
}

//Verbatim();

static void Verbatim()
{
    string myString = @"D:\Documents\";
    Console.WriteLine(myString);

    myString =@"How      
    are
       you?";
    Console.WriteLine(myString);
}

//StringsComparison();

static void StringsComparison()
{
    string string1 = "Hi";
    string string2 = "HI";
    Console.WriteLine($"string1:{string1} string2:{string2}");

    Console.WriteLine($" string1 == string2 {string1 == string2} ");
    Console.WriteLine($" string1 == \"Hi\"  {string1 == "Hi"}");
    Console.WriteLine($" string1 == \"HI\"  {string1 == "HI"}");
    Console.WriteLine($" string1 == \"hi\"  {string1 == "HI"}");
    Console.WriteLine($" Hi.Equals(string1) {"Hi".Equals(string1)}");
    Console.WriteLine($" string1.Equals(string2) {string1.Equals(string2)}");
}

//ChangeStringsBeforeComparison();

static void ChangeStringsBeforeComparison()
{
    string myString = "MEN";
    string enteredString = "men";

    Console.WriteLine(myString.ToUpper() == enteredString.ToUpper());
}

//ComparationWithCustomize();

static void ComparationWithCustomize()
{
    string s1 = "girl";
    string s2 = "GIRL";

    Console.WriteLine($"s1:{s1} s2:{s2} \n\r");
    Console.WriteLine($"s1.Equals(s2) : {s1.Equals(s2)}");
    Console.WriteLine($"s1.Equals(s2,StringComparison.OrdinalIgnoreCase) : {s1.Equals(s2,StringComparison.OrdinalIgnoreCase)}");
    Console.WriteLine($"s1.Equals(s2,StringComparison.InvariantCultureIgnoreCase) : {s1.Equals(s2, StringComparison.InvariantCultureIgnoreCase)}");

    Console.WriteLine($"s1.Equals(s2, StringComparison.OrdinalIgnoreCase): {s1.Equals(s2, StringComparison.OrdinalIgnoreCase)}");
    Console.WriteLine($"s1.Equals(s2, StringComparison.InvariantCultureIgnoreCase): {s1.Equals(s2, StringComparison.InvariantCultureIgnoreCase)}");
    Console.WriteLine();
    Console.WriteLine($"s1.IndexOf(\"I\"): {s1.IndexOf("I")}");
    Console.WriteLine($"s1.IndexOf(\"I\",StringComparison.OrdinalIgnoreCase)}}: {s1.IndexOf("I",StringComparison.OrdinalIgnoreCase)}");
    Console.WriteLine($"s1.IndexOf(\"I\",StringComparison.InvariantCultureIgnoreCase)}}: {s1.IndexOf("I", StringComparison.InvariantCultureIgnoreCase)}");
}


//StringInHeap();


static void StringInHeap()
{
    string myString = "Hi girl!"; // first object in heap
    Console.WriteLine(myString);

    Console.WriteLine(myString.ToUpper()); // second object in heap
    Console.WriteLine(myString);

    myString = "Hi"; // third object in heap
    Console.WriteLine(myString);
}

UsingStringBuilder();

static void UsingStringBuilder()
{
    StringBuilder mySB = new StringBuilder("Product list:",256);
    mySB.Append(Environment.NewLine);
    mySB.AppendLine("Apple");
    mySB.AppendLine("Garlic");
    mySB.AppendLine("Tomato");
    mySB.AppendLine("Bread");
    mySB.AppendLine("Milk");
    mySB.Replace("Milk", "Kefir");
    Console.WriteLine(mySB);
    Console.WriteLine(mySB.Length);

    
}