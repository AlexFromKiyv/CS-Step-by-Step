# Дослідження System.IO

Щоб переконатися, що ви можете запустити кожен із прикладів у цьому розділі, запустіть Visual Studio з правами адміністратора (просто клацніть правою кнопкою миші на значку Visual Studio та виберіть «Запуск від імені адміністратора»).

Простір імен System.IO присвячений службам введення та виведення (I/O) на основі файлів та пам’яті. Як і будь-який простір імен, System.IO визначає набір класів, інтерфейсів, перерахувань, структур і делегатів, більшість з яких можна знайти в mscorlib.dll. На додаток до типів, що містяться в mscorlib.dll, збірка System.dll визначає додаткові члени простору імен System.IO.
Багато типів у просторі імен System.IO зосереджені на програмному маніпулюванні фізичними каталогами та файлами. Однак додаткові типи забезпечують підтримку читання даних і запису даних у рядкові буфери, а також необроблені місця пам’яті. 

Основні класи System.IO

|Клас|Використання|
|----|------------|
|BinaryReader, BinaryWriter|Ці класи дозволяють зберігати та отримувати примітивні типи даних (цілі числа, логічні значення, рядки тощо) як двійкові значення.|
|BufferedStream|Цей клас надає тимчасове сховище для потоку байтів, які ви можете передати в сховище пізніше.|
|Directory, DirectoryInfo|Класи для керування структурою каталогів машини. Тип Directory надає функціональні можливості за допомогою статичних членів, тоді як тип DirectoryInfo надає подібні функції з дійсного посилання на об’єкт.|
|DriveInfo|Цей клас надає детальну інформацію про диски, які використовує дана машина.|
|File, FileInfo|Класи для маніпулювання набором файлів машини. Тип File надає функціональні можливості за допомогою статичних членів, тоді як тип FileInfo надає подібні функціональні можливості з дійсного посилання на об’єкт.|
|FileStream|Цей клас надає довільний доступ до файлів (наприклад, можливість пошуку) з даними, представленими у вигляді потоку байтів.|
|FileSystemWatcher|Цей клас дозволяє відстежувати модифікацію зовнішніх файлів у вказаному каталозі.|
|MemoryStream|Цей клас забезпечує довільний доступ до потокових даних, що зберігаються в пам’яті, а не у фізичному файлі.|
|Path|Цей клас виконує операції з типами System.String, які містять інформацію про шлях до файлу або каталогу, незалежно від платформи.|
|StreamWriter, StreamReader|Класи для зберігання і отримання текстової інформації у файл або з файлу. Ці типи не підтримують довільний доступ до файлів.|
|StringWriter, StringReader|Як і класи StreamReader/StreamWriter, ці класи також працюють з текстовою інформацією. Однак основним сховищем є рядковий буфер, а не фізичний файл.|

На додаток до цих конкретних типів класів, System.IO визначає кілька перерахувань, а також набір абстрактних класів (наприклад, Stream, TextReader і TextWriter), які визначають спільний поліморфний інтерфейс для всіх нащадків.

System.IO надає чотири класи, які дозволяють вам маніпулювати окремими файлами, а також взаємодіяти зі структурою каталогів машини. Directory та File, надають доступ до операцій створення, видалення, копіювання та переміщення за допомогою різних статичних елементів. Тісно пов’язані типи FileInfo та DirectoryInfo надають подібну функціональність, як і методи рівня екземпляра (отже, ви повинні створити їх за допомогою ключового слова new). Класи Directory і File безпосередньо розширюють System.Object, тоді як DirectoryInfo і FileInfo походять від абстрактного типу FileSystemInfo. 
FileInfo та DirectoryInfo зазвичай служать кращим вибором для отримання повної інформації про файл або каталог (наприклад, час створення або можливості читання/запису), оскільки їхні члени мають тенденцію повертати строго типізовані об’єкти. Навпаки, члени класів Directory і File мають тенденцію повертати прості рядкові значення, а не строго типізовані об’єкти. Однак це лише орієнтир; у багатьох випадках ви можете виконати ту саму роботу за допомогою File/FileInfo або Directory/DirectoryInfo.

## Абстрактний базовий клас FileSystemInfo.

Типи DirectoryInfo та FileInfo отримують багато поведінки від абстрактного базового класу FileSystemInfo. Здебільшого ви використовуєте члени класу FileSystemInfo для виявлення загальних характеристик (таких як час створення, різні атрибути тощо) певного файлу чи каталогу. 

Основні властивості класу FileSystemInfo.

|Властивість|Використання|
|-----------|------------|
|Attributes|Отримує або встановлює атрибути, пов’язані з поточним файлом, які представлені переліком FileAttributes (наприклад, чи файл або каталог доступні лише для читання, зашифровані, приховані чи стиснуті?)|
|CreationTime|Отримує або встановлює час створення для поточного файлу або каталогу|
|Exists|Визначає, чи існує даний файл або каталог|
|Extension|Отримує розширення файлу|
|FullName|Отримує повний шлях до каталогу або файлу|
|LastAccessTime|Отримує або встановлює час останнього доступу до поточного файлу чи каталогу|
|LastWriteTime|Отримує або встановлює час останнього запису в поточний файл або каталог|
|Name|Отримує назву поточного файлу або каталогу|

FileSystemInfo також визначає метод Delete(). Це реалізовано похідними типами для видалення певного файлу чи каталогу з жорсткого диска. Крім того, ви можете викликати Refresh() перед отриманням інформації про атрибути, щоб переконатися, що статистика щодо поточного файлу (або каталогу) не застаріла.

# Клас DirectoryInfo, Directory та DriveInfo.

## Робота з DirectoryInfo.

Клас містить набір членів, які використовуються для створення, переміщення, видалення та нумерації каталогів і підкаталогів. Крім функціональних можливостей, які надає базовий клас FileSystemInfo, DirectoryInfo пропонує ключові члени що до каталогів.

Ключові члени DirectoryInfo

|Член|Використання|
|----|------------|
|Create(), CreateSubdirectory()|Створює каталог або набір підкаталогів, коли отримує ім’я шляху|
|Delete()|Видаляє каталог і весь його вміст|
|GetDirectories()|Повертає масив об’єктів DirectoryInfo, які представляють усі підкаталоги в поточному каталозі|
|GetFiles()|Повертає масив об’єктів FileInfo, які представляють набір файли в поточному каталозі|
|MoveTo()|Переміщує каталог і його вміст на новий шлях|
|Parent|Отримує батьківський каталог цього каталогу|
|Root|Отримує кореневу частину шляху|

Викорситаємо ці методи. 

ExploringSystemIO\Program.cs
```cs
void ExploringDirectoryInfo()
{
    DirectoryInfo directoryInfo_1 = new(@"C:\");
    ShowDirectoryInfo(directoryInfo_1);
}
ExploringDirectoryInfo();

void ShowDirectoryInfo(DirectoryInfo directoryInfo)
{
    Console.WriteLine($"DirectoryInfo object: {directoryInfo.Name}\n");
    Console.WriteLine($"\tName : {directoryInfo.Name}");
    Console.WriteLine($"\tCreationTime : {directoryInfo.CreationTime}");
    Console.WriteLine($"\tAttributes : {directoryInfo.Attributes}");
    Console.WriteLine($"\tRoot : {directoryInfo.Root}");
    Console.WriteLine($"\tParent : {directoryInfo.Parent}");
    Console.WriteLine($"\tFullName : {directoryInfo.FullName}");
}
```
```
DirectoryInfo object: C:\

        Name : C:\
        CreationTime : 07.12.2019 11:03:44
        Attributes : Hidden, System, Directory, Archive
        Root : C:\
        Parent :
        FullName : C:\
```
Допоміжна функція показує дані про об'єкт. Вона використовує функціонал успадкований від FileSystemInfo.

Ствримо декілька різних об'єктів DirectoryInfo.

