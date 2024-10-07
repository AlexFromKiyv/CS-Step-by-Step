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

Створемо консольний додаток AutoLot.Samples. Додамо пекти 

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

    Database : Надає доступ до інформації та функцій, пов’язаних із базою даних, включаючи виконання операторів SQL.

    Model : Метадані про форму сутностей, зв’язки між ними та те, як вони відображаються в базі даних.

    ChangeTracker : Надає доступ до інформації та операцій для екземплярів сутності, які відстежує цей DbContext.

    DbSet<T> : Насправді не є членом DbContext, але властивості додано до настроюваного похідного класу DbContext. Властивості мають тип DbSet<T> і використовуються для запиту та збереження екземплярів сутностей програми. Запити LINQ щодо властивостей DbSet<T> перекладаються на запити SQL.

    Entry() : Надає доступ до інформації про відстеження змін і операцій для сутності, таких як явне завантаження пов’язаних сутностей або зміна EntityState. Також можна викликати невідстежувану сутність, щоб змінити стан на відстежуваний.

    Set<TEntity>() : Створює екземпляр властивості DbSet<T>, який можна використовувати для запиту та збереження даних.

    SaveChanges()/SaveChangesAsync() : Зберігає всі зміни сутності в базі даних і повертає кількість задіяних записів. Виконується в транзакції (явній або неявній).

    Add()/AddRange(), Update()/UpdateRange(), Remove()/RemoveRange() : Методи додавання, оновлення та видалення екземплярів сутності. Зміни зберігаються лише після успішного виконання SaveChanges(). Також доступні асинхронні версії. Примітка: Хоча ці методи доступні в похідному DbContext, вони зазвичай викликаються безпосередньо у властивостях DbSet<T>.

    Find() : Знаходить сутність типу із заданими значеннями первинного ключа. Також доступні асинхронні версії. Примітка: Хоча ці методи доступні в похідному DbContext, вони зазвичай викликаються безпосередньо у властивостях DbSet<T>.

    Attach()/AttachRange() : Починає відстежувати сутність (або список сутностей).Також доступні асинхронні версії. Примітка: Хоча ці методи доступні в похідному DbContext, вони зазвичай викликаються безпосередньо у властивостях DbSet<T>.

    SavingChanges() : Подія запускається на початку виклику SaveChanges()/SaveChangesAsync().

    SavedChanges() : Подія запускається в кінці виклику SaveChanges()/SaveChangesAsync().

    SaveChangesFailed : Подія запускається, якщо виклик SaveChanges()/SaveChangesAsync() не вдається.

    OnModelCreating() : Викликається, коли модель ініціалізовано, але до її завершення. Методи з API Fluent розміщуються в цьому методі для завершення форми моделі.

    OnConfiguring() : Конструктор, який використовується для створення або зміни параметрів для DbContext. Примітка. Рекомендується не використовувати це, натомість використовувати DbContextOptions для налаштування екземпляра DbContext під час виконання та використовувати екземпляр IDesignTimeDbContextFactory під час розробки.

## Побудова похідного касу від DBContext

### Створення класу

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

Екземпляр DbContext налаштовується за допомогою екземпляра класу DbContextOptions. Екземпляр DbContextOptions створюється за допомогою DbContextOptionsBuilder, оскільки клас DbContextOptions не призначений для безпосереднього створення у вашому коді. За допомогою екземпляра DbContextOptionsBuilder вибирається постачальник бази даних (разом із будь-якими параметрами, що стосуються постачальника), і встановлюються загальні параметри EF Core DbContext (наприклад, журналювання). Потім екземпляр DbContextOptions вставляється в базовий DbContext під час виконання.
Ця можливість динамічної конфігурації дозволяє змінювати налаштування під час виконання, просто вибираючи різні параметри (наприклад, MySQL замість постачальника SQL Server) і створюючи новий екземпляр вашого похідного DbContext.

### DbContext Factory під час розробки

