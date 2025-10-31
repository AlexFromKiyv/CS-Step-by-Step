# Створення Data Access Layer з EF Core

Цей розділ присвячено застосуванню того, що ви дізналися про EF Core, для створення рівня доступу до даних AutoLot. Цей розділ починається зі створення одного проекту для сутностей і іншого для коду бібліотеки доступу до даних. Відокремлення моделей від коду доступу до даних є звичайним проектним рішенням, яке використано в ASP.NET Core.
Наступним кроком є ​риштування(створення каркасу моделі) наявної бази даних із розділу ADO.NET на сутності та похідний DbContext за допомогою інтерфейсу командного рядка (CLI) EF Core. Це демонструє процес database first. Потім проект змінюється на code first, де дизайн бази даних керується сутностями C#. 
Сутності з розділу ADO.NET оновлюються до остаточної версії, нові сутності з попередніх розділів додаються в модель, а база даних оновлюється за допомогою міграцій EF Core. Потім збережена процедура, перегляд бази даних і визначені користувачем функції інтегруються в систему міграції EF Core, забезпечуючи розробникам унікальний механізм отримання повної копії бази даних. Остаточна міграція EF Core завершує базу даних. 
Наступним кроком є ​​створення репозиторіїв, які надають інкапсульований доступ для створення, читання, оновлення та видалення (CRUD) до бази даних. Останнім кроком у цій главі є додавання коду ініціалізації даних для надання зразкових даних, що є звичайною практикою для тестування рівня доступу до даних.

## Створення проектів AutoLot.Dal і AutoLot.Models

AutoLot прошарок доступу до даних складається з двох проектів: один для зберігання коду EF Core (похідний DbContext, фабрика контексту, репозиторії, міграції тощо), а інший для зберігання сутностей і моделей перегляду.
Створіть нове рішення під назвою AutoLotSolution, додайте до нього бібліотеку класів .NET Core під назвою AutoLot.Models і додайте до проекту пакети. Це можна зробити створивши папку AutoLotSolution і виконавши в ній команди.

```console
dotnet new sln 
dotnet new classlib -n AutoLot.Models -f net8.0
dotnet sln add .\AutoLot.Models
dotnet add AutoLot.Models package Microsoft.EntityFrameworkCore
dotnet add AutoLot.Models package Microsoft.EntityFrameworkCore.SqlServer
dotnet add AutoLot.Models package System.Text.Json
```

Пакет Microsoft.EntityFrameworkCore.Abstractions надає доступ до багатьох конструкцій EF Core (наприклад, анотацій даних), має меншу вагу, ніж пакет Microsoft.EntityFrameworkCore, і зазвичай використовується для модельних проектів. Однак підтримка нової функції IEntityTypeConfiguration<T> є не в пакеті Abstractions, а в повному пакеті EF Core.

Додайте до рішення інший проект бібліотеки класів .NET Core під назвою AutoLot.Dal. Додайте пакети.

```console
dotnet new classlib -n AutoLot.Dal -f net8.0
dotnet sln add .\AutoLot.Dal
dotnet add AutoLot.Dal package Microsoft.EntityFrameworkCore
dotnet add AutoLot.Dal package Microsoft.EntityFrameworkCore.Design
dotnet add AutoLot.Dal package Microsoft.EntityFrameworkCore.SqlServer
```

Додайте посилання на проект AutoLot.Models.
```console
dotnet add AutoLot.Dal reference AutoLot.Models
```
Якщо ви не використовуєте машину під керуванням Windows, налаштуйте роздільник каталогів для вашої операційної системи в попередніх командах.

Тепер можна вдкрити рішення в VS. Оновіть файл проекту AutoLot.Dal, щоб увімкнути доступ до моделі часу розробки під час виконання. Оновіть метадані для пакета Microsoft.EntityFrameworkCore.Design до наступного.

```xml
      <!--<IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>-->
```
Ця зміна необхідна для очищення тимчасових таблиць, розглянутих у розділі «Ініціалізація даних»


### Додайте представлення бази даних 

Якщо у вас немає бази даних AutoLot див. главу присвячену ADO.NET та Взаємодія з DBMS. Там вказано як відновити БД або створити з нуля.

Перш ніж робити риштування(scaffolding) сутностей і похідного DbContext з бази даних, додайте користувацьке представлення бази даних до бази даних AutoLot, яке використовуватиметься далі. Ми додаємо його зараз, щоб продемонструвати підтримку scaffolding для представлень. Підключіться до бази даних AutoLot (за допомогою SQL Server Management Studio або SQL Server Object Explorer) і виконайте такий оператор SQL.

```sql
CREATE VIEW [dbo].[CustomerOrderView]
AS
SELECT dbo.Customers.FirstName, dbo.Customers.LastName, dbo.Inventory.Color,
    dbo.Inventory.PetName, dbo.Makes.Name AS Make
FROM dbo.Orders
     INNER JOIN dbo.Customers ON dbo.Orders.CustomerId=dbo.Customers.Id
     INNER JOIN dbo.Inventory ON dbo.Orders.CarId=dbo.Inventory.Id
     INNER JOIN dbo.Makes ON dbo.Makes.Id=dbo.Inventory.MakeId;
```

# Створення каркасу(scaffolding) DbContext і Entities

Наступним кроком є створення похідного DbContext та сутностей бази даних AutoLot за допомогою інструментів EF Core CLI. Перейдіть до каталогу проекту AutoLot.Dal у командному рядку. Використовуйте інструменти EF Core CLI, щоб створити для базу даних AutoLot каркаси сутностей та похідний від DbContext клас за допомогою наступної команди, оновлюючи рядок підключення за потреби (всі в одному рядку):

```console
dotnet ef dbcontext scaffold "Server=(localdb)\mssqllocaldb;Database=AutoLot;Trusted_Connection=True;ConnectRetryCount=0" Microsoft.EntityFrameworkCore.SqlServer --data-annotations --context ApplicationDbContext --context-namespace AutoLot.Dal.EfStructures --context-dir EfStructures --no-onconfiguring --namespace AutoLot.Models.Entities --output-dir ..\AutoLot.Models\Entities --force
```

Попередня команда створює каркаси(scaffold) для бази даних, розташовану в наданому рядку підключення, використовуючи постачальника бази даних SQL Server. Прапор –data-annotations призначений для визначення пріоритетності анотацій даних, де це можливо (над Fluent API). --context називає контекст, --context-namespaces визначає простір імен для контексту, --context-dir вказує каталог (щодо поточного проекту) для створення контексту, --no-onconfiguring запобігає створення методу OnConfiguring, --output-dir є вихідним каталогом для сутностей (відносно каталогу проекту), а --namespace визначає простір імен для сутностей. Ця команда розміщує всі сутності в проекті AutoLot.Models у папці Entities, а клас ApplicationDbContext.cs — у папці EfStructures проекту AutoLot.Dal. Останній параметр (--force) використовується для примусового перезапису будь-яких існуючих файлів.
Команди EF Core CLI були детально розглянуті в розділі Entity Framework Core > Перший погляд.

## Розгляд результату scaffold.

Після виконання команди ви побачите шість сутностей у проекті AutoLot.Models (у папці Entities) і один похідний DbContext у проекті AutoLot.Dal (у папці EfStructures). Кожна таблиця риштується в клас сутності C# і додається як властивість DbSet<T> до похідного DbContext. Представлення риштується в сутності без ключа, додаються як DbSet<T> і зіставляються з відповідним представленням бази даних за допомогою Fluent API.
Команда scaffold, яку ми використовували, вказала прапорець --data-annotations, щоб надавати перевагу анотаціям над Fluent API. Вивчаючи створені класи, ви помітите, що в анотаціях є кілька промахів. Наприклад, властивості TimeStamp не мають атрибута [Timestamp], а натомість налаштовані як RowVersion ConcurrencyTokens у Fluent API.
Вважаю, наявність анотацій у класі робить код більш читабельним, ніж наявність усієї конфігурації у Fluent API. Якщо ви віддаєте перевагу використанню Fluent API, видаліть параметр –data-annotations із команди.

## Перемикання на Code First

Тепер, коли у вас є риштування база даних, в похідний DbContext і сутності, настав час переключитися з Database First на Code First. Процес не складний, але його не варто виконувати регулярно. Краще визначитися з парадигмою і дотримуватися її. Більшість гнучких команд віддають перевагу Code First, оскільки новий дизайн програми та її об’єктів перетікає в базу даних. Процес, який ми тут розглядаємо, імітує запуск нового проекту за допомогою EF Core, орієнтованого на існуючу базу даних.

Етапи, пов’язані з переходом, передбачають створення фабрики DbContext (для інструментів CLI), створення початкової міграції для поточного стану графа об’єктів, а потім видалення бази даних і повторне створення бази даних за допомогою або міграція або «підробки», застосована шляхом обману EF Core.

## Створення файлів GlobalUsings

У проектах AutoLot.Dal і AutoLot.Models додайте GlobalUsings.cs.

AutoLot.Dal/GlobalUsings.cs
```cs
global using System.Data;
global using System.Linq.Expressions;

global using Microsoft.Data.SqlClient;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.ChangeTracking;
global using Microsoft.EntityFrameworkCore.Design;
global using Microsoft.EntityFrameworkCore.Metadata;
global using Microsoft.EntityFrameworkCore.Migrations;
global using Microsoft.EntityFrameworkCore.Query;
global using Microsoft.EntityFrameworkCore.Storage;
global using Microsoft.Extensions.DependencyInjection;

global using AutoLot.Dal.EfStructures;

global using AutoLot.Models.Entities;
```
AutoLot.Models/GlobalUsings.cs
```cs
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using System.ComponentModel;
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Globalization;
global using System.Xml.Linq;

```


## Створення DbContext Design-Time Factory

Якшо ви виконаєте команду в рядку в проекті AutoLot.Dal вона не спрацює і вкаже причину.
```console
dotnet ef dbcontext info
```

Як ви пам’ятаєте з попередніх розділів EF Core, IDesignTimeDbContextFactory використовується інструментом CLI EF Core для створення екземпляра похідного класу DbContext. 

Створіть новий файл класу під назвою ApplicationDbContextFactory.cs у проекті AutoLot.Dal у каталозі EfStructures. Деталі фабрики були розглянуті в попередньому розділі, тому я просто збираюся перелічити код тут. Обов’язково оновіть рядок підключення відповідно до середовища.

```cs
namespace AutoLot.Dal.EfStructures;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[]? args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        string connectionString = @"Server=(localdb)\mssqllocaldb;Database=AutoLot;Trusted_Connection=True;ConnectRetryCount=0";
        optionsBuilder.UseSqlServer(connectionString);
        Console.WriteLine(connectionString);
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
```

Після цого команда в командному рядку dbcontext info покаже дані що до класу контексту.

## Створення початкової міграцію

Під час першої міграції буде створено три файли: два файли для самої міграції та повний знімок моделі. Введіть наступне в командному рядку (у каталозі AutoLot.Dal), щоб створити нову міграцію з іменем Initial (використовуючи екземпляр ApplicationDbContext), і розмістіть файли міграції в папці EfStructures\Migrations проекту AutoLot.Dal:

```console
dotnet ef migrations add Initial -o EfStructures\Migrations
```
Важливо переконатися, що жодних змін не застосовано до риштованих файлів або бази даних, доки не буде створено та застосовано цю першу міграцію. Зміни з будь-якої сторони призведуть до розсинхронізації коду та бази даних. Після застосування всі зміни в базі даних потрібно завершити за допомогою міграції EF Core.

Щоб підтвердити, що міграцію створено та очікує на застосування, виконайте команду list.

```console
dotnet ef migrations list
Build started...
Build succeeded.
Server=(localdb)\mssqllocaldb;Database=AutoLot;Trusted_Connection=True;ConnectRetryCount=0
20241125090346_Initial (Pending)
```
Результат покаже, що початкова міграція очікує на розгляд. Рядок підключення відображається у вихідних даних завдяки Console.Writeline() у методі CreateDbContext().

## Застосування міграції

Найпростіший спосіб застосувати міграцію до бази даних — видалити базу даних і створити її заново. Для цього можна ввести такі команди:

```console 
dotnet ef database drop -f
dotnet ef database update
```
Якщо видалити та повторно створити базу даних неможливо (наприклад, це база даних Azure SQL), тоді EF Core має переконатися, що міграцію застосовано. На щастя, це просто, оскільки EF Core виконує більшу частину роботи. Почніть із створення сценарію SQL із міграції за допомогою такої команди:

```console
dotnet ef migrations script --idempotent -o FirstMigration.sql
```       
В створеному скріпті все необхідне для застосування міграцвї на стороні сервера БД.
Відповідні частини цього сценарію створюють таблицю __EFMigrationsHistory, а потім додають запис міграції в таблицю, щоб вказати, що її було застосовано. Скопіюйте ці фрагменти до нового запиту в Azure Data Studio або SQL Server Manager Studio.

Тепер, якщо ви запустите команду migrations list, вона більше не відображатиме початкову міграцію як незавершену. Після застосування початкової міграції проект і база даних синхронізуються, і розробка може продовжувати в парадигмі Сode First. Перш ніж продовжити розробку бази даних, потрібно створити спеціальні винятки проекту. 

## Створення власних винятків

Загальною схемою обробки винятків є перехоплення системних винятків (та/або винятків EF Core, як у цьому прикладі), реєстрування винятків, а потім створення спеціального винятку. 
Створіть новий каталог із назвою Exceptions у проекті AutoLot.Dal. У цьому каталозі створіть чотири нових класи:

```cs
namespace AutoLot.Dal.Exceptions;

public class CustomException : Exception
{
    public CustomException(){}
    public CustomException(string? message) : base(message){}
    public CustomException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
```
```cs
namespace AutoLot.Dal.Exceptions;

public class CustomConcurrencyException : CustomException
{
    public CustomConcurrencyException(){}
    public CustomConcurrencyException(string? message) : base(message){}
    public CustomConcurrencyException(string? message, DbUpdateConcurrencyException? innerException) : base(message, innerException)
    {
    }
}
```
```cs
namespace AutoLot.Dal.Exceptions;

public class CustomDbUpdateException : CustomException
{
    public CustomDbUpdateException() {}

    public CustomDbUpdateException(string? message) : base(message) {}

    public CustomDbUpdateException(string? message, DbUpdateException? innerException) : base(message, innerException)
    {
    }
}
```
```cs
namespace AutoLot.Dal.Exceptions;

public class CustomRetryLimitExceededException : CustomException
{
    public CustomRetryLimitExceededException() {}

    public CustomRetryLimitExceededException(string? message) : base(message) {}

    public CustomRetryLimitExceededException(string? message, RetryLimitExceededException? innerException) : base(message, innerException)
    {
    }
}
```
Якщо настроюваний виняток перехоплюється в попередньому методі, розробник знає, що виняток уже було зареєстровано, і йому просто потрібно відповідним чином відреагувати на виняток у поточному блоці коду.