```cs
void ExploringDirectoryInfo()
{
    // ...

    // Bind to the current working directory.
    DirectoryInfo directoryInfo_2 = new(".");
    ShowDirectoryInfo(directoryInfo_2);

    // Bind to C:\Windows,
    // using a verbatim string.
    DirectoryInfo directoryInfo_3 = new(@"C:\Windows");
    ShowDirectoryInfo(directoryInfo_3);

}
ExploringDirectoryInfo();
```
```
DirectoryInfo object: net8.0

        Name : net8.0
        CreationTime : 19.04.2024 16:29:57
        Attributes : Directory
        Root : D:\
        Parent : D:\MyWork\...\bin\Debug
        FullName : D:\MyWork\...\net8.0

DirectoryInfo object: Windows

        Name : Windows
        CreationTime : 07.12.2019 11:03:44
        Attributes : Directory, Archive
        Root : C:\
        Parent : C:\
        FullName : C:\Windows
```

Як правило починаєте працювати з типом DirectoryInfo, вказуючи певний шлях до каталогу як параметр конструктора. Використовуйте крапку (.), якщо хочете отримати доступ до поточного робочого каталогу (каталог програми, що виконується).

```cs
    string currentDyretoryPath = new DirectoryInfo(".").FullName;
    Console.WriteLine(currentDyretoryPath);
```

У другому прикладі припускаєтся, що шлях, переданий у конструктор (C:\Windows), уже існує на фізичній машині. Якщо намагаєтесь взаємодіяти з неіснуючим каталогом, викидається виняткова ситуація System.IO.DirectoryNotFoundException. 

```cs
void ExploringDirectoryInfo()
{
    // ...

    // Bind to a nonexistent directory, then create it.
    DirectoryInfo directoryInfo_4 = new(@"D:\SuperCode\MyProject");
    directoryInfo_4.Create();
    ShowDirectoryInfo(directoryInfo_4);

}
ExploringDirectoryInfo();
```
```
DirectoryInfo object: MyProject

        Name : MyProject
        CreationTime : 19.04.2024 19:49:15
        Attributes : Directory
        Root : D:\
        Parent : D:\SuperCode
        FullName : D:\SuperCode\MyProject
```


Таким чином, якщо вкажете каталог, який ще не створено, вам потрібно викликати метод Create(), перш ніж продовжити.


## Корисні можливості Path.

Синтаксис шляху, використаний у попередньому прикладі, орієнтований на Windows.

```cs
void ExploringDirectoryInfo()
{
    // ...

    char vsc = Path.VolumeSeparatorChar;
    char dsc = Path.DirectorySeparatorChar;
    string path = $@"D{vsc}{dsc}SuperCode{dsc}MyProject";

    DirectoryInfo directoryInfo_5 = new(path);
    ShowDirectoryInfo(directoryInfo_5);
}
ExploringDirectoryInfo();
```
```
DirectoryInfo object: MyProject

        Name : MyProject
        CreationTime : 19.04.2024 19:49:15
        Attributes : Directory
        Root : D:\
        Parent : D:\SuperCode
        FullName : D:\SuperCode\MyProject
```

Якщо ви розробляєте програми .NET для різних платформ, вам слід використовувати конструкції Path.VolumeSeparatorChar і Path.DirectorySeparatorChar, які отримають відповідні символи на основі платформи.


## Прохід по файлах з використанням DirectoryInfo

На додаток до отримання основних деталей існуючого каталогу, можна використовувати деякі методи типу DirectoryInfo.

```cs
void ExploringDirectoryFiles(string directoryString)
{
    DirectoryInfo directoryInfo = new(directoryString);
    ShowInfoFilesJpg(directoryInfo);
}
ExploringDirectoryFiles(@"C:\Windows\Web\Wallpaper");

void ShowInfoFilesJpg(DirectoryInfo directoryInfo)
{
    FileInfo[] fileInfos = directoryInfo.
        GetFiles("*.jpg", SearchOption.AllDirectories);

    Console.WriteLine($"Files (*.jpg) in {directoryInfo.FullName}\n");
    Console.WriteLine($"Found {fileInfos.Length} files");

    foreach (FileInfo fileInfo in fileInfos)
    {
        ShowFileInfo(fileInfo);
    }
}
void ShowFileInfo(FileInfo fileInfo)
{
    Console.WriteLine($"\nData about file: {fileInfo.Name}");
    Console.WriteLine($"\tName:{fileInfo.Name} ");
    Console.WriteLine($"\tSize:{fileInfo.Length} ");
    Console.WriteLine($"\tAttributes:{fileInfo.Attributes} ");
    Console.WriteLine($"\tCreationTime:{fileInfo.CreationTime} ");
    Console.WriteLine($"\tFull name:{fileInfo.FullName} ");
}
```
```
Files (*.jpg) in C:\Windows\Web\Wallpaper

Found 13 files

Data about file: img50.jpg
        Name:img50.jpg
        Size:23615
        Attributes:Archive
        CreationTime:14.03.2024 10:25:41
        Full name:C:\Windows\Web\Wallpaper\Spotlight\img50.jpg

Data about file: img1.jpg
        Name:img1.jpg
        Size:626435
        Attributes:Archive
        CreationTime:07.12.2019 11:09:54
        Full name:C:\Windows\Web\Wallpaper\Theme1\img1.jpg

    ...

Data about file: img0.jpg
        Name:img0.jpg
        Size:393630
        Attributes:Archive
        CreationTime:07.12.2019 11:09:54
        Full name:C:\Windows\Web\Wallpaper\Windows\img0.jpg
```

По-перше, ви можете використати метод GetFiles(), щоб отримати інформацію про всі файли *.jpg, розташовані в каталозі C:\Windows\Web\Wallpaper\Theme2 (чи інший де є такі файли).
Метод GetFiles() повертає масив об’єктів FileInfo, кожен з яких розкриває деталі певного файлу. Зверніть увагу, що ви вказуєте параметр пошуку під час виклику GetFiles(); ви робите це, щоб переглянути всі підкаталоги кореня.

## Створення підкаталогів використовуючи DirectoryInfo.

Можна програмно розширити структуру каталогу за допомогою методу DirectoryInfo.CreateSubdirectory().

```cs
void HowToCreateASubdirectory()
{
    DirectoryInfo directoryInfo_1 = new(@"D:\SuperCode");

    if (!directoryInfo_1.Exists)
    {
        directoryInfo_1.Create();
    }

    ShowDirectoryInfo(directoryInfo_1);

    DirectoryInfo directoryInfo_1_1 = directoryInfo_1.CreateSubdirectory(@"Project");
    ShowDirectoryInfo(directoryInfo_1_1);

    DirectoryInfo directoryInfo_1_2 = directoryInfo_1.CreateSubdirectory(@"Project1\Data");
    ShowDirectoryInfo(directoryInfo_1_2);
}
HowToCreateASubdirectory();

```
```
DirectoryInfo object: SuperCode

        Name : SuperCode
        CreationTime : 19.04.2024 19:49:15
        Attributes : Directory
        Root : D:\
        Parent : D:\
        FullName : D:\SuperCode
DirectoryInfo object: Project

        Name : Project
        CreationTime : 20.04.2024 11:48:21
        Attributes : Directory
        Root : D:\
        Parent : D:\SuperCode
        FullName : D:\SuperCode\Project
DirectoryInfo object: Data

        Name : Data
        CreationTime : 20.04.2024 11:48:21
        Attributes : Directory
        Root : D:\
        Parent : D:\SuperCode\Project1
        FullName : D:\SuperCode\Project1\Data
```
Mетод може створити один підкаталог, а також кілька вкладених підкаталогів за один виклик функції. Не обов'язково присваювати змінній значення, яке повертає метод CreateSubdirectory, але ви повинні знати, що об’єкт DirectoryInfo, який представляє щойно створений елемент, передається назад після успішного виконання.

## Робота з класом Directory.

Клас виконує аналогічну функціональність DirectoryInfo але більшисть його членів статичні і зазвичай працють з рядками.

