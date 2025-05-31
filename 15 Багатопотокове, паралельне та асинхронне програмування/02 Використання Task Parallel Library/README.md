# Використання Task Parallel Library

На цьому етапі ви розглянули об'єкти простору імен System.Threading, які дозволяють створювати багатопотокове програмне забезпечення. Більш новий підхід до розробки багатопотокових застосунків полягає у використанні бібліотеки паралельного програмування під назвою Task Parallel Library (TPL). Використовуючи типи System.Threading.Tasks, ви можете створювати гарно розділений на частини, масштабований паралельний код без необхідності працювати безпосередньо з потоками або пулом потоків.
Однак це не означає, що ви не використовуватимете типи System.Threading, коли використовуватимете TPL. Обидва набори інструментів для потокової обробки можуть працювати разом цілком природно. Це особливо актуально, оскільки простір імен System.Threading все ще надає більшість примітивів синхронізації, які ви розглядали раніше (Monitor, Interlocked тощо). Однак, ви, ймовірно, виявите, що надасте перевагу роботі з TPL, а не з оригінальним простором імен System.Threading, враховуючи, що той самий набір завдань можна виконувати простіше.

## Простір імен System.Threading.Tasks 

Разом типи System.Threading.Tasks називаються бібліотекою паралельного виконання завдань Task Parallel Library (TPL). TPL автоматично розподілятиме робоче навантаження вашої програми між доступними процесорами динамічно, використовуючи пул потоків середовища виконання. TPL обробляє розподіл роботи, планування потоків, керування станом та інші низькорівневі деталі. В результаті ви можете максимізувати продуктивність своїх .NET Core-застосунків, одночасно захищаючись від багатьох складнощів безпосередньої роботи з потоками.

# Класс Task

В основі TPL лежить концепція завдань, які представняють собою окреме довге навантаження. Для завдань існує клас Task з простору імен System.Threading.Tasks. Цей клас використовується для асінхроного запуску окремого навантаження в одному з потоків із пулу. Також завдання можна виконати і синхроно.

## Створення і запуск завдання

Завдання можно створити і запустити декількома способами:

UsingTask\Programm.cs
```cs
void DoWork(int time, int taskId)
{
    Thread.Sleep(time);

    Thread thread = Thread.CurrentThread;

    string threadInfo = "\n"+
        $"\tManagedThreadId: {thread.ManagedThreadId}\t" +
        $"\tIsBackground: {thread.IsBackground}" +
        $"\tIsAlive:{thread.IsAlive}" +
        $"\tThreadState:{thread.ThreadState}";

    Console.WriteLine(threadInfo);

    int id = Thread.CurrentThread.ManagedThreadId;
    Console.WriteLine($"Task {taskId} in thread:{id} finished.\n");
}

void RunTasks()
{
    Task task1 = new Task(() => DoWork(2000,1));
    task1.Start();

    Task task2 = Task.Factory.StartNew(() => DoWork(2000,2));

    Task task3 = Task.Run(() => DoWork(1000,3));

    Console.WriteLine($"All tasks are working.");

    task1.Wait();
    task2.Wait();
    task3.Wait();
}
RunTasks();
```
```
All tasks are working.

        ManagedThreadId: 8              IsBackground: True      IsAlive:True    ThreadState:Background
Task 3 in thread:8 finished.


        ManagedThreadId: 7              IsBackground: True      IsAlive:True    ThreadState:Background

        ManagedThreadId: 6              IsBackground: True      IsAlive:True    ThreadState:Background
Task 1 in thread:6 finished.

Task 2 in thread:7 finished.
```
Як бачите конструктор типу Task приймає тип делегата Action без параметрів. В ціх викликах в окремому фоновому виконується метод на який вказує делегат. Виконання ведеться асінхронно. Перше завдання виконується те шо меньше займає часу.  Якшо не використовувати метод Wait завдання основного потоку закінчитися бистріше аніж виконається завдання окремих потоків бо потоки фонові. Цей метод блокує поток в якому викликається завданя поки вона не виконається. Спробуйте закоментувати ці рядки.

```
All tasks are working.
```
Це означає що всі вони були зупинені коли основний потік закінчився.

Класс Task має певні властивості.

```cs
void PropertyOfTask()
{

    Task task = new(() =>
    {
        //Console.WriteLine($"Task Id: {Task.CurrentId}");
        //Thread.Sleep(2000);
        DoWork(2000, 2);
    });
    task.Start();

    Console.WriteLine($"Id: {task.Id}");
    Console.WriteLine($"IsCompleted: {task.IsCompleted}");
    Console.WriteLine($"IsFaulted: {task.IsFaulted}");
    Console.WriteLine($"IsCanceled: {task.IsCanceled}");
    Console.WriteLine($"Status: {task.Status}");
    Console.WriteLine($"IsCompletedSuccessfully:{task.IsCompletedSuccessfully}");

    task.Wait();

    Console.WriteLine($"Id: {task.Id}");
    Console.WriteLine($"IsCompleted: {task.IsCompleted}");
    Console.WriteLine($"IsFaulted: {task.IsFaulted}");
    Console.WriteLine($"IsCanceled: {task.IsCanceled}");
    Console.WriteLine($"Status: {task.Status}");
    Console.WriteLine($"IsCompletedSuccessfully:{task.IsCompletedSuccessfully}");

}
PropertyOfTask();
```
```
Id: 1
IsCompleted: False
IsFaulted: False
IsCanceled: False
Status: Running
IsCompletedSuccessfully:False

        ManagedThreadId: 6              IsBackground: True      IsAlive:True    ThreadState:Background
Task 2 in thread:6 finished.

Id: 1
IsCompleted: True
IsFaulted: False
IsCanceled: False
Status: RanToCompletion
IsCompletedSuccessfully:True
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
            DoWork(1000, 2);
            Console.WriteLine("Inner task finished.");
        });
        inner.Wait();
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
Inner task starting.

        ManagedThreadId: 6              IsBackground: True      IsAlive:True    ThreadState:Background
Task 2 in thread:6 finished.

Inner task finished.
Outer task finished.
End main
```
Задачі виконуються незалежно одна від одної. Якщо закоментувати inner.Wait() буде видно що зовнішне завдання може закінчитись не дочекавшись виконання внутрішнього. 

