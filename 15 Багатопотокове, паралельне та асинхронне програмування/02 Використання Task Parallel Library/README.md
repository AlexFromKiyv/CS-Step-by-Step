# Task Parallel Library

Крім використання простору імен System.Threading для стоврення багатопотокових програм існує інший підхід, використовуючи бібліотеку паралельного програмування під назвою Task Parallel Library (TPL). Використовуючи типи  з System.Threading.Tasks, можна створювати детальний, маштабований паралельний код без необхідності працювати безпосередньо з потоками.
Однак це не означає, що ви не використовуватимете типи System.Threading, коли використовуєте TPL. Обидва набори інструментів розробки можуть працювати разом цілком природно. Це особливо вірно в тому, що простір імен System.Threading все ще надає більшість основних можливостей синхронізації, які ви досліджували раніше (Monitor, Interlocked тощо).Однак ви, швидше за все, виявите, що віддасте перевагу роботі з TPL, а не з оригінальним простором імен System.Threading, враховуючи, що той самий набір завдань можна виконувати більш простим способом.

## Простір імен System.Threading.Tasks

У сукупності типи System.Threading.Tasks називають бібліотекою паралельних завдань. TPL автоматично динамічно розподілятиме робоче навантаження вашої програми між доступними ЦП, використовуючи пул потоків виконання. TPL обробляє розподіл роботи, планування потоків, керування станом та інші деталі низького рівня. Результатом є те, що ви можете максимізувати продуктивність своїх програм .NET Core, захищаючись від багатьох складнощів безпосередньої роботи з потоками.

## Класс Task.

В основі TPL лежить концепція завдань, які представняють собою окреме довге навантаження. Для завдань існує клас Task з простору імен System.Threading.Tasks. Цей клас використовується для асінхроного запуску окремого навантаження в одному з потоків із 
пулу. Також завдання можна виконати і синхроно.

### Створення і запуск завдання.

Завдання можно створити і запустити декількома способами:

UsingTask\Programm.cs
```cs
void RunTask()
{
    Task task1 = new Task(() => { Console.WriteLine($"Task 1 on {Thread.CurrentThread.ManagedThreadId}");});
    task1.Start();

    Task task2 = Task.Factory.StartNew(() => { Console.WriteLine($"Task 2 on {Thread.CurrentThread.ManagedThreadId}"); ; ;});

    Task task3 = Task.Run(() => { Console.WriteLine($"Task 3 on {Thread.CurrentThread.ManagedThreadId} "); });

    task1.Wait();
    task2.Wait();
    task3.Wait();

}
RunTask();

``` 
```
Task 1 on 6
Task 3 on 8
Task 2 on 7
```    
Якшо не використовувати метод Wait завдання основного потоку закінчитися бистріше аніж виконається завдання окремих потоків. 

Цей метод блокує поток в якому викликається завданя поки вона не виконається 
```cs

void UsingWait()
{
    Console.WriteLine("Main thread.");
    
    Task task = new(()=>
    {
        Console.WriteLine("Start Task.");
        Thread.Sleep(1000);
        Console.WriteLine("End Task.");

    });
    task.Start();
    task.Wait();

    Console.WriteLine("End main thread."  );
}
UsingWait();
```
```
Main thread.
Start Task.
End Task.
End main thread.
```
Класс Task має певні властивості.
```cs
void PropertyOfTask()
{
    Console.WriteLine("Main thread.");

    Task task = new(() =>
    {
        Console.WriteLine($"Id: {Task.CurrentId}");
        Thread.Sleep(1000);
    });
    task.Start();

    Console.WriteLine($"Id: {task.Id}");
    Console.WriteLine($"IsCompleted: {task.IsCompleted}");
    Console.WriteLine($"IsFaulted: {task.IsFaulted}");
    Console.WriteLine($"IsCanceled: {task.IsCanceled}");
    Console.WriteLine($"Status: {task.Status}");

    task.Wait();

    Console.WriteLine("End main thread.");
}
PropertyOfTask();

```
```
Main thread.
Id: 1
IsCompleted: False
Id: 1
IsFaulted: False
IsCanceled: False
Status: Running
End main thread.

```

Одне завдання може мати внутрішне завдання.
```cs
void InnerTask()
{
    Console.WriteLine("Start main");
    Task outer = Task.Factory.StartNew(() =>
    {
        Console.WriteLine("Outer task starting.");

        Task inner = Task.Factory.StartNew(() =>
        {
            Console.WriteLine("Inner task starting.");
            Thread.Sleep(1000);
            Console.WriteLine("Inner task finished.");
        });
        //inner.Wait();
        Console.WriteLine("Outer task finished.");
    });
    outer.Wait();
    Console.WriteLine("End main");
}
InnerTask();
```
```
Start main
Outer task starting.
Outer task finished.
End main
```
Задачі виконуються незалежно одна від одної. Як видно з прикладу внутрішне зовнішне завдання може закінчитись не дочекавшись виконання внутрішнього.

