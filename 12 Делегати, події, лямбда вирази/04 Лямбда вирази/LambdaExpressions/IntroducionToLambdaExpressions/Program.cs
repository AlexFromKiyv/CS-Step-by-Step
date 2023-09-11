
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

//MoreSimpleLambdaWithMiltipeParameters();

void LambdaWithoutParameters()
{
    Call call = new();

    call.SetVSDelegate(() => "Hi girl!");

    call.SaySomething();
}

//LambdaWithoutParameters();

void LambdaAndOuterVariables()
{
    int outerVariable = 0;

    Func<int, int, int> sum = (x, y) =>
    {
        outerVariable++;
        return x + y;
    };

    sum(1, 2);
    sum(1, 2);

    Console.WriteLine(outerVariable);
}

//LambdaAndOuterVariables();

void StaticLambda()
{
    int outerVariable = 0;

    Func<int, int, int> sum = static (x, y) =>
    {
        // A compilation error occurs here.
        // Cannot a reference to ...
        //outerVariable++;
        return x + y;
    };

    sum(1, 2);
    sum(1, 2);

    Console.WriteLine(outerVariable);
}

void DiscardsWithLambda()
{
    int counter = 0;
    Func<int, bool> tik = (_) =>
    {
        counter++;
        return true;
    };

    Console.WriteLine(tik(1));
    Console.WriteLine(tik(2));
    Console.WriteLine(counter);
}

DiscardsWithLambda();