```
Start main
Outer task starting.
Outer task finished.
Inner task starting.
End main
```

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
        }, TaskCreationOptions.AttachedToParent);
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

        ManagedThreadId: 7              IsBackground: True      IsAlive:True    ThreadState:Background
Task 2 in thread:7 finished.

Inner task finished.
End main
```
При створені завдання вказується додадковий параметр TaskCreationOptions.AttachedToParent. Зновуж такі виконання внутрішнього завдання може бути незалежно. 

## Массив завдань

Якшо маємо масив завдань можемо їх запустити і вказати основному потоку шо треба дочекатись аби всі завдання закінчились.

```cs
void ArrayOfTasks()
{
    Task[] tasks =
    [
        new Task(() => DoWork(3000,1)),
        new Task(() => DoWork(3000,2)),
        new Task(() => DoWork(3000,3))
    ];

    for (int i = 0; i < 3; i++)
    {
        tasks[i].Start();
    }
    Console.WriteLine($"All tasks are working.");
    Task.WaitAll(tasks);
}
ArrayOfTasks();
```
```
All tasks are working.

        ManagedThreadId: 6              IsBackground: True      IsAlive:True    ThreadState:Background
Task 1 in thread:6 finished.


        ManagedThreadId: 8              IsBackground: True      IsAlive:True    ThreadState:Background
Task 3 in thread:8 finished.


        ManagedThreadId: 7              IsBackground: True      IsAlive:True    ThreadState:Background
Task 2 in thread:7 finished.
```
В синхронному виклику навантаженя потребувало б в три рази більше часу. Крім того існує метод Task.WaitAny який затримує основний потік докі не закінчить виконання будь якє завдання.

## Повернення результату виконаного завдання

Об'єкт Task може повертати результат виконання завдання. Для того щоб завдання повернула результат його треба типізувати типом який хочемо отримати.

```cs
int SlowlyGetSquare(int x)
{
    DoWork(2000, 1);
    return x*x;
}

void ObtainingTheResultOfTheTask()
{
    Console.Write($"Primary thread:{Thread.CurrentThread.ManagedThreadId}\nEnter number :");

    int.TryParse(Console.ReadLine(), out int x);

    Task<int> squareTask = new(() =>
    {
        Console.WriteLine($"Thread:{Thread.CurrentThread.ManagedThreadId}");
        return SlowlyGetSquare(x);
    });

    squareTask.Start();

    int result = squareTask.Result;

    Console.WriteLine($"Result:{result}");
}
ObtainingTheResultOfTheTask();
```
При звернені до власитвості task.Result поточний потік призупиняється і чекає виконання завданя в якому буде визначено результат.
```
Primary thread:1
Enter number :5
Thread:6

        ManagedThreadId: 6              IsBackground: True      IsAlive:True    ThreadState:Background
Task 1 in thread:6 finished.

