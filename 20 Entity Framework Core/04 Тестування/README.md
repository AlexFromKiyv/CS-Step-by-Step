# Тестування

Тепер, коли у вас є готовий рівень доступу до даних AutoLot, настав час провести його тест-драйв. Тестування інтеграції є невід’ємною частиною розробки програмного забезпечення та є чудовим способом переконатися, що ваш код доступу до даних поводиться як ви очікували. У цьому розділі ми будемо використовувати xUnit, тестовий фреймворк для .NET Core.

## Створення проекту тестування

Хоча ми зробили проект з декількома тестами вони охоплюють не все. Щоб створювати клієнтську програму для тестування завершеного рівня доступу до даних AutoLot, ми збираємося використовувати автоматизовані тести інтеграції. Тести продемонструють створення, читання, оновлення та видалення викликів бази даних. Це дозволяє нам досліджувати код без накладних витрат на створення іншої програми. Кожен із тестів у цьому розділі виконає запит (створити, прочитати, оновити або видалити), а потім матиме один або кілька операторів Assert для підтвердження того, що результат відповідає очікуванням.

Модульні тести призначені для перевірки окремої одиниці коду. Те, що ми будемо робити протягом цього розділу, — це технічно створювати інтеграційні тести, оскільки ми тестуємо код C# і EF Core на всьому шляху до бази даних і назад.

Для початку ми збираємося налаштувати платформу тестування інтеграції за допомогою xUnit, сумісної з .NET Core системи тестування. Почніть із додавання нового тестового проекту типу xUnit під назвою AutoLot.Dal.Tests. У Visual Studio цей тип проекту називається xUnit Test Project. 

Створимо папку AutoLotTesting і запустимо в ній:

```console
dotnet new sln 
dotnet new xunit -n AutoLot.Dal.Tests -f net8.0
dotnet sln add AutoLot.Dal.Tests
```
Додамо NuGet пакети до проекту AutoLot.Dal.Tests. Для CLI це наступні команди.

```console
dotnet add AutoLot.Dal.Tests package Microsoft.EntityFrameworkCore
dotnet add AutoLot.Dal.Tests package Microsoft.EntityFrameworkCore.Design
dotnet add AutoLot.Dal.Tests package Microsoft.EntityFrameworkCore.SqlServer
dotnet add AutoLot.Dal.Tests package Microsoft.Extensions.Configuration.Json
```
Версії пакетів Microsoft.NET.Test.Sdk і coverlet.collector, які постачаються разом із шаблоном проекту xUnit, зазвичай відстають від поточних доступних версій. Щоб оновити їх, скористайтеся диспетчером пакетів NuGet у Visual Studio, щоб оновити всі пакети NuGet, або використовуйте CLI. Щоб оновити їх за допомогою CLI, потім додайте їх знову, оскільки додавання пакетів із командного рядка завжди отримуватиме останню версію без попереднього випуску. Ось команди:

```console
dotnet add AutoLot.Dal.Tests package Microsoft.NET.Test.Sdk
dotnet add AutoLot.Dal.Tests package coverlet.collector
```
Скопіюємо папку з попередьного розділу AutoLotSolution в папку де ми створили папку AutoLotTesting. Далі додайте посилання на проект до AutoLot.Models і AutoLot.Dal.
Щоб зробити це з командного рядка, виконайте наступне (можете оновіть шлях і роздільник каталогів для ваших проектів і на копіювати рішення):

```console
dotnet sln add ..\AutoLotSolution\AutoLot.Dal
dotnet sln add ..\AutoLotSolution\AutoLot.Models
dotnet add AutoLot.Dal.Tests reference ..\AutoLotSolution\AutoLot.Dal
dotnet add AutoLot.Dal.Tests reference ..\AutoLotSolution\AutoLot.Models
```
Відкриємо рішення в VS.

У тестах використовуватиметься код ініціалізації даних, який очищає часові таблиці, тому таке ж налаштування файлу проекту необхідно внести щодо пакета Microsoft.EntityFrameworkCore.Design. Оновіть пакет, щоб видалити (або закоментувати) тег IncludeAssets:

```xml
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.0">
      <!--<IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>-->
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
```

Для перевірки (або використання) методів і класів у проекті AutoLot.Dal, позначених як внутрішні, внутрішні елементи потрібно зробити видимими для проекту AutoLot.Dal.Tests.
Відкрийте файл AutoLot.Dal.csproj і додайте наступне:

```xml
<ItemGroup>
  <AssemblyAttribute Include="System.Runtime.CompilerServices.InternalsVisibleToAttribute">
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

# Перший погляд на xUnit

Є два типи тестів, які будуть використані в цьому розділі. Методи тестування без параметрів називаються фактами (і використовують атрибут Fact). Тести, які приймають параметри, називаються теоріями (і використовують атрибут Theory). Теоретичні тести виконують кілька ітерацій методу тестування, передаючи різні значення для кожного циклу. Щоб продемонструвати ці типи тестів, створіть новий клас під назвою SampleTests.cs у проекті AutoLot.Dal.Tests.

```cs
namespace AutoLot.Dal.Tests;

public class SampleTests
{

}
```

## Тестовий метод Fact

Перший тест Fact. У тестах фактів усі значення містяться в методі тестування.

```cs
    [Fact]
    public void SimpleTestSum()
    {
        Assert.Equal(5,2+3);
    }
```
Доступні різні типи тверджень. У цьому прикладі тест стверджує, що фактичний результат (3+2) дорівнює очікуваному результату (5).

## Тестовий метод Theory

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


## Виконання тестів

Хоча тести xUnit можна виконувати з командного рядка (за допомогою dotnet test), для розробників краще використовувати Visual Studio для виконання тестів. Запустіть Test Explorer із меню Test, щоб отримати доступ до запуску та налагодження всіх або вибраних тестів. Запустіть тести і переглянте результат.

# Підготовка до тестування AutoLot.Dal

## Налаштування проекту і екземплярів DBContext

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

## Створіть помічник тестування інтеграції

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

## Додайте клас BaseTest

Клас BaseTest керуватиме інфраструктурою для тестів. Додайте нову папку під назвою Base до тестового проекту та додайте новий файл класу під назвою BaseTest.cs до цієї папки. Зробіть клас абстрактним і реалізуйте IDisposable. Додайте дві захищені властивості лише для читання, щоб зберігати екземпляри IConfiguration і ApplicationDbContext, а також метод видалення з пам'яті екземпляра ApplicationDbContext у віртуальному методі Dispose().

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
    protected readonly ITestOutputHelper OutputHelper;

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

## Додайте тестовий клас фікстур EnsureAutoLotDatabase

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

## Додайте інтеграційні тестові класи

Наступним кроком буде додавання класів, які будуть проводити автоматизовані тести. Ці класи називають test fixtures. Додайте нову папку під назвою IntegrationTests у папку AutoLot.Dal.Tests.

Залежно від можливостей виконавця тестів, тести xUnit виконуються послідовно в межах test fixture (класу), але паралельно між test fixture. Це може бути проблематично під час виконання інтеграційних тестів, які взаємодіють із базою даних. Паралельні тести бази даних з використанням одного екземпляра бази даних можуть спричиняти блокування, давати помилкові результати та, як правило, проблематичні. Виконання тесту xUnit можна змінити на послідовне виконання test fixture, додавши їх до однієї колекції тестів. Колекції тестів визначаються за назвою за допомогою атрибута Collection у класі. Наприклад можна додати такий атрібут

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
Налаштування для класу CustomerTests без додадкових змін, оскільки він не використовує CustomerRepo.

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

# Здійснення запитів до бази даних

Згадайте, що створення екземплярів сутності з даних бази даних передбачає виконання оператора LINQ або оператора SQL (за допомогою FromSqlRaw()/FromSqlInterpolated()) до властивостей DbSet<T>. Під час використання LINQ оператори перетворюються на SQL постачальником бази даних і механізмом перекладу LINQ, а відповідні дані зчитуються з бази даних. Дані також можна завантажити за допомогою методу FromSqlRaw() або FromSqlInterpolated() з використанням необроблених рядків SQL і, за бажанням, додаткових операторів LINQ. Сутності, завантажені в колекції DbSet<T>, додаються до ChangeTracker за замовчуванням, але їх можна додавати без відстеження. Дані, завантажені в безключові колекції DbSet<T>, ніколи не відстежуються.
Якщо пов’язані сутності вже завантажено в DbSet<T>, EF Core з’єднає нові екземпляри за властивостями навігації. Наприклад, якщо Cars завантажуються в колекцію DbSet<Car>, а потім пов’язані замовлення завантажуються в DbSet<Order> того самого екземпляра ApplicationDbContext, властивість навігації Car.Orders поверне пов’язані сутності Order без повторного запиту до бази даних. Багато методів, продемонстрованих тут, мають доступні асинхронні версії. Синтаксис запитів LINQ структурно однаковий, тому я продемонструю лише несинхронну версію.

## Запити LINQ

Тип колекції DbSet<T> реалізує (серед інших інтерфейсів) IQueryable<T>. Це дозволяє використовувати команди C# LINQ для створення запитів на отримання даних із бази даних. Хоча всі оператори C# LINQ доступні для використання з типом колекції DbSet<T>, деякі оператори LINQ можуть не підтримуватися постачальником бази даних, а додаткові оператори LINQ додаються EF Core. Непідтримувані оператори LINQ, які не можна перекласти на мову запитів постачальника бази даних, створять виняткову ситуацію під час виконання. Деякі оператори LINQ, які не підлягають перекладу, виконуватимуться на стороні клієнта, якщо вони є останніми операторами в ланцюжку LINQ; однак інші (наприклад, оновлення методу Take(), який працює з діапазонами) все одно видаватиме помилку, якщо запит спочатку не буде виконано за допомогою ToList() або подібної конструкції.

Там, де доступний ToQueryString(), тести в наступному розділі виводять результати тесту за допомогою ITestOutputHelper, щоб ви могли перевірити запит під час виконання тестів.

## Отримання всіх записів

Щоб отримати всі записи для таблиці, просто використовуйте властивість DbSet<T> безпосередньо без будь-яких операторів LINQ. Додайте такий факт до класу CustomerTests.cs:

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

## Тест з фільтром записів.

Наступний тест у класі CustomerTests показує запити для клієнтів, де прізвище починається з W (незалежно від регістру):

```cs
    [Fact]
    public void ShouldGetCustomersWithLastNameStartWithW()
    {
        IQueryable<Customer> query = Context.Customers
            .Where(c => c.PersonInformation.LastName.StartsWith("W"));
        OutputHelper.WriteLine(query.ToQueryString());
        List<Customer> customers = query.ToList();

        Assert.Equal(2, customers.Count);

        foreach (var customer in customers)
        {
            Person? person = customer.PersonInformation;
            OutputHelper.WriteLine(customer.Id+"\t"+person.LastName);
            Assert.StartsWith("W", person.LastName, StringComparison.OrdinalIgnoreCase);
        }
    }
