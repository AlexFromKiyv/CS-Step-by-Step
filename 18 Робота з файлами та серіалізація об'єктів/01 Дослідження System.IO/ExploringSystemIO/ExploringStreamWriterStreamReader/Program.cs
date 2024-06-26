﻿
// StreamWriter StreamReader
void WriteToTextReadFromText()
{
    string directoryFullName = @"D:\Temp";
    string fileFullName = @"D:\Temp\MyList.txt";

    WriteToText(directoryFullName, fileFullName);
    ReadFromText(fileFullName);
}
//WriteToTextReadFromText();

void WriteToText(string directoryFullName, string fileFullName)
{
    CheckOrCreateDirectory(directoryFullName);

    using StreamWriter streamWriter = File.CreateText(fileFullName);

    streamWriter.WriteLine("1 Build a house");
    streamWriter.WriteLine("2 Raise a Son");
    streamWriter.WriteLine("3 Plant a tree");
    streamWriter.WriteLine("4 Learn English");
    for (int i = 5; i <= 12; i++)
    {
        streamWriter.WriteLine(i + " ...");
    }
    for (int i = 0; i <= 12; i++)
    {
        streamWriter.Write(i + " ");
    }
    streamWriter.Write(streamWriter.NewLine);

    Console.WriteLine($"Creanted file {fileFullName}.");
}

void ReadFromText(string fileFullName)
{
    CheckFile(fileFullName);
    
    using StreamReader streamReader = File.OpenText(fileFullName);

    string? input;
    while ((input = streamReader.ReadLine()) != null)
    {
        Console.WriteLine(input);
    }
}

//  Directly Create
void DirectlyCreateStreamWriterStreamReader()
{
    string path = @"D:\Temp\MyText.txt";

    using StreamWriter streamWriter  = new(path);

    streamWriter.WriteLine("Good day!");

    streamWriter.Close();

    using StreamReader streamReader = new(path);

    string? input = streamReader.ReadToEnd();

    Console.WriteLine(input);
}
//DirectlyCreateStreamWriterStreamReader();


// Create Append

void CreateFileAndAppendToFileWithStreamWriter()
{
    string path = @"D:\Temp\MyText.txt";
    string text = "Hello\ngirl";

    using StreamWriter writer1 = new StreamWriter(path);
    writer1.Write(text);
    writer1.Close();

    using StreamWriter writer2 = new StreamWriter(path, true);
    writer2.WriteLine("s");
    writer2.Close();

    using StreamWriter writer3 = new StreamWriter(path, true, System.Text.Encoding.Default);
    writer3.WriteLine("How are you?");
    writer3.Close();
}
CreateFileAndAppendToFileWithStreamWriter();



// Helpers
void CheckOrCreateDirectory(string directoryInfoFullName)
{
    DirectoryInfo directoryInfo = new(directoryInfoFullName);
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