Result:25
```


## Завдання на продовження.

Завдання може бути виконанне після виконання іншого. Воно може отримати від іншого завдання результату.

```cs
void ContinuationTask()
{
    Task firstTask = new Task(() => Console.WriteLine($"Task Id:{Task.CurrentId}") );

    Task nextTask = firstTask.ContinueWith(AboutTask);

    firstTask.Start();

    firstTask.Wait();

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

```cs
void ContinuationTaskWithValue()
{
    int t = 5000;

    Task<int> squareTask = new(() =>
    {
        Console.WriteLine($"Task {Task.CurrentId} says: I'm starting");
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

        ManagedThreadId: 6              IsBackground: True      IsAlive:True    ThreadState:Background
Task 1 in thread:6 finished.

Task 2 says: I waited 25
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

## Скасування завдань.

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
            Console.Write($"{i} ");
            Thread.Sleep(500);
        }
    }, cancellationToken);
    task.Start();

    Thread.Sleep(3000);

    cancellationTokenSource.Cancel();

    Console.ReadLine();
}
CancellationWitoutThrow();
```
```
0 1 2 3 4 5 Task canceled
```
Для обробки скасування спочатку створюється об'єкт CancellationTokenSource який керує та відсилає повідомлення про скасування. В конструктор завдання надсилаеться властивість цього об'єкта CancellationTokenSource.Token. Коли поза завданням викликаеться метод CancellationTokenSource.Cancel() відсилаеться повідомленя про скасування і властивість в стуктурі cancellationToken.IsCancellationRequested примає значення true. В процесі проходження циклу можна перевірити чи не будо відмінено завдання.

Виконати скасування завдання можно викинувши Exception.
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

            Console.Write($"{i} ");
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
0 1 2 3 4 5 Task canceled
Task status:Canceled
```

# Роль класу Parallel.

Дуже важливим класом TPL є System.Threading.Tasks.Parallel. 

## Просте використання класу Parallel

Найпростіше запустити завдання паралельно можна за допомогою методу Invoke.

```cs
void SimpleUsingInvoke_1()
{
    Parallel.Invoke(
        () => DoWork(3000, 1),
        () => DoWork(1000, 2),
        () => DoWork(3000, 3),
        () => Console.WriteLine("Hi")
        );
}
SimpleUsingInvoke_1();
```
```
Hi

        ManagedThreadId: 8              IsBackground: True      IsAlive:True    ThreadState:Background
Task 2 in thread:8 finished.


        ManagedThreadId: 4              IsBackground: True      IsAlive:True    ThreadState:Background

        ManagedThreadId: 1              IsBackground: False     IsAlive:True    ThreadState:Running
Task 3 in thread:4 finished.

Task 1 in thread:1 finished.
```
В якості параметру метод Invoke отримує params Action[]. Таким чином ми можемо передати методи яки середовище буде виконувати паралельно. Але виконання методу Invoke відбувається в сінхронному контексі і основний потік чекає виконання всіх завдань.

Сінхронний виклик тих самих методів в одному потоці виконується довше.

```cs
void SimpleUsingInvoke_2()
{
    var watch = Stopwatch.StartNew();

    Parallel.Invoke(
        () => DoWork(3000, 1),
        () => DoWork(2000, 2),
        () => DoWork(3000, 3),
        () => Console.WriteLine("Hi")
        );

    watch.Stop();
    Console.WriteLine($"\tTime: {watch.ElapsedMilliseconds}");

    watch = Stopwatch.StartNew();

    DoWork(3000, 1);
    DoWork(2000, 2);
    DoWork(3000, 3);
    Console.WriteLine("Hi");

    watch.Stop();
    Console.WriteLine($"\tTime: {watch.ElapsedMilliseconds}");
}
SimpleUsingInvoke_2();
```
```
Hi

        ManagedThreadId: 8              IsBackground: True      IsAlive:True    ThreadState:Background
Task 2 in thread:8 finished.


        ManagedThreadId: 4              IsBackground: True      IsAlive:True    ThreadState:Background
Task 3 in thread:4 finished.


        ManagedThreadId: 1              IsBackground: False     IsAlive:True    ThreadState:Running
Task 1 in thread:1 finished.

        Time: 3048

        ManagedThreadId: 1              IsBackground: False     IsAlive:True    ThreadState:Running
Task 1 in thread:1 finished.


        ManagedThreadId: 1              IsBackground: False     IsAlive:True    ThreadState:Running
Task 2 in thread:1 finished.


        ManagedThreadId: 1              IsBackground: False     IsAlive:True    ThreadState:Running
Task 3 in thread:1 finished.

Hi
        Time: 8029
```
Як бачимо виконання в одному потоці проходить довше ніж в параллельних потоках.


## Parallel.For() та Parallel.ForEach()

Крім методу Invoce клас Paralell містить методи, що дозволяють вам виконувати ітерацію по колекції даних (зокрема, об'єкту, що реалізує IEnumerable\<T\>) паралельним чином, головним чином за допомогою двох основних статичних методів, Parallel.For() та Parallel.ForEach(), кожен з яких визначає численні перевантажені версії.
Ці методи дозволяють вам створювати набір операторів коду, які будуть оброблятися паралельно. За концепцією, ці оператори є такою ж логікою, яку ви б написали у звичайній конструкції циклу (через ключове слово for або foreach C#). Перевага полягає в тому, що клас Parallel вибиратиме потоки з пулу потоків (і керуватиме паралельністю) від вашого імені.
Обидва методи вимагають вказати контейнер, сумісний з IEnumerable або IEnumerable\<T\>, який містить дані, необхідні для паралельної обробки. Контейнер може бути простим масивом, неузагальненою колекцією (наприклад, ArrayList), узагальненою колекцією (наприклад, List\<T\>) або результатами запиту LINQ.
Крім того, вам потрібно буде використовувати делегати System.Func\<T\> та System.Action\<T\>, щоб вказати цільовий метод, який буде викликано для обробки даних. Нагадаємо, що Func\<T\> представляє метод, який може мати задане повернене значення та різноманітну кількість аргументів. Делегат Action\<T\> подібний до Func\<T\> тим, що він дозволяє вказувати на метод, що приймає певну кількість параметрів. Однак Action\<T\> визначає метод, який може повертати лише значення void.
Хоча ви можете викликати методи Parallel.For() та Parallel.ForEach() та передати об'єкт делегату Func\<T\> або Action\<T\> зі строгою типізацією, ви можете спростити своє програмування, використовуючи відповідний анонімний метод C# або лямбда-вираз.

## Паралелізм даних з класом Parallel

Перший спосіб використання TPL – це паралельне оброблення даних. Простіше кажучи, цей термін стосується завдання паралельного перебору масиву або колекції за допомогою методу Parallel.For() або Parallel.ForEach(). Припустимо, вам потрібно виконати деякі трудомісткі операції вводу/виводу файлів. Зокрема, вам потрібно завантажити велику кількість файлів *.jpg у пам'ять, перевернути їх догори дном і зберегти змінені дані зображення в новому місці.
У цьому прикладі ви побачите, як виконати те саме загальне завдання за допомогою графічного інтерфейсу користувача, тож ви можете розглянути використання «анонімних делегатів», щоб дозволити вторинним потокам оновлювати основний потік інтерфейсу користувача (тобто потік інтерфейсу користувача).

    Під час створення багатопотокової програми з графічним інтерфейсом користувача (GUI), вторинні потоки ніколи не можуть безпосередньо отримувати доступ до елементів керування інтерфейсом користувача. Причина полягає в тому, що елементи керування (кнопки, текстові поля, підписи, індикатори виконання тощо) мають спорідненість з потоком, який їх створив. У наступному прикладі я проілюструю один зі способів дозволити вторинним потокам отримувати доступ до елементів інтерфейсу користувача потокобезпечним способом. Ви побачите спрощений підхід, розглянувши ключові слова C# async та await.

Для ілюстрації створіть нову програму Windows Presentation Foundation (шаблон скорочено називається WPF Application) з назвою DataParallelismWithForEach. 
Для цього прикладу потрібен додатковий пакет NuGet (System.Drawing.Common). Щоб додати його до свого проєкту, введіть наступний рядок (все в одному рядку) у командному рядку (у тому ж каталозі, що й файл вашого рішення) або в консолі диспетчера пакетів у Visual Studio:

```console
dotnet add DataParallelismWithForEach package System.Drawing.Common
```
Замініть XAML наступним кодом

```xml
<Window x:Class="DataParallelismWithForEach.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:DataParallelismWithForEach"
        mc:Ignorable="d"
        Title="MainWindow" Height="450" Width="800">

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
            <Button Name="cmdCancel" Grid.Row="0" Grid.Column="0" Margin="10,10,0,10" Click="cmdCancel_Click">
                Cancel
            </Button>
            <Button Name="cmdProcess" Grid.Row="0" Grid.Column="2" Margin="0,10,10,10"
                    Click="cmdProcess_Click">
                Click to Flip Your Images!
            </Button>
        </Grid>
    </Grid>
   
</Window>

```
Не хвилюйтеся про те, що означає розмітка або як вона працює; ви витратите багато часу на роботу з WPF пізніше. Графічний інтерфейс програми складається з багаторядкового текстового поля (TextBox) та однієї кнопки (з назвою cmdProcess). Призначення текстової області — дозволити вам вводити дані під час виконання роботи у фоновому режимі, що ілюструє неблокувальний характер паралельного завдання.

Відкрийте файл MainWindow.xaml.cs (двічі клацніть його у Visual Studio — можливо, вам доведеться розгорнути стрілку за допомогою MainWindow.xaml) і додайте наступні оператори using у початок файлу:

```cs
using System.Drawing;
using System.IO;
```
Додайте обробники подій.

```cs
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdProcess_Click(object sender, RoutedEventArgs e)
        {
            Title = $"Starting...";
            var watch = Stopwatch.StartNew();
            ProcessFiles();
            watch.Stop();
            Title = $"Processing Complete. Time:{watch.ElapsedMilliseconds}";
        }

```
Додайте обробник зображень. Відповідно в папці D:\Temp\Pictures

```cs
        private void ProcessFiles()
        {
            var basePath = @"D:\Temp";
            var pictureDirectory = System.IO.Path.Combine(basePath, "Pictures");
            var outputDirectory = System.IO.Path.Combine(basePath, "ModifiedPictures");

            //Clear out any existing files
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
            }
            Directory.CreateDirectory(outputDirectory);

            // Process the image data in a blocking manner.
            string[] files = Directory.GetFiles(pictureDirectory,"*.jpg", SearchOption.AllDirectories);
            foreach (string currentFile in files)
            {
                string filename = System.IO.Path.GetFileName(currentFile);
                // Print out the ID of the thread processing the current image.
                Title = $"Processing on thread {Environment.CurrentManagedThreadId} {filename} ";

                using (Bitmap bitmap = new Bitmap(currentFile))
                {
                    bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    bitmap.Save(System.IO.Path.Combine(outputDirectory, filename));
                }
            }
        }

