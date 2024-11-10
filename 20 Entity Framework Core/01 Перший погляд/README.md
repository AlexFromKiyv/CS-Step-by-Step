# Перший погляд на Entity Framework Core

У попередньому розділі розглядалися основи ADO.NET. Як ви бачили, ADO.NET дозволяє програмістам .NET працювати з реляційними даними. Хоча ADO.NET є потужним інструментом для роботи з даними, він не обов’язково є ефективним інструментом. Мається на увазі ефективність розробника. Щоб підвищити ефективність розробника, Microsoft представила нову структуру для доступу до даних під назвою Entity Framework (або просто EF). 
EF надає можливість взаємодії з даними з реляційних баз даних за допомогою об’єктної моделі, яка відображається безпосередньо в бізнес-об’єктах (або об’єктах домену) у вашій програмі. Наприклад, замість того, щоб розглядати пакет даних як набір рядків і стовпців, ви можете працювати з набором строго типізованих об’єктів, які називаються сутностями(entities). Ці сутності зберігаються в спеціалізованих класах колекцій, які підтримують LINQ, що дозволяє виконувати операції доступу до даних за допомогою коду C#.
Класи колекції забезпечують запити до сховища даних, використовуючи ту саму граматику LINQ.
На додаток до роботи з вашими даними як моделлю домену програми (замість нормалізованої моделі бази даних), EF забезпечує ефективність, наприклад відстеження стану, операції одиниць роботи та підтримку внутрішніх транзакцій.
EF Core можна використовувати з існуючими базами даних, які можна об’єднати в класи сутностей і похідний DbContext, або ви можете використовувати EF Core для створення/оновлення бази даних із ваших класів сутностей і похідного DbContext.

### Об'єктно-реляційне відображення (Object-Relational Mapper).

ADO.NET надає вам структуру, яка дозволяє вибирати, вставляти, оновлювати та видаляти дані за допомогою підключень, команд і читачів даних. Ці аспекти ADO.NET змушують вас обробляти отримані дані у спосіб, який тісно пов’язаний із фізичною схемою бази даних. Згадайте, наприклад, коли ви отримуєте записи з бази даних, ви відкриваєте з’єднання, створюєте та виконуєте об’єкт команди, а потім використовуєте зчитувач даних для повторення кожного запису, використовуючи специфічні для бази даних імена стовпців.
Коли ви використовуєте ADO.NET, ви завжди повинні пам’ятати про фізичну структуру внутрішньої бази даних. Ви повинні знати схему кожної таблиці даних, створювати потенційно складні SQL-запити для взаємодії з таблицями даних, відстежувати зміни в отриманих (або доданих) даних тощо. Це може змусити вас створити досить багатослівний код C#, оскільки сам C# не розмовляє безпосередньо мовою схеми бази даних.
Що ще гірше, спосіб, у який зазвичай будується фізична база даних, прямо зосереджений на конструкціях бази даних, таких як зовнішні ключі, представлення, збережені процедури та нормалізація даних, а не на об’єктно-орієнтованому програмуванні.
Ще одна проблема для розробників додатків – відстеження змін. Отримання даних із бази даних є одним із кроків процесу, але будь-які зміни, додавання та/або видалення повинні відстежуватися розробником, щоб їх можна було зберегти в сховищі даних.

Доступність структур об’єктно-реляційного відображення (зазвичай відомих як ORM) у .NET значно покращила історію доступу до даних, керуючи більшою частиною завдань доступу до даних CRUD для розробника. Розробник створює відображення між об’єктами .NET і реляційною базою даних, а ORM керує з’єднаннями, створенням запитів, відстеженням змін і збереженням даних. Це дозволяє розробнику зосередитися на бізнес-потребах програми.

Важливо пам’ятати, що ORM – це не чарівник. Кожне рішення передбачає компроміси. ORM зменшують обсяг роботи для розробників, створюючи рівні доступу до даних, але також можуть спричинити проблеми з продуктивністю та масштабуванням, якщо вони використовуються неправильно. Використовуйте ORM для операцій CRUD і потужність вашої бази даних для операцій на основі наборів. Незважаючи на те, що різні ORM мають невеликі відмінності в тому, як вони працюють і як вони використовуються, усі вони мають, по суті, однакові частини та прагнуть до однієї мети — полегшити операції доступу до даних. Сутності (entities) — це класи, які відображаються в таблицях бази даних. Спеціалізований тип колекції містить одну або кілька сутностей. Механізм відстеження змін відстежує стан сутностей і будь-які внесені до них зміни, доповнення та/або видалення, а центральна конструкція контролює операції як лідер.

### Роль EF

EF Core використовує інфраструктуру ADO.NET, яку ви вже розглянули в попередньому розділі. Як і будь-яка інша взаємодія ADO.NET зі сховищем даних, EF Core використовує постачальника даних ADO.NET для взаємодії зі сховищем даних. Перш ніж постачальник даних ADO.NET зможе використовувати EF Core, його потрібно оновити для повної інтеграції з EF Core. Завдяки цій додатковій функції у вас може бути менше доступних постачальників даних EF Core, ніж постачальників даних ADO.NET.
Перевага EF Core, що використовує шаблон постачальника бази даних ADO.NET, полягає в тому, що він дає змогу поєднувати парадигми доступу до даних EF Core і ADO.NET в одному проекті, розширюючи ваші можливості. Наприклад, використання EF Core для надання з’єднання, схеми та назви таблиці для операцій масового копіювання використовує можливості зіставлення EF Core та функції BCP, вбудовані в ADO.NET. Цей змішаний підхід робить EF Core ще одним інструментом у вашій скрині. 
Коли ви побачите, скільки основних засобів доступу до даних обробляється для вас зручним і ефективним способом, EF Core, швидше за все, стане вашим основним механізмом доступу до даних.
EF Core найкраще вписується в процес розробки в ситуаціях форм поверх даних (або API поверх даних). Операції з невеликою кількістю об’єктів із використанням шаблону одиниці роботи для забезпечення узгодженості є найкращим місцем для EF Core. Він не дуже добре підходить для великомасштабних операцій із даними, таких як складскі додадки із витягом-перетворенням-завантаженням (ETL) або великі ситуації звітності.

# Складові Entity Framework

Основними компонентами EF Core є DbContext, ChangeTracker, спеціалізований тип колекції DbSet, постачальники бази даних і сутності програми.

Створемо консольний додаток AutoLot.Samples. Додамо пакети 

Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.Design
Microsoft.EntityFrameworkCore.SqlServer

Пакет Microsoft.EntityFrameworkCore надає загальні функції для EF Core. Пакет Microsoft.EntityFrameworkCore.SqlServer надає постачальника даних SQL Server, а пакет Microsoft.EntityFrameworkCore.Design потрібен для інструментів командного рядка EF Core.

Додайте файл GlobalUsings.cs 

```cs
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.ChangeTracking;
global using Microsoft.EntityFrameworkCore.Design;
global using Microsoft.EntityFrameworkCore.Metadata;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
```

## Клас DbContext

Клас DbContext є керівним компонентом EF Core і забезпечує доступ до бази даних через властивість Database. DbContext керує екземпляром ChangeTracker, надає віртуальний метод OnModelCreating() для доступу до API Fluent, зберігає всі властивості DbSet<T> і надає метод SaveChanges для збереження даних у сховищі даних. Він використовується не безпосередньо, а через спеціальний клас, який успадковує DbContext. Саме в похідному класі розміщуються властивості DbSet<T>.

Члени DbContext які часто використовуються

|Член|Опис|
|----|----|
|Database|Надає доступ до інформації та функцій, пов’язаних із базою даних, включаючи виконання операторів SQL.|
|Model|Метадані про форму сутностей, зв’язки між ними та те, як вони відображаються в базі даних.|
|ChangeTracker|Надає доступ до інформації та операцій для екземплярів сутності, які відстежує цей DbContext.|
|DbSet<T>|Насправді не є членом DbContext, але властивості додано до настроюваного похідного класу DbContext. Властивості мають тип DbSet<T> і використовуються для запиту та збереження екземплярів сутностей програми. Запити LINQ щодо властивостей DbSet<T> перекладаються на запити SQL.|
|Entry()|Надає доступ до інформації про відстеження змін і операцій для сутності, таких як явне завантаження пов’язаних сутностей або зміна EntityState. Також можна викликати невідстежувану сутність, щоб змінити стан на відстежуваний.|
|Set<TEntity>()|Створює екземпляр властивості DbSet<T>, який можна використовувати для запиту та збереження даних.|
|SaveChanges()/SaveChangesAsync()|Зберігає всі зміни сутності в базі даних і повертає кількість задіяних записів. Виконується в транзакції (явній або неявній).|
|Add()/AddRange(), Update()/UpdateRange(), Remove()/RemoveRange()|Методи додавання, оновлення та видалення екземплярів сутності. Зміни зберігаються лише після успішного виконання SaveChanges(). Також доступні асинхронні версії. Примітка: Хоча ці методи доступні в похідному DbContext, вони зазвичай викликаються безпосередньо у властивостях DbSet<T>.|
|Find()|Знаходить сутність типу із заданими значеннями первинного ключа. Також доступні асинхронні версії. Примітка: Хоча ці методи доступні в похідному DbContext, вони зазвичай викликаються безпосередньо у властивостях DbSet<T>.
|
|Attach()/AttachRange()|Починає відстежувати сутність (або список сутностей).Також доступні асинхронні версії. Примітка: Хоча ці методи доступні в похідному DbContext, вони зазвичай викликаються безпосередньо у властивостях DbSet<T>.|
|SavingChanges()|Подія запускається на початку виклику SaveChanges()/SaveChangesAsync().|
|SavedChanges()|Подія запускається в кінці виклику SaveChanges()/SaveChangesAsync().|
|SaveChangesFailed|Подія запускається, якщо виклик SaveChanges()/SaveChangesAsync() не вдається.|
|OnModelCreating()|Викликається, коли модель ініціалізовано, але до її завершення. Методи з API Fluent розміщуються в цьому методі для завершення форми моделі.|
|OnConfiguring()|Конструктор, який використовується для створення або зміни параметрів для DbContext. Примітка. Рекомендується не використовувати це, натомість використовувати DbContextOptions для налаштування екземпляра DbContext під час виконання та використовувати екземпляр IDesignTimeDbContextFactory під час розробки.|

## Побудова похідного касу від DBContext

### Створення класу ApplicationDbContext

Першим кроком у EF Core є створення спеціального класу, який успадковує DbContext.

Додайте клас із назвою ApplicationDbContext.cs

```cs
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options) 
        { 
        }
    }
```
Додано конструктор, який приймає строго типізований екземпляр DbContextOptions і передає екземпляр до базового класу. Це клас, який використовується для доступу до бази даних і роботи з сутностями, трекером змін і всіма компонентами EF Core.

### Конфігурування DbContext

Екземпляр похідного від DbContext класу налаштовується за допомогою екземпляра класу DbContextOptions. Екземпляр DbContextOptions створюється за допомогою DbContextOptionsBuilder, оскільки клас DbContextOptions не призначений для безпосереднього створення у вашому коді. За допомогою екземпляра DbContextOptionsBuilder вибирається постачальник бази даних (разом із будь-якими параметрами, що стосуються постачальника), і встановлюються загальні параметри EF Core DbContext (наприклад, журналювання). Потім екземпляр DbContextOptions передається в базовий DbContext під час виконання.
Ця можливість динамічної конфігурації дозволяє змінювати налаштування під час виконання, просто вибираючи різні параметри (наприклад, MySQL замість постачальника SQL Server) і створюючи новий екземпляр вашого похідного DbContext.

### DbContext Factory під час розробки

Фабрика DbContext під час розробки — це клас, який реалізує інтерфейс IDesignTimeDbContextFactory<T>, де T — похідний клас DbContext. Інтерфейс має один метод CreateDbContext(), який ви повинні реалізувати, щоб створити екземпляр вашого похідного DbContext. Цей клас не призначений для використання у виробництві, а лише під час розробки, і існує в основному для інструментів командного рядка EF Core, які ви незабаром дослідите. У прикладах у цьому та наступному розділах він використовуватиметься для створення нових екземплярів ApplicationDbContext. Вважається поганою практикою використовувати фабрику DbContext для створення екземплярів вашого похідного класу DbContext. Пам’ятайте, що це демонстраційний код, призначений для навчання, і використання його таким чином робить демонстраційний код чистішим. Ви побачите, як правильно створити екземпляр похідного класу DbContext у розділі ASP.NET Core.

Додамо клас ApplicationDbContextFactory.

```cs
namespace AutoLot.Samples;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        string connectionString = @"Server=(localdb)\mssqllocaldb;Database=AutoLotSamples;Trusted_Connection=True;ConnectRetryCount=0";

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(connectionString);
        
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
```
Клас ApplicationDbContextFactory використовує метод CreateDbContext() для створення строго типізованого DbContextOptionsBuilder для класу ApplicationDbContext, встановлює постачальника бази даних на постачальника SQL Server (використовуючи рядок підключення екземпляра Docker з попереднього розділу), а потім створює та повертає новий екземпляр ApplicationDbContext.

Знову ж таки, фабрика контексту розроблена для інтерфейсу командного рядка EF Core для створення екземпляра похідного класу DbContext, а не для використання у виробництві. Інтерфейс командного рядка використовує фабрику під час виконання таких дій, як створення або застосування міграції бази даних. Однією з основних причин, чому ви не хочете використовувати це у виробництві, є жорстко закодований рядок підключення. Оскільки це призначено для використання під час розробки, використання встановленого рядка підключення, який вказує на базу даних розробки, працює ідеально.
Метод CreateDbContext() приймає масив рядків з CLI як аргумент.

### OnModelCreating

Базовий клас DbContext надає метод OnModelCreating, який використовується для формування ваших сутностей за допомогою Fluent API. Це буде детально розглянуто пізніше в цьому розділі, а поки додайте наступний код до класу ApplicationDbContext

```cs
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent API calls go here / Сюди надходять виклики Fluent API
        }
```

## Використання похідного класу DBContext

### Збереження змін

Додайте до файлу GlobalUsings.cs

```cs
global using AutoLot.Samples;
```
Додайте метод до Program.cs

```cs
static void SimpleSaveCahnges()
{
    var context = new ApplicationDbContextFactory().CreateDbContext([]);
    //make some changes
    context.SaveChanges();
}
//SimpleSaveCahnges();
```
Щоб зберегти будь-які зміни (додати, оновити чи видалити) до сутностей, викличте метод SaveChanges() (або SaveChangesAsync()) у похідному DbContext. Методи SaveChanges()/SaveChangesAsync() обертають виклики бази даних у неявну транзакцію та зберігають їх як одиницю роботи.
Далі буде багато прикладів збереження змін.

### Підтримка транзакцій і точки збереження

EF Core обертає кожен виклик SaveChanges()/SaveChangesAsync() у неявну транзакцію. За замовчуванням транзакція використовує рівень ізоляції бази даних. Для більшого контролю ви можете залучити похідний DbContext до явної транзакції замість використання неявної транзакції за замовчуванням.

Щоб виконати явну транзакцію, створіть транзакцію за допомогою властивості Database похідного DbContext. Виконайте свою операцію (операції) як зазвичай, а потім зафіксуйте або відкотіть транзакцію. Ось фрагмент коду, який демонструє це

```cs
static void TransactedSaveChanges()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null!);
    using var transaction = context.Database.BeginTransaction();
	try
	{
        //Create, change, delete stuff
        context.SaveChanges();
        transaction.Commit();
    }
    catch (Exception ex)
	{
        Console.WriteLine(ex.Message);
        transaction.Rollback();
	}
}
```

Є можливість створювати точки збереження. 

