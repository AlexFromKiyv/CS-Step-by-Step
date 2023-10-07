void CollectionToConsole<T>( IEnumerable<T> collection)
{
    foreach (var item in collection)
    {
        Console.WriteLine(item);
    }
}

void QuryesOverStringsArray()
{
    string[] games =
    {
        "Morrowind",
        "Uncharted 2",
        "Fallout 3",
        "Daxter",
        "System Shock 2"
    };

    // Build a query expression to find the items in the array
    // that have an embedded space.

    IEnumerable<string> longNames = 
        from ng in games
        where ng.Contains(" ")
        orderby ng
        select ng;

    CollectionToConsole(longNames);
}

//QuryesOverStringsArray();

void QuryesOverStringsArrayWithExtentionMethods()
{
    string[] games =
    {
        "Morrowind",
        "Uncharted 2",
        "Fallout 3",
        "Daxter",
        "System Shock 2"
    };

    IEnumerable<string> longNames =
        games.Where(ng => ng.Contains(" ")).OrderBy(ng => ng).Select(ng => ng);

    CollectionToConsole(longNames);
}

//QuryesOverStringsArrayWithExtentionMethods();

void QueryOverStringsWithoutLINQ()
{
    string[] games =
{
        "Morrowind",
        "Uncharted 2",
        "Fallout 3",
        "Daxter",
        "System Shock 2"
    };

    string[] gamesWithSpace = new string[5];

    // Selection
    for (int i = 0; i < games.Length; i++)
    {
        if (games[i].Contains(" "))
        {
            gamesWithSpace[i] = games[i]; 
        }
    }

    //Sort
    Array.Sort(gamesWithSpace);

    //Print
    foreach (string s in gamesWithSpace)
    {
        if (s != null)
        {        
            Console.WriteLine(s);
        }
    }
}

//QueryOverStringsWithoutLINQ();


void ReflectOverQueryResult(object resultSet, string queryType = "Query Expressions")
{
    Console.WriteLine($"\nQuery type: {queryType}");
    Console.WriteLine($"Result is type of:{resultSet.GetType().Name}");
    Console.WriteLine($"This type locate:{resultSet.GetType().Assembly.GetName().Name}");
}

void ExploreResultSetQueryExpression()
{
    string[] games =
{
        "Morrowind",
        "Uncharted 2",
        "Fallout 3",
        "Daxter",
        "System Shock 2"
    };

    IEnumerable<string> longNames =
        from ng in games
        where ng.Contains(" ")
        orderby ng
        select ng;

    ReflectOverQueryResult(longNames);
}

//ExploreResultSetQueryExpression();

void ExploreResultSetExtensionMethods()
{
    string[] games =
{
        "Morrowind",
        "Uncharted 2",
        "Fallout 3",
        "Daxter",
        "System Shock 2"
    };

    IEnumerable<string> longNames =
        games
        .Where(ng => ng.Contains(" "))
        .OrderBy(ng => ng)
        .Select(ng => ng);

    ReflectOverQueryResult(longNames,"Extension Methods.");
}

//ExploreResultSetExtensionMethods();

void QueryOverInts()
{
    int[] ints = { 10, 20, 30, 40, 1, 2, 3, 8 };

    IEnumerable<int> intsLeesThan10 =
        from i in ints
        where i < 10
        select i;

    foreach (var item in intsLeesThan10)
    {
        Console.WriteLine(item);
    }

    ReflectOverQueryResult(intsLeesThan10);
}

//QueryOverInts();

void QueryOverIntsUseImplicitlyTypedLocalVariables()
{
    int[] ints = { 10, 20, 30, 40, 1, 2, 3, 8 };

    var intsLeesThan10 =
        from i in ints
        where i < 10
        select i;

    CollectionToConsole(intsLeesThan10);

}

//QueryOverIntsUseImplicitlyTypedLocalVariables();