```
Відповідно в папці D:\Temp\Pictures повині бити тестові зображення. Зверніть увагу, що метод ProcessFiles() буде обертати кожен файл *.jpg у вказаному каталозі. Наразі вся робота виконується в основному потоці виконуваного файлу. Таким чином, коли натискається кнопка обробки файлів, інтерфейс вводу стає не доступним і треба чекати виконання процесцю Крім того, підпис вікна також повідомлятиме, що файл обробляє той самий основний потік, оскільки у нас є лише один потік виконання.

## Робота циклу паралелльно

Щоб обробити файли на якомога більшій кількості процесорів, ви можете переписати поточний цикл foreach для використання Parallel.ForEach(). Цей метод неодноразово перевантажувався; однак, у найпростішій формі, необхідно вказати об'єкт, сумісний з IEnumerable<T>, який містить елементи для обробки (це буде масив рядків files), та делегат Action<T>, який вказує на метод, що виконуватиме цю роботу.
Ось відповідне оновлення, яке використовує лямбда-оператор C# замість літерального об’єкта делегату Action<T>.

Доджамо кнопку і обробник.

```xml
            <Button x:Name="cmdProcessParrallel" Grid.Row="0" Grid.Column="1" Margin="482,10,18,10"
                 Content="Do process parallel" Click="cmdProcessParrallel_Click"/>