```
```sql
SELECT [c].[Id], [c].[TimeStamp], [c].[FirstName], [c].[FullName], [c].[LastName]
FROM [Customers] AS [c]
WHERE [c].[LastName] LIKE N'W%'
2	Walton
4	Walton
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
```sql
SELECT [c].[Id], [c].[TimeStamp], [c].[FirstName], [c].[FullName], [c].[LastName]
FROM [Customers] AS [c]
WHERE [c].[LastName] LIKE N'W%' AND [c].[FirstName] LIKE N'M%'
2	Walton	Matt
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
Наступний тест у класі CustomerTests демонструє запити для клієнтів, де прізвище починається з W або прізвище починається з H (незалежно від регістру):

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
            OutputHelper.WriteLine(customer.Id + "\t" + person.LastName+"\t"+person.FirstName);
            Assert.True(
                    person.LastName.StartsWith("W",StringComparison.OrdinalIgnoreCase) 
                 || person.LastName.StartsWith("H",StringComparison.OrdinalIgnoreCase)
                );
        }
    }
```
```sql
SELECT [c].[Id], [c].[TimeStamp], [c].[FirstName], [c].[FullName], [c].[LastName]
FROM [Customers] AS [c]
WHERE [c].[LastName] LIKE N'W%' OR [c].[LastName] LIKE N'H%'
2	Walton	Matt
3	Hagen	Steve
4	Walton	Pat
```
Наступне в класі CustomerTests також запитує клієнтів, у яких прізвище починається з літери W або прізвище починається з літери H (незалежно від регістру). Цей тест демонструє використання методу EF.Functions.Like(). Зауважте, що ви повинні самостійно вказати символ підстановки (%).

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

## Тест з сортування записів

Згадайте, що сортування досягається за допомогою OrderBy()/OrderByDescending(). Якщо потрібно більше ніж один рівень виконання, додайте ThenBy()/ThenByDescending() для кожної наступної властивості. Сортування за зростанням і спаданням для різних властивостей можна використовувати разом, як показано в наступному тесті у файлі CustomerTests.cs:

```cs
[Fact]
public void ShouldSortByLastNameThenByDescendingFirstName()
{
    var query = Context.Customers
        .OrderBy(c => c.PersonInformation.LastName)
        .ThenByDescending(c => c.PersonInformation.FirstName);
    OutputHelper.WriteLine(query.ToQueryString()+"\n");

    var customers = query.ToList();

    foreach (var customer in customers)
    {
            OutputHelper.WriteLine(
                $"{customer.PersonInformation.LastName} " +
                $"{customer.PersonInformation.FirstName}");
    }

    for (int i = 0; i < customers.Count-1; i++)
    {
        Compare(customers[i].PersonInformation, customers[i+1].PersonInformation);
    }
    
    static void Compare(Person person1, Person person2)
    {
        var compareResult = string.Compare(person1.LastName, person2.LastName,
            StringComparison.CurrentCultureIgnoreCase);
        Assert.True(compareResult <= 0);
        if (compareResult == 0)
        {
            Assert.True(string.Compare(person1.FirstName, person2.FirstName,
                StringComparison.CurrentCultureIgnoreCase) >= 0);
        }
    }
}
```

Метод Reverse() змінює весь порядок сортування, як показано в наступному тесті

```cs
[Fact]
public void ShouldSortByFirstNameThenLastNameUsingReverse()
{
    var query = Context.Customers
.OrderBy(c => c.PersonInformation.LastName)
.ThenByDescending(c => c.PersonInformation.FirstName)
.Reverse();
    OutputHelper.WriteLine(query.ToQueryString() + "\n");

    var customers = query.ToList();

    foreach (var customer in customers)
    {
        OutputHelper.WriteLine(
            $"{customer.PersonInformation.LastName} " +
            $"{customer.PersonInformation.FirstName}");
    }

    //if only one customer, nothing to test
    if (customers.Count <= 1) { return; }
    for (int x = 0; x < customers.Count - 1; x++)
    {
        var pi1 = customers[x].PersonInformation;
        var pi2 = customers[x + 1].PersonInformation;
        var compareLastName = string.Compare(pi1.LastName,
        pi2.LastName, StringComparison.CurrentCultureIgnoreCase);
        Assert.True(compareLastName >= 0);
        if (compareLastName != 0) continue;
        var compareFirstName = string.Compare(pi1.FirstName,
        pi2.FirstName, StringComparison.CurrentCultureIgnoreCase);
        Assert.True(compareFirstName <= 0);
    }
}
```

## Запити з одним записом

Через негайне виконання інструкцій LINQ з одним записом метод ToQueryString() недоступний. Ви можете переглянути запит за допомогою SQL Server Profiler. Усі тести з одним записом містяться у файлі CustomerTests.cs.

## Використання First

У разі використання форми First() і FirstOrDefault() без параметрів буде повернено перший запис (на основі порядку бази даних або будь-яких попередніх положень порядку). Наступний тест показує запит першого запису на основі порядку бази даних.

```cs
    [Fact]
    public void GetFirstMatchingRecordDatabaseOrder()
    {
        var customer = Context.Customers.First();
        OutputHelper.WriteLine($"{customer.Id}");
        Assert.Equal(1, customer.Id);
    }
```

Наступний тест демонструє отримання першого запису на основі порядку «last name, first name»:

```cs
    [Fact]
    public void GetFirstMatchingRecordNameOrder()
    {
        var customer = Context.Customers
            .OrderBy(c=>c.PersonInformation.LastName)
            .ThenBy(c=>c.PersonInformation.FirstName)
            .First();
        OutputHelper.WriteLine($"{customer.Id}");
        Assert.Equal(1, customer.Id);
    }
```

Наступний тест підтверджує, що виникає виняток, якщо немає збігу під час використання First():

```cs
    [Fact]
    public void FirstShouldThrowExceptionIfNoneMatch()
    {
        //Filters based on Id. Throws due to no match
        Assert.Throws<InvalidOperationException>(() => Context.Customers.First(c => c.Id == 10)); 
    }
```
Assert.Throws() — це особливий тип оператора assert. Він очікує виняток із викинутого коду у виразі. Якщо виняток не генерується, твердження не виконується.

Під час використання FirstOrDefault() замість винятку результатом є null , якщо дані не повертаються. Цей тест показує створення змінної виразу

```cs
    [Fact]
    public void FirstOrDefaultShouldReturnDefaultIfNoneMatch()
    {
        Expression<Func<Customer, bool>> expression = c => c.Id == 10;
        var customer = Context.Customers.FirstOrDefault(expression);
        Assert.Null(customer);
    }
```

## Використання Last

У разі використання форми Last() і LastOrDefault() без параметрів буде повернуто останній запис (на основі будь-яких попередніх положень порядку). Нагадуємо, що EF Core створить виняток, якщо сортування не вказано. Наступний тест отримує останній запис на основі порядку «прізвище, ім’я»:

