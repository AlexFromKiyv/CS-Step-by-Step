# Простір імен System.Threading

## Взаємовідносини між Process,AppDomain,Context,Thread

Потік це шлях виконання у виконуваній програмі. Багато програм можуть виконуватися продуктивно в одному потоці. Основний потік збірки (створений середовищем виконання, коли виконується точка входу програми) може створювати вторинні потоки виконання в будь-який час для виконання додаткових одиниць роботи. Так можна ствоювати більш чутливі програми для користувача. 
Простір імен System.Threading  пропонує один підхід для створення багатопоточних програм. 
Важливим класом є Thread, оскільки представляє потік. Якщо ви хочете програмно отримати посилання на потік, який зараз виконує певний член, просто викличте статичну властивість Thread.CurrentThread, наприклад:

ThreadingNamespace\Program.cs
```cs
static void ExtractExecutingThread()
{
    Thread thread = Thread.CurrentThread;
    Console.WriteLine(thread.ManagedThreadId);
    
}
ExtractExecutingThread();
```
```
1
```
В .Net Core існує лише один AppDomain, який може мати численні потоки, що виконуються в ньому в будь який момент часу. Можна отримати посилання на AppDomain на якому розміщена програма поток якої виконується.
```cs
static void ExtractAppDomainHostingThread()
{
    // Obtain the AppDomain hosting the current thread.
    AppDomain appDomain = Thread.GetDomain();
    Console.WriteLine(appDomain.FriendlyName);
}
ExtractAppDomainHostingThread();
```
```
ThreadingNamespace
```
Один потік також може бути переміщений у контекст виконання в будь-який момент часу і він може бути переміщений у новому контексті виконання за примхою .Net Core Runtime. Можна отримати поточний контекст виконання, у якому виконуєтьься потік.

```cs
static void ExtractCurrentThreadExecutionContext()
{
    ExecutionContext executionContext = Thread.CurrentThread.ExecutionContext;
    Console.WriteLine(executionContext);
}
ExtractCurrentThreadExecutionContext();
```
```
System.Threading.ExecutionContext
```
Знову ж таки, .NET Core Runtime контролює переміщення потоків у (і з) контексти виконання. Як розробник .NET Core, ви зазвичай можете не знати, куди закінчується певний потік. Тим не менш, ви повинні знати про різні способи отримання базових складаючих типу.

## Проблема паралельності.

Один із неприємних аспектів в багатопотоковому програмувані полягає в тому що ви мало контролюєте, як базова ОС або runtime використовує свої потоки. Наприклад коли ви створюєте блок коду, який створює новий потік виконання, ви не можете гарантувати, що потік буде виконано негайно. Швидше такий код лише наказує ОС/Runtime виконати потік якнайшвидше(що зазвичай відбувається коли планувальник потоків доходить до цього). Крім того, враховуючи те, що потоки можна переміщувати між програмою та контекстними межами відповідно до вимог середовища виконання, ви повинні пам’ятати, які аспекти вашої програми є потоково-незалежними (наприклад, підлягають багатопоточному доступу), а які операції є атомарними (потоково-незалежними операції небезпечні!).

Щоб проілюструвати проблему, припустимо, що потік викликає метод певного об'єкта. Тепер припустимо, припустимо що цей потік отримав інструкцію від планувальника потоків призупинити свою діяльність, щоб дозволити іншому потоку отримати доступ до того мамого методу того самого об'єкту. Якшо перший потік не закінчив виконання методу повністю, другій приходячий потік може отримати і працювати з об'єктом у частково зміненому стані. Зчитуючи фальшиві дані другий потік приведе до дуже дивних помилок які важко відтворити і налагодити.

З іншого боку атомарні операції завжди безпечні в багатопотоковому середовищі. На жаль, у бібіліотеках базових класів .Net Core є кілька операцій, які гарантовано є атомарними. Навіть акт присвоєння значення змінній-члену не є атомарним! Якщо в документації .Net конкретно не вказано, що операція є атомарною, ви повині вважати, що вона нестійка до потоку, і вжити заходів обережності.   

## Роль синхронізації потоків.
Багатопотокові програми досить мінливі(мають місце постіцно змінюватись), оскільки числені потоки можуть працювати на спільних ресурсах (більш менш) одночасно. Щоб захістити ресурси програми від можливого пошкодження, розродники .Net повинні використовувати будь-яку кількість потокових примітивів (таких як блокуання, монітори та атрібут [Synchronization] або підтримка ключових слів мови), щоб контролювати доступ між потоками що виконуються.
Хоча платформа .NET Core не може повністю усунути труднощі зі створення надійних багатопоточних програм, процес значно спрощено.Використовуючи типи, визначені в просторі імен System.Threading, TPL (паралельній бібліотеці завдань)  та ключові слова мови C# async і await, ви можете працювати з кількома потоками з мінімальною суєтою та турботою.


## Складові простору імен System.Threading.