```cs
void ExploringDirectory()
{
    DirectoryInfo directoryInfo =
        Directory.CreateDirectory(@"D:\SuperCode\Project2");
    ShowDirectoryInfo(directoryInfo);

    DeleteDirectory(@"D:\SuperCode\Project2");

    ShowAllDriveWithDirectory();
}
ExploringDirectory();

void ShowAllDriveWithDirectory()
{
    string[] driveNames = Directory.GetLogicalDrives();
    
    Console.WriteLine("The mashine has drive:");
    foreach (string? drive in driveNames)
    {
        Console.WriteLine("\t"+drive);
    }
}

void DeleteDirectory(string directoryString)
{
    if (!Directory.Exists(directoryString)) 
    { 
        Console.WriteLine($"The directory does not exist: {directoryString}");
        return;
    }

    Console.Write($"Delete directory {directoryString} (Y/N):");
    string? answer = Console.ReadLine();

    if (answer !=null && answer.Equals("Y",StringComparison.OrdinalIgnoreCase)) 
    {
        try
        {
            // The second parameter specifies whether you
            // wish to destroy any subdirectories.
            Directory.Delete(directoryString,true);
            Console.WriteLine("The directory has been deleted");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
```
```
DirectoryInfo object: Project2

        Name : Project2
        CreationTime : 10.05.2024 18:08:48
        Attributes : Directory
        Root : D:\
        Parent : D:\SuperCode
        FullName : D:\SuperCode\Project2
Delete directory D:\SuperCode\Project2 (Y/N):N
The mashine has drive:
        C:\
        D:\
```
Зверніть увагу на те, що члени Directory зазвичай повертають рядкові дані, а не строго типізовані об’єкти FileInfo/DirectoryInfo.

## Робота з класом DriveInfo.

На відміну від методу Directory.GetLogicalDrives, який повертає масив рядків, метод класу DriveInfo надає більше даних про логічні диски.

```cs
void ExploringDriveInfo()
{
    DriveInfo[] driveInfos = DriveInfo.GetDrives();

    foreach (DriveInfo driveInfo in driveInfos)
    {
        ShowDriveInfo(driveInfo);
    }
}
ExploringDriveInfo();

void ShowDriveInfo(DriveInfo driveInfo)
{
    Console.WriteLine($"Drive: {driveInfo.Name}");
    Console.WriteLine($"\tVolumeLabel: {driveInfo.VolumeLabel}");
    Console.WriteLine($"\tTotalSize: {driveInfo.TotalSize}");
    Console.WriteLine($"\tTotalFreeSpace: {driveInfo.TotalFreeSpace}");
    Console.WriteLine($"\tAvailableFreeSpace: {driveInfo.AvailableFreeSpace}");
    Console.WriteLine($"\tDriveFormat: {driveInfo.DriveFormat}");
    Console.WriteLine($"\tDriveType: {driveInfo.DriveType}");
    Console.WriteLine($"\tRootDirectory: {driveInfo.RootDirectory}");
    Console.WriteLine($"\tIsReady: {driveInfo.IsReady}");
}
```
```
Drive: C:\
        VolumeLabel: Windows 10
        TotalSize: 113605767168
        TotalFreeSpace: 44747337728
        AvailableFreeSpace: 44747337728
        DriveFormat: NTFS
        DriveType: Fixed
        RootDirectory: C:\
        IsReady: True
Drive: D:\
        VolumeLabel:
        TotalSize: 13630435328
        TotalFreeSpace: 11511009280
        AvailableFreeSpace: 11511009280
        DriveFormat: NTFS
        DriveType: Fixed
        RootDirectory: D:\
        IsReady: True
```

## Корисні можливості Path та Environment. 

Середовище в якому виконується програма може бути різною.

```cs

void SpecialSettingsOS()
{
    Console.WriteLine("About platform:");
    Console.WriteLine($"Path.PathSeparator: {Path.PathSeparator}");
    Console.WriteLine($"Path.DirectorySeparatorChar: {Path.DirectorySeparatorChar}");
    Console.WriteLine($"Path.GetTempPath(): {Path.GetTempPath()}");

    Console.WriteLine($"Directory.GetCurrentDirectory(): {Directory.GetCurrentDirectory()}");
    Console.WriteLine($"Environment.CurrentDirectory: {Environment.CurrentDirectory}");
    Console.WriteLine($"Environment.SystemDirectory: {Environment.SystemDirectory}");
    Console.WriteLine($"{Environment.GetFolderPath(Environment.SpecialFolder.System)}");
    Console.WriteLine($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}");
    Console.WriteLine($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}");
    Console.WriteLine($"{Environment.GetFolderPath(Environment.SpecialFolder.Personal)}");
}
SpecialSettingsOS();
```
```
About platform:
Path.PathSeparator: ;
Path.DirectorySeparatorChar: \
Path.GetTempPath(): C:\Users\user\AppData\Local\Temp\
Environment.SystemDirectory: C:\Windows\system32
C:\Windows\system32
C:\Users\user\AppData\Roaming
C:\Users\user\Documents
C:\Users\user\Documents
```
Для побудови спеціального шляху для діректорії можна використовувати метод Path.Combine в парі з класом Environment.

```cs
void UsePathAndEnvironment()
{
    string folderPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.Personal),
        "MyFolder");
    Console.WriteLine(folderPath);

    Console.WriteLine(Directory.Exists(folderPath));

    Directory.CreateDirectory(folderPath);

    Console.WriteLine(Directory.Exists(folderPath));

    Directory.Delete(folderPath);

    Console.WriteLine(Directory.Exists(folderPath));
}
UsePathAndEnvironment();
```
```
C:\Users\user\Documents\MyFolder
False
True
False
```


# Класи FileInfo та File.

## Клас FileInfo.

Крім функціональних можливостей, успадкованих від FileSystemInfo, нижче приведені основні члени, унікальні для класу FileInfo.

Основні члени FileInfo

|Член|Вмкористання|
|----|------------|
|AppendText()|Створює об’єкт StreamWriter (описано пізніше), який додає текст до файлу|
|CopyTo()|Копіює наявний файл у новий файл|
|Create()|Створює новий файл і повертає об’єкт FileStream (описано пізніше) для взаємодії з новоствореним файлом|
|CreateText()|Створює об’єкт StreamWriter, який записує новий текстовий файл|
|Delete()|Видаляє файл, до якого прив’язаний екземпляр FileInfo|
|Directory|Отримує екземпляр батьківського каталогу|
|DirectoryName|Отримує повний шлях до батьківського каталогу|
|Length|Отримує розмір поточного файлу|
|MoveTo()|Переміщує вказаний файл у нове розташування, надаючи можливість вказати нове ім’я файлу|
|Name|Отримує назву файлу|
|Open()|Відкриває файл із різними правами на читання/запис і спільний доступ|
|OpenRead()|Створює об’єкт FileStream лише для читання|
|OpenText()|Створює об’єкт StreamReader (описано пізніше), який читає з наявного текстового файлу|
|OpenWrite()|Створює об’єкт FileStream лише для запису|

Зауважте, що більшість методів класу FileInfo повертають певний орієнтований на введення-виведення об’єкт (наприклад, FileStream і StreamWriter), який дозволяє вам почати читання та запис даних до (або читання з) пов’язаного файлу в різних форматах.

## Метод FileInfo.Create.

Для початку створимо допоміжний метод що відповідає за наявність діректорії.

```cs
void CheckOrCreateDirectory(string directoryInfoFullName)
{
    DirectoryInfo directoryInfo = new(directoryInfoFullName);
    if (!directoryInfo.Exists)
    { directoryInfo.Create(); }
}
```
Якшо каталогу нема метод його створить.

Один із способів отримати можливість працювати з файлом створити його за допомогою Create

```cs
void ExploringCreateFileWithFileInfo()
{
    CheckOrCreateDirectory(@"D:\Temp");

    FileInfo fileInfo = new FileInfo(@"D:\Temp\Test.dat");
    FileStream fileStream = fileInfo.Create();

    // Use FileStream

    fileStream.Close();
}
ExploringCreateFileWithFileInfo();
```
Метод FileInfo.Create() повертає об’єкт FileStream, який надає повний доступ для читання/запису всім користувачам. Майте на увазі при роботі методу, в такому варіанті парметрів, якшо такий файл існував його буде перезаписано без завуважень. Після завершення роботи з поточним об’єктом FileStream ви повинні переконатися, що ви закрили дескриптор, щоб звільнити базові некеровані ресурси потоку.

Аналогічно цьому можна щоб компілятор сам турбувався за виклик методу що реалізовує інтерфейс IDisposable. Для цього можна використовувати using.