```
```cs
        private void cmdProcessParrallel_Click(object sender, RoutedEventArgs e)
        {
            Title = $"Starting...";
            var watch = Stopwatch.StartNew();
            ProcessFilesParallel();
            watch.Stop();
            Title = $"Processing Complete. Time: {watch.ElapsedMilliseconds}";
        }
        private void ProcessFilesParallel()
        {
            var basePath = @"D:\Temp";
            var pictureDirectory = System.IO.Path.Combine(basePath, "Pictures");
            var outputDirectory = System.IO.Path.Combine(basePath, "ModifiedPictures");

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
                string filename = System.IO.Path.GetFileName(currentFile);
                //This code statement is now a problem
                //Title = $"Processing {filename} on thread: {Environment.CurrentManagedThreadId}";
                Debug.WriteLine($"Processing on thread: {Environment.CurrentManagedThreadId} file: {filename} ");

                using Bitmap bitmap = new(currentFile);
                bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                bitmap.Save(System.IO.Path.Combine(outputDirectory, filename));
            });
        }
```
Якшо запустити додаток в режимі налагодження в вікні Debug можна побачити.
```
Processing on thread: 10 file: 9. Пустыня.jpg 
Processing on thread: 5 file: 20. Карандаши.jpg 
Processing on thread: 8 file: 15. Облака.jpg 
Processing on thread: 9 file: 3. Леопард.jpg 
Processing on thread: 1 file: 1. Черный кот с рыбкой.jpg 
Processing on thread: 1 file: 10. Цветы.jpg 
Processing on thread: 9 file: 4. Кот с наушниками.jpg 
Processing on thread: 8 file: 16. Город.jpg 
Processing on thread: 10 file: 12. Вода.jpg 
Processing on thread: 15 file: 14. Листва.jpg 
Processing on thread: 5 file: 21. Горы.jpg 
Processing on thread: 1 file: 11. Закат.jpg 

```
Загальний час виконання процесу в двічи меньший. Зверніть увагу, що ви зараз коментуєте рядок коду, який відображає ідентифікатор потоку, що виконує поточний файл зображення.  Тепер, якщо ви запустите програму, TPL справді розподілить робоче навантаження на кілька потоків з пулу потоків, використовуючи якомога більше процесорів. Однак, оскільки заголовок завжди оновлюється з основного потоку, код оновлення заголовка більше не відображає поточний потік. Ви не зможете ввести текст у текстове поле, доки всі зображення не будуть оброблені! Причина полягає в тому, що основний потік інтерфейсу користувача все ще заблокований, очікуючи, поки всі інші потоки завершать свою роботу. Сам метод Parallel.ForEach виконує операції паралельно, але він викликається сінхронно і основний потік просто чекає його виконання і не йде далі. Таким чином блокується інтерфейс користувача.  

## Використання класу Task

Клас Task дозволяє асінхронно викликати метод у вторинному потоці та може використовуватися як проста альтернатива використанню асинхронних делегатів.

Додамо кнопку і обробник

```xml
            <Button x:Name="cmdProcessWithTaskFactory" Grid.Column="1" Content="Process with TaskFactory" HorizontalAlignment="Left" Margin="302,0,0,0" VerticalAlignment="Center" Width="144" 
                    Click="ProcessWithTaskFactory_Click"/>
```

```cs
        private void ProcessWithTaskFactory_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(ProcessFilesWithForEachAndTask);
            //Or
            //Task.Factory.StartNew(() => ProcessFilesWithForEachAndTask());
        }


        private void ProcessFilesWithForEachAndTask()
        {
            var basePath = @"D:\Temp";
            var pictureDirectory = System.IO.Path.Combine(basePath, "Pictures");
            var outputDirectory = System.IO.Path.Combine(basePath, "ModifiedPictures");

            //Recreate directory 
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
            }
            Directory.CreateDirectory(outputDirectory);

            //Process
            string[] files = Directory.GetFiles(pictureDirectory, "*.jpg", SearchOption.AllDirectories);
            Dispatcher?.Invoke(() => { Title = "Starting..."; });
            var watch = Stopwatch.StartNew();

            Parallel.ForEach(files, currentFile =>
            {
                string filename = System.IO.Path.GetFileName(currentFile);

                //For title
                int threadId = Environment.CurrentManagedThreadId;
                Dispatcher?.Invoke(() =>
                {
                    Title = $"Processing. Thread:{threadId}   File:{filename}";
                });

                using Bitmap bitmap = new(currentFile);
                bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                bitmap.Save(System.IO.Path.Combine(outputDirectory, filename));
            });

            //For title
            watch.Stop();
            Dispatcher?.Invoke(() => { Title = $"Processing Complete. Time: {watch.ElapsedMilliseconds}"; });
        }
