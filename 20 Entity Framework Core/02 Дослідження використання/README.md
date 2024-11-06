# Дослідження використання

Скопіюемо код AutoLot.Samples з попередньої глави. Видалемо папки Migrations, CompiledModels. Відкриемо рішення. Видалимо зайви комантарі в класі ApplicationDbContext в методі OnModelCreating

```cs
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    new CarConfiguration().Configure(modelBuilder.Entity<Car>());
    new RadioConfiguration().Configure(modelBuilder.Entity<Radio>());
    new DriverConfiguration().Configure(modelBuilder.Entity<Driver>());
    new CarMakeViewModelConfiguration().Configure(modelBuilder.Entity<CarMakeViewModel>());
}
```

Додамо міграцію і оновимо базу.

```console
dotnet ef migrations add Initial
dotnet ef database drop
dotnet ef database update
```
Таким чином у нас є сутності і їх відображення в БД для досліджень.

## Додавання записів

Записи додаються до бази даних шляхом їх створення в коді, додавання до їхнього DbSet<T> і виклику SaveChanges()/SaveChangesAsync() у контексті. Коли виконується SaveChanges(), ChangeTracker повідомляє про всі додані об’єкти, а EF Core (разом із постачальником бази даних) створює відповідний оператор(и) SQL для вставлення запису(ів).

SaveChanges() виконується в неявній транзакції, якщо не використовується явна транзакція. Якщо збереження відбулося успішно, згенеровані сервером значення запитуються для встановлення значень для сутностей.

Записи також можна додавати за допомогою похідного DbContext. Усі ці приклади використовуватимуть властивості колекції DbSet<T> для додавання записів. І DbSet<T>, і DbContext мають асинхронні версії Add()/AddRange(). Показані лише синхронні версії.

### Додавання одного запису і стан сутності.

Коли сутність створюється за допомогою коду, але ще не додається до DbSet<T>, EntityState є  Detached(відокремлений). Після додавання нової сутності до DbSet<T> EntityState встановлюється на Added. Після успішного виконання SaveChanges() EntityState встановлюється на Unchanged.

```cs
static void AddRecords()
{
    //The factory is not meant to be used like this, but it's demo code
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    
    Make newMake = new Make { Name ="BMW" };
    Console.WriteLine($"State of the entity is {context.Entry(newMake).State}");

    context.Makes.Add(newMake);
    Console.WriteLine($"State of the entity is {context.Entry(newMake).State}");

    
    ViewMake(newMake,"Bifore SaveChange");
    context.SaveChanges();
    Console.WriteLine($"State of the entity is {context.Entry(newMake).State}");
    ViewMake(newMake, "After SaveChange");
}
AddRecords();

static void ViewMake(Make make,string text)
{
    Console.WriteLine($"\t{text}");
    Console.WriteLine($"\tId:{make.Id}");
    Console.WriteLine($"\tName:{make.Name}");
}
```
```console
State of the entity is Detached
State of the entity is Added
        Bifore SaveChange
        Id:0
        Name:VW
Saving change for Server=(localdb)\mssqllocaldb;Database=AutoLotSamples;Trusted_Connection=True;ConnectRetryCount=0
Saved change 1 entities
State of the entity is Unchanged
        After SaveChange
        Id:6
        Name:VW
```
Щоб додати новий запис Make до бази даних, створіть новий екземпляр сутності та викличте метод Add() відповідного DbSet<T>. Щоб ініціювати збереження даних, необхідно також викликати SaveChanges() похідного класу DbContext.
Після того, як сутність було додано до засобу відстеження змін (за допомогою методу Add()), стан було змінено на Added. Повідомлення про збереження змін надходить від обробника подій SavingChanges, а повідомлення «Saved 1 entities» — від обробника подій SavedChanges. Після виклику SaveChanges() у контексті стан сутності змінюється на Unchanged.
На сервері буде виконано запит SQL.

