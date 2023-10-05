using FeaturesForLINQ;
using System.Drawing;

void ImpicitlyTypedLocalVariables()
{
    var variable1 = 2;
    var variable2 = "Hi";
    var variable3 = false; 
    
    Console.WriteLine($"{nameof(variable1)}\t{variable1}\t{variable1.GetType()}");
    Console.WriteLine($"{nameof(variable2)}\t{variable2}\t{variable2.GetType()}");
    Console.WriteLine($"{nameof(variable3)}\t{variable3}\t{variable3.GetType()}");
}

//ImpicitlyTypedLocalVariables();

void ObjectAndCollectionInitializationSyntax()
{
    List<Rectangle> rectangles = new() 
    {
        new Rectangle()
        {
            X = 0,Y = 0,Width = 10, Height = 10,
        },
        new Rectangle()
        {
            X = 10,Y = 10,Width = 20, Height = 20
        },
        new Rectangle()
        {
            X = 30,Y = 30,Width = 40, Height = 40
        },
    };
}


void LambdaExpressions()
{
    List<int> ints = new();

    ints.AddRange(new[] { 12, 31, 23, 21, 34 });

    List<int> evenNumbers = ints.FindAll( x => (x%2) == 0);

    foreach (var item in evenNumbers)
    {
        Console.WriteLine(item);
    }
}

//LambdaExpressions();

void UseExtentionMethod()
{
    int variable1 = 1;
    variable1.DisplayDefiningAssembly();

    System.Data.DataSet dataSet = new();
    dataSet.DisplayDefiningAssembly();
}

//UseExtentionMethod();

void UseAnonymousType()
{
    var purchaseItem = new
    {
        TimeBought = DateTime.Now,
        ItemBought = new
        {
            Color = "Grey",
            Make = "BMW",
        },
        Price = 35000

    };

    Console.WriteLine(purchaseItem.GetType());
    Console.WriteLine(purchaseItem.ItemBought.Color);
}

UseAnonymousType();