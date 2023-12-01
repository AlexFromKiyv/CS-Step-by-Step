# Процеси, домени програм і контексти завантаження

При виконанні програми середовище виконання певним чином розміщує змірку. Створюється зв'язок між процесами, доменами додатків і контекстами об'єктів.
Домен додадку це логічний підрозділ в межах данного процесу який містить набір пов'язаних збірок .Net Core. Як ви побачите, AppDomain далі поділяється на контекстні межі, які використовуються для групування однодумних об’єктів .NET Core. Використовуючи поняття контексту, середовище виконання може гарантувати, що об’єкти зі спеціальними вимогами обробляються належним чином. Розуміння цих тем є важливим під час роботи з численними API .NET Core, включаючи багатопотоковість, паралельну обробку та серіалізація об'єкта.

## Процес

Процес це програма що виконується. Це концепція на рівні операційної системи, яка використовуеться для опису набору ресурсів(наприклад, зовнішніх бібліотек та основного потоку) ш необхідного розподілу пам'яті, що використовуються працюючою програмою. Для кожної .Net Core програми, завантаженої в пам'ять, ОС створює окремий ізольований процес для використання протягом всіого часу роботи програми.
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

##