На платформі .Net простір імен System.Threading надає типи, які дозволяють безпосередне створення багатопоточних програм. Ці типи дозаоляють взаємодіяти з потоком .Net Core Runtime. Також цей простір імен визначає типи які дають доступ до пулу потоків, що підтримується .Net Core Runtime, простого класу Timer та багатьох типів, що використовуються для забезпечення синхронізованого доступу до спільних ресурсів. Нижче наведені деякі з важливих типів цього простору імен.

    Interlocked : Цей тип забеспечує атомарні операції для змінних, які спільно використовуються кількома потоками.

    Monitor : Цей тип забезпечує сінхронізацію потокових об'єктів за допомогою блокувані і очінування/сигналів. Ключеве слово lock використовує тип під капотом.

    Mutex : Цей примітив синхронізаціх можна використовувати жля сінхронізації між межами домену програми.

    ParameterizedThreadStart : Цей делегат дозволяє потоку викликати методи, які приймають будь-яку кількість аргументів.

    Semaphore : Цей тип дозволяє обмежити кількість потоків, які можуть одночасно отримувати доступ до ресурсу.

    Thread : Цей тип представляє потік, який виконується в середовищі виконання .Net. Використовуючи цей тип, ви можете створювати додадкові потоки в поточному AppDomain.

    ThreadPool : Цей тип дозволяє взаємодіяти з пулом потоків, шо підтримуються .Net Core Runtime, у певному процесі.

    ThreadPriority : Це enum представляє рівні пріорітетів потоку (Highest, Normal, etc.).

    ThreadStart : Цей делегат використовується для визначення методу виклику для данного потоку.На відміну від делегату ParameterizedThreadStart, цільовий метод ThreadStart завжди повинні мати однаковий прототип.

    ThreadState : Це enum представляє дійсні стани, які може приймати поток (Running, Aborted, etc.).

    Timer : Цей тип забеспечує механізм для виконання методу через заданий проміжок часу.

    TimerCallback : Цей тип делегату авикористовується в поєднанні з типами Timer.

## Складові класу System.Threading.Thread

Найбільш основним з усіх типів System.Threading є Thread. Він представляє об'єктно- орієнтовану оболонку навколо заданого шляху в середині AppDomain. Цей тип визначає кілька методів (статичних і рівня екземпляра), які дозволяють створювати нові потоки в межах поточного AppDomain, я також призупиняти ,зупиняти та знищувати потокі. 

Ключові статичні члени Thread.

    ExecutionContext : Це read-only властивість повертає інформацію, що стосується логічного потоку виконання, включаючи безпеку, виклик, сінхронізацію, локалізацію та контексти транзауцій.

    CurrentThread : Це read-only властивість повертає посилання на поточний потік.

    Sleep() : Метод призупиняє поточний потік на визначений час.


Ключові члени рівня екземпляра класу.

    IsAlive : Повертає логічне значення, яке вказує, чи виконується потік.

    IsBackground : Властивість, яку можна змінювати і яка вказує чи є поток "фоновим".

    Name : Дозволяє встановити зрозумілу назву потоку. 

    Priority : Властивісь яка зберігає значення enum ThreadPriority.

    ThreadState : Властивісь яка зберігає значення enum ThreadState.

    Abort() : Наказує середовищу виконання .NET Core завершити потік якомога швидше.

    Interrupt() : Перериває (наприклад, пробуджує) поточний потік із відповідного періоду очікування.

    Join() : Блокує потік, що викликає, доки вказаний потік(той, у якому викликається Join) не завершиться.

    Resume() : Відновлює потік, який раніше було призупинено.

    Start() : Наказує середовищу виконання .NET Core якомога швидше виконати потік.

    Suspend() : Призупиняє потік. Якщо потік уже призупинено, виклик Suspend() не має ефекту.

Переривання або призупинення активного потоку зазвичай вважається поганою ідеєю. Коли ви це зробите, існує ймовірність (хоть і невелика), що потік може «пропустити» своє робоче навантаження, коли його порушують або припинять.


## Отриманя даних про поточний потік.

Top-level оператори або метод main є точкою входу виклнувальної збірки і працюють в основному потоці виконання. Отримаємо посилання на об'єкт цього потоку і розглянемо його особливості.
```cs
void ExplorationTheThread()
{
    Thread primaryThread = Thread.CurrentThread;
    primaryThread.Name = "ThePrimaryThread";

    Console.WriteLine($"Name :{primaryThread.Name}");
    Console.WriteLine($"ManagedThreadId :{primaryThread.ManagedThreadId}");
    Console.WriteLine($"IsAlive :{primaryThread.IsAlive}");
    Console.WriteLine($"Priority :{primaryThread.Priority}");
    Console.WriteLine($"ThreadState :{primaryThread.ThreadState}"); 
    Console.WriteLine($"IsThreadPoolThread :{primaryThread.IsThreadPoolThread}");
    Console.WriteLine($"CurrentCulture :{primaryThread.CurrentCulture}");
}

ExplorationTheThread();
```
```
Name :ThePrimaryThread
ManagedThreadId :1
IsAlive :True
Priority :Normal
ThreadState :Running
IsThreadPoolThread :False
CurrentCulture :uk-UA
```
Статична властивість Thread.CurrentThread повертає потік що виконується. Властивість Name потоку за замовчуванням пустий рядок. Заданя назви потоку може значно спростити налагодження. Тоді в Visual Studio можна в окремому вікні можна побачити цей потік.  (підчас debugging session  Debug ➤ Windows ➤ Threads). 

![PrimaryThread](PrimaryThread.png)

Властивість Priority за замовчуванням має значення рівня пріорітету Normal. Цей рівень можна змінити виходячи з визначення enum ThreadPriority.

