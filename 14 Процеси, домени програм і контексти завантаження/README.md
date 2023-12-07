# Процеси, домени програм і контексти завантаження

При виконанні програми середовище виконання певним чином розміщує змірку. Створюється зв'язок між процесами, доменами додатків і контекстами об'єктів.
Домен додадку це логічний підрозділ в межах данного процесу який містить набір пов'язаних збірок .Net Core. Як ви побачите, AppDomain далі поділяється на контекстні межі, які використовуються для групування однодумних об’єктів .NET Core. Використовуючи поняття контексту, середовище виконання може гарантувати, що об’єкти зі спеціальними вимогами обробляються належним чином. Розуміння цих тем є важливим під час роботи з численними API .NET Core, включаючи багатопотоковість, паралельну обробку та серіалізація об'єкта.

# Процес

Процес це програма що виконується. Це концепція на рівні операційної системи, яка використовуеться для опису набору ресурсів(наприклад, зовнішніх бібліотек та основного потоку) і необхідного розподілу пам'яті, що використовуються працюючою програмою. Для кожної .Net Core програми, завантаженої в пам'ять, ОС створює окремий ізольований процес для використання протягом всіого часу роботи програми.
Використовуючи цей підхід для ізоляції програми, результатом є набагато більш надійне та стабільне середовише виконання, оскількі збій одного процесе не впливає на фунціонування іншого. Крім того дані одного процесу не можуть напряму доступні іншому, якшо не використовуються умисно спеціальні бібіліотеки(System.IO.Pipes,MemoryMappedFile). Таким чином процес можна розглядати як фіксовану безпечну межу для програми шо виконується.
Кожному процесу Windows присвоюється унікальний індентіфікатор процессу(PID). Ці індентіфікатори можна побачити в "Диспечері завдань Windows"(Ctrl+Shift+Esc) на закладці Details.

## Потік

Кожен процес у Windows містить початковий "потік" який функціонує як точка входу в програму. За допомогою .Net Core можна створювати багатопоточні програми які розгядаються в наступній главі. 
Потік це шлях виконання в рамках процесу. Перший потік створений точкою входу називають основним потоком. Будь-яка програма .NET позначає свою точку входу методом Main() або файлом, що містить оператори top-level (Які перетворюється на клас Program і метод Main()). Коли цей код викликається, основний поток створюється автоматично.
Процеси, які містять один основний поток виконання,за своєю суттю є потокобезпечними, оскільки інує лише один потік, який може отримати доступ до даних у програмі в певний момент часу. Однак однопотоковий процес (особливо той, що базується на графічному інтерфейсі користувача) часто не реагує на користувачів(виглядає як загальмований), якщо цей один потік виконує складну операцію (наприклад, роздруковує довгий текстовий файл, виконує інтенсивні математичні обчислення або намагаючись підключитися до віддаленого сервера, розташованого за тисячі миль).
Враховуючи цей недолік однопоточних програм, ОС які підтримуються .NET Core ( а також платформа .NET Core) дають змогу первинному потоку створювати додадкові вторині потоки (які називають робочими). Це робиться за допомогою кількох API функцій, таких як CreateThread(). Кожен потік стає унікальним шляхом виконання в процессі та має одночасний доступ до всіх спільних даних у процесі.
Розроюники створюють додадкові потоки аби покращити загальну швидкість реагування програми. Багатопотокові процеси створюють ілюзію того що числені дії відбуваються одночасно. Наприклад, програма може створити робочий потік для виконання трудомісткої одиниці роботи (знову ж таки, як друк великого текстового файлу).Оскільки цей допоміжний потік збивається, основний потік все ще реагує на введення користувача, що надає всьому процесу потенціал для підвищення продуктивності.Однак насправді це може бути не так: використання занадто великої кількості потоків в одному процесі може фактично знизити продуктивність, оскільки ЦП повинен перемикатися між активними потоками в процесі (на що потрібен час).
На деяких машинах багатопотоковість найчастіше є ілюзією, яку забезпечує ОС.
Якщо тематика для вас нова, не турбуйтеся про деталі. На цьому етапі просто пам’ятайте, що потік — це унікальний шлях виконання в процесі Windows. Кожен процес має основний потік (створений через точку входу виконуваного файлу) і може містити додаткові потоки, які були створені програмно.

