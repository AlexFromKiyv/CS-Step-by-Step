# Асінхроні виклики з патерном async await.

Патерн async/await, це спроба зробити можливість писати код послідовно. Виконуватись такий код буде не сінхроно з можливістю єкономії часу на виконаня і звільнення основних потоків.
Ключове слово async використовується для визначення того, що метод , лямбда-вираз або анонімний метод повинні викликатися в асінхронному режимі автоматично. Просто позначивши метод модифікатором async, .NET Core Runtime створить новий потік виконання для вирішення завдання методу. Крім того, коли ви викликаєте асинхронний метод, ключове слово await автоматично призупинить поточний потік від будь-якої подальшої діяльності до завершення завдання, залишаючи потік, що викликає, вільним для продовження.

Припустимо у нас є метод який щось довго робить.  

AsyncAwait\SimpleUsingAsyncAwait\Program.cs

```cs
    static string DoLongWork()
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"I star to do long work! Thread:{threadId}");
        Thread.Sleep(3000); // Emulation the long work
        return "Done with work!";
    }
```
Якшо цей метод використовувати послідовно з іншими завданнями весь хід роботи (може і інтерфейс корстувача) буде зупинено і треба буде чекати. 

```cs
void SlowWork()
{
    while (true)
    {
        Console.Clear();

        Console.WriteLine(DoLongWork());
        Console.WriteLine(DoLongWork());
        Console.WriteLine(DoLongWork());

        Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} says: Enter somthing");
        Console.ReadLine();
    }
 
    static string DoLongWork()
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"I star to do long work! Thread:{threadId}");
        Thread.Sleep(3000); // Emulation the long work
        return "Done with work!";
    }
}
SlowWork();
```

```
I star to do long work! Thread:1
Done with work!
I star to do long work! Thread:1
Done with work!
I star to do long work! Thread:1
Done with work!
Thread 1 says: Enter somthing
Hi girl
```
Такми чино сінхронне виконання виконується з затримками. Бажано довгі навантаженя відокремити від основного потоку таким чином вивільнивши основний потік для роботи користувача. 

Можна самому кодувати процес відділеня завданя від основного потоку але це може зробити за вас середовише. Для цього можна використати Task та згадані ключові слова.

```cs
static async Task<string> DoLongWorkAsync()
{
    return await Task.Run(() =>
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
        Thread.Sleep(3000);
        return $"\nDone with work. Thread: {threadId}";
    });
}


async void CallAsyncMethod()
{
    string taskResult = await DoLongWorkAsync();
    Console.WriteLine(taskResult);
}


while (true)
{
    Console.Clear();

    CallAsyncMethod();
    CallAsyncMethod();
    CallAsyncMethod();

    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} says: Enter somthing");
    Console.ReadLine();
}

```
```
Thread 1 says: Enter somthing
I star to do long work asynchronous! Thread: 8
I star to do long work asynchronous! Thread: 7
I star to do long work asynchronous! Thread: 4
Hi girl
Done with work. Thread: 7

Done with work. Thread: 8

Done with work. Thread: 4
Goodbay!
```
Top-level оператори обгортається в метод який за замовчуванням async. 

Зверніть увагу на ключове слово await.
```cs
 return await Task.Run(() =>
 {
     int threadId = Thread.CurrentThread.ManagedThreadId;
     Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
     Thread.Sleep(5000);
     return $"Done with work. Thread: {threadId}";
 });
```

Це важливо: якщо метод визначено async, але не маєте принаймні одного внутрішнього виклику методу, орієнтованого на очікування (await) , ви, по суті, створили синхронний виклик методу (фактично, вам буде надано попередження компілятора про це).
Крім того, async метод може поверати лише void, Task, Task<T>, ValueTask<T> типи. Замість того щоб повертати конкретне значення, як наприклад рядок, ми повертаем об'єкт Task<T> де T тип значення що повертається. Якшо метод не повертає значення просто повертаеться Task. 

В прикладі коли виконується виклик DoLongWorkAsync відбуваеться створення нового потоку в якому запускаеться новое завдання. В цьому потоці очікується виконання завдання. Коли завдання виконається повертаеться рядок як результат завдання. 

Зверніть увагу шо await повертає не Task<T>, а зачення типу T як результат роботи.