DbContext Factory під час розробки — це клас, який реалізує інтерфейс IDesignTimeDbContextFactory<T>, де T — похідний клас DbContext. Інтерфейс має один метод CreateDbContext(), який ви повинні реалізувати, щоб створити екземпляр вашого похідного DbContext. Цей клас не призначений для використання у виробництві, а лише під час розробки, і існує в основному для інструментів командного рядка EF Core, які ви незабаром дослідите. У прикладах у цьому та наступному розділах він використовуватиметься для створення нових екземплярів ApplicationDbContext. Вважається поганою практикою використовувати фабрику DbContext для створення екземплярів вашого похідного класу DbContext. Пам’ятайте, що це демонстраційний код, призначений для навчання, і використання його таким чином робить демонстраційний код чистішим. Ви побачите, як правильно створити екземпляр похідного класу DbContext у розділі ASP.NET Core.

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
Клас ApplicationDbContextFactory використовує метод CreateDbContext() для створення строго типізованого DbContextOptionsBuilder для класу ApplicationDbContext, встановлює постачальника бази даних на постачальника SQL Server (використовуючи рядок підключення екземпляра Docker з розділу 20), а потім створює та повертає новий екземпляр ApplicationDbContext.

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

    Add()/AddRange() : Починає відстежувати сутність/сутності в стані Додано. Елемент(и) буде додано під час виклику SaveChanges(). Також доступні асинхронні версії.

    AsAsyncEnumerable() : Повертає колекцію як IAsyncEnumerable<T>.

    AsQueryable() : Повертає колекцію як IQueryable<T>.

    Find() : Шукає сутність у ChangeTracker за первинним ключем. Якщо об’єкт не знайдено в системі відстеження змін, у сховищі даних запитується об’єкт. Також доступна асинхронна версія.

    Update()UpdateRange() : Починає відстежувати сутність/сутності у зміненому стані. Елемент(и) буде оновлено під час виклику SaveChanges. Також доступні асинхронні версії.

    Remove()RemoveRange() : Починає відстежувати сутність/сутності у стані "Видалено". Елемент(и) буде видалено під час виклику SaveChanges(). Також доступні асинхронні версії.

    Attach()AttachRange() : Починає відстежувати сутність/сутності. Сутності з цифровими первинними ключами, визначеними як ідентичність, і значенням, що дорівнює нулю, відстежуються як Додані. Усі інші відстежуються як незмінені. Також доступні асинхронні версії.

    FromSqlRaw() FromSqlInterpolated() : Створює запит LINQ на основі необробленого або інтерпольованого рядка, що представляє запит SQL. Може поєднуватися з додатковими операторами LINQ для виконання на сервері.

Тип DbSet<T> реалізує IQueryable<T>, що дозволяє використовувати запити LINQ для отримання записів із бази даних. На додаток до методів розширення, доданих EF Core, DbSet<T> підтримує ті самі методи розширення, про які ви дізналися в розділі про LINQ, наприклад ForEach(), Select() і All(). 
Ви додасте властивості DbSet<T> до ApplicationDbContext пізніше.

### ChangeTracker

Екземпляр ChangeTracker відстежує стан об’єктів, завантажених у DbSet<T> в екземплярі DbContext.

Значення перерахування стану сутності

    Added : Сутність відстежується, але ще не існує в базі даних.

    Deleted : Сутність відстежується та позначена для видалення з бази даних.

    Detached : Сутність не відстежується засобом відстеження змін.

    Modified : Сутність відстежується та був змінений.

    Unchanged : Сутність відстежується, існує в базі даних і не була змінена.

Якщо вам потрібно перевірити стан об'єкта, використовуйте такий код:

```cs
EntityState state = context.Entry(entity).State;
```

Ви також можете програмно змінити стан об’єкта, використовуючи той самий механізм. Щоб змінити стан на Видалено (наприклад), використовуйте такий код:

```cs
context.Entry(entity).State = EntityState.Deleted;
```

### Події ChangeTracker

