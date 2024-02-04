using System.Diagnostics;
using System.Net;
using System.Text;

void DownloadBookAndGetStatistic()
{
    string _textEbook = string.Empty;
    Console.WriteLine("Downloding book ... ");
    GetBook();
    Console.ReadLine();


    void GetBook()
    {
        using WebClient webClient = new WebClient();
        webClient.DownloadStringCompleted += (s, eArgs) =>
        {
            _textEbook = eArgs.Result;
            Console.WriteLine("Download complete.");
            GetStats();
        };
        webClient.DownloadStringAsync(new Uri("http://www.gutenberg.org/files/98/98-0.txt"));
    }

    void GetStats()
    {
        string[] words = _textEbook.Split(new char[] { ' ', '\u000A', ',', '.', ';', ':', '-', '?', '/' },StringSplitOptions.RemoveEmptyEntries);


        string[] tenMostCommon = [];
        string longestWord = string.Empty;

        // Witout Parallel
        //tenMostCommon = FindTenMostCommon(words);
        //longestWord = FindLongestWord(words);

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

        Console.WriteLine(stringBuilder.ToString(),"Book info");
        Console.WriteLine("Work done.");
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

}
DownloadBookAndGetStatistic();

async Task DownloadBookWihtAsyncAwaitAndGetStatisticAsync()
{
    string _theEbook = string.Empty;
    await GetBookAsync();
    Console.ReadLine();


    // Methods
    async Task GetBookAsync()
    {
        HttpClient httpClient = new();
        _theEbook = await httpClient.GetStringAsync("http://www.gutenberg.org/files/98/98-0.txt");
        await Console.Out.WriteLineAsync("Download complite");
        GetStats();        
    }

    void GetStats()
    {
        string[] words = _theEbook.Split(new char[] { ' ', '\u000A', ',', '.', ';', ':', '-', '?', '/' }, StringSplitOptions.RemoveEmptyEntries);

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
    }

    string[] FindTenMostCommon(string[] words)
    {
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
        var query = from word in words
                    orderby word.Length descending
                    select word;
        return query.FirstOrDefault()!;
    }
}
await DownloadBookWihtAsyncAwaitAndGetStatisticAsync();

