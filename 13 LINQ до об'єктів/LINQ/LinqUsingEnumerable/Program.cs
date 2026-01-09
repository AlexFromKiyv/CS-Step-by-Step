using LinqUsingEnumerable;

void CollectionToConsole<T>(IEnumerable<T>? collection)
{
    if (collection == null) return;

    foreach (var item in collection)
    {
        Console.WriteLine(item);
    }
    Console.WriteLine();
}

void CollectionToConsoleInLine<T>(IEnumerable<T>? collection)
{
    if (collection == null) return;

    foreach (var item in collection)
    {
        Console.Write(item+"\t");
    }
    Console.WriteLine();
}


void QueryStringWithOretators()
{
    string[] games = { "Morrowind", "Uncharted 2", "Fallout 3", "Daxter", "System Shock 2" };

    var subset =
        from game in games
        where game.Contains(" ")
        orderby game
        select game;

    CollectionToConsoleInLine(subset);      
}
//QueryStringWithOretators();

void QueryStringWithEnumerableAndLambdas()
{
    string[] games = { "Morrowind", "Uncharted 2", "Fallout 3", "Daxter", "System Shock 2" };

    var subset = games
        .Where(game => game.Contains(" "))
        .OrderBy(game => game)
        .Select(game => game);
    CollectionToConsoleInLine(subset);
}
//QueryStringWithEnumerableAndLambdas();


void QueryStringWithEnumerableAndLambdasLong()
{
    string[] games = { "Morrowind", "Uncharted 2", "Fallout 3", "Daxter", "System Shock 2" };
    CollectionToConsoleInLine(games);

    var subset = games
        .Where(game => game.Contains(" "))
        .OrderBy(game => game)
        .Select(game => game);
    CollectionToConsoleInLine(subset);

    Console.WriteLine("\n");
    
    CollectionToConsoleInLine(games);

    var gamesWithSpaces = games
        .Where(game => game.Contains(" "));
    CollectionToConsoleInLine(gamesWithSpaces);
    
    var gamesWithSpacesAndOrderby = gamesWithSpaces
        .OrderBy(game => game);
    CollectionToConsoleInLine(gamesWithSpacesAndOrderby);

    var gamesWithSpacesAndOrderbyAndSelect = gamesWithSpacesAndOrderby
        .Select(game => game);
    CollectionToConsoleInLine(gamesWithSpacesAndOrderbyAndSelect);

}
//QueryStringWithEnumerableAndLambdasLong();

void QueryStringWithWithAnonymousMethods()
{
    string[] games = { "Morrowind", "Uncharted 2", "Fallout 3", "Daxter", "System Shock 2" };
    CollectionToConsoleInLine(games);

    // Build the necessary Func<> delegates using anonymous methods.
    Func<string,bool> searchFilter = delegate(string gameName)
    {
        return gameName.Contains(" ");
    };
    Func<string, string> itemToProcess = delegate (string gameName)
    {
        return gameName;
    };

    var subset = games
        .Where(searchFilter)
        .OrderBy(itemToProcess)
        .Select(itemToProcess);

    CollectionToConsoleInLine(subset);

}
//QueryStringWithWithAnonymousMethods();

//VeryComplexQueryExpression.QueryStringsWithRawDelegates();