Запустивши приклад і декілька разів нажати Enter можна побачити як накоплюються завдання.
В більш розгалужених додадках при визові асінхроних методах не блокуєтся користувацький інтерфейс.    

Існує клас SynchonizationContext — це тип, який надає метод віртуальної публікації, який приймає делегат для асинхронного виконання. Він надає спосіб поставити одиницю роботи в чергу в контекст і веде підрахунок незавершених асинхронних операцій. Як ми обговорювали раніше, коли делегат стоїть у черзі для асинхронного запуску, його заплановано для запуску в окремому потоці. Цю деталь обробляє .NET Core Runtime. Зазвичай це робиться за допомогою керованого пулу потоків .NET Core Runtime, але його можна замінити за допомогою спеціальної реалізації.
Хоча цією роботою можна керувати вручну за допомогою коду, шаблон async/await виконує більшу частину важкої роботи. Коли асинхронний метод очікується, він використовує реалізації SynchronizationContext і TaskScheduler цільової структури. Наприклад, якщо ви використовуєте async/await у програмі WPF, структура WPF керує відправленням делегату та зворотним викликом кінцевого автомата, коли очікуване завдання завершується, щоб безпечно оновити елементи керування.
За замовчуванням очікування виконання завданя приведе до використання контексту синхронізації.

```cs
async void UseConfigureAsync()
{
    Stopwatch stopwatch1 = Stopwatch.StartNew();
    string message1 = await DoLongWorkAsync();
    Console.WriteLine($"\tmessage1 {message1}");
    stopwatch1.Stop();
    Console.WriteLine(stopwatch1.ElapsedMilliseconds);


    Stopwatch stopwatch2 = Stopwatch.StartNew();
    string message2 = await DoLongWorkAsync().ConfigureAwait(false);
    Console.WriteLine($"\tmessage2 {message2}");
    stopwatch2.Stop();
    Console.WriteLine(stopwatch2.ElapsedMilliseconds);
}

UseConfigureAsync();
Console.ReadLine();
```

```
I star to do long work asynchronous! Thread: 6
        message1
Done with work. Thread: 6
3027
I star to do long work asynchronous! Thread: 7
        message2
Done with work. Thread: 7
3006
```
Це корисно при роботі з додадками з GUI. Коли розробляється бібліотека без GUI це непотрібні накладні росходи. Викли string message2 = await DoLongWorkAsync().ConfigureAwait(false); не тратить ресурси на контекс сихронізації. Якщо ви пишете непрограмний код (наприклад, код бібліотеки), використовуйте ConfigureAwait(false).
Єдиним винятком для цього є ASP.NET Core. ASP.NET Core не створює настроюваний SynchronizationContext; тому ConfigureAwait(false) не дає переваги при використанні інших фреймворків.

### Домовленість про іменування асінхроних методів.

Метод який визначен як async можна викликати сінхроно. Тоді він поверне не значення що є результатом завдання. Як правило такі методи викликаяться з await. Ось приклад неправільного коду.
```cs
string result = DoLongWorkAsync();
```
Маркер await витягує внутрішнє повернуте значення, що міститься в об’єкті Task. Враховуючи те, що методи, які повертають об’єкти Task, тепер можна викликати неблокуючим способом через маркери async і await, рекомендовано називати будь-який метод, який повертає Task, суфіксом Async. Таким чином, розробники, які знають угоду про іменування, отримують візуальне нагадування про те, що ключове слово await є обов’язковим, якщо вони мають намір викликати метод в асинхронному контексті.

## Асінхроні методи шо повертають void.

Асінхроний метод може повертати void.

```cs
static async void MethodReturningVoidAsync()
{
    await Task.Run(() => 
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
        Thread.Sleep(3000); // Emulation the long work 
    });
    Console.WriteLine("Fire and forget void method completed");
}

MethodReturningVoidAsync();
Console.WriteLine("The work after calling the method.");
Console.ReadLine();
```
```
The work after calling the method.
I star to do long work asynchronous! Thread: 6
Hi Fire and forget void method completed
```
Якщо ви викликаєте цей метод, він працюватиме самостійно, не блокуючи основний потік. Це видно з повідомленя шо йде за викликом методу та можливость щось вводити.

Хоча це може здатися життєздатним варіантом для сценаріїв “fire and forget” (висрілити і забути), існує більша проблема. Якщо метод викидає виняток, йому нікуди йти, крім контексту синхронізації методу, що викликає.

