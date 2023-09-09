
using IntroducionToLambdaExpressions;

void ExampleWithoutLambdaExpression()
{
    List<int> ints = new List<int>();
    ints.AddRange(new int[] {1,12,3,2,4,29,48,4,5});

    Predicate<int> predicate = IsEvenNumber;

    List<int> evenNumbers = ints.FindAll(predicate);

    foreach (var item in evenNumbers)
    {
        Console.Write($"{item}\t");
    }

    static bool IsEvenNumber(int x)
    {
        return (x%2) == 0;
    }
}

//ExampleWithoutLambdaExpression();

void ExampleWithAnonymousMethod1()
{
    List<int> ints = new List<int>();
    ints.AddRange(new int[] { 1, 12, 3, 2, 4, 29, 48, 4, 5 });

    Predicate<int> predicate = delegate(int x) { return x % 2 == 0; } ;

    List<int> evenNumbers = ints.FindAll(predicate);

    foreach (var item in evenNumbers)
    {
        Console.Write($"{item}\t");
    }

}

//ExampleWithAnonymousMethod1();

void ExampleWithAnonymousMethod2()
{
    List<int> ints = new List<int>();
    ints.AddRange(new int[] { 1, 12, 3, 2, 4, 29, 48, 4, 5 });

    List<int> evenNumbers = ints.FindAll(delegate (int x) { return x % 2 == 0; });

    foreach (var item in evenNumbers)
    {
        Console.Write($"{item}\t");
    }

}

//ExampleWithAnonymousMethod2();

void UseLambdaExpression()
{
    List<int> ints = new List<int>();
    ints.AddRange(new int[] { 1, 12, 3, 2, 4, 29, 48, 4, 5 });

    List<int> evenNumbers = ints.FindAll( x => x % 2 == 0 );

    foreach (var item in evenNumbers)
    {
        Console.Write($"{item}\t");
    }
}

//UseLambdaExpression();

void LambdaWithMultipleStataments()
{
    List<int> ints = new List<int>();
    ints.AddRange(new int[] { 1, 12, 3, 2, 4, 29, 48, 4, 5 });

    List<int> evenNumbers = ints.FindAll((x) =>
    {
        bool isEvent = ((x % 2) == 0);
        Console.WriteLine( $"Is {x} ever :{isEvent}");
        return isEvent;
    });

    foreach (var item in evenNumbers)
    {
        Console.Write($"{item}\t");
    }
}

//LambdaWithMultipleStataments();

void LambdaWithMiltipeParameters()
{
    SimpleMath simpleMath = new();

    simpleMath.SetMathMessageHandler((string message, int result) =>
    {
        Console.WriteLine($"Message:{message}");
        Console.WriteLine($"Result:{result}");
    });

    simpleMath.Add(1, 2);
}

//LambdaWithMiltipeParameters();

void MoreSimpleLambdaWithMiltipeParameters()
{
    SimpleMath simpleMath = new();

    simpleMath.SetMathMessageHandler( (message, result) => 
    Console.WriteLine($"Message:{message}\nResult:{result}")
    );

    simpleMath.Add(1, 2);
}

MoreSimpleLambdaWithMiltipeParameters();