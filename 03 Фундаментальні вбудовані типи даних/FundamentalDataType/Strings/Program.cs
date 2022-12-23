//ExplorationOfStrings_1();

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

StringComparison();

static void StringComparison()
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