```cs
static void UsingSavePoints()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null!);
    using var transaction = context.Database.BeginTransaction();
    try
    {
        //Create, change, delete stuff
        transaction.CreateSavepoint("check point 1");
        context.SaveChanges();
        transaction.Commit();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        transaction.RollbackToSavepoint("check point 1");
    }
}
```
Коли викликається SaveChanges()/SaveChangesAsync() і транзакція вже виконується, EF Core створює точку збереження в цій транзакції. Якщо виклик не вдається, транзакція повертається до точки збереження, а не до початку транзакції. Точками збереження можна керувати програмно, викликаючи CreateSavePoint() і RollbackToSavepoint() у транзакції.

### Явні транзакції та стратегії виконання

Коли стратегія виконання активна (про це йдеться в наступному розділі в розділі «Відмовостійкість з’єднання»), перш ніж створювати явну транзакцію, ви повинні отримати посилання на поточну стратегію виконання, яка використовується. Потім викличте метод Execute() у стратегії, щоб створити явну транзакцію.

```cs
static void TransctionWithExecutionStrateies()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    var strategy = context.Database.CreateExecutionStrategy();
    strategy.Execute(() =>
    {
        using var transaction = context.Database.BeginTransaction();
        try
        {
            // actionToExecute();
            transaction.Commit();
            Console.WriteLine("Succeesed.");
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Console.WriteLine(ex.Message);
        }
    });
}
```

### Події SavingChanges, SavedChanges, SaveChangesFailed.

Ці події ініціюються методами SaveChanges()/SaveChangesAsync(). Подія SavingChanges запускається під час виклику SaveChanges(), але до виконання інструкцій SQL для сховища даних. Подія SavedChanges запускається після завершення SaveChanges(). Подія SaveChangesFailed запускається, якщо виклик SaveChanges() був невдалим.
Додамо обробники подій до конструктора класу ApplicationDbContext

```cs
        // Constructors
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options) 
        {
            SavingChanges += (sender, args) =>
            {
                Console.WriteLine($"Saving change for {((DbContext)sender!).Database.GetConnectionString()}");
            };

            SavedChanges += (sender, args) =>
            {
                Console.WriteLine($"Saved change {args.EntitiesSavedCount} entities");
            };

            SaveChangesFailed += (sender, args) =>
            {
                Console.WriteLine($"An exception orruced! {args.Exception.Message} entities ");               
            };
        }
```

### Клас DbSet<T>

Для кожного типу сутності (T) у вашій об’єктній моделі ви додаєте властивість типу DbSet<T> до похідного класу DbContext. Клас DbSet<T> — це спеціалізована властивість колекції, яка використовується для взаємодії з постачальником бази даних для читання, додавання, оновлення чи видалення записів у базі даних. Кожен DbSet<T> надає низку основних служб для взаємодії з базою даних, включаючи переклад запитів LINQ, що виконуються щодо властивості DbSet<T>, у запити до бази даних постачальником бази даних.

Загальні члени та методи розширення DbSet<T>

|Члени та методи розширення|Опис|
|--------------------------|----|
|Add()/AddRange()|Починає відстежувати сутність/сутності в стані Додано. Елемент(и) буде додано під час виклику SaveChanges(). Також доступні асинхронні версії.|
|AsAsyncEnumerable()|Повертає колекцію як IAsyncEnumerable<T>.|
|AsQueryable()|Повертає колекцію як IQueryable<T>.|
|Find()|Шукає сутність у ChangeTracker за первинним ключем. Якщо об’єкт не знайдено в системі відстеження змін, у сховищі даних запитується об’єкт. Також доступна асинхронна версія.|
|Update() UpdateRange()|Починає відстежувати сутність/сутності у зміненому стані. Елемент(и) буде оновлено під час виклику SaveChanges. Також доступні асинхронні версії.|
|Remove() RemoveRange()|Починає відстежувати сутність/сутності у стані "Видалено". Елемент(и) буде видалено під час виклику SaveChanges(). Також доступні асинхронні версії.|
|Attach() AttachRange()|Починає відстежувати сутність/сутності. Сутності з цифровими первинними ключами, визначеними як ідентичність, і значенням, що дорівнює нулю, відстежуються як Додані. Усі інші відстежуються як незмінені. Також доступні асинхронні версії.|
|FromSqlRaw() FromSqlInterpolated()|Створює запит LINQ на основі необробленого або інтерпольованого рядка, що представляє запит SQL. Може поєднуватися з додатковими операторами LINQ для виконання на сервері.|


Тип DbSet<T> реалізує IQueryable<T>, що дозволяє використовувати запити LINQ для отримання записів із бази даних. На додаток до методів розширення, доданих EF Core, DbSet<T> підтримує ті самі методи розширення, про які ви дізналися в розділі про LINQ, наприклад ForEach(), Select() і All(). 
Ви додасте властивості DbSet<T> до ApplicationDbContext пізніше.

### ChangeTracker

Екземпляр ChangeTracker відстежує стан об’єктів, завантажених у DbSet<T> в екземплярі DbContext.

Значення перерахування стану сутності
|Значення|Опис|
|--------|----|
|Added|Сутність відстежується, але ще не існує в базі даних.|
|Deleted|Сутність відстежується та позначена для видалення з бази даних.|
|Detached |Сутність не відстежується засобом відстеження змін.|
|Modified|Сутність відстежується та був змінений.|
|Unchanged|Сутність відстежується, існує в базі даних і не була змінена.|


Якщо вам потрібно перевірити стан об'єкта, використовуйте такий код:

```cs
EntityState state = context.Entry(entity).State;
```

Ви також можете програмно змінити стан об’єкта, використовуючи той самий механізм. Щоб змінити стан на Видалено (наприклад), використовуйте такий код:

```cs
context.Entry(entity).State = EntityState.Deleted;
```

### Події ChangeTracker

Є дві події, які можуть бути викликані у ChangeTracker. Перша — StateChanged, а друга — Tracked. Подія StateChanged запускається, коли змінюється стан об’єкта. Він не спрацьовує під час першого відстеження сутності. Подія Tracked спрацьовує, коли сутність починає відстежуватися, або через програмне додавання до екземпляра DbSet<T>, або коли повертається із запиту.
Оновіть конструктор для класу ApplicationDbContext до такого, щоб указати обробники подій для подій StateChanged і Tracked:

```cs
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options) 
        {
  
            // ... 

            ChangeTracker.StateChanged += ChangeTracker_StateChanged;
            ChangeTracker.Tracked += ChangeTracker_Tracked;
        }
```

### StateChanged

Як згадувалося, подія StateChanged запускається, коли стан сутності змінюється, але не тоді, коли сутність відстежується вперше.

```cs
        private void ChangeTracker_StateChanged(object? sender, EntityStateChangedEventArgs e)
        {
            if (e.OldState == EntityState.Modified && e.NewState == EntityState.Unchanged)
            {
                Console.WriteLine($"An entity of type {e.Entry.Entity.GetType().Name} was update.");
            }
        }
```

OldState і NewState доступні через EntityStateChangedEventArgs.

### Tracked

Подія Tracked запускається, коли ChangeTracker починає відстежувати сутність. У наступному прикладі виконується запис на консоль кожного разу, коли сутність завантажується з бази даних.

```cs
        private void ChangeTracker_Tracked(object? sender, EntityTrackedEventArgs e)
        {
            if (e.FromQuery)
            {
                Console.WriteLine($"An entity of type {e.Entry.Entity.GetType().Name} was loaded from the database.");
            }
        }
```
Властивість FromQuery EntityTrackedEventArgs вказує, чи була сутність завантажена через запит бази даних чи програмно.

### Скидання стану DbContext

Існує можливість скинути похідний DbContext назад до початкового стану. Метод ChangeTracker.Clear() видаляє всі сутності з колекцій DbSet<T>, установлюючи для них стан Detached. Основною перевагою цього є підвищення продуктивності. Як і з будь-яким ORM, створення екземпляра похідного класу DbContext вимагає певних витрат. Хоча ці накладні витрати зазвичай не є значними, можливість очистити вже створений контекст може допомогти підвищити продуктивність у деяких сценаріях.

## Сутності

Строго типізовані класи, які відображаються в таблицях бази даних, офіційно називаються сутностями. Набір сутностей у програмі містить концептуальну модель фізичної бази даних. Формально кажучи, ця модель називається моделлю даних сутності (entity data model EDM ), яку зазвичай називають просто моделю. Модель зіставляється з доменом програми/бізнесу. Сутності та їхні властивості зіставляються з таблицями та стовпцями за допомогою угод Entity Framework Core, конфігурації та Fluent API (код). Сутності не потребують прямого зіставлення зі схемою бази даних. Ви можете структурувати свої класи сутностей відповідно до потреб програми, а потім зіставляти унікальні сутності зі схемою бази даних.
Цей слабкий зв’язок між базою даних і вашими об’єктами означає, що ви можете формувати об’єкти відповідно до домену вашого бізнесу, незалежно від конструкції та структури бази даних. Наприклад, візьмемо просту таблицю Inventory в базі даних AutoLot і клас сутності Car з попереднього розділу.Назви різні, але сутність Car можна зіставити з таблицею  Inventory. EF Core перевіряє конфігурацію ваших сутностей у моделі, щоб зіставити представлення клієнтської сторони таблиці Inventory (у нашому прикладі класу Car) на правильні стовпці таблиці Inventory. 
У наступних кількох розділах детально описано, як угоди EF Core, анотації даних і код (за допомогою Fluent API) відображають сутності, властивості та зв’язки між сутностями на таблиці, стовпці та зв’язки зовнішнього ключа у вашій базі даних.

### Властивості сутності та стовпці бази даних

Під час використання реляційного сховища даних EF Core використовує дані зі стовпців таблиці для заповнення властивостей сутності під час читання зі сховища даних і записує властивості сутності в стовпці таблиці під час збереження даних. Якщо властивість є автоматичною властивістю, EF Core читає та записує через getter і setter. Якщо властивість має резервне поле, EF Core читатиме та записуватиме в резервне поле замість публічної власності, навіть якщо опорне поле є приватним. Хоча EF Core може читати та записувати в приватні поля, все одно має бути загальнодоступна властивість читання та запису, яка інкапсулює резервне поле. Два сценарії, коли підтримка резервного поля є перевагою, це використання шаблону INotifyPropertyChanged у програмах Windows Presentation Foundation (WPF) і коли значення за замовчуванням бази даних суперечать значенням за замовчуванням .NET.

### Схеми відображення таблиць

У EF Core доступні дві схеми відображення класів у таблиці: таблиця за ієрархією (table-per-hierarchy TPH) і таблиця за типом (table-per-type TPT). Відображення TPH є типовим і відображає ієрархію успадкування в одній таблиці. TPT відображає кожен клас в ієрархії на окрему таблицю. 
Класи також можуть бути зіставлені з представленнями та необробленими запитами SQL.

### Відображення таблиці на ієрархію (Table-Per-Hierarchy)

Створемо проект AutoLot.TPH та додамо до нього пакети. 

Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.Design
Microsoft.EntityFrameworkCore.SqlServer

Додайте папку Models. Розглянемо наступний приклад, який показує, що клас Car з попереднього розділу можна розділити на два класи: базовий клас (BaseEntity) для властивостей Id і TimeStamp, а також решту властивостей у класі Car.


AutoLot.TPH\Models\BaseEntity.cs
```cs
namespace AutoLot.TPH.Models;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public byte[] TimeStamp { get; set; }
}
```
AutoLot.TPH\Models\Car.cs

```cs
public class Car : BaseEntity
{
    public string Color { get; set; }           
    public string PetName { get; set; }
    public int MakeId { get; set; }
}
```
Щоб повідомити EF Core, що клас сутності є частиною об’єктної моделі, додайте властивість DbSet<T> для сутності. Створіть клас ApplicationDbContext і оновіть його до такого:

AutoLot.TPH\ApplicationDbContext.cs

```cs
using Microsoft.EntityFrameworkCore;
using AutoLot.TPH.Models;

namespace AutoLot.TPH;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Car> Cars { get; set; }
}
```
Зверніть увагу на властивість типу DbSet<T> у класі ApplicationDbContext. Це інформує EF Core, що клас Car зіставляється з таблицею Cars у базі даних. Також зауважте, що для класу BaseEntity немає властивості DbSet<T>. Це пояснюється тим, що в схемі TPH вся ієрархія стає єдиною таблицею. Властивості таблиць вище ланцюга успадкування згортаються в таблицю за допомогою властивості DbSet<T>. Це показано наступним SQL:

```sql
CREATE TABLE [dbo].[Cars](
    [Id] [INT] IDENTITY(1,1) NOT NULL,
    [Color] [NVARCHAR](MAX) NULL,
    [PetName] [NVARCHAR](MAX) NULL,
    [MakeId] [INT] NOT NULL,
    [TimeStamp] [VARBINARY](MAX) NULL,
 CONSTRAINT [PK_Cars] PRIMARY KEY CLUSTERED
(
    [Id] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF,
IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON,
OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
```

### Відображення таблиці за типом (Table-per-Type TPT)

Створемо проект AutoLot.TPT та додамо до нього пакети та такуж саму теку Models.

Щоб дослідити схему відображення TPT, можна використовувати класи BaseEntity та Car, навіть якщо базовий клас позначено як абстрактний. Оскільки TPH використовується за замовчуванням, EF Core має отримати вказівку відображати кожен клас у таблиці. Це можна зробити за допомогою анотацій даних (показаних далі в цьому розділі) або Fluent API. Щоб використовувати схему зіставлення TPT, використовуйте наведений нижче код API Fluent у методі OnModelCreating() ApplicationDbContext. 

AutoLot.TPT\ApplicationDbContext.cs
```cs
using AutoLot.TPT.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoLot.TPT;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Car> Cars { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaseEntity>().ToTable("BaseEntities");
        modelBuilder.Entity<Car>().ToTable("Cars");
    }
}
```
EF Core створить дві таблиці, показані тут. Індекси також показують, що таблиці мають однозначне відображення між таблицями BaseEntities і Cars.

```sql
CREATE TABLE [dbo].[BaseEntities](
    [Id] [INT] IDENTITY(1,1) NOT NULL,
    [TimeStamp] [VARBINARY](MAX) NULL,
 CONSTRAINT [PK_BaseEntities] PRIMARY KEY CLUSTERED
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF,
ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[Cars](
    [Id] [INT] NOT NULL,
    [Color] [NVARCHAR](MAX) NULL,
    [PetName] [NVARCHAR](MAX) NULL,
    [MakeId] [INT] NOT NULL,
 CONSTRAINT [PK_Cars] PRIMARY KEY CLUSTERED
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Cars]  WITH CHECK ADD  CONSTRAINT [FK_Cars_BaseEntities_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[BaseEntities] ([Id])
GO
ALTER TABLE [dbo].[Cars] CHECK CONSTRAINT [FK_Cars_BaseEntities_Id]
GO
```
Відображення таблиці за типом може мати значні наслідки для продуктивності, які слід враховувати перед використанням цієї схеми відображення. Для отримання додаткової інформації зверніться до документації.

### Властивості навігації та зовнішні ключі

Властивості навігації представляють, як класи сутностей пов’язані один з одним і дозволяють коду переходити від одного екземпляра сутності до іншого. За визначенням властивість навігації — це будь-яка властивість, яка відображається на нескалярний тип, як визначено постачальником бази даних. На практиці властивості навігації відображаються на іншу сутність (так звані властивості навігації посилання) або колекцію іншої сутності (називаються властивості навігації колекції). На стороні бази даних властивості навігації транслюються у зв’язки зовнішнього ключа між таблицями. Зв’язки «один до одного», «один до багатьох» і «багато до багатьох» підтримуються безпосередньо в EF Core. Класи сутностей також можуть мати властивості навігації, які вказують на себе, представляючи таблиці з самопосиланнями. Перш ніж охоплювати деталі навігаційних властивостей і шаблонів зв’язків сутностей, зверніться до термінів в моделях відносин.

Терміни, що використовуються для опису навігаційних властивостей і зв’язків

