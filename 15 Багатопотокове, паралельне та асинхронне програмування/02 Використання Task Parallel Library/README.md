# Використання Task Parallel Library

Крім використання простору імен System.Threading для стоврення багатопотокових програм існує інший підхід, використовуючи бібліотеку паралельного програмування під назвою Task Parallel Library (TPL). Використовуючи типи  з System.Threading.Tasks, можна створювати детальний, маштабований паралельний код без необхідності працювати безпосередньо з потоками.
Однак це не означає, що ви не використовуватимете типи System.Threading, коли використовуєте TPL. Обидва набори інструментів розробки можуть працювати разом цілком природно. Це особливо вірно в тому, що простір імен System.Threading все ще надає більшість основних можливостей синхронізації, які ви досліджували раніше (Monitor, Interlocked тощо). Однак ви, швидше за все, виявите, що віддасте перевагу роботі з TPL, а не з оригінальним простором імен System.Threading, враховуючи, що той самий набір завдань можна виконувати більш простим способом.

## Простір імен System.Threading.Tasks

У сукупності типи System.Threading.Tasks називають бібліотекою паралельних завдань. TPL автоматично динамічно розподілятиме робоче навантаження вашої програми між доступними ЦП, використовуючи пул потоків виконання. TPL обробляє розподіл роботи, планування потоків, керування станом та інші деталі низького рівня. Результатом є те, що ви можете максимізувати продуктивність своїх програм .NET Core, захищаючись від багатьох складнощів безпосередньої роботи з потоками.

## Класс Task.

В основі TPL лежить концепція завдань, які представняють собою окреме довге навантаження. Для завдань існує клас Task з простору імен System.Threading.Tasks. Цей клас використовується для асінхроного запуску окремого навантаження в одному з потоків із пулу. Також завдання можна виконати і синхроно.

## Створення і запуск завдання.

Завдання можно створити і запустити декількома способами:

UsingTask\Programm.cs
```cs
void RunTasks()
{
    Task task1 = new Task(() => { Console.WriteLine($"Task 1 on {Thread.CurrentThread.ManagedThreadId}");});
    task1.Start();

    Task task2 = Task.Factory.StartNew(() => { Console.WriteLine($"Task 2 on {Thread.CurrentThread.ManagedThreadId}"); ; ;});

    Task task3 = Task.Run(() => { Console.WriteLine($"Task 3 on {Thread.CurrentThread.ManagedThreadId} "); });

    task1.Wait();
    task2.Wait();
    task3.Wait();

}
RunTasks();

``` 
```
Task 1 on 6
Task 3 on 8
Task 2 on 7
```    
Якшо не використовувати метод Wait завдання основного потоку закінчитися бистріше аніж виконається завдання окремих потоків. 
Цей метод блокує поток в якому викликається завданя поки вона не виконається 

UsingTask\Programm.cs
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

UsingTask\Programm.cs
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

## Внутрішні завдання

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
Задачі виконуються незалежно одна від одної. Як видно з прикладу зовнішне завдання може закінчитись не дочекавшись виконання внутрішнього.

Можна вказати внутрішньому завданню що вона є частиною зовнішнього.

UsingTask\Programm.cs
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
        //inner.Wait();
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

## Массив завдань

Якшо маємо масив завдань можемо їх запустити і вказати основному потоку шо треба дочекатись аби всі завдання закінчились.

