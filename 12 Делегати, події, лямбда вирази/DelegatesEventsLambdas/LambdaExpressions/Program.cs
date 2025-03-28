
using LambdaExpressions;

static void TraditionalDelegateSyntax()
{
    List<int> ints = [20, 1, 4, 8, 9, 44];

    // Call FindAll() using traditional delegate syntax.
    Predicate<int> callback = IsEvenNumber;
    List<int> evenNumbers = ints.FindAll(callback);

    foreach (var number in evenNumbers)
    {
        Console.Write(number+"\t");
    }

    static bool IsEvenNumber(int i)
    {
        return (i % 2 == 0);
    }
}
//TraditionalDelegateSyntax();


static void AnonymousMethodSyntax()
{
    List<int> ints = [20, 1, 4, 8, 9, 44];

    // Now, use an anonymous method.
    List<int> evenNumbers = ints.FindAll(delegate (int i) { return (i % 2 == 0);});

    foreach (var number in evenNumbers)
    {
        Console.Write(number + "\t");
    }

}
//AnonymousMethodSyntax();

static void LambdaExpressionSyntax()
{
    List<int> ints = [20, 1, 4, 8, 9, 44];

    // Now, use a C# lambda expression.
    List<int> evenNumbers = ints.FindAll(i => (i % 2) == 0);

    foreach (var number in evenNumbers)
    {
        Console.Write(number + "\t");
    }

}
//LambdaExpressionSyntax();

static void LambdaExpressionSyntaxWithMultipleStatements()
{
    List<int> ints = [20, 1, 4, 8, 9, 44];

    // Now process each argument within a group of
    // code statements.
    List<int> evenNumbers = ints.FindAll(i =>
    {
        bool isEven = (i % 2 == 0);
        Console.WriteLine($"Value i ={i}. Is even?: {isEven}");
        return isEven;
    });
    Console.WriteLine();

    foreach (var number in evenNumbers)
    {
        Console.Write(number + "\t");
    }
}
//LambdaExpressionSyntaxWithMultipleStatements();

static void LambdaWithMultipleParameters()
{
    SimpleMath simpleMath = new();

    // Register with delegate as a lambda expression.
    simpleMath.SetMathHandler((message, result) => 
    {
        Console.WriteLine($"Message:{message} Result:{result}");
    });

    simpleMath.Add(5, 3);

}
//LambdaWithMultipleParameters();

static void LambdaWithoutParameters()
{
    VerySimpleDelegate simpleDelegate = new(() => "Enjoy your string!");

    // or

    VerySimpleDelegate simpleDelegate1 = () => "Enjoy your string!";

    Console.WriteLine(simpleDelegate());
    Console.WriteLine(simpleDelegate1());

}
//LambdaWithoutParameters();

void OtherLambdа()
{
    var outerVariable = 0;

    Func<int, int, bool> DoWork = (_, _) =>
    {
        //Compile error since it’s accessing an outer variable
        outerVariable++;
        return true;
    };

    DoWork(3,3);
    Console.WriteLine($"Outer variable now = {outerVariable}");
}
//OtherLambdа();



public delegate string VerySimpleDelegate();