```cs
static async void MethodReturningVoidWithExceptionAsync()
{
    await Task.Run(() =>
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
        Thread.Sleep(3000); // Emulation the long work 
        throw new Exception("Smomething bad happend!");
    });
    Console.WriteLine("Fire and forget void method completed");
}

try
{
    MethodReturningVoidWithExceptionAsync();
    Console.ReadLine();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
```
```
I star to do long work asynchronous! Thread: 6
Hi Unhandled exception. System.Exception: Smomething bad happend!
...

```
Блок catch не тільки не перехоплює виняток, але виняток розміщується в контексті потокового виконання. Тож, хоча це може здатися гарною ідеєю для сценаріїв “fire and forget”, вам краще сподіватися, що в методі async void не буде створено винятку, інакше вся ваша програма може вийти з ладу.

## Асінхроні методи шо повертають Task.

Кращий варіант коли асінхроний метод повертае Task замість void.

```cs
static async Task MethodReturningVoidTaskAsync()
{
    await Task.Run(() =>
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
        Thread.Sleep(3000); // Emulation the long work 
    });
    Console.WriteLine("Method with Task completed");
}

MethodReturningVoidTaskAsync();
Console.WriteLine("The work after calling the method.");
Console.ReadLine();
```
```
I star to do long work asynchronous! Thread: 6
The work after calling the method.
Hi Method with Task completed
```
Якщо викликати метод без ключового слова await, буде той самий результат як і попередній.

Буде такаж сама проблема з винятком.

```cs
static async Task MethodReturningVoidTaskAndExceptionAsync()
{
    await Task.Run(() =>
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
        Thread.Sleep(3000); // Emulation the long work 
        throw new Exception("Smomething bad happend!");
    });
    Console.WriteLine("Method with Task completed");
}

try
{
    MethodReturningVoidTaskAndExceptionAsync();
    Console.ReadLine();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
```
```
I star to do long work asynchronous! Thread: 6
Hi
```
Коли виникає виняток програма на це ніяк не реагує. Блок catch не перезоплює виняток. Коли виняток викидається з методу Task/Task<T>, виняток фіксується та розміщується в об’єкті Task. 

При використані await, Exeption стає доступним.
```cs
try
{
    await MethodReturningVoidTaskAndExceptionAsync();
    Console.ReadLine();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
```
```
I star to do long work asynchronous! Thread: 6
Smomething bad happend!
```
Таким чином краше уникати визначення метода як async void, а краще використовувати async Task в парі з await.  Як мінімум програма не буде аварійно закінчуватись.

## Асінхроний метод з багатьма await.

В асінхронному методі може бути декілька маркерів await.

```cs
static async Task MultipleAwaits()
{
    await Task.Run(() => 
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
        Thread.Sleep(2000); 
    });
    Console.WriteLine("Done 1");

    await Task.Run(() =>
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
        Thread.Sleep(2000);
    });
    Console.WriteLine("Done 2");

    await Task.Run(() =>
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
        Thread.Sleep(2000);
    });
    Console.WriteLine("Done 3");
}

await MultipleAwaits();
```
```
I star to do long work asynchronous! Thread: 6
Done 1
I star to do long work asynchronous! Thread: 6
Done 2
I star to do long work asynchronous! Thread: 6
Done 3
```
У цьому випадку кожне завданя чекає свого виколнання. 
Частіше є потреба не чекати послідовно виконання кожного завдання а чекати коли вони всі виконаються. Це більш вірогідний сценарій, коли є три речі (перевірити електронну пошту, оновити сервер, завантажити файли), які потрібно виконати пакетно, але їх можна виконати паралельно. 
```cs
static async Task UseTaskWhenAll()
{
    Task[] tasks = [

        Task.Run(() =>
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
            Thread.Sleep(3000);
            Console.WriteLine("Done 1");
        }),
        Task.Run(() =>
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
            Thread.Sleep(6000);
            Console.WriteLine("Done 2");
        }),
        Task.Run(() =>
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
            Thread.Sleep(9000);
            Console.WriteLine("Done 3");
        }),
    ];
    await Task.WhenAll(tasks);
}

await UseTaskWhenAll();
Console.Write("Enter something:"); Console.ReadLine();

```
```
I star to do long work asynchronous! Thread: 6
I star to do long work asynchronous! Thread: 7
I star to do long work asynchronous! Thread: 8
Done 1
Done 2
Done 3
Enter something:Hi
```
Таким чином всі завданя запускаються одночасно і поток призупиняється поки всі не завершаться.