```cs
void ExploringCreateFileWithFileInfo()
{
    CheckOrCreateDirectory(@"D:\Temp");
  
    FileInfo fileInfo = new FileInfo(@"D:\Temp\Test.dat");

    using FileStream fileStream = fileInfo.Create();

    // Use FileStream
}
ExploringCreateFileWithFileInfo();
```
Тобто такий шаблон є трохі компактнішим. 

## Метод FileInfo.Open.

Ви можете використовувати метод FileInfo.Open(), щоб відкривати наявні файли, а також створювати нові файли з набагато більшою точністю, ніж за допомогою FileInfo.Create().

```cs
void ExploringFileInfoOpen()
{
    CheckOrCreateDirectory(@"D:\Temp");

    FileInfo fileInfo = new FileInfo(@"D:\Temp\Test.dat");

    using FileStream fileStream = fileInfo.Open(FileMode.OpenOrCreate,
        FileAccess.ReadWrite, FileShare.None);
    
    // Use the FileStream object...
}
ExploringFileInfoOpen();
```
Метод Open() зазвичай приймає кілька параметрів, щоб точно визначити, як взаємодіяти з файлом, яким ви хочете маніпулювати. Ця версія перевантаженого методу Open() вимагає трьох параметрів. Перший параметр методу Open() визначає загальний тип запиту введення-виведення (наприклад, створити новий файл, відкрити існуючий файл і додати до файлу), який ви вказуєте за допомогою перерахування FileMode. 

```cs
public enum FileMode
{
  CreateNew,
  Create,
  Open,
  OpenOrCreate,
  Truncate,
  Append
}
```

Варіанти перерахування FileMode

|Варіант|Використання|
|-------|------------|
|CreateNew|Повідомляє ОС створити новий файл. Якщо він уже існує, створюється виняткова ситуація IOException.|
|Create|Повідомляє ОС створити новий файл. Якщо він уже існує, його буде перезаписано.|
|Open|Відкриває наявний файл. Якщо файл не існує, створюється виняткова ситуація FileNotFoundException.|
|OpenOrCreate|Відкриває файл, якщо він існує; інакше буде створено новий файл|
|Truncate|Відкриває наявний файл і скорочує його до 0 байт|
|Append|Відкриває файл, переміщується в кінець файлу та починає операції запису (ви можете використовувати цей прапор лише з потоком лише для запису). Якщо файл не існує, створюється новий файл.|

Другий параметр методу Open(), значення з переліку FileAccess, щоб визначити поведінку читання/запису основного потоку.

```cs
public enum FileAccess
{
  Read,
  Write,
  ReadWrite
}
```
Третій параметр методу Open(), FileShare, визначає спосіб спільного використання файлу серед інших обробників.

```cs
public enum FileShare
{
  None,
  Read,
  Write,
  ReadWrite,
  Delete,
  Inheritable
}
```

## Методи FileInfo.OpenRead , FileInfo.OpenWrite.

Метод FileInfo.Open() дозволяє отримати дескриптор файлу гнучким способом, але клас FileInfo також надає члени з іменами OpenRead() і OpenWrite(). Ці методи повертають належним чином налаштований об’єкт FileStream лише для читання або лише для запису без необхідності надання різних значень перерахування. Подібно до FileInfo.Create() і FileInfo.Open(), OpenRead() і OpenWrite() повертають об’єкт FileStream. Зауважте, що метод OpenRead() вимагає, щоб файл уже існував в тойже час OpenWrite() цього не потребує і якщо файл не існує він його створить.

```cs
void ExploringFileInfoOpenReadOpenWrite()
{
    CheckOrCreateDirectory(@"D:\Temp");

    FileInfo fileInfo = new FileInfo(@"D:\Temp\Test.dat");

    if (!fileInfo.Exists)
    {
        fileInfo.Create().Close();
    }

    FileStream fileStream = fileInfo.OpenRead();

    // Use the FileStream object... for read

    FileInfo fileInfo1 = new FileInfo(@"D:\Temp\Test1.dat");

    FileStream fileStream1 = fileInfo1.OpenWrite();

    // Use the FileStream object... for write

}
ExploringFileInfoOpenReadOpenWrite();
```

## Метод FileInfo.OpenText().

На відміну від Create(), Open(), OpenRead() або OpenWrite(), метод OpenText() повертає екземпляр типу StreamReader, а не типу FileStream.

```cs
void ExploringFileInfoOpenText()
{
    CheckOrCreateDirectory(@"D:\Temp");

    FileInfo fileInfo = new(@"D:\Temp\boot.ini");

    if (!fileInfo.Exists)
    {
        fileInfo.Create().Close();
    }
    using StreamReader streamReader = fileInfo.OpenText();

    //  Use the StreamReader object
}
ExploringFileInfoOpenText();
```
Як ви незабаром побачите, тип StreamReader забезпечує спосіб читання символьних даних із файлу.

## Методи FileInfo.CreateText та FileInfo.AppendText.

Обидва повертають об’єкт StreamWriter.

```cs
void ExploringFileInfoCreateTextAppendText()
{
    CheckOrCreateDirectory(@"D:\Temp");

    FileInfo fileInfo = new(@"D:\Temp\config.ini");

    using StreamWriter streamWriter = fileInfo.CreateText();

    // Use the StreamWriter object

    fileInfo = new(@"D:\Temp\boot.ini");

    using StreamWriter streamWriter1 = fileInfo.AppendText();

    // Use the StreamWriter object
}
ExploringFileInfoCreateTextAppendText();
```
Як ви могли здогадатися, тип StreamWriter забезпечує спосіб запису символьних даних у базовий файл.

## Робота з класом File.

Тип File використовує кілька статичних членів для забезпечення функціональності, майже ідентичної типу FileInfo. Як і FileInfo, File надає методи AppendText, Create, CreateText, Open, OpenRead, OpenWrite і OpenText. У багатьох випадках ви можете використовувати типи File та FileInfo як взаємозамінні. Зауважте, що OpenText і OpenRead вимагають, щоб файл уже існував.

```cs
void ExploringFileCreateAndOpen()
{
    CheckOrCreateDirectory(@"D:\Temp");

    var fileFullName = @"D:\Temp\Test.dat";

    using FileStream fileStream = File.Create(fileFullName);
    //Using FileStream object ...

    //using FileStream fileStream = File.Open(fileFullName, FileMode.OpenOrCreate,
    //    FileAccess.ReadWrite, FileShare.None);
    //// Using FileStream object ...

    //using FileStream fileStream = File.OpenRead(fileFullName);
    //// Using FileStream object with read-only permissions.

    //using FileStream fileStream = File.OpenWrite(fileFullName);
    //// Using FileStream object with write-only permissions.

    //using StreamReader streamReader = File.OpenText(fileFullName);
    //// Using StreamReader object ...

    //using StreamWriter streamWriter = File.CreateText(fileFullName);
    //// Using StreamWriter object ...

    //using StreamWriter streamWriter = File.AppendText(fileFullName);
    //// Using StreamWriter object ...
}
ExploringFileCreateAndOpen();
```
Кожен метод можно протестувати знімаючи відповідний коментар.

## Корисні методи класу File.

Тип File також підтримує кілька членів які можуть значно спростити процеси читання та запису даних.

Корисні методи класу File

|Член|Використання|
|----|------------|
|ReadAllBytes()|Відкриває вказаний файл, повертає двійкові дані у вигляді масиву байтів, а потім закриває файл|
|ReadAllLines()|Відкриває вказаний файл, повертає символьні дані у вигляді масиву рядків, а потім закриває файл|
|ReadAllText()|Відкриває вказаний файл, повертає символьні дані як System.String, а потім закриває файл|
|WriteAllBytes()|Відкриває вказаний файл, записує масив байтів, а потім закриває файл|
|WriteAllLines()|Відкриває вказаний файл, записує масив рядків, а потім закриває файл|
|WriteAllText()|Відкриває вказаний файл, записує символьні дані з указаного рядка, а потім закриває файл|


Ви можете використовувати ці методи типу File для читання та запису пакетів даних лише в кількох рядках коду.

