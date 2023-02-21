# Середовище розробки.

## Коротко про архітектура .NET

Базуючись на стандарті загальною мовної інфраструктури (CLI) Microsoft створило власну реалізацію середовиша виконання і розробки. .Net по суті це віртуальна система виконання (CLR common language runtime) та комплексний набор бібліотек классів.

Код C#, компілюється в проміжну мову (IL), яка відповідає специфікації CLI. Код IL зберsгаеться в збірці (.dll). При виконанні збірка завантажуеться в CLR яка перетвоює IL в машині інструкції.(Just-In-Time компіляція). CLR надає додадкові інструції повязані з автоматичним збиранням сміття , оброки винятків та керування ресурсами. Код який проходить такий шлях називають "керованим". 

Збірка підтримує використання різноманітних типів. Існують вбудовані типи. Наприклад типу System.Int32 відповідає ключове слово int. Використовуючи вбудовані типи можна ствоювати свої більш складні типи: class, interface, structure, enumeration, delegate. Створюючи рішення будується взаємодія між різноманітним типами. 

.Net включає в себе велику кількість бібліотек в яких визначенні різноманітні типи. Звязяні по смислу типи сгрупповані в namespace. Наприклад System.Data namespace містить типи роботи з базами данних. Фундаментальним простором імен є System.

Ключеве слово <em>using</em> полегшуе посилання на типи. Аби не не повторювати одні і тіж посилання в декілкох файлах найбільш важливі простори імен підключаються глобально.

Таким чином звертаючись до бібліотек .Net створюючи власні типи і їх взаємодію ви створюєте додаток.

## Встановлення SDK (Software development kit)

При встановленні Visual Studio SDK буде встановлено і залишиться лише це перевірити.

Для перевірки чи всановленний .NET SDKs можно запустивши  в command line :

```
dotnet --info
```

.NET SDK можна завантажити з <a href="https://dotnet.microsoft.com/download">dotnet.microsoft.com/download</a>

Щоб побачити наявны на машині середовиша виконання коду:

```
dotnet --list-runtimes
```

Для перевырки наявних оновлень(просто превіряє):

```
dotnet sdk check
```

Якшо ви робите проект на базі .NET 6 а хочете пробувати нові можливості .NET 7 можете запустити:

```
dotnet new globaljson –sdk-version 6.0.100
```
Ця команда створює файл global.json якій вказує з якою версією ви працюете глобально.
Таким чином можно прікрипити проект до попередніх версій і це покаже команда dotnet --version.


## Встановлення Git.

Для того шоб мати віддаленне сховише та співпацювати в проектах бажано створити account на <a href="https://github.com/">github.com</a>

Якшо Git встановити заздалегідь інші программи установки вже будуть бачить шо він встановленний. 

Git це потужна і необхідна для розробки система контролю версій. Ця программа працюе на вашому локальному комьютері та дозволяє зберігати напрацювання та виконувати співпрацю з іншими на віддаленому сховищи.

Git можна завантажити з <a href="https://git-scm.com/">git-scm.com</a>

Початкове локальне налаштуваня Git можна виконати запустивши в command line : 

```
git config --global user.name "Ivan Sirko"
git config --global user.email sirko@ukt.net
```

## Встановлення Visual Studio та створення програми.

VS комплексна IDE для .NET. Вона має чималий об'ем та все необхідне для розробки.
Ця система потужна i не потребує багато речей робить вручну i тому можливо не дає повного розуміня шо відбувается під зручни графічним інтерфейсом. Тобто вона може бути не найкращою аби вивчати платформу з нуля але більшість розробникив працює в ній. Бо це можна робити быльш швидше і еффективніше.

Версія для студентів і open source contributors називаеться Community(спільнота) яку можна завантажити з <a href="https://visualstudio.microsoft.com/">visualstudio.microsoft.com</a>

При встановлені зверніть увагу що бажано вибрати необхідний перелік можливостей.

Workloads :
1. ASP.NET and web development
2. Data storage and processing

Individual components:
1. Class Designer
2. Git for Windows(якшо не встановлений раніше)

Якщо треба буде додати можливості можна буде знову запустити Visual Studio Installer. 

При першому запуску вам потрібен обліковий запис Microsoft <a href="https://account.microsoft.com/account?lang=uk-ua">account.microsoft.com/account</a>

Після встановлення ви можете створити solution(рішення). Рішення дозволяе управляти декількома проєктами одночасно. Один проект може бути головним а додадковий проект може бути біблиотекою классів.  

Створимо просте рішення з простими проектами. Для цього треба:
1. Створіть папку. (Наприклад D:\Manual\Chapter01\VS)  
2. Запустити Visual Studio.
3. Create new project
4. Вибрати Console App. Next.
5. Location згідно з тим яку папку зробили в п. 1. 
3. Project name : FirstProject
4. Solution name : FirstSolution
5. Next.
6. Create.
7. Заменіть код файлу Program.cs
```cs
Console.Write("Enter your name:");

string? userName = Console.ReadLine();

Console.WriteLine($"Hi {userName}. I'm machine.");
```
8. Запустіть проект: Debug > Start wihout debugging (Ctrl+F5)

При створенні проекту створюються додаткові налаштування проекту. В файлі FirstSolution\obj\debug\netX.0\FirstProject.GlobalUsings.g.cs підключаеться System namespace. В System є статичний клас Console з якого і беруться методи для виконання.

В папці FirstProject\bin\debug\netX.0\ можна побачити FirstProject.exe який можно запустити.

Звеніть увагу шо у VS є 2 трикутники для запуску проеку той шо не зафарбований заускае проект швидше оскількі це Start wihout debugging. 

Додамо ще один проект в це рішення:

