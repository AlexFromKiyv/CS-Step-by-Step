int length = args.Length;
string[] appArgs = Environment.GetCommandLineArgs();

Console.WriteLine($"The number of parameters:{length}");
for (int i = 0; i < length; i++)
{
    Console.WriteLine($"Prameter {i}:" + args[i]);
}

