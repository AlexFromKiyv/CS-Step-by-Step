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
Додайте публічний статичний метод, щоб створити екземпляр інтерфейсу IConfiguration за допомогою файлу appsettings.testing.json. Додайте наступний код до класу:

```cs

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