```sql
exec sp_executesql N'SET NOCOUNT ON;
INSERT INTO [Makes] ([Name]) VALUES (@p0);
SELECT [Id], [TimeStamp]
FROM [Makes]
WHERE @@ROWCOUNT = 1 AND [Id] = scope_identity();
',N'@p0 nvarchar(50)',@p0=N'VW'
```
Формат запиту пов’язаний із процесом пакетування, який використовується EF Core для покращення продуктивності операцій з базою даних. Усі значення, передані в інструкцію SQL, параметризовані, щоб зменшити загрозу сценарних атак. Також зауважте, що нещодавно додана сутність запитується щодо властивостей, згенерованих базою даних (як можна здогадатись для заповненя властивостей сутності в пам'яті).

### Додавання нового запису за допомогою Attach

Коли первинний ключ сутності зіставляється зі стовпцем ідентифікації в SQL Server, EF Core розглядатиме цей екземпляр сутності як Доданий під час додавання до ChangeTracker, якщо значення властивості первинного ключа дорівнює нулю.

```cs
static void AddRecordsWithAttach()
{
    //The factory is not meant to be used like this, but it's demo code
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    Make newMake = new Make { Name = "BMW" };
    context.Makes.Add(newMake);
    context.SaveChanges();

    Car newCar = new Car
    {
        Color = "Blue",
        DateBuilt = new DateTime(2012, 12, 01),
        IsDrivable = true,
        PetName = "Bluesmobile",
        MakeId = newMake.Id
    };

    Console.WriteLine($"State of the {newCar.PetName} is {context.Entry(newCar).State}");
    context.Cars.Attach(newCar);
    Console.WriteLine($"State of the {newCar.PetName} is {context.Entry(newCar).State}");
    context.SaveChanges();
    Console.WriteLine($"State of the {newCar.PetName} is {context.Entry(newCar).State}");
}
AddRecordsWithAttach();
```
Наступний код додає новий запис Car за допомогою методу Attach() замість методу Add(). Зауважте, що SaveChanges() все одно має бути викликано для збереження даних.

```console
Saving change for Server=(localdb)\mssqllocaldb;Database=AutoLotSamples;Trusted_Connection=True;ConnectRetryCount=0
Saved change 1 entities
State of the Bluesmobile is Detached
State of the Bluesmobile is Added
Saving change for Server=(localdb)\mssqllocaldb;Database=AutoLotSamples;Trusted_Connection=True;ConnectRetryCount=0
Saved change 1 entities
State of the Bluesmobile is Unchanged
```
Ми бачимо таку саму послідовність станів, яку бачили з сутністю Make.

Буде виконаний оператор SQL для вставки такого типу

```sql
exec sp_executesql N'SET NOCOUNT ON;
INSERT INTO [dbo].[Inventory] ([Color], [DateBuilt], [IsDrivable], [MakeId], [PetName])
VALUES (@p0, @p1, @p2, @p3, @p4);

SELECT [Id], [Display], [TimeStamp]
FROM [dbo].[Inventory]

WHERE @@ROWCOUNT = 1 AND [Id] = scope_identity();
',N'@p0 nvarchar(50),@p1 datetime2(7),@p2 bit,@p3 int,@p4 nvarchar(50)',@p0=N'Blue',@p1='2016-12-01 00:00:00',@p2=1,@p3=1,@p4=N'Bluesmobile'
```
Щоб розглянути наступний приклад можна очистити БД видаливши її і створивши.

```console
dotnet ef database drop
dotnet ef database update
```

### Додавання кількох записів одночасно

Щоб вставити кілька записів в одну транзакцію, використовуйте метод AddRange() властивості DbSet<T>

```cs
static void AddMultipleRecords()
{
    //The factory is not meant to be used like this, but it's demo code
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    Make newMake = new Make { Name = "BMW" };
    context.Makes.Add(newMake);
    context.SaveChanges();

    var cars = new List<Car>
    {
        new() { Color = "Yellow", MakeId = newMake.Id, PetName = "Herbie" },
        new() { Color = "White", MakeId = newMake.Id, PetName = "Mach 5" },
        new() { Color = "Pink", MakeId = newMake.Id, PetName = "Avon" },
        new() { Color = "Blue", MakeId = newMake.Id, PetName = "Blueberry" },
    };
    context.Cars.AddRange(cars);
    context.SaveChanges();
}
AddMultipleRecords();

```
```console
Saving change for Server=(localdb)\mssqllocaldb;Database=AutoLotSamples;Trusted_Connection=True;ConnectRetryCount=0
Saved change 1 entities
Saving change for Server=(localdb)\mssqllocaldb;Database=AutoLotSamples;Trusted_Connection=True;ConnectRetryCount=0
Saved change 4 entities
```
Незважаючи на те, що було додано чотири записи, EF Core створив лише один оператор SQL для вставок.
Інструкція SQL для вставок показана тут:
```sql
exec sp_executesql N'SET NOCOUNT ON;
DECLARE @inserted0 TABLE ([Id] int, [_Position] [int]);
MERGE [dbo].[Inventory] USING (
VALUES (@p0, @p1, @p2, 0),
(@p3, @p4, @p5, 1),
(@p6, @p7, @p8, 2),
(@p9, @p10, @p11, 3)) AS i ([Color], [MakeId], [PetName], _Position) ON 1=0
WHEN NOT MATCHED THEN
INSERT ([Color], [MakeId], [PetName])
VALUES (i.[Color], i.[MakeId], i.[PetName])
OUTPUT INSERTED.[Id], i._Position
INTO @inserted0;
SELECT [t].[Id], [t].[DateBuilt], [t].[Display], [t].[IsDrivable], [t].[TimeStamp] FROM [dbo].[Inventory] t
INNER JOIN @inserted0 i ON ([t].[Id] = [i].[Id])
ORDER BY [i].[_Position];
',N'@p0 nvarchar(50),@p1 int,@p2 nvarchar(50),@p3 nvarchar(50),@p4 int,@p5 nvarchar(50),@p6 nvarchar(50),@p7 int,@p8 nvarchar(50),@p9 nvarchar(50),@p10 int,@p11 nvarchar(50)',@p0=N'Yellow',@p1=1,@p2=N'Herbie',@p3=N'White',@p4=1,@p5=N'Mach 5',@p6=N'Pink',@p7=1,@p8=N'Avon',@p9=N'Blue',@p10=1,@p11=N'Blueberry'
```

### Cтовпець ідентифікації під час додавання записів

Якщо сутність має числову властивість, визначену як первинний ключ, ця властивість (за замовчуванням) зіставляється зі стовпцем Identity у SQL Server. EF Core вважає будь-яку сутність зі значенням за замовчуванням (нуль) для властивості ключа новою, а будь-яку сутність зі значенням, відмінним від за замовчуванням, уже існує в базі даних. Якщо ви створюєте нову сутність і встановлюєте для властивості первинного ключа значення, відмінне від нуля, і намагаєтесь додати її до бази даних, EF Core не зможе додати запис, оскільки вставку ідентифікаційної інформації не ввімкнено на стороні сервера.
Для SQL Server вставлення ідентичності вмикається шляхом виконання команди SET IDENTITY_INSERT у явній транзакції. Для цієї команди потрібні схема бази даних і ім’я таблиці.

```cs
static void GetSchemaAndTableNameForType()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    IEntityType metadata = context.Model.FindEntityType(typeof(Car).FullName);
    var schema = metadata.GetSchema();
    var tableName = metadata.GetTableName();
    Console.WriteLine($"{schema} {tableName}");
}
//GetSchemaAndTableNameForType();
```
```console
dbo Inventory
```
При використанні ExecutionStrategy явні транзакції повинні виконуватися в межах цієї стратегії наступним чином:

```cs
var strategy = context.Database.CreateExecutionStrategy();
strategy.Execute(() =>
{
  using var trans = context.Database.BeginTransaction();
  try
  {
    //actionToExecute();
  trans.Commit();
  Console.WriteLine($'Insert succeeded');
  }
  catch (Exception ex)
  {
    trans.Rollback();
    Console.WriteLine($'Insert failed: {ex.Message}');
  }
});
```
Створимо метод що виконує в стратегії явну транзакцію і вмикає в ній дадавання необідних значень до індіфікаційних даних.

```cs
static void AddRowWithSetIdentityInsert()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    // Definition schema and tablename    
    IEntityType metadata = context.Model.FindEntityType(typeof(Car).FullName);
    string schema = metadata.GetSchema();
    string tableName = metadata.GetTableName();
    
    //Cteate strategy with explecitly transaction
    string sql;
    var strategy = context.Database.CreateExecutionStrategy();
    strategy.Execute( () => 
    {
        using var transaction = context.Database.BeginTransaction();
        try
        {   //Settings on server
            sql = $"SET IDENTITY_INSERT {schema}.{tableName} ON";
            context.Database.ExecuteSqlRaw(sql);
            
            // Insert row
            Car car = new Car
            {
                Id = 27,
                Color = "Blue",
                DateBuilt = new DateTime(2012, 12, 01),
                IsDrivable = true,
                PetName = "Bluesmobile",
                MakeId = 1
            };
            context.Cars.Add(car);
            context.SaveChanges();
            // Insert row
            transaction.Commit();
            Console.WriteLine("Insert succeeded");

        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Console.WriteLine($"Insert failed:{ex.Message}");
        }
        finally
        {
            //Settings on server
            sql = $"SET IDENTITY_INSERT {schema}.{tableName} OFF";
            context.Database.ExecuteSqlRaw(sql);
        }
    });
}
AddRowWithSetIdentityInsert();
```
```console
Saving change for Server=(localdb)\mssqllocaldb;Database=AutoLotSamples;Trusted_Connection=True;ConnectRetryCount=0
Saved change 1 entities
Insert succeeded
```
Якщо все проходить успішно, транзакцію зафіксовано. Якщо будь-яка його частина виходить з ладу, транзакція відкочується. У блоці finally вставку ідентифікаційної інформації вимкнуто.
EF Core надає два методи виконання команд безпосередньо в базі даних. Метод ExecuteSqlRaw() виконує рядок точно так, як він написаний, тоді як ExecuteSqlInterpolated() використовує інтерполяцію рядка C# для створення параметризованого запиту. Якщо використовуються відомі значення, як у цьому прикладі, метод ExecuteSqlRaw() є безпечним для використання. Однак, якщо ви збираєте вхідні дані від користувачів, вам слід використовувати версію ExecuteSqlInterpolated() для додаткового захисту.

Попередній код виконував наступні команди для бази даних:
```sql
SET IDENTITY_INSERT dbo.Inventory ON
SAVE TRANSACTION [__EFSavePoint];
exec sp_executesql N'SET NOCOUNT ON;
INSERT INTO [dbo].[Inventory] ([Id], [Color], [DateBuilt], [IsDrivable], [MakeId], [PetName])
VALUES (@p0, @p1, @p2, @p3, @p4, @p5);
SELECT [Display], [TimeStamp]
FROM [dbo].[Inventory]
WHERE @@ROWCOUNT = 1 AND [Id] = @p0;
',N'@p0 int,@p1 nvarchar(50),@p2 datetime2(7),@p3 bit,@p4 int,@p5 nvarchar(50)',@p0=27,@p1=N'Blue',@p2='2016-12-01 00:00:00',@p3=1,@p4=1,@p5=N'Bluesmobile'
SET IDENTITY_INSERT dbo.Inventory OFF
```

### Додавання графа об’єктів

Під час додавання сутності до бази даних дочірні записи можна додавати в тому самому виклику без спеціального додавання їх у власний DbSet<T>. Це досягається додаванням їх до властивості навігації колекції для батьківського запису.

```cs
static void AddEntityWithChild()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    
    var make = new Make { Name = "Honda" };

    Car car = new Car { Color = "Yellow", PetName = "Herbie" };
    // IEnumerable<Car> to List<Car>
    ((List<Car>)make.Cars).Add(car);

    context.Makes.Add(make);
    context.SaveChanges();
}
AddEntityWithChild();
```
```console
Saving change for Server=(localdb)\mssqllocaldb;Database=AutoLotSamples;Trusted_Connection=True;ConnectRetryCount=0
Saved change 2 entities
```
Створюється нова сутність Make, а дочірній запис Car додається до властивості Cars у Make. Коли сутність Make додається до властивості DbSet<Make>, EF Core також автоматично починає відстежувати дочірній запис Car, не додаючи його до властивості DbSet<Car> явно. Виконання SaveChanges() зберігає Make та Car разом.

```sql
exec sp_executesql N'SET NOCOUNT ON;
INSERT INTO [Makes] ([Name])
VALUES (@p0);
SELECT [Id], [TimeStamp]
FROM [Makes]
WHERE @@ROWCOUNT = 1 AND [Id] = scope_identity();
',N'@p0 nvarchar(50)',@p0=N'Honda'
exec sp_executesql N'SET NOCOUNT ON;
INSERT INTO [dbo].[Inventory] ([Color], [MakeId], [PetName])
VALUES (@p1, @p2, @p3);
SELECT [Id], [DateBuilt], [Display], [IsDrivable], [TimeStamp]
FROM [dbo].[Inventory]
WHERE @@ROWCOUNT = 1 AND [Id] = scope_identity();
',N'@p1 nvarchar(50),@p2 int,@p3 nvarchar(50)',@p1=N'Yellow',@p2=2,@p3=N'Herbie'
```
Зверніть увагу, як EF Core отримав ідентифікатор для нового запису Make і автоматично включив його в оператор вставки для запису Car.

### Додавання записів в таблиці з відношенням many-to-many

Для таблиць «many-to-many» записи можна додавати безпосередньо від однієї сутності до іншої, не переходячи через зведену таблицю. Тепер ви можете написати такий код, щоб додати записи Driver безпосередньо до записів Car:
```cs
static void AddRecordsToMantToManyTables()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    List<Driver> drivers = new List<Driver>
    {
        new() { PersonInfo = new Person { FirstName = "Fred", LastName = "Flinstone" } },
        new() { PersonInfo = new Person { FirstName = "Wilma", LastName = "Flinstone" } },
        new() { PersonInfo = new Person { FirstName = "BamBam", LastName = "Flinstone" } },
        new() { PersonInfo = new Person { FirstName = "Barney", LastName = "Rubble" } },
        new() { PersonInfo = new Person { FirstName = "Betty", LastName = "Rubble" } },
        new() { PersonInfo = new Person { FirstName = "Pebbles", LastName = "Rubble" } }
    };

    var cars = context.Cars.Take(2).ToList();

    //Cast the IEnumerable to a List to access the Add method
    //Range support works with LINQ to Objects, but is not translatable to SQL calls
    ((List<Driver>)cars[0].Drivers).AddRange(drivers.Take(..3));
    ((List<Driver>)cars[1].Drivers).AddRange(drivers.Take(3..));
    context.SaveChanges();
}
AddRecordsToMantToManyTables();
```
```console
An entity of type Car was loaded from the database.
An entity of type Car was loaded from the database.
Saving change for Server=(localdb)\mssqllocaldb;Database=AutoLotSamples;Trusted_Connection=True;ConnectRetryCount=0
Saved change 12 entities
```
Коли виконується метод SaveChanges(), виконуються два оператори вставки. Перший вставляє шість записів Driver у таблицю Drivers, а другий вставляє шість записів у таблицю InventoryDriver (зведену таблицю).

```sql
exec sp_executesql N'SET NOCOUNT ON;
DECLARE @inserted0 TABLE ([InventoryId] int, [DriverId] int, [_Position] [int]);
MERGE [dbo].[InventoryToDrivers] USING (
VALUES (@p12, @p13, 0),
(@p14, @p15, 1),
(@p16, @p17, 2),
(@p18, @p19, 3),
(@p20, @p21, 4),
(@p22, @p23, 5)) AS i ([InventoryId], [DriverId], _Position) ON 1=0
WHEN NOT MATCHED THEN
INSERT ([InventoryId], [DriverId])
VALUES (i.[InventoryId], i.[DriverId])
OUTPUT INSERTED.[InventoryId], INSERTED.[DriverId], i._Position
INTO @inserted0;
SELECT [t].[Id], [t].[TimeStamp] FROM [dbo].[InventoryToDrivers] t
INNER JOIN @inserted0 i ON ([t].[InventoryId] = [i].[InventoryId]) AND ([t].[DriverId] = [i].[DriverId])
ORDER BY [i].[_Position];
',N'@p12 int,@p13 int,@p14 int,@p15 int,@p16 int,@p17 int,@p18 int,@p19 int,@p20 int,@p21 int,@p22 int,@p23 int',@p12=1,@p13=1,@p14=1,@p15=2,@p16=1,@p17=3,@p18=2,@p19=4,@p20=2,@p21=5,@p22=2,@p23=6
```
Це набагато краще, ніж попередні версії EF Core, коли використовуються зв’язки «багато до багатьох», де вам доводилося самостійно керувати зведеною таблицею.

### Приклада додавання різних сутностей для подальшого дослідження запитів.

Додайте серію записів Make та Car для прикладів запитів на читання. 

```cs
static void LoadMakeAndCarData()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    List<Make> makes = new()
    {
        new() { Name = "VW" },
        new() { Name = "Ford" },
        new() { Name = "Saab" },
        new() { Name = "Yugo" },
        new() { Name = "BMW" },
        new() { Name = "Pinto" },

    };
    context.Makes.AddRange(makes);
    context.SaveChanges();

    List<Car> cars = new()
    {
        new() { MakeId = 1, Color = "Black", PetName = "Zippy" },
        new() { MakeId = 2, Color = "Rust", PetName = "Rusty" },
        new() { MakeId = 3, Color = "Black", PetName = "Mel" },
        new() { MakeId = 4, Color = "Yellow", PetName = "Clunker" },
        new() { MakeId = 5, Color = "Black", PetName = "Bimmer" },
        new() { MakeId = 5, Color = "Green", PetName = "Hank" },
        new() { MakeId = 5, Color = "Pink", PetName = "Pinky" },
        new() { MakeId = 6, Color = "Black", PetName = "Pete" },
        new() { MakeId = 4, Color = "Brown", PetName = "Brownie" },
        new() { MakeId = 1, Color = "Rust", PetName = "Lemon", IsDrivable = false },
    };

    context.Cars.AddRange(cars);
    context.SaveChanges();
}
LoadMakeAndCarData();
```
```console
Saving change for Server=(localdb)\mssqllocaldb;Database=AutoLotSamples;Trusted_Connection=True;ConnectRetryCount=0
Saved change 6 entities
Saving change for Server=(localdb)\mssqllocaldb;Database=AutoLotSamples;Trusted_Connection=True;ConnectRetryCount=0
Saved change 10 entities
```
Метод створює кілька сутностей марки та автомобіля та додає їх до бази даних. Сутності Make створюються та додаються до властивості Makes DbSet<Make> похідного ApplicationDbContext, а потім викликається метод SaveChanges(). Процес повторюеться  для записів Car, використовуючи властивість Cars DbSet<Car>.

### Приклад очистки таблиць БД.

Cтворимо метод, який очищає зразки даних, щоб, коли приклади запускаються кілька разів, попередні виконання не заважали прикладам. Створіть новий метод під назвою ClearSampleData().

```cs
static void ClearSampleData()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    string?[] entities = 
    {
        typeof(Driver).FullName,
        typeof(Car).FullName,
        typeof(Make).FullName,
    };

    foreach (var entityName in entities)
    {
        var entity = context.Model.FindEntityType(entityName);
        string? tableName = entity.GetTableName();
        string? schemaName = entity.GetSchema();

        string sql = $"DELETE FROM {schemaName}.{tableName}";
        context.Database.ExecuteSqlRaw(sql);

        sql = $"DBCC CHECKIDENT (\"{schemaName}.{tableName}\", RESEED, 0);";
        context.Database.ExecuteSqlRaw(sql);
    }
}
ClearSampleData();
```
Метод використовує метод FindEntityType() у властивості Model ApplicationDbContext, щоб отримати назву таблиці та схеми, а потім видаляє записи. Після видалення записів код використовує команду DBCC CHECKIDENT, щоб скинути ідентифікатор для кожної таблиці.

Загрузимо дані знову 

```cs
LoadMakeAndCarData();
```
```console
Saving change for Server=(localdb)\mssqllocaldb;Database=AutoLotSamples;Trusted_Connection=True;ConnectRetryCount=0
Saved change 6 entities
Saving change for Server=(localdb)\mssqllocaldb;Database=AutoLotSamples;Trusted_Connection=True;ConnectRetryCount=0
Saved change 10 entities
```
В базі даних можна побачити значення для додавання Id було скинкто до 0.

## Запит даних з однієї таблиці

Запит даних за допомогою EF Core зазвичай виконується за допомогою запитів LINQ. Пам'ятайте, що під час використання LINQ для запиту до бази даних для списку сутностей запит не виконується, доки запит не буде перебирати список, перетворюватнись на List<T> (або масив) або прив’язано до елемента керування списком (як сітка даних). Тобто до тих пір як дані реально потрібні для показу або використання. Для запитів з одним записом оператор виконується негайно, коли використовується виклик з одним записом (First(), Single() тощо).

Більше прикладів LINQ можна знайти в мережі інтернет за запитом "microsoft linq samples"

Ви можете викликати метод ToQueryString() у більшості запитів LINQ, щоб перевірити запит, який виконується до бази даних. Основним винятком є ​​будь-які запити негайного виконання, такі як First()/FirstOrDefault(). Для розділених запитів метод ToQueryString() повертає лише перший запит, який буде виконано.

### Отримання всіх запитів 

Щоб отримати всі записи для таблиці, просто використовуйте властивість DbSet<T> безпосередньо без будь-яких операторів LINQ. 

Створимо допоміжний метод.
```cs
static void CollectionCarToConsole(IEnumerable<Car> cars,string text)
{
    Console.WriteLine($"\t{text}");
    foreach (var car in cars)
    {
        Console.WriteLine($"{car.Id} {car.Color} {car.PetName}");
    }
}
```
Подивимость властивість context.Cars

```cs
static void ShowCars()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    var cars = context.Cars;
    CollectionCarToConsole(cars, "All cars");
    Console.WriteLine(context.Cars.GetType());
}
ShowCars();
```
```console
        All cars
An entity of type Car was loaded from the database.
1 Black Zippy
An entity of type Car was loaded from the database.
2 Rust Rusty
An entity of type Car was loaded from the database.
3 Black Mel
An entity of type Car was loaded from the database.
4 Yellow Clunker
An entity of type Car was loaded from the database.
5 Black Bimmer
An entity of type Car was loaded from the database.
6 Green Hank
An entity of type Car was loaded from the database.
7 Pink Pinky
An entity of type Car was loaded from the database.
8 Black Pete
An entity of type Car was loaded from the database.
9 Brown Brownie
An entity of type Car was loaded from the database.
10 Rust Lemon
Microsoft.EntityFrameworkCore.Internal.InternalDbSet`1[AutoLot.Samples.Models.Car]
```
Для негайного виконання додайте ToList() до властивості DbSet<T>.



Виберемо всі елементи.

```cs
static void QueryData_GetAllRecords()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    IQueryable<Car> cars = context.Cars;

    CollectionCarToConsole(cars, "All car from IQueryable<Car>");

    context.ChangeTracker.Clear();
    List<Car> listCars = context.Cars.ToList();

    CollectionCarToConsole(listCars, "All car from List<Car>");
}
QueryData_GetAllRecords();
```
Зверніть увагу, що повертається тип IQueryable<Car> під час використання DbSet<Car>, а тип повернення — List<Car> під час використання методу ToList(). Метод використовує метод Clear() на ChangeTracker для скидання ApplicationDbContext

```console
        All car from IQueryable<Car>