Додайте простір імен до GlobalUsings цього проекту

```cs
global using AutoLot.Dal.Exceptions;
```


# Доопрацювання сутностей(Entities) та моделей перегляду(ViewModel)

Цей розділ оновлює риштовані сутності до їх остаточної версії, додає додаткові сутності з попередніх двох розділів і додає сутність журналювання(logging entity).

## Сутності

У каталозі Entities проекту AutoLot.Models ви знайдете шість файлів, по одному для кожної таблиці в базі даних і один для перегляду бази даних. Зауважте, що назви є одниною, а не множиною (як у базі даних). Як випливає з назви, засіб множинності відображає однину імен сутностей у множину імен таблиць і навпаки. У попередніх розділах детально розглядалися угоди EF Core, анотації даних і Fluent API, тому більшу частину цього розділу складатимуть списки кодів із короткими описами.

## Клас BaseEntity

Клас BaseEntity буде містити стовпці Id і TimeStamp, які є в кожній сутності. Створіть новий каталог під назвою Base у каталозі Entities проекту AutoLot.Models. У цьому каталозі створіть новий файл під назвою BaseEntity.cs

```cs
namespace AutoLot.Models.Entities.Base;
public abstract class BaseEntity
{
    [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    // [Timestamp]
    // public byte[] TimeStamp { get; set; }
}
```
Усі сутності (крім сутності журналювання) буде оновлено для використання цього базового класу в наступних розділах. Властивість Timestamp покищо закоментовано тому що при застосувані міграції виконається помилка оскільки в таблицях вже є такий стовпеці з іншим типом і зміна бази даних виконається з помилкою. Коли всі сутності оновляться і буде виделено попередні стовпці ми дадамо стовпець до всіх сутностей.

Додайте простір імен до GlobalUsings обох проектів.

```cs
global using AutoLot.Models.Entities.Base;
```

## Owned сутність Person

Усі сутності Customer, CreditRisk і Driver мають властивості FirstName і LastName. Сутності, які мають однакові властивості в кожному, можуть отримати вигоду від Owned класу. Хоча переміщення цих двох властивостей у Owned клас є дещо тривіальним прикладом, Owned сутності допомагають зменшити дублювання коду та підвищити узгодженість.

Створіть новий каталог під назвою Owned у каталозі Entities проекту AutoLot.Models.У цьому новому каталозі створіть новий файл під назвою Person.cs.
```cs
namespace AutoLot.Models.Entities.Owned;

[Owned]
public class Person
{
    [Required, StringLength(50)]
    public string FirstName { get; set; }
    [Required, StringLength(50)]
    public string LastName { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public string FullName { get; set; }
}
```
Зверніть увагу на додатковий обчислений стовпець, який об’єднав імена у властивість FullName. Owned класи налаштовуються як частина класів-власників, тому конфігурація для зіставлення імен стовпців і обчисленого стовпця відбувається як частина конфігурації Customer, CreditRisk і Driver.

Додайте простір імен до GlobalUsings цього проекту.

```cs
global using AutoLot.Models.Entities.Owned;
```

## Сутності 

Таблицю Inventory було риштовано в клас сутності під назвою Inventory. Ми збираємося змінити ім’я сутності на Car, залишивши назву таблиці. Це приклад зіставлення сутності з таблицею з іншою назвою. Це легко виправити: змініть ім’я файлу на Car.cs і ім’я класу на Car. Атрибут Table уже застосовано правильно, тому можна просто додайте схему dbo. Зауважте, що параметр схеми є необов’язковим, оскільки SQL Server за замовчуванням має dbo, але я включив його для повноти. Простори імен також можна видалити, оскільки вони охоплюються глобальними просторами імен.

Далі успадкуйте від BaseEntity та видаліть властивості Id (і його атрибут) і TimeStamp.

```cs
[Table("Inventory")]
[Index("MakeId", Name = "IX_Inventory_MakeId")]
public partial class Car : BaseEntity
{
    public int MakeId { get; set; }

    [StringLength(50)]
    public string Color { get; set; } = null!;

    [StringLength(50)]
    public string PetName { get; set; } = null!;

    [ForeignKey("MakeId")]
    [InverseProperty("Inventories")]
    public virtual Make Make { get; set; } = null!;

    [InverseProperty("Car")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
```
Додайте атрибут DisplayName до властивості PetName, додайте властивість Display з атрибутом DatabaseGenerated, щоб зберігати обчислене значення з SQL Server, і додайте властивості Price і DateBuilt.

```cs
    [Required]
    [StringLength(50)]
    [DisplayName("Pet Name")]
    public string PetName { get; set; } = null!;
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public string Display { get; set; }
    public string? Price { get; set; }
    public DateTime? DateBuilt { get; set; }
```
Атрибут DisplayName використовується ASP.NET Core і буде розглянуто в цьому розділі.

Перш ніж міняти властивість навігації Make відкриемо цей клас і змінемо властивість.

```cs
    //public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
```


Властивість навігації Make потрібно перейменувати на MakeNavigation, а InverseProperty використовує магічний рядок замість методу C# nameof().

```cs
    [ForeignKey(nameof(MakeId))]
    [InverseProperty(nameof(Make.Cars))]
    public virtual Make MakeNavigation { get; set; } = null!;
```
Це яскравий приклад того, чому назвати властивість так само, як ім’я класу, стає проблематично. Якщо назву властивості залишити Make, то функція nameof не працюватиме належним чином, оскільки Make (у цьому випадку) посилається на властивість, а не на тип.

Компілятор може показати помилку в іншому класі ми їх виправимо після цього класу.

Риштованний код для властивості навігації Orders потребує оновлення, оскільки всі властивості навігації посилань матимуть суфікс Navigation, доданий до їхніх імен. 

```cs
    [InverseProperty(nameof(Order.CarNavigation))]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
```
Далі додайте властивість NotMapped, яка відображатиме значення Make автомобіля. Якщо пов’язану інформацію про марку було отримано з бази даних разом із записом автомобіля, відобразиться ім’я марки. Якщо відповідні дані не було отримано, властивість відображає Unknown. Нагадуємо, що властивості NotMapped не є частиною бази даних і існують лише в сутності.

```cs
    [NotMapped]
    public string MakeName => MakeNavigation?.Name ?? "Unknown";
```

Додайте перевизначення для методу ToString() для відображення інформації про транспортний засіб.
```cs
    public override string? ToString()
    {
        return $"{Id}\t{PetName ?? "No name"}\t{Color}\t{MakeNavigation?.Name}";
    }
```

Додайте атрибути Required і DisplayName до MakeId. Незважаючи на те, що EF Core вважає властивість MakeId необхідною, оскільки вона не допускає значення null, я завжди додаю її для зручності читання та підтримки інтерфейсу користувача.

```cs
    [Required]
    [DisplayName("Make")]
    public int MakeId { get; set; }
```

Наступна зміна полягає в додаванні властивості bool IsDrivable, що не допускає нульових значень, із резервним полем, що допускає нульові значення, і відображуваним ім’ям.

```cs
    private bool? _isDrivable;

    [Required]
    [DisplayName("Is Drivable")]
    public bool IsDrivable 
    {
        get => _isDrivable ?? true; 
        set => _isDrivable = value; 
    }
```
Оскільки клас Inventory було перейменовано на Car, клас ApplicationDbContext потрібно оновити. Знайдіть властивість DbSet<Inventory>  Inventories  і оновіть рядок до такого:

```cs
    public virtual DbSet<Car> Cars { get; set; }
```
Крім того компілятор покаже ше помикли де траба поміняти назви Invertory та посилання. Треба помінять назви в інших класах.

Також в класі Make

```cs
    [InverseProperty(nameof(Car.MakeNavigation))]
    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
```
В класі Order

```cs
    public virtual Car CarNavigation { get; set; } = null!;
```
Можна виправити інші помилки, але вони будуть виправлені в файлах конфігурації.

Створимо клас конфігурації для сутності.

Як і в попередньому розділі, ми будемо використовувати IEntityTypeConfiguration<T> для зберігання коду Fluent API. Це зберігає конфігурацію для кожної сутності у власному класі, значно зменшуючи розмір методу ApplicationDbContext OnModelCreating(). Почніть із створення нового каталогу під назвою Configuration у каталозі Entities. У цьому новому каталозі додайте новий файл під назвою CarConfiguration.cs, зробіть його загальнодоступним і реалізуйте інтерфейс IEntityTypeConfiguration<Car>:

```cs
namespace AutoLot.Models.Entities.Configuration;

public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        throw new NotImplementedException();
    }
}
```

Далі перемістіть вміст конфігурації для сутності Car (зверніть увагу, що вона може називатися Inventory) із методу OnModelCreating() у ApplicationDbContext у метод Configure() класу CarConfiguration. Скаффолдований код Fluent API для кожної сутності загорнутий у конструкцію, подібну до наведеної нижче:

```cs
modelBuilder.Entity<Car>(entity =>
{
  //Fluent API code here
});
```
Кожен IEntityTypeConfiguration<T> суворо типізований для сутності, тому для кожної сутності не потрібен зовнішній код, а лише внутрішній код скаффолду. Перемістіть увесь блок, а потім видаліть специфікатор сутності. Замініть змінну entity в кожному з блоків внутрішнього коду на змінну builder, а потім додайте додатковий код Fluent API:

```cs
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.HasOne(d => d.MakeNavigation)
            .WithMany(p => p.Cars)
            .HasForeignKey(c=>c.MakeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Inventory_Makes_MakeId");
        
        builder.HasQueryFilter(c => c.IsDrivable);
        builder.Property(p => p.IsDrivable)
            .HasField("_isDrivable")
            .HasDefaultValue(true);

        builder.Property(e => e.DateBuilt).HasDefaultValueSql("getdate()");

        builder.Property(e => e.Display)
        .HasComputedColumnSql("[PetName] + ' (' + [Color] + ')'", stored: true);

        builder.Property(p => p.Price).HasConversion(new StringToNumberConverter<decimal>());
    }
```
Таблицю Inventory буде налаштовано як часову таблицю, тому додайте наступне до методу Configure()

```cs
        builder.ToTable(b => b.IsTemporal(t =>
        {
            t.UseHistoryTable("InventoryAudit");
        }));
```
Переконайтеся, що весь код у методі OnModelBuilding() (у класі ApplicationDbContext.cs), який налаштовує клас Inventory, видалено, і додайте на його місце такий єдиний рядок коду:

```cs
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    new CarConfiguration().Configure(modelBuilder.Entity<Car>());

    //...
}
```

У класі Car додайте атрибут EntityTypeConfiguration

```cs
[Table("Inventory")]
[Index("MakeId", Name = "IX_Inventory_MakeId")]
[EntityTypeConfiguration(typeof(CarConfiguration))]
public partial class Car : BaseEntity
{
    //...
}    
```
Це ше не всі зміни cутності Car, але можно виконати першу міграцію стосовно цього класу.

Додамо постір імен в GlobalUsing.cs цього проекту
```cs
global using AutoLot.Models.Entities.Configuration;
```

```console
dotnet ef migrations add ChangeCar
dotnet ef database update
```

## Проект простих тестів

Додамо до рішеня проект типу ConsoleApp з назвою SimpleTest з посиланням на проект AutoLot.Dal.

Перевіримо роботу з БД.

SimpleTest\Program.cs
```cs
static void Test_Make_Car()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    // Create data

    Make make = new Make() { Name = "VW" };
    context.Makes.Add(make);

    Car car = new() { MakeNavigation = make, Color = "Grey", PetName = "Wolf" };
    context.Cars.Add(car);

    context.SaveChanges();

    // Read data

    var makes = context.Makes;
    foreach (var make1 in makes)
    {
        Console.WriteLine($"{make1.Id} {make1.Name}");
    }

    var cars = context.Cars;
    foreach (var car1 in cars)
    {
        Console.WriteLine(car1);
    }
}
Test_Make_Car();
```
```console
1 VW
1       Wolf    Grey    VW
```

## Сутність Driver , CarDriver та відношеня Many-To-Many між Car і Driver

## Сутність Driver

В БД немає таблиці як зберігає дани про водіїв. Оскільки цієї таблиці не було в базі даних, її немає в нашому риштованому коді. Додайте новий файл із іменем Driver.cs до папки Entities:

```cs
namespace AutoLot.Models.Entities;

public class Driver : BaseEntity
{
    public Person PersonInformation { get; set; } = new Person();

}
```
Оскільки це нова таблиця, нову властивість DbSet<Driver> необхідно додати до класу ApplicationDbContext.

```cs
    public virtual DbSet<Driver> Drivers { get; set; }
```
Далі створемо файл конфігурації. Додайте новий файл із назвою DriverConfiguration.cs у папку Configuration

```cs
namespace AutoLot.Models.Entities.Configuration;

public class DriverConfiguration : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.OwnsOne(o => o.PersonInformation,
            pd =>
            {
                pd.Property<string>(nameof(Person.FirstName))
                    .HasColumnName(nameof(Person.FirstName))
                    .HasColumnType("nvarchar(50)");
                pd.Property<string>(nameof(Person.LastName))
                    .HasColumnName(nameof(Person.LastName))
                    .HasColumnType("nvarchar(50)");
                pd.Property(p => p.FullName)
                     .HasColumnName(nameof(Person.FullName))
                     .HasComputedColumnSql("[LastName] + ', ' + [FirstName]");
            });
        builder.Navigation(d => d.PersonInformation).IsRequired(true);
    }
}
```
У класі Drive додайте атрибут EntityTypeConfiguration

```cs
[EntityTypeConfiguration(typeof(DriverConfiguration))]
public class Driver : BaseEntity
{
 //...
}
```
Сутність Driver використовує властивість, що належить Person, тому її не можна налаштувати як часову таблицю.

Оновіть метод ApplicationDbContext OnModelCreating().
```cs
new DriverConfiguration().Configure(modelBuilder.Entity<Driver>());
```

## Сутність CarDriver

Для налаштування зв’язку «Many-To-Many» між Car і Driver, додайте новий клас під назвою CarDriver.

```cs

namespace AutoLot.Models.Entities;

[Table("InventoryToDrivers")]
public class CarDriver : BaseEntity
{
    public int DriverId { get; set; }
    [ForeignKey(nameof(DriverId))]
    public virtual Driver DriverNavigation { get; set; } = null!;
    [Column("InventoryId")]
    public int CarId { get; set; }
    [ForeignKey(nameof(CarId))]
    public Car CarNavigation { get; set; } = null!;
}
```
Оскільки це нова таблиця, нову властивість DbSet<CarDriver> необхідно додати до класу ApplicationDbContext..