```
Властивість Factory класу Task повертає об'єкт TaskFactory. Коли ви викликаєте його метод StartNew(), ви передаєте делегат Action<T> (тут прихований за допомогою відповідного лямбда-виразу), який вказує на метод, який потрібно викликати асинхронно. З цим оновленням ви побачите, що заголовок вікна показуватиме, який потік з пулу потоків обробляє певний файл, а ще краще те, що текстова область зможе отримувати вхідні дані, оскільки потік інтерфейсу користувача більше не блокується. Об'єкт Dispatcher є екземпляром батьківського класу WPF. Він дозволяє визвати метод що оновить інтерфейс користувача. 
Таким чином основний потік виконується без зупинок і асінхронно в вторинному виконується обробка файлів.
Таким чином важливо зрозуміти шо завдання викликається в окремомому потоці асінхронно і це не блокує основний потік. В данному випадку завдання виконуєтьс в фоновому режимі і влаштовує.

## Обробка запиту на скасування

Одне з удосконалень, яке ви можете внести до поточного прикладу, — це надати користувачеві спосіб зупинити обробку даних зображення за допомогою кнопки «Cancel». Методи Parallel.For() та Parallel.ForEach() підтримують скасування за допомогою токенів скасування. Коли ви викликаєте методи в Parallel, ви можете передати об'єкт ParallelOptions, який, у свою чергу, містить об'єкт CancellationTokenSource.
Спочатку визначте таку нову приватну змінну-член у вашому класі, похідному від Form, типу CancellationTokenSource з назвою _cancelToken:

```cs
    public partial class MainWindow : Window
    {
        private CancellationTokenSource _cancellationTokenSource;
    ...
    } 
```
Оновіть подію Click кнопки Cancel наступним кодом:

```cs
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            // This will be used to tell all the worker threads to stop!
            _cancellationTokenSource.Cancel(); 
        }
```

Додамо кнопку і обробники натисканя для доданої з можливістю сказування.

```xml
            <Button x:Name="cmdProcessWithCancellation" Grid.Column="1" Content="Process with Cancellation" HorizontalAlignment="Left" Margin="137,0,0,0" VerticalAlignment="Center" Width="142" 
                Click="ProcessWithCancellation_Click"/>
```

```cs
        private void ProcessWithCancellation_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(ProcessFilesWithCancellation);
        }

        private void ProcessFilesWithCancellation()
        {
            _cancellationTokenSource = new();

            var basePath = @"D:\Temp";
            var pictureDirectory = System.IO.Path.Combine(basePath, "Pictures");
            var outputDirectory = System.IO.Path.Combine(basePath, "ModifiedPictures");

            //Recreate directory 
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
            }
            Directory.CreateDirectory(outputDirectory);

            // Use ParallelOptions instance to store the CancellationToken.
            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.CancellationToken = _cancellationTokenSource.Token;
            parallelOptions.MaxDegreeOfParallelism = Environment.ProcessorCount;

            //Process
            string[] files = Directory.GetFiles(pictureDirectory, "*.jpg", SearchOption.AllDirectories);
            Dispatcher?.Invoke(() => { Title = "Starting..."; });
            var watch = Stopwatch.StartNew();

            try
            {
                Parallel.ForEach(files, parallelOptions, currentFile =>
                {
                    parallelOptions.CancellationToken.ThrowIfCancellationRequested();

                    string filename = System.IO.Path.GetFileName(currentFile);
                    int threadId = Environment.CurrentManagedThreadId;

                    Dispatcher?.Invoke(() =>
                    {
                        Title = $"Processing. Thread:{threadId}   File:{filename}";
                    });

                    using Bitmap bitmap = new(currentFile);
                    bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    bitmap.Save(System.IO.Path.Combine(outputDirectory, filename));
                });
                //For title
                watch.Stop();
                Dispatcher?.Invoke(() => { Title = $"Processing Complete. Time: {watch.ElapsedMilliseconds}"; });

            }
            catch (OperationCanceledException ex)
            {
                Dispatcher?.Invoke(() => { Title = $"Process canceled! {ex.Message}"; });
            }
        }

```
Зверніть увагу, що ви робите налаштування об'єкта ParallelOptions, встановлюючи властивість CancellationToken для використання токена CancellationTokenSource. Також зверніть увагу, що під час виклику методу Parallel.ForEach() ви передаєте об'єкт ParallelOptions як другий параметр. У рамках логіки циклу ви викликаєте ThrowIfCancellationRequested() для токена, що гарантує, що якщо користувач натисне кнопку «Cancel», усі потоки зупиняться, і ви отримаєте сповіщення через виняток середовища виконання. Коли ви виявляєте помилку OperationCanceledException, ви встановлюєте текст головного вікна в повідомлення про помилку.

## Паралелізм завдань за допомогою класу Parallel

Окрім паралелізму даних, TPL також можна використовувати для легкого запуску будь-якої кількості асинхронних завдань за допомогою методу Parallel.Invoke(). Цей підхід трохи простіший, ніж використання членів з System.Threading; однак, якщо вам потрібен більший контроль над способом виконання завдань, ви можете відмовитися від використання Parallel.Invoke() та використовувати клас Task безпосередньо, як ви зробили в попередньому прикладі.
Щоб проілюструвати паралелізм завдань, створіть нову консольну програму під назвою MyEBookReader. Додайте оператори using в Program.cs:

```cs
using System.Net;
using System.Text;
```
Тут ви отримаєте загальнодоступну електронну книгу з проекту Gutenberg (www.gutenberg.org), а потім паралельно виконайте низку тривалих завдань.
Книга завантажується за допомогою методу GetBook(), як показано тут:

```cs
string _theEBook = "";
GetBook();
Console.WriteLine("Downloading book...");