An entity of type Car was loaded from the database.
1 Black Zippy
An entity of type Car was loaded from the database.
2 Rust Rusty
An entity of type Car was loaded from the database.
3 Black Mel
An entity of type Car was loaded from the database.
4 Yellow Clunker
An entity of type Car was loaded from the database.
5 Black Bimmer
An entity of type Car was loaded from the database.
6 Green Hank
An entity of type Car was loaded from the database.
7 Pink Pinky
An entity of type Car was loaded from the database.
8 Black Pete
An entity of type Car was loaded from the database.
9 Brown Brownie
An entity of type Car was loaded from the database.
10 Rust Lemon
An entity of type Car was loaded from the database.
An entity of type Car was loaded from the database.
An entity of type Car was loaded from the database.
An entity of type Car was loaded from the database.
An entity of type Car was loaded from the database.
An entity of type Car was loaded from the database.
An entity of type Car was loaded from the database.
An entity of type Car was loaded from the database.
An entity of type Car was loaded from the database.
An entity of type Car was loaded from the database.
        All car from List<Car>
1 Black Zippy
2 Rust Rusty
3 Black Mel
4 Yellow Clunker
5 Black Bimmer
6 Green Hank
7 Pink Pinky
8 Black Pete
9 Brown Brownie
10 Rust Lemon
```

### Where. Фільтрація записів.

Метод Where() використовується для фільтрації записів із DbSet<T>. Виберемо жовті. 
```cs
static void FilterData_1()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    //Yellow cars
    IQueryable<Car> cars = context.Cars.Where(c => c.Color == "Yellow");
    CollectionCarToConsole(cars, "All yellow cars");

}
FilterData_1();
```
```console
        All yellow cars
