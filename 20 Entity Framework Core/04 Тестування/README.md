# Тестування

Тепер, коли у вас є готовий рівень доступу до даних AutoLot, настав час провести його тест-драйв. Тестування інтеграції є невід’ємною частиною розробки програмного забезпечення та є чудовим способом переконатися, що ваш код доступу до даних поводиться як ви очікували. У цьому розділі ми будемо використовувати xUnit, тестову структуру для .NET Core.

## Створення проекту тестування

Хоча ми зробили проект з декількома тестами вони мало чого охоплюють. Щоб створювати клієнтську програму для тестування завершеного рівня доступу до даних AutoLot, ми збираємося використовувати автоматизовані тести інтеграції. Тести продемонструють створення, читання, оновлення та видалення викликів бази даних. Це дозволяє нам досліджувати код без накладних витрат на створення іншої програми. Кожен із тестів у цьому розділі виконає запит (створити, прочитати, оновити або видалити), а потім матиме один або кілька операторів Assert для підтвердження того, що результат відповідає очікуванням.

Модульні тести призначені для перевірки окремої одиниці коду. Те, що ми будемо робити протягом цього розділу, — це технічно створювати інтеграційні тести, оскільки ми тестуємо код C# і EF Core на всьому шляху до бази даних і назад.

Для початку ми збираємося налаштувати платформу тестування інтеграції за допомогою xUnit, сумісної з .NET Core системи тестування. Почніть із додавання нового тестового проекту типу xUnit під назвою AutoLot.Dal.Tests. У Visual Studio цей тип проекту називається xUnit Test Project. 

Створимо папку AutoLotTesting і запустимо в ній:

```console
dotnet new sln 
dotnet new xunit -n AutoLot.Dal.Tests
dotnet sln add AutoLot.Dal.Tests
```
Додамо NuGet пакети до проекту AutoLot.Dal.Tests. Для CLI це наступні команди.

```console
dotnet add AutoLot.Dal.Tests package Microsoft.EntityFrameworkCore
dotnet add AutoLot.Dal.Tests package Microsoft.EntityFrameworkCore.Design
dotnet add AutoLot.Dal.Tests package Microsoft.EntityFrameworkCore.SqlServer
dotnet add AutoLot.Dal.Tests package Microsoft.Extensions.Configuration.Json
```
У тестах використовуватиметься код ініціалізації даних, який очищає тимчасові дані, тому таке ж налаштування файлу проекту необхідно внести щодо пакета Microsoft.EntityFrameworkCore.Design. Оновіть пакет, щоб видалити (або закоментувати) тег IncludeAssets:

```xml
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.0">
      <!--<IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>-->
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
```
Версії пакетів Microsoft.NET.Test.Sdk і coverlet.collector, які постачаються разом із шаблоном проекту xUnit, зазвичай відстають від поточних доступних версій. Щоб оновити їх, скористайтеся диспетчером пакетів NuGet у Visual Studio, щоб оновити всі пакети NuGet, або використовуйте CLI. Щоб оновити їх за допомогою CLI, потім додайте їх знову, оскільки додавання пакетів із командного рядка завжди отримуватиме останню версію без попереднього випуску. Ось команди:

```console
dotnet add AutoLot.Dal.Tests package Microsoft.NET.Test.Sdk
dotnet add AutoLot.Dal.Tests package coverlet.collector
```
Скопіюємо папку з попередьного розділу AutoLotSolution в папку де ми створили AutoLotTesting. Далі додайте посилання на проект до AutoLot.Models і AutoLot.Dal.
Щоб зробити це з командного рядка, виконайте наступне (можете оновіть шлях і роздільник каталогів для ваших проектів і на копіювати рішення):

```console
dotnet sln add ..\AutoLotSolution\AutoLot.Dal
dotnet sln add ..\AutoLotSolution\AutoLot.Models
dotnet add AutoLot.Dal.Tests reference ..\AutoLotSolution\AutoLot.Dal
dotnet add AutoLot.Dal.Tests reference ..\AutoLotSolution\AutoLot.Models
```
Для перевірки (або використання) методів і класів у проекті AutoLot.Dal, позначених як внутрішні, внутрішні елементи потрібно зробити видимими для проекту AutoLot.Dal.Tests.
Відкрийте файл AutoLot.Dal.csproj і додайте наступне:

```xml
<ItemGroup>
  <AssemblyAttribute Include='System.Runtime.CompilerServices.InternalsVisibleToAttribute'>
    <_Parameter1>AutoLot.Dal.Tests</_Parameter1>
  </AssemblyAttribute>
</ItemGroup>
```

Створіть новий файл із назвою GlobalUsings.cs у корені проекту AutoLot.Dal.Tests. Це буде центральне розташування для всіх операторів using, необхідних у цьому проекті.

```cs
global using System.Data;
global using System.Linq.Expressions;

global using AutoLot.Dal.EfStructures;
global using AutoLot.Dal.Exceptions;
global using AutoLot.Dal.Initialization;
global using AutoLot.Dal.Repos;
global using AutoLot.Dal.Repos.Interfaces;

//global using AutoLot.Dal.Tests.Base;

global using AutoLot.Models.Entities;
global using AutoLot.Models.Entities.Owned;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.ChangeTracking;
global using Microsoft.EntityFrameworkCore.Storage;
global using Microsoft.EntityFrameworkCore.Query;
global using Microsoft.Extensions.Configuration;

global using Xunit;
global using Xunit.Abstractions;
```

## Перший погляд на xUnit

Є два типи тестів, які будуть використані в цьому розділі. Методи тестування без параметрів називаються фактами (і використовують атрибут Fact). Тести, які приймають параметри, називаються теоріями (і використовують атрибут Theory). Теоретичні тести виконують кілька ітерацій методу тестування, передаючи різні значення для кожного циклу. Щоб продемонструвати ці типи тестів, створіть новий клас під назвою SampleTests.cs у проекті AutoLot.Dal.Tests.

```cs
namespace AutoLot.Dal.Tests;


public class SampleTests
{

}
```

### Тестовий метод Fact

Перший тест Fact. У тестах фактів усі значення містяться в методі тестування.

```cs
    [Fact]
    public void SimpleTestSum()
    {
        Assert.Equal(5,2+3);
    }
```
Доступні різні типи тверджень. У цьому прикладі тест стверджує, що фактичний результат (3+2) дорівнює очікуваному результату (5).

### Тестовий метод Theory

Під час використання тестів типу Theory значення для тестів передаються в метод тестування. Попередній тест перевіряв лише один випадок, 3+2. Theory дозволяють тестувати кілька випадків використання без повторення тестового коду кілька разів. Значення можуть надходити з атрибута InlineData, методів або класів. Для наших цілей ми будемо використовувати лише атрибут InlineData. Створіть наступний тест, який надає різні суммуючи та очікувані результати до тесту:

```cs
    [Theory]
    [InlineData(2,3,5)]
    [InlineData(1,-1,0)]
    public void SimpleTheoryTestSum(int addend1, int addend2,int expectedResult)
    {
        Assert.Equal(expectedResult,addend1+addend2);
    }
```

У цій главі буде багато прикладів перевірок фактів і теорії, а також додаткові можливості для xUnit framework. Для отримання додаткової інформації про xUnit framework зверніться до документації, розміщеної на https://xunit.net/.


### Виконання тестів

Хоча тести xUnit можна виконувати з командного рядка (за допомогою dotnet test), для розробників краще використовувати Visual Studio для виконання тестів. Запустіть Test Explorer із меню Test, щоб отримати доступ до запуску та налагодження всіх або вибраних тестів.

## Підготовка до тестування

### Налаштування проекту і екземплярів DBContext

Щоб отримати рядок підключення під час виконання, ми будемо використовувати можливості конфігурації .NET Core за допомогою файлу JSON. Додайте файл JSON із назвою appsettings.testing.json до проекту та додайте інформацію про рядок підключення до файлу в такому форматі (за потреби оновіть рядок підключення з перелічених тут даних):