Інша поведінка методу WhenAny.
```cs
static async Task UseTaskWhenAny()
{
    Task[] tasks = [

        Task.Run(() =>
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
            Thread.Sleep(3000);
            Console.WriteLine("Done 1");
        }),
        Task.Run(() =>
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
            Thread.Sleep(6000);
            Console.WriteLine("Done 2");
        }),
        Task.Run(() =>
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
            Thread.Sleep(9000);
            Console.WriteLine("Done 3");
        }),
    ];
    await Task.WhenAny(tasks);
}

await UseTaskWhenAny();
Console.Write("Enter something:"); Console.ReadLine();
```
```
I star to do long work asynchronous! Thread: 6
I star to do long work asynchronous! Thread: 8
I star to do long work asynchronous! Thread: 7
Done 1
Enter something:Hi Done 2
Done 3
```
В цьому випадку всі завданя запускаються одночасно але основний потік звільняється коли будь-яке завдання закінчується. 
Також є перезагружені версії WhenAll ,WhenAny які в якості параметрів приймають наприклад List<Task<string>>.

## Виклик асінхронних методів з сінхронних.

Підчас виконання асінхронного методу створюеться окремий потік. Ключове слово await можна використовувати лише в async методі. Шо робити коли ви не можете або не хочете робити метод async.
Є способи викликати асинхронні методи в синхронному контексті. На жаль, більшість з них погані. Перший варіант полягає в тому, щоб просто відмовитися від ключового слова await, дозволяючи початковому потоку продовжувати виконання, тоді як асинхронний метод виконується в окремому потоці, ніколи не повертаючись до викликаючого. Це веде себе як попередній приклад виклику асинхронних методів які повертають Task. Будь-які значення, які повертає метод, втрачаються, а винятки проковтуються.

```cs
Task<string> task = DoLongWorkAsync();
Console.WriteLine(task.Result);
Console.ReadLine();
```
```
I star to do long work asynchronous! Thread: 6

Done with work. Thread: 6
Hi
```
Це може відповідати вашим потребам, але якщо ні, у вас є три варіанти. Перший — це просто використати властивість Result у Task<T> або метод Wait() у методах Task. Якщо метод не вдається, будь-які винятки загортаються в AggregateException, що потенційно ускладнює обробку помилок. Ви також можете викликати GetAwaiter().GetResult(). Це поводиться так само, як виклики Wait() і Result, з тією невеликою різницею, що винятки не загортаються в AggregateException. Хоча методи GetAwaiter().GetResult() працюють як з методами, що повертають значення, так і з методами без значення, що повертається, вони позначені в документації як «не для зовнішнього використання», що означає, що вони можуть змінитися або зникнути в певний момент в майбутньому.
Хоча ці два варіанти здаються нешкідливими замінами для використання await в асинхронному методі, існує більш серйозна проблема з їх використанням. Виклик Wait(), Result або GetAwaiter().GetResult() блокує викликаючий потік, обробляє асинхронний метод в іншому потоці, а потім повертається назад до викликаючого потоку, зв’язуючи два потоки для виконання роботи. Що ще гірше, кожен із них може спричинити взаємоблокування, особливо якщо потік виклику походить з інтерфейсу користувача програми.

Безпечно виконати асінхроний код дозаоляє пакет Microsoft.VisualStudio.Threading 

```cs
JoinableTaskFactory joinableTaskFactory = new JoinableTaskFactory(new JoinableTaskContext());
string message2 = joinableTaskFactory.Run(DoLongWorkAsync);
Console.WriteLine(message2);
```
```
I star to do long work asynchronous! Thread: 4

Done with work. Thread: 4
```

## Повернення ValueTask<T>

Асінхроний метод може повертати значення.

```cs
static async ValueTask<int> ReturnAnInt()
{
    await Task.Delay(3_000);
    return 5;
}

int c = await ReturnAnInt();
Console.WriteLine(c);
```
```
5
```
Для ValueTask застосовуються ті самі правила, що й для Task, оскільки ValueTask — це лише завдання для типів значень, а не примусове розміщення об’єкта в купі.