```cs
    [Fact]
    public void GetLastMatchingRecordNameOrder()
    {
        var customer = Context.Customers
          .OrderBy(c => c.PersonInformation.LastName)
          .ThenBy(c => c.PersonInformation.FirstName)
          .Last();
        Assert.Equal(4, customer.Id);
    }
```
Наступний тест підтверджує, що EF Core створює виняток, коли Last() використовується без OrderBy()/OrderByDescending():

```cs
    [Fact]
    public void LastShouldThrowIfNoSortSpecified()
    {
        Assert.Throws<InvalidOperationException>(() => Context.Customers.Last());
    }
```

## Використання Single

Концептуально Single()/SingleOrDefault() працює так само, як First()/FirstOrDefault(). Основна відмінність полягає в тому, що Single()/SingleOrDefault() повертає Top(2) замість Top(1) і створює виняток, якщо з бази даних повертаються два записи. Наступний тест отримує єдиний запис, де Id == 1:

```cs
    public void GetOneMatchingRecordWithSingle()
    {
        var customer = Context.Customers.Single(x => x.Id == 1);
        Assert.Equal(1, customer.Id);
    }
```
Single() генерує виняток, якщо не повернуто жодного запису.

```cs
[Fact]
public void SingleShouldThrowExceptionIfNoneMatch()
{
  //Filters based on Id. Throws due to no match
  Assert.Throws<InvalidOperationException>(() => Context.Customers.Single(x => x.Id == 10));
}
```

Якщо використовується Single() або SingleOrDefault() і повертається більше одного запису, виникає виняток.
```cs
[Fact]
public void SingleShouldThrowExceptionIfMoreThenOneMatch()
{
  // Throws due to more than one match
  Assert.Throws<InvalidOperationException>(() => Context.Customers.Single());
}

[Fact]
public void SingleOrDefaultShouldThrowExceptionIfMoreThenOneMatch()
{
  // Throws due to more than one match
  Assert.Throws<InvalidOperationException>(() => Context.Customers.SingleOrDefault());
}
```

Під час використання SingleOrDefault() замість винятку результатом є null, якщо дані не повертаються.

```cs
    [Fact]
    public void SingleOrDefaultShouldReturnDefaultIfNoneMatch()
    {
        //Expression<Func<Customer>> is a lambda expression
        Expression<Func<Customer, bool>> expression = x => x.Id == 10;
        //Returns null when nothing is found
        var customer = Context.Customers.SingleOrDefault(expression);
        Assert.Null(customer);
    }
```

## Глобальні фільтри запитів

Згадайте, що існує глобальний фільтр запитів для сутності Car, щоб відфільтрувати будь-які автомобілі, для яких IsDrivable має значення false. Відкрийте клас CarTests.cs і додайте такий тест, який отримує всі записи, які проходять фільтр запиту:

```cs
 [Fact]
 public void ShouldReturnDrivableCarsWithQueryFilterSet()
 {
     IQueryable<Car> query = Context.Cars;
     OutputHelper.WriteLine(query.ToQueryString());
     
     var cars = query.ToList();
     Assert.NotEmpty(cars);
     Assert.Equal(9, cars.Count());
 }
```
Згадайте, що ми створюємо 10 автомобілів у процесі ініціалізації даних, і один із них налаштований як непридатний для керування. Під час виконання запиту застосовується фільтр глобального запиту та виконується такий SQL:
```sql
SELECT [i].[Id], [i].[Color], [i].[DateBuilt], [i].[Display], [i].[IsDrivable], [i].[MakeId], [i].[PeriodEnd], [i].[PeriodStart], [i].[PetName], [i].[Price], [i].[TimeStamp]
FROM [Inventory] AS [i]
WHERE [i].[IsDrivable] = CAST(1 AS bit)
```

## Вимкання фільтра запитів

Щоб вимкнути глобальні фільтри запиту для сутностей у запиті, додайте метод IgnoreQueryFilters() до запиту LINQ. Якщо є кілька сутностей із глобальним фільтром запитів і потрібні деякі фільтри сутностей, їх потрібно додати до методів Where() інструкції LINQ. Додайте такий тест до класу CarTests.cs, який вимикає фільтр запитів і повертає всі записи:

```cs
    [Fact]
    public void ShouldGetAllOfTheCars()
    {
        IQueryable<Car> query = Context.Cars.IgnoreQueryFilters();
        OutputHelper.WriteLine(query.ToQueryString());
        var cars = query.ToList();
        Assert.Equal(10, cars.Count());

        foreach (var car in cars)
        {
            OutputHelper.WriteLine(car.ToString()+$"\t{car.IsDrivable}");
        }
    }
```
```sql
SELECT [i].[Id], [i].[Color], [i].[DateBuilt], [i].[Display], [i].[IsDrivable], [i].[MakeId], [i].[PeriodEnd], [i].[PeriodStart], [i].[PetName], [i].[Price], [i].[TimeStamp]
FROM [Inventory] AS [i]
1	Zippy	Black	 True
2	Rusty	Rust	 True
3	Mel	Black	 True
4	Clunker	Yellow	 True
5	Bimmer	Black	 True
6	Hank	Green	 True
7	Pinky	Pink	 True
8	Pete	Black	 True
9	Brownie	Brown	 True
10	Lemon	Rust	 False
```

## Фільтри запитів у властивостях навігації

На додаток до глобального фільтра запиту для сутності Car ми додали фільтр запиту до властивості CarNavigation сутності Order. Щоб побачити це в дії, додайте такий тест до класу OrderTests.cs:

```cs
    [Fact]
    public void ShouldGetAllOrdersExceptFiltered()
    {
        var query = Context.Orders;
        OutputHelper.WriteLine(query.ToQueryString());

        var orders = query.ToList();
        Assert.NotEmpty(orders);
        Assert.Equal(4, orders.Count);
    }
```
Оскільки навігаційна властивість CarNavigation є обов’язковою навігаційною властивістю, система перекладу запитів використовує INNER JOIN, усуваючи записи Order, у яких Car має значення для IsDrivable false .
```sql
SELECT [o].[Id], [o].[CarId], [o].[CustomerId], [o].[PeriodEnd], [o].[PeriodStart], [o].[TimeStamp]
FROM [Orders] AS [o]
INNER JOIN (
    SELECT [i].[Id], [i].[IsDrivable]
    FROM [Inventory] AS [i]
    WHERE [i].[IsDrivable] = CAST(1 AS bit)
) AS [i0] ON [o].[CarId] = [i0].[Id]
WHERE [i0].[IsDrivable] = CAST(1 AS bit)
```
Щоб повернути всі записи, додайте IgnoreQueryFilters() до свого запиту LINQ.

```cs
    [Fact]
    public void ShouldGetAllOrders()
    {
        var query = Context.Orders.IgnoreQueryFilters();
        OutputHelper.WriteLine(query.ToQueryString());

        var orders = query.ToList();
        Assert.NotEmpty(orders);
        Assert.Equal(5, orders.Count);
    }
```
```sql
SELECT [o].[Id], [o].[CarId], [o].[CustomerId], [o].[PeriodEnd], [o].[PeriodStart], [o].[TimeStamp]
FROM [Orders] AS [o]
```

## Швидке ​​завантаження пов’язаних даних

Сутності, пов’язані за допомогою навігаційних властивостей, можна створити в одному запиті за допомогою швидкого завантаження. Метод Include() вказує на приєднання до пов’язаної сутності, а метод ThenInclude() використовується для наступних з’єднань з іншими сутностями. Обидва ці методи будуть продемонстровані в цих тестах. Коли методи Include()/ThenInclude() транслюються в SQL, обов’язкові зв’язки використовують внутрішнє з’єднання, а необов’язкові – ліве з’єднання.
Додайте наступний тест до класу CarTests.cs, щоб показати один Include()

```cs
    [Fact]
    public void ShouldGetAllOfTheCarsWithMakes()
    {
        IIncludableQueryable<Car, Make> query = Context.Cars
            .Include(c => c.MakeNavigation);
        OutputHelper.WriteLine(query.ToQueryString());
        var cars = query.ToList();
        Assert.Equal(9, cars.Count());
        foreach (var car in cars)
        {
            OutputHelper.WriteLine(car.ToString());
        }
    }
```
Запит додає властивість MakeNavigation до результатів, виконуючи внутрішнє об’єднання з наступним виконанням SQL. Запит повертає всі стовпці з обох таблиць, а потім EF Core створив екземпляри Car і Make із отриманих даних. Зверніть увагу, що глобальний фільтр запитів діє.