Є дві події, які можуть бути викликані ChangeTracker. Перша — StateChanged, а друга — Tracked. Подія StateChanged запускається, коли змінюється стан об’єкта. Він не спрацьовує під час першого відстеження сутності. Подія Tracked спрацьовує, коли сутність починає відстежуватися, або через програмне додавання до екземпляра DbSet<T>, або коли повертається із запиту.
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

```
Властивість FromQuery EntityTrackedEventArgs вказує, чи була сутність завантажена через запит бази даних чи програмно.

### Скидання стану DbContext

Існує можливість скинути похідний DbContext назад до початкового стану. Метод ChangeTracker.Clear() видаляє всі сутності з колекцій DbSet<T>, установлюючи для них стан Detached. Основною перевагою цього є підвищення продуктивності. Як і з будь-яким ORM, створення екземпляра похідного класу DbContext вимагає певних витрат. Хоча ці накладні витрати зазвичай не є значними, можливість очистити вже створений контекст може допомогти підвищити продуктивність у деяких сценаріях.

## Сутності

Строго типізовані класи, які відображаються в таблицях бази даних, офіційно називаються сутностями. Набір сутностей у програмі містить концептуальну модель фізичної бази даних. Формально кажучи, ця модель називається моделлю даних сутності (entity data model EDM ), яку зазвичай називають просто моделлю. Модель зіставляється з доменом програми/бізнесу. Сутності та їхні властивості зіставляються з таблицями та стовпцями за допомогою угод Entity Framework Core, конфігурації та Fluent API (код). Сутності не потребують прямого зіставлення зі схемою бази даних. Ви можете структурувати свої класи сутностей відповідно до потреб програми, а потім зіставляти унікальні сутності зі схемою бази даних.
Цей слабкий зв’язок між базою даних і вашими об’єктами означає, що ви можете формувати об’єкти відповідно до домену вашого бізнесу, незалежно від конструкції та структури бази даних. Наприклад, візьмемо просту таблицю Inventory в базі даних AutoLot і клас сутності Car з попереднього розділу.Назви різні, але сутність Car можна зіставити з таблицею  Inventory. EF Core перевіряє конфігурацію ваших сутностей у моделі, щоб зіставити представлення клієнтської сторони таблиці Inventory (у нашому прикладі класу Car) на правильні стовпці таблиці Inventory. 
У наступних кількох розділах детально описано, як угоди EF Core, анотації даних і код (за допомогою Fluent API) відображають сутності, властивості та зв’язки між сутностями в режимі на таблиці, стовпці та зв’язки зовнішнього ключа у вашій базі даних.

### Властивості сутності та стовпці бази даних

Під час використання реляційного сховища даних EF Core використовує дані зі стовпців таблиці для заповнення властивостей сутності під час читання зі сховища даних і записує властивості сутності в стовпці таблиці під час збереження даних. Якщо властивість є автоматичною властивістю, EF Core читає та записує через getter і setter. Якщо властивість має опорне поле, EF Core читатиме та записуватиме в резервне поле замість публічної власності, навіть якщо опорне поле є приватним. Хоча EF Core може читати та записувати в приватні поля, все одно має бути загальнодоступна властивість читання та запису, яка інкапсулює резервне поле. Два сценарії, коли підтримка резервного поля є перевагою, це використання шаблону INotifyPropertyChanged у програмах Windows Presentation Foundation (WPF) і коли значення за замовчуванням бази даних суперечать значенням за замовчуванням .NET.

### Схеми відображення таблиць

У EF Core доступні дві схеми відображення класів у таблиці: таблиця за ієрархією (table-per-hierarchy TPH) і таблиця за типом (table-per-type TPT). Відображення TPH є типовим і відображає ієрархію успадкування в одній таблиці. TPT відображає кожен клас в ієрархії на окрему таблицю. 
Класи також можуть бути зіставлені з представленнями та необробленими запитами SQL.

### Відображення таблиці на ієрархію (Table-Per-Hierarchy)

Створемо проект AutoLot.TPH та додамо до нього пакети. 

Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.Design
Microsoft.EntityFrameworkCore.SqlServer

Розглянемо наступний приклад, який показує, що клас Car з попереднього розділу можна розділити на два класи: базовий клас (BaseEntity) для властивостей Id і TimeStamp, а також решту властивостей у класі Car.


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
Зверніть увагу на властивість DbSet<T> у класі ApplicationDbContext. Це інформує EF Core, що клас Car зіставляється з таблицею Cars у базі даних. Також зауважте, що для класу BaseEntity немає властивості DbSet<T>. Це пояснюється тим, що в схемі TPH вся ієрархія стає єдиною таблицею. Властивості таблиць вище ланцюга успадкування згортаються в таблицю за допомогою властивості DbSet<T>. Це показано наступним SQL:

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

    Principal entity(Основна сутність) : Сутність з якої виходять відносини.

    Dependent entity(Залежна сутність) : Сутність яка залежить від іншої.

    Principal key(Основний ключ) : Властивість/властивості, які використовуються для визначення головної сутності. Може бути первинним або альтернативним ключем. Ключі можна налаштувати за допомогою однієї властивості або кількох властивостей.

    Foreign key(Зовнішній ключ) : Властивість/властивості, які зберігаються дочірньою сутністю для зберігання основного ключа.

    Required relationship(Необхідні відносини) : Зв’язок, де потрібне значення зовнішнього ключа (не допускає значення null).

    Optional relationship(Необов'язковий зв'язок) : Відношення, де значення зовнішнього ключа може буте відсутьне(nullable).

#### Відсутні властивості зовнішнього ключа

Якщо сутність із властивістю посилальної навігації не має властивості для значення зовнішнього ключа, EF Core створить необхідну властивість/властивості сутності. Вони відомі як тіньові властивості зовнішнього ключа та мають імена у форматі <navigation property name><principal key property name> або <principal entity name><principal key property name>. Це справедливо для всіх типів зв’язків (один до багатьох, один до одного, багато до багатьох). Набагато чистіший підхід створити свої сутності з явною властивістю/властивостями зовнішнього ключа, ніж змусити EF Core створити їх для вас.

#### Відносини One-to-Many

Щоб створити зв’язок «One-to-Many», клас сутності з боку One (the principal) додає властивість колекції класу сутності, який знаходиться з боку Many (the dependent). Залежна сутність також повинна мати властивості для зовнішнього ключа назад до основної. Якщо ні, EF Core створить тіньові властивості зовнішнього ключа, як пояснювалося раніше.
Наприклад, у базі даних, створеній раніше, таблиці Makes (представлена ​​класом сутності Make) і таблиця Inventory (представлена ​​класом сутності Car) мають зв’язок «One-to-Many».

Додамо сутності і відносини в проект AutoLot.Samples

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
        public DbSet<Car> Cars { get; set; }
        public DbSet<Make> Makes { get; set; }
```
Коли база даних оновлюється за допомогою міграцій EF Core, створюються такі таблиці. 
Оновлення бази даних за допомогою міграцій EF Core розглядається далі в цьому розділі.

