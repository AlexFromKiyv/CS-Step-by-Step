# Task Parallel Library

Крім використання простору імен System.Threading для стоврення багатопотокових програм існує інший підхід, використовуючи бібліотеку паралельного програмування під назвою Task Parallel Library (TPL). Використовуючи типи  з System.Threading.Tasks, можна створювати детальний, маштабований паралельний код без необхідності працювати безпосередньо з потоками.
Однак це не означає, що ви не використовуватимете типи System.Threading, коли використовуєте TPL. Обидва набори інструментів розробки можуть працювати разом цілком природно. Це особливо вірно в тому, що простір імен System.Threading все ще надає більшість основних можливостей синхронізації, які ви досліджували раніше (Monitor, Interlocked тощо).Однак ви, швидше за все, виявите, що віддасте перевагу роботі з TPL, а не з оригінальним простором імен System.Threading, враховуючи, що той самий набір завдань можна виконувати більш простим способом.

## Простір імен System.Threading.Tasks

У сукупності типи System.Threading.Tasks називають бібліотекою паралельних завдань. TPL автоматично динамічно розподілятиме робоче навантаження вашої програми між доступними ЦП, використовуючи пул потоків виконання. TPL обробляє розподіл роботи, планування потоків, керування станом та інші деталі низького рівня. Результатом є те, що ви можете максимізувати продуктивність своїх програм .NET Core, захищаючись від багатьох складнощів безпосередньої роботи з потоками.

## Роль класу Parallel.

Ключовим класом TPL є System.Threading.Tasks.Parallel. Цей клас містить методи які дозволяють перебирати колекцію даних (зокрема, об'єкт що реалізовує IEnumerable<T>) в паралельний спосіб. Це робиться за допомогою двох основних статичних методів, Parallel.For() та Parallel.ForEach(), кожен з яких визначає численні презавантажені версії. 
Ці методи дозволяють створювати тіло набору операторів коду,які оброблятимуться в паралельним способом. За концепцією, ці оператори мають таку саму логіку яку б ви написали в звичайній конструкції переблору(for , foreach). Корсить в тому що клас Parallel витягуватиме потокі з пулу потоків(і куруватимеме паралелізмом) від вашого імені.
Обидва методи вимагають вказати IEnumerable або  IEnumerable<T> сумісний контейнер, якій містить дані, які потрібно обробити в параллельній манері. Це може бути простий масив, неузагальнена колекція(наприклад ArrayList), узагальнена коллекція(наприклад List<T>) або результат запиту LINQ.
Крім того, вам потрібно буде використовувати делегати System.Func<T> та System.Action<T> щоб указати цільовий метод, який буде викликано для обробки даних. 
Згадайте, що Func<T> представляє метод, який може мати задане повертане значення та різну кількість аргументів. Делегат Action<T> схожий на Func<T>, оскільки він дозволяє вказувати на метод, який приймає певну кількість параметрів. Однак Action<T> визначає метод, який може повертати лише void. Хоча ви можете викликати методи Parallel.For() і Parallel.ForEach() і передати строго типізований об’єкт делегату Func<T> або Action<T>, ви можете спростити своє програмування, використовуючи відповідний анонімний метод C# або лямбда-вираз.

## Паралелізм даних із класом Paralell

Перший спосіб використати TPL - виконати паралелізм даних. Простіше кажучи, цей термін відноситься до завдання прохождення елементв масиву або колекції паралельним способом за допомогою методу Parallel.For() або Parallel.ForEach(). Припустімо, що потрібно виконати деякі трудомісткі операції введення-виведення файлів. Зокрема, вам потрібно завантажити велику кількість файлів *.jpg у пам’ять, перевернути їх догори дном і зберегти змінені дані зображення в новому місці.
У цьому прикладі ви побачите, як виконати те саме загальне завдання за допомогою графічного інтерфейсу користувача, тож ви зможете вивчити використання «анонімних делегатів», щоб дозволити вторинним потокам оновлювати основний потік інтерфейсу користувача (він же потік інтерфейсу користувача). 
Коли ви створюєте програму з багатопоточним графічним інтерфейсом користувача (GUI), вторинні потоки ніколи не зможуть отримати прямий доступ до елементів керування інтерфейсу користувача. Причина в тому, що елементи керування (кнопки, текстові поля, мітки, індикатори виконання тощо) мають спорідненість потоку з потоком, який їх створив.
У наступному прикладі проілюстровано один із способів надання вторинним потокам доступу до елементів інтерфейсу користувача потокобезпечним способом (хоча простіше зробити за допомогою async і await).

Створемо проект типу WPF Application з назвою DataParallelismWithForEach і назвою рішення StudyTPL.

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
                Debug.WriteLine($"Processing {filename} on thread: {Environment.CurrentManagedThreadId}");

                using Bitmap bitmap = new(currentFile);
                bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                bitmap.Save(Path.Combine(outputDirectory, filename));
            }
        }