```json
{
  "ConnectionStrings": {
    "AutoLot": "Server=(localdb)\\mssqllocaldb;Database=AutoLot;Trusted_Connection=True;ConnectRetryCount=0"
  }
}
```
Оновіть файл проекту, щоб файл налаштувань копіювався до вихідної папки під час кожної збірки. Зробіть це, додавши таку групу ItemGroup до файлу AutoLot.Dal.Tests.csproj:

```xml
  <ItemGroup>
    <None Update="appsettings.testing.json">
      <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </None>
  </ItemGroup>
```

### Створіть помічник тестування інтеграції

Клас TestHelpers оброблятиме конфігурацію програми, а також створюватиме нові екземпляри ApplicationDbContext. Додайте новий загальнодоступний статичний клас під назвою TestHelpers.cs у корені проекту:

```cs
namespace AutoLot.Dal.Tests;

public static class TestHelpers
{

}
```
Додайте публічний статичний метод, щоб створити екземпляр інтерфейсу IConfiguration за допомогою файлу appsettings.testing.json:

```cs
    public static IConfiguration GetConfiguration =>
        new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.testing.json",true,true)
        .Build();

```
Налаштування збірок було розглянуто в главі "Створення та налаштування бібліотек класів".

Додайте інший публічний статичний метод для створення екземплярів класу ApplicationDbContext за допомогою екземпляра IConfiguration.

```cs
    public static ApplicationDbContext GetContext(IConfiguration configuration)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var connectionString = configuration.GetConnectionString("AutoLot");
        optionsBuilder.UseSqlServer(connectionString);
        return new ApplicationDbContext(optionsBuilder.Options);
    }
```

Додайте інший статичний метод, який створить новий екземпляр ApplicationDbContext. Це демонструє створення екземпляра класу ApplicationDbContext з існуючого екземпляра для спільного використання підключення та транзакції.

```cs
    public static ApplicationDbContext GetSecondContext(ApplicationDbContext oldContext,
        IDbContextTransaction transaction)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(oldContext.Database.GetDbConnection());
        var context = new ApplicationDbContext(optionsBuilder.Options);
        context.Database.UseTransaction(transaction.GetDbTransaction());
        return context;
    }
```

### Додайте клас BaseTest

Клас BaseTest керуватиме інфраструктурою для тестів. Додайте нову папку під назвою Base до тестового проекту та додайте новий файл класу під назвою BaseTest.cs до цієї папки. Зробіть клас абстрактним і реалізуйте IDisposable. Додайте дві захищені властивості лише для читання, щоб зберігати екземпляри IConfiguration і ApplicationDbContext, а також утилізуйте екземпляр ApplicationDbContext у віртуальному методі Dispose().

```cs

namespace AutoLot.Dal.Tests.Base;

public abstract class BaseTest : IDisposable
{
    protected readonly IConfiguration Configuration;

    protected readonly ApplicationDbContext Context;

    public virtual void Dispose()
    {
        Context.Dispose();
    }
}
```
Платформа тестування xUnit забезпечує механізм запуску коду до та після виконання кожного тесту. Тестові класи (так звані фікстури), які реалізують інтерфейс IDisposable, виконуватимуть код у конструкторі класів у ланцюжку успадкування перед запуском кожного тесту. Це зазвичай називають тестовою установкою. Після виконання кожного тесту виконується код у методах Dispose (через ланцюжок успадкування). Це називається тестовим демонтажем.
Додайте protected конструктор, який створює екземпляр IConfiguration і призначає його змінній захищеного класу.
Використовуйте конфігурацію, щоб створити екземпляр ApplicationDbContext за допомогою класу TestHelper, а також призначити його змінній захищеного класу.
```cs
    protected BaseTest()
    {
        Configuration = TestHelpers.GetConfiguration;
        Context = TestHelpers.GetContext(Configuration);
    }
```
Інтерфейс ITestOutputHelper дозволяє записувати вміст у вікно виводу тесту. У разі використання шаблону IDisposable із тестовими приладами xUnit екземпляр для цього інтерфейсу можна вставити в конструктор. Додайте захищену змінну лише для читання, щоб утримувати примірник і оновлювати конструктор:

