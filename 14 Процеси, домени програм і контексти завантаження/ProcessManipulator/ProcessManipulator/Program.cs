using System.Diagnostics;

static void ListAllRunningProcesses()
{
    // Get all the processes on the local machine, ordered by
    // PID.
    var runningProcs = from proc in Process.GetProcesses(".") 
                       orderby proc.Id 
                       select proc;

    // Print out PID and name of each process.
    foreach (var p in runningProcs)
    {
        string info = $"PID: {p.Id}\tName: {p.ProcessName}";
        Console.WriteLine(info);
    }
}
//ListAllRunningProcesses();

static void GetSpecificProcess()
{
    // Get all the processes on the local machine, ordered by
    // PID.
    var runningProcs = from proc in Process.GetProcesses(".")
                       orderby proc.Id
                       select proc.Id;

    int id = runningProcs.Max();
    Process process = Process.GetProcessById(id);

    Console.WriteLine(process.Id+"\t"+process.ProcessName);
}
//GetSpecificProcess();


static void EnumThreadsForProcess(Process theProc)
{

    // List out stats for each thread in the specified process.
    Console.WriteLine($"Here are the threads used by:{theProc.Id} {theProc.ProcessName}");
    ProcessThreadCollection theThreads = theProc.Threads;

    foreach (ProcessThread pt in theThreads)
    {
        string info =
            $"-> Thread ID: {pt.Id}\tStart Time: {pt.StartTime.ToShortTimeString()}\tPriority: {pt.PriorityLevel}";
        Console.WriteLine(info);
    }
}

static void EnumThreads()
{
    var runningProcs = from proc in Process.GetProcesses(".")
                       where proc.ProcessName == "chrome"
                       select proc;

    // Print out PID and name of each process.
    foreach (var p in runningProcs)
    {
        EnumThreadsForProcess(p);
    }
}
//EnumThreads();

static void EnumModulesForProcess(Process theProc)
{

    Console.WriteLine($"Here are the loaded modules for:{theProc.Id} {theProc.ProcessName}");
    ProcessModuleCollection theMods = theProc.Modules;
    foreach (ProcessModule pm in theMods)
    {
        string info = $"-> Mod Name: {pm.ModuleName}";
        Console.WriteLine(info);
    }
    Console.WriteLine("************************************\n");
}

static void EnumModules()
{
    Process process = Process.GetCurrentProcess();
    EnumModulesForProcess(process);
}
//EnumModules();

static void StartAndKillProcess()
{
    Process proc = null;
    // Launch Microsoft Edge
    try
    {
        //proc = Process.Start(@"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe", "www.weather.com");

        ProcessStartInfo startInfo = new ProcessStartInfo("MsEdge", "www.weather.com");
        startInfo.UseShellExecute = true;
        proc = Process.Start(startInfo);
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine(ex.Message);
    }
    Console.Write($"--> Hit enter to kill {proc.ProcessName}...");
    Console.ReadLine();

    try
    {
        foreach (var p in Process.GetProcessesByName("MsEdge"))
        {
            p.Kill(true);
        }
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine(ex.Message);
    }
}
//StartAndKillProcess();

void UseApplicationVerbs()
{
    ProcessStartInfo processStartInfo = new(@"D:\Hi.txt");

    int i = 0;
    foreach (string? verb in processStartInfo.Verbs)
    {
        Console.WriteLine($"  {i++}. {verb}");
    }
    Console.ReadLine();

    processStartInfo.WindowStyle = ProcessWindowStyle.Maximized;
    processStartInfo.Verb = "open";
    processStartInfo.UseShellExecute = true;
    Process.Start(processStartInfo);
}
//UseApplicationVerbs();