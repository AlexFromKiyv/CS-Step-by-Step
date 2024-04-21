// Exploring DirectoryInfo 

void ExploringDirectoryInfo()
{
    //DirectoryInfo directoryInfo_1 = new(@"C:\");
    //ShowDirectoryInfo(directoryInfo_1);

    //// Bind to the current working directory.
    //DirectoryInfo directoryInfo_2 = new(".");
    //ShowDirectoryInfo(directoryInfo_2);

    //// Bind to C:\Windows,
    //// using a verbatim string.
    //DirectoryInfo directoryInfo_3 = new(@"C:\Windows");
    //ShowDirectoryInfo(directoryInfo_3);

    //// Bind to a nonexistent directory, then create it.
    //DirectoryInfo directoryInfo_4 = new(@"D:\SuperCode\MyProject");
    //directoryInfo_4.Create();
    //ShowDirectoryInfo(directoryInfo_4);

    //For for different platforms 
    char vsc = Path.VolumeSeparatorChar;
    char dsc = Path.DirectorySeparatorChar;
    string path = $@"D{vsc}{dsc}SuperCode{dsc}MyProject";

    DirectoryInfo directoryInfo_5 = new(path);
    ShowDirectoryInfo(directoryInfo_5);
}
//ExploringDirectoryInfo();

void ShowDirectoryInfo(DirectoryInfo directoryInfo)
{
    Console.WriteLine($"DirectoryInfo object: {directoryInfo.Name}\n");
    Console.WriteLine($"\tName : {directoryInfo.Name}");
    Console.WriteLine($"\tCreationTime : {directoryInfo.CreationTime}");
    Console.WriteLine($"\tAttributes : {directoryInfo.Attributes}");
    Console.WriteLine($"\tRoot : {directoryInfo.Root}");
    Console.WriteLine($"\tParent : {directoryInfo.Parent}");
    Console.WriteLine($"\tFullName : {directoryInfo.FullName}");
}


// File in directory
void ExploringDirectoryFiles(string directoryString)
{
    DirectoryInfo directoryInfo = new(directoryString);
    ShowInfoFilesJpg(directoryInfo);
}

//ExploringDirectoryFiles(@"C:\Windows\Web\Wallpaper");

void ShowInfoFilesJpg(DirectoryInfo directoryInfo)
{
    FileInfo[] fileInfos = directoryInfo.
        GetFiles("*.jpg", SearchOption.AllDirectories);

    Console.WriteLine($"Files (*.jpg) in {directoryInfo.FullName}\n");
    Console.WriteLine($"Found {fileInfos.Length} files");

    foreach (FileInfo fileInfo in fileInfos)
    {
        ShowFileInfo(fileInfo);
    }
}
void ShowFileInfo(FileInfo fileInfo)
{
    Console.WriteLine($"\nData about file: {fileInfo.Name}");
    Console.WriteLine($"\tName:{fileInfo.Name} ");
    Console.WriteLine($"\tSize:{fileInfo.Length} ");
    Console.WriteLine($"\tAttributes:{fileInfo.Attributes} ");
    Console.WriteLine($"\tCreationTime:{fileInfo.CreationTime} ");
    Console.WriteLine($"\tFull name:{fileInfo.FullName} ");
}


// Create subdirectories

void HowCreateSubdirectory()
{
    DirectoryInfo directoryInfo_1 = new(@"D:\SuperCode");

    if (!directoryInfo_1.Exists)
    {
        directoryInfo_1.Create();
    }

    ShowDirectoryInfo(directoryInfo_1);

    DirectoryInfo directoryInfo_1_1 =  CreateSubdirectory(directoryInfo_1, @"Project");
    ShowDirectoryInfo(directoryInfo_1_1);

    DirectoryInfo directoryInfo_1_2 = CreateSubdirectory(directoryInfo_1, @"Project1\Data");
    ShowDirectoryInfo(directoryInfo_1_2);

}
//HowCreateSubdirectory();

DirectoryInfo CreateSubdirectory(DirectoryInfo directoryInfo,string name )
{
   return directoryInfo.CreateSubdirectory(name);
}


// Class Directory

void ExploringDirectory()
{
    ShowAllDriveWithDirectory();
    Console.WriteLine("\n");

    DeleteDirectory(@"D:\SuperCode\Project1");

}
ExploringDirectory();

void ShowAllDriveWithDirectory()
{
    string[] driveNames = Directory.GetLogicalDrives();
    
    Console.WriteLine("The masine has drive:");
    foreach (string? drive in driveNames)
    {
        Console.WriteLine("\t"+drive);
    }
}

void DeleteDirectory(string directoryString)
{
    if (!Directory.Exists(directoryString)) 
    { 
        Console.WriteLine($"The directory does not exist: {directoryString}");
        return;
    }

    Console.Write($"Delete directory {directoryString} (Y/N):");

    string? answer = Console.ReadLine();

    if (answer !=null && answer.Equals("Y",StringComparison.OrdinalIgnoreCase)) 
    {
        try
        {
            // The second parameter specifies whether you
            // wish to destroy any subdirectories.
            Directory.Delete(directoryString,true);
            Console.WriteLine("The directory has been deleted");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

