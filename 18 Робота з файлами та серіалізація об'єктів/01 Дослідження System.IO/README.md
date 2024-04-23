# Дослідження System.IO

Простір імен System.IO —  присвячена службам введення та виведення (I/O) на основі файлів та пам’яті. Як і будь-який простір імен, System.IO визначає набір класів, інтерфейсів, перерахувань, структур і делегатів, більшість з яких можна знайти в mscorlib.dll. На додаток до типів, що містяться в mscorlib.dll, збірка System.dll визначає додаткові члени простору імен System.IO.
Багато типів у просторі імен System.IO зосереджені на програмному маніпулюванні фізичними каталогами та файлами.Однак додаткові типи забезпечують підтримку читання даних і запису даних у рядкові буфери, а також необроблені місця пам’яті. 

Основні класи System.IO

    BinaryReader, BinaryWriter : Ці класи дозволяють зберігати та отримувати примітивні типи даних (цілі числа, логічні значення, рядки тощо) як двійкові значення.

    BufferedStream : Цей клас надає тимчасове сховище для потоку байтів, які ви можете передати в сховище пізніше.

    Directory, DirectoryInfo : Класи для керування структурою каталогів машини. Тип Directory надає функціональні можливості за допомогою статичних членів, тоді як тип DirectoryInfo надає подібні функції з дійсного посилання на об’єкт.

    DriveInfo : Цей клас надає детальну інформацію про диски, які використовує дана машина.

    File, FileInfo : Класи для маніпулювання набором файлів машини. Тип File надає функціональні можливості за допомогою статичних членів, тоді як тип FileInfo надає подібні функціональні можливості з дійсного посилання на об’єкт.

    FileStream : Цей клас надає довільний доступ до файлів (наприклад, можливість пошуку) з даними, представленими у вигляді потоку байтів.

    FileSystemWatcher : Цей клас дозволяє відстежувати модифікацію зовнішніх файлів у вказаному каталозі.

    MemoryStream : Цей клас забезпечує довільний доступ до потокових даних, що зберігаються в пам’яті, а не у фізичному файлі.

    Path : Цей клас виконує операції з типами System.String, які містять інформацію про шлях до файлу або каталогу, незалежно від платформи.

    StreamWriter, StreamReader : Класи для зберігання і отримання текстової інформації у або з файлу. Ці типи не підтримують довільний доступ до файлів.

    StringWriter, StringReader : Як і класи StreamReader/StreamWriter, ці класи також працюють з текстовою інформацією. Однак основним сховищем є рядковий буфер, а не фізичний файл.

На додаток до цих конкретних типів класів, System.IO визначає кілька перерахувань, а також набір абстрактних класів (наприклад, Stream, TextReader і TextWriter), які визначають спільний поліморфний інтерфейс для всіх нащадків.

System.IO надає чотири класи, які дозволяють вам маніпулювати окремими файлами, а також взаємодіяти зі структурою каталогів машини. Directory та File, надають доступ до операцій створення, видалення, копіювання та переміщення за допомогою різних статичних елементів. Тісно пов’язані типи FileInfo та DirectoryInfo надають подібну функціональність, як і методи рівня екземпляра (отже, ви повинні створити їх за допомогою ключового слова new). Класи Directory і File безпосередньо розширюють System.Object, тоді як DirectoryInfo і FileInfo походять від абстрактного типу FileSystemInfo. 
FileInfo та DirectoryInfo зазвичай служать кращим вибором для отримання повної інформації про файл або каталог (наприклад, час створення або можливості читання/запису), оскільки їхні члени мають тенденцію повертати строго типізовані об’єкти. Навпаки, члени класів Directory і File мають тенденцію повертати прості рядкові значення, а не строго типізовані об’єкти. Однак це лише орієнтир; у багатьох випадках ви можете виконати ту саму роботу за допомогою File/FileInfo або Directory/DirectoryInfo.

## Абстрактний базовий клас FileSystemInfo.

Типи DirectoryInfo та FileInfo отримують багато поведінки від абстрактного базового класу FileSystemInfo. Здебільшого ви використовуєте члени класу FileSystemInfo для виявлення загальних характеристик (таких як час створення, різні атрибути тощо) певного файлу чи каталогу. 