```
Зверніть увагу, що метод ProcessFilesWithoutParalell() обертатиме кожен файл *.jpg у вказаному каталозі. Наразі вся робота відбувається над основним потоком виконуваного файлу. При натискані кнопки починається процес обробки і видно що все відбувається в основному потоці програми з ID 1. При виконані процесу ми не можемо змінити дані TextBox покі всі зображення не будуть оброблені. Таким чином робота програми "виснить". 

Щоб обробити файли на якомога більшій кількості ядер процесора, ви можете переписати поточний цикл foreach на Parallel.ForEach(). 
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
В якості аргументів методу ForEach виступає масив рядків який є IEnumerable, та лямда вираз який теж саме що і об'єкт делегату Action<string>. Якшо програму запустити а режимі налагодженя то в віконці Debuger можна побачити, що оброба зображень ведеться в різних потоках. Також можна побачити шо час обробкі меньший попередьного варіанта це виводиться в заголовку програми.(можна зрівняти кількість операторів поставиши коментар на одній строчці).

### Доступ до елементів інтерфейсу користувача у вторинних потоках.

Як ви бачете в варіанті коли використовується ForEach закоментовано сторка якв зміню заголовок головного вікна. 
```cs
    //This code statement is now a problem 
    //Title = $"Processing {filename} on thread: {Environment.CurrentManagedThreadId}";
```
Eлементи керування GUI мають «спорідненість потоку» з потоком, який їх створив. Якщо вторинні потоки намагаються отримати доступ до елемента керування, який вони безпосередньо не створювали, це спричинить виняткову ситуацію під час виконання. В нашому випадку якшо не закоментувати строку програма може закінчити свою роботу "аварійно". 

## Клас Task.

Клас Task дозволяє легко викликати метод у вторинному потоці та може використовуватися як проста альтернатива використанню асинхронних делегатів.

Додамо кнопку і обробник

```
            <Button x:Name="cmdProcessWithTaskFactory" Grid.Column="1" Content="Process with TaskFactory" HorizontalAlignment="Left" Margin="564,0,0,0" VerticalAlignment="Center" Width="144" 
                    Click="ProcessWithTaskFactory_Click"/>
```
```cs

        private void ProcessWithTaskFactory_Click(object sender, RoutedEventArgs e)
        {
            Title = $"Starting...";
            var watch = Stopwatch.StartNew();
            Task.Factory.StartNew(ProcessFilesWithForEachAndTask);
            //Or
            //Task.Factory.StartNew(() => ProcessFilesWithForEachAndTask());
            watch.Stop();
            Title = $"Processing complite. Time: {watch.ElapsedMilliseconds}";
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
        Debug.WriteLine($"Processing {filename} on thread: {Environment.CurrentManagedThreadId}");

        using Bitmap bitmap = new(currentFile);
        bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
        bitmap.Save(Path.Combine(outputDirectory, filename));
    });
    Debug.WriteLine("Process complete.");
}
```

Клас Task репрезентує асінхроні операції. Його властивість Factory повертає об'єкт TaskFactory, я кий дозволяє створювати і кофігурувати об'єкти Task або Task<TResult>.
Коли визивається метод StartNew пердається об'єкт делегату Action<T> або відповідний лямбда-вираз, який вказує на метод який треба виконати в асінхроній манері.
Якшо запустити програму в режимі налагодження (debbuging) після натисканя кнопки можна побачити як оброблюються зображеня (в віконці Debug) той самий час вводити дані в текстове поле.  