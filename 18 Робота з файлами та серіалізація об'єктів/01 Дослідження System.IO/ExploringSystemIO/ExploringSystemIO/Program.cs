
// Class DirectoryInfo 

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

    DirectoryInfo directoryInfo_1_1 = directoryInfo_1.CreateSubdirectory(@"Project");
    ShowDirectoryInfo(directoryInfo_1_1);

    DirectoryInfo directoryInfo_1_2 = directoryInfo_1.CreateSubdirectory(@"Project1\Data");
    ShowDirectoryInfo(directoryInfo_1_2);
}
//HowCreateSubdirectory();


// Class Directory

void ExploringDirectory()
{
    DirectoryInfo directoryInfo =
        Directory.CreateDirectory(@"D:\SuperCode\Project2");
    ShowDirectoryInfo(directoryInfo);

    DeleteDirectory(@"D:\SuperCode\Project2");

    ShowAllDriveWithDirectory();
}
//ExploringDirectory();

void ShowAllDriveWithDirectory()
{
    string[] driveNames = Directory.GetLogicalDrives();
    
    Console.WriteLine("The mashine has drive:");
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


// Class DriveInfo

void ExploringDriveInfo()
{
    DriveInfo[] driveInfos = DriveInfo.GetDrives();

    foreach (DriveInfo driveInfo in driveInfos)
    {
        ShowDriveInfo(driveInfo);
    }
}
//ExploringDriveInfo();

void ShowDriveInfo(DriveInfo driveInfo)
{
    Console.WriteLine($"Drive: {driveInfo.Name}");
    Console.WriteLine($"\tVolumeLabel: {driveInfo.VolumeLabel}");
    Console.WriteLine($"\tTotalSize: {driveInfo.TotalSize}");
    Console.WriteLine($"\tTotalFreeSpace: {driveInfo.TotalFreeSpace}");
    Console.WriteLine($"\tAvailableFreeSpace: {driveInfo.AvailableFreeSpace}");
    Console.WriteLine($"\tDriveFormat: {driveInfo.DriveFormat}");
    Console.WriteLine($"\tDriveType: {driveInfo.DriveType}");
    Console.WriteLine($"\tRootDirectory: {driveInfo.RootDirectory}");
    Console.WriteLine($"\tIsReady: {driveInfo.IsReady}");
}


// Path  Environment

void SpecialSettingsOS()
{
    Console.WriteLine("About platform:");
    Console.WriteLine($"Path.PathSeparator: {Path.PathSeparator}");
    Console.WriteLine($"Path.DirectorySeparatorChar: {Path.DirectorySeparatorChar}");
    Console.WriteLine($"Path.GetTempPath(): {Path.GetTempPath()}");

    //Console.WriteLine($"Directory.GetCurrentDirectory(): {Directory.GetCurrentDirectory()}");
    //Console.WriteLine($"Environment.CurrentDirectory: {Environment.CurrentDirectory}");
    Console.WriteLine($"Environment.SystemDirectory: {Environment.SystemDirectory}");
    Console.WriteLine($"{Environment.GetFolderPath(Environment.SpecialFolder.System)}");
    Console.WriteLine($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}");
    Console.WriteLine($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}");
    Console.WriteLine($"{Environment.GetFolderPath(Environment.SpecialFolder.Personal)}");
}
//SpecialSettingsOS();

void UsePathAndEnvironment()
{
    string folderPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.Personal),
        "MyFolder");
    Console.WriteLine(folderPath);

    Console.WriteLine(Directory.Exists(folderPath));

    Directory.CreateDirectory(folderPath);

    Console.WriteLine(Directory.Exists(folderPath));

    Directory.Delete(folderPath);

    Console.WriteLine(Directory.Exists(folderPath));
}
//UsePathAndEnvironment();



// FileInfo.Create

void CheckOrCreateDirectory(string directoryInfoFullName)
{
    DirectoryInfo directoryInfo = new(directoryInfoFullName);
    if (!directoryInfo.Exists)
    { 
        directoryInfo.Create(); 
    }
}