```cs
        public virtual DbSet<CarDriver> CarsToDrivers { get; set; }
```
Далі створемо файл конфігурації. Через фільтр запиту для Car що IsDrivable у класі Car пов’язані таблиці (CarDriver і Order) повинні мати однаковий фільтр запиту, застосований до їхніх властивостей навігації. Додайте новий файл із назвою CarDriverConfiguration.cs у папку Configuration та оновіть код до такого:

```cs
namespace AutoLot.Models.Entities.Configuration;

public class CarDriverConfiguration : IEntityTypeConfiguration<CarDriver>
{
    public void Configure(EntityTypeBuilder<CarDriver> builder)
    {
        builder.HasQueryFilter(cd => cd.CarNavigation.IsDrivable);
    }
}
```
Таблицю InventoryToDrivers буде налаштовано як часову таблицю, тому додайте наступне до методу Configure()

```cs
    public void Configure(EntityTypeBuilder<CarDriver> builder)
    {
        //...

        builder.ToTable(b => b.IsTemporal(t =>
        {
            t.UseHistoryTable("InventoryToDriversAudit");
        }));
    }
```
У класі CarDrive додайте атрибут EntityTypeConfiguration

```cs

[EntityTypeConfiguration(typeof(CarDriverConfiguration))]
public class CarDriver :BaseEntity
{
    ///...
}
```
Оновіть метод ApplicationDbContext OnModelCreating().
```cs
        new CarDriverConfiguration().Configure(modelBuilder.Entity<CarDriver>());
```

## Створення ввідношеня Many-To-Many між Car і Driver

В класі Driver додамо властивість Cars.

```cs
    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
```

В клас Car додайте властивість навігації колекції для сутності Driver та CarDriver:
```cs
[InverseProperty(nameof(Driver.Cars))]
public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();

[InverseProperty(nameof(CarDriver.CarNavigation))]
public virtual ICollection<CarDriver> CarDrivers { get; set; } = new List<CarDriver>();
```
В класі Driver додайте зміни.

```cs
    [InverseProperty(nameof(Car.Drivers))]
    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();

    [InverseProperty(nameof(CarDriver.DriverNavigation))]
    public ICollection<CarDriver> CarDrivers { get; set; } = new List<CarDriver>();
```

В класі CarConfiuration в методі Configure додамо визначеня відносин.
```cs
        builder.HasMany(p => p.Drivers).WithMany(p => p.Cars)
        .UsingEntity<CarDriver>(
          j => j
              .HasOne(cd => cd.DriverNavigation)
              .WithMany(d => d.CarDrivers)
              .HasForeignKey(nameof(CarDriver.DriverId))
              .HasConstraintName("FK_InventoryDriver_Drivers_DriverId")
              .OnDelete(DeleteBehavior.Cascade),
          j => j
              .HasOne(cd => cd.CarNavigation)
              .WithMany(c => c.CarDrivers)
              .HasForeignKey(nameof(CarDriver.CarId))
              .HasConstraintName("FK_InventoryDriver_Inventory_InventoryId")
              .OnDelete(DeleteBehavior.ClientCascade),
          j =>
          {
              j.HasKey(cd => new { cd.CarId, cd.DriverId });
          });
```
Запишимо всі зміни (Ctrl+Shift+S).

Перивіримо зміни в проекті SimpleTest
Додамо міграцію і оновимо базу

```console
dotnet ef migrations add AddCarDriverManyToMany
dotnet ef database update
```
Перший метод додає запис Car, Driver та зв'язок між ними а другий потім завантажує їх з БД.

```cs
static int Test_Car_Driver_Create()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    Make make = new Make() { Name = "VW" };
    context.Makes.Add(make);
    Car? car = new() { MakeNavigation = make, Color = "Black", PetName = "Wolf" };
    context.Cars.Add(car);

    Driver driver = new Driver()
    {
        PersonInformation =
        new Person { FirstName = "Sara", LastName = "Conor" }
    };

    ((List<Driver>)car.Drivers).Add(driver);
    context.SaveChanges();

    Console.WriteLine(car);
    Console.WriteLine($"{driver.Id} {driver.PersonInformation.FullName}");

    return car.Id;
}

static void Test_Car_Driver()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    int id = Test_Car_Driver_Create();

    Car car = context.Cars.Include(c=>c.MakeNavigation)
        .Include(c => c.CarDrivers)
        .ThenInclude(cd => cd.DriverNavigation)
        .Where(c => c.Id == id)
        .Single();
        
    Driver? driver = car.Drivers.First();

    Console.WriteLine(car);
    Console.WriteLine($"{driver.Id} {driver.PersonInformation.FullName}");
}
Test_Car_Driver();
```
```console
2       Wolf    Black   VW
1 Conor, Sara
2       Wolf    Black   VW
1 Conor, Sara
```

## Сутність Radio та відношення One-To-One між Car і Radio

Додайте новий клас із назвою Radio.cs до папки Entities:

```cs
namespace AutoLot.Models.Entities;

public class Radio : BaseEntity
{
    public bool HasTweeters { get; set; }
    public bool HasSubWoofers { get; set; }
    [Required, StringLength(50)]
    public string RadioId { get; set; } = string.Empty;
    [Column("InventoryId")]
    public int CarId { get; set; }
    [ForeignKey(nameof(CarId))]
    public virtual Car CarNavigation { get; set; } = null!;
}
```
Додайте нову властивість до класу ApplicationDbContext

```cs
    public virtual DbSet<Radio> Radios { get; set; }
```

## Створення відношення One-To-One між Car і Radio 

У клас Car додайте властивість навігації посилання для сутності Radio:
```cs
    [InverseProperty(nameof(Radio.CarNavigation))]
    public virtual Radio RadioNavigation { get; set; } = null!;
```

Далі створимо клас конфігурації.

```cs
namespace AutoLot.Models.Entities.Configuration;

public class RadioConfiguration : IEntityTypeConfiguration<Radio>
{
    public void Configure(EntityTypeBuilder<Radio> builder)
    {
        builder.HasQueryFilter(r => r.CarNavigation.IsDrivable);
        builder.HasIndex(r => r.CarId, "IX_Radios_CarId")
            .IsUnique();
        builder.HasOne(r => r.CarNavigation)
            .WithOne(c => c.RadioNavigation)
            .HasForeignKey<Radio>(r => r.CarId);
    }
}
```
Таблиця Radios буде налаштована як часова таблиця, тому додайте наступне до методу Configure():

```cs
        builder.ToTable(tb => tb.IsTemporal(t =>
        {
            t.UseHistoryTable("RadiosAudit");
        }));
```
У класі Radio додайте атрибут EntityTypeConfiguration
```cs
[EntityTypeConfiguration(typeof(RadioConfiguration))]
public class Radio : BaseEntity
{
    //...
}
```
Оновіть метод OnModelCreating() у ApplicationDbContext
```cs
        new RadioConfiguration().Configure(modelBuilder.Entity<Radio>());
```
Перевіримо додану сутність.

```console
dotnet ef migrations add AddRadioOneToOne
dotnet ef database update
```
SimpleTest\Program.cs
```cs
static void Test_Car_Radio()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    // Create
    Make make = new() { Name = "VW" };
    context.Makes.Add(make);
    Car car = new() { MakeNavigation = make, Color = "Black", PetName = "Panter" };

    car.RadioNavigation = new Radio
    {
        HasTweeters = true,
        HasSubWoofers = true,
        RadioId = "RDV23451",
    }; 
    context.Cars.Add(car);
    context.SaveChanges();

    Car car_1 = context.Cars
        .Include(c => c.RadioNavigation)
        .Where(c => c.Id == car.Id)
        .Single();

    var radio = car_1.RadioNavigation;
    Console.WriteLine($"{radio.Id} {radio.RadioId}");
    Console.WriteLine(radio.CarNavigation);
}
Test_Car_Radio();
```
```console
1 RDV23451
3       Panter  Black   VW
```

## Сутність Customer

Таблицю Customers було риштовано в клас сутності під назвою Customer. Успадкуйте від BaseEntity та видаліть властивості Id і TimeStamp. Видаліть властивості FirstName і LastName, оскільки їх буде замінено Owned сутністю Person.

```cs
namespace AutoLot.Models.Entities;

public partial class Customer : BaseEntity
{
    public Person PersonInformation { get; set; } = new Person();

    [InverseProperty("Customer")]
    public virtual ICollection<CreditRisk> CreditRisks { get; set; } = new List<CreditRisk>();

    [InverseProperty("Customer")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
```
Виправимо навігаційну властивість в класах CreditRisk і Order

```cs
    public virtual Customer CustomerNavigation { get; set; } = null!;
```
Атрибути зворотної властивості потрібно оновити за допомогою суфікса навігації.

```cs
namespace AutoLot.Models.Entities;

public partial class Customer : BaseEntity
{
    public Person PersonInformation { get; set; } = new Person();

    [InverseProperty(nameof(CreditRisk.CustomerNavigation))]
    public virtual ICollection<CreditRisk> CreditRisks { get; set; } = new List<CreditRisk>();

    [InverseProperty(nameof(Order.CustomerNavigation))]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
```

Створимо файл конфігурації.

```cs
namespace AutoLot.Models.Entities.Configuration;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.OwnsOne(o => o.PersonInformation,
            pd =>
            {
                pd.Property<string>(nameof(Person.FirstName))
                    .HasColumnName(nameof(Person.FirstName))
                    .HasColumnType("nvarchar(50)");
                pd.Property<string>(nameof(Person.LastName))
                    .HasColumnName(nameof(Person.LastName))
                    .HasColumnType("nvarchar(50)");
                pd.Property(p => p.FullName)
                   .HasColumnName(nameof(Person.FullName))
                   .HasComputedColumnSql("[LastName] + ', ' + [FirstName]");

            });
        builder.Navigation(d => d.PersonInformation).IsRequired(true);
    }
}
```

У класі Customer додайте атрибут EntityTypeConfiguration
```cs

[EntityTypeConfiguration(typeof(CustomerConfiguration))]
public partial class Customer : BaseEntity
{
    //...
}
```
Видаліть код конфігурації в методі ApplicationDbContext OnModelCreating() і додайте рядок конфігурації.

```cs
        new CustomerConfiguration().Configure(modelBuilder.Entity<Customer>());
```
Виправіть лямбда-вирази з навігаційними посиланнями.

Збережіть зміни.

```console
dotnet ef migrations add ChangeCustomer
dotnet ef database update
```

Перевіримо  сутність.

```cs
static void Test_Customer()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    Customer customer = new Customer()
    {
        PersonInformation = new Person { FirstName = "Tommy", LastName = "Stark" }
    };

    context.Customers.Add(customer);
    context.SaveChanges();

    Customer customer_1 = context.Customers.Single(c=>c.Id == customer.Id);
    Console.WriteLine($"{customer_1.Id} {customer_1.PersonInformation.FullName}");
}
Test_Customer();
```
```console
1 Stark, Tommy
```

## Сутність Make

Таблиця Makes була створена для класу сутності під назвою Make. Успадкуйте від BaseEntity та видаліть властивості Id і TimeStamp.

```cs
namespace AutoLot.Models.Entities;

public partial class Make :BaseEntity
{

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [InverseProperty(nameof(Car.MakeNavigation))]
    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
```
Створимо файл конфігурації. Таблицю Makes буде налаштовано як часову таблицю.

```cs
namespace AutoLot.Models.Entities.Configuration;

public class MakeConfiguration : IEntityTypeConfiguration<Make>
{
    public void Configure(EntityTypeBuilder<Make> builder)
    {
        builder.ToTable(tb => tb.IsTemporal(t =>
        {
            t.UseHistoryTable("MakesAudit");
        }));
    }
}
```
У класі Make додайте атрибут EntityTypeConfiguration.

```cs
[EntityTypeConfiguration(typeof(MakeConfiguration))]
```

Видаліть риштовану конфігурацію для сутності Make з методу ApplicationDbContext. Оновіть метод OnModelCreating():

```cs
        new MakeConfiguration().Configure(modelBuilder.Entity<Make>());
```
Збережіть зміни.

Зробимо зміни БД.

```cs
dotnet ef migrations add ChangeMake
dotnet ef database update
```
Перивіримо сутність.

```cs
static void Test_Make()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    Make make = new Make { Name = "Toyota" };
    context.Makes.Add(make);
    context.SaveChanges();

    Make make_1 = context.Makes.Single(m => m.Id == make.Id);
    Console.WriteLine($"{make_1.Id} {make_1.Name}");
}
Test_Make();
```
```console
5 Toyota
```
Також можна запустити попередьно зроблений метод Test_Make_Car


## Сутність CreditRisk

Таблицю CreditRisks було риштовано в клас сутності під назвою CreditRisk. Успадкуйте від BaseEntity та видаліть властивості Id і TimeStamp. Видаліть властивості FirstName і LastName. Виправте властивість навігації за допомогою методу nameof() в атрибуті InverseProperty. Додати Owned власність. Відносини будуть далі налаштовані в API Fluent.

```cs
namespace AutoLot.Models.Entities;

[Index("CustomerId", Name = "IX_CreditRisks_CustomerId")]
public partial class CreditRisk :BaseEntity
{
    public Person PersonInformation { get; set; } = new Person();
    public int CustomerId { get; set; }

    [ForeignKey(nameof(CustomerId))]
    [InverseProperty(nameof(Customer.CreditRisks))]
    public virtual Customer CustomerNavigation { get; set; } = null!;
}
```
Як обговорювалося під час представлення таблиці CreditRisk, наявність класу, що належить людині, і властивості навігації до таблиці Customer здається дивним дизайном, і насправді це так. Усі ці таблиці створено, щоб навчати різному аспекту EF Core, і це нічим не відрізняється. Розгляньте додаткові Ім’я/Прізвище як місце для розміщення псевдоніма некредитоспроможної особи.

Створимо файл конфігурації.

```cs
namespace AutoLot.Models.Entities.Configuration;

public class CreditRiskConfiguration : IEntityTypeConfiguration<CreditRisk>
{
    public void Configure(EntityTypeBuilder<CreditRisk> builder)
    {
        builder.HasOne(d => d.CustomerNavigation)
            .WithMany(p => p.CreditRisks)
            .HasForeignKey(d => d.CustomerId)
            .HasConstraintName("FK_CreditRisks_Customers");

        builder.OwnsOne(o => o.PersonInformation,
            pd =>
            {
                pd.Property<string>(nameof(Person.FirstName))
                    .HasColumnName(nameof(Person.FirstName))
                    .HasColumnType("nvarchar(50)");
                pd.Property<string>(nameof(Person.LastName))
                    .HasColumnName(nameof(Person.LastName))
                    .HasColumnType("nvarchar(50)");
                pd.Property(p => p.FullName)
                     .HasColumnName(nameof(Person.FullName))
                     .HasComputedColumnSql("[LastName] + ', ' + [FirstName]");
            });
        builder.Navigation(d => d.PersonInformation).IsRequired(true);
    }
}
```
У класі CreditRisk додайте атрибут EntityTypeConfiguration.
```cs
[EntityTypeConfiguration(typeof(CreditRiskConfiguration))]
```
Оновіть метод ApplicationDbContext OnModelCreating(), видаливши конфігурацію CreditRisk і додавши рядок для класу CreditRiskConfiguration:
```cs
        new CreditRiskConfiguration().Configure(modelBuilder.Entity<CreditRisk>());
```