```cs
void ExploringFileWriteAllLines()
{
    string[] myList = { "carrots", "chicken", "grapes", "milk" };

    CheckOrCreateDirectory(@"D:\Temp");

    File.WriteAllLines(@"D:\Temp\FoodList.txt", myList);

    string[] myFood = File.ReadAllLines(@"D:\Temp\FoodList.txt");

    foreach (var product in myFood)
    {
        Console.WriteLine(product);
    }
}
ExploringFileWriteAllLines();
```
```
carrots
chicken
grapes
milk

```

Кожен із цих членів автоматично закриває основний дескриптор файлу. Код зберігає рядкові дані в новому файлі на диску (і зчитує його в пам’ять) з мінімальними зусиллями. Єдине зауваженя, якшо існував файл метод WriteAllLine затирає старі дані без запитів на збереження. Як бачимо це простий шлях роботи з файлом. 

Перевагою створення об’єкта FileInfo є те, що ви можете досліджувати файл за допомогою членів абстрактного базового класу FileSystemInfo. 


# Абстрактний клас Stream.

В прикладах ви бачили багато способів отримати об’єкти FileStream, StreamReader і StreamWriter, але вам ще належить зчитувати дані з або записати дані у файл за допомогою цих типів. Щоб зрозуміти, як це зробити, вам потрібно буде ознайомитися з поняттям Stream (потік, струм).

У світі маніпулювання вводом-виводом потік представляє фрагмент даних, що перетікає між джерелом і одержувачем. Потоки забезпечують загальний спосіб взаємодії з послідовністю байтів, незалежно від того, який тип пристрою (наприклад, файл, мережеве підключення або принтер) зберігає або відображає ці байти. Концепція потоку не обмежується файловим вводом-виводом. Бібліотеки .NET забезпечують потоковий доступ до мереж, місць пам’яті та інших потокоцентричних абстракцій. 

Абстрактний клас System.IO.Stream визначає кілька членів, які забезпечують підтримку синхронної та асинхронної взаємодії з носієм даних (наприклад, основним файлом або місцем пам’яті).
Нащадки потоку представляють дані як необроблений потік байтів; отже, робота безпосередньо з необробленими потоками може бути досить загадковою. Деякі типи, отримані від потоку, підтримують пошук, який відноситься до процесу отримання та коригування поточної позиції в потоці.

Члени класу Stream

|Член|Використання|
|----|------------|
|CanRead, CanWrite, CanSeek|Визначає, чи підтримує поточний потік читання, пошук і/або запис|
|Close()|Закриває поточний потік і звільняє всі ресурси (наприклад, сокети та дескриптори файлів), пов’язані з поточним потоком. Внутрішньо цей метод є псевдонімом методу Dispose(); отже, закриття потоку функціонально еквівалентно видаленню потоку|
|Flush()|Оновлює базове джерело даних або сховище поточним станом буфера, а потім очищає буфер. Якщо потік не реалізує буфер, цей метод нічого не робить|
|Length|Повертає довжину потоку в байтах|
|Position|Визначає позицію в поточному потоці|
|Read(), ReadByte(), ReadAsync()|Зчитує послідовність байтів (або один байт) із поточного потоку та переміщує поточну позицію в потоці на кількість прочитаних байтів|
|Seek()|Встановлює позицію в поточному потоці|
|SetLength()|Встановлює довжину поточного потоку|
|Write(), WriteByte(), WriteAsync()|Записує послідовність байтів (або один байт) у поточний потік і просуває поточну позицію в цьому потоці на кількість записаних байтів|


# Робота з FileStream.

Клас FileStream забезпечує реалізацію для абстрактних членів Stream у спосіб, відповідний для потокової передачі на основі файлів.

Це примітивний потік; він може читати або записувати лише один байт або масив байтів. Однак вам не часто потрібно буде безпосередньо взаємодіяти з членами типу FileStream. Замість цього ви, ймовірно, будете використовувати різні обгортки потоків, які полегшують роботу з спеціальними даними або типами .NET. Тим не менш, вам буде корисно поекспериментувати з можливостями синхронного читання/запису типу FileStream. Клас може працювати з текстовими або бінарними файлами. 

Припустимо нам потріблно записати рядок в файл Message.dat

ExploringFileStream\Program.cs
```cs
void ExploringWriteReadStringWithFileStream()
{
    string directoryFullName = @"D:\Temp";
    string fileFullName = @"D:\Temp\Message.dat";
    string message = "Hi girl! How are you?";

    WriteMassege(message,directoryFullName,fileFullName);

    ReadMessage(fileFullName);
}
ExploringWriteReadStringWithFileStream();

void WriteMassege(string message, string directoryFullName, string fileFullName)
{
    CheckOrCreateDirectory(directoryFullName);

    using FileStream fileStream = File.Open(fileFullName, FileMode.Create);

    byte[] bites = Encoding.Default.GetBytes(message);

    fileStream.Write(bites, 0, bites.Length);

    Console.WriteLine($"fileStream.Position after write:{fileStream.Position}");
}


void ReadMessage(string fileFullName)
{
    CheckFile(fileFullName);

    using FileStream fileStream = File.Open(fileFullName, FileMode.Open);

    Console.WriteLine($"fileStream.Position before read:{fileStream.Position}");

    byte[] bytes = new byte[fileStream.Length];
    for (int i = 0; i < fileStream.Length; i++)
    {
        bytes[i] = (byte)fileStream.ReadByte();
        Console.WriteLine($"{bytes[i]} {(char)bytes[i]}");
    }
    Console.WriteLine();

    Console.WriteLine(Encoding.Default.GetString(bytes));
}

// Helpers
void CheckOrCreateDirectory(string path)
{
    DirectoryInfo directoryInfo = new(path);
    if (!directoryInfo.Exists)
    { directoryInfo.Create(); }
}
void CheckFile(string path)
{
    if (!File.Exists(path))
    {
        throw new IOException("File not found.");
    }
}

```
```
fileStream.Position after write:21
fileStream.Position before read:0
72 H
105 i
32
103 g
105 i
114 r
108 l
33 !
32
72 H
111 o
119 w
32
97 a
114 r
101 e
32
121 y
111 o
117 u
63 ?

Hi girl! How are you?
```

FileStream може працювати лише з необробленими байтами, тому потрібно закодувати тип System.String у відповідний масив байтів. Простір імен System.Text визначає тип під назвою Encoding, який надає елементи, які кодують і декодують рядки до або з масиву байтів. Після кодування масив байтів зберігається у файлі за допомогою методу FileStream.Write(). Після запису fileStream.Position дорівнює кількості записаних байтів і тобто об'єкт вказує на кінець потоку байтів. 

Цей приклад заповнює файл даними, але він також підкреслює головний недолік роботи безпосередньо з типом FileStream: він вимагає працювати з необробленими байтами. Інші типи, отримані від Stream, працюють подібним чином. Наприклад, якщо ви хочете записати послідовність байтів в область пам’яті, ви можете використати MemoryStream.

Простір імен System.IO надає кілька типів, які інкапсулюють деталі роботи з потоковими типами.

## Асінхроний варіант запису та считування.

Клас FileStream має асінхронні варіанти запису та читивування. Як можна здогадатись вони направлені для швидшої обробки значних обсягів.

```cs
static async Task WriteAndReadFileAsync()
{
    Console.Write("Enter a string to write to a file:");
    string textForFile = Console.ReadLine()!;

    using FileStream fileStreamW = new(@"D:\Temp\note.txt", FileMode.Create);
    byte[] textArray = System.Text.Encoding.Default.GetBytes(textForFile);

    await fileStreamW.WriteAsync(textArray, 0, textArray.Length);

    fileStreamW.Close();

    using FileStream fileStreamR = File.OpenRead(@"D:\Temp\note.txt");
    byte[] array = new byte[fileStreamR.Length];

    await fileStreamR.ReadAsync(array, 0, array.Length);

    string textFromFile = System.Text.Encoding.Default.GetString(array);

    Console.WriteLine(textFromFile);

}
await WriteAndReadFileAsync();
```
```
Enter a string to write to a file:Hi girl! How are you?
Hi girl! How are you?
```

# Робота з StreamWriters та StreamReaders

Класи StreamWriter і StreamReader корисні, коли вам потрібно прочитати або записати символьні дані наприклад, рядки. Обидва працюють за замовчуванням із символами Unicode; однак ви можете змінити це, надавши правильно налаштоване посилання на об’єкт System.Text.Encoding.