UsingTask\Programm.cs
```cs
void ArrayOfTasks()
{
    Task[] tasks =
    [
        new Task( () => 
        {
            Console.WriteLine("Task 1 started.");
            Thread.Sleep(3000); 
            Console.WriteLine("Task 1 finished.");}),

        new Task(() =>
        {
            Console.WriteLine("Task 2 started.");
            Thread.Sleep(3000);
            Console.WriteLine("Task 2 finished.");
        }),
        new Task(() =>
        {
            Console.WriteLine("Task 3 started.");
            Thread.Sleep(3000);
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
Крім того існує метод Task.WaitAny який затримує основний потік докі не закінчить виконання будь якє завдання. 

## Повернення результату виконаного завдання.

Об'єкт Task може повертати результат виконання завдання.

UsingTask\Programm.cs
```cs
void ObtainingTheResultOfTheTask()
{
    Console.Write($"Thread:{Thread.CurrentThread.ManagedThreadId} Enter number :");
    
    int.TryParse(Console.ReadLine(),out int x);

    Task<int> squareTask = new(() =>
    {
        Console.WriteLine($"Thread:{Thread.CurrentThread.ManagedThreadId}");
        Thread.Sleep(3000);
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

## Завдання на продовження.

Завдання може бути виконанне після виконання іншого. Воно може отримати від іншого завдання результату.

UsingTask\Programm.cs
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
Це нагадує callback. Після виконання першого завдання викликається інше завдання. В метод наступного передається виконане завдання.

Завданна на продовження може оримати результат від попередьнього завдання.

UsingTask\Programm.cs
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

UsingTask\Programm.cs
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

UsingParalell\Program.cs
```cs
void SimpleUsingInvoke()
{
    Parallel.Invoke(
        () => {
            Console.WriteLine($"Task:{Task.CurrentId}");
            Thread.Sleep(1000);
            Console.WriteLine("Hi");

        },        
        ()=>SayWithDelay("girl",3000),
        
        SayGoodbay       
        );

    void SayGoodbay()
    {
        SayWithDelay("Goodbay", 5000);
    }
    
    void SayWithDelay(string phrase, int delay) 
    {
        Console.WriteLine($"Task:{Task.CurrentId}");
        Thread.Sleep(delay);
        Console.WriteLine(phrase);
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

UsingParalell\Program.cs
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
            Thread.Sleep(1000);
        }

    },cancellationToken);
    
    task.Start();

    Thread.Sleep(7000);

    cancellationTokenSource.Cancel();

    Console.WriteLine($"Task status:{task.Status}");
    
    Console.ReadLine();
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
6
Task status:Running
Task canceled

```
Для обробки скасування спочатку створюється об'єкт CancellationTokenSource який керує та відсилає повідомлення про скасування. В конструктор завдання надсилаеться властивість цього об'єкта CancellationTokenSource.Token. Коли поза завданням викликаеться метод CancellationTokenSource.Cancel() відсилаеться повідомленя про скасування і властивість в стуктурі cancellationToken.IsCancellationRequested примає значення true. В процесі проходження циклу можна перевірити чи не будо відмінено завдання. 

Виконати скасування циклу можно викинувши Exception.
UsingParalell\Program.cs
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

Крім методу Invoce клас Paralell містить методи які дозволяють перебирати колекцію даних (зокрема, об'єкт що реалізовує IEnumerable<T>) в паралельний спосіб. Це робиться за допомогою двох основних статичних методів, Parallel.For() та Parallel.ForEach(), кожен з яких визначає численні презавантажені версії. 
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

Для цього прикладу потрібен додатковий пакет NuGet (System.Drawing.Common). Він встановлюється командою:

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
Зверніть увагу, що метод ProcessFilesWithoutParalell() обертатиме кожен файл *.jpg у вказаному каталозі. Наразі вся робота відбувається над основним потоком виконуваного файлу. При натискані кнопки починається процес обробки і можна побачити що все відбувається в основному потоці програми з ID 1. При виконані процесу ми не можемо змінити дані TextBox покі всі зображення не будуть оброблені. Таким чином робота програми "зависає". 

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

## Обробка запиту на сказуваня обробки.

Якщо зображень для обробки буде дуже багато, може виникнути бажаня закінчити процес не дочекавчись його завершення. Тобто треба дозволити користувачу преравти процес обробки зображень при виконаниі циклу за допомогою кнопки Cencel.  Методи Parallel.For() та Parallel.ForEach() підтримують скасування за допомогою маркерів(token) скасування. Коли ви викликаєте методи Parallel, можна передати об'єкт ParalellOptions, який в свою чергу містить об'єкт CancellationTokenSource.

С початку додамо приватну зміну-член класу MainWindow.

```cs
    public partial class MainWindow : Window
    {
        private CancellationTokenSource _cancellationTokenSource;
    ...
    }    
```
Додамо кнопку і обробники натисканя для доданої, а також для кнопки Cancel.
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

StudyTPL\MyEBookReader\Program.cs
```cs
void DownloadBookAndGetStatistic()
{
    string _textEbook = string.Empty;
    Console.WriteLine("Downloding book ... ");
    GetBook();
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
        Console.WriteLine("Work done.");
    }

    string[] FindTenMostCommon(string[] words)
    {
        Console.WriteLine($"Method FindTenMostCommon in Thread:{Thread.CurrentThread.ManagedThreadId}");
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
        Console.WriteLine($"Method FindLongestWord in Thread:{Thread.CurrentThread.ManagedThreadId}");
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
Method FindLongestWord in Thread:6
Method FindTenMostCommon in Thread:9
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

Work done.

```
Клас WebClient є членом System.Net. Він застарілий клас і використовується для прикладу. Цей клас надає методи для надсилання даних і отримання даних з ресурсу, визначеного URI. Багато з цих методів мають асинхронну версію, наприклад DownloadStringAsync(). Цей метод автоматично створить новий потік із пулу потоків .NET Core Runtime. 
В прикладі додадно обробник події на закінченя завантаженя у вигляді лямда виразу. 
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

StiudyTPL\PLINQDataProcessing\Program.cs
```cs
void UsingPLINQ()
{
    Console.WriteLine("Processing");
    Task.Factory.StartNew(ProcessingIntData);
    Task.Factory.StartNew(ProcessingIntDataWithPLINQ);
    Console.ReadLine();


    void ProcessingIntData()
    {
        // Get a very large array of integers.
        int[] ints = Enumerable.Range( 0, 50_000_000 ).ToArray();
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
        int[] ints = Enumerable.Range(0, 50_000_000).ToArray();
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
Time:3395
        Amount:16666667
Time:5114
        Amount:16666667
```
При виконані запускаються два завдання які відрізняються запитом який досліджує великий масив. Включивши виклик AsParallel(), TPL намагатиметься передати навантаження на будь-який доступний ЦП. Зверніть увагу що якщо навантаженя невеликі то затрати на те, шоб зробити обробку паралельною, займають більше часу ніж звичайна обробка. Це можна побачит зменьшивши об'єм масиву.

### Скасування виконання запиту PLINQ.

Також можна використовувати об’єкт CancellationTokenSource, щоб повідомити запиту PLINQ припинити обробку за правильних умов (як правило, через втручання користувача).
```cs
    void ProcessingIntDataWithPLINQAndCancellation()
    {

        int[] ints = Enumerable.Range(0, 50_000_000).ToArray();

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
        catch(Exception ex)
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
Time:3427
        Amount:16666667

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