void GetBook()
{
    //NOTE: The WebClient is obsolete. 
    //We will revisit this example using HttpClient when we discuss async/await
    using WebClient wc = new WebClient();
    wc.DownloadStringCompleted += (s, eArgs) =>
    {
        _theEBook = eArgs.Result;
        Console.WriteLine("Download complete.");
        GetStats();
    };

    // The Project Gutenberg EBook of A Tale of Two Cities, by Charles Dickens
    // You might have to run it twice if you’ve never visited the site before, since the first
    // time you visit there is a message box that pops up, and breaks this code.
    wc.DownloadStringAsync(new Uri("http://www.gutenberg.org/files/98/98-0.txt"));
}
```
Клас WebClient є членом System.Net. Цей клас надає методи для надсилання даних до ресурсу та отримання даних з нього, ідентифікованого за допомогою URI. Багато з цих методів мають асинхронну версію, таку як DownloadStringAsync(). Цей метод автоматично запустить новий потік з пулу потоків .NET Core Runtime. Коли  WebClient завершить отримання даних, він викличе подію DownloadStringCompleted, яку ви обробляєте тут за допомогою лямбда-виразу C#. Якщо викликати синхронну версію цього методу (DownloadString()), повідомлення «Downloading book...» не відображатиметься, доки завантаження не завершиться.

    WebClient застарів і був замінений на HttpClient. Ми повернемося до цього прикладу, використовуючи клас HttpClient, у розділі «Асинхронні виклики з Async/Await» цього розділу.

Далі реалізовано метод GetStats() для вилучення окремих слів, що містяться у змінній theEBook, а потім передачі масиву рядків кільком допоміжним функціям для обробки наступним чином:

```cs
void GetStats()
{
    // Get the words from the ebook.
    string[] words = _theEBook.Split(new char[]
      { ' ', '\u000A', ',', '.', ';', ':', '-', '?', '/' },
      StringSplitOptions.RemoveEmptyEntries);
    // Now, find the ten most common words.
    string[] tenMostCommon = FindTenMostCommon(words);
    // Get the longest word.
    string longestWord = FindLongestWord(words);
    
    // Now that all tasks are complete, build a string to show all stats.
    StringBuilder bookStats = new StringBuilder("Ten Most Common Words are:\n");
    foreach (string s in tenMostCommon)
    {
        bookStats.AppendLine(s);
    }
    bookStats.AppendFormat($"Longest word is: {longestWord}");
    bookStats.AppendLine();
    Console.WriteLine(bookStats.ToString(), "Book info");
}
```
Метод FindTenMostCommon() використовує запит LINQ для отримання списку рядкових об'єктів, які найчастіше зустрічаються в масиві рядків, тоді як FindLongestWord() знаходить, власне, найдовше слово. Ці методи запуспаються послідовно і виконуються один за одним.

```cs
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
```
Якщо ви запустите цей проект, виконання всіх завдань може зайняти значний час, залежно від кількості процесорів вашого комп'ютера та загальної швидкості процесора. Зрештою, ви повинні побачити результат, показаний тут:

```
Downloading book...
Download complete.
Method FindTenMostCommon in Thread:10
Method FindLongestWord in Thread:10
Ten Most Common Words are:
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
Ви можете забезпечити використання застосунком усіх доступних процесорів, паралельно викликаючи методи FindTenMostCommon() та FindLongestWord(). Для цього змініть метод GetStats() наступним чином:

```cs
void GetStatsWithParalell()
{
    string[] words = _theEBook.Split(new char[] { ' ', '\u000A', ',', '.', ';', ':', '-', '?', '/' }, StringSplitOptions.RemoveEmptyEntries);
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
    Console.WriteLine("Work done.");
}
```
Метод Parallel.Invoke() очікує масив параметрів делегатів Action<>, які ви надали опосередковано за допомогою лямбда-виразів. Знову ж таки, хоча вихід ідентичний, перевага полягає в тому, що TPL тепер використовуватиме всі можливі процесори на машині для паралельного виклику кожного методу, якщо це можливо.

```
Downloading book...
Download complete.
Method FindLongestWord in Thread:6
Method FindTenMostCommon in Thread:8
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

# Паралельні запити LINQ (PLINQ)

TPL пропонує ще один спосіб інтеграції паралельних завдань у ваші програми .NET Core. Ви можете використовувати набір методів розширення, які дозволяють створювати запити LINQ, що виконуватимуть своє робоче навантаження паралельно (якщо це можливо). Відповідно, запити LINQ, розроблені для паралельного виконання, називаються запитами PLINQ.
Як і паралельний код, створений за допомогою класу Parallel, PLINQ має можливість ігнорувати ваш запит на паралельну обробку колекції, якщо це необхідно. Фреймворк PLINQ був оптимізований багатьма способами, включаючи визначення того, чи справді запит виконуватиметься швидше синхронно.
Під час виконання PLINQ аналізує загальну структуру запиту, і якщо запит ймовірно виграє від паралелізації, він виконуватиметься паралельно. Однак, якщо паралелізація запиту негативно впливає на продуктивність, PLINQ просто виконує запит послідовно. Якщо PLINQ має вибір між потенційно дорогим паралельним алгоритмом або недорогим послідовним алгоритмом, він за замовчуванням вибирає послідовний алгоритм.
Необхідні методи розширення знаходяться в класі ParallelEnumerable простору імен System.Linq.

Вибрані члени класу ParallelEnumerable

|Член|Значення|
|----|--------|
|AsParallel()|Вказує, що решту запиту слід розпаралелити, якщо це можливо|
|WithCancellation()|Вказує, що PLINQ має періодично контролювати стан наданого токена скасування та скасовувати виконання, якщо це запитується.|
|WithDegreeOfParallelism()|Визначає максимальну кількість процесорів, які PLINQ має використовувати для паралелізації запиту|
|ForAll()|Дозволяє обробляти результати паралельно без попереднього злиття назад із потоком споживача, як це було б у випадку перерахування результату LINQ за допомогою ключового слова foreach|

Щоб побачити PLINQ у дії, створіть консольний застосунок з назвою PLINQDataProcessing. Коли розпочнеться обробка, програма запустить нове завдання (Task), яке виконає запит LINQ, що досліджує великий масив цілих чисел, шукаючи лише ті елементи, де x % 3 == 0 є істинним. Ось непаралельна версія запиту:

```cs