Можна вказати внутрішньому завданню що вона є частиною зовнішнього.
```cs
void InnerTaskAsPartOuter()
{
    Console.WriteLine("Start main");
    Task outer = Task.Factory.StartNew(() =>
    {
        Console.WriteLine("Outer task starting.");

        Task inner = Task.Factory.StartNew(() =>
        {
            Console.WriteLine("Inner task starting.");
            Thread.Sleep(1000);
            Console.WriteLine("Inner task finished.");
        },TaskCreationOptions.AttachedToParent);

        Console.WriteLine("Outer task finished.");
    });
    outer.Wait();
    Console.WriteLine("End main");
}
InnerTaskAsPartOuter();
```
```
Start main
Outer task starting.
Outer task finished.
Inner task starting.
Inner task finished.
End main
```
При створені завдання вказується додадковий параметр TaskCreationOptions.AttachedToParent.

Якшо маємо масив завдань можемо їх запустити і вказати основному потоку шо треба дочекатись аби всі завдання закінчились.
```cs
void ArrayOfTasks()
{
    Task[] tasks =
    [
        new Task( () => 
        {
            Console.WriteLine("Task 1 started.");
            Thread.Sleep(5000); 
            Console.WriteLine("Task 1 finished.");}),

        new Task(() =>
        {
            Console.WriteLine("Task 2 started.");
            Thread.Sleep(5000);
            Console.WriteLine("Task 2 finished.");
        }),
        new Task(() =>
        {
            Console.WriteLine("Task 3 started.");
            Thread.Sleep(5000);
            Console.WriteLine("Task 3 finished.");
        }),

    ];

    for (int i = 0; i < 3; i++)
    {
        tasks[i].Start();
    }

    Task.WaitAll(tasks);
}
ArrayOfTasks();
```
```
Task 3 started.
Task 1 started.
Task 2 started.
Task 2 finished.
Task 3 finished.
Task 1 finished.
```
В синхронному виклику навантаженя потребувало б в три рази більше часу.

### Повернення результату виконаного завдання.

Об'єкт Task може повертати результат виконання завдання.

```cs
void ObtainingTheResultOfTheTask()
{
    Console.Write($"Thread:{Thread.CurrentThread.ManagedThreadId} Enter number :");
    
    int.TryParse(Console.ReadLine(),out int x);

    Task<int> squareTask = new(() =>
    {
        Console.WriteLine($"Thread:{Thread.CurrentThread.ManagedThreadId}");
        Thread.Sleep(5000);
        return x*x;
    });

    squareTask.Start();

    int result = squareTask.Result;

    Console.WriteLine($"Result:{result}");
}
ObtainingTheResultOfTheTask();
```
```
Thread:1 Enter number :10
Thread:6
Result:100
```
Для того щоб завдання повернула результат його треба типізувати типом який хочемо отримати. При звернені до власитвості task.Result поточний потік призупиняється і чекає виконання завданя в якому буде визначено результат. 

### Завдання на продовження.

Завдання може бути виконанне після виконання іншого. Воно може отримати від іншого завдання результату.

```cs
void ContinuationTask()
{
    Task firstTask = new Task(() => Console.WriteLine($"Task Id:{Task.CurrentId}") );

    Task nextTask = firstTask.ContinueWith(AboutTask);

    firstTask.Start();

    nextTask.Wait();

    void AboutTask(Task task)
    {
        Console.WriteLine($"Current Task Id:{Task.CurrentId}");
        Console.WriteLine($"Previous Task Id:{task.Id}");
    }
}
ContinuationTask();
```
```
Task Id:1
Current Task Id:2
Previous Task Id:1
```
Це нагадує callback. Після виконання першого завдання викликається інше завдання. В метод наступного передається викоанане завдання.

Завданна на продовження може оримати результат від попередьнього завдання.

```cs
void ContinuationTaskWithValue()
{
    int t = 5000;

    Task<int> squareTask = new(() =>
    {
        Console.WriteLine($"Task {Task.CurrentId} says: I'm starting"  );
        Thread.Sleep(t);
        return t;
    });

    Task nextTask = squareTask.ContinueWith(task => PrintResult(task.Result));

    squareTask.Start();

    Console.ReadLine();

    void PrintResult(int result)
    {
        Console.WriteLine($"Task {Task.CurrentId} says: I waited {result} ");
    }
 }
ContinuationTaskWithValue();

```
```
Task 1 says: I'm starting
Task 2 says: I waited 5000
```
Маючи таку можливість можна побудувати ланцюг завдань.
```cs
void ChainOfTask()
{
    Task firstTask = new(() => AboutTask(null));

    Task secondTask = firstTask.ContinueWith(AboutTask);

    Task thirdTask = secondTask.ContinueWith(AboutTask);

    firstTask.Start();

    Console.ReadLine();

    void AboutTask(Task? task) =>
        Console.WriteLine($"Current Task:{Task.CurrentId}  Previous Task:{task?.Id}");
}
ChainOfTask();
```
```
Current Task:1  Previous Task:
Current Task:2  Previous Task:1
Current Task:3  Previous Task:2
```