```cs
    public enum ThreadPriority
    {
        Lowest = 0,
        BelowNormal = 1,
        Normal = 2,
        AboveNormal = 3,
        Highest = 4
    }
```
Якщо ви повинні були призначити рівень пріоритету потоку до значення, відмінного від стандартного (ThreadPriority.Normal), зрозумійте, що ви не матимете прямого контролю над тим, коли планувальник потоків перемикається між потоками. Рівень пріоритету потоку пропонує середовищу виконання .NET Core підказку щодо важливості активності потоку. Таким чином, потік зі значенням ThreadPriority.Highest не обов’язково має найвищий пріоритет.
Знову ж таки, якщо планувальник потоків зайнятий певним завданням (наприклад, синхронізація об’єкта, перемикання потоків або переміщення потоків), рівень пріоритету, швидше за все, буде відповідно змінено. Однак за всіх рівних умов .NET Core Runtime зчитує ці значення та вказує планувальнику потоків, як найкраще розподілити часові проміжки. Потоки з ідентичним пріоритетом мають отримати однакову кількість часу для виконання своєї роботи.
У більшості випадків вам рідко (якщо взагалі взагалі) потрібно буде безпосередньо змінити рівень пріоритету потоку.

## Мануальне створення вторинного потоку.

Для відкритя додадкового потоку, який буде виконувати одиницю навантаження, притримуйтесь наступних пунктів при використані типів System.Threading

    1. Створіть метод, який стане точкою входу для нового потоку.
    2. Створіть новий ParameterizedThreadStart(або ThreadStart) делегат передавши конструктору адресу методу, визначенного в 1 пункті.
    3. Створіть об'єкт Thread передавши конструктору делегат ParameterizedThreadStart(або ThreadStart)
    4. Встановіть початкові характеристики потоку (Name, Pryority, ... )
    5. Визовідь метод Thread.Start().Це якнайшвидше запускає потік у методі, на який посилається делегат, створений на кроці 2.

Можна використовувати два типи делегатів ParameterizedThreadStart або ThreadStart, щоб "вказати" метод, який буде виконувати вторинний потік. ThreadStart делегат може вказувати на будь який метод який не приймає аргументів і нічого не повертає. Цей делегат може бути корисним, якщо метод призначений для простого виконання у фоновому режимі без подальшої взаємодії.
Обмеження ThreadStart полягає в тому, що ви не можете передати параметри для обробки. Однак тип делегату ParameterizedThreadStart допускає один параметр типу System.Object. Враховуючи те, що будь-що може бути представлено як System.Object, ви можете передати будь-яку кількість параметрів через спеціальний клас або структуру.
Однак зауважте, що делегати ThreadStart і ParameterizedThreadStart можуть вказувати лише на методи, які повертають void.

## Робота з делегатом ThreadStart.

Давайте продемонструємо корисність багатопоточності. Створемо проект типу Console Application з назвою SimpleMultiThreadApp, який дозволяє кінцевому користувачеві вибирати, чи виконуватиме програма свої обов'язки за допомогою єдиного основного потоку, чи розділить своє робоче навантаження за допомогою двох окремих потоків виконання.

Додамо клас
```cs
namespace SimpleMultiThreadApp
{
    public class Printer
    {
        public void PrintNumbers()
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} is executing PrintNumbers()");

            Console.WriteLine("Starting slow work: : ");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"\nIt's done step {i}");
                Thread.Sleep(2000);
            }
            Console.WriteLine();
        }
    }
}

```
Клас має метод який досить повільно працює і таким чином є для нас аналогом складної одиниці роботи в реальній програмі. Цей метод для додадкового потоку. 
Спробуємо використання цього методу з одним і двума потоками.
```cs
void WorkingWithThreadStart()
{
    Console.Write("Do you want 1 or 2 threads? [1/2]:");

    string? threadCount = Console.ReadLine();

    //Assigning the name of current thread.
    Thread primaryThread = Thread.CurrentThread;
    primaryThread.Name = "Primary";

    Console.WriteLine($"{Thread.CurrentThread.Name} is execution method in Top-level");

    Printer printer = new();

    switch (threadCount)
    {
        case "2":
            Thread backgroungThread = new Thread(new ThreadStart(printer.PrintNumbers));
            backgroungThread.Name = "Secondary";
            backgroungThread.Start();
            break;
        case "1":
        default:
            printer.PrintNumbers();
            break;
    }
    //Do some addition work.
    Console.WriteLine($"{Thread.CurrentThread.Name} is almost complete. Press Enter."  );
    Console.ReadLine();
}
WorkingWithThreadStart();
```
1
```
Do you want 1 or 2 threads? [1/2]:1
Primary is execution method in Top-level
Primary is executing PrintNumbers()
Starting slow work.

It's done step 0

It's done step 1

It's done step 2

It's done step 3

It's done step 4

It's done step 5

It's done step 6

It's done step 7

It's done step 8

It's done step 9

Primary is almost complete. Press Enter.
I can enter something else.
```
2
```
Do you want 1 or 2 threads? [1/2]:2
Primary is execution method in Top-level
Primary is almost complete. Press Enter.
Secondary is executing PrintNumbers()
Starting slow work.

It's done step 0
I
It's done step 1
ca
It's done step 2
n
It's done step 3
ente
It's done step 4
r
It's done step 5
someth
It's done step 6
ing
It's done step 7
el
It's done step 8
se
It's done step 9
```
В прикладі користувачеві надається можливість виконати "повільний" метод або в одному потоці з основиним потоком або застосувати окремий потік. Для додадкового потоку створюється делегат ThreadStart який вказує на метод PrintNumbers. Потім цей об'єкт передається конструктору Thread. Після додавання назви потоку визивається метод Start, щоб повідомити .Net Runtime, шо цей потік готовий до обробки.
Таким чином можна вивільнити основний поток від "повільного" методу і дозволити робити додадкову роботу або реагувати на дії користувача. За виконаня "тяжкого" методу в окремому потоці відповідає об'єкт Thread.

