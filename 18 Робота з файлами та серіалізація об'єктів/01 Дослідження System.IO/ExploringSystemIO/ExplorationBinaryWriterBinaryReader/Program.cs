static void WorkWithBinaryWriter()
{
    FileInfo fileInfo = new(@"D:\Temp\BinaryFile.dat");

    using BinaryWriter binaryWriter = new(fileInfo.OpenWrite());

    Console.WriteLine($"binaryWriter.BaseStream : {binaryWriter.BaseStream}");

    int myInt = 888;
    double myDouble = 888.88;
    string myString = "hi!";

    binaryWriter.Write(myInt);
    binaryWriter.Write(myDouble);
    binaryWriter.Write(myString);

    Console.WriteLine("Done.");
}
//WorkWithBinaryWriter();

static void WorkWithBinaryReader()
{
    FileInfo fileInfo = new(@"D:\Temp\BinaryFile.dat");

    using BinaryReader binaryReader = new(fileInfo.OpenRead());

    Console.WriteLine(binaryReader.ReadInt32());
    Console.WriteLine(binaryReader.ReadDouble());
    Console.WriteLine(binaryReader.ReadString());
}
WorkWithBinaryReader();