1. На значку Solution правий-клік Add > New Project.
2. Вибрати Console App.
3. Project name: SecondProject
4. Next.  
5. Create.
6. Заменіть код файлу Program.cs
```cs
Console.Write("Enter your name:");
string? name = Console.ReadLine();

Console.Clear();

Console.WriteLine("Today is " + DateTime.Now.DayOfWeek);
Console.WriteLine($"Have a nice day {name}");
```
7. Запустіть проект. На значку Solution правий-клік Set Startup Projects > Current selection. Далі треба клікнути на проект і запустимо проект: Debug > Start wihout debugging (Ctrl+F5);

Коли виконалиось все Debuger не закривае вікно автоматично і чекае натискання.
Є можливість це настроїти:
Tools > Options > Debugging > General > Automatically close the console when debugging stops.  

Якшо ви хочити змініти версію .NET проекту ви можете двойним кликом нажати на назві або зайти в Properties. Прописати наприклад:
```
<TargetFramework>net5.0</TargetFramework>
```
В папці проекту вы побачитие папки bin i obj в якій знаходиться все щоб це працювало.

Шо відбуваеться "під капотом". Копілятор перетворює ваш C# код в intermediate language (IL) (код проміжниї мови) і зберігае в збірці (DLL). Опертори IL низького рівня і виконуться CLR. CLR в свою чергу можуть працювати на різних ОС(Linux,macOS,Windows). Таким чином розривається жорстка залежність від OC. 

## Налагодження (debugging).

Для того щoб робити налагодження(debugging) коду на самій лівій сірій смужці можно встановити двойним кліком точку зупинки(breakpoint) коду. запустіть код F5 та подивиться на можливості ходу по точкам зупинки.
Треба зазначити шо операція Step Over в панелі пошагового налагоджування не входить в виконання функції, а Step Into переходить до шагів функції.


## Додадково

Для створення файла .gitignore існуе команда 
```
dotnet new gitignore
```
У редактора Visual Studio  є список комбінацій клавіш для виконання різних операцій Help > Keyboard Shortcuts Reference.

## Встановлення Visual Studio Code та створення програми.

VSC це компактний редактор с великими можливостями роботи з кодом. Встановлюеться на любих ОС. Порівнюючи з Visual Studio можливо треба робити більше руками але є розуміння що відбуваеться. 
 
VSC можна завантажити з  <a href="https://code.visualstudio.com/">code.visualstudio.com</a> 

Перевірте що встановлено SDK.(dotnet --info)

Додають більше функціональності і зручності в роботі розширеня якіми можно керувати в розділі View > Extentions

В полі пошуку можно ввести назву розширення.

Ось перелік найважливіших для роботи з .NET та С# :

| Назва |
| ----------- |
| C#|
|Polyglot Notebooks|
|MSBuild project tools |
|REST Client |
|SQL Server |

У VSC можна редагувати кількома проектами як одним цілим. 

Зробимо це:

1. Створіть папку. (Наприклад D:\Manual\Chapter01\VSС\FirstSolution)  
2. Запустіть VSC.
3. Якщо відкриті якісь файли чи папки закрийте їх. File > Close Folder
4. Open Folder вкажіть папку з п.1
5. File > Save Workspace as ... Save
6. Terminal > New Terminal
7. В терміналі запустіть
```
dotnet new sln 
dotnet new console -n FirstProject -o .\FirstProject
dotnet sln FirstSolution.sln add .\FirstProject
dotnet new console -n SecondProject -o .\SecondProject
dotnet sln FirstSolution.sln add .\SecondProject
```

8. На підказку "Req..." YES. Виберіть проект для запуску.  
9. Заменіть код файлу \FirstProject\Program.cs
```cs
Console.Write("Enter your name:");

string? userName = Console.ReadLine();

Console.WriteLine($"Hi {userName}. I'm machine.");
```
10. Заменіть код файлу \SecondProject\Program.cs
```cs
Console.Write("Enter your name:");
string? name = Console.ReadLine();

Console.Clear();

Console.WriteLine("Today is " + DateTime.Now.DayOfWeek);
Console.WriteLine($"Have a nice day {name}");
```
11. В терміналі команда: dotnet run --project FirstProject
12. В терміналі команда: dotnet run --project SecondProject
13. В Explorer на папці FirstProject правий клік Open in Integreted Termimal
14. В терміналі команда: dotnet run 

При створині проекту не обобв'язково створювати solution. Можна находячись в необхідній теці запустити команду:
```
dotnet new list
```
Ця команда покаже які додадки з шаблону мона створити. Наприклад можна виконати: 
```
dotnet new console
```
Ця команда створить поект з назвою теки.

Додадкові можливості command-line interface можна подивитись: 
```
dotnet -h
```
У редактора Visual Studio Code є список комбінацій клавіш для виконання різних операцій Help > Keyboard Shortcuts Reference.   

## Використовування WSL

В Windows (з версії 19041) є можливіст використовувати Windows Subsystem for Linux. Тобто безпосередньо в Windows дозволяє створити середовище Linux. Для встановлення
треба запустити 
```
wsl --install
```
Після цього буде встановлено ubuntu і в строці виконанння з'явиться можливість вибрати ubuntu.

Для встановлення SDK
```
sudo apt-get update && sudo apt-get install -y dotnet-sdk-7.0
```
Для створення і виконання також можна використовувати dotnet new та dotnet run

##  Корисні посилання на ресурсах Microsoft  

Багато корисного можно побачити на  <a href="https://www.dot.net">dot.net</a>.

<a href="https://learn.microsoft.com/en-us/dotnet/csharp/">Документація по С#</a>

<a href="https://learn.microsoft.com/uk-ua/dotnet/fundamentals/">Документація по .Net</a>