## Робота з делегатом ParameterizedThreadStart

Делегат ThreadStart може вказувати лише на метод який повертає void i не має аргументів. Якшо цього не достатьно і потрібно передати данні у вторинний потік можна використати тип делегату ParameterizedThreadStart. Цей делегат може вказувати на метод який отримую в якості параметра System.Object. Створемо клас, об'єкти якого будуть слугувати аргументами.

```cs
namespace SimpleMultiThreadApp
{
    internal class AddParams
    {
        public int a, b;
        public AddParams(int a, int b)
        {
            this.a = a;
            this.b = b;
        }
    }
}
```

Створемо метод який буде відповідати вимогам делегата и робити певну роботу.

```cs
void Add(object? data)
{
    if (data is AddParams ap)
    {
        Console.WriteLine($"Start work in method Add() into thread with ID : {Thread.CurrentThread.ManagedThreadId}");
        Console.WriteLine($"{ap.a} + {ap.b} is {ap.a + ap.b}");
    }
    Console.WriteLine("The method Add is finished.");
}

Використання делегата ParameterizedThreadStart досить просте.

void WorkingWithParameterizedThreadStart()
{
    Console.WriteLine($"Start work in main thread with ID : {Thread.CurrentThread.ManagedThreadId}");

    AddParams addParams = new(1, 2);
    Thread thread = new(new ParameterizedThreadStart(Add));
    thread.Start(addParams);
    //Thread.Sleep(5);
    Console.WriteLine("The main thread is finished.");
}
WorkingWithParameterizedThreadStart();
```
```
Start work in main thread with ID : 1
The main thread is finished.
Start work in method Add() into thread with ID : 8
1 + 2 is 3
The method Add is finished.
```
При виконані видно що потоки відрізняються і по різному закінчують свою роботу.

Оскільки метод Start не є типобезпечним і потрібно приводити дані до потрібного типу рекомендується щоб в об'єкті були і дані функція шо їх обробляє.

```cs
    internal class AddParams
    {
        public int a, b;
        public AddParams(int a, int b)
        {
            this.a = a;
            this.b = b;
        }
        public void AddPrint() => Console.WriteLine($"Sum is {a+b}");
    }
```
```cs
void WorkWitoutCasting()
{
    AddParams addParams = new(1, 2);
    Thread thread = new(addParams.AddPrint);
    thread.Start();
    Thread.Sleep(1000);
}
WorkWitoutCasting();
```
```
Sum is 3
```

## Клас AutoResetEvent.

У цих перших кількох прикладах немає чистого способу дізнатися, коли вторинний потік завершив свою роботу. В останньому прикладі код Thread.Sleep(5); спеціально гальмує основний поток, щоб вториний виконався раніше його закінчення. Одним із простих і безпечним для потоків способом змусити потік очікувати, поки не завершиться інший, є використання класу AutoResetEvent.

```cs
AutoResetEvent _waitHandler = new AutoResetEvent(false);
void AddWithSet(object? data)
{
    if (data is AddParams ap)
    {
        Console.WriteLine($"Start work in method Add() into thread with ID : {Thread.CurrentThread.ManagedThreadId}");
        Console.WriteLine($"{ap.a} + {ap.b} is {ap.a + ap.b}");
    }
    Console.WriteLine("The method Add is finished.");
    _waitHandler.Set();
}

void WorkingWithClassAutoResetEvent()
{
    

    Console.Write("Wait for finish second thread (Y/N):");
    string? toWait = Console.ReadLine();


    Console.WriteLine($"Start work method from main thread with ID : {Thread.CurrentThread.ManagedThreadId}");
    AddParams addParams = new(1, 2);
    Thread thread = new(new ParameterizedThreadStart(AddWithSet));
    thread.Start(addParams);

    if(toWait != null && (toWait == "Y" || toWait == "y"))
    {
        _waitHandler.WaitOne();
    }
    Console.WriteLine("The main thread is finished.");
}
WorkingWithClassAutoResetEvent();
```
```
Wait for finish second thread (Y/N):n
Start work method from main thread with ID : 1
The main thread is finished.
Start work in method Add() into thread with ID : 10
1 + 2 is 3
The method Add is finished.
```
```
Wait for finish second thread (Y/N):Y
Start work method from main thread with ID : 1
Start work in method Add() into thread with ID : 10
1 + 2 is 3
The method Add is finished.
The main thread is finished.

```
У потоці що повинен чекати створюється екземпляр класу. При створені в конструктор передається False, що означає що він ще не сповіщені. В момент коли треба чекати виконання вторинного потоку визиваеться метод WaitOne(). Коли вторинний потік закінчиє роботу він викличе метод Set у тому самому екземплярі класу.

## Передні і фонові потоки.