```sql
    SELECT [i].[Id], [i].[Color], [i].[DateBuilt], [i].[Display], [i].[IsDrivable], [i].[MakeId], [i].[PeriodEnd], [i].[PeriodStart], [i].[PetName], [i].[Price], [i].[TimeStamp], [m].[Id], [m].[Name], [m].[PeriodEnd], [m].[PeriodStart], [m].[TimeStamp]
FROM [Inventory] AS [i]
INNER JOIN [Makes] AS [m] ON [i].[MakeId] = [m].[Id]
WHERE [i].[IsDrivable] = CAST(1 AS bit)
1	Zippy	Black	VW
2	Rusty	Rust	Ford
3	Mel	Black	Saab
4	Clunker	Yellow	Yugo
5	Bimmer	Black	BMW
6	Hank	Green	BMW
7	Pinky	Pink	BMW
8	Pete	Black	Pinto
9	Brownie	Brown	Yugo
```

Наступний тест демонструє використання двох наборів пов’язаних даних. Перший – отримання інформації про Make (так само, як і в попередньому тесті), а другий – отримання замовлень, а потім клієнтів, долучених до замовлень. Весь тест також відфільтровує записи автомобілів, які не мають жодних замовлень.

```cs
[Fact]
public void ShouldGetCarsOnOrderWithCustomer()
{
    IIncludableQueryable<Car, Customer?> query = Context.Cars
        .Where(c => c.Orders.Any())
        .Include(c => c.MakeNavigation)
        .Include(c => c.Orders)
        .ThenInclude(o => o.CustomerNavigation);
    OutputHelper.WriteLine(query.ToQueryString());
    var cars = query.ToList();
    foreach (var car in cars)
    {
        OutputHelper.WriteLine($"{car.Id} {car.PetName} {car.MakeName}");
        foreach (var order in car.Orders)
        {
            OutputHelper.WriteLine(
                $"\t\t{order.Id} " +
                $"{order.CustomerNavigation.PersonInformation.LastName}");
        }
    }

    Assert.Equal(4, query.Count());
    cars.ForEach(c => 
    {
        Assert.NotNull(c.MakeNavigation);
        Assert.NotNull(c.Orders.ToList()[0].CustomerNavigation);
    });
}
```
Сформований запит досить об’ємний.
```console
SELECT [i].[Id], [i].[Color], [i].[DateBuilt], [i].[Display], [i].[IsDrivable], [i].[MakeId], [i].[PeriodEnd], [i].[PeriodStart], [i].[PetName], [i].[Price], [i].[TimeStamp], [m].[Id], [m].[Name], [m].[PeriodEnd], [m].[PeriodStart], [m].[TimeStamp], [s].[Id], [s].[CarId], [s].[CustomerId], [s].[PeriodEnd], [s].[PeriodStart], [s].[TimeStamp], [s].[Id0], [s].[TimeStamp0], [s].[FirstName], [s].[FullName], [s].[LastName], [s].[Id1]
FROM [Inventory] AS [i]
INNER JOIN [Makes] AS [m] ON [i].[MakeId] = [m].[Id]
LEFT JOIN (
    SELECT [o0].[Id], [o0].[CarId], [o0].[CustomerId], [o0].[PeriodEnd], [o0].[PeriodStart], [o0].[TimeStamp], [c].[Id] AS [Id0], [c].[TimeStamp] AS [TimeStamp0], [c].[FirstName], [c].[FullName], [c].[LastName], [i3].[Id] AS [Id1]
    FROM [Orders] AS [o0]
    INNER JOIN (
        SELECT [i2].[Id], [i2].[IsDrivable]
        FROM [Inventory] AS [i2]
        WHERE [i2].[IsDrivable] = CAST(1 AS bit)
    ) AS [i3] ON [o0].[CarId] = [i3].[Id]
    INNER JOIN [Customers] AS [c] ON [o0].[CustomerId] = [c].[Id]
    WHERE [i3].[IsDrivable] = CAST(1 AS bit)
) AS [s] ON [i].[Id] = [s].[CarId]
WHERE [i].[IsDrivable] = CAST(1 AS bit) AND EXISTS (
    SELECT 1
    FROM [Orders] AS [o]
    INNER JOIN (
        SELECT [i0].[Id], [i0].[IsDrivable]
        FROM [Inventory] AS [i0]
        WHERE [i0].[IsDrivable] = CAST(1 AS bit)
    ) AS [i1] ON [o].[CarId] = [i1].[Id]
    WHERE [i1].[IsDrivable] = CAST(1 AS bit) AND [i].[Id] = [o].[CarId])
ORDER BY [i].[Id], [m].[Id], [s].[Id], [s].[Id1]
1 Zippy VW
		2 Walton
4 Clunker Yugo
		3 Hagen
5 Bimmer BMW
		1 Brenner
7 Pinky BMW
		4 Walton
```
Якщо ви запустите той самий запит без фільтрів запиту, запит стане набагато простішим.

```cs

    [Fact]
    public void ShouldGetCarsOnOrderWithCustomerIgnoreQueryFilters()
    {
        IIncludableQueryable<Car, Customer?> query = Context.Cars
            .IgnoreQueryFilters()
            .Where(c => c.Orders.Any())
            .Include(c => c.MakeNavigation)
            .Include(c => c.Orders)
            .ThenInclude(o => o.CustomerNavigation);

        //...

        Assert.Equal(5, query.Count());

        //...
    }
```
```console
SELECT [i].[Id], [i].[Color], [i].[DateBuilt], [i].[Display], [i].[IsDrivable], [i].[MakeId], [i].[PeriodEnd], [i].[PeriodStart], [i].[PetName], [i].[Price], [i].[TimeStamp], [m].[Id], [m].[Name], [m].[PeriodEnd], [m].[PeriodStart], [m].[TimeStamp], [s].[Id], [s].[CarId], [s].[CustomerId], [s].[PeriodEnd], [s].[PeriodStart], [s].[TimeStamp], [s].[Id0], [s].[TimeStamp0], [s].[FirstName], [s].[FullName], [s].[LastName]
FROM [Inventory] AS [i]
INNER JOIN [Makes] AS [m] ON [i].[MakeId] = [m].[Id]
LEFT JOIN (
    SELECT [o0].[Id], [o0].[CarId], [o0].[CustomerId], [o0].[PeriodEnd], [o0].[PeriodStart], [o0].[TimeStamp], [c].[Id] AS [Id0], [c].[TimeStamp] AS [TimeStamp0], [c].[FirstName], [c].[FullName], [c].[LastName]
    FROM [Orders] AS [o0]
    INNER JOIN [Customers] AS [c] ON [o0].[CustomerId] = [c].[Id]
) AS [s] ON [i].[Id] = [s].[CarId]
WHERE EXISTS (
    SELECT 1
    FROM [Orders] AS [o]
    WHERE [i].[Id] = [o].[CarId])
ORDER BY [i].[Id], [m].[Id], [s].[Id]
1 Zippy VW True
		2 Walton
4 Clunker Yugo True
		3 Hagen
5 Bimmer BMW True
		1 Brenner
7 Pinky BMW True
		4 Walton
10 Lemon VW False
		5 Customer
```

## Розбиття запитів на пов’язані дані

Чим більше об’єднань додається до запиту LINQ, тим складнішим стає кінцевий запит. Як показано в попередніх прикладах, фільтри запитів можуть зробити запити ще складнішими. Існує можливість запускати складні об’єднання як розділені запити, додавши метод AsSplitQuery() до запиту LINQ. Як обговорювалося в попередніх розділах, це може підвищити ефективність через ризик неузгодженості даних. Наступний тест демонструє той самий щойно виконаний запит, але як розділений запит:

```cs
    [Fact]
    public void ShouldGetCarsOnOrderWithCustomerAsSplitQuery()
    {
        IQueryable<Car> query = Context.Cars
            .Where(c => c.Orders.Any())
            .Include(c => c.MakeNavigation)
            .Include(c => c.Orders)
            .ThenInclude(o => o.CustomerNavigation)
            .AsSplitQuery();

        // ...

        Assert.Equal(4, query.Count());

        // ...
    }
```
```console
SELECT [i].[Id], [i].[Color], [i].[DateBuilt], [i].[Display], [i].[IsDrivable], [i].[MakeId], [i].[PeriodEnd], [i].[PeriodStart], [i].[PetName], [i].[Price], [i].[TimeStamp], [m].[Id], [m].[Name], [m].[PeriodEnd], [m].[PeriodStart], [m].[TimeStamp]
FROM [Inventory] AS [i]
INNER JOIN [Makes] AS [m] ON [i].[MakeId] = [m].[Id]
WHERE [i].[IsDrivable] = CAST(1 AS bit) AND EXISTS (
    SELECT 1
    FROM [Orders] AS [o]
    INNER JOIN (
        SELECT [i0].[Id], [i0].[IsDrivable]
        FROM [Inventory] AS [i0]
        WHERE [i0].[IsDrivable] = CAST(1 AS bit)
    ) AS [i1] ON [o].[CarId] = [i1].[Id]
    WHERE [i1].[IsDrivable] = CAST(1 AS bit) AND [i].[Id] = [o].[CarId])
ORDER BY [i].[Id], [m].[Id]

This LINQ query is being executed in split-query mode, and the SQL shown is for the first query to be executed. Additional queries may also be executed depending on the results of the first query.
1 Zippy VW
		2 Walton
4 Clunker Yugo
		3 Hagen
5 Bimmer BMW
		1 Brenner
7 Pinky BMW
		4 Walton
```
Знову ж таки, видалення фільтрів запитів значно спрощує створення запитів.

