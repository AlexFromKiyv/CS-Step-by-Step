using System.Text;

LogicalExpression();

static void LogicalExpression()
{
    int weight = 70;

    Console.WriteLine($"weight = {weight}");
    Console.WriteLine($"weight == 70 : {weight == 70}");
    Console.WriteLine($"weight != 70 : {weight != 70}");
    Console.WriteLine($"weight > 70  : {weight > 70}");
    Console.WriteLine($"weight < 70  : {weight < 70}");
    Console.WriteLine($"weight >= 70  : {weight >= 70}");
    Console.WriteLine($"weight <= 70  : {weight <= 70}");
    
    Console.WriteLine("------------------------------");

    Console.WriteLine($" true && true   : {true && true }");
    Console.WriteLine($" true && false  : {true && false}");
    Console.WriteLine($" false && true  : {false && true}");
    Console.WriteLine($" false && false : {false && false}");

    Console.WriteLine("------------------------------");

    Console.WriteLine($" true || true   : {true || true}");
    Console.WriteLine($" true || false  : {true || false}");
    Console.WriteLine($" false || true  : {false || true}");
    Console.WriteLine($" false || false : {false || false}");

    Console.WriteLine("------------------------------");

    Console.WriteLine($" !true  : {!true}");
    Console.WriteLine($" !false : {!false}");

    Console.WriteLine($"false || true && true || true && false || true : {false || true && true || true && false || true }");
  


}





//SimpleIf();

static void SimpleIf()
{
    bool logicalExpression = true;
    if (logicalExpression)
    {
        Console.WriteLine("Logical expression is true");
    }

}


//SimpleIfElse();

static void SimpleIfElse()
{
    bool logicalExpression = false;
    if (logicalExpression)
    {
        Console.WriteLine("Logical expression is true");
    }
    else
    {
        Console.WriteLine("Logical expression is false");
    }

}

//UsingIs();
static void UsingIs()
{
    string myVariable1 = "Hi";

    if (myVariable1 is string newString)
    {
        Console.WriteLine(newString);
    }
    
    int myVariable2 = 70;
    if (myVariable2 is int newInt)
    {
        Console.WriteLine(newInt);
    }
}


//UsingTypePattern();
static void UsingTypePattern()
{
    Type myType = typeof(short);

    if(myType is Type)
    {
        Console.WriteLine($"{myType} is type.");
    }

}

//UsingParenthesizedPattern();
static void UsingParenthesizedPattern()
{
    char myChar = 'y';

    if (myChar is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z') or '.' or ',' or ';')
    {
        Console.WriteLine($"{myChar} is a character or separator");
    }
}

//UsingRelationalConjuctiveDisjunctivePattern();
static void UsingRelationalConjuctiveDisjunctivePattern()
{
    char myChar = 'y';

    if (myChar is >= 'a' and <= 'z' or >= 'A' and <= 'Z')
    {
        Console.WriteLine($"{myChar} is a character");
    };
}

//UsingNegativePattern();
static void UsingNegativePattern()
{
    object myObject = 0;
    if (myObject is not string) 
    {
        Console.WriteLine($"{myObject} not string");
    }

    if (myObject is not null)
    {
        Console.WriteLine($"{myObject} not null");
    }
}

//UsingConditionalOperator();

static void UsingConditionalOperator()
{
    int weight = 95;

    string result = (weight < 72) ? "It's good" : "It's no good";

    Console.WriteLine(result);

    // do not work
    //(weight < 72) ? Console.WriteLine("It's good") : Console.WriteLine("It's no good");
}


//UsingConditionalOperatorWithRef();

static void UsingConditionalOperatorWithRef()
{
    int[] smallArray = new int[] { 1, 2, 3, 4, 5 };
    int[] largeArray = new int[] { 10, 20, 30, 40, 50, 60, 70 };

    int index = 7;
    ref int refValue = ref ((index < 5) ? ref smallArray[index] : ref largeArray[index - 5]);
    refValue = 0;

    Console.WriteLine(string.Join(" ",largeArray));

    index = 2;
    ((index < 5) ? ref smallArray[index] : ref largeArray[index - 5]) = 100;
    Console.WriteLine(string.Join(" ", smallArray));

}