## Запис в текстовий файл.

Тип StreamWriter як і StringWriter ( який розглянете пізніше в цій главі) походить від абстрактного базового класу під назвою TextWriter. Цей клас визначає члени, які дозволяють похідним типам записувати текстові дані в заданий потік символів.

Основні члени TextWriter

|Член|Використання|
|----|------------|
|Close()|Цей метод закриває програму запису та звільняє всі пов’язані ресурси. У процесі буфер автоматично очищається ( таки, цей елемент функціонально еквівалентний виклику методу Dispose).|
|Flush()|Цей метод очищає всі буфери для поточного пристрою запису та змушує будь-які буферизовані дані записуватися на основний пристрій; проте це не закриває об'єкт потоку.|
|NewLine|Ця властивість вказує константу нового рядка для похідного класу запису. За замовчуванням символом закінчення рядка для ОС Windows є символ повернення каретки, за яким іде перевод рядка (\r\n)|
|Write(), WriteAsync()|Цей перевантажений метод записує дані в текстовий потік без константи нового рядка|
|WriteLine(), WriteLineAsync()|Цей перевантажений метод записує дані в текстовий потік із константою нового рядка|

Останні два члени класу TextWriter, ймовірно, здаються вам знайомими. Якщо ви пам’ятаєте, тип System.Console має члени Write() і WriteLine(), які надсилають текстові дані на стандартний пристрій виводу. Насправді властивість Console.In обгортає TextReader, а властивість Console.Out обгортає TextWriter. 

Похідний клас StreamWriter забезпечує відповідну реалізацію методів Write(), Close() і Flush(), а також визначає додаткову властивість AutoFlush. Якщо встановлено значення true, ця властивість змушує StreamWriter скидати всі дані кожного разу, коли ви виконуєте операцію запису. Можете підвищити продуктивність, встановивши для AutoFlush значення false, за умови, що ви завжди викликаєте Close(), коли завершуєте запис за допомогою StreamWriter.

Використаємо StreamWriter для запису рядків в текстовий файл.

ExploringStreamWriterStreamReader\Program.cs
```cs

void WriteToTextReadFromText()
{
    string directoryFullName = @"D:\Temp";
    string fileFullName = @"D:\Temp\MyList.txt";

    WriteToText(directoryFullName,fileFullName);
}
WriteToTextReadFromText();

void WriteToText(string directoryFullName, string fileFullName)
{
    CheckOrCreateDirectory(directoryFullName);

    using StreamWriter streamWriter = File.CreateText(fileFullName);

    streamWriter.WriteLine("1 Build a house");
    streamWriter.WriteLine("2 Raise a Son");
    streamWriter.WriteLine("3 Plant a tree");
    streamWriter.WriteLine("4 Learn English");
    for (int i = 5; i <= 12; i++)
    {
        streamWriter.WriteLine(i + " ...");
    }
    for (int i = 0; i <= 12; i++)
    {
        streamWriter.Write(i + " ");
    }
    streamWriter.Write(streamWriter.NewLine);

    Console.WriteLine($"Created file {fileFullName}.");
}

```
```
Created file D:\Temp\MyList.txt.
```
```
1 Build a house
2 Raise a Son
3 Plant a tree
4 Learn English
5 ...
6 ...
7 ...
8 ...
9 ...
10 ...
11 ...
12 ...
0 1 2 3 4 5 6 7 8 9 10 11 12 
```

Cтворюється новий файл за допомогою методу File.CreateText(). Використовуючи отриманий об’єкт StreamWriter, ви можете додати деякі текстові дані до нового файлу.

## Читання з текстового файлу.

Читати текстові дані з файлу програмним шляхом за допомогою відповідного типу StreamReader. Цей клас походить від абстрактного TextReader.

Функціональні можливості TextReader.

|Член|Використання|
|----|------------|
|Peek()|Повертає наступний доступний символ (виражений у вигляді цілого числа), не змінюючи положення засобу зчитування. Значення -1 означає, що ви перебуваєте в кінці потоку|
|Read(), ReadAsync()|Читає дані з вхідного потоку|
|ReadBlock(), ReadBlockAsync()|Читає вказану максимальну кількість символів із поточного потоку та записує дані в буфер, починаючи з указаного індексу|
|ReadLine(), ReadLineAsync()|Читає рядок символів із поточного потоку та повертає дані у вигляді рядка (нульовий рядок означає EOF)|
|ReadToEnd(), ReadToEndAsync()|Читає всі символи від поточної позиції до кінця потоку та повертає їх як один рядок.|

Прочитаємо напередодні створений файл.

```cs

void WriteToTextReadFromText()
{
    string directoryFullName = @"D:\Temp";
    string fileFullName = @"D:\Temp\MyList.txt";

    WriteToText(directoryFullName,fileFullName);
    ReadFromText(fileFullName);
}

void ReadFromText(string fileFullName)
{
    CheckFile(fileFullName);
    
    using StreamReader streamReader = File.OpenText(fileFullName);

    string? input;
    while ((input = streamReader.ReadLine()) != null)
    {
        Console.WriteLine(input);
    }
}

```
```
Creanted file D:\Temp\MyList.txt.
1 Build a house
2 Raise a Son
3 Plant a tree
4 Learn English
5 ...
6 ...
7 ...
8 ...
9 ...
10 ...
11 ...
12 ...
0 1 2 3 4 5 6 7 8 9 10 11 12
```

## Безпосередне створеня об'єктів StreamWriter та StreamReader.

Можна працювати з StreamWriters і StreamReaders іншим способом: створюючи їх безпосередньо. 

```cs
void DirectlyCreateStreamWriterStreamReader()
{
    string path = @"D:\Temp\MyText.txt";

    using StreamWriter streamWriter  = new(path);

    streamWriter.WriteLine("Good day!");

    streamWriter.Close();

    using StreamReader streamReader = new(path);

    string? input = streamReader.ReadToEnd();

    Console.WriteLine(input);
}
DirectlyCreateStreamWriterStreamReader();
```
```
Good day!
```
При створенні за допомогою конструктора можна вказати як ми хочено працювати з файлом з нуля або додадти текст.

```cs
void CreateFileAndAppendToFileWithStreamWriter()
{
    string path = @"D:\Temp\MyText.txt";
    string text = "Hello\ngirl";

    using StreamWriter writer1 = new StreamWriter(path);
    writer1.Write(text);
    writer1.Close();

    using StreamWriter writer2 = new StreamWriter(path,true);
    writer2.WriteLine("s");
    writer2.Close();

    using StreamWriter writer3 = new StreamWriter(path, true,System.Text.Encoding.Default);
    writer3.WriteLine("How are you?");
    writer3.Close();

    using StreamReader reader1 = new StreamReader(path);
    string textFromFile = reader1.ReadToEnd();
    Console.WriteLine(textFromFile);

}
CreateFileAndAppendToFileWithStreamWriter();
```
```
Hello
girls
How are you?
```
Серед методів часто використовують ReadToEnd() який читає весь файл.

Крім того існують асінхронні варіанти читання та запису.


## Робота з StringWriters та StringReaders

Ви можете використовувати типи StringWriter і StringReader для обробки текстової інформації як потоку символів у пам’яті. Це може виявитися корисним, коли ви хочете додати символьну інформацію до основного буфера.

ExploringStringWriterStringReader
```cs
void CreateStringWriter()
{
    using StringWriter stringWriter = new();

    stringWriter.WriteLine("What is love?");

    Console.WriteLine($"Contens of stringWriter: {stringWriter} ") ;
}
CreateStringWriter();
```
```
Contens of stringWriter: What is love?
```
Тут записується блок рядкових даних до об’єкта StringWriter, а не до файлу на локальному жорсткому диску.

StringWriter і StreamWriter походять від одного базового класу TextWriter, тому логіка запису подібна. Враховуючи природу StringWriter, ви також повинні знати, що цей клас дозволяє використовувати наступний метод GetStringBuilder() для вилучення об’єкта System.Text.StringBuilder

