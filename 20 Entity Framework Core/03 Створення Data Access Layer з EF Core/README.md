# Створення Data Access Layer з EF Core

Цей розділ присвячено застосуванню того, що ви дізналися про EF Core, для створення рівня доступу до даних AutoLot. Цей розділ починається зі створення одного проекту для сутностей і іншого для коду бібліотеки доступу до даних. Відокремлення моделей від коду доступу до даних є звичайним проектним рішенням, яке використано в ASP.NET Core.

Наступним кроком є ​риштування(створення каркасу моделі) наявної бази даних із розділу ADO.NET на сутності та похідний DbContext за допомогою інтерфейсу командного рядка (CLI) EF Core. Це демонструє процес database first. Потім проект змінюється на code first, де дизайн бази даних керується сутностями C#. 
Сутності з розділу ADO.NET оновлюються до остаточної версії, нові сутності з попередніх розділів додаються в модель, а база даних оновлюється за допомогою міграцій EF Core. Потім збережена процедура, перегляд бази даних і визначені користувачем функції інтегруються в систему міграції EF Core, забезпечуючи розробникам унікальний механізм отримання повної копії бази даних. Остаточна міграція EF Core завершує базу даних. 

Наступним кроком є ​​створення репозиторіїв, які надають інкапсульований доступ для створення, читання, оновлення та видалення (CRUD) до бази даних. Останнім кроком у цій главі є додавання коду ініціалізації даних для надання зразкових даних, що є звичайною практикою для тестування рівня доступу до даних.

## Створення проектів AutoLot.Dal і AutoLot.Models

AutoLot прошарок доступу до даних складається з двох проектів: один для зберігання коду EF Core (похідний DbContext, фабрика контексту, репозиторії, міграції тощо), а інший для зберігання сутностей і моделей перегляду. Створіть нове рішення під назвою AutoLotSolution, додайте до нього бібліотеку класів .NET Core під назвою AutoLot.Models і додайте до проекту пакети.

Створимо теку AutoLotSolution і вній виконаємо.

```console
dotnet new sln 
dotnet new classlib -n AutoLot.Models
dotnet sln add .\AutoLot.Models
dotnet add AutoLot.Models package Microsoft.EntityFrameworkCore
dotnet add AutoLot.Models package Microsoft.EntityFrameworkCore.SqlServer
dotnet add AutoLot.Models package System.Text.Json
```

Пакет Microsoft.EntityFrameworkCore.Abstractions надає доступ до багатьох конструкцій EF Core (наприклад, анотацій даних), має меншу вагу, ніж пакет Microsoft.EntityFrameworkCore, і зазвичай використовується для модельних проектів. Однак підтримка нової функції IEntityTypeConfiguration<T> є не в пакеті Abstractions, а в повному пакеті EF Core.

Додайте до рішення інший проект бібліотеки класів .NET Core під назвою AutoLot.Dal. Додайте пакети.

```console
dotnet new classlib -n AutoLot.Dal
dotnet sln add .\AutoLot.Dal
dotnet add AutoLot.Dal package Microsoft.EntityFrameworkCore
dotnet add AutoLot.Dal package Microsoft.EntityFrameworkCore.Design
```

Додайте посилання на проект AutoLot.Models.
```console
dotnet add AutoLot.Dal reference AutoLot.Models
```
Якщо ви не використовуєте машину під керуванням Windows, налаштуйте роздільник каталогів для вашої операційної системи в попередніх командах.

Оновіть файл проекту AutoLot.Dal, щоб увімкнути доступ до моделі часу розробки під час виконання. Оновіть метадані для пакета Microsoft.EntityFrameworkCore.Design до наступного.

```xml
      <!--<IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>-->
```
Ця зміна необхідна для очищення тимчасових таблиць, розглянутих у розділі «Ініціалізація даних»

### Додайте представлення бази даних 

Перш ніж риштувати(scaffolding) для сутностей і похідного DbContext з бази даних, додайте користувацьке представлення бази даних до бази даних AutoLot, яке використовуватиметься далі. Ми додаємо його зараз, щоб продемонструвати підтримку scaffolding для представлень. Підключіться до бази даних AutoLot (за допомогою SQL Server Management Studio або Azure Data Studio) і виконайте такий оператор SQL.

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
Якщо у вас немає бази даних див. главу присвячену ADO.NET та Взаємодія з DBMS.