Оновимо БД.

```console
dotnet ef migrations add ChangeCreditRisk
dotnet ef database update
```

Перевіримо в роботі.
```cs
static void Test_CreditRisk()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    Customer customer = new Customer()
    {
        PersonInformation = new Person { FirstName = "James", LastName = "Bond" }
    };

    CreditRisk creditRisk = new CreditRisk
    {
        PersonInformation = new Person 
        { 
            FirstName = customer.PersonInformation.FirstName,
            LastName = customer.PersonInformation.LastName            
        },
        CustomerNavigation = customer
    };

    context.CreditRisks.Add(creditRisk);
    context.SaveChanges();

    CreditRisk? creditRisk1 = context.CreditRisks.Find(creditRisk.Id);
    Console.WriteLine($"" +
        $"{creditRisk1?.Id} " +
        $"{creditRisk1?.PersonInformation.FirstName} " +
        $"{creditRisk1?.PersonInformation.LastName}");
}
Test_CreditRisk();
```
```console
1 James Bond
```

## Сутність Order

Таблицю Orders було риштовано в клас сутності під назвою Order. Успадкуйте від BaseEntity та видаліть властивості Id і TimeStamp. Змінить параметри для ForeignKey та InverseProperty використавши nameof

```cs
namespace AutoLot.Models.Entities;

[Index("CarId", Name = "IX_Orders_CarId")]
[Index("CustomerId", "CarId", Name = "IX_Orders_CustomerId_CarId", IsUnique = true)]
public partial class Order :BaseEntity
{
    public int CustomerId { get; set; }
    public int CarId { get; set; }
    [ForeignKey(nameof(CarId))]
    [InverseProperty(nameof(Car.Orders))]
    public virtual Car CarNavigation { get; set; } = null!;

    [ForeignKey(nameof(CustomerId))]
    [InverseProperty(nameof(Customer.Orders))]
    public virtual Customer CustomerNavigation { get; set; } = null!;
}
```
Створимо файл конфігурації. В конфігурації вказані зв'язки і те шо таблиця буде часовою.

```cs
namespace AutoLot.Models.Entities.Configuration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasIndex(o => new { o.CustomerId, o.CarId })
            .IsUnique(true);
        builder.HasQueryFilter(o => o.CarNavigation.IsDrivable);
        builder.HasOne(d => d.CarNavigation)
            .WithMany(p => p.Orders)
            .HasForeignKey(d => d.CarId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Orders_Inventory");

        builder.HasOne(d => d.CustomerNavigation)
            .WithMany(p => p.Orders)
            .HasForeignKey(d => d.CustomerId)
            .HasConstraintName("FK_Orders_Customers");

        builder.ToTable(b => b.IsTemporal(t =>
        {
            t.UseHistoryTable("OrdersAudit");
        }));
    }
}
```
У класі Order додайте атрибут EntityTypeConfiguration.

```cs
[EntityTypeConfiguration(typeof(OrderConfiguration))]
```
Оновіть метод ApplicationDbContext OnModelCreating(), видаливши конфігурацію Order.

```cs
        new OrderConfiguration().Configure(modelBuilder.Entity<Order>());
```

Оновимо БД і перевіримо її.

```console
dotnet ef migrations add ChangeOrder
dotnet ef database update
```

```cs
static void Test_Order()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    Customer customer = new Customer()
    {
        PersonInformation = new Person { FirstName = "Lara", LastName = "Croft" }
    };

    Make make = new Make() { Name = "Peugeot" };
    Car car = new() { MakeNavigation = make, Color = "Red", PetName = "Fox" };

    Order order = new Order
    {
        CustomerNavigation = customer,
        CarNavigation = car
    };
    context.Orders.Add(order);
    context.SaveChanges();

    Order order_1 = context.Orders
        .Include(o => o.CarNavigation)
        .ThenInclude(c=>c.MakeNavigation)
        .Include(o => o.CustomerNavigation)
        .Single(o => o.Id == 1);

    Console.WriteLine($"" +
        $"Car: {order_1.CarNavigation.Id}\t" +
        $"{order_1.CarNavigation.Color}\t{order_1.CarNavigation.PetName}\t" +
        $"{order_1.CarNavigation.MakeName}\n" +
        $"Customer: {order_1.CustomerNavigation.PersonInformation.FirstName}\t" +
        $"{order_1.CustomerNavigation.PersonInformation.LastName}");

}
Test_Order();
```
```
Car: 7  Fox     Red     Peugeot
Customer: Lara  Croft
```

## Додавання властивості TimeStamp в BaseEntity.

Коли в базі даних існує ствопець TimeStamp і ми хочемо створити міграцію, EF Core створює запроси які збираються змінити існуючий за допомогою ALTER. SQL Server не дозволяє змінівати стовпці типу TimeStamp. Ми по суті виділи TimeStamp з кожної сутності коли успадкували від BaseEntity. Для того щоб додати TimeStamp треба зняти коментар в класі BaseEntity.

```cs
public abstract class BaseEntity
{
    [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Timestamp]
    public byte[] TimeStamp { get; set; } = null;
}
```
Створимо створимо міграцію.

```console
dotnet ef migrations add ChangeBaseEntityAddTimeStamp
dotnet ef database update
```
Можливо для того шоб застосувати міграцію потрібно очистити БД.

```console
dotnet ef database drop -f
dotnet ef database update
```
Після цього можна запустити всі тестові методи вони повині спрацювати.

## Сутність SeriLogEntry

Базі даних потрібна додаткова таблиця для зберігання записів журналу. Проекти ASP.NET Core, наведені далі, використовують framework журналювання SeriLog, і одним із варіантів є запис логів журналу в таблицю SQL Server. Ми збираємося додати таблицю зараз, знаючи, що вона буде використовуватися через кілька глав.
Таблиця не пов’язана з іншими таблицями та не використовує клас BaseEntity. Додайте новий файл класу під назвою SeriLogEntry.cs у папку Entities.

```cs
namespace AutoLot.Models.Entities;

[Table("SeriLogs", Schema = "Logging")]
[EntityTypeConfiguration(typeof(SeriLogEntryConfiguration))]
public class SeriLogEntry
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Message { get; set; }
    public string MessageTemplate { get; set; }
    [MaxLength(128)]
    public string Level { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime TimeStamp { get; set; }
    public string Exception { get; set; }
    public string Properties { get; set; }
    public string LogEvent { get; set; }
    public string SourceContext { get; set; }
    public string RequestPath { get; set; }
    public string ActionName { get; set; }
    public string ApplicationName { get; set; }
    public string MachineName { get; set; }
    public string FilePath { get; set; }
    public string MemberName { get; set; }
    public int LineNumber { get; set; }
    [NotMapped]
    public XElement PropertiesXml => (Properties != null) ? XElement.Parse(Properties) : null;
}
```
Властивість TimeStamp у цій сутності не збігається з властивістю TimeStamp у класі BaseEntity. Назви ті самі, але в цій таблиці вона містить дату й час, коли запис було зареєстровано, а не версію рядка, яка використовується в інших сутностях.

Оновимо клас ApplicationDbContext.

```cs
    public virtual DbSet<SeriLogEntry> SeriLogEntries { get; set; }
```
Створимо файл конфігурації.

```cs
namespace AutoLot.Models.Entities.Configuration;

public class SeriLogEntryConfiguration : IEntityTypeConfiguration<SeriLogEntry>
{
    public void Configure(EntityTypeBuilder<SeriLogEntry> builder)
    {
        builder.Property(e => e.Properties).HasColumnType("Xml");
        builder.Property(e => e.TimeStamp).HasDefaultValueSql("GetDate()");
    }
}
```
Оновіть метод ApplicationDbContext OnModelCreating (), додавши рядок для класу SerilogentRyConfiguration

```cs
        new SeriLogEntryConfiguration().Configure(modelBuilder.Entity<SeriLogEntry>());
```

Оновимо БД.

```console
dotnet ef migrations add AddSeriLogEntry
dotnet ef database update
```

## Моделі перегляду (The View Models)

CustomerOrderView разом із таблицями бази даних було створено як безключову сутність. Інший термін, який використовується для безключових сутностей, — моделі перегляду(view models), оскільки вони призначені для перегляду даних, як правило, із кількох таблиць. Цей розділ оновить риштовану сутність до її остаточної форми, а також додасть нову модель перегляду для перегляду часових даних. 

## CustomerOrderViewModel

Почніть із додавання нової папки під назвою ViewModels у проект AutoLot.Models. Перемістіть клас CustomerOrderView.cs із папки Entities у цю папку та перейменуйте файл на CustomerOrderViewModel.cs, а клас — на CustomerOrderViewModel. 

Анотація даних KeyLess вказує на те, що це сутність, яка працює з даними, які не мають первинного ключа.

Далі додайте чотири нові властивості для сутності Car до моделі перегляду:
```cs
    public bool? IsDrivable { get; set; }
    public string Display { get; set; }
    public string? Price { get; set; }
    public DateTime? DateBuilt { get; set; }
```

Додайте нову NotMapped властивість під назвою FullDetail:

```cs
    [NotMapped]
    public string FullDetail =>
    $"{FirstName} {LastName} ordered a {Color} {Make} named {PetName}";
```
Властивість FullDetail має анотацію даних NotMapped. Нагадуємо, що це інформує EF Core про те, що ця властивість не повинна бути включена в дані, що надходять із бази даних.

Далі додайте перевизначення для методу ToString(). Перевизначення ToString() також ігнорується EF Core

```cs
public override string ToString() => FullDetail;
```
Створіть нову папку під назвою Interfaces у папці ViewModels. У цій папці додайте новий інтерфейс під назвою INonPersisted і оновіть код.
```cs
namespace AutoLot.Models.ViewModels.Interfaces;

public interface INonPersisted
{
}
```

Оновіть файли GlobalUsings.cs. Нові простори імен потрібно додати до файлів GlobalUsings.cs у проектах AutoLot.Dal і AutoLot.Models.

AutoLot.Models
```cs
global using AutoLot.Models.ViewModels.Interfaces;
global using AutoLot.Models.ViewModels.Configuration;
```
AutoLot.Dal
```cs
global using AutoLot.Models.ViewModels;
global using AutoLot.Models.ViewModels.Interfaces;
global using AutoLot.Models.ViewModels.Configuration;
```

Реалізуйте інтерфейс INonPersisted в класі CustomerOrderViewModel.
```cs
public partial class CustomerOrderViewModel : INonPersisted
```
Оновіть клас ApplicationDbContext. Оскільки клас CustomerOrderView було перейменовано на CustomerOrderViewModel, необхідно оновити клас ApplicationDbContext. Знайдіть властивість DbSet< CustomerOrderView> та оновіть рядок.

```cs
    public virtual DbSet<CustomerOrderViewModel> CustomerOrderViewModels { get; set; }
```
Створимо файл конфігурації. Створіть нову папку під назвою Configuration у папці ViewModels. У цій папці додайте новий файл із назвою CustomerOrderViewModelConfiguration.cs у папку Configuration та оновіть код.

```cs
namespace AutoLot.Models.ViewModels.Configuration;

public class CustomerOrderViewModelConfiguration : IEntityTypeConfiguration<CustomerOrderViewModel>
{
    public void Configure(EntityTypeBuilder<CustomerOrderViewModel> builder)
    {
        builder.ToView("CustomerOrderView");
    }
}
```
В класі CustomerOrderViewModel додайте атрибут EntityTypeConfiguration для класу конфігурації.
```cs
[EntityTypeConfiguration(typeof(CustomerOrderViewModelConfiguration))]
```
Оновіть метод ApplicationDbContext OnModelCreating(), видаливши конфігурацію для класу CustomerOrderView та додавши рядок для класу CustomerOrderViewModelConfiguration

```cs
        new CustomerOrderViewModelConfiguration().Configure(modelBuilder.Entity<CustomerOrderViewModel>());
```

Після цього якшо ви запустите команду порівняння моделі кода і БД ви побачите шо змін нема. 
```console
dotnet ef migrations has-pending-model-changes
No changes have been made to the model since the last migration.
```
Також ви побачите шо в Базі даних немає CustomerOrderView.(якшо ви видаляли і створювали базу знову) 

Аби перевірити роботу моделі перегляду треба запустити в БД.
```sql
                CREATE VIEW [dbo].[CustomerOrderView]
                AS
                SELECT c.FirstName, c.LastName, i.Color, i.PetName, 
                    i.DateBuilt, i.IsDrivable, i.Price, i.Display, m.Name AS Make
                FROM dbo.Orders o
                INNER JOIN dbo.Customers c ON c.Id = o.CustomerId
                INNER JOIN dbo.Inventory i ON i.Id = o.CarId
                INNER JOIN dbo.Makes m ON m.Id = i.MakeId
```
```cs
static void Test_CustomerOrderViewModel()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    var customerOrders = context.CustomerOrderViewModels.ToList();
    foreach (var customerOrder in customerOrders)
    {
        Console.WriteLine(customerOrder.FullDetail);
    }
}
Test_Order();
Test_CustomerOrderViewModel();
```
```console
Car: 4  Red     Fox     Peugeot
Customer: Lara  Croft
Lara Croft ordered a Red Peugeot named Fox
Lara Croft ordered a Red Peugeot named Fox
```

## TemporalViewModel

Згадайте з попереднього розділу, що під час роботи з тимчасовими даними корисно мати клас, який зберігає рядок разом із датами рядка з і до. Створіть новий клас під назвою TemporalViewModel у папці ViewModels.

```cs

namespace AutoLot.Models.ViewModels;

public class TemporalViewModel<T> where T : BaseEntity, new()
{
    public required T Entity { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
}
```
Оскільки цей клас використовуватиметься лише для зберігання результатів запитів у тимчасових таблицях, його не потрібно налаштовувати в ApplicationDbContext.

Таким чином ми переглянули всі сутності.

# Оновлення ApplicationDbContext

Настав час оновити файл ApplicationDbContext.cs. Конструктор приймає екземпляр об’єкта DbContextOptions і наразі працює добре.

## Додавання зіставлень функції бази даних

Додамо функції в БД