An entity of type Car was loaded from the database.
4 Yellow Clunker
```

Кілька методів Where() можна плавно зв’язати для динамічного створення запиту. З’єднані методи Where() завжди поєднуються за логічним and у створеному запиті. 
```cs
static void FilterData_2()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    IQueryable<Car> cars2 = context.Cars
    .Where(c => c.Color == "Yellow" && c.PetName == "Clunker");
    CollectionCarToConsole(cars2, "All yellow cars with a petname of Clunker.");
    context.ChangeTracker.Clear(); 
    Console.WriteLine();

    IQueryable<Car> cars3 = context.Cars
        .Where(c => c.Color == "Yellow")
        .Where(c=>c.PetName == "Clunker");
    CollectionCarToConsole(cars3, "All yellow cars with a petname of Clunker.");
    context.ChangeTracker.Clear(); 
    Console.WriteLine();
    
    IQueryable<Car> cars4 = context.Cars
    .Where(c => c.Color == "Pink" || c.PetName == "Clunker");
    CollectionCarToConsole(cars4, "All black cars or a petname of Clunker.");
}
FilterData_2();
```
```console
        All yellow cars with a petname of Clunker.
An entity of type Car was loaded from the database.
4 Yellow Clunker

        All yellow cars with a petname of Clunker.
