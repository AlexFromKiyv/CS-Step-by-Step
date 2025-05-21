using System.Net;
using System.Text;

string _theEBook = "";
GetBook();
Console.WriteLine("Downloading book...");
Console.ReadLine();

void GetBook()
{
    //NOTE: The WebClient is obsolete. 
    //We will revisit this example using HttpClient when we discuss async/await
    using WebClient wc = new WebClient();
    wc.DownloadStringCompleted += (s, eArgs) =>
    {
        _theEBook = eArgs.Result;
        Console.WriteLine("Download complete.");
        //GetStats();
        GetStatsWithParalell();
    };

    // The Project Gutenberg EBook of A Tale of Two Cities, by Charles Dickens
    // You might have to run it twice if you’ve never visited the site before, since the first
    // time you visit there is a message box that pops up, and breaks this code.
    wc.DownloadStringAsync(new Uri("http://www.gutenberg.org/files/98/98-0.txt"));
}

void GetStats()
{
    // Get the words from the ebook.
    string[] words = _theEBook.Split(new char[]
      { ' ', '\u000A', ',', '.', ';', ':', '-', '?', '/' },
      StringSplitOptions.RemoveEmptyEntries);
    // Now, find the ten most common words.
    string[] tenMostCommon = FindTenMostCommon(words);
    // Get the longest word.
    string longestWord = FindLongestWord(words);
    
    // Now that all tasks are complete, build a string to show all stats.
    StringBuilder bookStats = new StringBuilder("Ten Most Common Words are:\n");
    foreach (string s in tenMostCommon)
    {
        bookStats.AppendLine(s);
    }
    bookStats.AppendFormat($"Longest word is: {longestWord}");
    bookStats.AppendLine();
    Console.WriteLine(bookStats.ToString(), "Book info");
}

string[] FindTenMostCommon(string[] words)
{
    Console.WriteLine($"Method FindTenMostCommon in Thread:{Thread.CurrentThread.ManagedThreadId}");
    var frequencyOrder = from word in words
                         where word.Length > 6
                         group word by word into g
                         orderby g.Count() descending
                         select g.Key;
    string[] result = frequencyOrder.Take(20).ToArray();
    return result;
}

string FindLongestWord(string[] words)
{
    Console.WriteLine($"Method FindLongestWord in Thread:{Thread.CurrentThread.ManagedThreadId}");
    var query = from word in words
                orderby word.Length descending
                select word;
    return query.FirstOrDefault()!;
}


void GetStatsWithParalell()
{
    string[] words = _theEBook.Split(new char[] { ' ', '\u000A', ',', '.', ';', ':', '-', '?', '/' }, StringSplitOptions.RemoveEmptyEntries);
    string[] tenMostCommon = [];
    string longestWord = string.Empty;

    Parallel.Invoke(
            () => { tenMostCommon = FindTenMostCommon(words); },
            () => { longestWord = FindLongestWord(words); }
            );

    StringBuilder stringBuilder = new StringBuilder("Ten most common words are:\n");

    foreach (var word in tenMostCommon)
    {
        stringBuilder.AppendLine(word);
    }

    stringBuilder.AppendLine($"Longest word is: {longestWord}");

    Console.WriteLine(stringBuilder.ToString(), "Book info");
    Console.WriteLine("Work done.");
}