```sql
CREATE FUNCTION udf_CountOfMakes ( @makeid int )
RETURNS int
AS
BEGIN
    DECLARE @Result int
    SELECT @Result = COUNT(makeid) FROM dbo.Inventory WHERE makeid = @makeid
    RETURN @Result
END
GO
CREATE FUNCTION udtf_GetCarsForMake ( @makeId int )
RETURNS TABLE
AS
RETURN
(   
   SELECT Id, IsDrivable, DateBuilt, Color, PetName, 
   MakeId, TimeStamp, Display, Price, PeriodEnd, PeriodStart
   FROM Inventory WHERE MakeId = @makeId
)
GO
```

Визначені користувачем функції бази даних можна зіставити з функціями C# для використання в запитах LINQ. Додайте такі функції до ApplicationDbContext для двох визначених користувачем функцій:

```cs
    // DB Functions
    [DbFunction("udf_CountOfMakes",Schema ="dbo")]
    public static int InventoryCountFor(int makeId)
        => throw new NotSupportedException();
    [DbFunction("udtf_GetCarsForMake", Schema = "dbo")]
    public IQueryable<Car> GetCarsFor(int makeId)
        => FromExpression(() => GetCarsFor(makeId));
```

Перивіримо їх роботу.

```cs
static void Test_DB_Functions()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var query_1 = context.Makes.Where(m => ApplicationDbContext.InventoryCountFor(m.Id) > 1);
    Console.WriteLine(query_1.ToQueryString());
    Console.WriteLine();

    Make make = context.Makes.First();
    var query_2 = context.GetCarsFor(make.Id);
    Console.WriteLine(query_2.ToQueryString());
    Console.WriteLine();

    List<Car>? cars = query_2.ToList();

    foreach (var car in cars)
    {
        Console.WriteLine(car);
    }
}
Test_DB_Functions();
```
```console
SELECT [m].[Id], [m].[Name], [m].[PeriodEnd], [m].[PeriodStart], [m].[TimeStamp]
FROM [Makes] AS [m]
WHERE [dbo].[udf_CountOfMakes]([m].[Id]) > 1

DECLARE @__makeId_1 int = 1;

SELECT [u].[Id], [u].[Color], [u].[DateBuilt], [u].[Display], [u].[IsDrivable], [u].[MakeId], [u].[PeriodEnd], [u].[PeriodStart], [u].[PetName], [u].[Price], [u].[TimeStamp]
FROM [dbo].[udtf_GetCarsForMake](@__makeId_1) AS [u]
WHERE [u].[IsDrivable] = CAST(1 AS bit)

Wolf is a Grey VW with Id:1
```

## Обробка подій DbContext і ChangeTracker

Перейдіть до конструктора ApplicationDbContext і додайте три події DbContext, розглянуті в попередньому розділі.

```cs
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        //Handling events
        SavingChanges += (sender, args) =>
        {
            string cs = ((ApplicationDbContext)sender!).Database.GetConnectionString()!;
            Console.WriteLine($"Saving changes for {cs}");
        };
        SavedChanges += (sender, args) =>
        {
            string cs = ((ApplicationDbContext)sender!).Database.GetConnectionString()!;
            Console.WriteLine($"Saved {args!.EntitiesSavedCount} changes for {cs}");
        };
        SaveChangesFailed += (sender, args) =>
        {
            Console.WriteLine($"An exception occurred! {args.Exception.Message} entities");
        };

        ChangeTracker.Tracked += ChangeTracker_Tracked;
        ChangeTracker.StateChanged += ChangeTracker_StateChanged;
    }
```
Далі для подій StateChanged і Tracked додайте обробники.

```cs
    private void ChangeTracker_StateChanged(object? sender, EntityStateChangedEventArgs e)
    {
        if (e.Entry.Entity is not Car c)
        {
            return;
        }
        var action = string.Empty;
        Console.WriteLine($"Car {c.PetName} was {e.OldState} before the state changed to {e.NewState}");
        switch (e.NewState)
        {
            case EntityState.Unchanged:
                action = e.OldState switch
                {
                    EntityState.Added => "Added",
                    EntityState.Modified => "Edited",
                    _ => action
                };
                Console.WriteLine($"The object was {action}");
                break;
        }
    }

    private void ChangeTracker_Tracked(object? sender, EntityTrackedEventArgs e)
    {
        var source = (e.FromQuery) ? "Database" : "Code";
        if (e.Entry.Entity is Car c)
        {
            Console.WriteLine($"Car entry {c.PetName} was added from {source}");
        }
    }
```

Додамо метод для всіх тестів

```cs
static void Run()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine($"\tWe have methods:\n\n" +
            $"0 All\n" +
            $"1 Test_Make_Car()\n" +
            $"2 Test_Car_Driver()\n" +
            $"3 Test_Car_Radio()\n" +
            $"4 Test_Customer()\n" +
            $"5 Test_Make()\n" +
            $"6 Test_CreditRisk()\n" +
            $"7 Test_Order()\n" +
            $"8 Test_CustomerOrderViewModel()\n" +
            $"9 Test_DB_Functions()\n" 
            );
        Console.Write("\tWhich method to run: ");
        int.TryParse(Console.ReadLine(), out int choice);

        Console.Clear();
        Console.WriteLine("\tResult:\n");
        switch (choice)
        {
            case 0:
                Test_Make_Car(); Test_Make_Car();Test_Car_Driver(); 
                Test_Car_Radio();Test_Customer(); Test_CreditRisk();
                Test_Order(); Test_CustomerOrderViewModel(); Test_DB_Functions(); 
                break;
            case 1: Test_Make_Car(); break;
            case 2: Test_Car_Driver(); break;
            case 3: Test_Car_Radio(); break;
            case 4: Test_Customer(); break;
            case 5: Test_Make_Car(); break;
            case 6: Test_CreditRisk(); break;
            case 7: Test_Order(); break;
            case 8: Test_CustomerOrderViewModel(); break;
            case 9: Test_DB_Functions(); break;
            default: break;
        }
        Console.Write("\n\tBack to menu");
        Console.ReadKey();
    }
}
Run();
```

## Перевизначати домовленностей

Додайте в класі ApplicationDbContext перевизначення для ConfigureConventions. Наступні зміни за умовчанням  ігноруватимуть сутності, які реалізують інтерфейс INonPersisted. Будь-які анотації даних або команди Fluent API, які суперечать цим параметрам, замінять налаштовані угоди:

```cs
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.IgnoreAny<INonPersisted>();
    }
```

## Перевизначте метод SaveChanges

Метод SaveChanges() у базовому класі DbContext зберігає оновлення, додавання та видалення даних у базі даних. Заміна цього методу в похідному DbContext дає змогу інкапсулювати передачу винятків в одному місці.

```cs
    public override int SaveChanges()
    {
        try
        {
            return base.SaveChanges();
        }
        catch(DbUpdateConcurrencyException ex)
        {
            //A concurrency error occurred
            //Should log and handle intelligently
            throw new CustomConcurrencyException("A concurrency error happened.", ex);
        }
        catch(RetryLimitExceededException ex)
        {
            //DbResiliency retry limit exceeded
            //Should log and handle intelligently
            throw new CustomRetryLimitExceededException("Retry Limit Exceeded Exception", ex);
        }
        catch(DbUpdateException ex)
        {
            //Should log and handle intelligently
            throw new CustomDbUpdateException("An error occurred updating the database", ex);
        }
        catch(Exception ex)
        {
            //Should log and handle intelligently
            throw new CustomException("An error occurred updating the database", ex);
        }
    }
```

# Використовуйте EF Migrations для створення/оновлення об’єктів бази даних

Хоча ми створили CustomerOrderViewModel з рештованої CustomerOrderView яке існувало в базі даних, саме представлення не представлено в конфігорувані моделі кодом C#. Якщо ви видалите базу даних і заново створите її за допомогою міграцій EF Core, представлення не існуватиме. Якшо ви запустите програму тестуваня то тести для переглядів і функцій DB не виконаються і програма вивалиться з винятком.

Для об’єктів бази даних у вас є два варіанти: підтримувати їх окремо та застосовувати за допомогою SSMS/Azure Data Studio або використовувати міграції EF Core для обробки їх створення.
Згадайте, що кожен файл міграції EF Core має метод Up() (для застосування міграції до бази даних) і метод Down() (для відкату змін). MigrationBuilder також має метод Sql(), який виконує оператори SQL безпосередньо щодо бази даних. Додавши оператори CREATE і DROP до методів Up() і Down() міграції, система міграції оброблятиме застосування (і відкат) змін бази даних.

## Додайте клас MigrationHelpers

Допоміжний клас буде містити всі оператори SQL, які використовуються під час спеціальної міграції. Це розділення запобігає втраті коду, якщо міграцію буде видалено із системи. 

Створіть новий загальнодоступним і статичний клас під назвою MigrationHelpers.cs у папці EfStructures проекту AutoLot.Dal. Додайте наступні методи, які використовують MigrationBuilder для виконання операторів SQL щодо бази даних.

```cs
namespace AutoLot.Dal.EfStructures;

public static class MigrationHelpers
{
    public static void CreateCustomerOrderView(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"exec (N' 
                CREATE VIEW [dbo].[CustomerOrderView]
                AS
                SELECT c.FirstName, c.LastName, i.Color, i.PetName, 
                    i.DateBuilt, i.IsDrivable, i.Price, i.Display, m.Name AS Make
                FROM dbo.Orders o
                INNER JOIN dbo.Customers c ON c.Id = o.CustomerId
                INNER JOIN dbo.Inventory i ON i.Id = o.CarId
                INNER JOIN dbo.Makes m ON m.Id = i.MakeId')");
    }
    public static void DropCustomerOrderView(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("exec (N' DROP VIEW [dbo].[CustomerOrderView] ')");
    }


    public static void CreateSproc(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"exec (N' 
                CREATE PROCEDURE [dbo].[GetPetName]
                    @carID int,
                    @petName nvarchar(50) output
                AS
                    SELECT @petName = PetName from dbo.Inventory where Id = @carID')");
    }
    public static void DropSproc(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("EXEC (N' DROP PROCEDURE [dbo].[GetPetName]')");
    }


    public static void CreateFunctions(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"exec (N'
                CREATE FUNCTION [dbo].[udtf_GetCarsForMake] ( @makeId int )
                RETURNS TABLE 
                AS
                RETURN 
                (
                    SELECT Id, IsDrivable, DateBuilt, Color, PetName, MakeId, TimeStamp, Display, Price, PeriodStart, PeriodEnd
                    FROM Inventory WHERE MakeId = @makeId
                )')");
        migrationBuilder.Sql(@"exec (N'
                CREATE FUNCTION [dbo].[udf_CountOfMakes] ( @makeid int )
                RETURNS int
                AS
                BEGIN
                    DECLARE @Result int
                    SELECT @Result = COUNT(makeid) FROM dbo.Inventory WHERE makeid = @makeid
                    RETURN @Result
                END')");
    }
    public static void DropFunctions(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("EXEC (N' DROP FUNCTION [dbo].[udtf_GetCarsForMake]')");
        migrationBuilder.Sql("EXEC (N' DROP FUNCTION [dbo].[udf_CountOfMakes]')");
    }

    public static void Up(MigrationBuilder migrationBuilder)
    {
        CreateCustomerOrderView(migrationBuilder);
        CreateSproc(migrationBuilder);
        CreateFunctions(migrationBuilder);
    }
    public static void Down(MigrationBuilder migrationBuilder)
    {
        DropCustomerOrderView(migrationBuilder);
        DropSproc(migrationBuilder);
        DropFunctions(migrationBuilder);
    } 
}
```
Інструкції CREATE включено в інструкцію exec SQL Server, тому вони будуть успішно виконуватися під час міграції за сценарієм. Кожен процес міграції загорнутий у блок IF, а оператори створення мають бути загорнуті в оператори exec, коли вони виконуються всередині IF.

## Створення міграції для додавання view і функцій БД.

Виконайте команду і превірте шо ваша модель співпадає з моделью БД.

```console
dotnet ef migrations has-pending-model-changes
...
No changes have been made to the model since the last migration.
```
Виклик команди dotnet migrations add, коли немає жодних змін у моделі, усе одно створить файли міграції з правильними мітками часу з порожніми методами Up() і Down(). Виконайте наступне в томуж каталозі проекту, щоб створити порожню міграцію (але не застосовуйте міграцію):
```
dotnet ef migrations add AddViewAndStoredFunction
```
Відкрийте щойно створений клас міграції та зверніть увагу, що методи Up() і Down() порожні. Це ключ до цієї техніки. Використання пустої міграції, оновленої за допомогою методів MigrationHelpers, запобігає змішуванню спеціального коду з кодом, створеним EF Core. Статичні методи для створення об’єктів бази даних входять до методу Up() міграції, а методи видалення об’єктів бази даних – до методу Down() міграції. Під час застосування цієї міграції створюються об’єкти SQL Server, а під час відкоту міграції об’єкти SQL Server видаляються.

```cs
    public partial class AddViewAndStoredFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            MigrationHelpers.Up(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            MigrationHelpers.Down(migrationBuilder);
        }
    }
```
Якшо у вас в БД були раніше створені об'єкти які створює міграція оператори create SQL для цих об’єктів бази даних не вдасться виконати, відкочуючи всю міграцію. Просте виправлення полягає в тому, щоб видалити за допомогою SSMS або Azure Data Studio, як це:

```sql
DROP VIEW [dbo].[CustomerOrderView]
GO
DROP FUNCTION [dbo].[udtf_GetCarsForMake]
GO
DROP FUNCTION [dbo].[udf_CountOfMakes]
GO
DROP PROCEDURE [dbo].[GetPetName]
GO
```
Або видалити вручну.

Після перевірки можна застосувати міграцію.

```console
dotnet ef database update
```
Ви також можете написати код, який спочатку перевірить існування об’єкта та видалить його, якщо він уже існує, але я вважаю, що надмірне вирішення проблеми трапляється лише один раз під час переходу від бази даних до коду.

Тепер ви можете видалити базу даних і створимти заново і запустити тести в яких використовуються додані об'єкти на стороні сервера. 



# Репозиторії

Загальним патерном проектування доступу до даних є патерн repository(сховище). Як описано Мартіном Фаулером, основою цього шаблону є посередництво між шарами домену та відображення даних. Наявність загального базового сховища, яке містить загальний код доступу до даних, допомагає усунути дублювання коду. Наявність спеціальних репозиторіїв та інтерфейсів, які походять від базового репозиторію, також добре працює з інфраструктурою впровадження залежностей у ASP.NET Core.
Кожна сутність домену на рівні доступу до даних AutoLot матиме строго типізований репозіторій для інкапсуляції всієї роботи доступу до даних.

## Додайте інтерфейс IBaseViewRepo

Для початку створіть папку під назвою Repos у проекті AutoLot.Dal для зберігання всіх класів.
Створіть нову папку під назвою Base у каталозі Repos. Додайте новий інтерфейс у папку Repos\Base під назвою IBaseViewRepo. Інтерфейс IBaseViewRepo надає три методи отримання даних із моделі перегляду.  