An entity of type Car was loaded from the database.
4 Yellow Clunker

        All black cars or a petname of Clunker.
An entity of type Car was loaded from the database.
4 Yellow Clunker
An entity of type Car was loaded from the database.
7 Pink Pinky
```
У прикладі згенерований запит для cars2 і cars3 ідентичний. Щоб створити оператор ||, ви повинні використовувати ту саму метод Where() з логічним виразом ||. 

Зауважте, що повернутий тип також є IQueryable<Car>, коли використовується методу Where.

Коли іде мова про рядкові поля можна викориcтати наступе.

```cs
static void FilterData_3()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    IQueryable<Car> cars5 = context.Cars
    .Where(c => !string.IsNullOrWhiteSpace(c.Color));
    CollectionCarToConsole(cars5, "Cars with colors.");
}
FilterData_3();
```
```console
        Cars with colors.
An entity of type Car was loaded from the database.
1 Black Zippy
An entity of type Car was loaded from the database.
2 Rust Rusty
An entity of type Car was loaded from the database.
3 Black Mel
An entity of type Car was loaded from the database.
4 Yellow Clunker
An entity of type Car was loaded from the database.
5 Black Bimmer
An entity of type Car was loaded from the database.
6 Green Hank
An entity of type Car was loaded from the database.
7 Pink Pinky
An entity of type Car was loaded from the database.
8 Black Pete
An entity of type Car was loaded from the database.
9 Brown Brownie
An entity of type Car was loaded from the database.
10 Rust Lemon
```
EF Core обробляє перетворення string.IsNullOrWhiteSpace() у SQL. Буде наступний запитю

```sql
SELECT [i].[Id], [i].[Color], [i].[DateBuilt], [i].[Display], [i].[IsDrivable], [i].[MakeId], [i].[PetName], [i].[TimeStamp]
FROM [dbo].[Inventory] AS [i]
WHERE [i].[Color] <> N''
```

### Orderby. Сортування записів.

Методи OrderBy() і OrderByDescending() встановлюють сортування для запиту в порядку зростання або спадання відповідно. Якщо потрібне подальше сортування, використовуйте методи ThenBy() та/або ThenByDescending().

```cs
static void SortData_1()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);


    IQueryable<Car> cars = context.Cars
        .OrderBy(c => c.Color);
    CollectionCarToConsole(cars, "Cars ordered by Color.");
    context.ChangeTracker.Clear();
    Console.WriteLine();

    IQueryable<Car> cars1 = context.Cars
    .OrderBy(c => c.Color)
    .ThenBy(c => c.PetName);
    CollectionCarToConsole(cars1, "Cars ordered by Color then PetName.");
    context.ChangeTracker.Clear();
    Console.WriteLine();

    IQueryable<Car> cars2 = context.Cars
    .OrderByDescending(c => c.Color);
    CollectionCarToConsole(cars2, "Cars ordered by Color descending.");
}
SortData_1();
```
Аби краще побачити результат закоментуємо рядок в класі ApplicationDbContext

```cs
        //ChangeTracker.Tracked += ChangeTracker_Tracked;
```
```console
        Cars ordered by Color.
1 Black Zippy
3 Black Mel
5 Black Bimmer
8 Black Pete
9 Brown Brownie
6 Green Hank
7 Pink Pinky
10 Rust Lemon
2 Rust Rusty
4 Yellow Clunker

        Cars ordered by Color then PetName.
5 Black Bimmer
3 Black Mel
8 Black Pete
1 Black Zippy
9 Brown Brownie
6 Green Hank
7 Pink Pinky
10 Rust Lemon
2 Rust Rusty
4 Yellow Clunker

        Cars ordered by Color descending.
4 Yellow Clunker
2 Rust Rusty
10 Rust Lemon
7 Pink Pinky
6 Green Hank
9 Brown Brownie
1 Black Zippy
8 Black Pete
3 Black Mel
5 Black Bimmer
```
Упорядкування за зростанням і спаданням можна змішувати

```cs
static void SortData_2()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);


    IQueryable<Car> cars = context.Cars
        .OrderBy(c => c.Color)
        .ThenByDescending(c => c.PetName);

    CollectionCarToConsole(cars, "Cars ordered by Color then by PetName descending");
}
SortData_2();
```
```console
        Cars ordered by Color then by PetName descending
1 Black Zippy
8 Black Pete
3 Black Mel
5 Black Bimmer
9 Brown Brownie
6 Green Hank
7 Pink Pinky
2 Rust Rusty
10 Rust Lemon
4 Yellow Clunker
```

### Revers

Метод Reverse() змінює весь порядок сортування на протилежний

```cs
static void ReversData()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);


    IQueryable<Car> cars = context.Cars
        .OrderBy(c => c.Color)
        .ThenByDescending(c => c.PetName)
        .Reverse();

    string text = "Cars ordered by Color then PetName in reverse";
    CollectionCarToConsole(cars, text);
}
ReversData();
```
```console
        Cars ordered by Color then PetName in reverse
4 Yellow Clunker
10 Rust Lemon
2 Rust Rusty
7 Pink Pinky
6 Green Hank
9 Brown Brownie
5 Black Bimmer
3 Black Mel
8 Black Pete
1 Black Zippy
```
Зауважте, що тип даних, який повертається із запиту LINQ із пропозицією Reverse(), є IQueryable<Car>, а не IOrderedQueryable<Car>.

Попередній запит LINQ перетворюється на наступний

```sql
SELECT [i].[Id], [i].[Color], [i].[DateBuilt], [i].[Display],
    [i].[IsDrivable], [i].[MakeId], [i].[PetName], [i].[TimeStamp]