void UsingPLINQ()
{
    Console.WriteLine("Processing");
    Task.Factory.StartNew(ProcessingIntData);
    Console.ReadLine();


    void ProcessingIntData()
    {
        // Get a very large array of integers.
        int[] ints = Enumerable.Range(0, 50_000_000).ToArray();
        // Find the numbers where num % 3 == 0 is true, returned
        // in descending order.

        var query = from number in ints
                    where number % 3 == 0
                    orderby number descending
                    select number;

        var watch = Stopwatch.StartNew();
        int[] modThreeIsZero = query.ToArray();
        watch.Stop();

        Console.WriteLine($"Time:{watch.ElapsedMilliseconds}");
        Console.WriteLine($"Found {modThreeIsZero.Count()} numbers that match query!");
    }

}
UsingPLINQ();
```
```
Processing
Time:4203
Found 16666667 numbers that match query!
```

## Підключення до запиту PLINQ

Якщо ви хочете повідомити TPL про необхідність паралельного виконання цього запиту (якщо можливо), вам потрібно буде використовувати метод розширення AsParallel() наступним чином:

```cs
void ProcessingIntDataWithPLINQ()
{

    //...

    var query = from number in ints.AsParallel()
                where number % 3 == 0
                orderby number descending
                select number;

    //...

}
```
```
Processing
With Paralell
Time:3195
Found 16666667 numbers that match query!
```
Зверніть увагу, що загальний формат запиту LINQ ідентичний тому, що ви бачили в попередніх розділах. Однак, якщо включити виклик AsParallel(), TPL спробує передати робоче навантаження будь-якому доступному процесору.

## Скасування запиту PLINQ

Також можна використовувати об'єкт CancellationTokenSource, щоб повідомити запит PLINQ про необхідність зупинити обробку за певних умов (зазвичай через втручання користувача). Оголосіть об'єкт CancellationTokenSource на рівні класу з назвою _cancelToken та оновіть метод операторів верхнього рівня, щоб він приймав вхідні дані від користувача. Ось відповідне оновлення коду:

```cs
void UsingPLINQWithCancellation()
{
    CancellationTokenSource _cancelToken = new CancellationTokenSource();
  
    do
    {
        Console.WriteLine("Start any key to start processing");
        Console.ReadKey();
        Console.WriteLine("Processing");
        Task.Factory.StartNew(ProcessingIntDataWithPLINQAndCancellation);
        Console.Write("Enter Q to quit: ");
        string? answer = Console.ReadLine();
        // Does user want to quit?
        if (answer != null && answer.Equals("Q",StringComparison.OrdinalIgnoreCase))
        {
            _cancelToken.Cancel();
            break;
        }
    }
    while (true);
    Console.ReadLine();

    void ProcessingIntDataWithPLINQAndCancellation()
    {

        int[] ints = Enumerable.Range(0, 50_000_000).ToArray();

        var query = from number in ints.AsParallel().WithCancellation(_cancelToken.Token)
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
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
UsingPLINQWithCancellation();
```
```
Start any key to start processing
Processing
Enter Q to quit:
With Paralell
Time:2154
        Amount:16666667

Start any key to start processing
Processing
Enter Q to quit:
With Paralell
Time:1770
        Amount:16666667

Start any key to start processing
Processing
Enter Q to quit: q
The query has been canceled via the token supplied to WithCancellation.
```
Тут запит PLINQ повідомлено про те, що він повинен стежити за вхідним запитом на скасування, шляхом об'єднання методу розширення WithCancellation() та передачі токена. Крім того, вам потрібно буде обгорнути цей запит PLINQ у відповідну область try/catch та обробити можливий виняток.

Під час виконання цього процесу потрібно швидко натиснути Q та Enter, щоб побачити повідомлення від токена скасування. На моїй машині розробки у мене було менше секунди, щоб завершити роботу, перш ніж вона завершилася самостійно. Ви можете додати виклик Thread.Sleep(), щоб спростити скасування процесу:

```cs
        try
        {
            var watch = Stopwatch.StartNew();
            Thread.Sleep(1000);
            int[] modThreeIsZero = query.ToArray();
            watch.Stop();

            Console.WriteLine($"\nWith Paralell\nTime:{watch.ElapsedMilliseconds}");
            Console.WriteLine($"\tAmount:{modThreeIsZero.Count()}");
        }
```

Закінчуючи розділ слід зрозумітинаступне. За допомогою Task можно виконати завдання в вторинному фоновому потоці. Завдання можуть мати внутрішні і мати завдання на продовження. Масив завдань асінхронно виконується бистріше аніж свнхрона послідовність визовів. Завдання можуть повертати результат але це блокує основний потік. Класс Parallel можна використати щоб паралельно виконувати одиниці роботи. Цей клас дозволяє разпаралелити обробку колекцій даних. До колекцій можна використовувати методи розширення які будуть працюівати параллельно.   