```cs
namespace AutoLot.Dal.Repos.Base;

public interface IBaseViewRepo<T> : IDisposable where T : class, new()
{
    ApplicationDbContext Context { get; }
    IEnumerable<T> ExecuteSqlString(string sql);
    IEnumerable<T> GetAll();
    IEnumerable<T> GetAllIgnoreQueryFilters();
}
```
Тут обмеженя узагальнення вказує що тип повине бути reference тип і мати конструктор за замовчуванням.


## Додайте реалізацію BaseViewRepo

Додайте клас під назвою BaseViewRepo до каталогу Repos\Base. Цей клас реалізує інтерфейс IBaseViewRepo та забезпечить реалізацію інтерфейсу. Зробіть клас узагальненим за допомогою типу T і обмежте тип до класу та new(), який обмежує типи класами, які мають конструктор без параметрів. Реалізуйте інтерфейс IBaseViewRepo<T>.

```cs
namespace AutoLot.Dal.Repos.Base;

public abstract class BaseViewRepo<T> : IBaseViewRepo<T> where T : class, new()
{
}
```
Репозіторій потребує екземпляр ApplicationDbContext, вставлений у конструктор. У разі використання з контейнером впровадження залежностей (DI) ASP.NET Core контейнер оброблятиме один і той самий контекст протягом роботи програми. Другий конструктор, який використовується для тестування інтеграції, прийме екземпляр DbContextOptions і використає його для створення екземпляра ApplicationDbContext. Цей контекст потрібно буде примусово видалити з пам'яті (to be disposed), оскільки ним не керує контейнер DI. Оскільки цей клас є абстрактним, обидва конструктори protected. Додайте два конструктори та шаблон Dispose:

```cs
namespace AutoLot.Dal.Repos.Base;

public abstract class BaseViewRepo<T> : IBaseViewRepo<T> where T : class, new()
{
    private readonly bool _disposeContext;

    public ApplicationDbContext Context { get; }
    
    protected BaseViewRepo(ApplicationDbContext context)
    {
        Context = context;
        _disposeContext = false;
    }

    protected BaseViewRepo(DbContextOptions<ApplicationDbContext> options) : this(new ApplicationDbContext(options)) 
    {
        _disposeContext = true;
    }

    public virtual void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // Dispose pattern
    private bool _isDisposed;

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (disposing)
        {
            if (_disposeContext)
            {
                Context.Dispose();
            }
        }
        _isDisposed = true;
    }

    ~BaseViewRepo()
    {
        Dispose(false);
    }
    // Dispose pattern
}
```
На властивості DbSet<T> ApplicationDbContext можна посилатися за допомогою методу DbContext.Set<T>(). Створіть загальнодоступну властивість з назвою Table типу DbSet<T> і встановіть значення в початковому конструкторі:

```cs
        
    public DbSet<T> Table { get; }
    
    protected BaseViewRepo(ApplicationDbContext context)
    {
        Context = context;
        Table = context.Set<T>();
        _disposeContext = false;
    }
```

### Реалізація методів читання 

Наступна серія методів повертає записи за допомогою операторів LINQ або SQL-запиту. Методи повертають усі записи з таблиці. Перший отримує їх у порядку бази даних, а другий вимикає всі фільтри запитів. Метод ExecuteSqlString() призначений для виконання запитів FromSqlRaw().

```cs
    public virtual IEnumerable<T> GetAll() 
        => Table.AsQueryable();
    public virtual IEnumerable<T> GetAllIgnoreQueryFilters()
        => Table.AsQueryable().IgnoreQueryFilters();
    public IEnumerable<T> ExecuteSqlString(string sql)
        => Table.FromSqlRaw(sql);
```

## Додайте інтерфейс IBaseRepo

Інтерфейс IBaseRepo надає багато поширених методів, які використовуються для доступу до даних за допомогою властивостей DbSet<T>, де T має тип BaseEntity. Додайте новий інтерфейс у папку Repos\Base під назвою IBaseRepo.

```cs
namespace AutoLot.Dal.Repos.Base;

public interface IBaseRepo<T> : IBaseViewRepo<T> where T : BaseEntity, new()
{
    T? Find(int id);
    T? FindAsNoTracking(int id);
    T? FindIgnoreQueryFilters(int id);
    void ExecuteParameterizedQuery(string sql, object[] sqlParametersObjects);
    int Add(T entity, bool persist = true);
    int AddRange(IEnumerable<T> entities, bool persist = true);
    int Update(T entity, bool persist = true);
    int UpdateRange(IEnumerable<T> entities, bool persist = true);
    int Delete(int id, byte[] timeStamp, bool persist = true);
    int Delete(T entity, bool persist = true);
    int DeleteRange(IEnumerable<T> entities, bool persist = true);
    int SaveChanges();
}
```

## Додайте BaseRepo з реалізацією інтерфейсу

Додайте клас під назвою BaseRepo до каталогу Repos\Base. Цей клас реалізує інтерфейс IBaseRepo, успадкований від абстрактного класу BaseViewRepo<T>, і надає основну функціональність для певного типу репозиторіїв, які будуть створені для кожної сутності. Зробіть клас загальним із типом T і обмежте тип до BaseEntity та new(), що обмежує типи класами, які успадковують від BaseEntity та мають конструктор без параметрів. Реалізуйте інтерфейс IBaseRepo<T> наступним чином

```cs
namespace AutoLot.Dal.Repos.Base;

public abstract class BaseRepo<T> : BaseViewRepo<T>,IBaseRepo<T> where T : BaseEntity, new()
{

}
```
Репозіторій використовує BaseViewRepo<T> для обробки екземпляра ApplicationDbContext, а також реалізації шаблону Dispose(). Додайте наступний код для двох конструкторів

```cs
    protected BaseRepo(ApplicationDbContext context) : base(context) {}
    protected BaseRepo(DbContextOptions<ApplicationDbContext> options) : this(new ApplicationDbContext(options))
    {
    }
```

### Реалізуйте метод SaveChanges

BaseRepo має метод SaveChanges(), який викликає перевизначений метод SaveChanges() у класі ApplicationDbContext.

```cs
    public int SaveChanges()
    {
        try
        {
            return Context.SaveChanges();
        }
        catch (CustomException ex)
        {
            //Should handle intelligently - already logged
            Console.WriteLine(ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            //Should log and handle intelligently
            throw new CustomException("An error occurred updating the database", ex);
        }
    }
```

### Реалізуйте загальні методи читання

Наступна серія методів повертає записи за допомогою операторів LINQ. Метод Find() приймає значення первинного ключа та спочатку шукає ChangeTracker. Якщо сутність уже відстежується, повертається відстежуваний екземпляр. Якщо ні, запис витягується з бази даних.

```cs
    public virtual T? Find(int id) => 
        Table.Find(id);
```
Два додаткові методи Find() доповнюють базовий метод Find(). Наступний метод демонструє отримання запису, але не додавання його до ChangeTracker за допомогою AsNoTrackingWithIdentityResolution().

```cs
    public virtual T? FindAsNoTracking(int id) => 
        Table.AsNoTrackingWithIdentityResolution()
        .FirstOrDefault(e => e.Id == id);
```
Наступний варіант видаляє фільтри запиту з сутності, а потім використовує скорочену версію (пропускаючи метод Where()), щоб отримати FirstOrDefault().
```cs
    public virtual T? FindIgnoreQueryFilters(int id) =>
        Table.IgnoreQueryFilters()
        .FirstOrDefault(e => e.Id == id);
```
Наступний метод використовується для виконання параметризованого запиту.

```cs
    public virtual void ExecuteParameterizedQuery(string sql, object[] sqlParametersObjects)
    {
        Context.Database.ExecuteSqlRaw(sql, sqlParametersObjects);
    }
```

### Методи додавання, оновлення та видалення 

Наступний блок коду, який потрібно додати, охоплює відповідні методи Add()/AddRange(), Update()/UpdateRange() і Remove()/RemoveRange() у певній властивості DbSet<T>. Параметр persist визначає, чи репозиторій виконує SaveChanges() одразу після виклику методів репозиторію. Усі методи позначені віртуальними, щоб дозволити перевизначення нащадкам.

```cs
    public virtual int Add(T entity, bool persist = true)
    {
        Table.Add(entity);
        return persist ? SaveChanges() : 0;
    }
    public virtual int AddRange(IEnumerable<T> entities, bool persist = true)
    {
        Table.AddRange(entities);
        return persist ? SaveChanges() : 0;
    }
    public virtual int Update(T entity, bool persist = true)
    {
        Table.Update(entity);
        return persist ? SaveChanges() : 0;
    }
    public virtual int UpdateRange(IEnumerable<T> entities, bool persist = true)
    {
        Table.UpdateRange(entities);
        return persist ? SaveChanges() : 0;
    }
    public virtual int Delete(T entity, bool persist = true)
    {
        Table.Remove(entity);
        return persist ? SaveChanges() : 0;
    }
    public virtual int DeleteRange(IEnumerable<T> entities, bool persist = true)
    {
        Table.RemoveRange(entities);
        return persist ? SaveChanges() : 0;
    }

```
Є ще один метод Delete(), який не дотримується того самого шаблону. Цей метод використовує EntityState для виконання операції видалення, яка досить часто використовується в операціях ASP.NET Core для скорочення мережевого трафіку.

```cs
    public virtual int Delete(int id, byte[] timeStamp, bool persist = true)
    {
        var entity = new T { Id = id, TimeStamp = timeStamp };
        Context.Entry(entity).State = EntityState.Deleted;
        return persist ? SaveChanges() : 0;
    }
```

## Додайте інтерфейс ITemporalTableBaseRepo

Інтерфейс ITemporalTableBaseRepo розкриває часові можливості EF Core. Додайте новий інтерфейс у папку Repos\Base під назвою ITemporalTableBaseRepo.

```cs
namespace AutoLot.Dal.Repos.Base;

public interface ITemporalTableBaseRepo<T> : IBaseRepo<T> where T : BaseEntity,new()
{
    IEnumerable<TemporalViewModel<T>> GetAllHistory();
    IEnumerable<TemporalViewModel<T>> GetAllHistoryAsOf(DateTime dateTime);
    IEnumerable<TemporalViewModel<T>> GetHistoryBetween(DateTime startDateTime, DateTime endDateTime);
    IEnumerable<TemporalViewModel<T>> GetHistoryContainedIn(DateTime startDateTime, DateTime endDateTime);
    IEnumerable<TemporalViewModel<T>> GetHistoryFromTo(DateTime startDateTime, DateTime endDateTime);
}
```
## Додайте TemporalTableBaseRepo з реалізацією інтерфейсу

Додайте клас під назвою TemporalTableBaseRepo до каталогу Repos\Base. Цей клас реалізує інтерфейс ITemporalTableBaseRepo, успадкований від BaseRepo<T>, і забезпечує функціональність для використання тимчасових таблиць. Також зробіть клас загальним із типом T і обмежте тип до BaseEntity та new().

```cs
namespace AutoLot.Dal.Repos.Base;

public abstract class TemporalTableBaseRepo<T> : BaseRepo<T>, ITemporalTableBaseRepo<T> where T : BaseEntity, new()
{

}
```
Репозіторій використовує BaseRepo<T> для обробки екземпляра ApplicationDbContext, а також реалізації шаблону Dispose(). Додайте наступний код для двох конструкторів:

```cs
    protected TemporalTableBaseRepo(ApplicationDbContext context) : base(context) {}

    protected TemporalTableBaseRepo(DbContextOptions<ApplicationDbContext> options) : this(new ApplicationDbContext(options))
    {
    }
```

### Створіть допоміжні методи

У цьому класі є два допоміжні методи. Перший перетворює поточний час (на основі TimeZoneInfo комп’ютера, що виконується) на час UTC.

```cs
    // Helper methods
    internal static DateTime ConvertToUtc(DateTime dateTime) =>
        TimeZoneInfo.ConvertTimeToUtc(dateTime, TimeZoneInfo.Utc);
```
Наступний метод використовує IQueryable<T>, додає речення OrderBy для поля PeriodStart і проектує результати в колекцію екземплярів TemporalViewModel

```cs
    internal static IEnumerable<TemporalViewModel<T>> ExecuteQuery(IQueryable<T> query) =>
        query.OrderBy(e => EF.Property<DateTime>(e,"PeriodStart"))
        .Select(e=> new TemporalViewModel<T>
        {
            Entity = e,
            PeriodStart = EF.Property<DateTime>(e,"PeriodStart"),
            PeriodEnd = EF.Property<DateTime>(e,"PeriodEnd")
        } );
```

### Реалізація методів інтерфейсу ITemporalTableBaseRepo

Останнім кроком є ​​реалізація п’яти temporal(часових) методів інтерфейсу. Вони приймають необхідні параметри даних, викликають відповідний часовий метод EF Core, а потім передають виконання допоміжному методу ExecuteQuery():

```cs
    public IEnumerable<TemporalViewModel<T>> GetAllHistory() =>
        ExecuteQuery(Table.TemporalAll());
    public IEnumerable<TemporalViewModel<T>> GetAllHistoryAsOf(DateTime dateTime) =>
        ExecuteQuery(Table.TemporalAsOf(ConvertToUtc(dateTime)));
    public IEnumerable<TemporalViewModel<T>> GetHistoryBetween(DateTime startDateTime, DateTime endDateTime) =>
        ExecuteQuery(Table.TemporalBetween(ConvertToUtc(startDateTime),ConvertToUtc(endDateTime)));
    public IEnumerable<TemporalViewModel<T>> GetHistoryContainedIn(
        DateTime startDateTime, DateTime endDateTime)
        => ExecuteQuery(Table.TemporalContainedIn(ConvertToUtc(startDateTime), ConvertToUtc(endDateTime)));
    public IEnumerable<TemporalViewModel<T>> GetHistoryFromTo(
        DateTime startDateTime, DateTime endDateTime)
        => ExecuteQuery(Table.TemporalFromTo(ConvertToUtc(startDateTime), ConvertToUtc(endDateTime)));
```
Тепер, коли всі базові репозиторії готові, настав час створити репозиторії для окремих сутностей.


## Спеціальні інтерфейси для сутності

Кожна сутність і модель представлення матимуть строго типізований репозиторій, похідний від BaseRepo<T>, і інтерфейс, який реалізує IRepo<T>. Додайте нову папку під назвою Interfaces у каталозі Repos у проекті AutoLot.Dal. В ній будуть інтерфейси.


## Оновіть файли GlobalUsings.cs
Нові простори імен репозиторіїв потрібно додати до файлу GlobalUsings.cs у проекті AutoLot.Dal. Завдяки цьому базові класи і інтерфейси будуть доступні в кожному похідному без потреби використовувати using.
```cs
global using AutoLot.Dal.Repos.Base;
global using AutoLot.Dal.Repos.Interfaces;
```