## Створення каркасу(scaffolding) DbContext і Entities

Наступним кроком є створення похідного DbContext та сутностей бази даних AutoLot за допомогою інструментів EF Core CLI. Перейдіть до каталогу проекту AutoLot.Dal у командному рядку. Використовуйте інструменти EF Core CLI, щоб створити для базу даних AutoLot каркаси сутностей та похідний від DbContext клас за допомогою наступної команди, оновлюючи рядок підключення за потреби (всі в одному рядку):

```console
dotnet ef dbcontext scaffold "Server=(localdb)\mssqllocaldb;Database=AutoLot;Trusted_Connection=True;ConnectRetryCount=0" Microsoft.EntityFrameworkCore.SqlServer --data-annotations --context ApplicationDbContext --context-namespace AutoLot.Dal.EfStructures --context-dir EfStructures --no-onconfiguring --namespace AutoLot.Models.Entities --output-dir ..\AutoLot.Models\Entities --force
```

Попередня команда створює каркаси(scaffold) для бази даних, розташовану в наданому рядку підключення, використовуючи постачальника бази даних SQL Server. Прапор –data-annotations призначений для визначення пріоритетності анотацій даних, де це можливо (над Fluent API). --context називає контекст, --context-namespaces визначає простір імен для контексту, --context-dir вказує каталог (щодо поточного проекту) для створення контексту, --no-onconfiguring запобігає стоврення методу OnConfiguring, --output-dir є вихідним каталогом для сутностей (відносно каталогу проекту), а --namespace визначає простір імен для сутностей. Ця команда розміщує всі сутності в проекті AutoLot.Models у папці Entities, а клас ApplicationDbContext.cs — у папці EfStructures проекту AutoLot.Dal. Останній параметр (--force) використовується для примусового перезапису будь-яких існуючих файлів.
Команди EF Core CLI були детально розглянуті в розділі Entity Framework Core > Перший погляд.

### Розгляд результату scaffold.

Після виконання команди ви побачите шість сутностей у проекті AutoLot.Models (у папці Entities) і один похідний DbContext у проекті AutoLot.Dal (у папці EfStructures). Кожна таблиця риштується в клас сутності C# і додається як властивість DbSet<T> до похідного DbContext. Представлення риштується в сутності без ключа, додаються як DbSet<T> і зіставляються з відповідним представленням бази даних за допомогою Fluent API.
Команда scaffold, яку ми використовували, вказала прапорець --data-annotations, щоб надавати перевагу анотаціям над Fluent API. Вивчаючи створені класи, ви помітите, що в анотаціях є кілька промахів. Наприклад, властивості TimeStamp не мають атрибута [Timestamp], а натомість налаштовані як RowVersion ConcurrencyTokens у Fluent API.

Вважаю, наявність анотацій у класі робить код більш читабельним, ніж наявність усієї конфігурації у Fluent API. Якщо ви віддаєте перевагу використанню Fluent API, видаліть параметр –data-annotations із команди.

## Перемикання на Code First

Тепер, коли у вас є риштування база даних, в похідний DbContext і сутності, настав час переключитися з Database First на Code First. Процес не складний, але його не варто виконувати регулярно. Краще визначитися з парадигмою і дотримуватися її. Більшість гнучких команд віддають перевагу Code First, оскільки новий дизайн програми та її об’єктів перетікає в базу даних. Процес, який ми тут розглядаємо, імітує запуск нового проекту за допомогою EF Core, орієнтованого на існуючу базу даних.
Етапи, пов’язані з переходом, передбачають створення фабрики DbContext (для інструментів CLI), створення початкової міграції для поточного стану графа об’єктів, а потім видалення бази даних і повторне створення бази даних за допомогою або міграція або «підробки», застосована шляхом обману EF Core.

### Створення DbContext Design-Time Factory

Якшо ви виконаєте команду в рядку в проекті AutoLot.Dal вона не спрацює і вкаже причину.
```console
dotnet ef dbcontext info
```