## Класу Parallel.

Важливим класом TPL є System.Threading.Tasks.Parallel.

Найпростіше запустити завдання паралельно можна за допомогою методу Invoke.
```cs
void SimpleUsingInvoke()
{
    Parallel.Invoke(
        () => {
            Console.WriteLine($"Task:{Task.CurrentId}");
            Thread.Sleep(3000);
            Console.WriteLine("Hi");

        },        
        ()=>SayWithDelay("girl",5000),
        
        SayGoodbay       
        );

    void SayGoodbay()
    {
        SayWithDelay("Goodbay", 8000);
    }
    
    void SayWithDelay(string phrase, int delay) 
    {
        AboutTask();
        Thread.Sleep(delay);
        Console.WriteLine(phrase);
    }

    void AboutTask()
    {
        Console.WriteLine($"Task:{Task.CurrentId}");
    }
    
}
SimpleUsingInvoke();
```
```
Task:3
Task:2
Task:1
Hi
girl
Goodbay
```
В якості параметру метод Invoce отримує params Action[]. Таким чином ми можемо пердати методи яки середовище буде виконувати паралельно.

## Скасування паралельних завдань.

Іноді виникає потреба скасувати виконання дуже довгого завдання. В просторі імен System.Threading визначена стуктура CancellationToken яка поширює сповіщення про те, що операції мають бути скасовані. 
```cs
void CancellationWitoutThrow()
{
    using CancellationTokenSource cancellationTokenSource = new();
    CancellationToken cancellationToken = cancellationTokenSource.Token;
 
    Task task = new(() =>
    {
        for (int i = 0; i < 10; i++)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Task canceled");
                return;
            }

            Console.WriteLine(i);
            Thread.Sleep(500);
        }

    },cancellationToken);
    
    task.Start();

    Thread.Sleep(3000);

    cancellationTokenSource.Cancel();

    Thread.Sleep(500);

    Console.WriteLine($"Task status:{task.Status}");
}
CancellationWitoutThrow();
```
```
0
1
2
3
4
5
Task canceled
Task status:RanToCompletion

```
Для обробки скасування спочатку створюється об'єкт CancellationTokenSource який керує та відсилає повідомлення про скасування. В конструктор завдання надсилаеться властивість цього об'єкта CancellationTokenSource.Token. Коли поза завданням викликаеться метод CancellationTokenSource.Cancel() відсилаеться повідомленя про скасування і властивість в стуктурі cancellationToken.IsCancellationRequested примає значення true. В процесі проходження циклу можна перевірити чи не будо відмінено завдання. 

Виконати скасування циклу можно викинувши Exception.
```cs
void CancellationWithThrow()
{
    using CancellationTokenSource cancellationTokenSource = new();
    CancellationToken cancellationToken = cancellationTokenSource.Token;

    Task task = new(() =>
    {
        for (int i = 0; i < 10; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Console.WriteLine(i);
            Thread.Sleep(500);
        }

    }, cancellationToken);

    try
    {
        task.Start();

        Thread.Sleep(3000);

        cancellationTokenSource.Cancel();

        task.Wait();

    }
    catch (AggregateException ae)
    {
        foreach (Exception ex in ae.InnerExceptions)
        {
            if (ex is TaskCanceledException)
                Console.WriteLine("Task canceled");
            else
                Console.WriteLine(ex.Message);
        }
    }


    Console.WriteLine($"Task status:{task.Status}");
   
}
CancellationWithThrow();
```
```
0
1
2
3
4
5
Task canceled
Task status:Canceled
```
В цьому варіанті замість return викидається винаток cancellationToken.ThrowIfCancellationRequested().


## Parallel.For() та Parallel.ForEach()