```cs
    protected BaseTest(ITestOutputHelper outputHelper)
    {
        Configuration = TestHelpers.GetConfiguration;
        Context = TestHelpers.GetContext(Configuration);
        OutputHelper = outputHelper;
    }
```

Два методи в класі BaseTest дозволяють запускати тестові методи в транзакції. Методи прийматимуть делегат Action як єдиний параметр, створюватимуть явну транзакцію (або залучатимуть існуючу транзакцію), виконуватимуть делегат Action, а потім відкочуватимуть транзакцію.

```cs
    protected void ExecuteInATransaction(Action actionToExecute)
    {
        var strategy = Context.Database.CreateExecutionStrategy();
        strategy.Execute(() =>
        {
            using var transaction = Context.Database.BeginTransaction();
            actionToExecute();
            transaction.Rollback();
        });
    }
```
Ми робимо це, щоб будь-які тести створення/оновлення/видалення залишали базу даних у тому стані, в якому вона була до запуску тесту. Транзакції виконуються всередині стратегії виконання, якщо ApplicationDbContext налаштовано, щоб увімкнути повторну спробу при тимчасових помилках.

Метод ExecuteInASharedTransaction() дозволяє кільком екземплярам ApplicationDbContext спільно використовувати одну транзакцію.

```cs
    protected void ExecuteInASharedTransaction(Action<IDbContextTransaction> actionToExecute)
    {
        var strategy = Context.Database.CreateExecutionStrategy();
        strategy.Execute(() =>
        {
            using IDbContextTransaction transaction = Context
            .Database.BeginTransaction(IsolationLevel.ReadUncommitted);
            actionToExecute(transaction);
            transaction.Rollback();
        });
    }
```

### Додайте тестовий клас фікстур EnsureAutoLotDatabase

Тестовий фреймворк xUnit забезпечує механізм для запуску коду перед виконанням будь-якого з тестів (називається fixture setup) і після виконання всіх тестів (називається fixture teardown). Зазвичай ця практика не рекомендована, але в нашому випадку ми хочемо переконатися, що база даних створена та завантажена даними перед виконанням будь-яких тестів, а не перед виконанням кожного тесту.
Тестові класи, які реалізують IClassFixture<T> where T: TestFixtureClass, виконуватимуть код конструктора T (TestFixtureClass) перед виконанням будь-яких тестів, а код Dispose() запускатиметься після завершення всіх тестів. 
Додайте новий клас під назвою EnsureAutoLotDatabaseTestFixture.cs до каталогу Base та реалізуйте IDisposable. Зробіть клас public і sealed і додайте наступні оператори використання: 

```cs
namespace AutoLot.Dal.Tests.Base;

public sealed class EnsureAutoLotDatabaseTestFixture : IDisposable
{

    public void Dispose()
    {
    }
}
```
Додайте конструктор. 

```cs
    public EnsureAutoLotDatabaseTestFixture()
    {
        var configuration = TestHelpers.GetConfiguration;
        var context = TestHelpers.GetContext(configuration);
        SampleDataInitializer.ClearAndSeedData(context);
        context.Dispose();
    }
```
Код конструктора використовує клас TestHelpers для отримання екземпляра IConfiguration, а потім отримує екземпляр ApplicationDbContext. Далі він викликає метод ClearAndReseedDatabase() із SampleDataInitializer. Останній рядок позбавляється примірника контексту. У наших прикладах метод Dispose() не має коду, але його потрібно реалізувати, щоб задовольнити інтерфейс IDisposable.

### Додайте інтеграційні тестові класи

Наступним кроком буде додавання класів, які будуть проводити автоматизовані тести. Ці класи називають test fixtures. Додайте нову папку під назвою IntegrationTests у папку AutoLot.Dal.Tests.

