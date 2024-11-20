using AutoLot.Samples.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.Extensions.DependencyInjection;
using System.Xml.Serialization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

static void AddRecord()
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
            //Console.WriteLine(ex.InnerException.Message);
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
//AddRecordsToMantToManyTables();


static void CollectionCarToConsole(IEnumerable<Car> cars,string text)
{
    Console.WriteLine($"\t{text}");
    foreach (var car in cars)
    {
        Console.WriteLine($"{car.Id} {car.Color} {car.PetName}");
    }
}

static void ShowCars()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    var cars = context.Cars;
    CollectionCarToConsole(cars, "All cars");
    Console.WriteLine();
    Console.WriteLine(cars.ToQueryString());
    Console.WriteLine();
    Console.WriteLine(context.Cars.GetType());
}
//ShowCars();

static void QueryData_GetAllRecords()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    IQueryable<Car> cars = context.Cars;

    CollectionCarToConsole(cars, "All car from IQueryable<Car>");

    context.ChangeTracker.Clear();
    List<Car> listCars = context.Cars.ToList();

    CollectionCarToConsole(listCars, "All car from List<Car>");
}
//QueryData_GetAllRecords();


static void FilterData_1()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    //Yellow cars
    IQueryable<Car> cars = context.Cars.Where(c => c.Color == "Yellow");
    CollectionCarToConsole(cars, "All yellow cars");

}
//FilterData_1();

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
//FilterData_2();

static void FilterData_3()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    IQueryable<Car> cars5 = context.Cars
    .Where(c => !string.IsNullOrWhiteSpace(c.Color));
    CollectionCarToConsole(cars5, "Cars with colors.");
}
//FilterData_3();

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
//SortData_1();

static void SortData_2()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);


    IQueryable<Car> cars = context.Cars
        .OrderBy(c => c.Color)
        .ThenByDescending(c => c.PetName);

    CollectionCarToConsole(cars, "Cars ordered by Color then by PetName descending");
}
//SortData_2();

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
//ReversData();

static void UsingSkip()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var cars = context.Cars.Skip(2);

    CollectionCarToConsole(cars, "Skip the first two records");
}
//UsingSkip();

static void UsingTake()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var cars = context.Cars.Take(2);

    CollectionCarToConsole(cars, "Take the first two records");
}
//UsingTake();


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
//Paging();


static void CarToConsole(Car? car, string? text)
{
    Console.WriteLine($"\t{text}");
    Console.WriteLine($"{car?.Id} {car?.Color} {car?.PetName}");
}

static void UsingFirst_WithoutParameters()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var firstCar = context.Cars.First();

    CarToConsole(firstCar, "First record with database Sort");
}
//UsingFirst_WithoutParameters();

static void UsingFirst_OrderByColor()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var cars = context.Cars.OrderBy(c => c.Color);
    CollectionCarToConsole(cars, "Cars order by Color");
    Console.WriteLine();

    var firstCar = context.Cars.OrderBy(c=>c.Color).First();

    CarToConsole(firstCar, "First record with OrderBy sort");
}
//UsingFirst_OrderByColor();

static void UsingFirst_AsWhere()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var firstCar1 = context.Cars.Where(c=>c.Id == 3).First();
    CarToConsole(firstCar1, "First record with Where clause");
    Console.WriteLine();

    var firstCar2 = context.Cars.First(c => c.Id == 3);
    CarToConsole(firstCar1, "First record using First as Where clause");
}
//UsingFirst_AsWhere();

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
//UsingFirst_WithException();

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
//UsingFirstOrDefault_WithException();

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
//UsingLast();

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
//UsingLast_WithOrderBy();


static void UsingSingle()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var singleCar = context.Cars.Single(c => c.Id == 3);
    CarToConsole(singleCar, "Single record with Id == 3");
}
//UsingSingle();

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
//UsingSingleOrDefault();

static void UsingFind()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var car = context.Cars.Find(3);
    CarToConsole(car, "Car with Id = 3");
    Console.WriteLine();

    car = context.Cars.Find(300);
    CarToConsole(car, "Car with Id = 300");
}
//UsingFind();

static void Aggregation()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    int totalCars = context.Cars.Count();

    Console.WriteLine(totalCars);
}
//Aggregation();

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
//AggregationWithFilter();

static void MinMaxAverage()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var cars = context.Cars;
   
    Console.WriteLine(cars.Min(c => c.Id));
    Console.WriteLine(cars.Max(c => c.Id));
    Console.WriteLine(cars.Average(c => c.Id));
}
//MinMaxAverage();