Отже як ми бачили потокі можуть бути різного рівня. Формально визначаються наступні потоки.

    Передні потоки (Foreground threads) можуть запобігти завершенню поточної програми. .Net Runtime не вимикає програму (не вигружає розміщений AppDomain ) поки не завершаться всі потоки переднього плану.

    Фонові потоки (Background threads іноді називають daemon threads) розгладаються .Net Runtime як витратні шляхи виконання, які можна ігнорувати в будь який момент часу. Таким чином, якщо всі потоки переднього плану завершилися, усі фонові потоки автоматично припиняються, коли домен програми вивантажується. 

Важливо зауважити, що передні і фоновий потоки не є синонімами основного та робочого потоків. За замовчуванням кожен потік, який ви створюєте за допомогою методу Thread.Start(), автоматично стає потоком переднього плану. Це означає, що AppDomain не буде вивантажено, доки всі потоки виконання не завершать свої одиниці роботи. У більшості випадків це саме та поведінка, яка вам потрібна.

Спробуємо створити фотоний потік
```cs
void UseIsBackground()
{
    Console.Write("Do you want make worker thread backgrounded? (Y/N):");
    string? isBackgrounded = Console.ReadLine();

    Console.WriteLine($"Start the method from primary thread with ID : {Thread.CurrentThread.ManagedThreadId}");

    Printer printer = new();
    
    Thread workThread = new Thread(new ThreadStart(printer.PrintNumbers));
    workThread.Name = "Worker thread";
    workThread.IsBackground = (isBackgrounded == "Y" || isBackgrounded == "y");
    workThread.Start();

    Console.ReadLine();

    Console.WriteLine("The primary thread is finished.");
}

UseIsBackground();
```
```
Do you want make worker thread backgrounded? (Y/N):Y
Start the method from primary thread with ID : 1
Worker thread is executing PrintNumbers()
Starting slow work.

It's done step 0

It's done step 1

The primary thread is finished.
```
```
Do you want make worker thread backgrounded? (Y/N):n
Start the method from primary thread with ID : 1
Worker thread is executing PrintNumbers()
Starting slow work.

It's done step 0

It's done step 1

It's done step 2

It's done step 3

The primary thread is finished.

It's done step 4

It's done step 5

It's done step 6

It's done step 7

It's done step 8

It's done step 9

```
Треба зазначити для того щоб потік міг бути фоновим треба щоб метод на який вказує тип Thread ( через делегата ParameterizedThreadStart або ThreadStart) повинен мати моживість безпечно зупинитися щойно всі передні потоки закінять свою роботу.
В прикладі видно коли працює основний потік і створиений нами потік фоновий, тоді при переривані основного потоку другий теж закінчує роботу і програма закінчує роботу повністю. У випадку коли робочий поток стає на передньому плані програма закінчить свою роботу тільки коли він закінчить роботу. 
Здебільшого налаштування потоку для роботи у фоновому режимі може бути корисним, коли відповідний робочий потік виконує некритичне завдання, яке більше не потрібно після завершення основного завдання програми. Наприклад, ви можете створити програму, яка кожні кілька хвилин перевіряє сервер електронної пошти на наявність нових електронних листів, оновлює поточні погодні умови або виконує інше некритичне завдання.


# Проблема паралельної роботи потоків.

При створені багатопотокових додадків, треба гарантувати, що будь-яка частина спільних даних буде захищена від хаотичної зміни численними потоками. Враховуючи, що всі потоки в AppDomain мають одночасний доступ до спільних даних, уявіть що могло б статися, якби кілька потоків отримують доступ до однієї ж самої точки даних. Що буде якщо планувальник потоків змусить потоки випадково призупинити свою роботу і наприклад потік А буде змужено призупинитись на шляху до того як він завершить роботу? Поток B тепер очікують нестабільні дані.

Нехай ми маємо наступний клас.

IssueOfConcurrency/Printer.cs
```cs
namespace IssueOfConcurrency;
public class Printer
{
    public void PrintNumbers()
    {
        // Display Thread info.
        Console.WriteLine($"{Thread.CurrentThread.Name} is executing PrintNumbers()");

        //Print out numbers.
        for (int i = 0; i < 10; i++)
        {
            Random random = new();
            Thread.Sleep(200*random.Next(5));
            Console.Write($"{i} ");
        }
        Console.WriteLine();
    }
}
```
```cs
void OneThread()
{
    Thread.CurrentThread.Name = "Primary";
    Printer printer = new();
    printer.PrintNumbers();
}
OneThread();
```
```
Primary is executing PrintNumbers()
0 1 2 3 4 5 6 7 8 9
```
Метод PrintNumbers друкує послідовність чисел змушуючи поточний поток призупинятися на випадково згенерований проміжок часу.

Створимо масив потоків і запустимо їх.