FROM [dbo].[Inventory] AS [i]
ORDER BY [i].[Color] DESC, [i].[PetName] DESC
```

### Skip. Take. Створення сторінок (paging)

EF Core надає можливості розбити коллекцію на сторінки за допомогою Skip() і Take(). Skip() пропускає вказану кількість записів, тоді як Take() отримує вказану кількість записів. Використання методу Skip() із SQL Server виконує запит із командою OFFSET. Команда OFFSET — це версія SQL Server для пропуску записів, які зазвичай повертаються із запиту.
```cs
static void UsingSkip()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var cars = context.Cars.Skip(2);

    CollectionCarToConsole(cars, "Skip the first two records");
}
UsingSkip();
```

```console
        Skip the first two records
3 Black Mel
4 Yellow Clunker
5 Black Bimmer
6 Green Hank
7 Pink Pinky
8 Black Pete
9 Brown Brownie
10 Rust Lemon
```

Команда SQL Server OFFSET має меншу продуктивність, чим більше записів пропускається. Більшість програм, ймовірно, не використовуватимуть EF Core (або будь-яку ORM) із величезними обсягами даних, але переконайтеся, що ви тестуєте продуктивність усіх викликів, які використовують Skip(). Якщо є проблема з продуктивністю, можливо, краще перейти до FromSqlRaw()/FromSqlInterpolated(), щоб оптимізувати запит.
Код прикладу пропускає перші два записи та повертає решту. Трохи відредагований (для читабельності) запит показаний тут:
```sql
SELECT [i].[Id], [i].[Color], [i].[DateBuilt], [i].[Display],
    [i].[IsDrivable], [i].[MakeId], [i].[PetName], [i].[TimeStamp]
FROM [dbo].[Inventory] AS [i]
ORDER BY (SELECT 1)
OFFSET 2 ROWS
```
Зверніть увагу, що згенерований запит додає речення ORDER BY, навіть якщо оператор LINQ не мав жодного порядку. Це тому, що команду SQL Server OFFSET не можна використовувати без ORDER BY.

Метод Take() створює запит SQL Server, який використовує команду TOP.

```cs
static void UsingTake()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var cars = context.Cars.Take(2);

    CollectionCarToConsole(cars, "Take the first two records");
}
UsingTake();
```
```console
        Take the first two records
1 Black Zippy
2 Rust Rusty
```
Тут показано виконаний запит:

```sql
SELECT TOP(2) [i].[Id], [i].[Color], [i].[DateBuilt], [i].[Display],
    [i].[IsDrivable], [i].[MakeId], [i].[PetName], [i].[TimeStamp]
FROM [dbo].[Inventory] AS [i]
```
Комбінація методів Skip() і Take() дає змогу переглядати дані постранично. Наприклад, якщо розмір вашої сторінки дорівнює двом, і вам потрібно отримати другу сторінку, виконайте наступний запит LINQ

```cs
static void Paging()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    int totalCar = context.Cars.Count();
    int carOnPage = 2;
    int totalPage = (int)Math.Ceiling( (double) totalCar / carOnPage );

    int numberPage = 2;

    List<Car>? cars = context.Cars
        .Skip((numberPage - 1) * carOnPage)
        .Take(carOnPage)
        .ToList();
    CollectionCarToConsole(cars, $"Page {numberPage}");
}
Paging();
```
```console
        Page 2
3 Black Mel
4 Yellow Clunker
```
При поєднанні Skip() і Take() SQL Server використовує не команду TOP, а іншу версію команди OFFSET, як показано тут:

```sql
SELECT [i].[Id], [i].[Color], [i].[DateBuilt], [i].[Display], [i].[IsDrivable],
    [i].[MakeId], [i].[PetName], [i].[TimeStamp]
FROM [dbo].[Inventory] AS [i]
ORDER BY (SELECT 1)
OFFSET 2 ROWS FETCH NEXT 2 ROWS ONLY
```

### First/FirstOrDefault, Last/LastOrGefault, Single/SingleOrDefault методи.

Існує три основні методи (з варіантами OrDefault) для повернення одного запису за допомогою запиту: First()/FirstOrDefault(), Last()/LastOrDefault() і Single()/SingleOrDefault(). Хоча всі троє повертають один запис, їхні підходи відрізняються.

|Метод|Опис|
|-----|----|
|First()|Повертає перший запис, який відповідає умові запиту та будь-яким пунктам порядку.Якщо порядок не вказано, то повернутий запис базується на порядку бази даних.Якщо запис не повертається, створюється виняток.|
|FirstOrDefault()|Поведінка FirstOrDefault() відповідає First(), за винятком того, що якщо жоден запис не відповідає запиту, метод повертає значення за замовчуванням для типу (null).|
|Single()|Повертає один запис, який відповідає умові запиту та будь-яким пунктам порядку.Якщо порядок не вказано, то повернутий запис базується на порядку бази даних.Якщо запиту не відповідає жоден запис або більше ніж один запис, створюється виняток.|
|SingleOrDefault()|Поведінка SingleOrDefault() відповідає Single(), за винятком того, що якщо жоден запис не відповідає запиту, метод повертає значення за замовчуванням для типу (null).|
|Last()|повертає останній запис, який відповідає умові запиту та будь-яким пунктам порядку. Якщо порядок не вказано, створюється виняток. Якщо запис не повертається, створюється виняток.|
|LastOrDefault()|Поведінка відповідає Last(), за винятком того, що якщо жоден запис не відповідає запиту, метод повертає значення за замовчуванням для типу (null).|

Усі методи також можуть приймати Expression<Func<T, bool>> для фільтрації набору результатів. Це означає, що ви можете розмістити вираз Where() у виклику методів First()/Single(), якщо є лише одне речення Where().Наступні твердження еквівалентні:

```cs
context.Cars.Where(c=>c.Id < 5).First();
context.Cars.First(c=>c.Id < 5);
```


### First

Створимо додадковий метод для відображеня однієї сутності.

```cs
static void CarToConsole(Car? car, string? text)
{
    Console.WriteLine($"\t{text}");
    Console.WriteLine($"{car?.Id} {car?.Color} {car?.PetName}");
}
```
У разі використання форми First() і FirstOrDefault() без параметрів буде повернено перший запис (на основі порядку бази даних або будь-яких попередніх положень порядку). У наступному прикладі отримується перший запис на основі порядку бази даних:

```cs
static void UsingFirst_WithoutParameters()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var firstCar = context.Cars.First();

    CarToConsole(firstCar, "First record with database Sort");
}
UsingFirst_WithoutParameters();
```
```console
        First record with database Sort
1 Black Zippy
```
Попередній запит LINQ перетворюється на наступний

```sql
SELECT TOP(1) [i].[Id], [i].[Color], [i].[DateBuilt], [i].[Display],
    [i].[IsDrivable], [i].[MakeId], [i].[PetName], [i].[TimeStamp]