Клас Paralell містить методи які дозволяють перебирати колекцію даних (зокрема, об'єкт що реалізовує IEnumerable<T>) в паралельний спосіб. Це робиться за допомогою двох основних статичних методів, Parallel.For() та Parallel.ForEach(), кожен з яких визначає численні презавантажені версії. 
Ці методи дозволяють створювати тіло набору операторів коду,які оброблятимуться в паралельним способом. За концепцією, ці оператори мають таку саму логіку яку б ви написали в звичайній конструкції переблору(for , foreach). Корсить в тому що клас Parallel витягуватиме потокі з пулу потоків(і куруватимеме паралелізмом) від вашого імені.
Обидва методи вимагають вказати IEnumerable або  IEnumerable<T> сумісний контейнер, якій містить дані, які потрібно обробити в параллельній манері. Це може бути простий масив, неузагальнена колекція(наприклад ArrayList), узагальнена коллекція(наприклад List<T>) або результат запиту LINQ.
Крім того, вам потрібно буде використовувати делегати Func<T> або Action<T> щоб указати цільовий метод, який буде викликано для обробки даних. 
Згадайте, що Func<T> представляє метод, який може мати задане повертане значення та різну кількість аргументів. Делегат Action<T> схожий на Func<T>, оскільки він дозволяє вказувати на метод, який приймає певну кількість параметрів. Однак Action<T> визначає метод, який може повертати лише void. Хоча ви можете викликати методи Parallel.For() і Parallel.ForEach() і передати строго типізований об’єкт делегату Func<T> або Action<T>, ви можете спростити своє програмування, використовуючи відповідний анонімний метод C# або лямбда-вираз.

## Паралелізм даних із класом Paralell

Корсиний спосіб використати TPL - виконати паралелізм даних. Простіше кажучи, цей термін відноситься до завдання прохождення елементв масиву або колекції паралельним способом за допомогою методу Parallel.For() або Parallel.ForEach(). Припустімо, що потрібно виконати деякі трудомісткі операції введення-виведення файлів. Зокрема, вам потрібно завантажити велику кількість файлів *.jpg у пам’ять, перевернути їх догори дном і зберегти змінені дані зображення в новому місці.
У цьому прикладі ви побачите, як виконати те саме загальне завдання за допомогою графічного інтерфейсу користувача, тож ви зможете вивчити використання «анонімних делегатів», щоб дозволити вторинним потокам оновлювати основний потік інтерфейсу користувача (він же потік інтерфейсу користувача). 
Коли ви створюєте програму з багатопоточним графічним інтерфейсом користувача (GUI), вторинні потоки ніколи не зможуть отримати прямий доступ до елементів керування інтерфейсу користувача. Причина в тому, що елементи керування (кнопки, текстові поля, мітки, індикатори виконання тощо) мають спорідненість потоку з потоком, який їх створив.
У наступному прикладі проілюстровано один із способів надання вторинним потокам доступу до елементів інтерфейсу користувача потокобезпечним способом (хоча простіше зробити за допомогою async і await).

Створемо проект типу WPF Application з назвою DataParallelismWithForEach і назвою рішення DataParallelismWithForEach.

Для тестування в католог D:\Pitures скопіюємо декілька будь-яких зображень з розширенням *.jpg.

Змінемо файл MainWindow.xalm

```
<Window x:Class="DataParallelismWithForEach.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:DataParallelismWithForEach"
        mc:Ignorable="d"
        Title="Using Paralell Class" Height="400" Width="800">
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
            <Button Name="cmdCancel" Grid.Column="0" Margin="65,0,665,0" Click="cmdCancel_Click" Height="20" VerticalAlignment="Center" Grid.ColumnSpan="2">
                Cancel
            </Button>
            <Button Name="cmdProcessWitoutParallel" Grid.Row="0" Grid.Column="1" Margin="410,10,225,10"
                    Click="cmdProcessWitoutParallel_Click">
                Process without Parallel
            </Button>
            <Button Name="cmdProcessWithParallel" Grid.Row="0" Grid.Column="1" Margin="596,10,67,10"
                    Click="cmdProcessWithParallel_Click">
                Process with Paralell
            </Button>
        </Grid>
    </Grid>
</Window>

```
В даному випадку графічний інтерфейс складається з багаторядкового текстового поля(TextBox) та кнопок. Призначеня текстової області - дозволити користувачу вводити текст,поки в фоновому режимі обродляються фотографії. Таким чином ілюструється неблокуючий характер паралельного завдання.

Для цього прикладу потрібен додатковий пакет NuGet (System.Drawing.Common).Для цього прикладу потрібен додатковий пакет NuGet (System.Drawing.Common). Він встановлюється командою:

dotnet add DataParallelismWithForEach package System.Drawing.Common
Або 

Tools > NuGet Package Manager > Manage NuGet Packages for Solution > В рядку пошуку System.Drawing.Common > Install

Змінимо файл MainWindow.xalm.сs (можливо треба розгорнути стрілку файлу MainWindow.xalm). 
В верхній частині треба додати оператори using.
```cs
using System.Drawing;
using System.IO;
```
Видаліть using System.Windows.Shapes;

На кнопці cmdProcessWitoutParallel зробимо подвійний клік і забінимо шаблон обробки події.

```cs
        private void cmdProcessWithoutParalell_Click(object sender, RoutedEventArgs e)
        {
            Title = $"Starting...";
            var watch = Stopwatch.StartNew();
            ProcessFilesWithoutParalell(@"D:\Pictures", @$"D:\ModifiedPictures");
            watch.Stop();
            Title = $"Processing complite. Time: {watch.ElapsedMilliseconds}";
        }

        private void ProcessFilesWithoutParalell(string pictureDirectory, string outputDirectory)
        {
            //Recreate directory 
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
            }
            Directory.CreateDirectory(outputDirectory);

            //Process
            string[] files = Directory.GetFiles(pictureDirectory,"*.jpg",SearchOption.AllDirectories);

            foreach (var currentFile in files)
            {
                string filename = Path.GetFileName(currentFile);
                Title = $"Processing {filename} on thread: {Environment.CurrentManagedThreadId}";

                using Bitmap bitmap = new(currentFile);
                bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                bitmap.Save(Path.Combine(outputDirectory, filename));
            }
        }
```
Зверніть увагу, що метод ProcessFilesWithoutParalell() обертатиме кожен файл *.jpg у вказаному каталозі. Наразі вся робота відбувається над основним потоком виконуваного файлу. При натискані кнопки починається процес обробки і видно що все відбувається в основному потоці програми з ID 1. При виконані процесу ми не можемо змінити дані TextBox покі всі зображення не будуть оброблені. Таким чином робота програми "виснить". 

Щоб обробити файли на якомога більшій кількості потоків, ви можете переписати поточний цикл foreach на Parallel.ForEach(). 
Пам'ятайте, що цей метод багато разів перевантажувався; однак у найпростішій формі ви повинні вказати IEnumerable<T>-сумісний об’єкт, який містить елементи для обробки (це буде масив рядків файлів) і делегат Action<T>, який вказує на метод, який виконуватиме роботу.

```cs
        private void cmdProcessWithForEach_Click(object sender, RoutedEventArgs e)
        {
            Title = $"Starting...";
            var watch = Stopwatch.StartNew();
            ProcessFilesWithForEach(@"D:\Pictures", @$"D:\ModifiedPictures");
            watch.Stop();
            Title = $"Processing complite. Time: {watch.ElapsedMilliseconds}";
        }

        private void ProcessFilesWithForEach(string pictureDirectory, string outputDirectory)
        {
            //Recreate directory 
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
            }
            Directory.CreateDirectory(outputDirectory);

            //Process
            string[] files = Directory.GetFiles(pictureDirectory, "*.jpg", SearchOption.AllDirectories);

            Parallel.ForEach(files, currentFile =>
            {
                string filename = Path.GetFileName(currentFile);
                //This code statement is now a problem 
                //Title = $"Processing {filename} on thread: {Environment.CurrentManagedThreadId}";
                Debug.WriteLine($"Processing {filename} on thread: {Environment.CurrentManagedThreadId}");

                using Bitmap bitmap = new(currentFile);
                bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                bitmap.Save(Path.Combine(outputDirectory, filename));
            });
        }
```
В якості аргументів методу ForEach виступає масив рядків який є IEnumerable, та лямда вираз який теж саме що і об'єкт делегату Action<>. Якшо програму запустити а режимі налагодженя то в віконці Debuger можна побачити, що оброба зображень ведеться в різних потоках. Також можна побачити шо час обробкі меньший попередьного варіанта це виводиться в заголовку програми.(можна зрівняти кількість операторів поставиши коментар на одній строчці). Але поки під час обробки зображень не можно одночасно міняти текстове поле.

### Доступ до елементів інтерфейсу користувача у вторинних потоках.

Як ви бачете в варіанті коли використовується ForEach закоментовано сторка якв зміню заголовок головного вікна. 
```cs
    //This code statement is now a problem 
    //Title = $"Processing {filename} on thread: {Environment.CurrentManagedThreadId}";
```
Eлементи керування GUI мають «спорідненість потоку» з потоком, який їх створив. Якщо вторинні потоки намагаються отримати доступ до елемента керування, який вони безпосередньо не створювали, це спричинить виняток під час виконання. В нашому випадку якшо не закоментувати строку програма може закінчити свою роботу "аварійно". 

Клас Task дозволяє легко викликати метод у вторинному потоці та може використовуватися як проста альтернатива використанню асинхронних делегатів.

Додамо кнопку і обробник

```
            <Button x:Name="cmdProcessWithTaskFactory" Grid.Column="1" Content="Process with TaskFactory" HorizontalAlignment="Left" Margin="564,0,0,0" VerticalAlignment="Center" Width="144" 
                    Click="ProcessWithTaskFactory_Click"/>
```
```cs

        private void ProcessWithTaskFactory_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(ProcessFilesWithForEachAndTask);
            //Or
            //Task.Factory.StartNew(() => ProcessFilesWithForEachAndTask());
        }

```
```cs
        private void ProcessFilesWithForEachAndTask()
        {
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

            Parallel.ForEach(files, currentFile =>
            {
                string filename = Path.GetFileName(currentFile);
                
                //For title
                int threadId = Environment.CurrentManagedThreadId;
                Dispatcher?.Invoke(() =>
                {
                    Title = $"Processing. Thread:{threadId}   File:{filename}";
                });

                using Bitmap bitmap = new(currentFile);
                bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                bitmap.Save(Path.Combine(outputDirectory, filename));
            });
            //For title
            Dispatcher?.Invoke(() => { Title = "Process complete"; });
        }
```

Клас Task репрезентує асінхроні операції. Його властивість Factory повертає об'єкт TaskFactory, який дозволяє створювати і кофігурувати об'єкти Task або Task<TResult>.
Коли визивається метод StartNew пердається об'єкт делегату Action<T> або відповідний лямбда-вираз, який вказує на метод який треба виконати в асінхроній манері.
Об'єкт Dispatcher є екземпляром батьківського класу WPF. Він дозволяє визвати метод що оновить інтерфейс користувача. Тепер можно бачити процес обробки зображень та змінювати текстове поле одночасно.

## Обробка запиту на сказуваня обробку.

Якщо зображень для обробки буде дуже багато, може виникнути бажаня закінчити процес не дочекавчись його завершення. Тобто треба дозволити користувачу преравти процес обробки зображень при виконаниі циклу за допомогою кнопки Cencel.  Методи Parallel.For() та Parallel.ForEach() підтримують скасування за допомогою маркерів(token) скасування. Коли ви викликаєте методи Parallel, можна передати об'єкт ParalellOptions, який в свою чергу містить об'єкт CancellationTokenSource.

С початку додамо приватну зміну-член класу MainWindow.

```cs
    public partial class MainWindow : Window
    {
        private CancellationTokenSource _cancellationTokenSource;
    ...
    }    
```
Додамо кнопку і обробники.
```
            <Button x:Name="cmdProcessWithCancellation" Grid.Column="1" Content="Process with Cancellation" HorizontalAlignment="Left" Margin="586,0,0,0" VerticalAlignment="Center" Width="142" 
                Click="ProcessWithCancellation_Click"/>
```
```cs
        private void ProcessWithCancellation_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(ProcessFilesWithCancellation);
        }
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            // This will be used to tell all the worker threads to stop!
            _cancellationTokenSource?.Cancel();
        }
```
```cs
        private void ProcessFilesWithCancellation()
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
                Parallel.ForEach(files, parallelOptions, currentFile =>
                {
                    parallelOptions.CancellationToken.ThrowIfCancellationRequested();
                    
                        string filename = Path.GetFileName(currentFile);
                        int threadId = Environment.CurrentManagedThreadId;

                        Dispatcher?.Invoke(() =>
                        {
                            Title = $"Processing. Thread:{threadId}   File:{filename}";
                        });

                        using Bitmap bitmap = new(currentFile);
                        bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        bitmap.Save(Path.Combine(outputDirectory, filename));
                    
                });
                Dispatcher?.Invoke(() => { Title = "Process complete"; });
            }
            catch(OperationCanceledException ex)
            {
                Dispatcher?.Invoke(() => { Title = $"Process canceled! {ex.Message}"; });
            } 
        }
```
Зауважте, що метод обробки починається із налаштування об’єкта ParallelOptions, встановлюючи властивість CancellationToken на використання маркера CancellationTokenSource. Також зауважте, що коли викликається метод Parallel.ForEach(), передається об’єкт ParallelOptions як другий параметр.
В середині циклу перевіряється чи не нвзначено в токені скасування. Якшо так викидається виняток і всі потоку зупиняються. Потім виняток перехоплюється і виводиться напис на заголовок.


## Паралелізм Task за допомогою класу Parallel.

Окрім пралелізму даних, TPL можна використовувати для легкого запуску будь-якої кількості асінхроних завдань за допомогою методу Parallel.Invoke(). Цей підхід є трохи більш простим, ніж використання членів із System.Threading; однак, якщо вам потрібен більший контроль над тим, як виконуються завдання, ви можете відмовитися від використання Parallel.Invoke() і використовувати безпосередньо клас Task, як це було в попередньому прикладі.
Спробуємо отримати загальнодоступну електрону книгу, а потім паралельно виконати набір тривалих завдань.

```cs
void DownloadBookAndGetStatistic()
{
    string _textEbook = string.Empty;
    GetBook();
    Console.WriteLine("Downloding book ... ");
    Console.ReadLine();


    void GetBook()
    {
        using WebClient webClient = new WebClient();
        webClient.DownloadStringCompleted += (s, eArgs) =>
        {
            _textEbook = eArgs.Result;
            Console.WriteLine("Download complete.");
            GetStats();
        };
        webClient.DownloadStringAsync(new Uri("http://www.gutenberg.org/files/98/98-0.txt"));
    }

    void GetStats()
    {
        string[] words = _textEbook.Split(new char[] { ' ', '\u000A', ',', '.', ';', ':', '-', '?', '/' },StringSplitOptions.RemoveEmptyEntries);


        string[] tenMostCommon = [];
        string longestWord = string.Empty;

        // Witout Parallel
        //tenMostCommon = FindTenMostCommon(words);
        //longestWord = FindLongestWord(words);

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

        Console.WriteLine(stringBuilder.ToString(),"Book info");
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
DownloadBookAndGetStatistic();
```
```
Downloding book ...
Download complete.
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
Клас WebClient є членом System.Net. Він застарілий клас і використовується для прикладу. Цей клас надає методи для надсилання даних і отримання даних з ресурсу, визначеного URI. Як виявилося, багато з цих методів мають асинхронну версію, наприклад DownloadStringAsync(). Цей метод автоматично створить новий потік із пулу потоків .NET Core Runtime. 
В прикладі додадно обробнип події на закінченя завантаженя у вигляді лямда виразу. 
Якщо викликаєи синхронну версію цього методу (DownloadString()), повідомлення «Download complete.» не відображатиметься, доки не завершиться завантаження. 
Виклик обробників масиву слів можна виконати використвани метод Invoкe класу Parallel. Метод очікує масиву з об'єктами типу делегат Action<> або лямбда-виразів. Перевага полягає в тому, що TPL тепер використовуватиме всі можливі процесори на машині для виклику кожного методу паралельно, якщо це можливо. 


## Паралельні запити LINQ (PLINQ)

Є інший спосіб, яким ви можете включити паралельні завдання у свої програми. Можна використати набір методів розширення, які дозволяють створювати запити LINQ, який виконуватиме робочі навантаження паралельно.Відповідно, запити LINQ, розроблені для паралельного виконання, називаються запитами PLINQ. Подібно до паралельного коду, створеного за допомогою класу Parallel, у PLINQ є можливість ігнорувати ваш запит на паралельну обробку колекції, якщо це необхідно.
Під час виконання PLINQ аналізує загальну структуру запиту, і якщо розпаралелювання виграє від запиту, він виконуватиметься одночасно.Однак, якщо розпаралелювання запиту погіршить продуктивність, PLINQ просто виконує запит послідовно.Якщо PLINQ має вибір між потенційно дорогим паралельним алгоритмом або недорогим послідовним алгоритмом, він вибирає послідовний алгоритм за замовчуванням.
Необхідні методи розширення знаходяться в класі ParallelEnumerable простору імен System.Linq.
Деякі корисні розширення PLINQ.

    AsParallel() : Вказує, що решту запиту слід розпаралелювати, якщо це можливо

    WithCancellation() : Вказує, що PLINQ має періодично відстежувати стан наданого маркера скасування та скасовувати виконання, якщо це запитується

    WithDegreeOfParallelism() : Визначає максимальну кількість процесорів, які PLINQ має використовувати для розпаралелювання запиту

    ForAll() : Дозволяє паралельно обробляти результати без попереднього злиття з потоком споживача, як у випадку перерахування результату LINQ за допомогою ключового слова foreach

Спробуємо створити запит з використанням звичайної так і паралельної роботи.

PLINQDataProcessing\Program.cs
```cs
void UsingPLINQ()
{
    Console.WriteLine("Processing");
    Task.Factory.StartNew(ProcessingIntData);
    Task.Factory.StartNew(ProcessingIntDataWithPLINQ);
    Console.ReadKey();


    void ProcessingIntData()
    {
        // Get a very large array of integers.
        int[] ints = Enumerable.Range( 0, 100_000_000 ).ToArray();

        // Find the numbers where num % 3 == 0 is true, returned
        // in descending order.
        var query = from number in ints
                    where number%3 == 0
                    orderby number descending
                    select number;

        var watch = Stopwatch.StartNew();
        
        int[] modThreeIsZero = query.ToArray();
  
        watch.Stop();
        Console.WriteLine($"Time:{watch.ElapsedMilliseconds}");
        Console.WriteLine($"\tAmount:{modThreeIsZero.Count()}");
    }

    void ProcessingIntDataWithPLINQ()
    {
        // Get a very large array of integers.
        int[] ints = Enumerable.Range(0, 100_000_000).ToArray();
        // Find the numbers where num % 3 == 0 is true, returned
        // in descending order.

        var query = from number in ints.AsParallel()
                    where number % 3 == 0
                    orderby number descending
                    select number;

        var watch = Stopwatch.StartNew();

        int[] modThreeIsZero = query.ToArray();

        watch.Stop();
        Console.WriteLine($"With Paralell\nTime:{watch.ElapsedMilliseconds}");
        Console.WriteLine($"\tAmount:{modThreeIsZero.Count()}");
    }

}
UsingPLINQ();

```
```
Processing
With Paralell
Time:14680
        Amount:33333334
Time:20064
        Amount:33333334
```
При виконані запускаються два завдання які відрізняються запитом який досліджує великий масив. Включивши виклик AsParallel(), TPL намагатиметься передати навантаження на будь-який доступний ЦП. Зверніть увагу що якщо навантаженя невеликі то затрати на те, шоб зробити обробку паралельною, займають більше часу ніж звичайна обробка. Це можна побачит зменьшивши об'єм масиву.

### Скасування виконання запиту PLINQ.

Також можна використовувати об’єкт CancellationTokenSource, щоб повідомити запиту PLINQ припинити обробку за правильних умов (як правило, через втручання користувача).
```cs
void CancellationPLINQ()
{
    CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    
    Console.WriteLine("Processing ...");
    Task.Factory.StartNew(ProcessingIntDataWithPLINQAndCancellation);
    
    Console.Write("To cancel press Q:");
    string? answer = Console.ReadLine();
    if (answer != null && answer.Equals("Q", StringComparison.OrdinalIgnoreCase))
    {
        _cancellationTokenSource.Cancel();
    }
    Console.ReadLine();

    void ProcessingIntDataWithPLINQAndCancellation()
    {

        int[] ints = Enumerable.Range(0, 100_000_000).ToArray();

        var query = from number in ints.AsParallel().WithCancellation(_cancellationTokenSource.Token)
                    where number % 3 == 0
                    orderby number descending
                    select number;
        try
        {
            var watch = Stopwatch.StartNew();

            int[] modThreeIsZero = query.ToArray();

            watch.Stop();
            Console.WriteLine($"\nWith Paralell\nTime:{watch.ElapsedMilliseconds}");
            Console.WriteLine($"\tAmount:{modThreeIsZero.Count()}");
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
CancellationPLINQ();
```
```
Processing ...
To cancel press Q:
With Paralell
Time:12724
        Amount:33333334

```
```
Processing ...
To cancel press Q:q
The query has been canceled via the token supplied to WithCancellation.
```
На рівні де створюєьбся завданя повинен бути доступний об'єкт CancellationTokenSource. На цьомуж рівні відбуваеться опитування користувача на необхіжність скасування.
При створенні запиту ми вказуємо що він повинен стежити за вхідним запитом на скасування. Церобиться в ланцюгу виклику об'єктів розширення. Маркер передається в метод WithCancellation.

```cs
        var query = from number in ints.AsParallel().WithCancellation(_cancellationTokenSource.Token)
                    where number % 3 == 0
                    orderby number descending
                    select number;
```
При відміні виникає виняток який треба обробити. Як показує приклад відмінити обробку можна за період часу який показує обробник без скасування.

## Асинхронні виклики з використанням шаблону async/await.

Цей шаблон, це спроба зробити можливість писати код послідовно але виконуватись він буде з можливістю єкономії часу на виконаня і звільнення основних потоків.
Ключове слово async використовується для визначення того, що метод , лямбда-вираз або анонімний метод повинні викликатися в асінхронному режимі автоматично. Просто позначивши метод модифікатором async, .NET Core Runtime створить новий потік виконання для вирішення завдання методу. Крім того, коли ви викликаєте асинхронний метод, ключове слово await автоматично призупинить поточний потік від будь-якої подальшої діяльності до завершення завдання, залишаючи потік, що викликає, вільним для продовження.

Припустимо у нас є метод який щось довго робить.  

AsyncAwait\SimpleUsingAsyncAwait\Program.cs

```cs
static string DoLongWork()
{
    Thread.Sleep(3000);
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
sstatic async Task<string> DoLongWorkAsync()
{
    return await Task.Run(() =>
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
        Thread.Sleep(5000);
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

Хоча це може здатися життєздатним варіантом для сценаріїв “fire and forget”, існує більша проблема. Якщо метод викидає виняток, йому нікуди йти, крім контексту синхронізації методу, що викликає.

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
Таким чином краше уникати визначення метода як async void, а краще використовувати async Task. Як мінімум програма не буде аварійно закінчуватись.

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
Частіше є потрема не чекати послідовно виконання кожного завдання а чекати коли вони всі виконаються. Це більш вірогідний сценарій, коли є три речі (перевірити електронну пошту, оновити сервер, завантажити файли), які потрібно виконати пакетно, але їх можна виконати паралельно. 
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
В цьому випадку всі завданя запускаються одночасно але основний потік звільняється коли будьяке завдання закінчується. 
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
Додамо зміну рівня класа та обробник події натискання Cancel.

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

                if (!_cancellationTokenSource.IsCancellationRequested)
                {
                    Title = "Process complite";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            _cancellationTokenSource = null;
        }
```
При виникнені винятку цикл переривається.  