Залежно від можливостей виконавця тестів, тести xUnit виконуються послідовно в межах test fixture (класу), але паралельно між test fixture.Це може бути проблематично під час виконання інтеграційних тестів, які взаємодіють із базою даних. Паралельні тести бази даних з використанням одного екземпляра бази даних можуть спричиняти блокування, давати помилкові результати та, як правило, проблематичні. Виконання тесту xUnit можна змінити на послідовне виконання test fixture, додавши їх до однієї колекції тестів. Колекції тестів визначаються за назвою за допомогою атрибута Collection у класі. Наприклад можна додати такий атрібут

```cs
[Collection("Integration Tests")]
```

Додамо класи в папку IntegrationTests. Успадкувати від BaseTest і реалізувати інтерфейс IClassFixture. Додайте конструктор, щоб отримати екземпляр ITestOutputHelper і передати його базовому класу.

```cs
namespace AutoLot.Dal.Tests.IntegrationTests;

[Collection("Integration Tests")]
public class CarTests : BaseTest, IClassFixture<EnsureAutoLotDatabaseTestFixture>
{
    public CarTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }
}
```
```cs
namespace AutoLot.Dal.Tests.IntegrationTests;

[Collection("Integration Tests")]
public class CustomerOrderViewModelTests : BaseTest, IClassFixture<EnsureAutoLotDatabaseTestFixture>
{
    public CustomerOrderViewModelTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }
}
```
```cs
namespace AutoLot.Dal.Tests.IntegrationTests;

[Collection("Integration Tests")]
public class CustomerTests : BaseTest, IClassFixture<EnsureAutoLotDatabaseTestFixture>
{
    public CustomerTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }
}
```
```cs
namespace AutoLot.Dal.Tests.IntegrationTests;

[Collection("Integration Tests")]
public class MakeTests : BaseTest, IClassFixture<EnsureAutoLotDatabaseTestFixture>
{
    public MakeTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }
}
```
```cs
namespace AutoLot.Dal.Tests.IntegrationTests;

[Collection("Integration Tests")]
public class OrderTests : BaseTest, IClassFixture<EnsureAutoLotDatabaseTestFixture>
{
    public OrderTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }
}
```
Для класу CarTests оновіть конструктор, щоб створити екземпляр CarRepo та призначити екземпляр приватній змінній рівня класу лише для читання.

```cs
    private readonly ICarRepo _carRepo;
    public CarTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
        _carRepo = new CarRepo(Context);
    }
    public override void Dispose()
    {
        _carRepo.Dispose();
        base.Dispose();
    }
```
Повторіть процес для класу CustomerOrderViewModelTests, використовуючи натомість CustomerOrderViewModelRepo

```cs
    private readonly ICustomerOrderViewModelRepo _repo;
    public CustomerOrderViewModelTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
        _repo = new CustomerOrderViewModelRepo(Context);
    }

    public override void Dispose()
    {
        _repo.Dispose();
        base.Dispose();
    }
```
Налаштування для класу CustomerTests простіше і без додадкових змін, оскільки він не використовує CustomerRepo.

Той самий процес,як для CarTests, необхідний для класу MakeTests з використанням MakeRepo.

```cs
    private readonly IMakeRepo _repo;
    public MakeTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
        _repo = new MakeRepo(Context);
    }

    public override void Dispose()
    {
        _repo.Dispose();
        base.Dispose();
    }
```
Потрібно оновити репозиторій у класі OrderTests з використанням OrderRepo.

```cs
    private readonly IOrderRepo _repo;
    public OrderTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
        _repo = new OrderRepo(Context);
    }

    public override void Dispose()
    {
        _repo.Dispose();
        base.Dispose();
    }
```

## Здійснення запитів до бази даних