static void UsingAny()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var cars = context.Cars;

    Console.WriteLine(cars.Any(c => c.MakeId == 1));
    Console.WriteLine(cars.Where(c => c.MakeId==1).Any());
}
//UsingAny();

static void UsingAll()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var cars = context.Cars;

    Console.WriteLine(cars.All(c => c.MakeId == 1));
}
//UsingAll();

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
//CallStopedProcedure();



static void EagerLoading_1()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
   
    var query = context
        .Cars
        .Include(c => c.MakeNavigation);

    Console.WriteLine(query.ToQueryString()); Console.WriteLine();
    var cars = query.ToList();
    
    foreach (var car in cars)
    {
        Console.WriteLine($"{car.Id} {car.MakeNavigation.Name} {car.Color}");
    }    
}
//EagerLoading_1();

static void EagerLoading_2()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var query = context
        .Makes
        .Include(m => m.Cars)
        .ThenInclude(c=>c.Drivers);

    Console.WriteLine(query.ToQueryString());
    Console.WriteLine();

    Make? make = query.First();
    List<Car>? cars = make?.Cars.ToList();
    CollectionCarToConsole(cars, $"Cars of {make?.Name}");
    Console.WriteLine();

    Car? car = cars.First();
    Driver? driver = car.Drivers.First();
    Console.WriteLine($"" +
        $"Driver {driver.PersonInfo.FirstName} {driver.PersonInfo.LastName} " +
        $"of car {car.Id} {car.MakeNavigation.Name} {car.Color} {car.PetName}");
 
}
//EagerLoading_2();

static void EagerLoading_3()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var query = context
        .Makes
        .Include(m => m.Cars)
        .ThenInclude(c => c.Drivers)
        .OrderBy(m => m.Name);

    Console.WriteLine(query.ToQueryString());
}
//EagerLoading_3();


static void FilteredInclude()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var query = context
        .Makes
        .Include(m => m.Cars.Where(c => c.Color == "Yellow"));

    Console.WriteLine(query.ToQueryString());
    Console.WriteLine();

    var makes = query.ToList();
    Console.WriteLine(makes.Count());
}
//FilteredInclude();

static void EagerLoadingWithSplitQueries()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var query = context
        .Makes.AsSplitQuery()
        .Include(m => m.Cars.Where(c => c.Color == "Yellow"));

    Console.WriteLine(query.ToQueryString());
    Console.WriteLine();

    Console.WriteLine(query.Count());
}
//EagerLoadingWithSplitQueries();

static void ManyToManyQueries()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var query = context
        .Cars
        .Include(c => c.Drivers)
        .Where(c => c.Drivers.Any());

    Console.WriteLine(query.ToQueryString());
    Console.WriteLine();

    Console.WriteLine(query.Count());
}
//ManyToManyQueries();


static void ExplicitLoading()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    //Get the Car record
    Car? car = context.Cars.First(c => c.Id == 1);
    Console.WriteLine($"{car.Id} {car.MakeId} {car.MakeNavigation?.Name}");

    //Load Make entity and define MakeNavigation 
    context.Entry(car).Reference(c => c.MakeNavigation).Load();
    Console.WriteLine($"{car.Id} {car.MakeId} {car.MakeNavigation?.Name}");

}
//ExplicitLoading();

static void ExplicitLoadingCollectionOneToMany()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    Make? make = context.Makes.Single(m => m.Id == 1);
    Console.WriteLine($"{make.Id} {make.Name}");
    Console.WriteLine();

    var query = context.Entry(make).Collection(c => c.Cars).Query();
    string sql = query.ToQueryString();
    Console.WriteLine(sql);
    Console.WriteLine();

    query.Load();
    Console.WriteLine("Entities cars loaded into memory.\n");
    List<Car>? cars = query.ToList();

    CollectionCarToConsole(cars,$"{make.Name} cars");

}
//ExplicitLoadingCollectionOneToMany();

static void ExplicitLoadingCollectionManyToMany()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    //Get the Car record
    Car? car = context.Cars.First(c => c.Id == 1);
    CarToConsole(car, "Car with id = 1");
    Console.WriteLine();

    var query = context.Entry(car).Collection(c => c.Drivers).Query();

    string sql = query.ToQueryString();
    Console.WriteLine(sql);
    Console.WriteLine();
    
    //Load drivers to memory
    query.Load();
    Console.WriteLine();

    List<Driver>? drivers = query.ToList();
    foreach (var driver in drivers)
    {
        Console.WriteLine($"" +
            $"{driver.Id} " +
            $"{driver.PersonInfo.FirstName} " +
            $"{driver.PersonInfo.LastName} ");
    }
}
//ExplicitLoadingCollectionManyToMany();