```cs
void UseStringBuilder()
{
    using StringWriter stringWriter = new();
    stringWriter.WriteLine("What is love?");
    Console.WriteLine($"Contens of stringWriter: {stringWriter}");

    StringBuilder stringBuilder = stringWriter.GetStringBuilder();

    stringBuilder.Insert(0, "Hi!");
    Console.WriteLine($"Contens of stringWriter: {stringWriter}");

    stringBuilder.Remove(0, "Hi!".Length);
    Console.WriteLine($"Contens of stringWriter: {stringWriter}");


}
UseStringBuilder();
```
```
Contens of stringWriter: What is love?

Contens of stringWriter: Hi!What is love?

Contens of stringWriter: What is love?
```

Якщо ви хочете прочитати з потоку символьних даних, ви можете використовувати відповідний тип StringReader, який функціонує ідентично пов’язаному класу StreamReader.

```cs
void UsingStringReader()
{
    using StringWriter stringWriter = new();
    stringWriter.WriteLine("What is love?");

    using StringReader stringReader = new(stringWriter.ToString());

    string? input = null;

    while ((input = stringReader.ReadLine()) != null)
    {
        Console.WriteLine(input);
    }
}
UsingStringReader();
```
```
What is love?
```

## Кодування та декодування тексту.

Текстові символи можуть бути представлені різними способами. Наприклад, алфавіт можна закодувати за допомогою азбуки Морзе в ряд крапок і тире для передачі по телеграфній лінії. 

Подібним чином текст всередині комп’ютера зберігається у вигляді бітів (одиниць і нулів), що представляють кодову точку в кодовому просторі. Більшість кодових точок представляють один символ, але вони також можуть мати інші значення, наприклад форматування.

Наприклад, ASCII має кодовий простір із 128 кодовими точками. .NET використовує стандарт Unicode для внутрішнього кодування тексту. Unicode містить більше мільйона кодових точок.

Іноді вам потрібно буде перемістити текст за межі .NET для використання в системах, які не використовують Unicode або використовують різновиди Unicode, тому важливо навчитися конвертувати кодування між собою.

Альтернативні кодування тексту, які зазвичай використовуються комп’ютерами.

|Кодування|Опис|
|ASCII|Це кодує обмежений діапазон символів за допомогою молодших семи бітів байта|
|UTF-8|Це представляє кожну кодову точку Unicode як послідовність від одного до чотирьох байтів|
|UTF-16|Це представляє кожну кодову точку Unicode як послідовність одного або двох 16-бітних цілих чисел|
|ANSI/ISO|Це забезпечує підтримку різноманітних кодових сторінок, які використовуються для підтримки певної мови або групи мов|

У більшості випадків сьогодні UTF-8 є хорошим стандартним кодуванням, тому це буквально кодування за замовчуванням, тобто Encoding.Default.

Звичайно, вам може знадобитися створити текст, використовуючи це кодування для сумісності з іншою системою, тому це має залишитися опцією в .NET.

```cs

using System.Text;

static void EncodingText()
{
    Console.WriteLine("Encodings");
    Console.WriteLine("[1] ASCII");
    Console.WriteLine("[2] UTF-7");
    Console.WriteLine("[3] UTF-8");
    Console.WriteLine("[4] UTF-16 (Unicode)");
    Console.WriteLine("[5] UTF-32");
    Console.WriteLine("[6] Latin1");
    Console.WriteLine("[any other key] Default encoding");
    Console.WriteLine();

    Console.Write("Press a number to choose an encoding:");
    ConsoleKey number = Console.ReadKey(intercept: true).Key;

    Encoding encoder = number switch
    {
        ConsoleKey.D1 or ConsoleKey.NumPad1 => Encoding.ASCII,
        ConsoleKey.D2 or ConsoleKey.NumPad2 => Encoding.UTF7,
        ConsoleKey.D3 or ConsoleKey.NumPad3 => Encoding.UTF8,
        ConsoleKey.D4 or ConsoleKey.NumPad4 => Encoding.Unicode,
        ConsoleKey.D5 or ConsoleKey.NumPad5 => Encoding.UTF32,
        ConsoleKey.D6 or ConsoleKey.NumPad6 => Encoding.Latin1,
        _ => Encoding.Default
    };

    Console.WriteLine($"\n\nYou chose:"+encoder.BodyName);
        
    var message = "Café £4.39";
        
    Console.WriteLine($"\nText to encode: {message}  Characters: {message.Length}");
    // encode the string into a byte array
    byte[] encoded = encoder.GetBytes(message);
    // check how many bytes the encoding needed
    Console.WriteLine("{0} used {1:N0} bytes.",
      encoder.GetType().Name, encoded.Length);
    Console.WriteLine();
    // enumerate each byte 
    Console.WriteLine($"BYTE | HEX | CHAR");
    foreach (byte b in encoded)
    {
        Console.WriteLine($"{b,4} | {b.ToString("X"),3} | {(char)b,4}");
    }
    // decode the byte array back into a string and display it
    string decoded = encoder.GetString(encoded);
    Console.WriteLine(decoded);

}
//EncodingText();

while (true)
{
    Console.Clear();
    EncodingText();
    Console.ReadKey();
}
```
```
Encodings
[1] ASCII
[2] UTF-7
[3] UTF-8
[4] UTF-16 (Unicode)
[5] UTF-32
[6] Latin1
[any other key] Default encoding

Press a number to choose an encoding:

You chose:us-ascii

Text to encode: Cafe ?4.39  Characters: 10
ASCIIEncodingSealed used 10 bytes.

BYTE | HEX | CHAR
  67 |  43 |    C
  97 |  61 |    a
 102 |  66 |    f
  63 |  3F |    ?
  32 |  20 |
  63 |  3F |    ?
  52 |  34 |    4
  46 |  2E |    .
  51 |  33 |    3
  57 |  39 |    9
Caf? ?4.39
```
При виборі ASCII, що під час виведення байтів знак фунта (£) і наголошений e (é) не можуть бути представлені в ASCII, тому замість нього використовується знак питання.
Зауважте, що UTF-16 вимагає двох байтів для кожного символу, тобто загалом 20 байтів, і він може кодувати та декодувати символи é та £. Це кодування використовується внутрішньо .NET для зберігання символьних і рядкових значень.

Використовуючи допоміжні класи такі як StreamReader і StreamWriter, ви можете вказати кодування, яке ви хочете використовувати. Коли ви пишете в помічник, текст автоматично кодуватиметься, а коли ви читатимете з помічника, байти автоматично декодуватимуться.

Щоб указати кодування, передайте кодування як другий параметр у конструктор.

```cs
StreamReader reader = new(stream, Encoding.UTF8); 
StreamWriter writer = new(stream, Encoding.UTF8);
```
Часто у вас не буде вибору, яке кодування використовувати, оскільки ви створюватимете файл для використання в іншій системі. Однак, якщо ви це зробите, виберіть такий, який використовує найменшу кількість байтів, але може зберігати всі потрібні символи.

## Робота з BinaryWriters та BinaryReaders.

Обидва класи походять безпосередньо від System.Object. Ці типи дозволяють читати та записувати дискретні типи даних у базовий потік у компактному двійковому форматі.

Клас BinaryWriter визначає дуже перевантажений метод Write() для розміщення типу даних у базовому потоці. На додаток до члена Write(), BinaryWriter надає додаткові члени, які дозволяють отримати або встановити тип, отриманий від потоку; він також пропонує підтримку довільного доступу до даних.

Основні члени BinaryWriter

|Член|Опис|
|----|----|
|BaseStream|Ця властивість лише для читання надає доступ до основного потоку, який використовується з об’єктом BinaryWriter|
|Close()|Цей метод закриває бінарний потік|
|Flush()|Цей метод очищає двійковий потік|
|Seek()|Цей метод встановлює позицію в поточному потоці|
|Write()|Цей метод записує значення в поточний потік|


Клас BinaryReader доповнює функціональність, яку пропонує BinaryWriter.

Основні члени BinaryReader