Згадайте, що створення екземплярів сутності з даних бази даних передбачає виконання оператора LINQ або оператора SQL (за допомогою FromSqlRaw()/FromSqlInterpolated()) до властивостей DbSet<T>. Під час використання LINQ оператори перетворюються на SQL постачальником бази даних і механізмом перекладу LINQ, а відповідні дані зчитуються з бази даних. Дані також можна завантажити за допомогою методу FromSqlRaw() або FromSqlInterpolated() з використанням необроблених рядків SQL і, за бажанням, додаткових операторів LINQ. Сутності, завантажені в колекції DbSet<T>, додаються до ChangeTracker за замовчуванням, але їх можна додавати без відстеження. Дані, завантажені в безключові колекції DbSet<T>, ніколи не відстежуються.
Якщо пов’язані сутності вже завантажено в DbSet<T>, EF Core з’єднає нові екземпляри за властивостями навігації. Наприклад, якщо Cars завантажуються в колекцію DbSet<Car>, а потім пов’язані замовлення завантажуються в DbSet<Order> того самого екземпляра ApplicationDbContext, властивість навігації Car.Orders поверне пов’язані сутності Order без повторного запиту до бази даних. Багато методів, продемонстрованих тут, мають доступні асинхронні версії. Синтаксис запитів LINQ структурно однаковий, тому я продемонструю лише несинхронну версію.

### Запити LINQ

Тип колекції DbSet<T> реалізує (серед інших інтерфейсів) IQueryable<T>. Це дозволяє використовувати команди C# LINQ для створення запитів на отримання даних із бази даних. Хоча всі оператори C# LINQ доступні для використання з типом колекції DbSet<T>, деякі оператори LINQ можуть не підтримуватися постачальником бази даних, а додаткові оператори LINQ додаються EF Core. Непідтримувані оператори LINQ, які не можна перекласти на мову запитів постачальника бази даних, створять виняткову ситуацію під час виконання. Деякі оператори LINQ, які не підлягають перекладу, виконуватимуться на стороні клієнта, якщо вони є останніми операторами в ланцюжку LINQ; однак інші (наприклад, оновлення методу Take(), який працює з діапазонами) все одно видаватиме помилку, якщо запит спочатку не буде виконано за допомогою ToList() або подібної конструкції.

Там, де доступний ToQueryString(), тести в наступному розділі встановлюють це значення для змінної (qs) і виводять результати тесту за допомогою ITestOutputHelper, щоб ви могли перевірити запит під час виконання тестів.

### Отримання всіх записів

Щоб отримати всі записи для таблиці, просто використовуйте властивість DbSet<T> безпосередньо без будь-яких операторів LINQ. Додайте такий факт до класу CustomerTests.cs:

```cs
    [Fact]
    public void SouldGetAllOfTheCustomers()
    {
        var query = Context.Customers;
        var customers = query.ToList();
        Assert.Equal(5, customers.Count);
    }
```

При запуску теста виникне помилка яка вкаже що не спрацьовує метод ClearData класу SampleDataInitializer.

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

Є можливість подивитись який запит був виконано. Змінемо метод.

```cs
    [Fact]
    public void SouldGetAllOfTheCustomers()
    {
        var query = Context.Customers;
        OutputHelper.WriteLine(query.ToQueryString());
        var customers = query.ToList();
        Assert.Equal(5, customers.Count);
    }
```
```sql
SELECT [c].[Id], [c].[TimeStamp], [c].[FirstName], [c].[FullName], [c].[LastName]
FROM [Customers] AS [c]
```
Той самий процес використовується для безключових сутностей, як-от CustomerOrderViewModel, яка налаштована на отримання даних із CustomerOrderView. Додайте наступний тест до класу CustomerOrderViewModelTest.cs, щоб показати отримання даних із представлення:

```cs
    [Fact]
    public void ShouldGetAllViewModels()
    {
        var query = Context.CustomerOrderViewModels;
        OutputHelper.WriteLine(query.ToQueryString());
        var list = query.ToList();
        Assert.NotEmpty(list);
        Assert.Equal(5,list.Count);
    }
```
```sql
    SELECT [c].[Color], [c].[DateBuilt], [c].[Display], [c].[FirstName], [c].[IsDrivable], [c].[LastName], [c].[Make], [c].[PetName], [c].[Price]
    FROM [CustomerOrderView] AS [c]
```

