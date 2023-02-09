Console.WriteLine("OS: "+Environment.OSVersion);
Console.WriteLine("Number of processors: " + Environment.ProcessorCount);
Console.WriteLine("Machine: " + Environment.MachineName);
Console.WriteLine(".NET: " + Environment.Version);
Console.WriteLine("Version: " + Environment.Version);
foreach (string drive in Environment.GetLogicalDrives())
{
    Console.WriteLine("Drive: " + drive);
}
Console.WriteLine("New line in this OS:" + Environment.NewLine);
Console.WriteLine("Is OS 64-bit: "+ Environment.Is64BitOperatingSystem);
Console.WriteLine("Path to the system directory: " + Environment.SystemDirectory);
Console.WriteLine("Current directory: " + Environment.CurrentDirectory);
Console.WriteLine("User name: "+Environment.UserName);