## Взаємодія з процесами.

Розуміння процесів і потоків потрібно для створення багатопоточних збірок. Взаємодіяти з процесами можна за допомогою бібліотеки базових класів .Net .
Простір імен System.Diagnostics визначає декілька типів які дозволяють програмно взаємодіяти з процесами. Також там визначено типи для веденя журналу подій та визначників продуктивності. 
Нижче вказані типи з цього простору для роботи з процессом.

    Process : Клас надає доступ до  локальних та відаленних процесів і дозволяє програмно запускати й зупиняти процеси.

    ProcessModule : Цей тип представляє модуль (*.dll,*.exe), якій завантажується в процес. Він може представляти будь-який модуль на основі COM, .Net або двійковий файл на основі C.

    ProcessModuleCollection : Це предоставляє строго типізовану коллекцію об'єктів ProcessModule.

    ProcessStartInfo : Визначає набір значень, які використовуються під час запуску процесу методом Process.Start().

    ProcessThread : Цей тип представляє потік у данному процесі.Він потрібен для діагностики набору потоків процесу і не використовується для створення нових потоків виконання в процесі.

    ProcessThreadCollection : Це предоставляє строго типізовану коллекцію об'єктів ProcessThread.

Клас System.Diagnostics.Process дозволяє аналізувати процеси що виконуються на певній машині (локальній або віддаленій). Він предоставляє члени що дозволяють програмно запускати та преривати процеси, переглядати рівень пріорітету та отримувати список активних потоків та завантажувати модулі в данному процесі. 
Дякі ключові властивості класу System.Diagnostics.Process

    Id : Ця властивість отримує унікальний індентіфікатор відповідного процесу.(PID)

    MachineName : Ця властивість отримує назву компьютера на якому процес запущено.

    ProcessName : Ця властивість отримує назву процесу (назва самої програми).

    Handle : Ця властивість повертає дескриптор (представлений IntPtr), пов’язаний із процесом ОС. Це може бути корисним під час створення додатків .NET, яким потрібно спілкуватися з некерованим кодом.

    MainWindowTitle : Отримує заголовок головного вікна процесу(якшо вікна нема тоді це прожній рядок).

    Modules : Ця властивість пердоставляє доступ до строго типізованого типу ProcessModuleCollection, який представляє набір модулів(*.dll,*.exe), завантажених у поточному процесі.

    Responding : Ця властивість отримує значення, яке вказує, чи користувальницький інтерфейс процесу реагує на введення користувача (або наразі «зависло»).

    StartTime : Ця властивість отримує час, коли пов’язаний процес був запущений (через тип DateTime).

    ExitTime : Ця властивість отримує мітку часу, пов'язану з процесом, який завершився(має тип DateTime).

    Threads : Ця властивість отримує набір потоків, які виконуються у пов’язаному процесі (представленому через колекцію об’єктів ProcessThread).

System.Diagnostics.Process також визначає кілька корисних методів 

    CloseMainWindow() : Цей метод закриває процес, який має інтерфейс користувача, надсилаючи повідомлення про закриття в його головне вікно.

    GetCurrentProcess() : Цей статичний метод повертає новий об’єкт Process, який представляє поточний активний процес.

    GetProcesses() : Цей статичний метод повертає масив нових об’єктів Process, запущених на певній машині.

    Kill() : Цей метод негайно зупиняє відповідний процес.

    Start() : Цей метод запускає процес.

## Перегляд працюючих процесів.

Process\Program.cs
```cs
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

```
```
0 Idle
4 System
108 Registry
432 smss
448 svchost
516 ServiceHub.TestWindowStoreHost
556 csrss

...

15924 conhost
16064 chrome
16936 SecurityHealthSystray
17088 ctfmon
17156 chrome
17336 ServiceHub.IndexingService

Total number of processes:194
```
Статичний метод Process.GetProcesses() повертає массив об'єктів Process, які представляють процеси шо виконуються на поточному компьютері. Праметр крапка вказує на локальний компьютер.

