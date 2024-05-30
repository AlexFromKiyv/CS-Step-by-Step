
using System.Text;

static void EncodingText()
{
    Console.WriteLine("Encodings");
    Console.WriteLine("[1] ASCII");
    Console.WriteLine("[2] UTF-7");
    Console.WriteLine("[3] UTF-8");
    Console.WriteLine("[4] UTF-16 (Unicode)");
    Console.WriteLine("[5] UTF-32");
    Console.WriteLine("[6] Latin1");
    Console.WriteLine("[any other key] Default encoding");
    Console.WriteLine();

    Console.Write("Press a number to choose an encoding:");
    ConsoleKey number = Console.ReadKey(intercept: true).Key;

    Encoding encoder = number switch
    {
        ConsoleKey.D1 or ConsoleKey.NumPad1 => Encoding.ASCII,
        ConsoleKey.D2 or ConsoleKey.NumPad2 => Encoding.UTF7,
        ConsoleKey.D3 or ConsoleKey.NumPad3 => Encoding.UTF8,
        ConsoleKey.D4 or ConsoleKey.NumPad4 => Encoding.Unicode,
        ConsoleKey.D5 or ConsoleKey.NumPad5 => Encoding.UTF32,
        ConsoleKey.D6 or ConsoleKey.NumPad6 => Encoding.Latin1,
        _ => Encoding.Default
    };

    Console.WriteLine($"\n\nYou chose:"+encoder.BodyName);
    
    var message = "Café £4.39";
    
    Console.WriteLine($"\nText to encode: {message}  Characters: {message.Length}");
    // encode the string into a byte array
    byte[] encoded = encoder.GetBytes(message);
    // check how many bytes the encoding needed
    Console.WriteLine("{0} used {1:N0} bytes.",
      encoder.GetType().Name, encoded.Length);
    Console.WriteLine();
    // enumerate each byte 
    Console.WriteLine($"BYTE | HEX | CHAR");
    foreach (byte b in encoded)
    {
        Console.WriteLine($"{b,4} | {b.ToString("X"),3} | {(char)b,4}");
    }
    // decode the byte array back into a string and display it
    string decoded = encoder.GetString(encoded);
    Console.WriteLine(decoded);
}
//EncodingText();

while (true)
{
    Console.Clear();
    EncodingText();
    Console.ReadKey();
}