```cs

    [Fact]
    public void ShouldGetCarsOnOrderWithCustomerAsSplitQueryIgnoreFilters()
    {
        IQueryable<Car> query = Context.Cars
            .IgnoreQueryFilters()
            .Where(c => c.Orders.Any())
            .Include(c => c.MakeNavigation)
            .Include(c => c.Orders)
            .ThenInclude(o => o.CustomerNavigation)
            .AsSplitQuery();
        //...
        Assert.Equal(5, query.Count());
        //...

    }
```

## Фільтрування пов’язаних даних

Існує можливість фільтрації при включенні властивостей колекції. Додайте наступний тест до класу MakeTests.cs, який демонструє отримання всіх записів Make і жовтих автомобілів:


```cs

    [Fact]
    public void ShouldGetAllMakesAndCarsThatAreYellow()
    {
        IQueryable<Make> query = Context.Makes
            .IgnoreQueryFilters()
            .Include(m => m.Cars.Where(c => c.Color == "Yellow"));
        OutputHelper.WriteLine(query.ToQueryString());

        List<Make> makes = query.ToList();

        foreach (var make in makes)
        {
            OutputHelper.WriteLine($"{make.Id} {make.Name}");
            foreach (var car in make.Cars)
            {
                OutputHelper.WriteLine($"\t{car.Id} {car.MakeName} {car.Color} {car.PetName}");
            }
        }
        Assert.NotNull(makes);
        Assert.NotEmpty(makes);
        Assert.Contains(makes, m => m.Cars.Any());
        Assert.Empty(makes.First(m => m.Id == 1).Cars);
        Assert.Empty(makes.First(m => m.Id == 2).Cars);
        Assert.Empty(makes.First(m => m.Id == 3).Cars);
        Assert.Empty(makes.First(m => m.Id == 5).Cars);
        Assert.NotEmpty(makes.First(m => m.Id == 4).Cars);
    }
```
```console
    SELECT [m].[Id], [m].[Name], [m].[PeriodEnd], [m].[PeriodStart], [m].[TimeStamp], [i0].[Id], [i0].[Color], [i0].[DateBuilt], [i0].[Display], [i0].[IsDrivable], [i0].[MakeId], [i0].[PeriodEnd], [i0].[PeriodStart], [i0].[PetName], [i0].[Price], [i0].[TimeStamp]
FROM [Makes] AS [m]
LEFT JOIN (
    SELECT [i].[Id], [i].[Color], [i].[DateBuilt], [i].[Display], [i].[IsDrivable], [i].[MakeId], [i].[PeriodEnd], [i].[PeriodStart], [i].[PetName], [i].[Price], [i].[TimeStamp]
    FROM [Inventory] AS [i]
    WHERE [i].[Color] = N'Yellow'
) AS [i0] ON [m].[Id] = [i0].[MakeId]
ORDER BY [m].[Id]
1 VW
2 Ford
3 Saab
4 Yugo
	4 Yugo Yellow Clunker
5 BMW
6 Pinto
```

## Явне завантаження пов’язаних даних

Якщо пов’язані дані потрібно завантажити після того, як основний об’єкт був запитаний до пам’яті, пов’язані об’єкти можна отримати з бази даних з наступними викликами бази даних. Це запускається за допомогою методу Entry() у похідному DbContext. Під час завантаження сутностей на багатьох кінцях зв’язку «one-to-many» використовуйте метод Collection() для результату Entry. Щоб завантажити сутності на одному кінці зв’язку «one-to-many» (або у зв’язку «one-to-one »), використовуйте метод Reference(). Виклик Query() у методі Collection() або Reference() повертає IQueryable<T>, який можна використовувати для отримання рядка запиту (як показано в наступних тестах) і для керування фільтрами запитів (як показано в наступному розділі). Щоб виконати запит і завантажити запис(и), викличте метод Load() у методі Collection(), Reference() або Query().
Виконання запиту відбувається негайно після виклику Load().
Наступний тест в класі CarTests.cs показує, як завантажити навігаційну властивість навігації в сутність Car

```cs

    [Fact]
    public void ShouldGetReferenceRelatedInformationExplicitly()
    {
        Car? car = Context.Cars.First(c => c.Id == 1);
        Assert.Null(car.MakeNavigation);
        var query = Context.Entry(car).Reference(c => c.MakeNavigation).Query();
        OutputHelper.WriteLine(query.ToQueryString());
        query.Load();
        OutputHelper.WriteLine(car?.MakeNavigation?.Name);
        Assert.NotNull(car?.MakeNavigation);
    }
```
Згенерований SQL для отримання інформації Make виглядає наступним чином (запис Car уже завантажено)
```sql
DECLARE @__p_0 int = 1;

SELECT [m].[Id], [m].[Name], [m].[PeriodEnd], [m].[PeriodStart], [m].[TimeStamp]
FROM [Makes] AS [m]
WHERE [m].[Id] = @__p_0
VW
```
Наступний тест показує, як завантажити властивість навігації колекції в сутність Car

```cs
    [Fact]
    public void ShouldGetCollectionRelatedInformationExplicitly()
    {
        Car? car = Context.Cars.First(c => c.Id == 1);
        Assert.Empty(car.Orders);
        var query = Context.Entry(car).Collection(c => c.Orders).Query();
        OutputHelper.WriteLine(query.ToQueryString());
        query.Load();
        Assert.Single(car.Orders);
    }
```

```sql
DECLARE @__p_0 int = 1;

SELECT [o].[Id], [o].[CarId], [o].[CustomerId], [o].[PeriodEnd], [o].[PeriodStart], [o].[TimeStamp]
FROM [Orders] AS [o]
INNER JOIN (
    SELECT [i].[Id], [i].[IsDrivable]
    FROM [Inventory] AS [i]
    WHERE [i].[IsDrivable] = CAST(1 AS bit)
) AS [i0] ON [o].[CarId] = [i0].[Id]
WHERE [i0].[IsDrivable] = CAST(1 AS bit) AND [o].[CarId] = @__p_0
```

## Завантаження пов’язаних даних та фільтр запитів

Окрім формування запитів, створених під час активного завантаження пов’язаних даних, глобальні фільтри запитів активні під час явного завантаження пов’язаних даних. Виконайте такий тест у класі MakeTests.cs:

```cs
[Theory]
[InlineData(1, 1)]
[InlineData(2, 1)]
[InlineData(3, 1)]
[InlineData(4, 2)]
[InlineData(5, 3)]
[InlineData(6, 1)]
public void ShouldGetAllCarsForAMakeExplicitlyWithQueryFilters(int makeId, int carCount)
{
    Make? make = Context.Makes.Single(m => m.Id == makeId);
    IQueryable <Car> query = Context.Entry(make).Collection(m => m.Cars).Query();
    OutputHelper.WriteLine(query.ToQueryString());
    query.Load();
    Assert.Equal(carCount, make.Cars.Count);
}
```
Спочатку завантажується Маке а потім виконується наступний запит.
```sql
DECLARE @__p_0 int = 1;

SELECT [i].[Id], [i].[Color], [i].[DateBuilt], [i].[Display], [i].[IsDrivable], [i].[MakeId], [i].[PeriodEnd], [i].[PeriodStart], [i].[PetName], [i].[Price], [i].[TimeStamp]
FROM [Inventory] AS [i]
WHERE [i].[IsDrivable] = CAST(1 AS bit) AND [i].[MakeId] = @__p_0
```

Зауважте, що фільтр запиту все ще використовується, незважаючи на те, що основною сутністю в запиті є запис Make. Щоб вимкнути фільтри запитів під час явного завантаження записів, викличте IgnoreQueryFilters() у поєднанні з методом Query().

```cs
    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 1)]
    [InlineData(3, 1)]
    [InlineData(4, 2)]
    [InlineData(5, 3)]
    [InlineData(6, 1)]
    public void ShouldGetAllCarsForAMakeExplicitlyWithoutQueryFilters(int makeId, int carCount)
    {
        Make? make = Context.Makes.Single(m => m.Id == makeId);
        IQueryable<Car> query = Context.Entry(make).Collection(m => m.Cars)
            .Query().IgnoreQueryFilters();
        OutputHelper.WriteLine(query.ToQueryString());
        query.Load();
        Assert.Equal(carCount, make.Cars.Count);
    }
```

## Запити до часових таблиць

У цьому розділі демонструється здатність EF Core отримувати хронологічні дані з часових таблиць. Репозиторії, які походять від TemporalTableBaseRepo, містять методи для запиту до часових таблиць за допомогою одного з п’яти операторів тимчасового запиту. Щоб продемонструвати це, відкрийте клас MakeTests і додайте наступний тест:

```cs
    [Fact]
    public void ShouldGetAllHistoryRows()
    {
        Make make = new Make { Name = "Make" };
        _repo.Add(make);
        Thread.Sleep(1000);
        make.Name = "NewMake";
        _repo.Update(make);
        Thread.Sleep(1000);
        _repo.Delete(make);

        var list = _repo.GetAllHistory().Where(m => m.Entity.Id == make.Id).ToList();
        Assert.Equal(2, list.Count);
        Assert.Equal("Make", list[0].Entity.Name);
        Assert.Equal("NewMake", list[1].Entity.Name);
        Assert.Equal(list[0].PeriodEnd, list[1].PeriodStart);
    }
```
Тест створює новий запис Make і додає його до бази даних. Після призупинення операції на секунду ім’я оновлюється, а зміни зберігаються. Після ще однієї паузи запис видаляється. Потім тест використовує MakeRepo, щоб отримати всю історію для запису Make. Підтверджує наявність двох записів в історії. Переконується, що PeriodEnd першого запису точно відповідає PeriodStart другого запису.

## Запити SQL з LINQ

У цьому розділі демонструється здатність EF Core отримувати дані за допомогою необроблених запитів SQL за допомогою методів FromSqlRaw() або FromSqlInterpolated() DbSet<T>. Перший тест у класі CarTests.cs використовує необроблений SQL-запит, щоб отримати всі записи з таблиці Inventory. Зауважте, що запит має використовувати імена бази даних, а не імена об’єктів, а також додавати стовпці позначки часу для часової функціональності:

```cs
    [Fact]
    public void ShouldNotGetAllCarsUsingFromSql()
    {
        var entity = Context.Model.FindEntityType(typeof(Car).FullName!);
        var tableName = entity!.GetTableName();
        var schemaName = entity!.GetSchema();

        string sql = $"Select *,PeriodStart,PeriodEnd from {schemaName}.{tableName}";
        var query = Context.Cars.FromSqlRaw(sql);
        OutputHelper.WriteLine(query.ToQueryString());

        var cars = query.ToList();
        Assert.Equal(9, cars.Count);

        foreach (var car in cars)
        {
            OutputHelper.WriteLine(car.ToString());
        }
    }

```
Під час використання необроблених запитів SQL EF Core обгортає запит у більший запит для підтримки фільтра запитів. Якщо оператор завершувався крапкою з комою, запит не можна було б виконати на SQL Server.
```sql
    SELECT [a].[Id], [a].[Color], [a].[DateBuilt], [a].[Display], [a].[IsDrivable], [a].[MakeId], [a].[PeriodEnd], [a].[PeriodStart], [a].[PetName], [a].[Price], [a].[TimeStamp]
FROM (
    Select *,PeriodStart,PeriodEnd from .Inventory
) AS [a]
WHERE [a].[IsDrivable] = CAST(1 AS bit)
1	Zippy	Black	
2	Rusty	Rust	
3	Mel	Black	
4	Clunker	Yellow	
5	Bimmer	Black	
6	Hank	Green	
7	Pinky	Pink	
8	Pete	Black	
9	Brownie	Brown

```
Після ігнорування фільтра запиту створений SQL стає тим самим SQL, що й рядок, переданий у метод FromSqlRaw():

```cs
[Fact]
public void ShouldGetAllCarsUsingFromSqlWithoutFilter()
{
    //...
    var query = Context.Cars.FromSqlRaw(sql).IgnoreQueryFilters();
    //...
    Assert.Equal(10, cars.Count);
}
```
```sql
    Select *,PeriodStart,PeriodEnd from .Inventory
```

Наступний тест демонструє використання FromSqlInterpolated() із додатковими операторами LINQ (включаючи MakeNavigation):
```cs
    [Fact]
    public void ShouldGetOneCarUsingInterpolation()
    {
        int carId = 1;
        FormattableString sql = $"Select *,PeriodStart,PeriodEnd from dbo.Inventory where Id = {carId}";
        var query = Context.Cars.FromSqlInterpolated(sql).Include(c => c.MakeNavigation);
        OutputHelper.WriteLine(query.ToQueryString());
        Car? car = query.First();
        Assert.Equal("Black", car.Color);
        Assert.Equal("VW", car.MakeNavigation.Name);
    }
```
```sql
DECLARE p0 int = 1;

SELECT [a].[Id], [a].[Color], [a].[DateBuilt], [a].[Display], [a].[IsDrivable], [a].[MakeId], [a].[PeriodEnd], [a].[PeriodStart], [a].[PetName], [a].[Price], [a].[TimeStamp], [m].[Id], [m].[Name], [m].[PeriodEnd], [m].[PeriodStart], [m].[TimeStamp]
FROM (
    Select *,PeriodStart,PeriodEnd from dbo.Inventory where Id = @p0
) AS [a]
INNER JOIN [Makes] AS [m] ON [a].[MakeId] = [m].[Id]
WHERE [a].[IsDrivable] = CAST(1 AS bit)
```

## Агрегатні методи

Наступний набір тестів демонструє агрегатні методи на стороні сервера (Max(), Min(), Count(), Average() тощо). Агрегатні методи можна додати в кінець запиту LINQ за допомогою методів Where(), або вираз фільтра може міститися в самому агрегатному методі (так само, як First() і Single()). Агрегація виконується на стороні сервера, і із запиту повертається єдине значення. Глобальні фільтри запитів також впливають на агрегатні методи, і їх можна вимкнути за допомогою IgnoreQueryFilters().

Цей перший тест (у CarTests.cs) просто підраховує всі записи про автомобілі в базі даних.

```cs
    [Fact]
    public void ShouldGetTheCountOfCars()
    {
        var count = Context.Cars.Count();
        Assert.Equal(9, count);
    }
```
```sql
SELECT COUNT(*)
FROM [dbo].[Inventory] AS [i]
WHERE [i].[IsDrivable] = CAST(1 AS bit)
```
Додаючи IgnoreQueryFilters(), ми ігноруємо глобальний фільтр.

```cs
[Fact]
public void ShouldGetTheCountOfCarsIgnoreQueryFilters()
{
  var count = Context.Cars.IgnoreQueryFilters().Count();
  Assert.Equal(10, count);
}
```
```sql
SELECT COUNT(*) FROM [dbo].[Inventory] AS [i]
```

Наступні тести демонструють метод Count() із умовою where. Перший тест додає вираз безпосередньо в метод Count(), а другий додає метод Count() у кінець оператора LINQ.

```cs
    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(3, 1)]
    [InlineData(4, 2)]
    [InlineData(5, 3)]
    [InlineData(6, 1)]
    public void ShouldGetTheCountOfCarsByMake1(int makeId, int expectedCount)
    {
        var count = Context.Cars.Count(c => c.MakeId == makeId);
        Assert.Equal(expectedCount, count);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(3, 1)]
    [InlineData(4, 2)]
    [InlineData(5, 3)]
    [InlineData(6, 1)]
    public void ShouldGetTheCountOfCarsByMake2(int makeId, int expectedCount)
    {
        var count = Context.Cars.Where(c => c.MakeId == makeId).Count();
        Assert.Equal(expectedCount, count);
    }
```
Обидва тести створюють однакові виклики SQL до сервера, як показано тут (MakeId змінюється з кожним тестом на основі InlineData):
```sql
exec sp_executesql N'SELECT COUNT(*)
FROM [dbo].[Inventory] AS [i]
WHERE ([i].[IsDrivable] = CAST(1 AS bit)) AND ([i].[MakeId] = @__makeId_0)'
,N'@__makeId_0 int',@__makeId_0=6
```

## Any та All

Методи Any() і All() перевіряють набір записів, щоб перевірити, чи відповідають якісь записи критеріям (Any()), чи всі записи відповідають критеріям (All()).  Global query filters affect Any() and All() methods functions as well and can be disabled with IgnoreQueryFilters(). 
Цей перший тест перевіряє, чи є в записах про автомобіль певний MakeId:

```cs
    [Theory]
    [InlineData(1, true)]
    [InlineData(11, false)]
    public void ShouldCheckForAnyCarsWithMake(int makeId, bool expectedResult)
    {
        var result = Context.Cars.Any(c => c.MakeId == makeId);
        Assert.Equal(expectedResult, result);
    }
```
```sql
exec sp_executesql N'SELECT CASE
    WHEN EXISTS (
        SELECT 1
        FROM [dbo].[Inventory] AS [i]
        WHERE ([i].[IsDrivable] = CAST(1 AS bit)) AND ([i].[MakeId] = @__makeId_0)) THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END',N'@__makeId_0 int',@__makeId_0=1
```

Другий тест перевіряє, чи всі записи автомобіля мають певний MakeId