Як ви пам’ятаєте з попередніх розділів EF Core, IDesignTimeDbContextFactory використовується інструментом CLI EF Core для створення екземпляра похідного класу DbContext. Створіть новий файл класу під назвою ApplicationDbContextFactory.cs у проекті AutoLot.Dal у каталозі EfStructures. Деталі фабрики були розглянуті в попередньому розділі, тому я просто збираюся перелічити код тут. Обов’язково оновіть рядок підключення відповідно до середовища.

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

### Створення початкової міграцію

Під час першої міграції буде створено три файли: два файли для самої міграції та повний знімок моделі. Введіть наступне в командному рядку (у каталозі AutoLot.Dal), щоб створити нову міграцію з іменем Initial (використовуючи екземпляр ApplicationDbContext, який щойно створили), і розмістіть файли міграції в папці EfStructures\Migrations проекту AutoLot.Dal:

```console
dotnet ef migrations add Initial -o EfStructures\Migrations
```
Важливо переконатися, що жодних змін не застосовано до риштованих файлів або бази даних, доки не буде створено та застосовано цю першу міграцію. Зміни з будь-якої сторони призведуть до розсинхронізації коду та бази даних. Після застосування всі зміни в базі даних потрібно завершити за допомогою міграції EF Core.

Щоб підтвердити, що міграцію створено та очікує на застосування, виконайте команду list.

```console
PS D:\...\AutoLot.Dal> dotnet ef migrations list
Build started...
Build succeeded.
Server=(localdb)\mssqllocaldb;Database=AutoLot;Trusted_Connection=True;ConnectRetryCount=0
20241125090346_Initial (Pending)
```
Результат покаже, що початкова міграція очікує на розгляд. Рядок підключення відображається у вихідних даних завдяки Console.Writeline() у методі CreateDbContext().

### Застосування міграції

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

## Створення файлів GlobalUsings

Перейменуйте файли Class1.cs у проектах AutoLot.Dal і AutoLot.Models на GlobalUsings.cs. Очистіть увесь код у кожному з файлів і замініть їх наступним чином:

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


## Доопрацювання сутностей(Entities) та моделей перегляду(ViewModel)

Цей розділ оновлює риштовані сутності до їх остаточної версії, додає додаткові сутності з попередніх двох розділів і додає сутність журналювання(logging entity).

## Сутності

У каталозі Entities проекту AutoLot.Models ви знайдете шість файлів, по одному для кожної таблиці в базі даних і один для перегляду бази даних. Зауважте, що назви є одниною, а не множиною (як у базі даних). Як випливає з назви, засіб множинності відображає однину імен сутностей у множину імен таблиць і навпаки. У попередніх розділах детально розглядалися угоди EF Core, анотації даних і Fluent API, тому більшу частину цього розділу складатимуть списки кодів із короткими описами.

### Клас BaseEntity

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
Усі сутності (крім сутності журналювання) буде оновлено для використання цього базового класу в наступних розділах. Властивість Timestamp покищо закоментовано тому що при застосувані міграції виконається помилка оскільки в таблицях вже є такий стовпецб з іншим типом і зміна бази даних виконається з помилкою. Коли всі сутності оновляться і буде виделено попередні стовпці ми дадамо стовпець до всіх сутностей.

Додайте простір імен до GlobalUsings обох проектів.

```cs
global using AutoLot.Models.Entities.Base;
```

### Owned сутність Person

Усі сутності Customer, CreditRisk і Driver мають властивості FirstName і LastName. Сутності, які мають однакові властивості в кожному, можуть отримати вигоду від Owned класу. Хоча переміщення цих двох властивостей у Owned клас є дещо тривіальним прикладом, Owned сутності допомагають зменшити дублювання коду та підвищити узгодженість.

Створіть новий каталог під назвою Owned у каталозі Entities проекту AutoLot.Models.У цьому новому каталозі створіть новий файл під назвою Person.cs.
```cs
namespace AutoLot.Models.Entities.Owned;
[Owned]
public class Person
{
    [Required,StringLength(50)]
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

### Сутності Car 

Таблицю Inventory було риштовано в клас сутності під назвою Inventory. Ми збираємося змінити ім’я сутності на Car, залишивши назву таблиці. Це приклад зіставлення сутності з таблицею з іншою назвою. Це легко виправити: змініть ім’я файлу на Car.cs і ім’я класу на Car. Атрибут Table уже застосовано правильно, тому просто додайте схему dbo. Зауважте, що параметр схеми є необов’язковим, оскільки SQL Server за замовчуванням має dbo, але я включив його для повноти. Простори імен також можна видалити, оскільки вони охоплюються глобальними просторами імен.

Далі успадкуйте від BaseEntity та видаліть властивості Id (і його атрибут) і TimeStamp, а також конструктор.

```cs
[Table("Inventory",Schema = "dbo")]
[Index("MakeId", Name = "IX_Inventory_MakeId")]
public partial class Car : BaseEntity
{
    public int MakeId { get; set; }