FROM [dbo].[Inventory] AS [i]
```
Наступний код отримує перший запис на основі порядку кольорів.
```cs
static void UsingFirst_OrderByColor()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var cars = context.Cars.OrderBy(c => c.Color);
    CollectionCarToConsole(cars, "Cars order by Color");
    Console.WriteLine();

    var firstCar = context.Cars.OrderBy(c=>c.Color).First();

    CarToConsole(firstCar, "First record with OrderBy sort");
}
UsingFirst_OrderByColor();
```
```console
        Cars order by Color
1 Black Zippy
3 Black Mel
5 Black Bimmer
8 Black Pete
9 Brown Brownie
6 Green Hank
7 Pink Pinky
10 Rust Lemon
2 Rust Rusty
4 Yellow Clunker

        First record with OrderBy sort
1 Black Zippy
```
Попередній запит LINQ перетворюється на таке:
```sql
SELECT TOP(1) [i].[Id], [i].[Color], [i].[DateBuilt], [i].[Display],
    [i].[IsDrivable], [i].[MakeId], [i].[PetName], [i].[TimeStamp]
FROM [dbo].[Inventory] AS [i]
ORDER BY [i].[Color]
```

Наступний код показує, що First() використовується з реченням Where(), а потім використовується First() як речення Where().
```cs
static void UsingFirst_AsWhere()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var firstCar1 = context.Cars.Where(c=>c.Id == 3).First();
    CarToConsole(firstCar1, "First record with Where clause");
    Console.WriteLine();

    var firstCar2 = context.Cars.First(c => c.Id == 3);
    CarToConsole(firstCar1, "First record using First as Where clause");
}
UsingFirst_AsWhere();
```
```console
        First record with Where clause
3 Black Mel

        First record using First as Where clause
3 Black Mel
```
Обидва попередні оператори перекладаються на наступний SQL:
```sql
SELECT TOP(1) [i].[Id], [i].[Color], [i].[DateBuilt], [i].[Display],
    [i].[IsDrivable], [i].[MakeId], [i].[PetName], [i].[TimeStamp]
FROM [dbo].[Inventory] AS [i]
WHERE [i].[Id] = 3
```

У наведеному нижче прикладі показано, що під час використання First() виникає виняток, якщо немає збігу.

```cs
static void UsingFirst_WithException()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    try
    {
        var firstCar = context.Cars.First(c => c.Id == 3);
        CarToConsole(firstCar, "First record with Id == 3");
        Console.WriteLine();

        firstCar = context.Cars.First(c => c.Id == 300);
        CarToConsole(firstCar, "First record with Id == 300");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        //throw;
    }
}
UsingFirst_WithException();
```
```console
        First record with Id == 3
3 Black Mel

Sequence contains no elements
```

Під час використання FirstOrDefault() замість винятку результатом є null, якщо дані не повертаються.

```cs
static void UsingFirstOrDefault_WithException()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var firstCar = context.Cars.FirstOrDefault(c => c.Id == 3);
    CarToConsole(firstCar, "First record with Id == 3");
    Console.WriteLine();

    firstCar = context.Cars.FirstOrDefault(c => c.Id == 300);
    CarToConsole(firstCar, "First record with Id == 300");

    Console.WriteLine(firstCar == null);    
}
UsingFirstOrDefault_WithException();
```
```console
        First record with Id == 3
3 Black Mel

        First record with Id == 300