|Термін|Опис|
|------|----|
|Principal entity(Основна сутність)|Сутність з якої виходять відносини.|
|Dependent entity(Залежна сутність)|Сутність яка залежить від іншої.|
|Principal key(Основний ключ)|Властивість/властивості, які використовуються для визначення головної сутності. Може бути первинним або альтернативним ключем. Ключі можна налаштувати за допомогою однієї властивості або кількох властивостей.|
|Foreign key(Зовнішній ключ)|Властивість/властивості, які зберігаються дочірньою сутністю для зберігання основного ключа.|
|Required relationship(Необхідні відносини)|Зв’язок, де потрібне значення зовнішнього ключа (не допускає значення null).|
|Optional relationship(Необов'язковий зв'язок)|Відношення, де значення зовнішнього ключа може буте відсутьне(nullable).|

#### Відсутні властивості зовнішнього ключа

Якщо сутність із властивістю посилальної навігації не має властивості для значення зовнішнього ключа, EF Core створить необхідну властивість/властивості сутності. Вони відомі як тіньові властивості зовнішнього ключа та мають імена у форматі <navigation property name><principal key property name> або <principal entity name><principal key property name>. Це справедливо для всіх типів зв’язків (один до багатьох, один до одного, багато до багатьох). Набагато чистіший підхід створити свої сутності з явною властивістю/властивостями зовнішнього ключа, ніж змусити EF Core створити їх для вас.

#### Відносини One-to-Many

Щоб створити зв’язок «One-to-Many», клас сутності з боку One (the principal) додає властивість колекції класу сутності, який знаходиться з боку Many (the dependent). Залежна сутність також повинна мати властивості для зовнішнього ключа назад до основної. Якщо ні, EF Core створить тіньові властивості зовнішнього ключа, як пояснювалося раніше.
Наприклад, у базі даних, створеній раніше, таблиці Makes (представлена ​​класом сутності Make) і таблиця Inventory (представлена ​​класом сутності Car) мають зв’язок «One-to-Many».


Додамо в проекті AutoLot.Samples папку Models. Створимо сутності і відносини. 

AutoLot.Samples\Models\BaseEntity.cs
```cs
namespace AutoLot.Samples.Models;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public byte[] TimeStamp { get; set; }
}
```

AutoLot.Samples\Models\Make.cs
```cs
namespace AutoLot.Samples.Models;

public class Make : BaseEntity
{
    public string Name { get; set; }
    public IEnumerable<Car> Cars { get; set; } = new List<Car>();
}
```

AutoLot.Samples\Models\Car.cs
```cs
namespace AutoLot.Samples.Models;

public class Car : BaseEntity
{
    public string Color { get; set; }
    public string PetName { get; set; }
    public int MakeId { get; set; }
    public Make MakeNavigation { get; set; }
}
```
У прикладі Car/Make сутність Car є залежною сутністю (the many of the one-to-many), а сутність Make є основною сутністю (the one of the one-to-many). 

Під час формування існуючої бази даних (як ви зробите в наступному розділі), EF Core іменує властивості навігації посилань так само, як ім’я типу властивості (наприклад, public Make Make {get; set;}). Це може спричинити проблеми з навігацією та IntelliSense, а також ускладнити роботу з кодом. Можна додавати суфікс Navigation до властивостей посилання на навігацію для ясності, як показано в попередньому прикладі.

Оновіть файл GlobalUsings.cs, щоб включити новий простір імен для моделей.

AutoLot.Samples\GlobalUsings.cs
```cs
\\...

global using AutoLot.Samples.Models;
```

Далі додайте властивості DbSet<Car> і DbSet<Make> до ApplicationDbContext.

```cs
    //Properties
    public DbSet<Car> Cars { get; set; }
    public DbSet<Make> Makes { get; set; }
```
Тепер коли у нас створено невеликий похідний клас DBContext з даними підключеня до бази даних і двома сутностями ми можемо створити базу данних. Для цого нам знадобится CLI EF Core Global Tool якій детально описан нижче.

Якщо не встановлено, всановимо dotnet-ef глобальний CLI інструмент EF Core

```console
dotnet tool install --global dotnet-ef
```
Щоб перевірити шо він всановлено введіть команду.

```console
dotnet ef
```

Створимо нову міграцію яка створює базу даних.

```console
dotnet ef migrations add Initial_CreateDB_Add_Makes_Cars 
```
Після цього в каталозі проекту з'явится тека Migrations. Подивимось які зміни будуть зроблені при оновлені БД цією міграцією.

```console
dotnet ef migrations script --no-transactions
```

Виконається наступний запит:
```console
CREATE TABLE [Makes] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [TimeStamp] varbinary(max) NOT NULL,
    CONSTRAINT [PK_Makes] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Cars] (
    [Id] int NOT NULL IDENTITY,
    [Color] nvarchar(max) NOT NULL,
    [PetName] nvarchar(max) NOT NULL,
    [MakeId] int NOT NULL,
    [TimeStamp] varbinary(max) NOT NULL,
    CONSTRAINT [PK_Cars] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Cars_Makes_MakeId] FOREIGN KEY ([MakeId]) REFERENCES [Makes] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Cars_MakeId] ON [Cars] ([MakeId]);
GO
```

Зверніть увагу на зовнішній ключ MakeId і перевірку обмеження FK_Cars_Makes_MakeId, створені в залежній таблиці (Cars).

Для створення бази даних треба виконаним команду 

```console
dotnet ef database update
```
В SQL Server Object Explorer можна побачити нову БД.

#### Відносини One-to-One

У зв’язках «One-to-One» обидві сутності мають посилання на навігаційну властивість для іншої сутності. Під час побудови зв’язків «One-to-One» EF Core має знати, яка сторона є головною сутністю. Це можна зробити, маючи чітко визначений зовнішній ключ до головної сутності або вказавши головну сутьність за допомогою Fluent API. Якщо EF Core не отримує інформацію за допомогою одного з цих двох методів, він вибере один на основі своєї здатності виявляти зовнішній ключ. На практиці ви повинні чітко визначити залежну, додавши властивості зовнішнього ключа. Це усуває будь-яку неоднозначність і гарантує, що ваші таблиці правильно налаштовані.

Додайте новий клас під назвою Radio.cs.

```cs
namespace AutoLot.Samples.Models;

public class Radio : BaseEntity
{
    public bool HasTweeters { get; set; }
    public bool HasSubWoofers { get; set; }
    public string RadioId { get; set; }
    public int CarId { get; set; }
    public Car CarNavigation { get; set; }
}
```
Додайте властивість навігації до класу Car:

```cs
public class Car : BaseEntity
{
    // ...

    public Radio RadioNavigation { get; set; }
}
```
Оскільки Radio має зовнішній ключ до класу Car (заснований на конвенції, про яку ми розглянемо пізніше), Radio є залежною сутністю, а Car є основною сутністю. EF Core неявно створює необхідний унікальний індекс властивості зовнішнього ключа в залежній сутності. Якщо ви хочете змінити назву індексу, це можна зробити за допомогою анотацій даних або Fluent API.

Додайте властивість DbSet<Radio> до класу ApplicationDbContext:

```cs
    public class ApplicationDbContext : DbContext
    {
        // Properies

        //...

        public DbSet<Radio> Radios { get; set; }
    
        //...
    }    
```
Зберіжіть змінені файли (Save All). Виконаємо наступну команду.

```console
PS D:\...\AutoLot.Samples> dotnet ef migrations has-pending-model-changes
Build started...
Build succeeded.
Changes have been made to the model since the last migration. Add a new migration.
```
Це каже про те що EF Core виявив що ми зробили зміни в моделі даних. Тепер створемо міграцію з назвою яка відповідає змінам.

```console
dotnet ef migrations add Add_Radios
```
Подивимось зміни які буде робить міграція.

```console
 dotnet ef migrations script 20241023150501_Initial_CreateDB_Add_Makes_Cars
...
CREATE TABLE [Radios] (
    [Id] int NOT NULL IDENTITY,
    [HasTweeters] bit NOT NULL,
    [HasSubWoofers] bit NOT NULL,
    [RadioId] nvarchar(max) NOT NULL,
    [CarId] int NOT NULL,
    [TimeStamp] varbinary(max) NOT NULL,
    CONSTRAINT [PK_Radios] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Radios_Cars_CarId] FOREIGN KEY ([CarId]) REFERENCES [Cars] ([Id]) ON DELETE CASCADE
);
GO

CREATE UNIQUE INDEX [IX_Radios_CarId] ON [Radios] ([CarId]);
GO
...
```
Зверніть увагу на зовнішній ключ і обмеження перевірки, створені в залежній таблиці Radios. Коли базу даних оновлено за допомогою таких міграцій EF Core, таблиця Cars не змінюється.

Оновимо БД

```console
 dotnet ef database update
 ...
 Applying migration '20241023155227_Add_Radios'.
Done.
```


#### Відносини Many-to-Many

У зв’язках «Many-to-Many» обидві сутності мають властивість навігації колекції для іншої сутності. Це реалізовано в сховищі даних із окремою об’єднаною таблицею між двома таблицями сутностей. Ця об’єднана таблиця за замовчуванням названа на честь двох таблиць за допомогою <Entity1Entity2>, але її можна змінити програмно за допомогою Fluent API. Сутність об’єднання має зв’язки «One-to-Many» із кожною з таблиць сутностей.

Створіть клас Driver, який матиме зв’язок багато-до-багатьох із класом Car.

```cs

namespace AutoLot.Samples.Models;

public class Driver : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public IEnumerable<Car> Cars { get; set; } = new List<Car>(); 
}
```

Додайте властивість DbSet<Driver> до класу ApplicationDbContext

```cs
    public class ApplicationDbContext : DbContext
    {
        // Properies

        //...

        public DbSet<Driver> Drivers { get; set; }
    
        //...
    }    
```
Далі оновіть клас Car, щоб мати властивість навігації колекції для нового класу Driver:

```cs
public class Car : BaseEntity
{
    // ...

    public IEnumerable<Driver> Drivers { get; set; }
}
```

Збережемо всі міни та створимо нову міграцію.

```console
dotnet ef migrations add Add_Drivers
```
Подивимось як буде змінено БД.

```console
PS D:\...\AutoLot.Samples> dotnet ef migrations script 20241023155227_Add_Radios
...

CREATE TABLE [Drivers] (
    [Id] int NOT NULL IDENTITY,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [TimeStamp] varbinary(max) NOT NULL,
    CONSTRAINT [PK_Drivers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [CarDriver] (
    [CarsId] int NOT NULL,
    [DriversId] int NOT NULL,
    CONSTRAINT [PK_CarDriver] PRIMARY KEY ([CarsId], [DriversId]),
    CONSTRAINT [FK_CarDriver_Cars_CarsId] FOREIGN KEY ([CarsId]) REFERENCES [Cars] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CarDriver_Drivers_DriversId] FOREIGN KEY ([DriversId]) REFERENCES [Drivers] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_CarDriver_DriversId] ON [CarDriver] ([DriversId]);
GO
```
Коли база даних оновлюється, таблиця Cars не змінюється, а таблиці Drivers і CarDriver створюються. 
Зверніть увагу, що складений первинний ключ, обмеження перевірки (зовнішні ключі) і каскадна поведінка створені EF Core, щоб переконатися, що таблицю CarDriver налаштовано як правильну таблицю об’єднання.

Оновимо БД.

```console
dotnet ef database update
```

Еквівалентний зв’язок Car-Driver «Many-to-Many» можна досягти шляхом явного створення трьох таблиць(і саме так це має бути зроблено у версіях EF Core, раніших за EF Core 5). 

Ось скорочений приклад

```cs
public class Driver
{
...
  public IEnumerable<CarDriver> CarDrivers { get; set; }
}
public class Car
{
...
  public IEnumerable<CarDriver> CarDrivers { get; set; }
}
public class CarDriver
{
  public int CarId {get;set;}
  public Car CarNavigation {get;set;}
  public int DriverId {get;set;}
  public Driver DriverNavigation {get;set;}
}
```

### Каскадна поведінка

Більшість сховищ даних (наприклад, SQL Server) мають правила, які контролюють поведінку під час видалення рядка. Якщо пов’язані (залежні) записи також потрібно видалити, це називається каскадним видаленням. У EF Core є три дії, які можуть відбуватися, коли видаляється основна сутність (із залежними сутностями, завантаженими в пам’ять).

    Залежні записи видаляються.

    Залежні зовнішні ключі мають значення null.

    Залежна сутність залишається незмінною.

Поведінка за умовчанням для необов’язкових і обов’язкових зв’язків відрізняється. Поведінка також може бути налаштована на одне із семи значень, хоча для використання рекомендовано лише п’ять. Поведінка налаштовується за допомогою переліку DeleteBehavior за допомогою Fluent API. Опції, доступні в переліку, перераховані тут:

    Cascade

    ClientCascade

    ClientNoAction (not recommended for use)

    ClientSetNull

    NoAction (not recommended for use)

    SetNull

    Restrict

У EF Core зазначена поведінка запускається лише після видалення сутності та виклику SaveChanges() для похідного DbContext. Перегляньте розділ «Виконання запиту», щоб дізнатися більше про те, коли EF Core взаємодіє зі сховищем даних.

#### Каскадна поведінка при необов'язкових відносинах (Optional Relationships)

Необов’язкові зв’язки – це те, де залежна сутність може встановити значення зовнішнього ключа на null. Для необов’язкових зв’язків типовою поведінкою є ClientSetNull. У таблиці показано каскадну поведінку залежних сутностей і вплив на записи бази даних під час використання SQL Server.


