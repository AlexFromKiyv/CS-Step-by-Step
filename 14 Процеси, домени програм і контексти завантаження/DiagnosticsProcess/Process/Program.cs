using System.Diagnostics;

static void GetAllRunningProcesses()
{
    var runningProcesses = from p in Process.GetProcesses(".")
                           orderby p.Id
                           select p;

    foreach (var p in runningProcesses)
    {
        string aboutProcess = $"{p.Id} {p.ProcessName}";
        Console.WriteLine(aboutProcess);
    }

    Console.WriteLine($"\nTotal number of processes:{runningProcesses.Count()}");
}
GetAllRunningProcesses(); 