void ExploringCreateFileWithFileInfo()
{
    CheckOrCreateDirectory(@"D:\Temp");

    var fileInfoFullName = @"D:\Temp\Test.dat";
    
    FileInfo fileInfo = new(fileInfoFullName);

    //FileStream fileStream = fileInfo.Create();

    //// Use FileStream

    //fileStream.Close();

    using FileStream fileStream = fileInfo.Create();

    // Use FileStream

}
//ExploringCreateFileWithFileInfo();



//FileInfo.Open

void ExploringFileInfoOpen()
{
    CheckOrCreateDirectory(@"D:\Temp");

    var fileInfoFullName = @"D:\Temp\Test.dat";

    FileInfo fileInfo = new(fileInfoFullName);

    using FileStream fileStream = fileInfo.Open(FileMode.OpenOrCreate,
        FileAccess.ReadWrite, FileShare.None);
    
    // Use the FileStream object...
}
//ExploringFileInfoOpen();


// FileInfo.OpenRead FileInfo.OpenWrite
void ExploringFileInfoOpenReadOpenWrite()
{
    CheckOrCreateDirectory(@"D:\Temp");

    FileInfo fileInfo = new(@"D:\Temp\Test.dat");

    if (!fileInfo.Exists)
    {
        fileInfo.Create().Close();
    }

    FileStream fileStream = fileInfo.OpenRead();

    // Use the FileStream object... for read

    FileInfo fileInfo1 = new(@"D:\Temp\Test1.dat");

    FileStream fileStream1 = fileInfo1.OpenWrite();

    // Use the FileStream object... for write

}
//ExploringFileInfoOpenReadOpenWrite();


// FileInfo.OpenText
void ExploringFileInfoOpenText()
{
    CheckOrCreateDirectory(@"D:\Temp");

    FileInfo fileInfo = new(@"D:\Temp\boot.ini");

    if (!fileInfo.Exists)
    {
        fileInfo.Create().Close();
    }
    using StreamReader streamReader = fileInfo.OpenText();

    //  Use the streamReader object
}
//ExploringFileInfoOpenText();


// FileInfo.CreateText  FileInfo.AppendText

void ExploringFileInfoCreateTextAppendText()
{
    CheckOrCreateDirectory(@"D:\Temp");

    FileInfo fileInfo = new(@"D:\Temp\config.ini");

    using StreamWriter streamWriter = fileInfo.CreateText();

    // Use the StreamWriter object

    fileInfo = new(@"D:\Temp\boot.ini");

    using StreamWriter streamWriter1 = fileInfo.AppendText();

    // Use the StreamWriter object
}
//ExploringFileInfoCreateTextAppendText();



// Class File

void ExploringFileCreateAndOpen()
{
    CheckOrCreateDirectory(@"D:\Temp");

    var fileFullName = @"D:\Temp\Test.dat";

    using FileStream fileStream = File.Create(fileFullName);
    //Using FileStream object ...

    //using FileStream fileStream = File.Open(fileFullName, FileMode.OpenOrCreate,
    //    FileAccess.ReadWrite, FileShare.None);
    //// Using FileStream object ...

    //using FileStream fileStream = File.OpenRead(fileFullName);
    //// Using FileStream object with read-only permissions.

    //using FileStream fileStream = File.OpenWrite(fileFullName);
    //// Using FileStream object with write-only permissions.

    //using StreamReader streamReader = File.OpenText(fileFullName);
    //// Using StreamReader object ...

    //using StreamWriter streamWriter = File.CreateText(fileFullName);
    //// Using StreamWriter object ...

    //using StreamWriter streamWriter = File.AppendText(fileFullName);
    //// Using StreamWriter object ...
}
//ExploringFileCreateAndOpen();




// ReadAllBytes ReadAllLines ReadAllText WriteAllBytes WriteAllLines WriteAllText

void ExploringFileWriteAllLines()
{
    string[] myList = ["carrots", "chicken", "grapes", "milk"];

    CheckOrCreateDirectory(@"D:\Temp");

    File.WriteAllLines(@"D:\Temp\FoodList.txt", myList);

    string[] myFood = File.ReadAllLines(@"D:\Temp\FoodList.txt");

    foreach (var product in myFood)
    {
        Console.WriteLine(product);
    }
}
//ExploringFileWriteAllLines();