| Delete Behavior|Вплив на залежних (у пам'яті)| Вплив на залежних (у базі даних)|
| -------- | ---------|-----------|
| Cascade|Сутності видаляються.| Сутності видаляються базою даних.|
| ClientCascade| Сутності видаляються.| Для баз даних, які не підтримують каскадне видалення, EF Core видаляє сутності.|
| ClientSetNull (default)| Для властивості/властивостей зовнішнього ключа встановлено значення null.| Нічого|
|SetNull| Для властивості/властивостей зовнішнього ключа встановлено значення null.|Для властивості/властивостей зовнішнього ключа встановлено значення null.|
|Restrict| Нічого|Нічого|

#### Каскадна поведінка при обов'язкових відносинах (Required Relationships)

Обов’язкові зв’язки – це випадки, коли залежна сутність не може встановити значення(а) зовнішнього ключа на null. Для обов’язкових зв’язків типовою поведінкою є Cascade. У таблиці показано каскадну поведінку залежних сутностей і вплив на записи бази даних під час використання SQL Server.

| Delete Behavior|Вплив на залежних (у пам'яті)| Вплив на залежних (у базі даних)|
| -------- | ---------|-----------|
| Cascade (default)|Сутності видаляються.| Сутності видаляються.|
| ClientCascade| Сутності видаляються.| Для баз даних, які не підтримують каскадне видалення, EF Core видаляє сутності.|
| ClientSetNull| SaveChanges створює виняток.| Нічого|
|SetNull| SaveChanges створює виняток.|SaveChanges створює виняток.|
|Restrict| Нічого|Нічого|


#### Домовленості щодо сутностей

Є багато угод(conventions), які використовує EF Core для визначення сутності та того, як вона пов’язана зі сховищем даних. Конвенції завжди ввімкнено, якщо вони не скасовуються анотаціями даних або кодом у Fluent API. У таблиці наведено деякі з найважливіших угод EF Core.


| Угода|Значення в використані|
| -------- | ---------|
| Включені таблиці| Усі класи з властивістю DbSet і всі класи, доступні (через властивості навігації) за допомогою класу DbSet, створюються в базі даних. |
| Включені колонки | Усі загальнодоступні властивості з геттером і сеттером (включаючи автоматичні властивості) зіставляються зі стовпцями.|
| Назва таблиці | Зіставляється з назвою властивості DbSet у похідному DbContext. Якщо DbSet не існує, використовується ім'я класу.|
|Назва стовпця |Імена стовпців зіставляються з іменами властивостей класу. |
| Тип даних стовпця| Типи даних вибираються на основі типу даних .NET і перекладаються постачальником бази даних (SQL Server). DateTime відображається на datetime2(7), а рядок – на nvarchar(max). Рядки як частина первинного ключа відображаються на nvarchar(450).|
| Nullability стовпців| Типи даних, які не допускають значення null, створюються як стовпці збереження Not Null.|
| Primary key| Властивості з назвою Id або EntityTypeNameId буде налаштовано як первинний ключ. Ключі типу short, int, long або Guid мають значення, які контролюються сховищем даних. Числові значення створюються як стовпці Identity (SQL Server).|
| Relationships | Зв’язки між таблицями створюються, коли між двома класами сутностей є властивості навігації.|
| Foreign key| Властивості під назвою OtherClassNameId є зовнішніми ключами для властивостей навігації типу <OtherClassName>.|
| Схема |Таблиці створюються за типовою схемою сховища даних (dbo на SQL Server). |

У попередніх прикладах властивостей навігації всі використовують конвенції EF Core для побудови зв’язків між таблицями

### Відображення властивостей у стовпці

За домовленістю загальнодоступні read-write властивості відображаються на однойменні стовпці. Тип даних відповідає еквіваленту сховища даних типу даних CLR властивості. Для властивостей, які не допускають значення null, у сховищі даних встановлено значення NOT NULL, а для властивостей, що допускають значення not null (включаючи типи посилань із можливістю значення NULL, представлені в C# 8), встановлено значення NULL.
EF Core також підтримує читання та запис у резервні(backing) поля властивостей. EF Core очікує, що backing поле має бути названо з використанням одного з таких умов (у порядку пріоритету):

    _<camel-cased property name>

    _<property name>

    m_<camel-cased property name>

    m_<property name>

Якщо властивість Color класу Car оновлено для використання резервного поля, йому (за умовами) потрібно буде назвати одне з _color, _Color, m_color або m_Color, ось так:

```cs
private string _color;
public string Color
{
  get => _color;
  set => _color = value;
}
```

### Перевизначення основних угод EF

Угоди можна змінити за допомогою методу ConfigureConventions(). Наприклад, якщо ви хочете, щоб властивості рядка за умовчанням мали певний розмір (замість nvarchar(max)), ви можете додати такий код до класу ApplicationDbContext:

```cs
protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
{
  configurationBuilder.Properties<string>().HaveMaxLength(50);
}
```
Коли буде створено та виконано нову міграцію, ви побачите, що всі властивості типу string оновлено до nvarchar(50).

### Анотації даних Entity Framework

Анотації даних – це атрибути C#, які використовуються для подальшого формування ваших сутностей. Таблиця містить деякі з найбільш часто використовуваних анотацій даних для визначення того, як ваші класи сутності та властивості відображаються в таблицях і полях бази даних. Анотації даних перекривають будь-які суперечливі угоди. Існує багато інших анотацій, які можна використовувати для вдосконалення сутностей у моделі, як ви побачите далі.

Деякі анотації даних, які підтримуються EF Core


| Анотація даних | Значення в використані |
| -------- | --------- |
| Table | Визначає схему та назву таблиці для сутності. |
| EntityTypeConfiguration | У поєднанні з інтерфейсом IEntityTypeConfiguration дозволяє конфігурувати сутність у власному класі за допомогою Fluent API. Використання цього атрибута розглядається в розділі Fluent API.|
| Keyless | Вказує, що сутність не має ключа (наприклад, представлення бази даних). |
| Column | Визначає назву стовпця для властивості. |
| BackingField | Визначає резервне поле C# для властивості. |
| Key | Визначає первинний ключ для сутності. Ключові поля також неявно є [обов’язковими]. |
| Index | Розміщується в класі для визначення індексу одного стовпця або кількох стовпців. Дозволяє вказати унікальний індекс. |
| Owned | Оголошує, що клас буде належати іншому класу сутності. |
| Required | Оголошує властивість у базі даних як NOT NULL. |
| ForeignKey | Оголошує властивість, яка використовується як зовнішній ключ для властивості навігації. |
| InverseProperty | Оголошує властивість навігації на іншому кінці зв’язку. |
| StringLength | Визначає максимальну довжину властивості рядка. |
| TimeStamp | Оголошує тип як версію рядка в SQL Server і додає перевірки паралельності до операцій бази даних із залученням сутності. |
| ConcurrencyCheck | Поле прапорів, яке використовується під час перевірки паралельності під час виконання оновлень і видалень. |
| DatabaseGenerated | Визначає, чи поле створено базою даних, чи ні. Приймає значення DatabaseGeneratedOption Computed, Identity або None. |
| DataType | Забезпечує більш конкретне визначення поля, ніж внутрішній тип даних. |
| Unicode | Зіставляє властивість рядка зі стовпцем бази даних Unicode/non-Unicode без визначення рідного типу даних. |
| Precision | Визначає точність і масштаб для стовпця бази даних без визначення рідного типу даних. |
| NotMapped | Виключає властивість або клас щодо полів і таблиць бази даних. |

У наступному коді показано клас BaseEntity з анотаціями, які оголошують поле Id як первинний ключ. Друга анотація даних у властивості Id вказує на те, що це стовпець Identity у SQL Server. Властивість TimeStamp буде властивістю timestamp/rowversion SQL Server (для перевірки паралельності, розглянутої далі в цьому розділі).

```cs
namespace AutoLot.Samples.Models;

public abstract class BaseEntity
{
    [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Timestamp]
    public byte[] TimeStamp { get; set; }
}
```
Створимо міграцію.
```console
dotnet ef migrations add ChangeBaseEntity
```
Застосуємо міграцію
```console
dotnet ef database update
```
База даних не оновиться.
```console
Cannot alter column 'TimeStamp' to be data type timestamp.
```
Це пояснюється тим, що SQL Server не дозволяє змінювати тип даних існуючого стовпця на тип даних із міткою часу з іншого типу даних. Стовпець потрібно видалити, а потім знову додати з новим типом даних позначки часу. EF Core бачить стовпець як уже існуючий і видає оператор alter, а не парні команди drop/add, які потрібні для внесення змін.

видалимо останю міграцію
```console
dotnet ef migrations remove
```
В класі Закоментуємо властивість TimeStamp у класі BaseEntity

```cs
    //[Timestamp]
    //public byte[] TimeStamp { get; set; }
```
По суті це означає видалити стовпец в усіх таблицях де використовується поле. Створемо мірацію

```console
 dotnet ef migrations add ChangeBaseEntityDropTimeStamp
```
Ця міграція видалить всі ствовпці.

Знімемо коментар з властивості і створимо нову міграцію.
```cs
    [Timestamp]
    public byte[] TimeStamp { get; set; }
```
```console
dotnet ef migrations add ChangeBaseEntityAddTimeStamp
```
Оновимо базу даних.
```console
dotnet ef database update
```
Ці операцію видалюять і створюють знову стовці необхідного нам типу. Це можна подивитись виконавши команду:

```console
dotnet ef migrations script 20241023182843_Add_Drivers --no-transactions
```


Розглянемо клас Car і анотації даних, які формують його в базі даних:

```cs
namespace AutoLot.Samples.Models;
[Table("Inventory",Schema ="dbo")]
[Index(nameof(MakeId),Name = "IX_Inventory_MakeId")]
public class Car : BaseEntity
{
    private string _color;
    [Required,StringLength(50)]
    public string Color { get => _color; set => _color = value; }
    [Required,StringLength(50)]
    public string PetName { get; set; }
    public int MakeId { get; set; }
    [ForeignKey(nameof(MakeId))]
    public Make MakeNavigation { get; set; }
    public Radio RadioNavigation { get; set; }
    [InverseProperty(nameof(Driver.Cars))]
    public IEnumerable<Driver> Drivers { get; set; }
}
```

Атрибут Table зіставляє клас Car з таблицею Inventory у схемі dbo. Атрибут Column (у цьому прикладі не показано) працює подібним чином і використовується для зміни назви стовпця або типу даних. Атрибут Index створює індекс зовнішнього ключа MakeId. Для двох текстових полів (Color і PetName) встановлено значення Required і максимальну StringLength 50 символів. Атрибути InverseProperty та ForeignKey пояснюються в наступному розділі.
Нижче наведено зміни щодо конвенцій EF Core:

    Перейменування таблиці з Cars на Inventory.

    Встановлення нульового значення для стовпців Color і PetName з Null на Not Null.

    Явне встановлення розміру стовпців Color і PetName на nvarchar(50). Це вже було оброблено, коли конвенції EF Core для властивостей рядка були перевизначені, але включені тут для видимості.

    Перейменування індексу стовпця MakeId.

Решта використовуваних анотацій відповідає конфігурації, визначеній угодами EF Core. 

Створимо і застосуємо нову мігацію.

```console
dotnet ef migrations add 20241024103403_ChangeCarAddAnotations
dotnet ef database update 
```

Змінемо сутність Radio

```cs
public class Radio : BaseEntity
{
    public bool HasTweeters { get; set; }
    public bool HasSubWoofers { get; set; }
    [Required,StringLength(50)]
    public string RadioId { get; set; }
    [Column("InventoryId")]
    public int CarId { get; set; }
    [ForeignKey(nameof(CarId))]
    public Car CarNavigation { get; set; }
}

```
Властивість CarId зіставленна з полем під назвою InventoryId. RadioId зроблено обов’язковим і явно встановлено розмір 50.

Створимо і застосуємо нову мігацію та подивимось зміни.

```console
dotnet ef migrations add ChangeRadioAddAnotations
dotnet ef database update 
dotnet ef migrations script 20241024103403_ChangeCarAddAnotations --no-transactions
```

Як останній крок в оновленні наших моделей, оновіть властивість Name в сутності Make так, щоб вона була обов’язковою, а також встановіть максимальну довжину 50 і зробіть те саме для властивостей FirstName і LastName в сутності Driver:

```cs
namespace AutoLot.Samples.Models;
[Table("Makes",Schema ="dbo")]
public class Make : BaseEntity
{
    [Required, StringLength(50)]
    public string Name { get; set; }
    [InverseProperty(nameof(Car.MakeNavigation))]
    public IEnumerable<Car> Cars { get; set; } = new List<Car>();
}
```
```cs
namespace AutoLot.Samples.Models;

public class Driver : BaseEntity
{
    [Required, StringLength(50)]
    public string FirstName { get; set; }
    [Required, StringLength(50)]
    public string LastName { get; set; }
    [InverseProperty(nameof(Car.Drivers))]
    public IEnumerable<Car> Cars { get; set; } = new List<Car>(); 
}
```
```console
dotnet ef migrations add ChangeMakeDriverAddAnotations
dotnet ef database update 
```

#### Анотації та навігаційні властивості

Анотація ForeignKey дозволяє EF Core знати, яка властивість є резервним полем для властивості навігації. Відповідно до домовленості <TypeName>Id буде автоматично встановлено як властивість зовнішнього ключа. У попередніх прикладах він явно встановлений для читабельності. Це підтримує різні стилі іменування, а також наявність кількох зовнішніх ключів до однієї таблиці. Зауважте, що у відносинах «один-до-одного» лише залежна сутність має зовнішній ключ. InverseProperty інформує EF Core про зв’язок між сутностями, вказуючи властивість навігації в іншій сутності, яка повертає до цієї сутності. InverseProperty потрібен, коли сутність пов’язана з іншою сутністю більше одного разу, а також робить код більш читабельним.



### The Fluent API (за допомогою виразів)

Fluent API налаштовує сутності програми за допомогою коду C#. Методи надаються екземпляром ModelBuilder, доступним у методі DbContext OnModelCreating(). Fluent API є найпотужнішим із методів конфігурації та перевизначає будь-які конвенції чи анотації даних, які суперечать. Деякі параметри конфігурації доступні лише за допомогою Fluent API, наприклад налаштування значень за замовчуванням і каскадної поведінки для властивостей навігації.

#### Методи класів і властивостей

Fluent API — це надмножина анотацій даних під час формування ваших окремих сутностей. Він підтримує всі функції, що містяться в анотаціях даних, але має додаткові можливості, такі як визначення складених ключів та індексів і визначення обчислених стовпців.

#### Відображення класів і властивостей

Повернемося в клас ApplicationDbContext і його методу OnModelCreating. 
```cs
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ...

            modelBuilder.Entity<Car>(entity => 
            {
                entity.ToTable("Inventory", "dbo");
            });
        }
```

Цей код показує попередній приклад Car з Fluent API, еквівалентним використаним анотаціям даних що до назви таблиці (без навігаційних властивостей, які будуть розглянуті далі).

```cs
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //...

            modelBuilder.Entity<Radio>(entity => 
            {
                entity.Property(e => e.CarId).HasColumnName("InventoryId");            
            });

        }
```
Властивість CarId класу Radio відповідає стовпцю InventoryId таблиці Radios.

Після запису ціх змін в методі запустимо команду

```console
PS D:\...\AutoLot.Samples> dotnet ef migrations has-pending-model-changes
Build started...
Build succeeded.
No changes have been made to the model since the last migration.
```
Це говорить про те шо ми не зробили змін в моделі данних.


#### Ключі та індекси

Щоб установити первинний ключ для сутності, використовуйте метод HasKey(), як показано тут:

```cs
            modelBuilder.Entity<Car>(entity => 
            {
                entity.ToTable("Invertory", "dbo");
                entity.HasKey(e => e.Id);
            });
```
Щоб установити складений ключ, виберіть властивості ключа у виразі для методу HasKey(). Наприклад, якщо первинним ключем для об’єкта Car мають бути стовпці Id і властивість OrganisationId, його потрібно встановити так:

```cs
            modelBuilder.Entity<Car>(entity => 
            {
                entity.ToTable("Invertory", "dbo");
                entity.HasKey(e => new { e.Id, e.OrganizationId });
            });
```

Процес створення індексів такий самий, за винятком того, що він використовує метод Fluent API HasIndex(). Наприклад, щоб створити індекс із назвою IX_Inventory_MakeId, використовуйте такий код:

```cs
            modelBuilder.Entity<Car>(entity => 
            {
                entity.ToTable("Invertory", "dbo");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.MakeId, "IX_Inventory_MakeId");
            });
```
Щоб зробити індекс унікальним, використовуйте метод IsUnique(). Метод IsUnique() приймає необов’язковий bool, який за замовчуванням має значення true:

```cs
entity.HasIndex(e => e.MakeId, "IX_Inventory_MakeId").IsUnique();

```

#### Розмір поля та можливість NULL

Властивості налаштовуються шляхом їх вибору за допомогою методу Property(), а потім за допомогою додаткових методів для налаштування властивості. Ви вже бачили приклад зіставлення властивості CarId зі стовпцем InventoryId.

```cs
            modelBuilder.Entity<Car>(entity => 
            {
                entity.ToTable("Invertory", "dbo");
                entity.HasKey(e => e.Id);
                //entity.HasKey(e => new { e.Id, e.OrganizationId });
                //entity.HasIndex(e => e.MakeId, "IX_Inventory_MakeId").IsUnique();
                entity.Property(e => e.Color)
                .IsRequired()
                .HasMaxLength(50);
                entity.Property(e => e.PetName)
                .IsRequired()
                .HasMaxLength(50);
            });
```
IsRequired() приймає необов’язковий bool, який за замовчуванням має значення true та визначає NULL стовпець бази даних. Метод HasMaxLength() встановлює розмір стовпця.

#### Значення за замовчуванням

Fluent API надає методи встановлення значень за замовчуванням для стовпців. Значенням за замовчуванням може бути тип значення або рядок SQL. Наприклад, щоб встановити колір за замовчуванням для нового автомобіля на чорний, скористайтеся наступним:

```cs

            modelBuilder.Entity<Car>(entity => 
            {
                // ...

                entity.Property(e => e.Color)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Black");

                // ...
            });
```
Щоб встановити значення для функції бази даних (наприклад, getdate()), використовуйте метод HasDefaultValueSql(). Припустімо, що властивість DateTime під назвою DateBuilt додано до класу Car.

```cs
public class Car : BaseEntity
{

    //...

    public DateTime? DateBuilt { get; set; }
}

```
Наприклад за замовчуванням має бути поточна дата за допомогою методу SQL Server getdate(). Щоб налаштувати цю властивість за умовчанням, використовуйте такий код Fluent API:

```cs
            modelBuilder.Entity<Car>(entity => 
            {
                //...
                .HasDefaultValueSql("getdate()");
            });
```
SQL Server використовуватиме результат функції getdate(), якщо властивість DateBuilt сутності не має значення під час збереження в базі даних.

Створимо і застосуємо нову мігацію.

```console
dotnet ef migrations add ChangeCarAddDateBuilt
dotnet ef database update 
```

#### Проблеми з значеннями за замовчуванням

Проблема виникає, коли логічне або числове значення має значення бази даних за замовчуванням, яке суперечить значенню за замовчуванням CLR. Наприклад, якщо для логічної властивості (наприклад, IsDrivable) у базі даних за замовчуванням встановлено значення true, база даних установить значення true під час вставлення запису, якщо значення не вказано для цього стовпця. Це, звісно, ​​очікувана поведінка з боку бази даних. Однак значення CLR за замовчуванням для логічних властивостей є false, що спричиняє проблему через те, як EF Core обробляє значення за замовчуванням. 

Наприклад, нехай є властивість Bool під назвою IsDrivable до класу Car. 

```cs
public class Car : BaseEntity
{
   //...
    public bool IsDrivable { get; set; }
}
```

Давайте розглянемо поведінку EF Core для типу даних Boolean.

Нижче код, щоб створити новий запис про автомобіль із значенням false для IsDrivable:

```cs
context.Cars.Add(new() { MakeId = 1, Color = 'Rust', PetName = 'Lemon', IsDrivable = false });
context.SaveChanges();
```

В результаті буде згенерований SQL для вставки:

```sql
INSERT INTO [dbo].[Inventory] ([Color], [IsDrivable], [MakeId], [PetName], [Price])
VALUES (@p0, @p1, @p2, @p3, @p4);
SELECT [Id], [DateBuilt], [Display], [IsDeleted], [TimeStamp], [ValidFrom], [ValidTo]
FROM [dbo].[Inventory]
WHERE @@ROWCOUNT = 1 AND [Id] = scope_identity();
',N'@p0 nvarchar(50),@p1 bit,@p2 int,@p3 nvarchar(50),@p4 decimal(18,2)',@p0=N'Rust',@p1=0,@p2=1,@p3=N'Lemon',@p4=NULL
```
Тепер встановіть значення за замовчуванням для відображення стовпця властивості у методі OnModelCreating() ApplicationDbContext (знову створюючи та застосовуючи нову міграцію бази даних):

```cs

            modelBuilder.Entity<Car>(entity => 
            {
               //...
                entity.Property(e => e.IsDrivable)
                .HasDefaultValue(true);
            });
```
Виконання того самого коду для вставки попереднього запису Car генерує інший SQL:

```sql
INSERT INTO [dbo].[Inventory] ([Color], [MakeId], [PetName], [Price])
VALUES (@p0, @p1, @p2, @p3);
SELECT [Id], [DateBuilt], [Display], [IsDeleted], [IsDrivable], [TimeStamp], [ValidFrom], [ValidTo]
FROM [dbo].[Inventory]
WHERE @@ROWCOUNT = 1 AND [Id] = scope_identity();
',N'@p0 nvarchar(50),@p1 int,@p2 nvarchar(50),@p3 decimal(18,2)',@p0=N'Rust',@p1=1,@p2=N'Lemon',@p3=NULL
```

Зверніть увагу, що стовпець IsDrivable не включено в оператор вставки. EF Core знає, що значенням властивості IsDrivable є значення за замовчуванням CLR, і він знає, що стовпець має значення за замовчуванням SQL Server, тому стовпець не включено в інструкцію. Таким чином, коли ви зберігаєте новий запис із IsDrivable = false, значення ігнорується, і використовуватиметься стандартне значення бази даних. Це означає, що значення для IsDrivable завжди буде true!
EF Core попереджає вас про цю проблему, коли ви створюєте міграцію. У цьому конкретному прикладі виводиться це попередження

```console
The 'bool' property 'IsDrivable' on entity type 'Car' is configured with a database-generated default. This default will always be used for inserts when the property has the value 'false', since this is the CLR default for the 'bool' type. Consider using the nullable 'bool?' type instead, so that the default will only be used for inserts when the property value is 'null'. 
```
Одне з рішень для цього полягає в тому, щоб зробити вашу загальнодоступну властивість (і, отже, стовпець) null, оскільки значення за замовчуванням для типу значення nullable дорівнює null, тому встановлення властивості Boolean на false працює належним чином. Однак зміна нульовості властивості може не відповідати потребам бізнесу.

Інше рішення забезпечує EF Core і його підтримка резервних( backing) полів. Пам’ятайте раніше, якщо резервне поле існує (і ідентифікується як резервне поле для властивості через угоду, анотацію даних або Fluent API), то EF Core використовуватиме резервне поле для дій читання-запису, а не загальнодоступну властивість.
Якщо ви оновите IsDrivable, щоб використовувати резервне поле з можливістю нульового значення (але збережете властивість не нульовим), EF Core читатиме та записуватиме з поля резервної підтримки, а не властивості.Значення за замовчуванням для nullable bool є null, а не false.

```cs
public class Car : BaseEntity
{

    //...

    private bool? _IsDivable;
    public bool IsDrivable
    {
        get => _IsDivable ?? true;
        set => _IsDivable = value;
    }
}
```

Тепер завдяки цій зміні властивість працює належним чином.

API Fluent використовується для інформування EF Core про резервне поле.

```cs

            modelBuilder.Entity<Car>(entity => 
            {
                //...
                entity.Property(e => e.IsDrivable)
                .HasField("_IsDrivable")
                .HasDefaultValue(true);
            });
```
У цьому прикладі метод HasField() не потрібний, оскільки ім’я резервного поля відповідає умовам іменування. Він додано, щоб показати, як використовувати Fluent API, щоб налаштувати його та зберегти код читабельним.

EF Core перетворює поле на таке визначення SQL:

```sql
CREATE TABLE [dbo].[Inventory](
...
  [IsDrivable] [BIT] NOT NULL,
...
GO
ALTER TABLE [dbo].[Inventory] ADD  DEFAULT (CONVERT([BIT],(1))) FOR [IsDrivable]
GO
```
Хоча це не показано в попередніх прикладах, числові властивості працюють так само. Якщо ви встановлюєте ненульове значення за замовчуванням, резервне поле (або сама властивість, якщо резервне поле не використовується) має бути nullable.

Якшо, попередження все одно з’являтиметься, навіть якщо поля належним чином налаштовано з резервними полями з можливістю нульових значень. Попередження можна придушити; однак я рекомендую залишити його на місці як нагадування перевірити, чи правильно налаштовано поле/властивість. Якщо ви хочете придушити його, установіть такий параметр у DbContextOptions:

```cs
options.ConfigureWarnings(wc => wc.Ignore(RelationalEventId.BoolWithDefaultWarning));
```

Таким чином в наш проект додамо 

```cs
public class Car : BaseEntity
{

    //..
    public bool? IsDrivable { get; set; }

}

```
```cs
        modelBuilder.Entity<Car>(entity =>
        {
            //...
            entity.Property(e => e.IsDrivable)
            .HasDefaultValue(true);
        });
```
Створимо і застосуємо нову мігацію.

```console
dotnet ef migrations add ChangeCarAddIsDrivable
dotnet ef database update 
```

#### Токени RowVersion/Concurrency

Щоб установити властивість як тип даних rowversion, використовуйте метод IsRowVersion(). Щоб також встановити властивість як маркер паралельності, скористайтеся методом IsConcurrencyToken(). Поєднання цих двох методів має той самий ефект, що й анотація даних [Timestamp]:

```cs
            modelBuilder.Entity<Car>(entity => 
            {
                // ...
                entity.Property(e => e.TimeStamp)
                .IsRowVersion()
                .IsConcurrencyToken();
            });
```

#### SQL Server Sparse(pозріджені) стовпці

Розріджені стовпці SQL Server оптимізовано для зберігання нульових значень. EF Core розріджені стовпці за допомогою методу IsSparse() у Fluent API. Наступний код ілюструє налаштування фіктивної властивості IsRaceCar для використання розріджених стовпців SQl Server:

```cs
modelBuilder.Entity<Car>(entity =>
{
  entity.Property(p => p.IsRaceCare).IsSparse();
});
```

#### Обчислені стовпці

Стовпці також можна встановити як обчислені на основі можливостей сховища даних. Для SQL Server двома варіантами є обчислення значення на основі значення інших полів у тому самому записі або використання скалярної функції. Наприклад, щоб створити обчислений стовпець у таблиці Inventory, який поєднує значення PetName і Color для створення властивості під назвою Display, скористайтеся функцією HasComputedColumnSql().

```cs
public class Car : BaseEntity
{
   //...
    public string Display { get; set; }
}
```
Потім додайте Fluent API виклик HasComputedColumnSql():

```cs
              modelBuilder.Entity<Car>(entity => 
            {
                // ...          

                entity.Property(e => e.Display)
                .HasComputedColumnSql("[PetName] + ' (' + [Color] + ')'");
            });
```
Обчислені значення, представлені в EF Core 5, можна зберігати, тому значення обчислюється лише під час створення або оновлення рядка. Хоча SQL Server підтримує це, не всі сховища даних підтримують, тому перевірте документацію свого постачальника бази даних.

Анотація даних DatabaseGenerated часто використовується в поєднанні з API Fluent для покращення читабельності коду. Ось оновлена ​​версія властивості Display із включеною анотацією:

```cs
public class Car : BaseEntity
{
   //...
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public string Display { get; set; }
}
```
```console
dotnet ef migrations add ChangeCarAddDisplay
dotnet ef database update
```


#### Перевірка обмежень

Перевірка обмежень — це функція SQL Server, яка визначає умову для рядка, яка має бути істинною. Наприклад, у системі електронної комерції можна додати обмеження перевірки, щоб переконатися, що кількість є більшою за нуль або що ціна вища за ціну зі знижкою. Ми створимо надумане обмеження, яке запобігає використанню назви «Lemon» у таблиці Makes.
Додайте наступне до методу OnModelCreating() у класі ApplicationDbContext, який створює обмеження перевірки, що запобігає назві «Lemon» у таблиці Makes

```cs

        modelBuilder.Entity<Make>()
            .ToTable(t => t.HasCheckConstraint("CH_Name", "[Name]<>'Lemon'"));

```
Перший параметр дає обмеженню назву в моделі, другий — це SQL для обмеження, а останній призначає ім’я SQL Server для обмеження перевірки. 

```console
 dotnet ef migrations add ChangeMakeAddCheckConstrains
 dotnet ef database update
 dotnet ef migrations script --no-transactions
```
Ось обмеження перевірки, як визначено в SQL:

```sql
ALTER TABLE [dbo].[Makes] ADD CONSTRAINT [CH_Name] CHECK ([Name]<>'Lemon');
GO
```
Тепер, коли до таблиці додається запис із іменем «Lemon», буде створено виняток SQL. Виконайте наступний код, щоб побачити виняток у дії:

```cs
var context = new ApplicationDbContextFactory().CreateDbContext(null);
context.Makes.Add(new Make { Name = 'Lemon' });
context.SaveChanges();
```
Це викликає такий виняток:

```console
The INSERT statement conflicted with the CHECK constraint 'CK_Check_Name'. The conflict occurred in database 'AutoLotSamples', table 'dbo.Makes', column 'Name'.
```
Можете скасувати міграцію для перевірочного обмеження та видалити міграцію, оскільки далі не використовується перевірочне обмеження. Його було додано в цей розділ з метою демонстрації.

```console
PS D:\...\AutoLot.Samples> dotnet ef migrations list
Build started...
Build succeeded.
20241023150501_Initial_CreateDB_Add_Makes_Cars
20241023155227_Add_Radios
20241023182843_Add_Drivers
20241024090325_ChangeBaseEntityDropTimeStamp
20241024091404_ChangeBaseEntityAddTimeStamp
20241024103403_ChangeCarAddAnotations
20241024105231_ChangeRadioAddAnotations
20241024113639_ChangeMakeDriverAddAnotations
20241024162310_ChangeCarAddDateBuilt
20241025083915_ChangeCarAddIsDrivable
20241025085019_ChangeCarAddDisplay
20241025093921_ChangeMakeAddCheckConstrains
 PS D:\...\AutoLot.Samples\AutoLot.Samples> dotnet ef database update 20241025085019_ChangeCarAddDisplay
Build started...
Build succeeded.
Reverting migration '20241025093921_ChangeMakeAddCheckConstrains'.
PS D:\...\AutoLot.Samples> dotnet ef migrations remove
Build started...
Build succeeded.
Removing migration '20241025093921_ChangeMakeAddCheckConstrains'.
Reverting the model snapshot.
Done.
PS D:\...\AutoLot.Samples> dotnet ef migrations list
```

#### Відносини «One-to-Many».

Щоб використовувати Fluent API для визначення зв’язків «One-to-Many», виберіть one з сутностей для оновлення.
Обидві сторони ланцюга навігації встановлюються в одному блоці коду.

```cs
            modelBuilder.Entity<Car>(entity => 
            {

                //...
                entity.HasOne(c => c.MakeNavigation)
                .WithMany(m => m.Cars)
                .HasForeignKey(c => c.MakeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventory_Makes_MakeId");
            });
```

Якщо вибрати основну сутність як основу для конфігурації властивості навігації, код виглядатиме так:

```cs

            modelBuilder.Entity<Make>(entity =>
            {
                entity.HasMany(m => m.Cars)
                .WithOne(c=>c.MakeNavigation)
                .HasForeignKey(c => c.MakeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventory_Makes_MakeId");
            });
```
Збереженя класів показує зміну моделі даних.

```console
 dotnet ef migrations has-pending-model-changes
 dotnet ef migrations add ChangeRelationshipCarsMakes
 dotnet ef migrations script
 dotnet ef database update
 ```



#### Відносини «One-to-One».

Зв’язки «один-до-одного» налаштовуються так само, за винятком того, що замість WithMany() використовується метод Fluent API WithOne(). Крім того, для залежної сутності потрібен унікальний індекс, який буде створено автоматично, якщо його не визначено. У наступному прикладі явно створюється унікальний індекс для визначення імені. Ось код зв’язку між об’єктами Car і Radio за допомогою залежного об’єкта (Radio)

```cs
            modelBuilder.Entity<Radio>(entity => 
            {
                //...

                entity.HasIndex(e => e.CarId, "IX_Radios_InventoryId");

                entity.HasOne(r => r.CarNavigation)
                .WithOne(c => c.RadioNavigation)
                .HasForeignKey<Radio>(r => r.CarId);
            });
```
Якщо зв’язок визначено для головної сутності, до залежної сутності все одно буде додано унікальний індекс. Ось код зв’язку між об’єктами Car і Radio, у якому використовується основний об’єкт для зв’язку та вказується ім’я індексу залежного об’єкта:

```cs
           modelBuilder.Entity<Radio>(entity => 
           {
                // ...

               entity.HasIndex(r => r.CarId, "IX_Radios_InventoryId")
               .IsUnique();
           });           
```

Ці зміни не змінюють нашу модель.

```console
PS D:\...\AutoLot.Samples> dotnet ef migrations has-pending-model-changes
Build started...
Build succeeded.
No changes have been made to the model since the last migration.
```

#### Відносини «Many-to-Many»

Зв’язки «Many-to-Many» набагато краще налаштовуються за допомогою Fluent API. Імена полів зовнішнього ключа, імена індексів і каскадна поведінка можуть бути встановлені в операторах, які визначають зв’язок. Це також дозволяє безпосередньо вказувати зведену таблицю, що дозволяє додавати додаткові поля та спрощувати запити.

Додамо сутність CarDriver:

```cs
namespace AutoLot.Samples.Models;

[Table("InventoryToDrivers",Schema ="dbo")]
public class CarDriver : BaseEntity
{
    public int DriverId { get; set; }
    [ForeignKey(nameof(DriverId))]
    public Driver DriverNavigation { get; set; }
    [Column("InventoryId")]
    public int CarId { get; set; }
    [ForeignKey(nameof(CarId))]
    public Car CarNavigation { get; set; }
}
```
Додайте DbSet<T> для нової сутності в ApplicationDbContext

```cs
        public DbSet<CarDriver> CarsToDrivers { get; set; }
```

Потім оновіть сутність Car, щоб додати властивість навігації для нової сутності CarDriver:

```cs
public class Car : BaseEntity
{
   //...
    [InverseProperty(nameof(CarDriver.CarNavigation))]
    public IEnumerable<CarDriver> CarDrivers { get; set; } = new List<CarDriver>();

}
```

Тепер оновіть сутність Driver для властивості навігації до сутності CarDriver:

```cs
public class Driver : BaseEntity
{

    //...
    [InverseProperty(nameof(CarDriver.DriverNavigation))]
    public IEnumerable<CarDriver> CarDrivers { get; set; } = new List<CarDriver>();
}
```
Нарешті, додайте код API Fluent для зв’язку «Many-to-Many»:

```cs
modelBuilder.Entity<Car>()
  .HasMany(p => p.Drivers)
  .WithMany(p => p.Cars)
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
```console
dotnet ef migrations add ChangeCarDriverRelationships
dotnet ef migrations script 20241025121451_ChangeRelationshipCarsMakes --no-transactions
dotnet ef database update 
```

#### Виключення сутностей із міграцій

Якщо сутність спільно використовується кількома DbContexts, кожен DbContext створить код у файлах міграції для створення або зміни цієї сутності. Це спричиняє проблему, оскільки другий сценарій міграції не вдасться виконати, якщо зміни вже є в базі даних. 
DbContext може позначити сутність як виключену з міграцій, дозволяючи іншому DbContext стати системою запису для цієї сутності. Наступний код показує, що сутність виключена з міграцій:

```cs
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
  modelBuilder.Entity<LogEntry>().ToTable('Logs', t => t.ExcludeFromMigrations());
}
```

#### Використання класів IEntityTypeConfiguration

Як ви могли здогадатися на цьому етапі роботи з Fluent API, метод OnModelCreating() може стати досить довгим (і громіздким), чим складнішою стає ваша модель. Інтерфейс IEntityTypeConfiguration і атрибут EntityTypeConfiguration дозволяють перемістити конфігурацію Fluent API для сутності у власний клас. Це робить більш чистим ApplicationDbContext і підтримує принцип проектування поділу проблем.
Почніть із створення нового каталогу під назвою Configuration у каталозі Models. У цьому новому каталозі додайте новий клас під назвою CarConfiguration.cs, зробіть його загальнодоступним і реалізуйте інтерфейс IEntityTypeConfiguration<Car>:

```cs
namespace AutoLot.Samples.Models.Configuration;

public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        
    }
}
```
Потім перемістіть вміст конфігурації для сутності Car з методу OnModelCreating() у ApplicationDbContext у метод Configure() класу CarConfiguration. Замініть змінну entity на змінну builder, щоб метод Configure() виглядав так:
```cs

