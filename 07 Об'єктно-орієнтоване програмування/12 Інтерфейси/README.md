# Інтерфейси

Об'єкти реального світу мають різноманітну поведінку. Наприклад транспортний засім має поведінкову особливість перемішатись. Інтерфейс виражає поведінку яку даний клас або структура може підтримувати. Клас може підтримувати скільки необхідно інтерфейсів шо означає шо клас може підтримувати декілька поведінок, так само як людина може і бігати і плавати і стрибати.
Бібіліотеки .Net побудований таким чином шо визначена значна кількість інтерфейсів та їх реализацій для різних потреб. Наприклад в технології ADO.NET дозволяє комунікувати з різноманітних типами систем управліня БД. Ви можете підклячичтися до БД за допомогою різних об'єктів (SqlConnection, OleDbConnection, OdbcConnection,...) а також до різних БД(MySQL, Oracle,...).Всі вони реалізовують загальний інтерфейс IDbConnection. 

```cs
namespace System.Data
{
    //
    // Summary:
    //     Represents an open connection to a data source, and is implemented by .NET data
    //     providers that access relational databases.
    public interface IDbConnection : IDisposable
    {
        string ConnectionString { get; set; }
        int ConnectionTimeout { get; }
        string Database { get; }

        IDbTransaction BeginTransaction();
        IDbTransaction BeginTransaction(IsolationLevel il);
        void ChangeDatabase(string databaseName);
        void Close();
        IDbCommand CreateCommand();
        void Open();
    }
}
```
Цей інтерфайс визначає набір членів які визначають поведінку комунікувати з БД. Це гарантує що кожен об'єкт класу, який реалізовує цей інтерфейс, буде мати ці члени.
Кожний клас реалізовує ці методи в залежності від БД але поведінковий сенс однаковий. Тобто з якою б СУБД ви не працювали ви можете використати властивості і методи цього інтерфейсу (Open() Close() CreateCommand()). 
Іншими словами інтерфейс це наче контракт яку поведінку повинен реалізувати клас.
Інтерфейсів .NET зазвичай  починаються з великої літери I. 

## Інтерфейсні типи в порівняні з абстрактними класами. 

Може здатися шо інтерфейси не відрізняються від абстрактних базових класів. Коли клас позначено як абстрактний, він може визначати будь-яку кількість абстрактних членів, щоб забезпечити поліморфний інтерфейс для всіх похідних типів. В абстрактному класі можна визначити конструктори, неабстракні чтени з реалізацією. Інтерфейси містять в більшості визначення але можуть також мати реалізацію за замовчуванням і статичні члени. 
Є лише дві реальні відмінності: інтерфейси не можуть мати нестатичні конструктори, а клас може реалізувати декілька інтерфейсів. Тобто інтерфейс більше видповідають за поведінка , а абстрактний клас є основою сімейста класів.
Поліморфний інтерфейс, створений абстрактним батьківським класом, має одне велике обмеження, а саме те, що лише похідні типи підтримують члени, визначені абстрактним батьківським класом. Але великі проекти можкть мати великі розгалужені іерархіЇ і не мати одного спільного базового класу. 

Проект InterfaceTypesVsAbstractClasses\Classes.cs
```cs
    public abstract class CloneableType
    {
        public abstract object Clone();
    }

    public class Car
    {
    }
    
    public class Bus : Car, CloneableType { } // cannot multiple base classes 
```
Тільки нашадки класу CloneableType будуть підтримувати метод Clone(). Інший набір класів не може отримати цей поліморфний інтерфейс. Також ми не можето зробити базовими декілька класів. 

Інтерфейси можна реалізовувати в будь-якому класі або структурі. Інтерфейси дуже поліморфні. Розглянемо вбудований інтерфейс IClonable.

```cs
namespace System
{
    //
    // Summary:
    //     Supports cloning, which creates a new instance of a class with the same value
    //     as an existing instance.
    public interface ICloneable
    {
        //
        // Summary:
        //     Creates a new object that is a copy of the current instance.
        //
        // Returns:
        //     A new object that is a copy of this instance.
        object Clone();
    }
}
```
Якшо ви подиветесь бібліотеку базових класів .Net багато класів реалізовують його (System.Array, System.Data.SqlClient.SqlConnection, System.OperatingSystem, System.String ...). Хоча типи різні їх об'єкти можна обробляти за допомогою типу інтерфейсу IClonable.

```cs
ExplorationICloneable();
void ExplorationICloneable()
{
    CloneMe("Hi");

    int[] myArray = new int[3] { 1, 2, 3 };
    CloneMe(myArray);

    OperatingSystem operatingSystem = new OperatingSystem(PlatformID.Unix, new Version());
    CloneMe(operatingSystem);


    void CloneMe(ICloneable cloneable)
    {
        object TheClone = cloneable.Clone();
        Console.WriteLine("\n"+cloneable);
        Console.WriteLine($"Clone:{TheClone}");
        Console.WriteLine($"Type:{TheClone.GetType()}");
        Console.WriteLine($"ReferenceEquals:{ReferenceEquals(cloneable, TheClone)}");
    }
}
```
```

Hi
Clone:Hi
Type:System.String
ReferenceEquals:True

System.Int32[]
Clone:System.Int32[]
Type:System.Int32[]
ReferenceEquals:False

Unix 0.0
Clone:Unix 0.0
Type:System.OperatingSystem
ReferenceEquals:False
```
Функція CloneMe має параметр типу ICloneable який приймає любий об'єкт який реалізовує цей інтрефейс. 

Розглядаючи абстракний клас також треба зазначити шо похідні відного класи забовязані реалізувати абстрактні члени. Наприклад є абстракний клас Shape і ви вирішили додати метод GetNumberOfPoints(), який повертає кількість граничних точок фігури. Після цього всі похідні класи (Circle, Hexagon та ThreeDCircle) мають мати реалізацію цього метода навіть коли в цьому нема сенсу. Інтерфейсні типи можуть допомогти в цьому випадку. Якщо ви визначаєте інтерфейс, який представляє поведінку «наявності точок», ви можете просто підключити його до типу Hexagon, залишивши Circle і ThreeDCircle недоторканими.

## Визначення вланих інтерфейсів 