static void NoLazyLoad()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var query = context.Cars.AsQueryable();
    Car car = query.First();

    try
    {
        Console.WriteLine(car.MakeNavigation.Name);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine("The navigation property has not been loaded.");
        Console.WriteLine($"Is car.MakeNavigation == null " +
            $":{car.MakeNavigation == null}");
    }
}
//NoLazyLoad();

static void LazyLoad()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(["lazy"]);

    var query = context.Cars.AsQueryable();
    Car car = query.First();

    try
    {
        Console.WriteLine(car.MakeNavigation.Name);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine("The navigation property has not been loaded.");
        Console.WriteLine($"Is car.MakeNavigation == null " +
            $":{car.MakeNavigation == null}");
    }
}
//LazyLoad();

static void UpdateOneRecord()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var car = context.Cars.First();
    car.Color = "Green";
    context.SaveChanges();

    ShowFirstCar();
}
//UpdateOneRecord();

static void ShowFirstCar()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var car = context.Cars.Include(c => c.MakeNavigation).First();

    Console.WriteLine($"{car.Id} {car.MakeNavigation.Name} {car.Color} {car.PetName}");
}

static void UpdateNontrackedEntities()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var car = context.Cars.AsNoTracking().First();
    car.Color = "Orange";

    context.Cars.Update(car); //!!!
    //or
    //context.Entry(car).State = EntityState.Modified;

    context.SaveChanges();

    ShowFirstCar();
}
//UpdateNontrackedEntities();

static void LoadInitialDataToDatabase()
{
    ClearSampleData();
    LoadMakeAndCarData();
    AddRecordsToMantToManyTables();
}
//LoadInitialDataToDatabase();

static void DeleteOneRecord()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    // Viewing green cars
    var greenCars = context.Cars.Where(c => c.Color == "Green").ToList();
    CollectionCarToConsole(greenCars,"Green cars");

    // Removing green car
    var greenCar = context.Cars.First(c => c.Color == "Green");
    context.Cars.Remove(greenCar);
    context.SaveChanges();
    CarToConsole(greenCar,"Green car still in memory?");
    Console.WriteLine(context.Entry(greenCar).State);

    // Viewing green cars
    greenCars = context.Cars.Where(c => c.Color == "Green").ToList();
    CollectionCarToConsole(greenCars, "Green cars");
}
//DeleteOneRecord();


static void DeleteNontrackedEntities()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var car = context.Cars.AsNoTracking().First(c => c.Color == "Green");
    context.Cars.Remove(car);
    context.SaveChanges();

    context.ChangeTracker.Clear();
    car = context.Cars.AsNoTracking().First(c => c.Color == "Yellow");
    context.Entry(car).State = EntityState.Deleted;
    context.SaveChanges();

    context.ChangeTracker.Clear();
    var cars = context.Cars;
    CollectionCarToConsole(cars, "All cars");

}
//DeleteNontrackedEntities();

static void CatchCascadeDeleteFailures()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var make = context.Makes.First();
    Console.WriteLine(make.Name);

    context.Makes.Remove(make);

    try
    {
        context.SaveChanges();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine(ex.InnerException.Message);
    }
}
//CatchCascadeDeleteFailures();

static void CarCountWithGlobalFilter()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var query = context.Cars;
    Console.WriteLine(query.ToQueryString());
    Console.WriteLine();

    int totalCars = query.Count();

    Console.WriteLine($"Total cars: {totalCars}");

    int drivableCars = query.Where(c => c.IsDrivable == true).Count();
    Console.WriteLine($"Drivable cars: {drivableCars}");
}
//CarCountWithGlobalFilter();

static void CarCountWithoutGlobalFilter()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var query = context.Cars.IgnoreQueryFilters();
    Console.WriteLine(query.ToQueryString());
    Console.WriteLine();

    int totalCars = query.Count();

    Console.WriteLine($"Total cars: {totalCars}");

    int drivableCars = query.Where(c => c.IsDrivable == true).Count();
    Console.WriteLine($"Drivable cars: {drivableCars}");
}
//CarCountWithoutGlobalFilter();