namespace AutoLot.Samples.Models.Configuration;

public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.ToTable("Inventory", "dbo");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.MakeId, "IX_Inventory_MakeId");
        builder.Property(e => e.Color)
          .IsRequired()
          .HasMaxLength(50)
          .HasDefaultValue("Black");
        builder.Property(e => e.PetName)
          .IsRequired()
          .HasMaxLength(50);
        builder.Property(e => e.DateBuild).HasDefaultValueSql("getdate()");
        builder.Property(e => e.IsDrivable)
          .HasField("_isDrivable")
          .HasDefaultValue(true);
        builder.Property(e => e.TimeStamp)
          .IsRowVersion()
          .IsConcurrencyToken();
        builder.Property(e => e.Display).HasComputedColumnSql("[PetName] + '(' + [Color] + ')'", stored: true);
        builder.HasOne(d => d.MakeNavigation)
          .WithMany(p => p.Cars)
          .HasForeignKey(d => d.MakeId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("FK_Inventory_Makes_MakeId");
    }
}
```

Ця конфігурація також працює з плавною конфігурацією «Many-to-Many» між Car і Driver. Додавати конфігурацію до класу CarConfiguration або створювати клас DriverConfiguration ви самі вибираєте. Для цього прикладу перемістіть його в клас CarConfiguration у кінці методу Configure():

```cs
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        //...
        builder
        .HasMany(p => p.Drivers)
        .WithMany(p => p.Cars)
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
    }