```console
dotnet ef migrations add Initial -o Migrations -c AutoLot.Samples.ApplicationDbContext
dotnet ef database update Initial -c AutoLot.Samples.ApplicationDbContext
```

Виконається наступний запит:
```sql
CREATE TABLE [dbo].[Makes](
  [Id] [int] IDENTITY(1,1) NOT NULL,
  [Name] [nvarchar](max) NULL,
  [TimeStamp] [varbinary](max) NULL,
 CONSTRAINT [PK_Makes] PRIMARY KEY CLUSTERED
(
  [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[Cars](
  [Id] [int] IDENTITY(1,1) NOT NULL,
  [Color] [nvarchar](max) NULL,
  [PetName] [nvarchar](max) NULL,
  [TimeStamp] [varbinary](max) NULL,
  [MakeId] [int] NOT NULL,
 CONSTRAINT [PK_Cars] PRIMARY KEY CLUSTERED
(
  [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Cars] WITH CHECK ADD  CONSTRAINT [FK_Cars_Makes_MakeId]
  FOREIGN KEY([MakeId]) REFERENCES [dbo].[Makes] ([Id])
  ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Cars] CHECK CONSTRAINT [FK_Cars_Makes_MakeId]
GO
```
Зверніть увагу на зовнішній ключ MakeId і перевірку обмеження FK_Cars_Makes_MakeId, створені в залежній таблиці (Cars).