|Член|Опис|
|----|----|
|BaseStream|Ця властивість лише для читання надає доступ до основного потоку, який використовується з об’єктом BinaryReader|
|Close()|Цей метод закриває об'єкт BinaryReader|
|PeekChar()|Цей метод повертає наступний доступний символ без просування позиції в потоці|
|Read()|Цей метод читає заданий набір байтів або символів і зберігає їх у вхідному масиві|
|ReadXXXX()|Клас BinaryReader визначає численні методи читання, які захоплюють наступний тип із потоку (наприклад, ReadBoolean(), ReadByte() і ReadInt32())|

Запишемо дані за допомогою BinaryWriter.

```cs
void WorkWithBinaryWriter()
{
    FileInfo fileInfo = new(@"D:\Temp\BinaryFile.dat");

    using BinaryWriter binaryWriter = new(fileInfo.OpenWrite());

    Console.WriteLine($"binaryWriter.BaseStream : {binaryWriter.BaseStream}");

    int myInt = 888;
    double myDouble = 888.88;
    string myString = "hi!";

    binaryWriter.Write(myInt);
    binaryWriter.Write(myDouble);
    binaryWriter.Write(myString);

    Console.WriteLine("Done.");
}
WorkWithBinaryWriter();
```
```
binaryWriter.BaseStream : System.IO.FileStream
Done.

```
Зверніть увагу, як об’єкт FileStream, повернутий із FileInfo.OpenWrite(), передається конструктору типу BinaryWriter. Використання цієї техніки дозволяє легко розшарувати потік перед записом даних. Зауважте, що конструктор BinaryWriter приймає будь-який тип, отриманий від Stream (наприклад, FileStream, MemoryStream або BufferedStream). Таким чином, запис двійкових даних у пам'ять натомість є таким же простим, як надання дійсного об'єкта MemoryStream.

Прочитаємо дані за допомогою BinaryReader.

```cs
static void WorkWithBinaryReader()
{
    FileInfo fileInfo = new(@"D:\Temp\BinaryFile.dat");

    using BinaryReader binaryReader = new(fileInfo.OpenRead());

    Console.WriteLine(binaryReader.ReadInt32());
    Console.WriteLine(binaryReader.ReadDouble());
    Console.WriteLine(binaryReader.ReadString());
}
WorkWithBinaryReader();
```
```
888
888,88
hi!
```

# Клас FileSystemWatcher.

Цей тип може бути дуже корисним, якщо ви хочете контролювати або «наблюдати» за файлами у вашій системі програмно. Зокрема, ви можете наказати типу FileSystemWatcher контролювати файли для будь-яких дій, визначених переліком System.IO.NotifyFilters.

```cs
    //
    // Summary:
    //     Specifies changes to watch for in a file or folder.
    //     Визначає зміни, які потрібно спостерігати у файлі чи папці.
    [Flags]
    public enum NotifyFilters
    {
        //
        // Summary:
        //     The name of the file.
        FileName = 1,
        //
        // Summary:
        //     The name of the directory.
        DirectoryName = 2,
        //
        // Summary:
        //     The attributes of the file or folder.
        Attributes = 4,
        //
        // Summary:
        //     The size of the file or folder.
        Size = 8,
        //
        // Summary:
        //     The date the file or folder last had anything written to it.
        LastWrite = 16,
        //
        // Summary:
        //     The date the file or folder was last opened.
        LastAccess = 32,
        //
        // Summary:
        //     The time the file or folder was created.
        CreationTime = 64,
        //
        // Summary:
        //     The security settings of the file or folder.
        Security = 256
    }
```
Щоб розпочати роботу з типом FileSystemWatcher, вам потрібно встановити властивість Path, щоб указати ім’я та розташування каталогу, який містить файли, які ви хочете контролювати, а також властивість Filter, яка визначає розширення файлів, які ви хочу стежити.

Можна вибрати обробку подій Changed, Created і Deleted, усі вони працюють у поєднанні з делегатом FileSystemEventHandler.

```cs
    //
    // Summary:
    //     Represents the method that will handle the System.IO.FileSystemWatcher.Changed,
    //     System.IO.FileSystemWatcher.Created, or System.IO.FileSystemWatcher.Deleted event
    //     of a System.IO.FileSystemWatcher class.
    //
    // Parameters:
    //   sender:
    //     The source of the event.
    //
    //   e:
    //     The System.IO.FileSystemEventArgs that contains the event data.
    public delegate void FileSystemEventHandler(object sender, FileSystemEventArgs e);
```

Ви також можете обробити подію Renamed за допомогою типу делегату RenamedEventHandler,

```cs
    //
    // Summary:
    //     Represents the method that will handle the System.IO.FileSystemWatcher.Renamed
    //     event of a System.IO.FileSystemWatcher class.
    //
    // Parameters:
    //   sender:
    //     The source of the event.
    //
    //   e:
    //     The System.IO.RenamedEventArgs that contains the event data.
    public delegate void RenamedEventHandler(object sender, RenamedEventArgs e);
```

Виконаємо відстеження за файлами *.txt.


ExploringFileSystemWatcher
```cs

static void MonitoringFilesInADirectory()
{
    // Establish the path to the directory to watch
    FileSystemWatcher fileSystemWatcher = new();
	try
	{
		fileSystemWatcher.Path = @"D:\Temp";
        Console.WriteLine($"Set up the fileSystemWatcher.Path = " +
            $"{fileSystemWatcher.Path}");
    }
    catch (Exception ex)
	{
        Console.WriteLine(ex.Message);
	}

    // Set up the things to be on the lookout for.
    fileSystemWatcher.NotifyFilter = 
          NotifyFilters.LastAccess
        | NotifyFilters.LastWrite
        | NotifyFilters.FileName
        | NotifyFilters.DirectoryName;
    Console.WriteLine($"Set up the fileSystemWatcher.NotifyFilter = " +
        $"{fileSystemWatcher.NotifyFilter}");

    // Only watch text files.
    fileSystemWatcher.Filter = "*.txt";

    Console.WriteLine($"Set up the fileSystemWatcher.Filter = " +
        $"{fileSystemWatcher.Filter}");

    // Add event handlers.
    
    // Specify what is done when a file is changed, created, or deleted.
    fileSystemWatcher.Changed += (s, e) =>
    Console.WriteLine($"File {e.FullPath} {e.ChangeType} ");
    fileSystemWatcher.Created += (s, e) =>
    Console.WriteLine($"File {e.FullPath} {e.ChangeType} ");
    fileSystemWatcher.Deleted += (s, e) =>
    Console.WriteLine($"File {e.FullPath} {e.ChangeType} ");

    // Specify what is done when a file is renamed.
    fileSystemWatcher.Renamed += (s, e) =>
    Console.WriteLine($"File {e.OldFullPath} renamed to {e.FullPath} ");

    // Begin watching the directory.
    fileSystemWatcher.EnableRaisingEvents = true;
    Console.WriteLine($"I'm watching what happened with text file in directory " +
        $"{fileSystemWatcher.Path}");
    Console.WriteLine("Press End to quit.\n\n");

    // Raise some events.
    DoSomethingWithTheFiles();

    while (Console.ReadKey().Key != ConsoleKey.End) { }
}
MonitoringFilesInADirectory();


// Test helper
static void DoSomethingWithTheFiles()
{
    using StreamWriter streamWriter = File.CreateText(@"D:\Temp\MyDoc.txt");

    streamWriter.WriteLine("Chapter 1");
    streamWriter.Close();

    File.Move(@"D:\Temp\MyDoc.txt", @"D:\Temp\MyDocumetation.txt");
    File.Delete(@"D:\Temp\MyDoc.txt");
}

```
```
Set up the fileSystemWatcher.Path = D:\Temp
Set up the fileSystemWatcher.NotifyFilter = FileName, DirectoryName, LastWrite, LastAccess
Set up the fileSystemWatcher.Filter = *.txt
I'm watching what happened with text file in directory D:\Temp
Press End to quit.


File D:\Temp\MyDoc.txt Created
File D:\Temp\MyDoc.txt Changed
File D:\Temp\MyDoc.txt renamed to D:\Temp\MyDocumetation.txt

```
Крім того протестувати роботу коду можна безпосередньо змінюючи файли txt в каталозі тим самим викликаючи події.


Всі розглятуті класи для оперцій I/O ви, звичайно, будете використовувати в багатьох своїх програмах, але служби серіалізації об’єктів можуть значно спростити збереження великих обсягів даних.
