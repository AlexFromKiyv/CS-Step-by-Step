static void AddRecord()
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
//AddRecord();

static void ViewMake(Make make,string text)
{
    Console.WriteLine($"\t{text}");
    Console.WriteLine($"\tId:{make.Id}");
    Console.WriteLine($"\tName:{make.Name}");
}

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
//AddRecordsWithAttach();

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
//AddMultipleRecords();


static void GetSchemaAndTableNameForType()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    IEntityType metadata = context.Model.FindEntityType(typeof(Car).FullName);
    var schema = metadata.GetSchema();
    var tableName = metadata.GetTableName();
    Console.WriteLine($"{schema} {tableName}");
}
//GetSchemaAndTableNameForType();

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
//AddRowWithSetIdentityInsert();

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
//AddEntityWithChild();

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
    ((List<Driver>) cars[0].Drivers).AddRange(drivers.Take(..3));
    ((List<Driver>)cars[1].Drivers).AddRange(drivers.Take(3..));
    context.SaveChanges();
}
//AddRecordsToMantToManyTables();

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
//LoadMakeAndCarData();

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
//ClearSampleData();

//LoadMakeAndCarData();

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