```cs
void WorkManyThreads()
{
    int length = 10;

    Printer printer = new();

    //Make many threads that are all pointing to
    //the same method on the same object
    Thread[] threads = new Thread[length];
    for (int i = 0; i < length; i++)
    {
        threads[i] = new Thread(new ThreadStart(printer.PrintNumbers)) 
        { Name = $"Work thread {i}" };
    }

    foreach (Thread thread in threads)
    {
        thread.Start();
    }
}
WorkManyThreads();
```
```
Work thread 1 is executing PrintNumbers()
Work thread 8 is executing PrintNumbers()
Work thread 2 is executing PrintNumbers()
Work thread 0 is executing PrintNumbers()
Work thread 5 is executing PrintNumbers()
Work thread 6 is executing PrintNumbers()
Work thread 7 is executing PrintNumbers()
Work thread 9 is executing PrintNumbers()
Work thread 3 is executing PrintNumbers()
Work thread 4 is executing PrintNumbers()
0 0 0 1 2 0 0 0 3 0 1 1 1 0 1 2 0 0 1 2 2 2 1 1 4 1 2 2 5 1 3 4 3 3 3 4 3 4 2 6 3 5 4 4 2 5 7 5 6 2 3 4 6 7 8 9 4 3 7 8 5
5 3 5 6 6 8 5 6 7 7 8 6 9
8 9
4 7 7 4 8 5 6 9
6 8 9
7 9
9
5 7 8 9
8 6 7 8 9
9
```
Основний потік у цьому AppDomain створює додадкові робочі потоки. Кожному потоку наказано запустити метод PrintNumbers на тому самому екземпляру класу Print. Не вжито жодних запобіжних заходів, щоб заблокувати спільний ресурси цього об'єкту (Сonsol) є великий шанс, шо поточний потік буде вигнано зі шляху до того, як метод PrintNumbers може надрукувати повний результат. Оскільки нам не відомо коли це станеться може статися, ви обоб'язково отримаєте непередбачувальний результат.
При кожному запуску прикладу отримуєте різний результат.
Оскільки кожен потік повідомляє принтеру надрукувати числові дані, планувальник потоків із задоволенням міняє потоки у фоновому режимі.Результат – непослідовний результат.Потрібен спосіб програмного забезпечення синхронізованого доступу до спільних ресурсів.Простір імен System.Threading надає кілька типів, орієнтованих на синхронізацію. Мова програмування C# також надає ключове слово для самого завдання синхронізації спільних даних у багатопоточних програмах.

## Синхронізація потоків за допомогою lock.