```
Оновіть файл GlobalUsings.cs, щоб включити новий простір імен для класів конфігурації:

```cs
global using AutoLot.Samples.Models.Configuration;
```
Замініть увесь код у методі OnModelBuilding() (у класі ApplicationDbContext.cs), який налаштовує клас Car і зв’язок Car to Driver many-to many, на такий єдиний рядок коду:

```cs
new CarConfiguration().Configure(modelBuilder.Entity<Car>());
```

Останнім кроком для класу Car є додавання атрибута EntityTypeConfiguration:

```cs
[EntityTypeConfiguration(typeof(CarConfiguration))]
public class Car : BaseEntity
{
    //...
}    
```
Перевіримо чи змінилась модель.
```console
PS D:\...\AutoLot.Samples> dotnet ef migrations has-pending-model-changes
Build started...
Build succeeded.
No changes have been made to the model since the last migration.
```

Потім повторіть ті самі дії для коду API Radio Fluent. Створіть новий клас під назвою RadioConfiguration, реалізуйте інтерфейс IEntityTypeConfiguration<Radio> і додайте код із методу ApplicationDbContext OnModelBuilding():

```cs
namespace AutoLot.Samples.Models.Configuration;

internal class RadioConfiguration : IEntityTypeConfiguration<Radio>
{
    public void Configure(EntityTypeBuilder<Radio> builder)
    {
        builder.Property(e => e.CarId).HasColumnName("InventoryId");
        builder.HasIndex(e => e.CarId, "IX_Radios_CarId").IsUnique();
        builder.HasOne(d => d.CarNavigation)
          .WithOne(p => p.RadioNavigation)
          .HasForeignKey<Radio>(d => d.CarId);
    }
}
```

Оновіть метод OnModelCreating() у ApplicationDbContext:

```cs
            new RadioConfiguration().Configure(modelBuilder.Entity<Radio>());
```
Додайте атрибут EntityTypeConfiguration до класу Radio

```cs
[EntityTypeConfiguration(typeof(RadioConfiguration))]
public class Radio : BaseEntity
{
    //...
}    
```
Хоча це не зменшило загальну кількість рядків коду, ця нова функція зробила ApplicationDbContext набагато чистішим.

#### Умовні позначення, анотації та Fluent API, боже!

На цьому етапі вам може бути цікаво, який із трьох варіантів використовувати для формування ваших сутностей і їхнього зв’язку між собою та сховищем даних. Відповідь - усі три. Умовні угоди завжди активні (якщо ви не заміните їх анотаціями даних або Fluent API). Анотації даних можуть робити майже все, що можуть робити методи API Fluent, і самі зберігати інформацію в класі сутності, що може підвищити читабельність коду та підтримку. Fluent API є найпотужнішим з усіх трьох. Незалежно від того, використовуєте ви анотації даних чи Fluent API, знайте, що анотації даних переважають над вбудованими угодами, а методи Fluent API переважають над усім.


#### Типи сутностей Owned(У власності)

Іноді дві або більше сутності будуть містити однаковий набір властивостей. Можна використовувати клас C# як властивість сутності для визначення набору властивостей для іншої сутності. Коли типи, позначені атрибутом [Owned] (або налаштовані за допомогою Fluent API), додаються як властивість сутності, EF Core додасть усі властивості з класу сутності [Owned] до сутності-власника. Це збільшує можливість повторного використання коду C#.
За лаштунками EF Core вважає це відношення один до одного. Клас у власності є залежною сутністю, а клас що володіє є головною сутністю. Клас, який є у власносі, навіть якщо він вважається сутністю, не може існувати без сутності-власника. Назви стовпців за замовчуванням із типу власності будуть відформатовані як NavigationPropertyName_OwnedEntityPropertyName (наприклад, PersonalNavigation_FirstName). Назви за замовчуванням можна змінити за допомогою Fluent API.

Додамо клас Person:

```cs

namespace AutoLot.Samples.Models;
[Owned]
public class Person
{
    [Required,StringLength(50)]
    public string FirstName { get; set; }
    [Required, StringLength(50)]
    public string LastName { get; set; }
}
```
Зверніть увагу на атрибут Owned.
За допомогою цього ми можемо замінити властивості FirstName і LastName у класі Driver на новий клас Person:

```cs
public class Driver : BaseEntity
{
    public Person PersonInfo { get; set; } = new Person();
    public IEnumerable<Car> Cars { get; set; } = new List<Car>();
    [InverseProperty(nameof(CarDriver.DriverNavigation))]
    public IEnumerable<CarDriver> CarDrivers { get; set; } = new List<CarDriver>();
}
```
За замовчуванням дві властивості Person зіставляються зі стовпцями з іменами PersonInfo_FirstName та PersonInfo_LastName. Щоб змінити це, спочатку додайте новий файл DriverConfiguration.cs у папку Configuration і оновіть код до наступного:

```cs
namespace AutoLot.Samples.Models.Configuration;

public class DriverConfiguration : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.OwnsOne(o => o.PersonInfo,
            pd =>
            {
                pd.Property<string>(nameof(Person.FirstName))
                       .HasColumnName(nameof(Person.FirstName))
                       .HasColumnType("nvarchar(50)");
                pd.Property<string>(nameof(Person.LastName))
                       .HasColumnName(nameof(Person.LastName))
                       .HasColumnType("nvarchar(50)");
            });
    }
}

```
Оновіть метод ApplicationDbContext OnConfiguring():
```cs
            new DriverConfiguration().Configure(modelBuilder.Entity<Driver>());
