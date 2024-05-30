
using System.Text;

void ExploringWriteReadStringWithFileStream()
{
    string directoryFullName = @"D:\Temp";
    string fileFullName = @"D:\Temp\Message.dat";
    string message = "Hi girl! How are you?";

    WriteMassege(message,directoryFullName,fileFullName);

    ReadMessage(fileFullName);
}
//ExploringWriteReadStringWithFileStream();

void WriteMassege(string message, string directoryFullName, string fileFullName)
{
    CheckOrCreateDirectory(directoryFullName);

    using FileStream fileStream = File.Open(fileFullName, FileMode.Create);

    byte[] bites = Encoding.Default.GetBytes(message);

    fileStream.Write(bites, 0, bites.Length);

    Console.WriteLine($"fileStream.Position after write:{fileStream.Position}");
}


void ReadMessage(string fileFullName)
{
    CheckFile(fileFullName);

    using FileStream fileStream = File.Open(fileFullName, FileMode.Open);

    Console.WriteLine($"fileStream.Position before read:{fileStream.Position}");

    byte[] bytes = new byte[fileStream.Length];
    for (int i = 0; i < fileStream.Length; i++)
    {
        bytes[i] = (byte)fileStream.ReadByte();
        Console.WriteLine($"{bytes[i]} {(char)bytes[i]}");
    }
    Console.WriteLine();

    Console.WriteLine(Encoding.Default.GetString(bytes));
}



// Helpers
void CheckOrCreateDirectory(string path)
{
    DirectoryInfo directoryInfo = new(path);
    if (!directoryInfo.Exists)
    { directoryInfo.Create(); }
}
void CheckFile(string path)
{
    if (!File.Exists(path))
    {
        throw new IOException("File not found.");
    }
}


// WriteAsync ReadAsync

static async Task WriteAndReadFileAsync()
{
    Console.Write("Enter a string to write to a file:");
    string textForFile = Console.ReadLine()!;

    using FileStream fileStreamW = new(@"D:\Temp\note.txt", FileMode.Create);
    byte[] textArray = System.Text.Encoding.Default.GetBytes(textForFile);

    await fileStreamW.WriteAsync(textArray, 0, textArray.Length);

    fileStreamW.Close();

    using FileStream fileStreamR = File.OpenRead(@"D:\Temp\note.txt");
    byte[] array = new byte[fileStreamR.Length];

    await fileStreamR.ReadAsync(array, 0, array.Length);

    string textFromFile = System.Text.Encoding.Default.GetString(array);

    Console.WriteLine(textFromFile);

}
await WriteAndReadFileAsync();