## Дослідження окремого процесу.

Крім отриманя колекції процесів на конкретній машині є можливість отримати об'єкт конкретного процесу по PID за допомогою статичного методу Process.GetProcessById(). Якшо за такім індентіфікаторм процесу не існує викидається винаток. 
```cs
Process? GetSpecificProcess()
{
    GetAllRunningProcesses();

    Console.Write("Enter PID of the process:");
    int.TryParse(Console.ReadLine(),out int pidOfProcess);

    //Process theProcess = null;
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
```
```
0 Idle
4 System
108 Registry
432 smss
448 svchost
516 ServiceHub.TestWindowStoreHost
556 csrss
560 svchost
648 wininit

...

15524 svchost
15732 CancelAutoPlay_df
15888 dotnet
15892 RuntimeBroker
16064 chrome
16484 MicrosoftEdgeUpdate
16488 MSBuild
16936 SecurityHealthSystray
17088 ctfmon
17336 ServiceHub.IndexingService

Total number of processes:196
Enter PID of the process:15888
dotnet
```
Тиким чином можна виконати пошук об'єкта процесу по PID.
Клас Process дозволяє виявити набір поточних потоків і бібліотек, які використовуються в данному процесі. 

## Дослідження набору потоків процесу.

Набір потоків процесу представлений суворо типізованою колекцією ProcessThreadCollection, яка містить певну кількість об'єктів ProcessThread.  

```cs
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
GetThreadsOfProcess();
```
```
0 Idle
4 System
108 Registry
432 smss
448 svchost
492 ServiceHub.IntellicodeModelService
556 csrss
560 svchost
648 wininit
716 PerfWatson2

...

16604 msedge
16712 msedge
16944 winlogon
17164 msedge
17360 chrome

Total number of processes:194
Enter PID of the process:17360
chrome

Ivestigating the process : chrome
Thread Id:17008 StartTime:30.11.2023    PriorityLevel:AboveNormal
Thread Id:9384  StartTime:30.11.2023    PriorityLevel:Normal
Thread Id:15820 StartTime:30.11.2023    PriorityLevel:Normal
Thread Id:7044  StartTime:30.11.2023    PriorityLevel:Normal
Thread Id:1124  StartTime:30.11.2023    PriorityLevel:Normal
Thread Id:9768  StartTime:30.11.2023    PriorityLevel:-3
Thread Id:380   StartTime:30.11.2023    PriorityLevel:AboveNormal
Thread Id:10964 StartTime:30.11.2023    PriorityLevel:Normal
Thread Id:5044  StartTime:30.11.2023    PriorityLevel:Normal
Thread Id:6940  StartTime:30.11.2023    PriorityLevel:Normal
Thread Id:3812  StartTime:30.11.2023    PriorityLevel:AboveNormal
Thread Id:2820  StartTime:30.11.2023    PriorityLevel:Normal
Thread Id:15204 StartTime:30.11.2023    PriorityLevel:Lowest
Thread Id:7200  StartTime:30.11.2023    PriorityLevel:Normal
```
Властивість Threads об'єкту System.Diagnostics.Process надає доступ до колекції потоків у вигляді об'єкту класу ProcessThreadCollection. 

Тип ProcessThread має додаткові цікаві члени приведені нижче.

    Id : Отримує унікальний індентіфікатор потоку.

    CurrentPriority : Отримує поточний пріоритет потоку.

    PriorityLevel : Отримує або встановлює рівень пріоритету потоку.

    IdealProcessor : Встановлює бажаний процесор для роботи цього потоку.

    ProcessorAffinity : Встановлює процесори, на яких може працювати відповідний потік.

    StartAddress : Отримує адресу пам'яті функції, викликаної операційною системою, яка запустила цей потік.

    StartTime : Отримує час, коли операційна система запустила потік

    ThreadState : Отримує поточний стан цього потоку.

    TotalProcessorTime : Отримує загальну кількість часу, який цей потік витратив на використання процесора.

    WaitReason : Отримує причину того, що потік очікує.