```
Оновіть клас Driver:

```cs
[EntityTypeConfiguration(typeof(DriverConfiguration))]
public class Driver : BaseEntity
{
    //...
}
```
Якшо проект створювався з вимкненням тпів нульових посилань C#, тоді таблиця Driver оновлюється таким чином (зауважте, що nullability стовпці FirstName та LastName не відповідають анотаціям Required в сутності що у власності).

```sql
CREATE TABLE [dbo].[Drivers](
        [Id] [INT] IDENTITY(1,1) NOT NULL,
        [FirstName] [NVARCHAR](50) NULL,
        [LastName] [NVARCHAR](50) NULL,
        [TimeStamp] [TIMESTAMP] NULL,
 CONSTRAINT [PK_Drivers] PRIMARY KEY CLUSTERED
(
        [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
```
Хоча клас Person має анотацію Required для обох своїх властивостей, обидва стовпці SQL Server мають значення NULL. Це пов’язано з проблемою того, як система міграції перекладає сутності власності, коли вони використовуються з необов’язковим зв’язком. Щоб виправити це, є кілька варіантів. Перший — увімкнути типи нульових посилань C# (на рівні проекту або в класах). Це робить навігаційну властивість PersonInfo не допускаючою значення, що враховується EF Core, і, у свою чергу, EF Core потім належним чином налаштовує стовпці у сутності, що належить. Інший варіант полягає в додаванні оператора Fluent API, щоб зробити властивість навігації обов’язковою.
```cs
public class DriverConfiguration : IEntityTypeConfiguration<Driver>
{
  public void Configure(EntityTypeBuilder<Driver> builder)
  {
    //...
    builder.Navigation(d=>d.PersonInfo).IsRequired(true);
  }
}
```
Це оновлює властивості типу, що належить Person, для встановлення ненульового стовпця в SQL Server:

```sql
CREATE TABLE [dbo].[Drivers](
      [Id] [INT] IDENTITY(1,1) NOT NULL,
      [FirstName] [NVARCHAR](50) NOT NULL,
      [LastName] [NVARCHAR](50) NOT NULL,
      [TimeStamp] [TIMESTAMP] NULL,
 CONSTRAINT [PK_Drivers] PRIMARY KEY CLUSTERED
(
      [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF,
      IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON,
      OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
```
Існує чотири обмеження на використання типів, що належать:

    Ви не можете створити DbSet<T> для owned типу.

    Ви не можете викликати Entity<T>() із власним типом у ModelBuilder.

    Екземпляри одного типу сутності не можуть бути спільними для кількох власників.

    Типи owned не можуть мати ієрархії успадкування.

Існують додаткові параметри, які можна досліджувати за допомогою об’єктів власності, зокрема колекції, поділ таблиці та вкладення. Щоб отримати більше інформації, зверніться до документації EF Core.

#### Типи запит

Типи запит — це колекції DbSet<T>(наприклад DbSet<Car>), які використовуються для відображення представлень(views), оператора SQL або таблиць без первинного ключа. У попередніх версіях EF Core для цього використовувався DbQuery<T>, але, починаючи з EF Core 3.x, тип DbQuery припинено. Типи запитів додаються до похідного DbContext за допомогою властивостей DbSet<T> і налаштовуються як безключові.
Типи запитів зазвичай використовуються для подання комбінацій таблиць, наприклад для поєднання деталей із таблиць Make та Invertory.
Розглянемо, наприклад, цей запит:

```sql
SELECT m.Id MakeId, m.Name Make, i.Id CarId, i.IsDrivable, 
i.Display, i.DateBuilt, i.Color, i.PetName
FROM dbo.Makes m
INNER JOIN dbo.Inventory i ON i.MakeId = m.Id

```
Щоб зберегти результати цього запиту, створіть нову папку під назвою ViewModels і в цій папці створіть новий клас під назвою CarMakeViewModel:

```cs
namespace AutoLot.Samples.ViewModels;
[Keyless]
public class CarMakeViewModel
{
    public int MakeId { get; set; }
    public string Make { get; set; }
    public int CarId { get; set; }
    public bool IsDrivable { get; set; }
    public string Display { get; set; }
    public DateTime DateBuild { get; set; }
    public string Color { get; set; }
    public string PetName { get; set; }
    [NotMapped]
    public string FullDetail => $"The {Color} {Make} is named {PetName}";
    public override string? ToString() => FullDetail;
}
```
Атрибут Keyless вказує EF Core, що ця сутність є типом запит та ніколи не використовуватиметься для оновлень, а також має бути виключена з засобу відстеження змін під час запиту. Зверніть увагу на використання атрибута NotMapped для створення відображуваного рядка, який об’єднує кілька властивостей в єдиний, зрозумілий людині рядок. Оновіть ApplicationDbContext, щоб включити DbSet<T> для моделі перегляду:

```cs
    public class ApplicationDbContext : DbContext
    {
        // Properies
        //...
        public DbSet<CarMakeViewModel> CarMakeViewModels { get; set; }
    }

```

Решта конфігурації виконується у Fluent API. Створіть нову папку під назвою Configuration у папці ViewModels і в цій папці створіть новий клас під назвою CarMakeViewModelConfiguration та оновіть код до такого:

```cs
namespace AutoLot.Samples.ViewModels.Configuration;

public class CarMakeViewModelConfiguration : IEntityTypeConfiguration<CarMakeViewModel>
{
    public void Configure(EntityTypeBuilder<CarMakeViewModel> builder)
    {
        
    }
}
```
Оновіть файл GlobalUsings.cs, щоб включити новий простір імен для моделі перегляду та конфігурації :

```cs
global using AutoLot.Samples.ViewModels.Configuration;
```
Оновіть клас CarMakeViewModel, щоб додати атрибут EntityTypeConfiguration:

```cs
[EntityTypeConfiguration(typeof(CarMakeViewModelConfiguration))]
public class CarMakeViewModel
{
    //...
}

```
Оновіть метод OnModelCreating() до наступного:
```cs
new CarMakeViewModelConfiguration().Configure(modelBuilder.Entity<CarMakeViewModel>());
```
Змінемо метод Configure:

```cs
public class CarMakeViewModelConfiguration : IEntityTypeConfiguration<CarMakeViewModel>
{
    public void Configure(EntityTypeBuilder<CarMakeViewModel> builder)
    {
        builder.HasNoKey().ToSqlQuery(@"
                SELECT m.Id MakeId, m.Name Make, i.Id CarId, i.IsDrivable, 
                   i.DisplayName, i.DateBuild, i.Color, i.PetName
                FROM dbo.Makes m 
                INNER JOIN dbo.Inventory i ON i.MakeId = m.Id");
    }
}

```
У наведеному прикладі об’єкт встановлюється як безключовий і відображає тип запиту на сирий запит SQL. Метод HasNoKey() Fluent API не є необхідним, якщо в моделі є анотація даних Keyless, і навпаки, але він показаний у цьому прикладі для повноти.




Типи запитів також можуть бути зіставлені з представленням бази даних. Якщо припустити, що є представлення з назвою dbo.CarMakeView, конфігурація виглядатиме так:

```cs
builder.HasNoKey().ToView('CarMakeView', 'dbo');
```
Під час використання міграцій EF Core для оновлення бази даних типи запитів, зіставлені з представленням даних, не створюються як таблиці. Типи запитів, які не зіставляються з представленнями, створюються як безключові таблиці.

Якщо ви не хочете, щоб модель перегляду зіставлялася з таблицею у вашій базі даних і не маєте представлення для зіставлення, скористайтеся таким перевантаженням методу ToTable(), щоб виключити елемент із міграцій:
```cs
    public void Configure(EntityTypeBuilder<CarMakeViewModel> builder)
    {
        //...
        builder.ToTable(x => x.ExcludeFromMigrations());
    }
```
```console
dotnet ef migrations has-pending-model-changes
...
No changes have been made to the model since the last migration.
```

Механізмами, з якими можна використовувати типи запитів, є методи FromSqlRaw() і FromSqlInterpolated(). Вони будуть детально розглянуті в наступному розділі, але ось короткий огляд:

```cs
var records = context.CarMakeViewModel.FromSqlRaw(
    @" SELECT m.Id MakeId, m.Name Make, i.Id CarId, i.IsDrivable,
        i.Display, i.DateBuilt, i.Color, i.PetName
        FROM dbo.Makes m
        INNER JOIN dbo.Inventory i ON i.MakeId = m.Id ");
```

#### Гнучке відображення Query/Table

У EF Core є можливість зіставляти один і той самий клас із кількома об’єктами бази даних. Ці об’єкти можуть бути таблицями, представленнями чи функціями. Наприклад клас CarViewModel із попередного розділу можна зіставити з представленням, яке повертає назву марки з даними автомобіля та таблицею Inventory. Потім EF Core надсилатиме запити з подання та надсилатиме оновлення до таблиці.

```cs
modelBuilder.Entity<CarViewModel>()
  .ToTable('Inventory')
  .ToView('InventoryWithMakesView');
```

## Виконання запиту

Запити на отримання даних створюються за допомогою запитів LINQ, написаних до властивостей DbSet<T>. Запит LINQ змінюється на мову бази даних (наприклад, T-SQL) механізмом перекладу LINQ постачальника бази даних і виконується на стороні сервера. Багатозаписні (або потенційно багатозаписні) запити LINQ не виконуються, доки запит не почне перебиратись (наприклад, за допомогою foreach) або буде прив’язаний до елемента керування для відображення (наприклад, сітка даних). Це відкладене виконання дозволяє створювати запити в коді, не страждаючи від проблем із продуктивністю через спілкування з базою даних або отримання більшої кількості записів, ніж заплановано.
Наприклад, щоб отримати всі записи про жовті автомобілі з бази даних, виконується такий запит:

```cs
var cars = context.Cars.Where(c=>c.Color == 'Yellow');
```
При відкладеному виконанні запит до бази даних фактично не відбувається, доки результати не починає використовуватись. Для негайного виконання запиту використовуйте ToList().

```cs
var listOfCars = context.Cars.Where(x=>x.Color == 'Yellow').ToList();
```
Оскільки запити не виконуються до тих пір, поки вони не запущені, вони можуть складатися з кількох рядків коду.

```cs
var query = context.Cars.AsQueryable();
query = query.Where(x=>x.Color == 'Yellow');
var moreCars = query.ToList();
```
Запити з одним записом (наприклад, під час використання First()/FirstOrDefault()) виконуються негайно після виклику дії (наприклад, FirstOrDefault()), а оператори створення, оновлення та видалення виконуються негайно, коли метод DbContext.SaveChanges() виконується.

У наступних розділах дуже детально розглядається виконання операцій CRUD.

## Tracking та NoTracking запити

Коли дані зчитуються з бази даних в екземпляр DbSet<T> із первинним ключем, сутності (за замовчуванням) відстежуються засобом відстеження змін. Зазвичай це те, що ви хочете. Будь-які зміни в елементі потім можна зберегти в базі даних, просто викликавши SaveChanges() у вашому похідному екземплярі DbContext без будь-якої додаткової роботи з вашого боку. Крім того, після відстеження екземпляра засобом відстеження змін будь-які подальші звернення до бази даних для того самого елемента (на основі первинного ключа) призведуть до оновлення елемента, а не до дублювання.
Однак можуть бути випадки, коли вам потрібно отримати деякі дані з бази даних, але ви не хочете, щоб їх відстежував засіб відстеження змін. Причиною може бути продуктивність (відстеження початкових і поточних значень для великого набору записів може збільшити тиск на пам’ять), або, можливо, ви знаєте, що ці записи ніколи не будуть змінені частиною програми, якій потрібні дані.
Щоб завантажити дані в екземпляр DbSet<T> без додавання даних до ChangeTracker, додайте AsNoTracking() до оператора LINQ. Наприклад, щоб завантажити запис автомобіля, не додаючи його до ChangeTracker, виконайте наступне:

```cs
var untrackedCar = context.Cars.Where(x=>x.Id ==1).AsNoTracking();
```

Це дає перевагу не додавати потенційний тиск на пам’ять із потенційним недоліком: додаткові виклики для отримання того самого автомобіля створять додаткові копії запису.
За рахунок використання більшої пам’яті та дещо повільнішого часу виконання запит можна змінити, щоб забезпечити наявність лише одного екземпляра незіставленого автомобіля.

```cs
var untrackedWithIdResolution =
    context.Cars.Where(x=>x.Id == 1).AsNoTrackingWithIdentityResolution();
```

Типи запитів ніколи не відстежуються, оскільки їх неможливо оновити. Винятком є ​​використання гнучкого відображення запитів/таблиць. У цьому випадку екземпляри відстежуються за замовчуванням, щоб їх можна було зберегти в цільовій таблиці.

## Code First чи Database First

Незалежно від того, створюєте ви нову програму чи додаєте EF Core до існуючої програми, у вас буде один із двох варіантів: у вас є існуюча база даних, з якою вам потрібно працювати, або у вас ще немає бази даних і вам потрібно створити з нуля.

Спочатку код (Code First) означає, що ви створюєте та налаштовуєте свої класи сутностей і похідний DbContext у коді, а потім використовуєте міграції для оновлення бази даних. Так розробляється більшість нових проектів. Перевага полягає в тому, що під час створення програми ваші сутності розвиваються відповідно до потреб вашої програми. Міграції забезпечують синхронізацію бази даних, тому дизайн бази даних розвивається разом із вашою програмою. Цей новий процес проектування популярний серед гнучких команд розробників, оскільки ви створюєте потрібні частини в потрібний час.

Якщо у вас уже є база даних або ви бажаєте, щоб дизайн бази даних керував вашою програмою, це називається Database First. Замість того, щоб створювати похідний DbContext і всі сутності вручну, ви створюєте класи з бази даних. Коли база даних змінюється, вам потрібно перебудувати свої класи, щоб підтримувати синхронізацію коду з базою даних. Будь-який настроюваний код в сутностях або похідному DbContext має бути розміщено в часткових класах, щоб він не перезаписувався під час повторного створення каркасів класів. На щастя, процес скаффолдингу створює часткові класи саме з цієї причини.

Який би метод ви не вибрали, спочатку код або базу даних, знайте, що це, по суті, зобов’язання. Якщо ви спочатку використовуєте код, усі зміни вносяться до класів сутності та контексту, а база даних оновлюється за допомогою міграцій. Якщо ви спочатку працюєте з базою даних, усі зміни потрібно внести в базу даних, а потім класи повторно створюються. Доклавши певних зусиль і плануючи, ви можете переключитися з бази даних спочатку на код (і навпаки), але ви не повинні вручну вносити зміни в код і базу даних одночасно.

# Команди CLI EF Core Global Tool

dotnet-ef глобальний інструмент CLI EF Core містить необхідні команди для стоврення коду для існуючої БД, створювати/видаляти міграції бази даних і працювати з базою даних (оновлювати, видаляти тощо). Перш ніж ви зможете використовувати глобальний інструментарій dotnet-ef, його потрібно встановити за допомогою такої команди:

```console
dotnet tool install --global dotnet-ef
```
Якщо у вас встановлено попередню версію інструментів командного рядка EF Core, вам потрібно буде видалити стару версію, перш ніж інсталювати останню версію. Щоб видалити глобальний інструмент, використовуйте

```console
dotnet tool uninstall --global dotnet-ef
```

Щоб перевірити встановлення, відкрийте командний рядок і введіть таку команду:
```console
dotnet ef


Usage: dotnet ef [options] [command]

Options:
  --version        Show version information
  -h|--help        Show help information
  -v|--verbose     Show verbose output.
  --no-color       Don't colorize output.
  --prefix-output  Prefix output with level.

Commands:
  database    Commands to manage the database.
  dbcontext   Commands to manage DbContext types.
  migrations  Commands to manage migrations.

Use "dotnet ef [command] --help" for more information about a command.
```

Таблиця описує три основні команди в глобальному інструменті EF Core.

| Команда | Значення в використані |
| -------- | --------- |
| Database | Команди керування базою даних. Підкоманди включають drop і update.|
| DbContext | Команди для керування типами DbContext. Підкоманди включають scaffold, list і info.|
| Migrations | Команди для керування міграціями. Підкоманди включають add, list, remove і script.|

Команди EF Core виконуються у файлах проекту .NET. Цільовий проект має посилатися на пакет інструментів EF Core NuGet Microsoft.EntityFrameworkCore.Design. Команди діють із файлом проекту, розташованим у тому самому каталозі, де виконуються команди, або файлом проекту в іншому каталозі, якщо на нього посилаються через параметри командного рядка. 
Для команд EF Core CLI, яким потрібен екземпляр похідного класу DbContext (Database and Migrations), якщо в проекті є лише один, використовуватиметься він. Якщо їх більше ніж один, то DbContext потрібно вказати в параметрах командного рядка. Похідний клас DbContext буде створено за допомогою екземпляра класу, що реалізує інтерфейс IDesignTimeDbContextFactory<TContext>, якщо його можна знайти. Якщо інструменти не можуть його знайти, похідний DbContext буде створено за допомогою конструктора без параметрів. Якщо жодного з них не існує, команда не вдасться виконати. Зауважте, що використання конструктора без параметрів (а не конструктора, який приймає DbContextOptions<T>) вимагає наявності перевизначення OnConfiguring, що не вважається хорошою практикою. Найкращий (і насправді єдиний) варіант — завжди створювати IDesignTimeDbContextFactory<TContext> для кожного похідного DbContext, який є у вашій програмі.

Для команд EF Core доступні загальні параметри, наведені в таблиці. Багато команд мають додаткові параметри або аргументи.

Опції команд EF Core

| Опція(Скорочено та Повно) | Значення в використані |
| -------- | --------- |
|--c  --context DBCONTEXT | Повністю кваліфікований похідний клас DbContext для використання. Якщо в проекті існує більше одного похідного DbContext, це обов’язковий параметр. |
|-p --project PROJECT |Проект для використання (де розмістити файли). За замовчуванням використовується поточний робочий каталог. |
|-s --startup-project PROJECT| Стартовий проект для використання (містить похідний DbContext). За замовчуванням використовується поточний робочий каталог. |
|-h  --help| Відображає довідку та всі параметри. |
|-v  --verbose| Показує докладну довідку.|

Щоб перелічити всі аргументи та параметри для команди, введіть dotnet ef <command> -h у командному вікні :

```console
dotnet ef migrations add -h
```
Важливо зауважити, що команди CLI не є командами C#, тому правила екранування скісних риск і лапок не застосовуються.

Аби краще розібратися з роботою CLI EF Core створіть простий консольний проект наприклад MyShop. Додайте пакети

Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.Design
Microsoft.EntityFrameworkCore.SqlServer

Додайте класи. 

```cs
namespace MyShop;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }

}
```
```cs
namespace MyShop;

public class ApplicationDbContext : DbContext
{
    //Properties
    public DbSet<Product> Products { get; set; }

    //Constructors
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}
```
```cs
namespace MyShop;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        string connectionString = @"Server=(localdb)\mssqllocaldb;Database=MyShop;Trusted_Connection=True;ConnectRetryCount=0";

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
```
Запишить всі змінені файли. 

Відкрийте командний рядок Tools > Command line > Deweloper PowerShell

```console
PS D:\...\MyShop> dir
PS D:\...\MyShop> cd MyShop
```
Тепер можна виконувати команди.

## Команди migrations

```console
dotnet ef migrations -h
```
Команди міграції використовуються для додавання, видалення, списків і міграцій сценаріїв. Коли міграції застосовуються до бази, у таблиці __EFMigrationsHistory створюється запис. 
Команди EF Core Migrations

| Команда | Значення в використані |
| -------- | --------- |
|add|Створює нову міграцію на основі змін попередньої міграції|
|remove|Перевіряє, чи було застосовано останню міграцію в проекті до бази даних, і, якщо ні, видаляє файл міграції (і його конструктор), а потім повертає клас знімка до попередньої міграції|
|has-pending-model-changes|Перевіряє, чи були внесені будь-які зміни в модель з часу останньої міграції.|
|list|Перелічує всі міграції для похідного DbContext і їхній статус (застосовано чи очікує на розгляд)|
|bundle|Створює виконуваний файл для оновлення бази даних.|
|script|Створює сценарій SQL для всіх, одного або діапазону міграцій|

### Команда add

```console
 dotnet ef migrations add -h
```
Команда add створює нову міграцію бази даних на основі поточної об’єктної моделі. Процес перевіряє кожну сутність із властивістю DbSet<T> у похідному DbContext (і кожну сутність, до якої можна отримати доступ із цих сутностей за допомогою властивостей навігації), і визначає, чи є якісь зміни, які потрібно застосувати до бази даних. Якщо є зміни, генерується відповідний код для оновлення бази даних.
Для команди add потрібен аргумент імені, який використовується для назви класу створення та файлів для міграції. На додаток до загальних параметрів, параметр -o PATH або –output-dir PATH вказує, куди мають бути розміщені файли міграції. Стандартний каталог має назву Migrations відносно поточного шляху.

Створимо міграцію.

```console
 dotnet ef migrations add InitialCreateDBAddProduct
```
Кожна додана міграція створює два файли, які є частинами одного класу. Імена обох файлів починаються з позначки часу та імені міграції, яке використовується як аргумент команди add. Перший файл має назву <YYYYMMDDHHMMSS>_<MigrationName>.cs, а другий — <YYYYMMDDHHMMSS>_<MigrationName>.Designer.cs. Мітка часу базується на даті створення файлу та точно збігається для обох файлів. Перший файл представляє код, згенерований для змін бази даних у цій міграції, а файл конструктора представляє код для створення та оновлення бази даних на основі всіх міграцій до цієї міграції включно.

Головний файл містить два методи Up() і Down(). Метод Up() містить код для оновлення бази даних змінами цієї міграції, а метод Down() містить код для відкоту змін цієї міграції. Ось приклад:

```cs
    public partial class InitialCreateDBAddProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
```
Як бачите, метод Up() створює таблиці, стовпці, індекси тощо. Метод Down() видаляє створені елементи. Механізм міграції видасть оператори alter, add і drop, якщо необхідно, щоб база даних відповідала вашій моделі. 

Перша міграція створює додатковий файл у цільовому каталозі з іменем похідного DbContext у форматі <DerivedDbContextName>ModelSnapshot.cs. Формат цього файлу такий самий, і містить код, який є сумою всіх міграцій. Коли міграції додаються або видаляються, цей файл автоматично оновлюється відповідно до змін.

Файл з дизайном моделі містить атрибут, який прив’язує частину до імені файлу та похідного DbContext.

```cs
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        //...
    }    
```

Надзвичайно важливо не видаляти файли міграції вручну. Це призведе до того, що <DerivedDbContext>ModelSnapshot.cs не синхронізується з вашими міграціями, фактично порушуючи їх. Якщо ви збираєтеся видалити їх вручну, видаліть їх усіх і почніть спочатку. Щоб видалити міграцію, скористайтеся командою видалення, про яку ми коротко розглянемо.

Додамо трохи незграбниу сутність. 

```cs
namespace MyShop;

public class Bolt
{
    public int Id { get; set; }
    public double Price { get; set; }
}
```
```cs
public class ApplicationDbContext : DbContext
{
    //Properties
 
    // ...

    public DbSet<Bolt> Bolts { get; set; }

    //...
}
```
Виконаемо перевірку чи є зміни моделі даних

```console
dotnet ef migrations -h
dotnet ef migrations has-pending-model-changes

Changes have been made to the model since the last migration. Add a new migration.
```
EF Core побачив зміни моделі та запропонував зробити нову міграцію.

```console
 dotnet ef migrations add AddBolt

 Done. To undo this action, use 'ef migrations remove'
```
### Команда list

```console
dotnet ef migrations list
```
Команда list використовується для показу всіх міграцій для похідного DbContext. За замовчуванням він перелічить усі міграції та запитає базу даних, щоб визначити, чи було їх застосовано. Якщо вони не були застосовані, вони будуть указані як очікуючі(Pending) на розгляд. Є можливість передати певний рядок з’єднання та інший варіант, щоб узагалі не підключатися до бази даних і натомість просто перерахувати міграції.

```console
dotnet ef migrations list -h


Usage: dotnet ef migrations list [options]

Options:
  --connection <CONNECTION>              The connection string to the database. Defaults to the one specified in AddDbContext or OnConfiguring.
  --no-connect                           Don't connect to the database.

dotnet ef migrations list

20241028092947_InitialCreateDBAddProduct (Pending)
20241028100158_AddBolt (Pending)
```

### Команда remove

```console
 dotnet ef migrations remove
```

Команда видалення використовується для видалення міграцій із проекту та завжди працює з останньою міграцією (на основі позначок часу міграцій). Під час видалення міграції EF Core переконається, що її не було застосовано, перевіривши таблицю __EFMigrationsHistory у базі даних. Якщо міграцію було застосовано, процес не вдається. Якщо міграцію ще не застосовано або її було відкочено, міграцію буде видалено, а файл знімка моделі оновлено. Є один додатковий параметр, параметр force (-f || --force). Це призведе до відкоту останньої міграції, а потім її видалення за один крок.

Видалемо останню міграцію.
```console
dotnet ef migrations remove

Removing migration '20241028100158_AddBolt'.
Reverting the model snapshot.
Done.
```
Видалимо код непотрібної сутності.


### Команда bundle

```console
dotnet ef migrations bundle -h
```
Команда bundle створює виконуваний файл для оновлення бази даних. Згенерований виконуваний файл, створений для цільового середовища виконання (наприклад, Windows, Linux), застосує всі наявні міграції до бази даних.

Загальні аргументи для команди bundle
| Аргумент | Значення в використані |
| -------- | --------- |
|-o | --output <file>|Шлях до виконуваного файлу, який потрібно створити.|
|-f | --force|Перезаписати наявні файли.|
|--self-contained|Також об’єднайте середовище виконання .NET із виконуваним файлом.|
|-r | --target-runtime <RUNTIME ID>|Цільове середовище виконання для пакетування. Якщо час виконання не вказано, файл використовуватиме час виконання поточної операційної системи машини.|

Виконуваний файл використовуватиме рядок підключення з IDesignTimeDbContextFactory; однак інший рядок підключення можна передати у виконуваний файл за допомогою прапорця --connection. Якщо міграції вже було застосовано до цільової бази даних, вони не будуть застосовані повторно.
Якщо застосувати прапорець --self-contained, розмір виконуваного файлу значно збільшиться оскільки збільшиться кодом для CLR .Net. 

В нашому випадку створеться файл.
```console
dotnet ef migrations bundle

Done. Migrations Bundle: D:\...\MyShop\efbundle.exe
```

### Команда script

```console
dotnet ef migrations script -h
```
Команда script створює сценарій SQL на основі однієї або кількох міграцій. Команда приймає два необов’язкові аргументи, що представляють міграцію для початку та міграцію для завершення. Якщо жоден з них не введено, усі міграції виконуються за сценарієм.

Аргументи для команди script
| Аргумент | Значення в використані |
| -------- | --------- |
|NameMigrationFrom|Початкова міграція. За замовчуванням 0 (нуль), початкова міграція.|
|NameMigrationTo|Цільова міграція. За умовчанням використовується остання міграція.|

Якщо жодна міграція не вказана, створений сценарій буде загальним підсумком усіх міграцій.

```console
//Script all of the migrations
dotnet ef migrations script
//Script of the migration 20241015151929_AlterMakeAddName
dotnet ef migrations script 20241015150453_AddMakes 20241015151929_AlterMakeAddName
```
Доступні додаткові опції. Опція -o дозволяє вказати файл для сценарію (каталог є відносно того, де виконується команда). Опція -i створює ідемпотентний сценарій. Це означає, що він містить перевірки, щоб побачити, чи міграція вже була застосована, і пропускає цю міграцію, якщо вона була. Параметр –no-transaction вимикає звичайні транзакції, які додаються до сценарію.

| Опції | Значення в використані |
| -------- | --------- |
|-o -output FILE|Файл, до якого потрібно записати кінцевий сценарій|
|-i --idempotent|Створює сценарій, який перевіряє, чи було вже застосовано міграцію перед її застосуванням|
|--no-transactions|Не загортає кожну міграцію в транзакцію|

```console
PS D:\...\MyShop> dotnet ef migrations script --no-transactions
Build started...
Build succeeded.
IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

CREATE TABLE [Products] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241028092947_InitialCreateDBAddProduct', N'8.0.10');
GO
```

## Команди database

```console
dotnet ef database -h
```
Існує дві команди бази даних: drop та update. Команда drop видаляє базу даних, якщо вона існує. Команда update оновлює базу даних за допомогою міграцій.

### Команда drop

```console
dotnet ef database drop
```
Команда drop видаляє базу даних, визначену рядком підключення, у фабрикі контексту методу OnConfiguring DbContext. Використання опції -force не запитує підтвердження та примусово закриває всі з’єднання.

| Опції | Значення в використані |
| -------- | --------- |
|-f --force|Не запиує підтвердження видалення. Примусово закрити всі з’єднання.|
|--dry-run|Покаже, яку базу даних буде видалено, але не видаляе її. |

### Команда update

Команда оновлення приймає один аргумент (ім’я міграції) і звичайні параметри. Команда має один додатковий параметр --connection <CONNECTION>. Це дозволяє використовувати рядок підключення, який не налаштовано у фабрикі під час розробки або DbContext.

Якщо команда виконується без імені міграції, команда оновлює базу даних використовуючи остані очікуючи міграції, створюючи базу даних, якщо необхідно.Якщо міграцію названо, базу даних буде оновлено відповідно до цієї міграції. Усі попередні міграції, які ще не застосовано, також будуть застосовані. Коли міграції застосовуються, їх імена зберігаються в таблиці __EFMigrationsHistory.

Якщо названа міграція має мітку часу, яка є ранішою за інші застосовані міграції, усі пізніші міграції відкочуються. Якщо 0 (нуль) передано як назву міграції, усі міграції повертаються, залишаючи порожню базу даних (окрім таблиці __EFMigrationsHistory).

```console
PS D:\MyWork\CS-Step-by-Step\20 Entity Framework Core\01 Перший погляд на Entity Framework Core\MyShop\MyShop> dotnet ef database update
Build started...
Build succeeded.
Applying migration '20241028092947_InitialCreateDBAddProduct'.
Done.
```
Після цієї команди можна побачити базу даних з таблицею в SQL Server Object Explorer. 

## Команди dbcontext

```console
dotnet ef dbcontext -h
```
Є чотири команди DbContext. Три з них (list, info, script) працюють із похідними класами DbContext проекту. Команда scaffold створює похідний DbContext і сутності з існуючої бази даних.

Команди dbcontext
|Команда|Значення в використані|
| ----- | ----- |
|Info|Дістає інформацію про тип DbContext|
|List|Список доступних типів DbContext|
|Optimize|Створює скомпільовану версію моделі, яка використовується DbContext|
|Scaffold|Генерує DbContext і типи сутностей для бази даних|
|Script|Генерує сценарій SQL з DbContext на основі об’єктної моделі, оминаючи будь-які міграції|

Для команд list та info доступні звичайні параметри. Команда list містить список похідних класів DbContext у цільовому проекті. Команда info надає подробиці про вказаний похідний клас DbContext, включаючи рядок підключення, ім’я постачальника, ім’я бази даних і джерело даних. 
```console
PS D:\...\MyShop> dotnet ef dbcontext info

Type: MyShop.ApplicationDbContext
Provider name: Microsoft.EntityFrameworkCore.SqlServer
Database name: MyShop
Data source: (localdb)\mssqllocaldb
Options: None
```
Команда script створює сценарій SQL, який створює вашу базу даних на основі об’єктної моделі, ігноруючи будь-які міграції, які можуть бути присутніми. 
```console
PS D:\...\MyShop> dotnet ef dbcontext script

CREATE TABLE [Products] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([Id])
);
GO
```

Команда scaffold використовується для зворотного проектування існуючої бази даних.

### Команда scaffold

```console
dotnet ef dbcontext scaffold -h
```

Команда scaffold створює класи C# (похідний DbContext та сутності) разом із анотаціями даних (за запитом) і командами Fluent API з існуючої бази даних. Існує два обов’язкові аргументи: рядок підключення до бази даних і повний провайдер даних (наприклад, Microsoft.EntityFrameworkCore.SqlServer).

Агументи команди scaffold
|Аргумент|Значення в використані|
| ------ | ----- |
|Connection|Рядок підключення до бази даних|
|Provider|Провайдер бази даних EF Core для використання (наприклад, Microsoft.EntityFrameworkCore.SqlServer)|

Доступні параметри включають вибір конкретних схем і таблиць, ім’я та простір імен створеного контекстного класу, вихідний каталог та простір імен згенерованих класів сутностей та багато іншого. Також доступні стандартні варіанти.

Опції команди dbcontext scaffold
|Опція|Значення в використані|
| ------ | ----- |
|-d --data-annotations | Використовуйте атрибути для налаштування моделі (де можливо). Якщо пропущено, використовується тільки Fluent API.|
|-c --context NAME|Ім’я похідного DbContext для створення.|
|-f --force|Замінює всі існуючі файли в цільовому каталозі.|
|-o --output-dir PATH|Каталог для розміщення згенерованих класів сутностей. Відносно каталогу проекту.|
|--schema SCHEMA_NAME|Схеми таблиць для генерації типів сутностей.|
|-t --table TABLE_NAME|Таблиці для генерації типів сутностей.|
|--use-database-names|Використовувати імена таблиць і стовпців безпосередньо з бази даних.|
|-n --namespaces NAMESPACE|Простір імен для згенерованих класів сутностей. За замовчуванням відповідає каталогу.|
|--context-namespace NAMESPACE|Простір імен для згенерованого похідного класу DbContext. За замовчуванням відповідає каталогу.|
|--no-onconfiguring|Не створює метод OnConfiguring.|
|--no-pluralize|Не використовати множині нази.|

Якщо вибрано параметр анотації даних (-d), EF Core використовуватиме анотації даних, де це можливо, і заповнюватиме відмінності за допомогою Fluent API. Якщо цей параметр не вибрано, у Fluent API кодується вся конфігурація (де вона відрізняється від домовленостей). Ви можете вказати простір імен, схему та розташування для згенерованих сутностей і похідних файлів DbContext. Якщо ви не хочете створювати всю базу даних, ви можете вибрати певні схеми та таблиці. Параметр --no-onconfiguring усуває метод OnConfiguring() із класу. Параметр –no-pluralize вимикає множинний сутність, який перетворює окремі сутності (Car) на множинні таблиці (Cars) під час створення міграцій і перетворює множинні таблиці на окремі сутності під час scaffolding.

Додамо в рішеня ше один проект YourShop і встанвимо тіж самі пакети.

Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.Design
Microsoft.EntityFrameworkCore.SqlServer

Виконаємо команди

```console
cd ..
cd YourShop
dotnet ef dbcontext scaffold "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MyShop" Microsoft.EntityFrameworkCore.SqlServer

To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https: ...
```
Після цого в каталозі проекту появиться класи Product і MyShopContext.


### Команда optimize

```console
dotnet ef dbcontext optimize -h
```
Команда optimize оптимізує похідний DbContext, виконуючи багато кроків, які зазвичай відбуваються під час першого використання похідного DbContext. Доступні параметри включають визначення каталогу для розміщення скомпільованих результатів, а також того, який простір імен використовувати.

|Опція|Значення в використані|
| ------ | ----- |
|-o --output-dir|Каталог для розміщення файлів. Шляхи відносно каталогу проекту.|
|-n --namespace NAMESPACE|Простір імен для використання. За замовчуванням відповідає каталогу.|

Коли похідний DbContext компілюється, результати включають клас для кожної сутності у вашій моделі, скомпільований похідний DbContext і скомпільований похідний DbContext ModelBuilder. 

Повернемось в рішення AutoLot.Samples і відкриемо командний рядок. 

Можемо скомпілювати AutoLot.Samples.ApplicationDbContext за допомогою наступної команди:

```console
dotnet ef dbcontext optimize -o CompiledModels
```
Зкомпільовані файли розміщуються в каталозі під назвою CompiledModels. Щоб використовувати скомпільовану модель, викличте метод UseModel() у DbContextOptions:

```cs
var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
var connectionString = @"Server=(localdb)\mssqllocaldb;Database=MyDatabase;Trusted_Connection=True;ConnectRetryCount=0";
optionsBuilder.UseSqlServer(connectionString).UseModel(ApplicationDbContextModel.Instance);
var context = new ApplicationDbContext(optionsBuilder.Options);
```
Компіляція похідного DbContext може значно підвищити продуктивність у певних ситуаціях, але є деякі обмеження: 

- Глобальні фільтри запитів не підтримуються.
- Проксі з відкладеним завантаженням не підтримуються.
- Проксі відстеження змін не підтримуються.
- Модель потрібно перекомпілювати кожного разу, коли вона змінюється.

Якщо ці обмеження не є проблемою для вашої ситуації, використання оптимізації DbContext може значно покращити продуктивність ваших програм.