Основні властивості класу FileSystemInfo.

    Attributes : Отримує або встановлює атрибути, пов’язані з поточним файлом, які представлені переліком FileAttributes (наприклад, чи файл або каталог доступні лише для читання, зашифровані, приховані чи стиснуті?).

    CreationTime : Отримує або встановлює час створення для поточного файлу або каталогу.

    Exists : Визначає, чи існує даний файл або каталог.

    Extension : Отримує розширення файлу.

    FullName : Отримує повний шлях до каталогу або файлу.

    LastAccessTime : Отримує або встановлює час останнього доступу до поточного файлу чи каталогу.

    LastWriteTime : Отримує або встановлює час останнього запису в поточний файл або каталог.

    Name : Отримує назву поточного файлу або каталогу.

FileSystemInfo також визначає метод Delete(). Це реалізовано похідними типами для видалення певного файлу чи каталогу з жорсткого диска. Крім того, ви можете викликати Refresh() перед отриманням інформації про атрибути, щоб переконатися, що статистика щодо поточного файлу (або каталогу) не застаріла.

## Клас DirectoryInfo, Directory та DriveInfo.

### Робота з DirectoryInfo.

Клас містить набір членів, які використовуються для створення, переміщення, видалення та нумерації каталогів і підкаталогів. Крім функціональних можливостей, які надає базовий клас FileSystemInfo, DirectoryInfo пропонує ключові члени що до каталогів.

Ключові члени DirectoryInfo

    Create(), CreateSubdirectory() : Створює каталог або набір підкаталогів, коли отримує ім’я шляху

    Delete() : Видаляє каталог і весь його вміст

    GetDirectories() : Повертає масив об’єктів DirectoryInfo, які представляють усі підкаталоги в поточному каталозі

    GetFiles() : Повертає масив об’єктів DirectoryInfo, які представляють усі підкаталоги в поточному каталозі

    MoveTo() : Переміщує каталог і його вміст на новий шлях

    Parent : Отримує батьківський каталог цього каталогу

    Root : Gets the root portion of a path

Викорситаємо ці методи. Спочатку створемо допоміжну функцію яка буде показувати дані про об'єкт.

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
Цей допоміжний метод виуористовує функціонал успадкований від FileSystemInfo.

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
        Parent : D:\MyWork\CS-Step-by-Step\18 Робота з файлами та сер?ал?зац?я об'єкт?в\01 Досл?дження System.IO\ExploringSystemIO\ExploringSystemIO\bin\Debug
        FullName : D:\MyWork\CS-Step-by-Step\18 Робота з файлами та сер?ал?зац?я об'єкт?в\01 Досл?дження System.IO\ExploringSystemIO\ExploringSystemIO\bin\Debug\net8.0
DirectoryInfo object: Windows

        Name : Windows
        CreationTime : 07.12.2019 11:03:44
        Attributes : Directory, Archive
        Root : C:\
        Parent : C:\
        FullName : C:\Windows
```

Як правило починаєте працювати з типом DirectoryInfo, вказуючи певний шлях до каталогу як параметр конструктора. Використовуйте крапку (.), якщо хочете отримати доступ до поточного робочого каталогу (каталог програми, що виконується).
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

### Прохід по файлах з використанням DirectoryInfo

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

### Створення підкаталогів використовуючи DirectoryInfo.

Можна програмно розширити структуру каталогу за допомогою методу DirectoryInfo.CreateSubdirectory().

```cs
void HowCreateSubdirectory()
{
    DirectoryInfo directoryInfo_1 = new(@"D:\SuperCode");

    if (!directoryInfo_1.Exists)
    {
        directoryInfo_1.Create();
    }

    ShowDirectoryInfo(directoryInfo_1);

    DirectoryInfo directoryInfo_1_1 =  CreateSubdirectory(directoryInfo_1, @"Project");
    ShowDirectoryInfo(directoryInfo_1_1);

    DirectoryInfo directoryInfo_1_2 = CreateSubdirectory(directoryInfo_1, @"Project1\Data");
    ShowDirectoryInfo(directoryInfo_1_2);

}
HowCreateSubdirectory();

DirectoryInfo CreateSubdirectory(DirectoryInfo directoryInfo,string name )
{
   return directoryInfo.CreateSubdirectory(name);
}
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
Mетод може створити один підкаталог, а також кілька вкладених підкаталогів за один виклик функції. Не обов'язково присваюватизмінній значення, яке повертає метод CreateSubdirectory, але ви повинні знати, що об’єкт DirectoryInfo, який представляє щойно створений елемент, передається назад після успішного виконання.

### Робота з класом Directory.

Клас виконує аналогічну функціональність DirectoryInfo але більшисть його членів статичні і зазвичай працють з рядками.