Тип ProcessThread не є сутністю, яку використовують для створення, призупинення або завершення потоків на платформі .Net Core. Швидше, ProcessThread — це засіб, який використовується для отримання діагностичної інформації для активних потоків Windows у запущеному процесі.

## Дослідженя набору модулів процесу.

Модуль це загалний термін який використоаується для опису *.dll або *.exe, який розміщується певним процесом. Через властивість Process.Modules можна отримату об'єкт типу ProcessModuleCollection який і є коллкуцією шо представляють модулі. 

```cs
void GetModulesOfProcess()
{
    Process? theProcess = GetSpecificProcess();

    if (theProcess == null) return;

    Console.WriteLine($"\nIvestigating the process : {theProcess.ProcessName}");

    ProcessModuleCollection theModules = theProcess.Modules;

    foreach(ProcessModule module in theModules)
    {
        Console.WriteLine($"Module:{module.ModuleName} File:{module.FileName}");
    }
}
GetModulesOfProcess();
```
```
0 Idle
4 System
72 WUDFHost
108 Registry
424 smss
528 msedge

...

12096 svchost
12260 dotnet

Total number of processes:189
Enter PID of the process:7592
Process

Ivestigating the process : Process
Module:Process.exe
Module:ntdll.dll
Module:KERNEL32.DLL
Module:KERNELBASE.dll
Module:USER32.dll
Module:win32u.dll
Module:GDI32.dll
Module:gdi32full.dll
Module:msvcp_win.dll
Module:ucrtbase.dll
Module:SHELL32.dll
Module:ADVAPI32.dll
Module:msvcrt.dll
Module:sechost.dll
Module:RPCRT4.dll
Module:IMM32.DLL
Module:hostfxr.dll
Module:hostpolicy.dll
Module:coreclr.dll
Module:ole32.dll
Module:combase.dll
Module:OLEAUT32.dll
...

``` 
Як бачимо навіть простий консольий проект завантажує велику кількість модулів.

## Запуск та зупинка процесу програмно.

В класі System.Diagnostics.Process є методи Start() та Kill().

```cs
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
UseStartAndKill();
```
```
Нажмiть Enter аби закрити msedge
```
Метод Start має декілька перезавантажень одне з яких ми використали. Після запуску процесу метод повертає посилання на активований процес. 
У цьому прикладі, оскільки Microsoft Edge запускає багато процесів, ви виконуєте цикл, щоб знищити всі запущені процеси. Важливо обгорнути процес Kill() в блок try .. catch оскільки процес може бути закінчений в інший спосіб

## Контроль за запуском процесу використовуючи клас ProcessStartInfo.

В якості параметра в метод Start можна передати об'єкт типу ProcessStartInfo, з налаштуваннями стосовно того як треба запускати процес.

Нижче деякі визначення цього класу.

```cs
public sealed class ProcessStartInfo : object
{
  public ProcessStartInfo();
  public ProcessStartInfo(string fileName);
  public ProcessStartInfo(string fileName, string arguments);
  public string Arguments { get; set; }
  public bool CreateNoWindow { get; set; }
  public StringDictionary EnvironmentVariables { get; }
  public bool ErrorDialog { get; set; }
  public IntPtr ErrorDialogParentHandle { get; set; }
  public string FileName { get; set; }
  public bool LoadUserProfile { get; set; }
  public SecureString Password { get; set; }
  public bool RedirectStandardError { get; set; }
  public bool RedirectStandardInput { get; set; }
  public bool RedirectStandardOutput { get; set; }
  public Encoding StandardErrorEncoding { get; set; }
  public Encoding StandardOutputEncoding { get; set; }
  public bool UseShellExecute { get; set; }
  public string Verb { get; set; }
  public string[] Verbs { get; }
  public ProcessWindowStyle WindowStyle { get; set; }
  public string WorkingDirectory { get; set; }
}
```
Запустимо процес за допомогою класу налаштувань.

