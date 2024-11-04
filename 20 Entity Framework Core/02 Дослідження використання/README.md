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

    context.Makes.Add( newMake );
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
Щоб розглянути наступний приклад можна видалити і створити БД.
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

Якщо сутність має числову властивість, визначену як первинний ключ, ця властивість (за замовчуванням) зіставляється зі стовпцем Identity у SQL Server. EF Core вважає будь-яку сутність зі значенням за замовчуванням (нуль) для властивості ключа новою, а будь-яку сутність зі значенням, відмінним від за замовчуванням, уже існує в базі даних. Якщо ви створюєте нову сутність і встановлюєте для властивості первинного ключа значення, відмінне від нуля, і намагаєтесь додати її до бази даних, EF Core не зможе додати запис, оскільки вставку ідентифікаційної інформації не ввімкнено.
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
    IEntityType metadata = context.Model.FindEntityType(typeof(Car).FullName);
    string schema = metadata.GetSchema();
    string tableName = metadata.GetTableName();
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
В базі даних можна побачити значеня для додавання Id було скинкто до 0.

## Запит даних

Запит даних за допомогою EF Core зазвичай виконується за допомогою запитів LINQ. Нагадую, що під час використання LINQ для запиту до бази даних для списку сутностей запит не виконується, доки запит не буде перебирати список, перетворюватнись на List<T> (або масив) або прив’язано до елемента керування списком (як сітка даних). Тобто до тих пір як дані реально потрібні для показу або використання. Для запитів з одним записом оператор виконується негайно, коли використовується виклик з одним записом (First(), Single() тощо).

Більше прикладів LINQ можна знайти в шнтернеті за запитом "microsoft linq samples"

Ви можете викликати метод ToQueryString() у більшості запитів LINQ, щоб перевірити запит, який виконується до бази даних. Основним винятком є ​​будь-які запити негайного виконання, такі як First()/FirstOrDefault(). Для розділених запитів метод ToQueryString() повертає лише перший запит, який буде виконано.

### Отримання всіх запитів 

Щоб отримати всі записи для таблиці, просто використовуйте властивість DbSet<T> безпосередньо без будь-яких операторів LINQ. Для негайного виконання додайте ToList() до властивості DbSet<T>.

```cs
static void QueryData_GetAllRecords()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    IQueryable<Car> cars = context.Cars;
    foreach (var car in cars)
    {
        Console.WriteLine($"{car.Id}\t{car.PetName}\t{car.Color}");
    }

    context.ChangeTracker.Clear();

    List<Car> listCars = context.Cars.ToList();
    foreach (var car in listCars)
    {
        Console.WriteLine($"{car.Id}\t{car.PetName}\t{car.Color}");
    }
}
QueryData_GetAllRecords();
```
Зверніть увагу, що повертається тип IQueryable<Car> під час використання DbSet<Car>, а тип повернення — List<Car> під час використання методу ToList().

```console
An entity of type Car was loaded from the database.
1       Zippy   Black
An entity of type Car was loaded from the database.
2       Rusty   Rust
An entity of type Car was loaded from the database.
3       Mel     Black
An entity of type Car was loaded from the database.
4       Clunker Yellow
An entity of type Car was loaded from the database.
5       Bimmer  Black
An entity of type Car was loaded from the database.
6       Hank    Green
An entity of type Car was loaded from the database.
7       Pinky   Pink
An entity of type Car was loaded from the database.
8       Pete    Black
An entity of type Car was loaded from the database.
9       Brownie Brown
An entity of type Car was loaded from the database.
10      Lemon   Rust
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
1       Zippy   Black
2       Rusty   Rust
3       Mel     Black
4       Clunker Yellow
5       Bimmer  Black
6       Hank    Green
7       Pinky   Pink
8       Pete    Black
9       Brownie Brown
10      Lemon   Rust
```