## Перевірка параметрів асінхроних методів.

Наступний метод при його визові без await не покаже жодної проблеми в виконані завдання.

```cs
static async Task MethodWithProblem(int t)
{
   await Task.Run(() =>
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
        Thread.Sleep(2000);
        t = 5 / t;
        Console.WriteLine(t);
    });
}
MethodWithProblem(0);
```
Виняток виникне якшо визвати метод з await. 
Як варіант можете (і повинні) додати перевірки на початок методу, але оскільки весь метод є асинхронним, немає гарантії, коли перевірки будуть виконані. Але є і інший варіант.

```cs
static async Task MethodWithVerification(int t)
{
    if(!Verification(t))
    {
        Console.WriteLine("Bad parameter");
        return;
    }
    await Implementation();

    // privat function
    static bool Verification(int p) => (p == 0) ? false : true;

    async Task Implementation()
    {
        await Task.Run(() =>
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
            Thread.Sleep(3000);
            t = 15 / t;
            Console.WriteLine($"Ok {t}");
        });
    }
}

MethodWithVerification(0);
await MethodWithVerification(0);
await MethodWithVerification(5);
```
```
Bad parameter
Bad parameter
I star to do long work asynchronous! Thread: 7
Ok 3
```
Таким чином перевірки виконуються синхронно, а потім приватна функція виконується асинхронно.

## Скасування операцій в патерні async/await .

Скасування не складно реалізувати в шаблоні async/await. 

Створемо проект типу WPF Application з назвою PictureHandlerWithAsyncAwait і назвою рішення PictureHandlerWithAsyncAwait.

Встановити пакет System.Drawing.Common.

    Tools > NuGet Package Manager > Manage NuGet Packages for Solution > В рядку пошуку System.Drawing.Common > Install

Для тестування в католог D:\Pitures скопіюємо декілька будь-яких зображень з розширенням *.jpg.

Змінемо файл MainWindow.xalm

```
<Window x:Class="PictureHandlerWithAsyncAwait.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:PictureHandlerWithAsyncAwait"
        mc:Ignorable="d"
        Title="Picture Handler with Async/Await." Height="450" Width="800">
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>
        <Label Grid.Row="0" Grid.Column="0">
            Feel free to type here while the images are processed...
        </Label>
        <TextBox Grid.Row="1" Grid.Column="0"  Margin="10,10,10,10"/>
        <Grid Grid.Row="2" Grid.Column="0">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="Auto"/>
                <ColumnDefinition Width="*"/>
                <ColumnDefinition Width="Auto"/>
            </Grid.ColumnDefinitions>
            <Button Name="cmdCancel" Grid.Column="0" Margin="65,0,665,10" Click="cmdCancel_Click" Height="20" VerticalAlignment="Bottom" Grid.ColumnSpan="2">
                Cancel
            </Button>
            <Button Name="cmdProcess" Grid.Row="0" Grid.Column="1" Margin="410,10,225,10"
                    Click="cmdProcess_Click">
                Process
            </Button>

        </Grid>
    </Grid>
</Window>

```
Змінимо файл MainWindow.xalm.сs (можливо треба розгорнути стрілку файлу MainWindow.xalm).
В верхній частині оператори using.
```cs
//using System.Windows.Shapes;
using System.IO;
using System.Drawing;
```
Додамо зміну рівня класа типу CancellationTokenSource та обробник події натискання Cancel.

```cs
        private CancellationTokenSource? _cancellationTokenSource = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource?.Cancel();
        }

```

Додамо метод який обробляє одне зобрадення. 

```cs
        private async Task ProcessFileAsync(string currentFile,string outputDirectory, CancellationToken token)
        {
            string filename = Path.GetFileName(currentFile);
            using Bitmap bitmap = new(currentFile);
            try
            {
                await Task.Run(() =>
                {
                    Dispatcher?.Invoke(() => { Title = $"Processing {filename}"; });
                    bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    bitmap.Save(Path.Combine(outputDirectory, filename));
                }, token);
            }
            catch (OperationCanceledException ex)
            {
                Dispatcher?.Invoke(() => { Title = $"Process canceled! {ex.Message}"; });
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
```
В цьому методі в метод Run предаеться CancellationToken якій відповідає за сказування завдання. Коли у об'єкта CancellationTokenSource викликаеться метод Cancel при виконані метода Run виникає TaskCanceledException яке є нашадком  OperationCanceledException.