static void GlobalQueryFiltersOnNavigationProperties()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var query = context.Radios;
    Console.WriteLine(query.ToQueryString());
    Console.WriteLine();

    var query1 = context.Radios.IgnoreQueryFilters();
    Console.WriteLine(query1.ToQueryString());
}
//GlobalQueryFiltersOnNavigationProperties();


static void ReletedDataWithGlobalQueryFilters()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var queryCars = context.Cars.IgnoreQueryFilters().Where(c=>c.MakeId==1);

    Console.WriteLine($"Car with MakerId = 1 : {queryCars.Count()}");
    Console.WriteLine();
    context.ChangeTracker.Clear();
 
    var make = context.Makes.First(m => m.Id == 1);
    context.Entry(make).Collection(m => m.Cars).Load();

}
//ReletedDataWithGlobalQueryFilters();

static void ReletedDataWithIgnoreGlobalQueryFilters()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
 
    var make = context.Makes.First(m => m.Id == 1);
    context.Entry(make).Collection(m => m.Cars).Query().IgnoreQueryFilters().Load();

}
//ReletedDataWithIgnoreGlobalQueryFilters();

static void ShowSchemaTableName()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    IEntityType? metadata = context.Model.FindEntityType(typeof(Car).FullName);

    Console.WriteLine(metadata?.GetSchema());
    Console.WriteLine(metadata?.GetTableName());

}
//ShowSchemaTableName();

static void UsingFromSqlRawInterpolated()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    int carId = 1;
    var query = context.Cars
        .FromSqlInterpolated($"Select * from dbo.Inventory where Id = {carId}")
        .Include(c => c.MakeNavigation);

    Console.WriteLine(query.ToQueryString());
    Console.WriteLine();

    var car = query.First();
    Console.WriteLine($"{car.Id} {car.PetName}");
}
//UsingFromSqlRawInterpolated();

static void ProjectionsToIds()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var query = context.Cars.Select(c => c.Id);
    Console.WriteLine(query.ToQueryString());
    Console.WriteLine();

    List<int> ids = query.ToList();
    foreach (var item in ids)
    {
        Console.Write(item+"\t");
    }
}
//ProjectionsToIds();

static void ProjectionToCarMakeViewModel()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    
    var query = context.Cars.Select(c => new CarMakeViewModel
    {
        CarId = c.Id,
        Color = c.Color,
        DateBuild = c.DateBuilt.GetValueOrDefault(),
        Display = c.Display,
        IsDrivable = c.IsDrivable.GetValueOrDefault(false),
        Make = c.MakeNavigation.Name,
        MakeId = c.MakeId,
        PetName = c.PetName
    });
    Console.WriteLine(query.ToQueryString());
    Console.WriteLine();

    foreach (CarMakeViewModel cmvm in query)
    {
        Console.WriteLine(cmvm.CarId+"\t"+cmvm.Make+"\t"+cmvm.Color);
    }
}
//ProjectionToCarMakeViewModel();


static void AddACar()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    Car car = new()
    {
        Color = "Yellow",
        MakeId = 1,
        PetName = "Herbie"
    };

    context.Cars.Add(car);
    context.SaveChanges();
}
//AddACar();

static void AddACarWithDefaultsSet()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    Car car = new()
    {
        Color = "Yellow",
        MakeId = 1,
        PetName = "Herbie",
        IsDrivable = true,
        DateBuilt = new DateTime(2021, 01, 01)
    };

    context.Cars.Add(car);
    context.SaveChanges();
}
//AddACarWithDefaultsSet();

static void UpdateACar()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    var car = context.Cars.First(c => c.Id == 1);
    car.Color = "White";
    context.SaveChanges();
}


static void ThrowConcurrencyException()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    try
    {
        Car car = context.Cars.First();

        //update the database outside of the context
        FormattableString sql = $"Update dbo.Inventory set Color='Pink' where Id = {car.Id}";
        context.Database
            .ExecuteSqlInterpolated(sql);

        //update the car record in the change tracker and then try and save changes
        car.Color = "Yellow";
        context.SaveChanges();
    }
    catch (DbUpdateConcurrencyException ex)
    {
        //Get the entity that failed to update
        var entry = ex.Entries[0];
        //Get the original values (when the entity was loaded)
        PropertyValues originalProperty = entry.OriginalValues;
        //Get the current values (updated by this code path)
        PropertyValues currentProperty = entry.OriginalValues;
        // get the current values from the data store –
        //Note: This needs another database call
        PropertyValues? databaseProperty = entry.GetDatabaseValues();
    }
}
//ThrowConcurrencyException();

