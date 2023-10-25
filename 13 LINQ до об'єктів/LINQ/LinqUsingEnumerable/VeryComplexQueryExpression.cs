using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqUsingEnumerable;

internal class VeryComplexQueryExpression
{
    //MethodForCall
    public static void QueryStringsWithRawDelegates()
    {
        string[] games = { "Morrowind", "Uncharted 2", "Fallout 3", "Daxter", "System Shock 2" };
        CollectionToConsoleInLine(games);

        // Build the necessary Func<> delegates.
        Func<string, bool> searchFilter = new Func<string, bool>(Filter);
        Func<string, string> itemToProcess = new Func<string, string>(ProcessItem);

        var subset = games
            .Where(searchFilter)
            .OrderBy(itemToProcess)
            .Select(itemToProcess);
        CollectionToConsoleInLine(subset);            

    }

    // Delegate targets.
    public static bool Filter(string item)
    {
        return item.Contains(" ");
    }

    public static string ProcessItem(string  item)
    {
        return item;
    }

    // Helper mathod
    public static void CollectionToConsoleInLine<T>(IEnumerable<T>? collection)
    {
        if (collection == null) return;

        foreach (var item in collection)
        {
            Console.Write(item + "\t");
        }
        Console.WriteLine();
    }
}