### Тест з фільтром записів.

Наступний тест у класі CustomerTests показує запити для клієнтів, де прізвище починається з W (незалежно від регістру):

```cs
    [Fact]
    public void ShouldGetCustomersWithLastNameStartWithW()
    {
        IQueryable<Customer> query = Context.Customers
            .Where(c => c.PersonInformation.LastName.StartsWith("W"));
        OutputHelper.WriteLine(query.ToQueryString());
        List<Customer> customers = query.ToList();
        
        Assert.Equal(2,customers.Count);
        foreach (var customer in customers)
        {
            Person? person = customer.PersonInformation;
            Assert.StartsWith("W",person.LastName, StringComparison.OrdinalIgnoreCase);
        }
    }
```
```sql
SELECT [c].[Id], [c].[TimeStamp], [c].[FirstName], [c].[FullName], [c].[LastName]
FROM [Customers] AS [c]
WHERE [c].[LastName] LIKE N'W%'
```
Наступний тест у класі CustomerTests демонструє ланцюжок методів Where() у запиті LINQ для пошуку клієнтів, у яких прізвище починається з W, а ім’я — з M. Зауважте, що оскільки SQL Server нечутливий до регістру, ці запити є також не враховує регістр:

```cs
[Fact]
public void ShouldGetCustomersWithLastNameStartWithWAndFirstNameStartWithM()
{
    IQueryable<Customer> query = Context.Customers
        .Where(c => c.PersonInformation.LastName.StartsWith("W"))
        .Where(c => c.PersonInformation.FirstName.StartsWith("M"));
    OutputHelper.WriteLine(query.ToQueryString());
    List<Customer> customers = query.ToList();

    Assert.Single(customers);
    foreach (var customer in customers)
    {
        Person? person = customer.PersonInformation;
        Assert.StartsWith("W", person.LastName, StringComparison.OrdinalIgnoreCase);
        Assert.StartsWith("M", person.FirstName, StringComparison.OrdinalIgnoreCase);
    }
}
```
Ми можемо зробити так, щоб тест у класі CustomerTests повторював той самий фільтр, використовуючи один метод Where() замість двох з’єднаних методів:

```cs
        IQueryable<Customer> query = Context.Customers
            //.Where(c => c.PersonInformation.LastName.StartsWith("W"))
            //.Where(c => c.PersonInformation.FirstName.StartsWith("M"));
            .Where(x => x.PersonInformation.LastName.StartsWith("W") &&
                           x.PersonInformation.FirstName.StartsWith("M"));
```
Обидва запити перекладаються в такий SQL:
```sql
SELECT [c].[Id], [c].[TimeStamp], [c].[FirstName], [c].[FullName], [c].[LastName]
FROM [Customers] AS [c]
WHERE [c].[LastName] LIKE N'W%' AND [c].[FirstName] LIKE N'M%'
```
Наступний тест у класі CustomerTests демонструє запити для клієнтів, де прізвище починається з W (незалежно від регістру) або прізвище починається з H (без урахування регістру):

```cs
    [Fact]
    public void ShouldGetCustomersWithLastNameStartWithWOrLastNameStartWithH()
    {
        IQueryable<Customer> query = Context.Customers
            .Where(c => c.PersonInformation.LastName.StartsWith("W") ||
                           c.PersonInformation.LastName.StartsWith("H"));
        OutputHelper.WriteLine(query.ToQueryString());
        List<Customer> customers = query.ToList();

        Assert.Equal(3, customers.Count);
        foreach (var customer in customers)
        {
            Person? person = customer.PersonInformation;
            Assert.True(
                    person.LastName.StartsWith("W",StringComparison.OrdinalIgnoreCase) 
                 || person.LastName.StartsWith("H",StringComparison.OrdinalIgnoreCase)
                );
        }
    }
```
Наступне в класі CustomerTests також запитує клієнтів, у яких прізвище починається з літери W (незалежно від регістру) або прізвище починається з літери H (не чутливе до регістру). Цей тест демонструє використання методу EF.Functions.Like(). Зауважте, що ви повинні самостійно вказати символ підстановки (%).

