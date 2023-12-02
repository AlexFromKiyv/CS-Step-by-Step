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
//GetAllRunningProcesses(); 

Process? GetSpecificProcess()
{
    GetAllRunningProcesses();

    Console.Write("Enter PID of the process:");
    int.TryParse(Console.ReadLine(),out int pidOfProcess);

    try
    {
        Process theProcess = Process.GetProcessById(pidOfProcess);
        Console.WriteLine(theProcess.ProcessName);
        return theProcess;
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine(ex.Message);
        return null;
    }
    
}

//GetSpecificProcess();

void GetThreadsOfProcess()
{
    Process? theProcess = GetSpecificProcess();

    if (theProcess == null) return;

    Console.WriteLine($"\nIvestigating the process : {theProcess.ProcessName}");

    ProcessThreadCollection theThreads = theProcess.Threads;

    foreach (ProcessThread thread in theThreads) 
    {
        string info =

            $"Thread Id:{thread.Id}\t" +
            $"StartTime:{thread.StartTime.ToShortDateString()}\t" +
            $"PriorityLevel:{thread.PriorityLevel}";

        Console.WriteLine(info);
    }
}
//GetThreadsOfProcess();

void GetModulesOfProcess()
{
    Process? theProcess = GetSpecificProcess();

    if (theProcess == null) return;

    Console.WriteLine($"\nIvestigating the process : {theProcess.ProcessName}");

    ProcessModuleCollection theModules = theProcess.Modules;

    foreach(ProcessModule module in theModules)
    {
        Console.WriteLine($"Module:{module.ModuleName}");
    }
}
//GetModulesOfProcess();

void UseStartAndKill()
{
    Process? process = null;
    
    // Start
    try
    {
      process = Process.Start(@"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe", "http://www.sclass.kiev.ua");

    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine(ex.Message);
    }

    Console.WriteLine($"Нажмiть Enter аби закрити {process?.ProcessName}");
    Console.ReadLine();

    //Kill
    try
    {
        foreach (var p in Process.GetProcessesByName("msedge"))
        {
            p.Kill(true);
        }

    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine(ex.Message);
    }
}
//UseStartAndKill();

void StartWithProcessStartInfo()
{
    Process? process = null;

    // Start
    try
    {
        ProcessStartInfo processStartInfo = new("MsEdge", "www.facebook.com");
        processStartInfo.UseShellExecute = true;
        process = Process.Start(processStartInfo);
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine(ex.Message);
    }

    Console.WriteLine($"Нажмiть Enter аби закрити {process?.ProcessName}");
    Console.ReadLine();

    //Kill
    try
    {
        foreach (var p in Process.GetProcessesByName("msedge"))
        {
            p.Kill(true);
        }

    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine(ex.Message);
    }
}

//StartWithProcessStartInfo();

void UseApplicationVerbs()
{
    ProcessStartInfo processStartInfo = new(@"D:\TheGirl.txt");

    foreach (string? verb in processStartInfo.Verbs)
    {
        Console.WriteLine(verb);
    }

    processStartInfo.WindowStyle = ProcessWindowStyle.Maximized;
    processStartInfo.Verb = "open";
    processStartInfo.UseShellExecute = true;
    Process.Start(processStartInfo);
}

//UseApplicationVerbs();