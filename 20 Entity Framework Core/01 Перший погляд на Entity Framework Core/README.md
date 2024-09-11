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

## Складові Entity Framework

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

### Клас DbContext

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

### Побудова похідного касу від DBContext

#### Створення класу

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

#### Конфігурування DbContext

Екземпляр DbContext налаштовується за допомогою екземпляра класу DbContextOptions. Екземпляр DbContextOptions створюється за допомогою DbContextOptionsBuilder, оскільки клас DbContextOptions не призначений для безпосереднього створення у вашому коді. За допомогою екземпляра DbContextOptionsBuilder вибирається постачальник бази даних (разом із будь-якими параметрами, що стосуються постачальника), і встановлюються загальні параметри EF Core DbContext (наприклад, журналювання). Потім екземпляр DbContextOptions вставляється в базовий DbContext під час виконання.
Ця можливість динамічної конфігурації дозволяє змінювати налаштування під час виконання, просто вибираючи різні параметри (наприклад, MySQL замість постачальника SQL Server) і створюючи новий екземпляр вашого похідного DbContext.

#### DbContext Factory під час розробки

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

#### OnModelCreating

Базовий клас DbContext надає метод OnModelCreating, який використовується для формування ваших сутностей за допомогою Fluent API. Це буде детально розглянуто пізніше в цьому розділі, а поки додайте наступний код до класу ApplicationDbContext

```cs
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent API calls go here / Сюди надходять виклики Fluent API
        }
```

#### Збереження змін

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

#### Підтримка транзакцій і точки збереження

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

#### Явні транзакції та стратегії виконання

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

#### Події SavingChanges, SavedChanges, SaveChangesFailed.

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

#### Клас DbSet<T>

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

#### ChangeTracker

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

#### Події ChangeTracker

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

#### StateChanged

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

#### Tracked

Подія Tracked запускається, коли ChangeTracker починає відстежувати сутність. У наступному прикладі виконується запис на консоль кожного разу, коли сутність завантажується з бази даних.

```cs

```
Властивість FromQuery EntityTrackedEventArgs вказує, чи була сутність завантажена через запит бази даних чи програмно.

#### Скидання стану DbContext

Існує можливість скинути похідний DbContext назад до початкового стану. Метод ChangeTracker.Clear() видаляє всі сутності з колекцій DbSet<T>, установлюючи для них стан Detached. Основною перевагою цього є підвищення продуктивності. Як і з будь-яким ORM, створення екземпляра похідного класу DbContext вимагає певних витрат. Хоча ці накладні витрати зазвичай не є значними, можливість очистити вже створений контекст може допомогти підвищити продуктивність у деяких сценаріях.

### Сутності

Строго типізовані класи, які відображаються в таблицях бази даних, офіційно називаються сутностями. Набір сутностей у програмі містить концептуальну модель фізичної бази даних. Формально кажучи, ця модель називається моделлю даних сутності (entity data model EDM ), яку зазвичай називають просто моделлю. Модель зіставляється з доменом програми/бізнесу. Сутності та їхні властивості зіставляються з таблицями та стовпцями за допомогою угод Entity Framework Core, конфігурації та Fluent API (код). Сутності не потребують прямого зіставлення зі схемою бази даних. Ви можете структурувати свої класи сутностей відповідно до потреб програми, а потім зіставляти унікальні сутності зі схемою бази даних.
Цей слабкий зв’язок між базою даних і вашими об’єктами означає, що ви можете формувати об’єкти відповідно до домену вашого бізнесу, незалежно від конструкції та структури бази даних. Наприклад, візьмемо просту таблицю Inventory в базі даних AutoLot і клас сутності Car з попереднього розділу.Назви різні, але сутність Car можна зіставити з таблицею  Inventory. EF Core перевіряє конфігурацію ваших сутностей у моделі, щоб зіставити представлення клієнтської сторони таблиці Inventory (у нашому прикладі класу Car) на правильні стовпці таблиці Inventory. 
У наступних кількох розділах детально описано, як угоди EF Core, анотації даних і код (за допомогою Fluent API) відображають сутності, властивості та зв’язки між сутностями в режимі на таблиці, стовпці та зв’язки зовнішнього ключа у вашій базі даних.

#### Властивості сутності та стовпці бази даних

Під час використання реляційного сховища даних EF Core використовує дані зі стовпців таблиці для заповнення властивостей сутності під час читання зі сховища даних і записує властивості сутності в стовпці таблиці під час збереження даних. Якщо властивість є автоматичною властивістю, EF Core читає та записує через getter і setter. Якщо властивість має опорне поле, EF Core читатиме та записуватиме в резервне поле замість публічної власності, навіть якщо опорне поле є приватним. Хоча EF Core може читати та записувати в приватні поля, все одно має бути загальнодоступна властивість читання та запису, яка інкапсулює резервне поле. Два сценарії, коли підтримка резервного поля є перевагою, це використання шаблону INotifyPropertyChanged у програмах Windows Presentation Foundation (WPF) і коли значення за замовчуванням бази даних суперечать значенням за замовчуванням .NET.

#### Схеми відображення таблиць

У EF Core доступні дві схеми відображення класів у таблиці: таблиця за ієрархією (table-per-hierarchy TPH) і таблиця за типом (table-per-type TPT). Відображення TPH є типовим і відображає ієрархію успадкування в одній таблиці. TPT відображає кожен клас в ієрархії на окрему таблицю. 
Класи також можуть бути зіставлені з представленнями та необробленими запитами SQL.

#### Відображення таблиці на ієрархію (Table-Per-Hierarchy)

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

#### Відображення таблиці за типом (Table-per-Type TPT)

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