Додамо обробник натискання кнопки Process який виконує обробку всіх зображень.

```cs
        private async void cmdProcess_Click(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource = new();
            string pictureDirectory = @"D:\Pictures";
            string outputDirectory = @"D:\ModifiedPictures";

            //Recreate directory 
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
            }
            Directory.CreateDirectory(outputDirectory);

            //Process
            string[] files = Directory.GetFiles(pictureDirectory, "*.jpg", SearchOption.AllDirectories);

            try
            {
                foreach (var file in files)
                {
                    await ProcessFileAsync(file, outputDirectory, _cancellationTokenSource.Token);
                }
                Title = "Process complite";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            _cancellationTokenSource = null;
        }
```
При виникнені винятку цикл переривається. 
Подія настисканя на виконання обробки не впливає яна користувацький інтерфейс бо все відбувається в різних потоках.


## Асінхроний метод ForEachAsync.

В класі Paralell є метод ForEachAsync який є асінхроним і дозволяє використати асінхроний метод для тіла циклу.

Додамо в проект кнопку та обробник натискання.
```cs
        private async void cmdProcessWithForEachAsync_Click(object sender, RoutedEventArgs e)
        {
            await ProcessWithForEachAsync(); 
        }

        private async Task ProcessWithForEachAsync()
        {
            _cancellationTokenSource = new();

            string pictureDirectory = @"D:\Pictures";
            string outputDirectory = @"D:\ModifiedPictures";

            // Use ParallelOptions instance to store the CancellationToken.
            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.CancellationToken = _cancellationTokenSource.Token;
            parallelOptions.MaxDegreeOfParallelism = Environment.ProcessorCount;

            //Recreate directory 
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
            }
            Directory.CreateDirectory(outputDirectory);

            //Process
            string[] files = Directory.GetFiles(pictureDirectory, "*.jpg", SearchOption.AllDirectories);

            try
            {
                await Parallel.ForEachAsync(files, parallelOptions, async (currentFile, token) =>
                {
                    token.ThrowIfCancellationRequested();
                    string filename = Path.GetFileName(currentFile);
                    
                    //For title
                    int threadId = Environment.CurrentManagedThreadId;
                    Dispatcher?.Invoke(() =>
                    {
                        Title = $"Processing. Thread:{threadId}   File:{filename}";
                    });

                    using Bitmap bitmap = new Bitmap(currentFile);

                    bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    bitmap.Save(Path.Combine(outputDirectory, filename));
                });
                Dispatcher?.Invoke(() => Title = "Process complite.");
            }
            catch (OperationCanceledException ex)
            {
                Dispatcher?.Invoke(() => { Title = $"Process canceled! {ex.Message}"; });
            }
        }
```
В об'єкті ParallelOptions зберігаеться послання на властивість CancellationTokenSource.Token якій відповідає для скасування виконнання циклу.
Тіло цилу використовує асінхроний метод який вказано у вигляді лямбда виразу. Можливо в цьому випадку краще було б зробити окрмий метод замість лямбда-виразу. Як видно завдання повністью відпрацьовується швидше з використання Parallel.ForEachAsync  ніж коли обробляється коже окреме зображення.


## Скасування в патерні async/await за допомогою методу WaitAsync().

Повернемся до проекту AsyncAwait\SimpleUsingAsyncAwait.

Скасувати завдання можна за допомогою методу WaitAsync().
```cs
async Task UsingWaitAsync()
{
    CancellationTokenSource cancellationTokenSource = new();

    try
    {
        string message = await DoLongWorkAsync().WaitAsync(TimeSpan.FromSeconds(12));
        await Console.Out.WriteLineAsync(message);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    try
    {
        string message = await DoLongWorkAsync().WaitAsync(TimeSpan.FromSeconds(2));
        await Console.Out.WriteLineAsync(message);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    try
    {
        string message = await DoLongWorkAsync().WaitAsync(cancellationTokenSource.Token);
        await Console.Out.WriteLineAsync(message);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }


    cancellationTokenSource.Cancel();

    try
    {
        _ = await DoLongWorkAsync().WaitAsync(cancellationTokenSource.Token);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    try
    {
        _ = await DoLongWorkAsync().WaitAsync(TimeSpan.FromSeconds(2),cancellationTokenSource.Token);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

}
await UsingWaitAsync();
```
```
I star to do long work asynchronous! Thread: 6

Done with work. Thread: 6
I star to do long work asynchronous! Thread: 7
The operation has timed out.
I star to do long work asynchronous! Thread: 11

Done with work. Thread: 11
I star to do long work asynchronous! Thread: 7
A task was canceled.
I star to do long work asynchronous! Thread: 6
A task was canceled.

```
Таким чином можна скасувати завдання або задопомогою токена скасуваня або вказавши проміжок часу після якого завдання закінчуеться.
В сінхронному контексті можна використати аналогічний метод Wait.