Синхронізацію потоків можна за допомогою ключового слова lock. Воно дозволяє визначити область операторів які повині бути синхронізовані між потоками. Роблячи таким чином, вхідні потоки не можуть переривати поточний потік, таким чином зберігаючи його від недозавершення його роботи. Ключеве слово вимагає вказати маркер(посилання на об'єкт), який має отримати потік, щоб увійти в область блокування. Якщо треба заблокувати приватний метод рівня екземпляра, можно передати посилання на поточний тип.
```cs
private void SomePrivateMethod()
{
  // Use the current object as the thread token.
  lock(this)
  {
    // All code within this scope is thread safe.
  }
}
```   
Однак, якщо ви блокуєто область коду в публічному методі, безпечно(і краща практика) оголосити змінну члена приватного об'єкта, яка буде слугувати маркером блокування.
```cs
public class Printer
{
  // Lock token.
  private object threadLock = new object();
  public void PrintNumbers()
  {
    // Use the lock token.
    lock (threadLock)
    {
      ...
    }
  }
}
```
Якшо розглянути метод PrintNumbers, спільним ресурсом за доступ до якого змагаються потоки є косноль. Спробуємо заблокувати обсяг коду який виконують потоки з консолью.
Додамо в клас Print метод 
```cs
 //Lock token
 private object threadLock = new object();
 public void PrintNumbersWithLock()
 {
     lock (threadLock) 
     { 
         // Display Thread info.
         Console.WriteLine($"{Thread.CurrentThread.Name} is executing PrintNumbers()");

         //Print out numbers.
         for (int i = 0; i < 10; i++)
         {
             Random random = new();
             Thread.Sleep(100 * random.Next(5));
             Console.Write($"{i} ");
         }
     Console.WriteLine();
     }
 }
```
Протестуємо метод
```cs
void UseLock()
{
    int length = 3;

    Printer printer = new();

    Thread[] threads = new Thread[length];
    for (int i = 0; i < length; i++)
    {
        threads[i] = new Thread(new ThreadStart(printer.PrintNumbersWithLock))
        { Name = $"Work thread {i}" };
    }

    foreach (Thread thread in threads)
    {
        thread.Start();
    }
}
UseLock();
```
```
Work thread 0 is executing PrintNumbers()
0 1 2 3 4 5 6 7 8 9
Work thread 1 is executing PrintNumbers()
0 1 2 3 4 5 6 7 8 9
Work thread 2 is executing PrintNumbers()
0 1 2 3 4 5 6 7 8 9
```
Метод з використанням lock дозволяє поточному потоку виконати своє завдання. Коли потік потрапляє в область блокування, токен(маркер) блокування стає недоступним для інших потоків, доки блокування не буде знято при виходу з області блокування. Таким чином, якщо потік A отримав маркер блокування, інші потоки не зможуть увійти в будь-яку область, яка використовує той самий маркер блокування, доки потік A не відмовиться від маркера блокування.
Якщо ви намагаєтеся заблокувати код у статичному методі, просто оголосите змінну-член приватного статичного об’єкта, яка буде служити маркером блокування.

## Сінхронізація за допомогою типа System.Threading.Monitor.

Оператор lock це скоречена нотація використання класу Monitor. аналог попередньому прикладу можно представити так.

```cs
    public void PrintNumbersWithMonitor()
    {
        Monitor.Enter(threadLock);
        try
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} is executing PrintNumbers()");

            //Print out numbers.
            for (int i = 0; i < 10; i++)
            {
                Random random = new();
                Thread.Sleep(100 * random.Next(5));
                Console.Write($"{i} ");
            }
            Console.WriteLine();
        }
        finally 
        { 
            Monitor.Exit(threadLock); 
        } 
    }
```
```cs
void UseMonitor()
{
    int length = 3;

    Printer printer = new();

    Thread[] threads = new Thread[length];
    for (int i = 0; i < length; i++)
    {
        threads[i] = new Thread(new ThreadStart(printer.PrintNumbersWithMonitor))
        { Name = $"Work thread {i}" };
    }

    foreach (Thread thread in threads)
    {
        thread.Start();
    }
}
UseMonitor();
```
```
Work thread 0 is executing PrintNumbers()
0 1 2 3 4 5 6 7 8 9
Work thread 1 is executing PrintNumbers()
0 1 2 3 4 5 6 7 8 9
Work thread 2 is executing PrintNumbers()
0 1 2 3 4 5 6 7 8 9
```
Метод Enter є кінцевим одержувачем тонена потоку. Область яка блокується обгорається оператором try. Блок finally гарантує звільнення марекра потоку.
Викрористання типу Monitor на пряму може дати більше можливостей контролю. Можна наказати активному потоку чекати деякий час (через метод Monitor.Wait), повідомити потоки, що очікують, коли поточний потік буде завершено( через статичний метод Monitor.Pulse та Monitor.PulseAll), та інші.
Як і слід було очікувати, у багатьох випадках ключове слово C# lock підійде. Однак, якщо ви зацікавлені в перевірці додаткових членів класу Monitor, зверніться до документації .NET Core.

## Сінхронізація за допомогою типа System.Threading.Interlocked.

Признасення зміній та прості арифметичні операції не є атомарними. Аби безпечно виконувати такі дії, простір імен System.Threading надає тип, який дозволяє працювати з однією точкою даних атомарно з меньшими накладними витратами, ніж з типом Monitor.
Тип System.Threading.Interlocked предоставляє такі ключові статичні члени.

    CompareExchange() : Безпечно перевіряє два значення на рівність, і якщо вони рівні, обмінює одне із значень на третє.

    Increment() : Безпечно збільшує значення на 1

    Decrement() : Безпечно зменьшує значення на 1

    Exchange() : Безпечно міняє два значення.

Процес атомарної зміни одного значення досить поширений у багатопоточному середовищі.
Припустімо, що у вас є код, який збільшує цілочисельну змінну-член з іменем intVal. Можно писати код з сінхронізацією.
```cs
void AssigningWithLock()
{
    int intValue = 5;
    object lockTocken = new();
    lock(lockTocken)
    {
        intValue++;
    }
    Console.WriteLine(intValue);
}
AssigningWithLock();
```
Аби зменшити наклодні росходи можна використати статичний метод.
```cs
void UseInterlockedIncrement()
{
    int intValue = 5;
    intValue = Interlocked.Increment(ref intValue);
    Console.WriteLine(intValue);
}
UseInterlockedIncrement();
```
Зауважте, що метод Increment() не лише коригує значення вхідного параметра, але й повертає нове значення.

Спробуємо виконати призначення. 
```cs
void UseInterlockedExchange()
{
    int intValue = 5;
    Interlocked.Exchange(ref intValue,10);
    Console.WriteLine(intValue);
}
UseInterlockedExchange();

```
Тип Interlocked за допомогою методу Exchange дозволяє атомарно призначати числові або об'єктні дані, уникаючи використання lock та Monitor.
    
Спробуємо зробити порівняння.
```cs

void UseInterlockedCompareExchange()
{
    int intValue = 5;
    Interlocked.CompareExchange(ref intValue,15,5);
    Console.WriteLine(intValue);

    Interlocked.CompareExchange(ref intValue, 5, 10);
    Console.WriteLine(intValue);
}
UseInterlockedCompareExchange();
```
```
15
15
```
Якшо треба зробити перевірку на еквівалентність і змінити початкову точку порівняння в  потокобезпечний спосіб, можна використати CompareExchange.  

# Використання Timer Callbacks.

Багато програм потребують виклику певного методу протягом регулярного інтервалу часу. Наприклад у вас може бути програма, якій потрібно відображати поточний час в панелі стану за допомогою допоміжної функції. Як інший приклад, ви можете захотіти, щоб ваша програма час від часу викликала допоміжну функцію для виконання некритичних фонових завдань, таких як перевірка нових повідомлень електронної пошти. Для подібних ситуацій ви можете використовувати тип System.Threading.Timer у поєднанні з пов’язаним делегатом під назвою TimerCallback.

Створемо проект ExplorationTimer який буде друкувати час кожну секунду.

Для почастку створемо метод шо друкує час.
```cs
static void PrintTime(object? state)
{

    Console.Write($" {DateTime.Now.ToLongTimeString()} " +
        $"ThreadID : {Thread.CurrentThread.ManagedThreadId} " +
        $"{state?.ToString()}");

    Console.SetCursorPosition(0, 1);
    Console.CursorVisible = false;
}
PrintTime(null);
```
```
 12:47:52 ThreadID : 1
```
Зверніть увагу, що метод PrintTime() має один параметр типу System.Object і повертає void. Така сігнатура методу відповідає делегату TimerCallback. В якості аргументу можна передати будь-який тип виходячи з завдання методу (наприклад адрес почтового сервера, тощо ).Також зауважте, що враховуючи, що цей параметр справді є System.Object, ви можете передати кілька аргументів за допомогою System.Array або спеціального класу/структури.

Далі налаштуємо екземпляр делегата TimerCallback і передамо його в об'єкт Timer.
```cs
void UseTimer()
{
    Console.WriteLine($"ThreadID : {Thread.CurrentThread.ManagedThreadId}");
    // The TimerCallback delegate object.
    TimerCallback timerCallback = new(PrintTime);

    Timer timer = new Timer(
        timerCallback,// The TimerCallback delegate object.
        null,         // Any info to pass into the called method (null for no info).
        0,            // Amount of time to wait before starting (in milliseconds).
        1000);        // Interval of time between calls (in milliseconds).

    Console.ReadLine();
}
```
```
ThreadID : 1
 12:49:13 ThreadID : 6
```
Зверніть увагу шо якшо не визивати Console.ReadLine(), тобто в основному потоці не вказати чеканя вводу сторки, програма закінчить роботу без запуску методу. Як видно метод виконується в окремому потоці. 
В конструктор Timer можна додати інформацію, у вигляді System.Object, який буде передано цільовому методу делегата.

```cs
void UseTimerWithInformation()
{
    Console.WriteLine($"ThreadID : {Thread.CurrentThread.ManagedThreadId}");

    TimerCallback timerCallback = new(PrintTime);

    _ = new Timer(timerCallback,
        "Good moment",  // Any info to pass into the called method (null for no info).
        0,1000);

    Console.ReadLine();
}
UseTimerWithInformation();
```
```
ThreadID : 1
 12:52:09 ThreadID : 7 Good moment
```
Оскільки зміна типу Timer не використовується в жодному шляху виконання її можна видкинути викристовуючи _. 

# Пул потоків.

Створення додадкового потоку потребує затрат, тому з метою підвищення єфективності пул потоків зберігає створені потокі (але не активні), поки вони не знадобляться. Щоб дозволити вам взаємодіяти з цім пулом потоків, що очікують,в просторі імен System.Threading предоставляє клас ThreadPool. 
Якшо ви хочете поставити виклик методу в чергу на виконання у робочому процесі у пулі, можна використати ThreadPool.QueueUserWorkItem метод. Цей метод було перезавантажено, щоб дозволити кірм екземпляра делегата, передати додадкові користувацьки дані.   

```cs
public static class ThreadPool
{
  ...
  public static bool QueueUserWorkItem(WaitCallback callBack);
  public static bool QueueUserWorkItem(WaitCallback callBack,
                                      object state);
}
```
Делегат WaitCallback може вказувати на будь-який метод, що приймає System.Object як єдиний параметр (що є необов'язкові дані стану) і нічого не повертає. Зауважте, що якщо ви не надаєте System.Object під час виклику QueueUserWorkItem(), середовище виконання .NET Core автоматично передає нульове значення.

StudyOfTheThreadPool\Program.sc
```cs
static void UseQueueUserWorkItem()
{
    Thread primary = Thread.CurrentThread;
    primary.Name = "Primary";
    Console.WriteLine($"Main thread started. ThreadId:{primary.ManagedThreadId}");

    Printer printer = new();
    printer.PrintNumbersWithLock();
    Console.WriteLine();

    WaitCallback workItem = new WaitCallback(PrintTheNumbers);

    int length = 10;
    for (int i = 0; i < length; i++)
    {
        ThreadPool.QueueUserWorkItem(workItem,printer);
    }
    Console.WriteLine("All tasks queued");
    Console.ReadLine();

    static void PrintTheNumbers(object? state)
    {
        if (state is Printer task)
        {
            task.PrintNumbersWithLock();
        }
    }
}
UseQueueUserWorkItem();
```
```
Main thread started. ThreadId:1
Primary is executing PrintNumbers()
0 1 2 3 4 5 6 7 8 9

All tasks queued
.NET TP Worker is executing PrintNumbers()
0 1 2 3 4 5 6 7 8 9
.NET TP Worker is executing PrintNumbers()
0 1 2 3 4 5 6 7 8 9
.NET TP Worker is executing PrintNumbers()
0 1 2 3 4 5 6 7 8 9
.NET TP Worker is executing PrintNumbers()
0 1 2 3 4 5 6 7 8 9
.NET TP Worker is executing PrintNumbers()
0 1 2 3 4 5 6 7 8 9
.NET TP Worker is executing PrintNumbers()
0 1 2 3 4 5 6 7 8 9
.NET TP Worker is executing PrintNumbers()
0 1 2 3 4 5 6 7 8 9
.NET TP Worker is executing PrintNumbers()
0 1 2 3 4 5 6 7 8 9
.NET TP Worker is executing PrintNumbers()
0 1 2 3 4 5 6 7 8 9
.NET TP Worker is executing PrintNumbers()
0 1 2 3 4 5 6 7 8 9
```
В цьому випадку не створюється масив об'єктів Thread, а відбуваетья призначеня членам пула потоків callback який буде виконувати запуск цільового методу. 

Використання пулу потоків, замість створеня окремих робочих потоків, має наступні переваги:

    - Пул потоків ефективно керує потоками, мінімізуючи кількість потоків, які потрібно створити , запустити та зупинити.

    - Використовуючи пул потоків, ви можете зосередитися на своїй бізнес-проблемі, а не на потоковій інфраструктурі програми.


Однак у деяких випадках краще використовувати керування потоками вручну:

    - Якшо вам потрибен "foreground" потік або потрібно встановити пріорітет потоку.
    - Якщо вам потрібен потік із фіксованим ідентифікатором, щоб перервати його, призупинити його або знайти його за назвою.