```cs
void ExploringDirectory()
{
    ShowAllDriveWithDirectory();
    Console.WriteLine("\n");

    DeleteDirectory(@"D:\SuperCode\Project1");

}
ExploringDirectory();

void ShowAllDriveWithDirectory()
{
    string[] driveNames = Directory.GetLogicalDrives();
    
    Console.WriteLine("The masine has drive:");
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
The masine has drive:
        C:\
        D:\


Delete directory D:\SuperCode\Project1 (Y/N):Y
The directory has been deleted
```
Зверніть увагу на те, що члени Directory зазвичай повертають рядкові дані, а не строго типізовані об’єкти FileInfo/DirectoryInfo.

### Робота з класом DriveInfo.

На відміну від методу Directory.GetLogicalDrives метод класу DriveInfo надає більше даних про логічні диски.

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

## Класи FileInfo та File.

### Клас FileInfo.

Крім функціональних можливостей, успадкованих від FileSystemInfo, нижче приведені основні члени, унікальні для класу FileInfo.

Основні члени FileInfo

    AppendText() : Створює об’єкт StreamWriter (описано пізніше), який додає текст до файлу

    CopyTo() : Копіює наявний файл у новий файл

    Create() : Створює новий файл і повертає об’єкт FileStream (описано пізніше) для взаємодії з новоствореним файлом

    CreateText() : Створює об’єкт StreamWriter, який записує новий текстовий файл

    Delete() : Видаляє файл, до якого прив’язаний екземпляр FileInfo

    Directory : Отримує екземпляр батьківського каталогу

    DirectoryName : Отримує повний шлях до батьківського каталогу

    Length : Gets the size of the current file

    MoveTo() : Переміщує вказаний файл у нове розташування, надаючи можливість вказати нове ім’я файлу

    Name : Отримує назву файлу

    Open() : Відкриває файл із різними правами на читання/запис і спільний доступ

    OpenRead() : Створює об’єкт FileStream лише для читання

    OpenText() : Створює об’єкт StreamReader (описано пізніше), який читає з наявного текстового файлу

    OpenWrite() : Створює об’єкт FileStream лише для запису

Зауважте, що більшість методів класу FileInfo повертають певний орієнтований на введення-виведення об’єкт (наприклад, FileStream і StreamWriter), який дозволяє вам почати читання та запис даних до (або читання з) пов’язаного файлу в різних форматах.