    [StringLength(50)]
    public string Color { get; set; } = null!;

    [StringLength(50)]
    public string PetName { get; set; } = null!;

    [ForeignKey("MakeId")]
    [InverseProperty("Cars")]
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
    public DateTime? DateBuild { get; set; }
```
Атрибут DisplayName використовується ASP.NET Core і буде розглянуто в цьому розділі.

Властивість Make navigation потрібно перейменувати на MakeNavigation, а інверсна властивість використовує магічний рядок замість методу C# nameof().

```cs
    [ForeignKey(nameof(MakeId)]
    [InverseProperty(nameof(Make.Cars))]
    public virtual Make MakeNavigation { get; set; } = null!;
```
Це яскравий приклад того, чому назвати властивість так само, як ім’я класу, стає проблематично. Якщо назву властивості залишити Make, то функція nameof не працюватиме належним чином, оскільки Make (у цьому випадку) посилається на властивість, а не на тип.

Риштованний код для властивості навігації Orders потребує оновлення, оскільки всі властивості навігації посилань матимуть суфікс Navigation, доданий до їхніх імен. Зміна цієї навігаційної властивості полягає в тому, що тип властивості введено як IEnumerable<Order> замість ICollection<Order> та ініціалізовано новим List<Order>.Це не є обов’язковою зміною, оскільки ICollection<Order> також працюватиме. Я віддаю перевагу використанню нижчого рівня IEnumerable<T> у властивостях навігації колекції (оскільки IQueryable<T> та ICollection<T> обидва походять від IEnumerable<T>).

```cs
    [InverseProperty(nameof(Order.CarNavigation))]
    public virtual IEnumerable<Order> Orders { get; set; } = new List<Order>();
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
        return $"{PetName ?? "No name"} is a {Color} {MakeNavigation?.Name} with Id:{Id}";
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
Оскільки клас Inventory було перейменовано на Car, клас ApplicationDbContext потрібно оновити. Знайдіть властивість DbSet<Inventory> і оновіть рядок до такого:

```cs
    public virtual DbSet<Car> Cars { get; set; }
```
Створимо клас конфігурації

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

        builder.Property(e => e.DateBuild).HasDefaultValueSql("getdate()");

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
    //...

    new CarConfiguration().Configure(modelBuilder.Entity<Car>());

    //...
}
```

У класі Car додайте атрибут EntityTypeConfiguration

```cs
[Table("Inventory",Schema = "dbo")]
[Index("MakeId", Name = "IX_Inventory_MakeId")]
[EntityTypeConfiguration(typeof(CarConfiguration))]
public partial class Car : BaseEntity
{
    //...
}    
```
Це ше не всі зміни cутності Car, але можно виконати першу міграцію стосовно цього класу.

```console
dotnet ef migrations add ChangeCar_1
dotnet ef database update
```

Додамо до рішеня проект типу ConsoleApp з назвою SimpleTest з посиланням на проект AutoLot.Dal.

Перевіримо роботу з БД.

```cs
static void Test_Make_Car()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    Make make = new Make()  {  Name = "VW" };
    context.Makes.Add(make);

    Car car = new() {  MakeNavigation = make, Color = "Navy", PetName = "Wolf" };
    context.Cars.Add(car);

    context.SaveChanges();

    Console.WriteLine($"{make.Id} {make.Name}");
    Console.WriteLine(car);

    context.Cars.Remove(car);
    context.Makes.Remove(make);
    context.SaveChanges();
}
Test_Make_Car();
```
```console
1 VW
Wolf is a Navy VW with Id:1
```

### Сутність Driver , CarDriver та відношеня Many-To-Many між Car і Driver

#### Сутність Driver

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

#### Сутність CarDriver

Продовжуючи налаштування зв’язку «Many-To-Manyх» між автомобілем і водієм, додайте новий клас під назвою CarDriver.

```cs
namespace AutoLot.Models.Entities;