#### Відносини One-to-One

У зв’язках «One-to-One» обидві сутності мають посилання на навігаційну властивість для іншої сутності. Під час побудови зв’язків «One-to-One» EF Core має знати, яка сторона є головною сутністю. Це можна зробити, маючи чітко визначений зовнішній ключ до головної сутності або вказавши принципала за допомогою Fluent API. Якщо EF Core не отримує інформацію за допомогою одного з цих двох методів, він вибере один на основі своєї здатності виявляти зовнішній ключ. На практиці ви повинні чітко визначити залежну, додавши властивості зовнішнього ключа. Це усуває будь-яку неоднозначність і гарантує, що ваші таблиці правильно налаштовані.

Додайте новий клас під назвою Radio.cs.

```cs
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

Коли базу даних оновлено за допомогою таких міграцій EF Core, таблиця Cars не змінюється, і створюється така таблиця Radios:

```console
dotnet ef migrations add Radio -o Migrations -c AutoLot.Samples.ApplicationDbContext
dotnet ef database update Radio  -c AutoLot.Samples.ApplicationDbContext
```
Виконається наступний запит:
```sql
CREATE TABLE [dbo].[Radios](
  [Id] [int] IDENTITY(1,1) NOT NULL,
  [HasTweeters] [bit] NOT NULL,
  [HasSubWoofers] [bit] NOT NULL,
  [RadioId] [nvarchar](max) NULL,
  [TimeStamp] [varbinary](max) NULL,
  [CarId] [int] NOT NULL,
 CONSTRAINT [PK_Radios] PRIMARY KEY CLUSTERED
(
        [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Radios]  WITH CHECK ADD  CONSTRAINT [FK_Radios_Cars_CarId] FOREIGN KEY([CarId])
REFERENCES [dbo].[Cars] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Radios] CHECK CONSTRAINT [FK_Radios_Cars_CarId]
GO
```
Зверніть увагу на зовнішній ключ і обмеження перевірки, створені в залежній таблиці Radios.

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

Щоб оновити базу даних, використовуйте такі команди міграції (знову ж таки, міграції будуть повністю пояснені далі в цьому розділі):

```console
dotnet ef migrations add Drivers -o Migrations -c AutoLot.Samples.ApplicationDbContext
dotnet ef database update Drivers  -c AutoLot.Samples.ApplicationDbContext
```
Коли база даних оновлюється, таблиця Cars не змінюється, а таблиці Drivers і CarDriver створюються. Настуаний запит виконається:

```sql
CREATE TABLE [dbo].[Drivers](
  [Id] [INT] IDENTITY(1,1) NOT NULL,
  [FirstName] [NVARCHAR](MAX) NULL,
  [LastName] [NVARCHAR](MAX) NULL,
  [TimeStamp] [VARBINARY](MAX) NULL,
 CONSTRAINT [PK_Drivers] PRIMARY KEY CLUSTERED
(
  [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[CarDriver](
  [CarsId] [int] NOT NULL,
  [DriversId] [int] NOT NULL,
 CONSTRAINT [PK_CarDriver] PRIMARY KEY CLUSTERED
(
  [CarsId] ASC,
  [DriversId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CarDriver]  WITH CHECK ADD  CONSTRAINT [FK_CarDriver_Cars_CarsId]
  FOREIGN KEY([CarsId]) REFERENCES [dbo].[Cars] ([Id]) ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CarDriver] CHECK CONSTRAINT [FK_CarDriver_Cars_CarsId]
GO
ALTER TABLE [dbo].[CarDriver]  WITH CHECK ADD  CONSTRAINT [FK_CarDriver_Drivers_DriversId]
  FOREIGN KEY([DriversId]) REFERENCES [dbo].[Drivers] ([Id]) ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CarDriver] CHECK CONSTRAINT [FK_CarDriver_Drivers_DriversId]
GO
```
Зверніть увагу, що складений первинний ключ, обмеження перевірки (зовнішні ключі) і каскадна поведінка створені EF Core, щоб переконатися, що таблицю CarDriver налаштовано як правильну таблицю об’єднання.

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
| Primary key| Властивості з назвою Id або <EntityTypeName>Id буде налаштовано як первинний ключ. Ключі типу short, int, long або Guid мають значення, які контролюються сховищем даних. Числові значення створюються як стовпці Identity (SQL Server).|
| Relationships | Зв’язки між таблицями створюються, коли між двома класами сутностей є властивості навігації.|
| Foreign key| Властивості під назвою <OtherClassName>Id є зовнішніми ключами для властивостей навігації типу <OtherClassName>.|
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

### Перевизначення основних конвенцій EF

Конвенції можна змінити за допомогою методу ConfigureConventions(). Наприклад, якщо ви хочете, щоб властивості рядка за умовчанням мали певний розмір (замість nvarchar(max)), ви можете додати такий код до класу ApplicationDbContext:

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

Ось клас Car і анотації даних, які формують його в базі даних:

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

    Зміна стовпця TimeStamp з varbinary(max) на тип даних timestamp SQL Server.

    Встановлення нульового значення для стовпців Color і PetName з Null на Not Null.

    Явне встановлення розміру стовпців Color і PetName на nvarchar(50). Це вже було оброблено, коли конвенції EF Core для властивостей рядка були перевизначені, але включені тут для видимості.

    Перейменування індексу стовпця MakeId.

Решта використовуваних анотацій відповідає конфігурації, визначеній угодами EF Core. Щоб підтвердити зміни, ми перевіряємо таблицю, створену EF Core:

```sql
CREATE TABLE [dbo].[Inventory](
        [Id] [INT] IDENTITY(1,1) NOT NULL,
        [Color] [NVARCHAR](50) NOT NULL,
        [PetName] [NVARCHAR](50) NOT NULL,
        [MakeId] [INT] NOT NULL,
        [TimeStamp] [TIMESTAMP] NULL,
 CONSTRAINT [PK_Inventory] PRIMARY KEY CLUSTERED
(
        [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Inventory] ADD  DEFAULT (N'') FOR [Color]
GO
ALTER TABLE [dbo].[Inventory] ADD  DEFAULT (N'') FOR [PetName]
GO
ALTER TABLE [dbo].[Inventory]  WITH CHECK ADD  CONSTRAINT [FK_Inventory_Makes_MakeId]
  FOREIGN KEY([MakeId]) REFERENCES [dbo].[Makes] ([Id]) ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Inventory] CHECK CONSTRAINT [FK_Inventory_Makes_MakeId]
GO
```
Якщо ви стежили за кожною з цих змін і запускали міграції, ви можете бути здивовані, побачивши помилку під час оновлення стовпця TimeStamp до типу даних timestamp SQL Server. Це пояснюється тим, що SQL Server не дозволяє змінювати тип даних існуючого стовпця на тип даних із міткою часу з іншого типу даних. Стовпець потрібно видалити, а потім знову додати з новим типом даних позначки часу. EF Core бачить стовпець як уже існуючий і видає оператор alter, а не парні команди drop/add, які потрібні для внесення змін. Щоб оновити базу даних, закоментуйте властивість TimeStamp у базовому класі, створіть нову міграцію та застосуйте її, а потім розкоментуйте властивість TimeStamp і створіть іншу міграцію та застосуйте її.

Зверніть увагу на значення за замовчуванням, додані до стовпців Color і PetName. Якщо будь-які дані мали нульові значення для будь-якого з цих стовпців, це спричинило б помилку міграції. Ця зміна гарантує, що зміна на ненульове буде успішною шляхом розміщення порожнього рядка в цих стовпцях, якщо вони були нульовими під час застосування міграції.

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

Команди міграції та результуюча таблиця показані тут:

```console
dotnet ef migrations add UpdateRadio -o Migrations -c AutoLot.Samples.ApplicationDbContext
dotnet ef database update UpdateRadio  -c AutoLot.Samples.ApplicationDbContext
```
```sql
CREATE TABLE [dbo].[Radios](
        [Id] [INT] IDENTITY(1,1) NOT NULL,
        [HasTweeters] [BIT] NOT NULL,
        [HasSubWoofers] [BIT] NOT NULL,
        [RadioId] [NVARCHAR](50) NOT NULL,
        [InventoryId] [INT] NOT NULL,
        [TimeStamp] [TIMESTAMP] NULL,
 CONSTRAINT [PK_Radios] PRIMARY KEY CLUSTERED
(
        [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Radios] ADD  DEFAULT (N'') FOR [RadioId]
GO
ALTER TABLE [dbo].[Radios]  WITH CHECK ADD  CONSTRAINT [FK_Radios_Inventory_InventoryId]
  FOREIGN KEY([InventoryId]) REFERENCES [dbo].[Inventory] ([Id]) ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Radios] CHECK CONSTRAINT [FK_Radios_Inventory_InventoryId]
GO
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
                entity.ToTable("Invrntory", "dbo");
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
                entity.Property(e => e.CarId).HasColumnName("InvertoryId");            
            });

        }
```
Властивість CarId класу Radio відповідає стовпцю InventoryId таблиці Radios.

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
                entity.HasIndex(e => e.MakeId, "IX_Inventory_MakeId").IsUnique();
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

    public DateTime? DateBuild { get; set; }
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

Проблема виникає, коли логічне або числове значення має значення бази даних за замовчуванням, яке суперечить значенню за замовчуванням CLR. Наприклад, якщо для логічної властивості (наприклад, IsDrivable) у базі даних за замовчуванням встановлено значення true, база даних установить значення true під час вставлення запису, якщо значення не вказано для цього стовпця. Це, звісно, ​​очікувана поведінка з боку бази даних. Однак значення CLR за замовчуванням для логічних властивостей є false, що спричиняє проблему через те, як EF Core обробляє значення за замовчуванням. 
Наприклад, додайте властивість Bool під назвою IsDrivable до класу Car. Якщо ви слідкуєте за цим в БД, переконайтеся, що створили та застосували нову міграцію для оновлення бази даних.

```cs
public class Car : BaseEntity
{
   //...
    public bool IsDrivable { get; set; }
}
```

Давайте розглянемо поведінку EF Core для типу даних Boolean.

Скористайтеся наведеним нижче кодом, щоб створити новий запис про автомобіль із значенням false для IsDrivable:

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
Одне з рішень для цього полягає в тому, щоб зробити вашу загальнодоступну властивість (і, отже, стовпець) нульовою, оскільки значення за замовчуванням для типу значення nullable дорівнює null, тому встановлення властивості Boolean на false працює належним чином. Однак зміна нульовості властивості може не відповідати потребам бізнесу.

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

Нарешті, попередження все одно з’являтиметься, навіть якщо поля належним чином налаштовано з резервними полями з можливістю нульових значень. Попередження можна придушити; однак я рекомендую залишити його на місці як нагадування перевірити, чи правильно налаштовано поле/властивість. Якщо ви хочете придушити його, установіть такий параметр у DbContextOptions:

```cs
options.ConfigureWarnings(wc => wc.Ignore(RelationalEventId.BoolWithDefaultWarning));
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

#### Перевірка обмежень

Перевірка обмежень — це функція SQL Server, яка визначає умову для рядка, яка має бути істинною. Наприклад, у системі електронної комерції можна додати обмеження перевірки, щоб переконатися, що кількість є більшою за нуль або що ціна вища за ціну зі знижкою. Ми створимо надумане обмеження, яке запобігає використанню назви «Lemon» у таблиці Makes.
Додайте наступне до методу OnModelCreating() у класі ApplicationDbContext, який створює обмеження перевірки, що запобігає назві «Lemon» у таблиці Makes

```cs

            modelBuilder.Entity<Make>()
               .HasCheckConstraint("CH_Name", "[Name]<>'Lemon'", c => c.HasName("CK_Check_Name"));

```
Перший параметр дає обмеженню назву в моделі, другий — це SQL для обмеження, а останній призначає ім’я SQL Server для обмеження перевірки. Ось обмеження перевірки, як визначено в SQL:

```sql
ALTER TABLE [dbo].[Makes]  WITH CHECK ADD  CONSTRAINT [CK_Check_Name] CHECK  (([Name]<>'Lemon'))
```
Тепер, коли до таблиці додається запис із іменем «Лимон», буде створено виняток SQL. Виконайте наступний код, щоб побачити виняток у дії:

```cs
var context = new ApplicationDbContextFactory().CreateDbContext(null);
context.Makes.Add(new Make { Name = 'Lemon' });
context.SaveChanges();
```
Це викликає такий виняток:

```console
The INSERT statement conflicted with the CHECK constraint 'CK_Check_Name'. The conflict occurred in database 'AutoLotSamples', table 'dbo.Makes', column 'Name'.
```
Можете скасувати міграцію для перевірочного обмеження та видалити міграцію, оскільки решта книги не використовує перевірочне обмеження. Його було додано в цей розділ з метою демонстрації.

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

#### Відносини «One-to-One».

Зв’язки «один-до-одного» налаштовуються так само, за винятком того, що замість WithMany() використовується метод Fluent API WithOne(). Крім того, для залежної сутності потрібен унікальний індекс, який буде створено автоматично, якщо його не визначено. У наступному прикладі явно створюється унікальний індекс для визначення імені. Ось код зв’язку між об’єктами Car і Radio за допомогою залежного об’єкта (Radio)

```cs
            modelBuilder.Entity<Radio>(entity => 
            {
                //...

                entity.HasIndex(e => e.CarId, "IX_Radios_CarId");

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

               entity.HasIndex(r => r.CarId, "IX_Radios_CarId")
               .IsUnique();
           });           
```

#### Відносини «Many-to-Many»

Зв’язки «Many-to-Many» набагато краще налаштовуються за допомогою Fluent API. Імена полів зовнішнього ключа, імена індексів і каскадна поведінка можуть бути встановлені в операторах, які визначають зв’язок. Це також дозволяє безпосередньо вказувати зведену таблицю, що дозволяє додавати додаткові поля та спрощувати запити.

Почніть із додавання сутності CarDriver:

```cs
namespace AutoLot.Samples.Models;

[Table("InventoryToDrivers",Schema ="dbo")]
internal class CarDriver : BaseEntity
{
    public int DriverId { get; set; }
    [ForeignKey(nameof(DriverId))]
    public Driver DriverNavigation { get; set; }

    [Column("InvertoryId")]
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

#### Виключення сутностей із міграцій

Якщо сутність спільно використовується кількома DbContexts, кожен DbContext створить код у файлах міграції для створення або зміни цієї сутності. Це спричиняє проблему, оскільки другий сценарій міграції не вдасться виконати, якщо зміни вже є в базі даних. 
DbContext може позначити сутність як виключену з міграцій, дозволяючи іншому DbContext стати системою запису для цієї сутності. Наступний код показує, що сутність виключена з міграцій:

```cs
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
  modelBuilder.Entity<LogEntry>().ToTable('Logs', t => t.ExcludeFromMigrations());
}
```