True
```
Згадайте з глави про LINQ for object, що методи OrDefault() можуть вказати значення за замовчуванням, коли запит нічого не повертає. На жаль, ця можливість не підтримується в EF Core.

### Last

У разі використання форми Last() і LastOrDefault() без параметрів буде повернуто останній запис (на основі будь-яких попередніх положень порядку). Під час використання Last() запит LINQ має містити використання OrderBy()/OrderByDescending(), інакше буде викинуто виключення InvalidOperationException:

```cs
static void UsingLast()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var cars = context.Cars;
    CollectionCarToConsole(cars, "All cars");
    Console.WriteLine();

    try
    {
        var lastCar = context.Cars.Last();
        CarToConsole(lastCar, "Last car");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
UsingLast();
```
```console
        All cars
1 Black Zippy
2 Rust Rusty
3 Black Mel
4 Yellow Clunker
5 Black Bimmer
6 Green Hank
7 Pink Pinky
8 Black Pete
9 Brown Brownie
10 Rust Lemon

Queries performing 'Last' operation must have a deterministic sort order. Rewrite the query to apply an 'OrderBy' operation on the sequence before calling 'Last'.
```
Тому терба відсортувати послідовність.

```cs
static void UsingLast_WithOrderBy()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var cars = context.Cars.OrderBy(c=>c.Color);
    CollectionCarToConsole(cars, "All cars order by color");
    Console.WriteLine();

    try
    {
        var lastCar = context.Cars.OrderBy(c=>c.Color).Last();
        CarToConsole(lastCar, "Last car");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
UsingLast_WithOrderBy();
```
```console
        All cars order by color
1 Black Zippy
3 Black Mel
5 Black Bimmer
8 Black Pete
9 Brown Brownie
6 Green Hank
7 Pink Pinky
10 Rust Lemon
2 Rust Rusty
4 Yellow Clunker

        Last car
4 Yellow Clunker

```
EF Core змінює оператори ORDER BY, а потім бере top(1), щоб отримати результат.

```sql
SELECT TOP(1) [i].[Id], [i].[Color], [i].[DateBuilt], [i].[Display],
    [i].[IsDrivable], [i].[MakeId], [i].[PetName], [i].[TimeStamp]
FROM [dbo].[Inventory] AS [i]
ORDER BY [i].[Color] DESC
```

### Single

Концептуально Single()/SingleOrDefault() працює так само, як First()/FirstOrDefault(). Основна відмінність полягає в тому, що Single()/SingleOrDefault() повертає Top(2) замість Top(1) і створює виняток, якщо з бази даних повертаються два записи.
```cs
static void UsingSingle()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var singleCar = context.Cars.Single(c => c.Id == 3);
    CarToConsole(singleCar, "Single record with Id == 3");
}
UsingSingle();
```
```console
        Single record with Id == 3
3 Black Mel
```
Попередній запит LINQ перетворюється на наступний:
```sql
SELECT TOP(2) [i].[Id], [i].[Color], [i].[DateBuilt], [i].[Display],
    [i].[IsDrivable], [i].[MakeId], [i].[PetName], [i].[TimeStamp]
FROM [dbo].[Inventory] AS [i]
WHERE [i].[Id] = 3
```

Single() створює виняток, якщо не повертається жодного запису або повертається більше одного запису:
```cs
static void UsingSingle_WithExceptions()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    try
    {
        var singleCar = context.Cars.Single(c => c.Id > 1);
        CarToConsole(singleCar, "Single record with Id > 1");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    try
    {
        var singleCar = context.Cars.Single(c => c.Id > 100);
        CarToConsole(singleCar, "Single record with Id > 100");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
UsingSingle_WithExceptions();
```
```console
Sequence contains more than one element
Sequence contains no elements
```
У разі використання SingleOrDefault() замість винятку результат буде null, якщо дані не повертаються.

```cs
static void UsingSingleOrDefault()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    try
    {
        var singleCar = context.Cars.SingleOrDefault(c => c.Id > 1);
        CarToConsole(singleCar, "Single record with Id > 1");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    try
    {
        var singleCar = context.Cars.SingleOrDefault(c => c.Id > 100);
        CarToConsole(singleCar, "Single record with Id > 100");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
UsingSingleOrDefault();
```
```console
Sequence contains more than one element
        Single record with Id > 100

```


### Find

Метод Find() також повертає один запис, але поводиться трохи інакше, ніж інші методи з одним записом. Параметр(и) методу Find() представляють первинний ключ(и) сутності. Потім він шукає в ChangeTracker примірник сутності з відповідним первинним ключем і повертає його, якщо він знайдений. Якщо ні, то здійснить виклик до бази даних, щоб отримати запис. Якшо сутність не знайдено в БД повертає null.

```cs
static void UsingFind()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var car = context.Cars.Find(3);
    CarToConsole(car, "Car with Id = 3");
    Console.WriteLine();

    car = context.Cars.Find(300);
    CarToConsole(car, "Car with Id = 300");
}
UsingFind();
```

```console
        Car with Id = 3
3 Black Mel

        Car with Id = 300

```
Якщо сутність має складений первинний ключ, передайте значення, що представляють складений ключ:

```cs
var item = context.MyClassWithCompoundKey.Find(27,3);
```

### Методи агрегування

EF Core також підтримує агрегатні методи на стороні сервера (Max(), Min(), Count() і Average()). Усі агрегатні методи можна використовувати в поєднанні з методами Where() і повертати одне значення. Запити на агрегацію виконуються на стороні сервера. Глобальні фільтри запитів також впливають на агрегатні методи, і їх можна вимкнути за допомогою IgnoreQueryFilters().
Зверніть увагу, що кожен із агрегатних методів є завершальною функцією. Іншими словами, вони завершують інструкцію LINQ під час виконання, оскільки кожен метод повертає одне числове значення. Виконання запиту також відбувається негайно.

Метод ToQueryString() не працює з агрегацією.

Цей перший приклад підраховує кількість всіх записів в базі даних таблиці.

```cs
static void Aggregation()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    int totalCars = context.Cars.Count();

    Console.WriteLine(totalCars);
}
Aggregation();

```
```console
10
```
```sql
SELECT COUNT(*)
FROM [dbo].[Inventory] AS [i]
```

Метод Count() може містити вираз фільтра, як First() і Single(). Наступні приклади демонструють метод Count() із умовою where. Перший додає вираз безпосередньо в метод Count(), а другий додає метод Count() у кінець оператора LINQ після методу Where().

```cs
static void AggregationWithFilter()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var cars = context.Cars;
    foreach (var item in cars)
    {
        Console.WriteLine($"{item.Id} {item.MakeId}");
    }
    Console.WriteLine();

    Console.WriteLine(cars.Count(c=>c.MakeId == 1) );
    Console.WriteLine(cars.Where(c => c.MakeId == 1).Count());

}
AggregationWithFilter();
```
```console
1 1
2 2
3 3
4 4
5 5
6 5
7 5
8 6
9 4
10 1

2
2

```
```sql
SELECT COUNT(*)
FROM [dbo].[Inventory] AS [i]
WHERE [i].[MakeId] = 1
```
У наступних прикладах показано Min(), Max() і Average(). Кожен метод приймає вираз для вказівки властивості, над якою виконується операція.

```cs
static void MinMaxAverage()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var cars = context.Cars;
   
    Console.WriteLine(cars.Min(c => c.Id));
    Console.WriteLine(cars.Max(c => c.Id));
    Console.WriteLine(cars.Average(c => c.Id));
}
MinMaxAverage();
```
```console
1
10
5,5
```
```sql
SELECT MAX([i].[Id]) FROM [dbo].[Inventory] AS [i]
SELECT MIN([i].[Id]) FROM [dbo].[Inventory] AS [i]
SELECT AVG(CAST([i].[Id] AS float)) FROM [dbo].[Inventory] AS [i]
```

### Any, All

Методи Any() і All() перевіряють набір записів, щоб перевірити, чи відповідають якісь записи критеріям (Any()), чи всі записи відповідають критеріям (All()). Як і методи агрегації, метод Any() (але не метод All()) можна додати в кінець запиту LINQ за допомогою методів Where(), або вираз фільтра може міститися в самому методі. Методи Any() і All() виконуються на стороні сервера, і запит повертає логічне значення. Обидва є завершальними функціями. Глобальні фільтри запитів також впливають на функції методів Any() і All(), і їх можна вимкнути за допомогою IgnoreQueryFilters(). Метод ToQueryString() не працює з Any()/All().

```cs
static void UsingAny()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var cars = context.Cars;

    Console.WriteLine(cars.Any(c => c.MakeId == 1));
    Console.WriteLine(cars.Where(c => c.MakeId==1).Any());
}
UsingAny();
```
```
True
True
```
```sql
SELECT CASE
  WHEN EXISTS (
      SELECT 1
      FROM [dbo].[Inventory] AS [i]
      WHERE [i].[MakeId] = 1) THEN CAST(1 AS bit)
  ELSE CAST(0 AS bit)
END
```

Цей другий приклад перевіряє, чи всі записи автомобіля мають певний MakeId.

```cs
static void UsingAll()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var cars = context.Cars;

    Console.WriteLine(cars.All(c => c.MakeId == 1));
}
UsingAll();
```
```
false
```
```sql
SELECT CASE
  WHEN NOT EXISTS (
      SELECT 1
      FROM [dbo].[Inventory] AS [i]
      WHERE [i].[MakeId] <> 1) THEN CAST(1 AS bit)
  ELSE CAST(0 AS bit)
END
```

### Отримання даних із збережених процедур

Останній шаблон пошуку даних, який потрібно дослідити, — це отримання даних із збережених процедур. Хоча в EF Core є деякі прогалини щодо збережених процедур, пам’ятайте, що EF Core створено на основі ADO.NET. Нам просто потрібно опустити рівень і згадати, як ми називали збережені процедури до ORM.

Першим кроком є ​​створення збереженої процедури в нашій базі даних:

```sql
CREATE PROCEDURE [dbo].[GetPetName]
    @carID int,
    @petName nvarchar(50) output
AS
SELECT @petName = PetName from dbo.Inventory where Id = @carID
```
Наступний метод створює необхідні параметри (вхідні та вихідні), використовує властивість ApplicationDbContext Database і викликає ExecuteSqlRaw():

```cs
static void CallStopedProcedure()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var parameterId = new SqlParameter
    {
        ParameterName = "@carId",
        SqlDbType = System.Data.SqlDbType.Int,
        Value = 3
    };

    var parameterName = new SqlParameter
    {
        ParameterName = "@petName",
        SqlDbType = System.Data.SqlDbType.NVarChar,
        Size = 50,
        Direction = System.Data.ParameterDirection.Output
    };

    string sql = "EXEC [dbo].[GetPetName] @carId, @petName OUTPUT";

    _ = context.Database.ExecuteSqlRaw(sql, parameterId, parameterName);

    Console.WriteLine(parameterName.Value);
}
CallStopedProcedure();
```
```
Mel
```
```sql
declare @p4 nvarchar(50)
set @p4=N'Mel'
exec sp_executesql N'EXEC [dbo].[GetPetName] @carId, @petName OUTPUT',
  N'@carId int,@petName nvarchar(50) output',@carId=1,@petName=@p4 output
select @p4
```

## Запит пов’язаних даних з відповідних таблиць 


