[Table("InventoryToDrivers",Schema ="dbo")]
public class CarDriver :BaseEntity
{
    public int DriverId { get; set; }
    [ForeignKey(nameof(DriverId))]
    public virtual Driver DriverNavigation { get; set; }
    [Column("InventoryId")]
    public int CarId { get; set; }
    [ForeignKey(nameof(CarId))]
    public Car CarNavigation { get; set; }
}
```
Оскільки це нова таблиця, нову властивість DbSet<CarDriver> необхідно додати до класу ApplicationDbContext..

```cs
    public DbSet<CarDriver> CarsToDrivers { get; set; }
```
Далі створемо файл конфігурації. Через фільтр запиту для некерованих автомобілів у класі Car пов’язані таблиці (CarDriver і Order) повинні мати однаковий фільтр запиту, застосований до їхніх властивостей навігації. Додайте новий файл із назвою CarDriverConfiguration.cs у папку Configuration та оновіть код до такого:

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

#### Створення ввідношеня Many-To-Many між Car і Driver

В класі Driver додамо властивість Cars.

```cs
    public virtual IEnumerable<Car> Cars { get; set; } = new List<Car>();
```

В клас Car додайте властивість навігації колекції для сутності Driver та CarDriver:
```cs
[InverseProperty(nameof(Driver.Cars))]
public virtual IEnumerable<Driver> Drivers { get; set; } = new List<Driver>();
[InverseProperty(nameof(CarDriver.CarNavigation))]
public virtual IEnumerable<CarDriver> CarDrivers { get; set; } = new List<CarDriver>();
```

В класі CarConfiuration в методі Configure додамо визначеня відносин.
```cs
  builder
    .HasMany(p => p.Drivers)
    .WithMany(p => p.Cars)
    .UsingEntity<CarDriver>(
      j => j
        .HasOne(cd => cd.DriverNavigation)
        .WithMany(d => d.CarDrivers)
        .HasForeignKey(nameof(CarDriver.DriverId))
        .HasConstraintName('FK_InventoryDriver_Drivers_DriverId')
        .OnDelete(DeleteBehavior.Cascade),
      j => j
        .HasOne(cd => cd.CarNavigation)
        .WithMany(c => c.CarDrivers)
        .HasForeignKey(nameof(CarDriver.CarId))
        .HasConstraintName('FK_InventoryDriver_Inventory_InventoryId')
        .OnDelete(DeleteBehavior.ClientCascade),
      j =>
        {
          j.HasKey(cd => new { cd.CarId, cd.DriverId });
        });
```

Перивіримо зміни
Додамо міграцію і оновимо базу

```console
dotnet ef migrations add AddDriverManyToMany
dotnet ef database drop -f
dotnet ef database update
```
Метод додає запис Car, Driver та зв'язок між ними а потім завантажує їх з БД.

```cs
static void Test_Car_Driver()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    // Create
    Make make = new Make() { Name = "VW" };
    context.Makes.Add(make);
    Car? car = new() { MakeNavigation = make, Color = "Navy", PetName = "Wolf" };
    context.Cars.Add(car);

    Driver driver = new Driver()
    {
        PersonInformation =
        new Person { FirstName = "John", LastName = "Conor" }
    };

    ((List<Driver>)car.Drivers).Add(driver);
    context.SaveChanges();


    // Read
    Car? car_1 = context.Cars
        .Where(c => c.PetName == "Wolf")
        .Include("Drivers").First();

    Driver? driver_1 = car_1.Drivers.First();

    Console.WriteLine(car_1);
    Console.WriteLine($"{driver_1.Id} {driver_1.PersonInformation.FullName}");

    context.Cars.Remove(car);
    context.Drivers.Remove(driver);
    context.SaveChanges();
}
Test_Car_Driver();
```
```console
Wolf is a Navy VW with Id:1
1 Conor, John
```

### Сутність Radio та відношення One-To-One між Car і Radio

Додайте новий файл із назвою Radio.cs до папки Entities:

```cs
namespace AutoLot.Models.Entities;