```cs
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

StartWithProcessStartInfo();

```
```
Нажмiть Enter аби закрити msedge
```
В цьому прикладі процес запускається за допомогою того шо в Windows є ассосціяція між скороченя(ярлика) "MsEdge" і відповідним файлом який треба запустити. Крім того аби такий звязок система могла визначити треба задати UseShellExecute в true. Тоді спрацює така частину коду:
```cs
Process.Start('msedge');
```   

## Використання властивості Verb налаштувань ProcessStartInfo.

Крім викорситання ярликів програм для запуску також можна використати ассоціації файлів Windows. Якшо на файлі кляцнути правою кнопкою то з ним можна виконати різні дії наприклад роздрукувати. За допомогою ProcessStartInfo можна виявити шо можно виконати з файлом.

```cs
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

UseApplicationVerbs();
```
```
open
print
printto
```
Тут за допомогою властивості WindowStyle вікно робиться на весь екран і далі зв допомогою Verb вказується дія. 

# .NET Домени додатків (Application Domains).

На платформі .Net виконувальні файли не розміщуються безпосередьно в процесі Windows, як це відбуваеться в традиційних некерованих програмах. Навпаки, .Net виконувальні файли розміщуються в логічному розділі процесу, який називається доменом програми. Це дає кілька переваг.

    Домени додатків є ключовим аспектом нейтрального щодо ОС характеру платформи .NET Core, враховуючи, що цей логічний поділ абстрагує відмінності в тому, як базова ОС представляє завантажений виконуваний файл.

    Домени додатків набагато дешевші з точки зору обчислювальної потужності та пам’яті, ніж повномасштабний процес. Таким чином, CoreCLR може завантажувати та вивантажувати домени додатків набагато швидше, ніж формальний процес, і може значно покращити масштабованість серверних додатків.

Домени додатків повністю і повністю ізольовані від інших додатків у межах процесу. Враховуючи цей факт, майте на увазі, що програма, яка працює в одному AppDomain, не може отримати будь-які дані (глобальні змінні чи статичні поля) в іншому AppDomain, якщо вони не використовують розподілений протокол програмування.

## Клас System.AppDomain.

Хоча цей клас можна рахувати застарілим є функції які корисні.

AppDomainClass\Program.cs
```cs
static void InvestigationAppDomain()
{
    AppDomain appDomain = AppDomain.CurrentDomain;

    Console.WriteLine($"FriendlyName: {appDomain.FriendlyName}");
    Console.WriteLine($"Id: {appDomain.Id}");
    Console.WriteLine($"IsDefaultAppDomain: {appDomain.IsDefaultAppDomain}");
    Console.WriteLine($"BaseDirectory: {appDomain.BaseDirectory}");
    Console.WriteLine($"SetupInformation.ApplicationBase: {appDomain.SetupInformation.ApplicationBase}");
    Console.WriteLine($"SetupInformation.TargetFrameworkName: {appDomain.SetupInformation.TargetFrameworkName}");
    Console.WriteLine($"{appDomain}");
}
InvestigationAppDomain();
```
```
FriendlyName: AppDomainClass
Id: 1
IsDefaultAppDomain: True
BaseDirectory: D:\MyWork\...\AppDomainClass\AppDomainClass\bin\Debug\net8.0\
SetupInformation.ApplicationBase: D:\MyWork\...\AppDomainClass\AppDomainClass\bin\Debug\net8.0\
SetupInformation.TargetFrameworkName: .NETCoreApp,Version=v8.0
```
Програма має доступ до домену програми за замовчуванням завдяки властивості CurrentDomain.
Маючи такий об'єкт можна дослідити властивості домену. 
Назва домену додадку співпадає з назвою виконуваного файлу. Також зауважте, що значення базового каталогу, яке використовуватиметься для пошуку зовнішніх приватних збірок, відповідає поточному розташуванню розгорнутого виконуваного файлу.

## Перегляд завантажених збірок.