static void UsingMappedDBFunction()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    var query = context.Makes.Where(m => ApplicationDbContext.InventoryCountFor(m.Id) > 1);
    Console.WriteLine(query.ToQueryString());
    Console.WriteLine();

    List<Make> makes = query.ToList();
    foreach (var make in makes)
    {
        Console.WriteLine($"{make.Name}");
    }
}
//UsingMappedDBFunction();

static void UsingTableValuedDBFunction()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    var query = context.GetCarsFor(5);
    Console.WriteLine(query.ToQueryString());
    Console.WriteLine();

    List<Car>? cars = query.ToList();
    CollectionCarToConsole(cars,"Cars MakeId = 5");
}
//UsingTableValuedDBFunction();

static void UsingEFFunctions_Like()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    var query = context.Cars.IgnoreQueryFilters().Where(c => EF.Functions.Like(c.PetName, "%r%"));
    Console.WriteLine(query.ToQueryString());
    Console.WriteLine();

    List<Car>? cars = query.ToList();
    CollectionCarToConsole(cars, "Cars are Like(c.PetName, \"%r%\")");
}
//UsingEFFunctions_Like();

static void Batching()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    var cars = new List<Car>
    {
        new Car {Color = "Yellow", MakeId = 1, PetName = "Herbie"},
        new Car {Color = "White", MakeId = 2, PetName = "Mach 5"},
        new Car {Color = "Pink", MakeId = 3, PetName = "Avon"},
        new Car {Color = "Blue", MakeId = 4, PetName = "Blueberry"},
    };
    context.Cars.AddRange(cars);
    context.SaveChanges();
}
//Batching();

static void UsingValueConverter()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var car = context.Cars.First();
    car.Price = "777.00";
    context.SaveChanges();
    
}
//UsingValueConverter();

static void ShowPriceOfFirstCar()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    var car = context.Cars.First();
    Console.WriteLine($"{car.Id} {car.PetName} {car.Price}");

}
//ShowPriceOfFirstCar();

static void ShadowProperties()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var car = new Car()
    {
        Color = "White",
        PetName = "Snuppy",
        MakeId = 1,
        Price = "1000.00"
        //Compile error
        //IsDeleted = false
    };

    context.Cars.AddRange(car);
    context.Entry(car).Property("IsDeleted").CurrentValue = true;
    context.SaveChanges();
}
//ShadowProperties();

static void UsingShadowPropertiesWithLINQ()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var cars = context.Cars.ToList();

    foreach (var car in cars.Where(c => c.Id % 2 == 0))
    {
        context.Entry(car).Property("IsDeleted").CurrentValue = false;
    }
    context.SaveChanges();

    var noDeletedCars = context.Cars.Where(c => !EF.Property<bool>(c,"IsDeleted")).ToList();
    foreach (var car in noDeletedCars)
    {
        Console.WriteLine($"{car.Id} {car.PetName} is deleted " +
            $"{context.Entry(car).Property("IsDeleted").CurrentValue}");
    }
}
//UsingShadowPropertiesWithLINQ();

static void UsingLinqForTemporalTable()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var query = context.Cars;
    Console.WriteLine(query.ToQueryString());
    Console.WriteLine();

    var cars = query.ToList();
    CollectionCarToConsole(cars, "Cars");
}
//UsingLinqForTemporalTable();

static void UsingFromSqlRawInterpolatedWithTemporalTable()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    int carId = 1;
    var query = context.Cars
        .FromSqlInterpolated($"Select *,PeriodEnd,PeriodStart from dbo.Inventory where Id = {carId}")
        .Include(c => c.MakeNavigation);

    Console.WriteLine(query.ToQueryString());
    Console.WriteLine();

    var car = query.First();
    Console.WriteLine($"{car.Id} {car.MakeNavigation.Name} {car.PetName}");
}
//UsingFromSqlRawInterpolatedWithTemporalTable();

static void MainTableAndHistoryTableInteractions_1()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var car = new Car
    {
        Color = "LightBlue",
        MakeId = 1,
        PetName = "Sky",
        Price = "500.00"
    };
    context.Cars.Add(car);
    context.SaveChanges();
}
//MainTableAndHistoryTableInteractions_1();


static void MainTableAndHistoryTableInteractions_2()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    int maxId = context.Cars.Max(c => c.Id);
    var car = context.Cars.Find(maxId);

    if (car == null) return;

    car.Color = "Red";
    context.Cars.Update(car);
    context.SaveChanges();

}
//MainTableAndHistoryTableInteractions_2();