public class Radio : BaseEntity
{
    public bool HasTweeters { get; set; }
    public bool HasSubWoofers { get; set; }
    [Required, StringLength(50)]
    public string RadioId { get; set; }
    [Column("InventoryId")]
    public int CarId { get; set; }
    [ForeignKey(nameof(CarId))]
    public virtual Car CarNavigation { get; set; }
}
```
Додайте нову властивість до класу ApplicationDbContext

```cs
    public DbSet<Radio> Radios { get; set; }
```

#### Створення відношення One-To-One між Car і Radio 

У клас Car додайте властивість навігації посилання для сутності Radio:
```cs
[InverseProperty(nameof(Radio.CarNavigation))]
public virtual Radio RadioNavigation { get; set; }
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
[Table("Radios", Schema = "dbo")]
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
    Make make = new Make() { Name = "VW" };
    context.Makes.Add(make);
    Car? car = new() { MakeNavigation = make, Color = "Navy", PetName = "Wolf" };

    car.RadioNavigation = new Radio
    {
        HasTweeters = true,
        HasSubWoofers = true,
        RadioId ="RDV23451",
    };

    context.Cars.Add(car);
    context.SaveChanges();

    var radio = context.Radios.First();
    Console.WriteLine($"{radio.Id} {radio.RadioId}");
    Console.WriteLine(radio.CarNavigation);

    context.Radios.Remove(radio);
    context.Cars.Remove(car);
    context.SaveChanges();
}
Test_Car_Radio();
```
```console
1 RDV23451
Wolf is a Navy VW with Id:1
```

### Сутність Customer

Таблицю Customers було риштовано в клас сутності під назвою Customer. Успадкуйте від BaseEntity та видаліть властивості Id і TimeStamp. Додайте атрибут Table зі схемою. Видаліть властивості FirstName і LastName, оскільки їх буде замінено Owned сутністю Person.

```cs
namespace AutoLot.Models.Entities;
[Table("Customers",Schema ="dbo")]
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
Атрибути зворотної властивості потрібно оновити за допомогою суфікса навігації, а типи змінити на IEnumerable<T> та ініціалізувати.

```cs
namespace AutoLot.Models.Entities;
[Table("Customers",Schema ="dbo")]
public partial class Customer : BaseEntity
{
    public Person PersonInformation { get; set; } = new Person();

    [InverseProperty(nameof(CreditRisk.CustomerNavigation))]
    public virtual IEnumerable<CreditRisk> CreditRisks { get; set; } = new List<CreditRisk>();

    [InverseProperty(nameof(Order.CustomerNavigation))]
    public virtual IEnumerable<Order> Orders { get; set; } = new List<Order>();
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
[Table("Customers",Schema ="dbo")]
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
Перевіримо  сутність.

```console
dotnet ef migrations add ChangeCustomer
dotnet ef database update
```
```cs
static void Test_Customer()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    Customer customer = new Customer()
    {
        PersonInformation = new Person { FirstName = "John", LastName = "Conor" }
    };

    context.Customers.Add(customer);
    context.SaveChanges();

    Customer customer_1 = context.Customers.First();
    Console.WriteLine($"{customer_1.Id} {customer_1.PersonInformation.FullName}");

    context.Customers.Remove(customer);
    context.SaveChanges();
}
Test_Customer();
```
```console
1 Conor, John
```

### Сутність Make

Таблиця Makes була створена для класу сутності під назвою Make. Успадкуйте від BaseEntity та видаліть властивості Id і TimeStamp. Додайте атрибут Table зі схемою. Pvsysnm на IEnumerable властивість навігації на коллекцію та додайте ініціалізатор.

```cs
namespace AutoLot.Models.Entities;
[Table("Makes",Schema ="dbo")]
public partial class Make :BaseEntity
{

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [InverseProperty(nameof(Car.MakeNavigation))]
    public virtual IEnumerable<Car> Cars { get; set; } = new List<Car>();
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
    Make make = new Make { Name = "BMW" };
    context.Makes.Add(make);
    context.SaveChanges();