```cs
[Theory]
[InlineData(1, false)]
[InlineData(11, false)]
public void ShouldCheckForAllCarsWithMake(int makeId, bool expectedResult)
{
  var result = Context.Cars.All(x => x.MakeId == makeId);
  Assert.Equal(expectedResult, result);
}
```
```sql
exec sp_executesql N'SELECT CASE
    WHEN NOT EXISTS (
        SELECT 1
        FROM [dbo].[Inventory] AS [i]
        WHERE ([i].[IsDrivable] = CAST(1 AS bit)) AND ([i].[MakeId] <> @__makeId_0)) THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END',N'@__makeId_0 int',@__makeId_0=1
```

## Отримання даних із збережених процедур

Тест полягає в тому, щоб переконатися, що CarRepo може отримати PetName із збереженої процедури.

```cs
    [Theory]
    [InlineData(1, "Zippy")]
    [InlineData(2, "Rusty")]
    [InlineData(3, "Mel")]
    [InlineData(4, "Clunker")]
    [InlineData(5, "Bimmer")]
    [InlineData(6, "Hank")]
    [InlineData(7, "Pinky")]
    [InlineData(8, "Pete")]
    [InlineData(9, "Brownie")]
    public void ShouldGetValueFromStoredProc(int id, string expectedName)
    {
        Assert.Equal(expectedName, _carRepo.GetPetName(id));
    }
```

# Тестування CRUD

## Створення записів

Записи додаються до бази даних шляхом їх створення в коді, додавання до їхнього DbSet<T> і виклику SaveChanges()/SaveChangesAsync() у контексті. Коли виконується SaveChanges(), ChangeTracker повідомляє про всі додані об’єкти, а EF Core (разом із постачальником бази даних) створює відповідний оператор(и) SQL для вставлення запису(ів).
SaveChanges() виконується в неявній транзакції, якщо не використовується явна транзакція. Якщо збереження відбулося успішно, згенеровані сервером значення запитуються для встановлення значень для сутностей. Усі ці тести використовуватимуть явну транзакцію, щоб зміни можна було відкотити, залишаючи базу даних у тому самому стані, що й на момент початку виконання тесту.
Записи також можна додавати за допомогою похідного DbContext. Усі ці приклади використовуватимуть властивості колекції DbSet<T> для додавання записів. І DbSet<T>, і DbContext мають асинхронні версії Add()/AddRange().

## Додавання окремого запису

Наступний тест демонструє, як додати один запис до таблиці запасів:

```cs
[Fact]
public void ShouldAddACar()
{
    ExecuteInATransaction(RunTheTest);

    void RunTheTest()
    {
        int carCount = Context.Cars.Count();

        Car car = new Car
        {
            Color = "Yellow",
            MakeId = 1,
            PetName = "Herbie"
        };
        Context.Cars.Add(car);
        Assert.Equal(0,car.Id);

        int countAdded = Context.SaveChanges();
        
        int newCarCount = Context.Cars.Count();
        Assert.NotEqual(0, car.Id);  
        Assert.Equal(1,countAdded);
        Assert.Equal(carCount + 1, newCarCount);
    }
}
```
Тут показано виконану інструкцію SQL. Зауважте, що до нещодавно доданої сутності запитуються властивості, згенеровані базою даних (Id і TimeStamp). Коли результати запиту надходять до EF Core, сутність оновлюється значеннями на стороні сервера.
```sql
exec sp_executesql N'SET NOCOUNT ON;
INSERT INTO [dbo].[Inventory] ([Color], [MakeId], [PetName], [Price])
VALUES (@p0, @p1, @p2, @p3);
SELECT [Id], [DateBuilt], [Display], [IsDrivable], [TimeStamp]
FROM [dbo].[Inventory]
WHERE @@ROWCOUNT = 1 AND [Id] = scope_identity();
',
N'@p0 nvarchar(50),@p1 int,@p2 nvarchar(50),@p3 nvarchar(50)',@p0=N'Yellow',@p1=1,@p2=N'Herbie',@p3=NULL
```
Наступний тест створює нову сутність автомобіля з ідентифікатором, залишеним на нульовому значенні за замовчуванням. Коли сутність приєднана до ChangeTracker, стан встановлюється на Added, і виклик SaveChanges() додає сутність до бази даних.

```cs
[Fact]
public void ShouldAddACarWithAttach()
{
    ExecuteInATransaction(RunTheTest);
    void RunTheTest()
    {
        var car = new Car
        {
            Color = "Yellow",
            MakeId = 1,
            PetName = "Herbie"
        };
        var carCount = Context.Cars.Count();
        Context.Cars.Attach(car);
        Assert.Equal(EntityState.Added, Context.Entry(car).State);
        Context.SaveChanges();
        var newCarCount = Context.Cars.Count();
        Assert.Equal(carCount + 1, newCarCount);
    }
}
```

## Додавання кількох записів одночасно

Щоб вставити кілька записів в одну транзакцію, використовуйте метод AddRange() DbSet<T>, як показано в цьому тесті. Зауважте, що для SQL Server для використання пакетної обробки під час збереження даних має бути виконано принаймні чотири дії.

```cs
    [Fact]
    public void ShouldAddMultipleCars()
    {
        ExecuteInATransaction(RunTheTest);

        void RunTheTest()
        {
            //Have to add 4 to activate batching
            var cars = new List<Car>
                {
                    new() {Color = "Yellow", MakeId = 1, PetName = "Herbie"},
                    new() {Color = "White", MakeId = 2, PetName = "Mach 5"},
                    new() {Color = "Pink", MakeId = 3, PetName = "Avon"},
                    new() {Color = "Blue", MakeId = 4, PetName = "Blueberry"},
                };
            var carCount = Context.Cars.Count();
            Context.Cars.AddRange(cars);
            Context.SaveChanges();
            var newCarCount = Context.Cars.Count();
            Assert.Equal(carCount + 4, newCarCount);
        }
    }
```

Інструкції add об’єднуються в один виклик до бази даних, і всі згенеровані стовпці запитуються. Коли результати запиту надходять до EF Core, сутності оновлюються значеннями на стороні сервера. Виконаний оператор SQL показано тут:

```sql
exec sp_executesql N'SET NOCOUNT ON;
DECLARE @inserted0 TABLE ([Id] int, [_Position] [int]);
MERGE [dbo].[Inventory] USING (
VALUES (@p0, @p1, @p2, @p3, 0),
(@p4, @p5, @p6, @p7, 1),
(@p8, @p9, @p10, @p11, 2),
(@p12, @p13, @p14, @p15, 3))
AS i ([Color], [MakeId], [PetName], [Price], _Position) ON 1=0
WHEN NOT MATCHED THEN
INSERT ([Color], [MakeId], [PetName], [Price])
VALUES (i.[Color], i.[MakeId], i.[PetName], i.[Price])
OUTPUT INSERTED.[Id], i._Position
INTO @inserted0;
SELECT [t].[Id], [t].[DateBuilt], [t].[Display], [t].[IsDrivable], [t].[TimeStamp]
FROM [dbo].[Inventory] t
INNER JOIN @inserted0 i ON ([t].[Id] = [i].[Id])
ORDER BY [i].[_Position];
',
N'@p0 nvarchar(50),@p1 int,@p2 nvarchar(50),@p3 nvarchar(50),
  @p4 nvarchar(50),@p5 int,@p6 nvarchar(50),@p7 nvarchar(50),
  @p8 nvarchar(50),@p9 int,@p10 nvarchar(50),@p11 nvarchar(50),
  @p12 nvarchar(50),@p13 int,@p14 nvarchar(50),@p15 nvarchar(50)',
  @p0=N'Yellow',@p1=1,@p2=N'Herbie',@p3=NULL,@p4=N'White',@p5=2,
  @p6=N'Mach 5',@p7=NULL,@p8=N'Pink',@p9=3,@p10=N'Avon',@p11=NULL,@p12=N'Blue',
  @p13=4,@p14=N'Blueberry',@p15=NULL
```

### Додавання графа об’єктів

Наступний тест демонструє додавання графа об’єктів (пов’язаних записів Make, Car та Radio):

```cs
    [Fact]
    public void ShouldAddAnObjectGraph()
    {
        ExecuteInATransaction(RunTheTest);

        void RunTheTest()
        {
            Make make = new Make { Name = "Honda" };
            Car car = new()
            {
                Color = "Yellow",
                PetName = "Harbie",
                RadioNavigation = new Radio
                {
                    HasTweeters = true,
                    HasSubWoofers = true,
                    RadioId = "Bose 1234"
                }
            };

            make.Cars.Add(car);
            Context.Makes.Add(make);
            var carCount = Context.Cars.Count();
            var makeCount = Context.Makes.Count();
            Context.SaveChanges();
            var newCarCount = Context.Cars.Count();
            var newMakeCount = Context.Makes.Count();
            Assert.Equal(carCount + 1, newCarCount);
            Assert.Equal(makeCount + 1, newMakeCount);
        }
    }
```

##  Оновлення записів