У певному домені можна виявити всі завантажені збірки за допомогою методу GetAssemblies().
```cs
using System.Reflection;

...

void GetAssemliesOfAppDomain()
{
    AppDomain appDomain = AppDomain.CurrentDomain;

    Assembly[] assemblies = appDomain.GetAssemblies();

    Console.WriteLine($"Assemlies of {appDomain.FriendlyName}\n");

    foreach (var assembly in assemblies)
    {
        Console.WriteLine($"{assembly.GetName().Name} {assembly.GetName().Version}");
    }
}
GetAssemliesOfAppDomain();
```
```
Assemlies of AppDomainClass

System.Private.CoreLib 8.0.0.0
AppDomainClass 1.0.0.0
System.Runtime 8.0.0.0
Microsoft.Extensions.DotNetDeltaApplier 17.0.0.0
System.IO.Pipes 8.0.0.0
System.Linq 8.0.0.0
System.Collections 8.0.0.0
System.Console 8.0.0.0
```
Метод повертає масив об'єктів Assembly. Домен програими, у якому розміщено виконувальний файли використовує бібліотеки які предсталени ціми об'єктами. Цей список може змінюватися коли створюється новий код C# і додається використання інших бібіліотек.

```cs

void GetAssemliesOfAppDomainWithChange()
{
    AppDomain appDomain = AppDomain.CurrentDomain;

    Assembly[] assemblies = appDomain.GetAssemblies();

    Array.Reverse(assemblies);

    Console.WriteLine($"\nAssemlies of {appDomain.FriendlyName}\n");

    foreach (var assembly in assemblies)
    {
        Console.WriteLine($"{assembly.GetName().Name} {assembly.GetName().Version}");
    }
}
GetAssemliesOfAppDomain();
GetAssemliesOfAppDomainWithChange();
```
```
Assemlies of AppDomainClass

System.Private.CoreLib 8.0.0.0
AppDomainClass 1.0.0.0
System.Runtime 8.0.0.0
Microsoft.Extensions.DotNetDeltaApplier 17.0.0.0
System.IO.Pipes 8.0.0.0
System.Linq 8.0.0.0
System.Collections 8.0.0.0
System.Console 8.0.0.0

Assemlies of AppDomainClass

System.Threading.Overlapped 8.0.0.0
System.Runtime.InteropServices 8.0.0.0
System.Text.Encoding.Extensions 8.0.0.0
System.Threading 8.0.0.0
System.Collections.Concurrent 8.0.0.0
System.Console 8.0.0.0
System.Collections 8.0.0.0
System.Linq 8.0.0.0
System.IO.Pipes 8.0.0.0
Microsoft.Extensions.DotNetDeltaApplier 17.0.0.0
System.Runtime 8.0.0.0
AppDomainClass 1.0.0.0
System.Private.CoreLib 8.0.0.0
```
Як ви бачите зміна коду впливає на тє які збірки завантажуються в пам'ять.

## Ізоляція збірки з AssemblyLoadContext.

Як шойно бачили, AppDomains - це логічний розділ, який використовується для розміщеня збірок .Net. Крім того домен програми може бути додадково поділена на числені межі контексту завантаження. Концептуально, контекст завантаження створює область для завантаженя, вирішення та потенційно вивантаження набору збірок. Контекст завантаження .Net Core надає можливість для одного AppDomain створити "конкретний дім" для певного об'єкта. Більшість програм .Net не вимагає роботи з контекстами об'єктів але розуміня процесів і доменів важлива.
Клас AssemblyLoadContext предоставляє можливість завантажувати додадкові збірки у власні контексти. 
Для розляду питання додако власну бібіліотеку до рішення.

1. На ярлику Solution "App..." правий-клік
2. Add > New project > Class Lirbary > Next
3. Project name: MyLibrary
4. Create
5. На проекті "AppDomainClass" правий-клік.
6. Add > Project Reference > MyLibrary
7. Вадалити з MyLibrary Class1.cs

Додамо в MyLibrary клас Car