### Метод FileInfo.Create.

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

    var fileInfoFullName = @"D:\Temp\Test.dat";
    
    FileInfo fileInfo = new FileInfo(fileInfoFullName);
    FileStream fileStream = fileInfo.Create();

    // Use FileStream

    fileStream.Close();
}
ExploringCreateFileWithFileInfo();
```
Зверніть увагу, що метод FileInfo.Create() повертає об’єкт FileStream, який надає повний доступ для читання/запису всім користувачам.
Зауважте, що після завершення роботи з поточним об’єктом FileStream ви повинні переконатися, що ви закрили дескриптор, щоб звільнити базові некеровані ресурси потоку.
Аналогічно цьому можна щоб компілятор сам турбувався за виклик методу що реалізовує інтерфейс IDisposable. Для цього иожна використовувати using.

```cs
void ExploringCreateFileWithFileInfo()
{
    CheckOrCreateDirectory(@"D:\Temp");

    var fileInfoFullName = @"D:\Temp\Test.dat";
    
    FileInfo fileInfo = new FileInfo(fileInfoFullName);

    using FileStream fileStream = fileInfo.Create();

    // Use FileStream

}
ExploringCreateFileWithFileInfo();
```
Тобто такий шаблон є трохі компактнішим. Майте на увазі при роботі методу, в такому варіанті парметрів, якшо такий файл існував його буде перезаписано без завуважень.

### Метод FileInfo.Open.

Ви можете використовувати метод FileInfo.Open(), щоб відкривати наявні файли, а також створювати нові файли з набагато більшою точністю, ніж за допомогою FileInfo.Create().

```cs
void ExploringFileInfoOpen()
{
    CheckOrCreateDirectory(@"D:\Temp");

    var fileInfoFullName = @"D:\Temp\Test.dat";

    FileInfo fileInfo = new FileInfo(fileInfoFullName);

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

    CreateNew : Повідомляє ОС створити новий файл. Якщо він уже існує, створюється виняткова ситуація IOException.

    Create : Повідомляє ОС створити новий файл. Якщо він уже існує, його буде перезаписано.

    Open : Відкриває наявний файл. Якщо файл не існує, створюється виняткова ситуація FileNotFoundException.

    OpenOrCreate : Відкриває файл, якщо він існує; інакше буде створено новий файл.

    Truncate : Відкриває наявний файл і скорочує його до 0 байт.

    Append : Відкриває файл, переміщується в кінець файлу та починає операції запису (ви можете використовувати цей прапор лише з потоком лише для запису). Якщо файл не існує, створюється новий файл.

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

### Методи FileInfo.OpenRead , FileInfo.OpenWrite.

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

### Метод FileInfo.OpenText().

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
Як ви незабаром побачите, тип StreamReader забезпечує спосіб читання символьних даних із  файлу.

### Методи FileInfo.CreateText та FileInfo.AppendText.

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

### Корисні методи класу File.

Тип File також підтримує кілька членів які можуть значно спростити процеси читання та запису даних.

Корисні методи класу File

    ReadAllBytes() : Відкриває вказаний файл, повертає двійкові дані у вигляді масиву байтів, а потім закриває файл

    ReadAllLines() : Відкриває вказаний файл, повертає символьні дані у вигляді масиву рядків, а потім закриває файл

    ReadAllText() : Відкриває вказаний файл, повертає символьні дані як System.String, а потім закриває файл

    WriteAllBytes() : Відкриває вказаний файл, записує масив байтів, а потім закриває файл

    WriteAllLines() : Відкриває вказаний файл, записує масив рядків, а потім закриває файл

    WriteAllText() : Відкриває вказаний файл, записує символьні дані з указаного рядка, а потім закриває файл

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

Кожен із цих членів автоматично закриває основний дескриптор файлу. Код зберігає рядкові дані в новому файлі на диску (і зчитує його в пам’ять) з мінімальними зусиллями. Єдине зауваженя, якшо існував файл метод WriteAllLine затирає старі дані без запитів на збереження. Як бачимо це простий шлях роботи з файлом. Перевагою створення об’єкта FileInfo є те, що ви можете досліджувати файл за допомогою членів абстрактного базового класу FileSystemInfo. 



## Абстрактний клас Stream.

В прикладах ви бачили багато способів отримати об’єкти FileStream, StreamReader і StreamWriter, але вам ще належить зчитувати дані з або записати дані у файл за допомогою цих типів. Щоб зрозуміти, як це зробити, вам потрібно буде ознайомитися з поняттям потоку.
У світі маніпулювання вводом-виводом потік представляє фрагмент даних, що перетікає між джерелом і одержувачем. Потоки забезпечують загальний спосіб взаємодії з послідовністю байтів, незалежно від того, який тип пристрою (наприклад, файл, мережеве підключення або принтер) зберігає або відображає ці байти. Концепція потоку не обмежується файловим вводом-виводом. Бібліотеки .NET забезпечують потоковий доступ до мереж, місць пам’яті та інших потокоцентричних абстракцій. 
Абстрактний клас System.IO.Stream визначає кілька членів, які забезпечують підтримку синхронної та асинхронної взаємодії з носієм даних (наприклад, основним файлом або місцем пам’яті).
Нащадки потоку представляють дані як необроблений потік байтів; отже, робота безпосередньо з необробленими потоками може бути досить загадковою. Деякі типи, отримані від потоку, підтримують пошук, який відноситься до процесу отримання та коригування поточної позиції в потоці.

Члени класу Stream

    CanRead, CanWrite, CanSeek : Визначає, чи підтримує поточний потік читання, пошук і/або запис.

    Close() : Закриває поточний потік і звільняє всі ресурси (наприклад, сокети та дескриптори файлів), пов’язані з поточним потоком. Внутрішньо цей метод є псевдонімом методу Dispose(); отже, закриття потоку функціонально еквівалентно видаленню потоку.

    Flush() : Оновлює базове джерело даних або сховище поточним станом буфера, а потім очищає буфер. Якщо потік не реалізує буфер, цей метод нічого не робить.

    Length : Повертає довжину потоку в байтах.

    Position : Determines the position in the current stream.

    Read(), ReadByte(), ReadAsync() : Зчитує послідовність байтів (або один байт) із поточного потоку та переміщує поточну позицію в потоці на кількість прочитаних байтів.

    Seek() : Встановлює позицію в поточному потоці. 

    SetLength() : Встановлює довжину поточного потоку.

    Write(), WriteByte(), WriteAsync() : Записує послідовність байтів (або один байт) у поточний потік і просуває поточну позицію в цьому потоці на кількість записаних байтів.