Записи оновлюються, завантажуючи їх у DbSet<T> як відстежувану сутність, змінюючи їх за допомогою коду, а потім викликаючи SaveChanges() у контексті. Коли виконується SaveChanges(), ChangeTracker повідомляє про всі змінені об’єкти, а EF Core (разом із постачальником бази даних) створює відповідні оператори SQL для оновлення записів.

## Оновлення відстежуваних об’єктів

Наступний тест оновлює один запис, але процес є таким самим, якщо оновлено та збережено кілька відстежуваних об’єктів.

```cs
    [Fact]
    public void ShouldUpdateACar()
    {
        ExecuteInASharedTransaction(RunTheTest);

        void RunTheTest(IDbContextTransaction transaction)
        {
            Car? car = Context.Cars.Find(1);
            Assert.Equal("Black", car?.Color);
            car!.Color = "White";
            //Calling update is not needed because the entity is tracked
            //Context.Cars.Update(car);
            Context.SaveChanges();
            Assert.Equal("White", car.Color);

            var otherContext = TestHelpers.GetSecondContext(Context, transaction);
            Car? otherCar = otherContext.Cars.Find(1);
            Assert.Equal("White", otherCar?.Color);
        }
    }
```
Попередній код використовує спільну транзакцію між двома екземплярами ApplicationDbContext. Це необхідно для забезпечення ізоляції між контекстом, що виконує тест, і контекстом, що перевіряє результат тесту.

```sql
exec sp_executesql N'SET NOCOUNT ON;
UPDATE [dbo].[Inventory] SET [Color] = @p0
WHERE [Id] = @p1 AND [TimeStamp] = @p2;
SELECT [TimeStamp]
FROM [dbo].[Inventory]
WHERE @@ROWCOUNT = 1 AND [Id] = @p1;
',N'@p1 int,@p0 nvarchar(50),@p2 varbinary(8)',@p1=1,@p0=N'White',@p2=0x000000000000862D
```

## Оновлення невідстежуваних об’єктів

Невідстежувані сутності також можна використовувати для оновлення записів бази даних. Процес подібний до оновлення відстежуваних сутностей, за винятком того, що сутність створюється в коді, а не запитується, і EF Core має бути повідомлено, що сутність уже має існувати в базі даних і її потрібно оновити.

У наступному прикладі запис читається як невідстежуваний, створюється новий екземпляр класу Car із цього запису та змінюється одна властивість (Color). Потім він або встановлює стан, або використовує метод Update() для DbSet<T>, залежно від того, який рядок коду ви розкоментуєте. Метод Update() також змінює стан на Modified. Потім тест викликає SaveChanges(). Усі додаткові контексти є для забезпечення точності тесту та відсутності будь-якого перетину між контекстами.

```cs
    [Fact]
    public void ShouldUpdateACarAsNoTracking()
    {
        ExecuteInASharedTransaction(RunTheTest);
        void RunTheTest(IDbContextTransaction transaction)
        {
            Car? car = Context.Cars.AsNoTracking().First(c => c.Id == 1);
            Assert.Equal("Black", car?.Color);

            Car updatedCar = new()
            {
                Color = "White",
                Id = car!.Id,
                MakeId = car.MakeId,
                PetName = car.PetName,
                TimeStamp = car.TimeStamp,
                IsDrivable = car.IsDrivable
            };

            var context2 = TestHelpers.GetSecondContext(Context, transaction);
            context2.Cars.Update(updatedCar);
            //context2.Entry(updatedCar).State = EntityState.Modified;
            context2.SaveChanges();

            var context3 = TestHelpers.GetSecondContext(Context, transaction);
            Car? otherCar = context3.Cars.Find(1);
            Assert.Equal("White", otherCar?.Color);
        }
    }
```

## Перевірка паралельності під час оновлення записів

Якщо для сутності визначено властивість Timestamp, значення цієї властивості використовується в реченні where, коли зміни (оновлення чи видалення) зберігаються в базі даних. Замість простого пошуку первинного ключа значення TimeStamp додається до запиту, як у цьому прикладі:

```sql
UPDATE [dbo].[Inventory] SET [PetName] = @p0
WHERE [Id] = @p1 AND [TimeStamp] = @p2;
```
У наведеному нижче тесті показано приклад створення винятку паралельного виконання, його перехоплення та використання записів для отримання вихідних значень, поточних значень і значень, які наразі зберігаються в базі даних.Отримання поточних значень потребує ще одного виклику бази даних.

```cs
  [Fact]
  public void ShouldThrowConcurrencyException()
  {
      ExecuteInATransaction(RunTheTest);
      void RunTheTest()
      {
          var car = Context.Cars.First();

          //Update the database outside of the context
          FormattableString sql = 
              $"Update dbo.Inventory set Color='Pink' where Id = {car.Id}";
          Context.Database.ExecuteSqlInterpolated(sql);

          //update the car record in the change tracker
          car.Color = "Yellow";
          var ex = Assert.Throws<CustomConcurrencyException>(() => Context.SaveChanges());
          OutputHelper.WriteLine(ex.InnerException.Message);
          var entry = ((DbUpdateConcurrencyException)ex.InnerException)?.Entries[0];
          PropertyValues originalProps = entry.OriginalValues;
          PropertyValues currentProps = entry.CurrentValues;
          //This needs another database call
          PropertyValues databaseProps = entry.GetDatabaseValues();
      }
  }
```

## Видалення записів

Окрему сутність позначають для видалення викликом Remove() на DbSet<T> або встановленням її стану на Deleted. Список записів позначається для видалення шляхом виклику RemoveRange() у DbSet<T>. Процес видалення спричинить каскадні ефекти для властивостей навігації на основі правил, налаштованих у Fluent API (або умовностях EF Core). Якщо через каскадну політику видаленню заборонено, створюється виняток.

## Видалити відстежувані записи

Процес видалення аналогічний процесу оновлення. Після відстеження сутності викличте Remove() для цього екземпляра, а потім викличте SaveChanges(), щоб видалити запис із бази даних.

```cs
    [Fact]
    public void ShouldRemoveACar()
    {
        ExecuteInATransaction(RunTheTest);
        void RunTheTest()
        {
            int carCount = Context.Cars.Count();
            Car car = Context.Cars.Find(9)!;
            Context.Cars.Remove(car);
            Context.SaveChanges();
            Assert.Equal(carCount - 1, Context.Cars.Count());
            Assert.Equal(EntityState.Detached, Context.Entry(car).State);
        }
    }
```
Після виклику SaveChanges() екземпляр сутності все ще існує, але його більше немає в ChangeTracker. Під час перевірки стану EntityState стан буде Detached.

## Видалення невідстежуваних сутностей

Невідстежувані сутності можуть видаляти записи так само, як невідстежувані сутності можуть оновлювати записи. Різниця полягає в тому, що сутність відстежується шляхом виклику Remove()/RemoveRange() або встановлення стану на Deleted, а потім виклику SaveChanges(). У наступному прикладі запис читається як невідстежуваний. Потім він або встановлює стан, або використовує метод Remove() у DbSet<T> (залежно від того, який рядок ви розкоментуєте). Потім тест викликає SaveChanges(). Усі додаткові контексти створені для того, щоб уникнути перетину між контекстами.

```cs
     [Fact]
    public void ShouldRemoveACarAsNoTracking()
    {
        ExecuteInASharedTransaction(RunTheTest);
        void RunTheTest(IDbContextTransaction transacton)
        {
            var context1 = TestHelpers.GetSecondContext(Context, transacton);
            int countCar = context1.Cars.Count();
            Car? car = context1.Cars.
                AsNoTracking().
                IgnoreQueryFilters().First(c => c.Id == 9);

            var context2 = TestHelpers.GetSecondContext(Context, transacton);
            context2.Cars.Remove(car);
            //context2.Entry(car).State = EntityState.Deleted;
            context2.SaveChanges();

            Assert.Equal(countCar - 1, context2.Cars.Count());
            Assert.Equal(EntityState.Detached, Context.Entry(car).State);
        }
    }
```

## Перехоплення помилок каскадного видалення

EF Core викличе DbUpdateException, коли спроба видалити запис не вдається через правила каскаду. Наступний тест демонструє це в дії:

```cs
    [Fact]
    public void ShouldFailToRemoveACar()
    {
        ExecuteInATransaction(RunTheTest);
        void RunTheTest()
        {
            Car? car = Context.Cars.Find(1);
            if (car == null) return;
            Context.Cars.Remove(car);
            Assert.Throws<CustomDbUpdateException>(() => Context.SaveChanges());
        }
    }
```
Delete також використовує перевірку паралельності, якщо сутність має властивість TimeStamp. Відбуваеться те саме що і при оновлені даних.

# Підсумки

В цьому розділі ми створили проект тестування і налатували роботу тестів в транзакції яка відкачуетця. Створили тести для перевілки рівня доступу до даних. На цьому ми завершуємо нашу подорож крізь доступ до даних та Entity Framework Core. 