static void MainTableAndHistoryTableInteractions_3()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    int maxId = context.Cars.Max(c => c.Id);
    var car = context.Cars.Find(maxId);

    if (car == null) return;
     
    context.Cars.Remove(car);
    context.SaveChanges();

}
//MainTableAndHistoryTableInteractions_3();

static void PreparationQueryingTemporalTables()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var car = new Car
    {
        Color = "Navy",
        MakeId = 1,
        PetName = "Warrior",
    };
    context.Cars.Add(car);
    context.SaveChanges();
    Thread.Sleep(5000);

    car.Color = "DarkBlue";
    context.Cars.Update(car);
    context.SaveChanges();
    Thread.Sleep(5000);

    context.Cars.Remove(car);
    context.SaveChanges();
}
//PreparationQueryingTemporalTables();

static void QueryingTemporalTables()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var query = context.Cars
        .TemporalAll()
        .OrderBy(e=> EF.Property<DateTime>(e,"PeriodStart"))
        .Select( e => new 
        {
            Car = e,
            PeriodStart = EF.Property<DateTime>(e,"PeriodStart"),
            PeriodEnd = EF.Property<DateTime>(e, "PeriodEnd")
        });

    Console.WriteLine(query.ToQueryString());
    Console.WriteLine();


    foreach ( var car in query)
    {
        Console.WriteLine($"{car.Car.PetName} {car.PeriodStart} {car.PeriodEnd}");
    }
}
//QueryingTemporalTables();

static void ClearingTemporalTables()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    
    var strategy = context.Database.CreateExecutionStrategy();
    strategy.Execute(()=>
    { 
        using var transaction = context.Database.BeginTransaction();

        string sql = "ALTER TABLE [dbo].[Inventory] SET (SYSTEM_VERSIONING = OFF)";
        context.Database.ExecuteSqlRaw(sql);
        sql = "DELETE FROM audits.InventoryAudit";
        context.Database.ExecuteSqlRaw(sql);
        sql = "ALTER TABLE dbo.Inventory SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE=audits.InventoryAudit))";
        context.Database.ExecuteSqlRaw(sql);

        transaction.Commit();
    });
}
//ClearingTemporalTables();

static void SchemaAndNameForTemporal()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var serviceCollection = new ServiceCollection();
    serviceCollection.AddDbContextDesignTimeServices(context);
    var serviceProvider = serviceCollection.BuildServiceProvider();
    IModel? designTimeModel = serviceProvider.GetService<IModel>();

    var designTimeEntity = designTimeModel?.FindEntityType(typeof(Car).FullName);

    string? historySchema = designTimeEntity?.GetHistoryTableSchema();
    string? tableName = designTimeEntity?.GetHistoryTableName();

    Console.WriteLine(historySchema + "\t" + tableName);
}
//SchemaAndNameForTemporal();

static void ClearSampleDataAndTemporal()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    string?[] entities =
    [
        typeof(Driver).FullName,
        typeof(Car).FullName,
        typeof(Make).FullName
    ];

    var serviceCollection = new ServiceCollection();
    serviceCollection.AddDbContextDesignTimeServices(context);
    var serviceProvider = serviceCollection.BuildServiceProvider();
    IModel? designTimeModel = serviceProvider.GetService<IModel>();

    foreach (var entityName in entities)
    {
        var entity = context.Model.FindEntityType(entityName);
        var tableName = entity.GetTableName();
        var schemaName = entity.GetSchema();
        context.Database.ExecuteSqlRaw($"DELETE FROM {schemaName}.{tableName}");
        context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT (\"{schemaName}.{tableName}\", RESEED, 0);");
        if (entity.IsTemporal())
        {
            var strategy = context.Database.CreateExecutionStrategy();
            strategy.Execute(() =>
            {
                using var trans = context.Database.BeginTransaction();
                var designTimeEntity = designTimeModel.FindEntityType(entityName);
                var historySchema = designTimeEntity.GetHistoryTableSchema();
                var historyTable = designTimeEntity.GetHistoryTableName();
                context.Database.ExecuteSqlRaw($"ALTER TABLE {schemaName}.{tableName} SET (SYSTEM_VERSIONING = OFF)");
                context.Database.ExecuteSqlRaw($"DELETE FROM {historySchema}.{historyTable}");
                context.Database.ExecuteSqlRaw($"ALTER TABLE {schemaName}.{tableName} SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE={historySchema}.{historyTable}))");
                trans.Commit();
            });
        }
    }
}
//ClearSampleDataAndTemporal();