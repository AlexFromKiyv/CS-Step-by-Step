
using System.Text;

static void CreateStringWriter()
{
    using StringWriter stringWriter = new();

    stringWriter.WriteLine("What is love?");

    Console.WriteLine($"Contens of stringWriter: {stringWriter} ") ;
}
//CreateStringWriter();


static void UseStringBuilder()
{
    using StringWriter stringWriter = new();
    stringWriter.WriteLine("What is love?");
    Console.WriteLine($"Contens of stringWriter: {stringWriter}");

    StringBuilder stringBuilder = stringWriter.GetStringBuilder();

    stringBuilder.Insert(0, "Hi!");
    Console.WriteLine($"Contens of stringWriter: {stringWriter}");
    Console.WriteLine(stringBuilder.ToString());

    stringBuilder.Remove(0, "Hi!".Length);
    Console.WriteLine($"Contens of stringWriter: {stringWriter}");
    Console.WriteLine(stringBuilder.ToString());

}
//UseStringBuilder();

static void UsingStringReader()
{
    using StringWriter stringWriter = new();
    stringWriter.WriteLine("What is love?");

    using StringReader stringReader = new(stringWriter.ToString());

    string? input = null;

    while ((input = stringReader.ReadLine()) != null)
    {
        Console.WriteLine(input);
    }
}
UsingStringReader();

