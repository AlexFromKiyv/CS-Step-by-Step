
static void MonitoringFilesInADirectory()
{
    // Establish the path to the directory to watch
    FileSystemWatcher fileSystemWatcher = new();
	try
	{
		fileSystemWatcher.Path = @"D:\Temp";
        Console.WriteLine($"Set up the fileSystemWatcher.Path = " +
            $"{fileSystemWatcher.Path}");
    }
    catch (Exception ex)
	{
        Console.WriteLine(ex.Message);
	}

    // Set up the things to be on the lookout for.
    fileSystemWatcher.NotifyFilter = 
          NotifyFilters.LastAccess
        | NotifyFilters.LastWrite
        | NotifyFilters.FileName
        | NotifyFilters.DirectoryName;
    Console.WriteLine($"Set up the fileSystemWatcher.NotifyFilter = " +
        $"{fileSystemWatcher.NotifyFilter}");

    // Only watch text files.
    fileSystemWatcher.Filter = "*.txt";

    Console.WriteLine($"Set up the fileSystemWatcher.Filter = " +
        $"{fileSystemWatcher.Filter}");

    // Add event handlers.
    
    // Specify what is done when a file is changed, created, or deleted.
    fileSystemWatcher.Changed += (s, e) =>
    Console.WriteLine($"File {e.FullPath} {e.ChangeType} ");
    fileSystemWatcher.Created += (s, e) =>
    Console.WriteLine($"File {e.FullPath} {e.ChangeType} ");
    fileSystemWatcher.Deleted += (s, e) =>
    Console.WriteLine($"File {e.FullPath} {e.ChangeType} ");

    // Specify what is done when a file is renamed.
    fileSystemWatcher.Renamed += (s, e) =>
    Console.WriteLine($"File {e.OldFullPath} renamed to {e.FullPath} ");

    // Begin watching the directory.
    fileSystemWatcher.EnableRaisingEvents = true;
    Console.WriteLine($"I'm watching what happened with text file in directory " +
        $"{fileSystemWatcher.Path}");
    Console.WriteLine("Press End to quit.\n\n");

    // Raise some events.
    DoSomethingWithTheFiles();

    while (Console.ReadKey().Key != ConsoleKey.End) { }
}
MonitoringFilesInADirectory();


// Test helper
static void DoSomethingWithTheFiles()
{
    using StreamWriter streamWriter = File.CreateText(@"D:\Temp\MyDoc.txt");

    streamWriter.WriteLine("Chapter 1");
    streamWriter.Close();

    File.Move(@"D:\Temp\MyDoc.txt", @"D:\Temp\MyDocumetation.txt");
    File.Delete(@"D:\Temp\MyDoc.txt");
}