    Make make1 = context.Makes.First(m => m.Name == "BMW");
    Console.WriteLine($"{make1.Id} {make1.Name}");
    context.Makes.Remove(make);
    context.SaveChanges();
}
Test_Make();
```
```console
4 BMW
```
Також можна запустити попередьно зроблений метод Test_Make_Car

```console
5 VW
Wolf is a Navy VW with Id:3
```

### Сутність CreditRisk

Таблицю CreditRisks було риштовано в клас сутності під назвою CreditRisk. Успадкуйте від BaseEntity та видаліть властивості Id і TimeStamp. Додайте атрибут Table зі схемою. Видаліть властивості FirstName і LastName. Виправте властивість навігації за допомогою методу nameof() в атрибуті InverseProperty. Додати Owned власність. Відносини будуть далі налаштовані в API Fluent.

```cs
namespace AutoLot.Models.Entities;

[Table("CreditRisks",Schema ="dbo")]
[Index("CustomerId", Name = "IX_CreditRisks_CustomerId")]
public partial class CreditRisk :BaseEntity
{
    public Person PersonInformation { get; set; } = new Person();
    public int CustomerId { get; set; }

    [ForeignKey("CustomerId")]
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
        PersonInformation = new Person { FirstName = "John", LastName = "Conor" }
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

    CreditRisk creditRisk1 = context.CreditRisks.Find(creditRisk.Id);
    Console.WriteLine($"" +
        $"{creditRisk1.Id} " +
        $"{creditRisk1.PersonInformation.FirstName} " +
        $"{creditRisk1.PersonInformation.LastName}");

    context.CreditRisks.Remove(creditRisk);
    context.Customers.Remove(customer);
    context.SaveChanges();
}
Test_CreditRisk();
```
```console
1 John Conor
```

### Сутність Order

Таблицю Orders було риштовано в клас сутності під назвою Order. Успадкуйте від BaseEntity та видаліть властивості Id і TimeStamp. Змінить параметри для ForeignKey та InverseProperty використавши nameof

```cs
namespace AutoLot.Models.Entities;

[Table("Orders",Schema ="dbo")]
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
Створимо файл конфігурації. В конфігурації вказані зв'язки і те шо табличя буде часовою.

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

Оновимо БД і перевіримо її здатність.

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
        PersonInformation = new Person { FirstName = "John", LastName = "Conor" }
    };

    Make make = new Make() { Name = "VW" };
    Car car = new() { MakeNavigation = make, Color = "Navy", PetName = "Wolf" };

    Order order = new Order
    {
        CustomerNavigation = customer,
        CarNavigation = car
    };

    context.Orders.Add(order);
    context.SaveChanges();

    Order order1 = context.Orders
        .Include(o=>o.CarNavigation)
        .Include(o=>o.CustomerNavigation)
        .First();

    Console.WriteLine($"" +
        $"{order1.CarNavigation.PetName} " +
        $"{order1.CustomerNavigation.PersonInformation.FirstName}");

    context.Orders.Remove(order);
    context.Cars.Remove(car);
    context.Customers.Remove(customer);
    context.SaveChanges();
}
Test_Order();
```
```
Wolf John
```

### Додавання властивості TimeStamp в BaseEntity.

Коли в базі даних існує ствопець TimeStamp і ми хочемо створити міграцію, EF Core створює запроси які збираються змінити існуючий за допомогою alter. SQL Server не дозволяє змінівати стовпці типу TimeStamp. Ми по сутіли виділи TimeStamp з кожної сутності коли успадкували від BaseEntity. Для того додати TimeStamp треба зняти коментар в класі BaseEntity.

```cs
public abstract class BaseEntity
{
    [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Timestamp]
    public byte[] TimeStamp { get; set; }
}
```
Створимо створимо міграцію.

```console
dotnet ef migrations add ChangeBaseEntityAddTimeStamp
```
Для того шоб застосувати міграцію требо очистити БД.

```console
dotnet ef database drop -f
dotnet ef database update
```
Після цього можна запустити всі тестові методи вони повині спрацювати.

### Сутність SeriLogEntry

Базі даних потрібна додаткова таблиця для зберігання записів журналу. Проекти ASP.NET Core, наведені далі , використовують framework журналювання SeriLog, і одним із варіантів є запис логів журналу в таблицю SQL Server. Ми збираємося додати таблицю зараз, знаючи, що вона буде використовуватися через кілька розділів.
Таблиця не пов’язана з іншими таблицями та не використовує клас BaseEntity.Додайте новий файл класу під назвою SeriLogEntry.cs у папку Entities.

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