## Інтерфейс репозиторію CarDriver

Створіть інтерфейс ICarDriverRepo.cs. Цей інтерфейс не додає жодної функціональності, окрім тієї, що надається в TemporalTableBaseRepo.

```cs
namespace AutoLot.Dal.Repos.Interfaces;

public interface ICarDriverRepo : ITemporalTableBaseRepo<CarDriver>
{
}
```
## Інтерфейс репозиторію Car

Створіть інтерфейс ICarRepo.cs. Змініть інтерфейс на публічний і визначте репо наступним чином

```cs
namespace AutoLot.Dal.Repos.Interfaces;

public interface ICarRepo : ITemporalTableBaseRepo<Car>
{
    IEnumerable<Car> GetAllBy(int makeId);
    string GetPetName(int id);
}
```

## Інтерфейс репозиторію CreditRisk

Створіть інтерфейс ICreditRiskRepo.cs. Цей інтерфейс не додає жодної функціональності, окрім тієї, що надається в BaseRepo.

```cs
namespace AutoLot.Dal.Repos.Interfaces;

public interface ICreditRiskRepo : IBaseRepo<CreditRisk>
{
}
```
## Інтерфейс репозиторію CustomerOrderViewModel

Створіть інтерфейс ICustomerOrderViewModelRepo.cs. Цей інтерфейс не додає жодної функціональності, окрім тієї, що надається в BaseViewRepo.

```cs
namespace AutoLot.Dal.Repos.Interfaces;

public interface ICustomerOrderViewModelRepo : IBaseViewRepo<CustomerOrderViewModel>
{
}
```
## Інтерфейс репозиторію Customer

Створіть інтерфейс ICustomerRepo.cs. Цей інтерфейс не додає жодної функціональності, окрім тієї, що надається в BaseRepo.

```cs
namespace AutoLot.Dal.Repos.Interfaces;

public interface ICustomerRepo : IBaseRepo<Customer>
{
}
```
## Інтерфейс репозиторію Driver

Створіть інтерфейс IDriverRepo.cs. Цей інтерфейс не додає жодної функціональності, окрім тієї, що надається в BaseRepo.

```cs
namespace AutoLot.Dal.Repos.Interfaces;

public interface IDriverRepo : IBaseRepo<Driver>
{
}
```
## Інтерфейс репозиторію Make

Створіть інтерфейс IMakeRepo.cs. Цей інтерфейс не додає жодної функціональності, окрім тієї, що надається в TemporalTableBaseRepo.

```cs
namespace AutoLot.Dal.Repos.Interfaces;

public interface IMakeRepo : ITemporalTableBaseRepo<Make>
{
}
```
## Інтерфейс репозиторію Order

Створіть інтерфейс IOrderRepo.cs. Цей інтерфейс не додає жодної функціональності, окрім тієї, що надається в TemporalTableBaseRepo.

```cs
namespace AutoLot.Dal.Repos.Interfaces;

public interface IOrderRepo : ITemporalTableBaseRepo<Order>
{
}
```
## Інтерфейс репозиторію Radio

Створіть інтерфейс IRadioRepo.cs.

```cs
namespace AutoLot.Dal.Repos.Interfaces;

public interface IRadioRepo : ITemporalTableBaseRepo<Radio>
{
}
```
Це завершує створення інтерфейсів для репозиторіїв для окремих сутностей.


## Створення репозиторіїв для окремих сутностей

Реалізовані репозиторії отримують більшу частину своєї функціональності від базового класу. У цьому розділі описано функціональні можливості, додані або перевизначені з базового репозіторія. 

Всі в каталозі Repos. Ви помітите, що жоден із класів репозиторію не має коду обробки помилок або журналювання. Це зроблено навмисно, щоб приклади були зосередженими. Ви захочете переконатися, що ви обробляєте (і реєструєте) помилки у своєму робочому коді.


## Репозіторій CarDriver

Створіть клас CarDriverRepo.cs, змініть на public, успадкуйте від TemporalTableBaseRepo<CarDriver> і запровадьте ICarDriverRepo. 

```cs

namespace AutoLot.Dal.Repos;

public class CarDriverRepo : TemporalTableBaseRepo<CarDriver>, ICarDriverRepo
{
}
```

Кожний з репозиторіїв має реалізовувати два конструктори з BaseRepo. Перший конструктор використовуватиметься ASP.NET Core і його вбудованим процесом впровадження залежностей. Другий використовуватиметься в інтеграційних тестах (розглянуто в наступному розділі).

```cs
    public CarDriverRepo(ApplicationDbContext context) : base(context)
    {
    }
    internal CarDriverRepo(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
```
Далі створіть внутрішній метод, який містить властивості CarNavigation і DriverNavigation. Зауважте, що тип повернення – IIncludableQueryable\<CarDriver, Driver\>. Якщо використовується кілька Include, відкритий тип використовує базовий тип (CarDriver) і остаточний включений тип (Driver). Цей метод використовуватиметься загальнодоступними методами репозіторія.

```cs
    internal IIncludableQueryable<CarDriver, Driver> BuildBaseQuery()
        => Table.Include(cd => cd.CarNavigation).Include(cd => cd.DriverNavigation);
```
Перевизначте методи GetAll(), GetAllIgnoreQueryFilters() і Find(), щоб використовувати внутрішній метод

```cs
    public override IEnumerable<CarDriver> GetAll()
        => BuildBaseQuery();
    public override IEnumerable<CarDriver> GetAllIgnoreQueryFilters()
        => BuildBaseQuery().IgnoreQueryFilters();
    public override CarDriver? Find(int id)
        => BuildBaseQuery().IgnoreQueryFilters()
        .Where(cd=>cd.Id == id).FirstOrDefault();
```

## Репозіторій Car

Створіть клас CarRepo.cs і змініть його на public, успадкуйте від TemporalTableBaseRepo<Car> і реалізуйте ICarRepo та конструктори:

```cs

namespace AutoLot.Dal.Repos;

public class CarRepo : TemporalTableBaseRepo<Car>, ICarRepo
{
    public CarRepo(ApplicationDbContext context) : base(context)
    {
    }
    public CarRepo(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}
```
Потім створіть внутрішній метод, який включає MakeNavigation і OrderBy() для властивості PetName. Зверніть увагу, що повертається тип IOrderedQueryable<Car>. Це буде використано загальнодоступними методами:

```cs
    internal IOrderedQueryable<Car> BuildBaseQuery() =>
        Table.Include(c => c.MakeNavigation).OrderBy(c => c.PetName);
```
Додайте перевизначення для GetAll() і GetAllIgnoreQueryFilters(), використовуючи базовий запит, щоб включити властивість MakeNavigation і порядок за значеннями PetName.

```cs
public override IEnumerable<Car> GetAll()
    => BuildBaseQuery();
public override IEnumerable<Car> GetAllIgnoreQueryFilters()
    => BuildBaseQuery().IgnoreQueryFilters();
```

Реалізуйте метод GetAllBy(). Цей метод отримує всі записи Inventory з указаним MakeId

```cs
    public IEnumerable<Car> GetAllBy(int makeId) =>
        BuildBaseQuery().Where(c => c.MakeId == makeId);
```
Додайте перевизначення для Find(), щоб включити властивість MakeNavigation і ігнорувати фільтри запитів.

```cs
    public override Car? Find(int id) =>
        Table.IgnoreQueryFilters()
        .Where(c => c.Id == id)
        .Include(c => c.MakeNavigation)
        .FirstOrDefault();
```

Додайте метод для отримання значення PetName за допомогою збереженої процедури. Це використовує метод ExecuteParameterizedQuery() базового класу та повертає значення параметра OUTPUT.

```cs
    public string GetPetName(int id)
    {
        var parameterId = new SqlParameter
        {
            ParameterName = "@carId",
            SqlDbType = SqlDbType.Int,
            Value = id
        };
        var parameterName = new SqlParameter
        {
            ParameterName = "@petName",
            SqlDbType = SqlDbType.NVarChar,
            Size = 50,
            Direction = ParameterDirection.Output
        };

        string sqlQuery = "EXEC [dbo].[GetPetName] @carId, @petName OUTPUT";
        ExecuteParameterizedQuery(sqlQuery, [parameterId, parameterName]);
        return (string) parameterName.Value;
    }
```

Перивіримо клас репозіторію.

SimpleTest\Program.cs
```cs
static void Test_CarRepo()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    CarRepo carRepo = new(context);

    Make make = new Make() { Name = "VW" };
    Car car = new() { MakeNavigation = make, Color = "White", PetName = "Electron" };
    carRepo.Add(car, true);

    int id = carRepo.Table.Max(c => c.Id);

    ShowCars(carRepo.GetAll()); Console.WriteLine();
    ShowCars(carRepo.GetAllBy(id)); Console.WriteLine();
    Console.WriteLine(carRepo.GetPetName(id)); Console.WriteLine();
    Console.WriteLine(carRepo.Find(id));

    static void ShowCars(IEnumerable<Car> cars)
    {
        foreach (var car in cars)
        {
            Console.WriteLine(car);
        }
    }
}
```
```cs
static void Run()
{
    while (true)
    {
            //...
            $"10 Test_CarRepo()\n"
            //...
            case 10: Test_CarRepo(); break;
            //...
    }        
}
```

## Репозіторій CreditRisk

Створіть клас CreditRiskRepo.cs і змініть його на public, успадкуйте від BaseRepo<CreditRisk>, реалізуйте ICreditRiskRepo та додайте два необхідні конструктори.

```cs

namespace AutoLot.Dal.Repos;

public class CreditRiskRepo : BaseRepo<CreditRisk>, ICreditRiskRepo
{
    public CreditRiskRepo(ApplicationDbContext context) : base(context)
    {
    }

    public CreditRiskRepo(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}
```

## Репозиторій CustomerOrderViewModel

Створіть клас CustomerOrderViewModelRepo.cs і змініть його на public, успадкуйте від BaseViewRepo<CustomerOrderViewModel>, реалізуйте ICustomerOrderViewModelRepo та додайте два необхідні конструктори.

```cs

namespace AutoLot.Dal.Repos;

public class CustomerOrderViewModelRepo : BaseViewRepo<CustomerOrderViewModel>, ICustomerOrderViewModelRepo
{
    public CustomerOrderViewModelRepo(ApplicationDbContext context) : base(context)
    {
    }

    public CustomerOrderViewModelRepo(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}
```

## Репозиторій Customer

Створіть клас CustomerRepo.cs і змініть його на public, успадкуйте від BaseRepo<Customer>, реалізуйте ICustomerRepo та додайте два необхідні конструктори.

```cs

namespace AutoLot.Dal.Repos;

public class CustomerRepo : BaseRepo<Customer>, ICustomerRepo
{
    public CustomerRepo(ApplicationDbContext context) : base(context)
    {
    }

    public CustomerRepo(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}
```
Додайте метод, який повертає всі записи Customer із їхніми замовленнями, відсортованими за прізвищем.

```cs
    public override IEnumerable<Customer> GetAll() =>
        Table.Include(c => c.Orders)
        .OrderBy(o => o.PersonInformation.LastName);
```

## Репозиторій Driver

Створіть клас DriverRepo.cs і змініть його на public, успадкуйте від BaseRepo<Driver>, реалізуйте IDriverRepo та додайте два конструктори.

```cs

namespace AutoLot.Dal.Repos;

public class DriverRepo : BaseRepo<Driver>, IDriverRepo
{
    public DriverRepo(ApplicationDbContext context) : base(context)
    {
    }

    public DriverRepo(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}
```

Далі створіть внутрішній метод, який включає порядок за властивостями LastName, а потім FirstName з класу PersonInformation. Зауважте, що тип повернення – IOrderedQueryable<Driver>. Це використовуватиметься публічними функціями:

```cs
    internal IOrderedQueryable<Driver> BuildQuery() =>
        Table
        .OrderBy(d => d.PersonInformation.LastName)
        .OrderBy(d => d.PersonInformation.FirstName);
```

Перевизначте методи GetAll() і GetAllIgnoreQueryFilters(), щоб використовувати внутрішній метод:

```cs
    public override IEnumerable<Driver> GetAll() =>
        BuildQuery();
    public override IEnumerable<Driver> GetAllIgnoreQueryFilters() =>
        BuildQuery().IgnoreQueryFilters();
```

## Репозиторій Make

Створіть клас MakeRepo.cs і змініть його на public, успадкуйте від TemporalTableBaseRepo<Make>, реалізуйте IMakeRepo та додайте два необхідні конструктори.

```cs

namespace AutoLot.Dal.Repos;

public class MakeRepo : TemporalTableBaseRepo<Make>, IMakeRepo
{
    public MakeRepo(ApplicationDbContext context) : base(context)
    {
    }

    public MakeRepo(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}
```
Далі створіть внутрішній метод, який включає порядок за властивістю Name. Зверніть увагу, що тип повернення є IOrderedQueryable<Make>:
```cs
    internal IOrderedQueryable<Make> BuildQuery() =>
        Table.OrderBy(m => m.Name);
```
Перевизначте методи GetAll() і GetAllIgnoreQueryFilters(), які сортують значення Make за іменем.

```cs
    public override IEnumerable<Make> GetAll() =>
        BuildQuery();
    public override IEnumerable<Make> GetAllIgnoreQueryFilters() =>
        BuildQuery().IgnoreQueryFilters();
```

## Репозиторій Order

Створіть клас OrderRepo.cs і змініть його на public, успадкуйте від TemporalTableBaseRepo<Order>, реалізуйте IOrderRepo та додайте два необхідні конструктори.

```cs

namespace AutoLot.Dal.Repos;

public class OrderRepo : TemporalTableBaseRepo<Order>, IOrderRepo
{
    public OrderRepo(ApplicationDbContext context) : base(context)
    {
    }

    public OrderRepo(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}
```

## Репозиторій Radio

Створіть клас RadioRepo.cs і змініть його на public, успадкуйте від TemporalTableBaseRepo<Radio>, реалізуйте IRadioRepo та додайте два необхідні конструктори.

```cs

namespace AutoLot.Dal.Repos;

public class RadioRepo : TemporalTableBaseRepo<Radio>, IRadioRepo
{
    public RadioRepo(ApplicationDbContext context) : base(context)
    {
    }

    public RadioRepo(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}
```
# Програмна обробка бази даних і міграцій

Властивість Database DbContext надає програмні методи для видалення та створення бази даних, а також виконання всіх міграцій. Ось методи, пов'язані з цими операціями.

Програмна робота з базою даних