## Використання async/await для завантажень.

Повернемось до проекту StudyTPL\MyEBookReader
Процес завантаженя можна змінити.

```cs
async Task DownloadBookWihtAsyncAwaitAndGetStatisticAsync()
{
    string _theEbook = string.Empty;

    await GetBookAsync();
    Console.ReadLine();

    // Methods
    async Task GetBookAsync()
    {
        HttpClient httpClient = new();
        _theEbook = await httpClient.GetStringAsync("http://www.gutenberg.org/files/98/98-0.txt");
        await Console.Out.WriteLineAsync("Download complite");
        GetStats();        
    }

    void GetStats()
    {
        string[] words = _theEbook.Split(new char[] { ' ', '\u000A', ',', '.', ';', ':', '-', '?', '/' }, StringSplitOptions.RemoveEmptyEntries);

        string[] tenMostCommon = [];
        string longestWord = string.Empty;

        Parallel.Invoke(
            () => { tenMostCommon = FindTenMostCommon(words); },
            () => { longestWord = FindLongestWord(words); }
            );

        StringBuilder stringBuilder = new StringBuilder("Ten most common words are:\n");

        foreach (var word in tenMostCommon)
        {
            stringBuilder.AppendLine(word);
        }

        stringBuilder.AppendLine($"Longest word is: {longestWord}");

        Console.WriteLine(stringBuilder.ToString(), "Book info");
    }

    string[] FindTenMostCommon(string[] words)
    {
        var frequencyOrder = from word in words
                             where word.Length > 6
                             group word by word into g
                             orderby g.Count() descending
                             select g.Key;
        string[] result = frequencyOrder.Take(20).ToArray();
        return result;
    }

    string FindLongestWord(string[] words)
    {
        var query = from word in words
                    orderby word.Length descending
                    select word;
        return query.FirstOrDefault()!;
    }
}

await DownloadBookWihtAsyncAwaitAndGetStatisticAsync();
```
```
Download complite
Ten most common words are:
Defarge
himself
Manette
through
nothing
business
another
looking
prisoner
Cruncher
Stryver
CHAPTER
without
Monsieur
Monseigneur
Tellson's
Charles
returned
husband
Gutenberg
Longest word is: undistinguishable
```
В цьому прикладі оновлено метод GetBookAsync

```cs
    async Task GetBookAsync()
    {
        HttpClient httpClient = new();
        _theEbook = await httpClient.GetStringAsync("http://www.gutenberg.org/files/98/98-0.txt");
        await Console.Out.WriteLineAsync("Download complite");
        GetStats();        
    }
```
Тут використовується асінхроний метод GetStringAsync класу httpClient. Цей метод робить виклик HTTP Get для отриманя тексту.

Ключові моменти шаблону async/await.

    Методи, лямбда-вирази та анатонімні методи можуть бути асінхроними з допомогою ключового слова async. Ці методи воконують роботу неблокуючим способом.

    Асінхроний метод може мати декілька await контекстів.

    Коли при виконані зустрічається вираз await , потік шо викликає, призупняється до завершеня очікуваня завдання. Тим часом керування повертаеться до викликаючого метода.

    Ключове слово await приховує об'єкт Task, і виглядає оператором повернення значення яке є результатом роботи завдання. Методи, які не повертають значення, просто повертають void.

    Перевірку параметрів та іншу обробку помилок слід виконувати в основному розділі методу, а фактичну асинхронну частину перемістити до приватної функції.

    Для змінних стека ValueTask є більш ефективним, ніж об’єкт Task, що може спричинити упаковку та розпакування.

    Відповідно до умов іменування, методи, які мають викликатися асинхронно, мають бути позначені суфіксом Async.







