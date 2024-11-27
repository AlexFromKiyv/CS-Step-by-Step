# Створення Data Access Layer з EF Core

Цей розділ присвячено застосуванню того, що ви дізналися про EF Core, для створення рівня доступу до даних AutoLot. Цей розділ починається зі створення одного проекту для сутностей і іншого для коду бібліотеки доступу до даних. Відокремлення моделей від коду доступу до даних є звичайним проектним рішенням, яке використано в ASP.NET Core.

Наступним кроком є ​​об’єднання наявної бази даних із розділу ADO.NET на сутності та похідний DbContext за допомогою інтерфейсу командного рядка (CLI) EF Core. Це демонструє процес database first. Потім проект змінюється на code first, де дизайн бази даних керується сутностями C#. 
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

Перш ніж створювати каркаси(scaffolding) для сутностей і похідного DbContext з бази даних, додайте користувацьке представлення бази даних до бази даних AutoLot, яке використовуватиметься далі. Ми додаємо його зараз, щоб продемонструвати підтримку scaffolding для представлень. Підключіться до бази даних AutoLot (за допомогою SQL Server Management Studio або Azure Data Studio) і виконайте такий оператор SQL.

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

Після виконання команди, щоб риштувати базу даних у класи C#, ви побачите шість сутностей у проекті AutoLot.Models (у папці Entities) і один похідний DbContext у проекті AutoLot.Dal (у папці EfStructures). Кожна таблиця риштується в клас сутності C# і додається як властивість DbSet<T> до похідного DbContext. Представлення риштується в сутності без ключа, додаються як DbSet<T> і зіставляються з відповідним представленням бази даних за допомогою Fluent API.
Команда scaffold, яку ми використовували, вказала прапорець --data-annotations, щоб надавати перевагу анотаціям над Fluent API. Вивчаючи створені класи, ви помітите, що в анотаціях є кілька промахів. Наприклад, властивості TimeStamp не мають атрибута [Timestamp], а натомість налаштовані як RowVersion ConcurrencyTokens у Fluent API.

Вважаю, наявність анотацій у класі робить код більш читабельним, ніж наявність усієї конфігурації у Fluent API. Якщо ви віддаєте перевагу використанню Fluent API, видаліть параметр –data-annotations із команди.

## Перемикання на Code First

Тепер, коли у вас є риштування база даних, в похідний DbContext і сутності, настав час переключитися з Database First на Code First. Процес не складний, але його не варто виконувати регулярно. Краще визначитися з парадигмою і дотримуватися її. Більшість гнучких команд віддають перевагу Code First, оскільки новий дизайн програми та її об’єктів перетікає в базу даних. Процес, який ми тут розглядаємо, імітує запуск нового проекту за допомогою EF Core, орієнтованого на існуючу базу даних.
Етапи, пов’язані з переходом, передбачають створення фабрики DbContext (для інструментів CLI), створення початкової міграції для поточного стану графа об’єктів, а потім видалення бази даних і повторне створення бази даних за допомогою або міграція або «підробка», застосована шляхом обману EF Core.

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
    public ApplicationDbContext CreateDbContext(string[] args)
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
В стоареному скріпті все необхідне для застосування міграцвї на стороні сервера БД.
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