```cs
namespace MyLibrary
{
    public class Car
    {
        public string Name { get; set; } = string.Empty;
        public string Make { get; set; } = string.Empty;
        public int Speed { get; set; }
    }
}
```
В початок файлу AppDomainClass\Program.cs додайте:
```cs
using System.Reflection;
using System.Runtime.Loader;
```
```cs
static void LoadAdditionalAssembliesDifferentContexts()
{
    var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MyLibrary.dll");
  
    AssemblyLoadContext assemblyLoadContext1 = new("NewContext1", false);
    var assembly1 = assemblyLoadContext1.LoadFromAssemblyPath(path);
    var class1 = assembly1.CreateInstance("MyLibrary.Car");

    AssemblyLoadContext assemblyLoadContext2 = new("NewContext2", false);
    var assembly2 = assemblyLoadContext2.LoadFromAssemblyPath(path);
    var class2 = assembly2.CreateInstance("MyLibrary.Car");

    Console.WriteLine($"assembly1.Equals(assembly2) : {assembly1.Equals(assembly2)}");
    Console.WriteLine($"assembly1 == assembly2 : {assembly1 == assembly2}");
   
    Console.WriteLine($"class1.Equals(class2) : {class1?.Equals(class2)}");
    Console.WriteLine($"class1 == class2 : {class1 == class2}");
}

LoadAdditionalAssembliesDifferentContexts();
```
```
assembly1.Equals(assembly2) : False
assembly1 == assembly2 : False
class1.Equals(class2) : False
class1 == class2 : False
```
Перший рядок створює католог для збірки MyLibrary.dll. Створений об'єкт AssemblyLoadContext використовується для завантаженя збірки я потім і для створеня об'єкта цієї збірки. Потім ценй процес повторюється. 
Це демонструє, що ту саму збірку завантажено двічі в домен програми. Класи також різні, як і слід було очікувати.

Завантаженя можна зробити з тим самим  AssemblyLoadContext.
```cs
static void LoadAdditionalAssembliesSameContext()
{
    var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MyLibrary.dll");

    AssemblyLoadContext assemblyLoadContext1 = new(null, false);
    var assembly1 = assemblyLoadContext1.LoadFromAssemblyPath(path);
    var class1 = assembly1.CreateInstance("MyLibrary.Car");
       
    var assembly2 = assemblyLoadContext1.LoadFromAssemblyPath(path);
    var class2 = assembly2.CreateInstance("MyLibrary.Car");

    Console.WriteLine($"assembly1.Equals(assembly2) : {assembly1.Equals(assembly2)}");
    Console.WriteLine($"assembly1 == assembly2 : {assembly1 == assembly2}");

    Console.WriteLine($"class1.Equals(class2) : {class1?.Equals(class2)}");
    Console.WriteLine($"class1 == class2 : {class1 == class2}");
}

LoadAdditionalAssembliesSameContext();
```
```
assembly1.Equals(assembly2) : True
assembly1 == assembly2 : True
class1.Equals(class2) : False
class1 == class2 : False
```
В цьому варівнті створюється один AssemblyLoadContext. Тепер, коли збірка MyLibrary завантажується двічі, друга збірка є просто покажчиком на перший примірник збірки.

## Пудсумки

Здебільшого .NET Core автоматично обробляє деталі процесів, домени програм і контексти завантаження від вашого імені. Ця інформація забезпечує надійну основу для розуміння багатопотокового програмування на платформі .NET Core.
Як ви бачили, колишне уявлення про процес Windows було змінено, щоб відповідати потребам CoreCLR. Один процес (яким можна програмно керувати за допомогою типу System.Diagnostics.Process) тепер складається з домену програми, який представляє ізольовані та незалежні межі в межах процесу. Домен програми здатний розміщувати та виконувати будь-яку кількість пов’язаних збірок. Крім того, один домен програми може містити будь-яку кількість контекстів завантаження для подальшої ізоляції збірки. Використовуючи цей додатковий рівень ізоляції типів, CoreCLR може забезпечити правильну обробку спеціальних об’єктів.