|Метод Database|Опис| 
|--------------|----|
|EnsureDeleted()|Видаляє базу даних, якщо вона існує. Нічого не робить, якщо її не існує.|
|EnsureCreated()|Створює базу даних, якщо вона не існує. Нічого не робить, якщо вона робить. Створює таблиці та стовпці на основі класів, доступних за допомогою властивостей DbSet. Не застосовує жодних міграцій.Примітка: це не слід використовувати разом із міграціями.|
|Migrate()|Створює базу даних, якщо вона не існує.Застосовує всі міграції до бази даних.|

Як зазначено в таблиці, метод EnsureCreated() створить базу даних, якщо вона не існує, а потім створить таблиці, стовпці та індекси на основі моделі сутності. Він не застосовує жодних міграцій. Якщо ви використовуєте міграції (як ми), це призведе до помилок під час роботи з базою даних, і вам доведеться обдурити EF Core (як ми робили раніше), щоб повірити, що міграції застосовано. Вам також доведеться вручну створювати будь-які спеціальні об’єкти SQL до бази даних. Коли ви працюєте з міграціями, завжди використовуйте метод Migrate() для програмного створення бази даних, а не метод EnsureCreated().

## Drop, Create, та Clean для бази даних

Під час розробки може бути корисно видалити та повторно створити базу даних розробки, а потім заповнити її зразками даних. Це створює стабільну та передбачувану настройку бази даних, корисну під час тестування (ручного чи автоматичного). Створіть нову папку з назвою Initialization у проекті AutoLot.Dal. У цій папці створіть новий клас під назвою SampleDataInitializer.cs. Зробіть клас public і static.

```cs
namespace AutoLot.Dal.Initialization;

public static class SampleDataInitializer
{

}
```
Створіть метод під назвою DropAndCreateDatabase, який приймає екземпляр ApplicationDbContext як єдиний параметр. Цей метод використовує властивість Database ApplicationDbContext, щоб спочатку видалити базу даних (за допомогою методу EnsureDeleted()), а потім створити базу даних (за допомогою методу Migrate()).

```cs
    public static void DropAndCreateDatabase(ApplicationDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.Migrate();
    }
```
Цей процес дуже добре працює, коли ви використовуєте локальну базу даних (наприклад, у контейнері Docker, на локальному диску тощо). Це не працює під час використання SQL Azure, оскільки команди EF Core не можуть створювати екземпляри бази даних SQL Azure. Якщо ви використовуєте SQL Azure, замість цього використовуйте метод ClearData(), детально описаний далі.

Створіть інший метод під назвою ClearData(), який видаляє всі дані в базі даних і скидає ідентифікаційні значення для кожного первинного ключа таблиці. Метод циклично переглядає список сутностей домену та використовує властивість DbContext Model, щоб отримати схему та назву таблиці, з якою зіставлено кожну сутність. Потім він виконує інструкцію видалення та скидає ідентифікатор для кожної таблиці за допомогою методу ExecuteSqlRaw() властивості DbContext Database. Якщо таблиця є тимчасовою, він видаляє дані історії.

```cs
    internal static void ClearData(ApplicationDbContext context)
    {
        var entities = new[]
        {
            typeof(Order).FullName,
            typeof(Customer).FullName,
            typeof(CarDriver).FullName,
            typeof(Driver).FullName,
            typeof(Radio).FullName,
            typeof(Car).FullName,
            typeof(Make).FullName,
            typeof(CreditRisk).FullName
        };
      
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddDbContextDesignTimeServices(context);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var designTimeModel = serviceProvider.GetService<IModel>();

        foreach (var entityName in entities)
        {
            var entity = context.Model.FindEntityType(entityName);
            var tableName = entity.GetTableName();
            var schemaName = entity.GetSchema();
            context.Database.ExecuteSqlRaw($"DELETE FROM {schemaName}.{tableName}");
            context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT (\"{schemaName}.{tableName}\", RESEED, 0);");
            if (entity.IsTemporal())
            {
                var strategy = context.Database.CreateExecutionStrategy();
                strategy.Execute(() =>
                {
                    using var trans = context.Database.BeginTransaction();
                    var designTimeEntity = designTimeModel.FindEntityType(entityName);
                    var historySchema = designTimeEntity.GetHistoryTableSchema();
                    var historyTable = designTimeEntity.GetHistoryTableName();
                    context.Database.ExecuteSqlRaw($"ALTER TABLE {schemaName}.{tableName} SET (SYSTEM_VERSIONING = OFF)");
                    context.Database.ExecuteSqlRaw($"DELETE FROM {historySchema}.{historyTable}");
                    context.Database.ExecuteSqlRaw($"ALTER TABLE {schemaName}.{tableName} SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE={historySchema}.{historyTable}))");
                    trans.Commit();
                });
            }
        }
    }
```
Слід обережно використовувати метод ExecuteSqlRaw() фасаду бази даних, щоб запобігти потенційним атакам SQL-ін’єкцій.

## Ініціалізація даних

Ми збираємося створити власну систему заповнення даних, яку можна запускати за потреби.
Першим кроком є ​​створення зразків даних, а потім додавання методів у SampleDataInitializer, який використовується для завантаження зразків даних у базу даних.

## Зразки даних

Додайте новий файл із назвою SampleData.cs до папки Initialization. Зробіть клас публічним і статичним. Клас складається з восьми статичних властивостей із методом, який створює зразки даних.

```cs

namespace AutoLot.Dal.Initialization;

public static class SampleData
{
    public static List<Customer> Customers => new()
    {
        new() { Id = 1, PersonInformation = new() { FirstName = "Dave", LastName = "Brenner" } },
        new() { Id = 2, PersonInformation = new() { FirstName = "Matt", LastName = "Walton" } },
        new() { Id = 3, PersonInformation = new() { FirstName = "Steve", LastName = "Hagen" } },
        new() { Id = 4, PersonInformation = new() { FirstName = "Pat", LastName = "Walton" } },
        new() { Id = 5, PersonInformation = new() { FirstName = "Bad", LastName = "Customer" } },
    };
    public static List<Make> Makes => new()
    {
        new() { Id = 1, Name = "VW" },
        new() { Id = 2, Name = "Ford" },
        new() { Id = 3, Name = "Saab" },
        new() { Id = 4, Name = "Yugo" },
        new() { Id = 5, Name = "BMW" },
        new() { Id = 6, Name = "Pinto" },
    };

    public static List<Driver> Drivers => new()
    {
        new() { Id = 1, PersonInformation = new() { FirstName = "Fred", LastName = "Flinstone" } },
        new() { Id = 2, PersonInformation = new() { FirstName = "Barney", LastName = "Rubble" } }
    };

    public static List<Car> Inventory => new()
    {
        new() { Id = 1, MakeId = 1, Color = "Black", PetName = "Zippy" },
        new() { Id = 2, MakeId = 2, Color = "Rust", PetName = "Rusty" },
        new() { Id = 3, MakeId = 3, Color = "Black", PetName = "Mel" },
        new() { Id = 4, MakeId = 4, Color = "Yellow", PetName = "Clunker" },
        new() { Id = 5, MakeId = 5, Color = "Black", PetName = "Bimmer" },
        new() { Id = 6, MakeId = 5, Color = "Green", PetName = "Hank" },
        new() { Id = 7, MakeId = 5, Color = "Pink", PetName = "Pinky" },
        new() { Id = 8, MakeId = 6, Color = "Black", PetName = "Pete" },
        new() { Id = 9, MakeId = 4, Color = "Brown", PetName = "Brownie" },
        new() { Id = 10, MakeId = 1, Color = "Rust", PetName = "Lemon", IsDrivable = false },
    };
    public static List<Radio> Radios => new()
    {
        new() { Id = 1, CarId = 1, HasSubWoofers = true, RadioId = "SuperRadio 1", HasTweeters = true },
        new() { Id = 2, CarId = 2, HasSubWoofers = true, RadioId = "SuperRadio 2", HasTweeters = true },
        new() { Id = 3, CarId = 3, HasSubWoofers = true, RadioId = "SuperRadio 3", HasTweeters = true },
        new() { Id = 4, CarId = 4, HasSubWoofers = true, RadioId = "SuperRadio 4", HasTweeters = true },
        new() { Id = 5, CarId = 5, HasSubWoofers = true, RadioId = "SuperRadio 5", HasTweeters = true },
        new() { Id = 6, CarId = 6, HasSubWoofers = true, RadioId = "SuperRadio 6", HasTweeters = true },
        new() { Id = 7, CarId = 7, HasSubWoofers = true, RadioId = "SuperRadio 7", HasTweeters = true },
        new() { Id = 8, CarId = 8, HasSubWoofers = true, RadioId = "SuperRadio 8", HasTweeters = true },
        new() { Id = 9, CarId = 9, HasSubWoofers = true, RadioId = "SuperRadio 9", HasTweeters = true },
        new() { Id = 10, CarId = 10, HasSubWoofers = true, RadioId = "SuperRadio 10", HasTweeters = true },
    };
    public static List<CarDriver> CarsAndDrivers => new()
    {
        new() { Id = 1, CarId = 1, DriverId = 1 },
        new() { Id = 2, CarId = 2, DriverId = 2 }
    };

    public static List<Order> Orders => new()
    {
        new() { Id = 1, CustomerId = 1, CarId = 5 },
        new() { Id = 2, CustomerId = 2, CarId = 1 },
        new() { Id = 3, CustomerId = 3, CarId = 4 },
        new() { Id = 4, CustomerId = 4, CarId = 7 },
        new() { Id = 5, CustomerId = 5, CarId = 10 },
    };

    public static List<CreditRisk> CreditRisks => new()
    {
        new()
        {
            Id = 1,
            CustomerId = Customers[4].Id,
            PersonInformation = new()
            {
                FirstName = Customers[4].PersonInformation.FirstName,
                LastName = Customers[4].PersonInformation.LastName
            }
        }
    };
}
```

## Завантаження зразків даних

Внутрішній метод SeedData() у класі SampleDataInitializer додає дані з методів SampleData в екземпляр ApplicationDbContext, а потім зберігає дані в базі даних.

```cs
    internal static void SeedData(ApplicationDbContext context)
    {
        try
        {
            ProcessInsert(context, context.Customers, SampleData.Customers);
            ProcessInsert(context, context.Makes, SampleData.Makes);
            ProcessInsert(context, context.Drivers, SampleData.Drivers);
            ProcessInsert(context, context.Cars, SampleData.Inventory);
            ProcessInsert(context, context.Radios, SampleData.Radios);
            ProcessInsert(context, context.CarsToDrivers, SampleData.CarsAndDrivers);
            ProcessInsert(context, context.Orders, SampleData.Orders);
            ProcessInsert(context, context.CreditRisks, SampleData.CreditRisks);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            //Set a break point here to determine what the issues is
            throw;
        }

        static void ProcessInsert<TEntity>(ApplicationDbContext context,
            DbSet<TEntity> table, List<TEntity> records) where TEntity : BaseEntity
        {
            if(table.Any()) { return; }

            IExecutionStrategy strategy = context.Database.CreateExecutionStrategy();

            strategy.Execute(() =>
            {
                using var transaction = context.Database.BeginTransaction();
                try
                {
                    var metaData = context.Model.FindEntityType(typeof(TEntity).FullName);
                    string sqlON = $"SET IDENTITY_INSERT {metaData.GetSchema()}.{metaData.GetTableName()} ON";
                    string sqlOFF = $"SET IDENTITY_INSERT {metaData.GetSchema()}.{metaData.GetTableName()} OFF";

                    context.Database.ExecuteSqlRaw(sqlON);
                    table.AddRange(records);
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw(sqlOFF);

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            });
        }
    }
```

Метод SeedData() використовує локальну функцію для обробки даних. Спочатку він перевіряє, чи є в таблиці записи, і, якщо ні, переходить до обробки зразків даних. ExecutionStrategy створюється з фасаду бази даних, і це використовується для створення явної транзакції, яка потрібна для ввімкнення та вимкнення вставки ідентифікаційної інформації. Записи додаються, і якщо все пройшло успішно, транзакція фіксується; в іншому випадку він відкочується.

Наступні два методи є загальнодоступними та використовуються для скидання бази даних. InitializeData() видаляє та повторно створює базу даних перед її заповненням, а метод ClearDatabase() просто видаляє всі записи, скидає ідентифікатор, а потім заповнює дані.

```cs
    public static void InitializeData(ApplicationDbContext context)
    {
        DropAndCreateDatabase(context);
        SeedData(context);
    }

    public static void ClearAndSeedData(ApplicationDbContext context)
    {
        ClearData(context);
        SeedData(context);
    }
```

Спробуємо перивірити.
Program.cs
```cs

static void Test_InitializeData()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    SampleDataInitializer.InitializeData(context);
}


static void Test_ClearAndSeedData()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    SampleDataInitializer.ClearAndSeedData(context);
}


static void Run()
{
    while (true)
    {
            //...
            $"11 Test_InitializeData()\n" +
            $"12 Test_ClearAndSeedData()\n"
            //...
            case 11: Test_InitializeData(); break;
            case 12: Test_ClearAndSeedData(); break;
            //...
    }        
}
```
Тест методу Test_ClearAndSeedData() не проходить з винятком повязаним з недоступністю файлів. Аби рішити цю проблему можна в проект SimpleTest додадти пакет 

Microsoft.EntityFrameworkCore.Design

Потім закоментувати рядок 

```xml
      <!--<IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>-->
```
Після виправленя цієї помилки, при виконані Test_ClearAndSeedData(),  з'явится інша яка вказує що часові таблиці не вказані як таблиці з схемою і назвою таблиці.
Для вирішеня поеблеми треба додати визначення схеми для тимчасових таблиць усіх сутностей. 

CarConfigurations.cs
```cs
            t.UseHistoryTable("InventoryAudit","dbo");
```
CarDriverConfiguration.cs
```cs
            t.UseHistoryTable("InventoryToDriversAudit","dbo");
```
MakeConfiguration.cs
```cs
            t.UseHistoryTable("MakesAudit","dbo");
```
OrderConfiguration.cs
```cs
            t.UseHistoryTable("OrdersAudit","dbo");
```
RadioConfiguration.cs
```cs
            t.UseHistoryTable("RadiosAudit","dbo");
```

Створмо нову міграцію і застосуємо.

```console
dotnet ef migrations add ChangeTemporalTableAddSchema
dotnet ef database update
```
Після ціх змін тест виконається.

Код ініціалізації буде детально опрацьований у наступній главі.

# Підсумки

У цій главі використано знання, отримані в попередній, для завершення рівня доступу до даних для бази даних AutoLot. Ви використовували інструменти командного рядка EF Core для створення каркасу існуючої бази даних, оновили модель до її остаточної версії, а потім створили міграції та застосували їх. Для інкапсуляції доступу до даних було додано репозиторії, а код ініціалізації бази даних зі зразками даних може бути видалений та створений у повторюваний та надійний спосіб. Наступний розділ зосереджений на тестуванні рівня доступу до даних.