```cs
    [Fact]
    public void ShouldGetCustomersWithLastNameStartWithWOrLastNameStartWithHWithEFFunction()
    {
        IQueryable<Customer> query = Context.Customers
            .Where(c => EF.Functions.Like(c.PersonInformation.LastName,"W%")
            || EF.Functions.Like(c.PersonInformation.LastName, "H%"));
        OutputHelper.WriteLine(query.ToQueryString());
        List<Customer> customers = query.ToList();
        Assert.Equal(3, customers.Count);
    }
```
Зробимо простий тест для методу Find.

```cs
    [Fact]
    public void ShouldGetCustomersWithId1()
    {
        Customer? customer = Context.Customers.Find(1);

        Assert.Equal("Dave", customer?.PersonInformation.FirstName);
        Assert.Equal("Brenner", customer?.PersonInformation.LastName);
    }
```


Наступний тест у класі CarTests.cs використовує теорію для запиту кількості записів про автомобілі (придатні чи ні) у таблиці Inventory на основі вказаного MakeId:

```cs
    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 1)]
    [InlineData(3, 1)]
    [InlineData(4, 2)]
    [InlineData(5, 3)]
    [InlineData(6, 1)]
    public void ShouldGetTheCarsByMake(int makeId, int expectedCount)
    {
        IQueryable<Car> query = Context.Cars
            .IgnoreQueryFilters().Where(c=>c.MakeId ==  makeId);
        OutputHelper.WriteLine(query.ToQueryString());
        var cars = query.ToList();
        Assert.Equal(expectedCount,cars.Count());
    }
```
Кожен рядок InlineData стає унікальним тестом у програмі виконання тестів. У цьому прикладі обробляється шість тестів і виконується шість запитів до бази даних. Ось SQL з одного з тестів (єдина різниця в запитах з інших тестів у Теорії – це значення для MakeId):

```sql
DECLARE @__makeId_0 int = 1;

SELECT [i].[Id], [i].[Color], [i].[DateBuilt], [i].[Display], [i].[IsDrivable], [i].[MakeId], [i].[PeriodEnd], [i].[PeriodStart], [i].[PetName], [i].[Price], [i].[TimeStamp]
FROM [Inventory] AS [i]
WHERE [i].[MakeId] = @__makeId_0
```
Наступний тест використовує репозиторій CarRepo, щоб отримати кількість записів для кожної марки. Оскільки метод GetCarsBy() залишає фільтри запиту, стає на один запис менше, коли MakeId дорівнює одиниці.

```cs
    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(3, 1)]
    [InlineData(4, 2)]
    [InlineData(5, 3)]
    [InlineData(6, 1)]
    public void ShouldGetTheCarsByMakeUsingCarRepo(int makeId, int expectedCount)
    {
        var query = _carRepo.GetAllBy(makeId);
        OutputHelper.WriteLine(query.AsQueryable().ToQueryString());

        var cars = query.ToList();
        Assert.Equal(expectedCount, cars.Count());
    }
```
Переглядаючи створений запит, ви можете побачити фільтр запиту, який виключив записи, які IsDrivable == true:
```sql
DECLARE @__makeId_0 int = 1;

SELECT [i].[Id], [i].[Color], [i].[DateBuilt], [i].[Display], [i].[IsDrivable], [i].[MakeId], [i].[PeriodEnd], [i].[PeriodStart], [i].[PetName], [i].[Price], [i].[TimeStamp], [m].[Id], [m].[Name], [m].[PeriodEnd], [m].[PeriodStart], [m].[TimeStamp]
FROM [Inventory] AS [i]
INNER JOIN [Makes] AS [m] ON [i].[MakeId] = [m].[Id]
WHERE [i].[IsDrivable] = CAST(1 AS bit) AND [i].[MakeId] = @__makeId_0
ORDER BY [i].[PetName]
```