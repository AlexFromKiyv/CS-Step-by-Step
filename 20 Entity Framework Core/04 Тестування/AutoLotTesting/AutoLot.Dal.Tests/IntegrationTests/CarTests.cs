
namespace AutoLot.Dal.Tests.IntegrationTests;

[Collection("Integration Tests")]
public class CarTests : BaseTest, IClassFixture<EnsureAutoLotDatabaseTestFixture>
{
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

    [Fact]
    public void ShouldGetNotAllCars()
    {
        IQueryable<Car> query = Context.Cars.AsQueryable();
        OutputHelper.WriteLine(query.ToQueryString());
        var cars = query.ToList();
        Assert.NotEmpty(cars);
        foreach (var car in cars)
        {
            OutputHelper.WriteLine(car.ToString());
        }
    }

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
            .IgnoreQueryFilters().Where(c => c.MakeId == makeId);
        OutputHelper.WriteLine(query.ToQueryString());
        var cars = query.ToList();
        Assert.Equal(expectedCount, cars.Count());
    }

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


    [Fact]
    public void ShouldReturnDrivableCarsWithQueryFilterSet()
    {
        IQueryable<Car> query = Context.Cars;
        OutputHelper.WriteLine(query.ToQueryString());

        var cars = query.ToList();
        foreach (var car in cars)
        {
            OutputHelper.WriteLine(car.ToString());
        }

        Assert.NotEmpty(cars);
        Assert.Equal(9, cars.Count());
    }

    [Fact]
    public void ShouldGetAllOfTheCars()
    {
        IQueryable<Car> query = Context.Cars.IgnoreQueryFilters();
        OutputHelper.WriteLine(query.ToQueryString());

        var cars = query.ToList();
        foreach (var car in cars)
        {
            OutputHelper.WriteLine(car.ToString() + $"\t{car.IsDrivable}");
        }

        Assert.Equal(10, cars.Count());
    }

    [Fact]
    public void ShouldGetAllOfTheCarsWithMakes()
    {
        IIncludableQueryable<Car, Make> query = Context.Cars
            .Include(c => c.MakeNavigation);

        OutputHelper.WriteLine(query.ToQueryString());

        var cars = query.ToList();
        foreach (var car in cars)
        {
            OutputHelper.WriteLine(car.ToString());
        }

        Assert.Equal(9, cars.Count());
    }

    [Fact]
    public void ShouldGetCarsOnOrderWithCustomer()
    {
        IIncludableQueryable<Car, Customer?> query = Context.Cars.IgnoreQueryFilters()
            .Where(c => c.Orders.Any())
            .Include(c => c.MakeNavigation)
            .Include(c => c.Orders)
            .ThenInclude(o => o.CustomerNavigation);
        OutputHelper.WriteLine(query.ToQueryString());

        var cars = query.ToList();
        foreach (var car in cars)
        {
            OutputHelper.WriteLine($"{car.Id}\t{car.PetName}\t{car.MakeName}");
            foreach (var order in car.Orders)
            {
                OutputHelper.WriteLine(
                    $"\t{order.Id}\t"+
                    $"{order.CustomerId}\t"+
                    $"{order.CustomerNavigation.PersonInformation.LastName}\t");
            }
        }

        Assert.Equal(5, query.Count());
        cars.ForEach(c =>
        {
            Assert.NotNull(c.MakeNavigation);
            Assert.NotNull(c.Orders.ToList()[0].CustomerNavigation);
        });
    }

    [Fact]
    public void ShouldGetCarsOnOrderWithCustomerAsSplitQuery()
    {
        IQueryable<Car> query = Context.Cars.IgnoreQueryFilters()
            .Where(c => c.Orders.Any())
            .Include(c => c.MakeNavigation)
            .Include(c => c.Orders)
            .ThenInclude(o => o.CustomerNavigation)
            .AsSplitQuery();
        OutputHelper.WriteLine(query.ToQueryString());

        var cars = query.ToList();
        foreach (var car in cars)
        {
            OutputHelper.WriteLine($"{car.Id}\t{car.PetName}\t{car.MakeName}");
            foreach (var order in car.Orders)
            {
                OutputHelper.WriteLine(
                    $"\t{order.Id}\t" +
                    $"{order.CustomerId}\t" +
                    $"{order.CustomerNavigation.PersonInformation.LastName}\t");
            }
        }

        Assert.Equal(5, query.Count());
        cars.ForEach(c =>
        {
            Assert.NotNull(c.MakeNavigation);
            Assert.NotNull(c.Orders.ToList()[0].CustomerNavigation);
        });
    }




    [Fact]
    public void ShouldGetReferenceRelatedInformationExplicitly()
    {
        Car car = Context.Cars.First(c => c.Id == 1);
        OutputHelper.WriteLine(car.ToString());

        Assert.Null(car.MakeNavigation);

        var query = Context.Entry(car).Reference(c => c.MakeNavigation).Query();
        OutputHelper.WriteLine(query.ToQueryString());
        query.Load();
        OutputHelper.WriteLine(car?.MakeNavigation?.Name);
        
        Assert.NotNull(car?.MakeNavigation);
    }

    [Fact]
    public void ShouldGetCollectionRelatedInformationExplicitly()
    {
        Car car = Context.Cars.First(c => c.Id == 4);
        OutputHelper.WriteLine(car.ToString());
        OutputHelper.WriteLine(car.Orders.Count().ToString());
        Assert.Empty(car.Orders);
        
        var query = Context.Entry(car).Collection(c => c.Orders).Query();
        OutputHelper.WriteLine(query.ToQueryString());
        query.Load();
        OutputHelper.WriteLine(car.Orders.Count().ToString());
        Assert.NotEmpty(car.Orders);

        var orders = query.ToList();
        foreach (var order in orders)
        {
            OutputHelper.WriteLine(
                $"{order.Id}\t" +
                $"{order.CarId}\t" +
                $"{order.CustomerId}"
                );
        }
        Assert.Single(car.Orders);
        Assert.NotNull(orders[0].CarNavigation);
        Assert.Null(orders[0].CustomerNavigation);
    }

    [Fact]
    public void ShouldNotGetAllCarsUsingFromSql()
    {
        var entity = Context.Model.FindEntityType(typeof(Car).FullName!);
        var tableName = entity!.GetTableName();
        var schemaName = entity!.GetSchema();
        OutputHelper.WriteLine(tableName);
        OutputHelper.WriteLine(schemaName);

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

    [Fact]
    public void ShouldGetAllCarsUsingFromSql()
    {
        var entity = Context.Model.FindEntityType(typeof(Car).FullName!);
        var tableName = entity!.GetTableName();
        var schemaName = entity!.GetSchema();

        string sql = $"Select *,PeriodStart,PeriodEnd from {schemaName}.{tableName}";
        var query = Context.Cars.FromSqlRaw(sql).IgnoreQueryFilters();
        OutputHelper.WriteLine(query.ToQueryString());

        var cars = query.ToList();
        Assert.Equal(10, cars.Count);

        foreach (var car in cars)
        {
            OutputHelper.WriteLine(car.ToString());
        }
    }

    [Fact]
    public void ShouldGetOneCarUsingInterpolation()
    {
        int carId = 1;
        FormattableString sql = $"Select *,PeriodStart,PeriodEnd from dbo.Inventory where Id = {carId}";
        var query = Context.Cars.FromSqlInterpolated(sql).Include(c => c.MakeNavigation);
        OutputHelper.WriteLine(query.ToQueryString());

        Car? car = query.First();
        OutputHelper.WriteLine(car.ToString());

        Assert.Equal("Black", car.Color);
        Assert.Equal("VW", car.MakeNavigation.Name);
    }

    [Fact]
    public void ShouldGetTheCountOfCars()
    {
        var count = Context.Cars.Count();
        OutputHelper.WriteLine(count.ToString());
        Assert.Equal(9, count);
    }

    [Fact]
    public void ShouldGetTheCountOfCarsIgnoreQueryFilters()
    {
        var count = Context.Cars.IgnoreQueryFilters().Count();
        Assert.Equal(10, count);
    }

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

    [Theory]
    [InlineData(1, true)]
    [InlineData(11, false)]
    public void ShouldCheckForAnyCarsWithMake(int makeId, bool expectedResult)
    {
        var result = Context.Cars.Any(c => c.MakeId == makeId);
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(1, false)]
    [InlineData(11, false)]
    public void ShouldCheckForAllCarsWithMake(int makeId, bool expectedResult)
    {
        var result = Context.Cars.All(x => x.MakeId == makeId);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void ShouldCheckNotAllCarsIsDrivable()
    {
        var result = Context.Cars.IgnoreQueryFilters().All(c => c.IsDrivable);
        Assert.False(result);
    } 


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
    [InlineData(10, "Lemon")]
    public void ShouldGetValueFromStoredProc(int id, string expectedName)
    {
        string petName = _carRepo.GetPetName(id);
        OutputHelper.WriteLine($"{id}\t{petName}");

        Assert.Equal(expectedName, _carRepo.GetPetName(id));
    }

    //CRUD

    [Fact]
    public void ShouldAddACar()
    {
        ExecuteInATransaction(RunTheTest);

        void RunTheTest()
        {
            Car car = new Car
            {
                Color = "Yellow",
                MakeId = 1,
                PetName = "Herbie"
            };
            OutputHelper.WriteLine(car.ToString());

            int carCount = Context.Cars.Count();
            OutputHelper.WriteLine(carCount.ToString());


            Context.Cars.Add(car);
            Assert.Equal(0, car.Id);

            int countAdded = Context.SaveChanges();
           
            Assert.NotEqual(0, car.Id);
            Assert.Equal(1, countAdded);

            int newCarCount = Context.Cars.Count();
            Assert.Equal(carCount + 1, newCarCount);

            OutputHelper.WriteLine(newCarCount.ToString());
            OutputHelper.WriteLine(car.ToString());
        }
    }

    [Fact]
    public void ShouldAddAndReadACar()
    {
        ExecuteInATransaction(RunTheTest);

        void RunTheTest()
        {
            //Create in DB
            Car car = new()
            {
                Color = "Gray",
                MakeId = 2,
                PetName = "Wolf",
                Price ="2000",
            };
            OutputHelper.WriteLine(car.ToString());
            Context.Cars.Add(car);
            int countAdded = Context.SaveChanges();
            OutputHelper.WriteLine(car.ToString());

            int id = car.Id;
            string? dateBuilt = car.DateBuilt.ToString();
            string? timeStamp = car.TimeStamp.ToString();

            OutputHelper.WriteLine(countAdded.ToString());

            Assert.Equal(1, countAdded);

            Context.ChangeTracker.Clear();

            // Reade from DB
            Car? newCar = Context.Cars.Find(id);
            Assert.False(object.ReferenceEquals(car, newCar));

            OutputHelper.WriteLine(newCar?.ToString());

            Assert.NotNull(newCar);
            Assert.Equal(id, newCar.Id);
            Assert.Equal(2, newCar.MakeId);
            Assert.Equal("Gray", newCar.Color);
            Assert.Equal("Wolf", newCar.PetName);
            Assert.Equal(dateBuilt, newCar.DateBuilt.ToString());
            Assert.True(newCar.IsDrivable);
            Assert.Equal("2000.00", newCar.Price);
            Assert.Equal("Wolf (Gray)", newCar.Display);
            Assert.Equal(timeStamp, newCar.TimeStamp.ToString());
        }
    }

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
            OutputHelper.WriteLine(car.ToString());

            var carCount = Context.Cars.Count();
            Context.Cars.Attach(car);
            OutputHelper.WriteLine(car.ToString());

            Assert.Equal(EntityState.Added, Context.Entry(car).State);
            
            Context.SaveChanges();
            OutputHelper.WriteLine(car.ToString());
            var newCarCount = Context.Cars.Count();
            Assert.Equal(carCount + 1, newCarCount);
        }
    }

    [Fact]
    public void ShouldAddAndReadACarUsingRepo()
    {
        ExecuteInATransaction(RunTheTest);

        void RunTheTest()
        {
            //Create in DB
            Car car = new()
            {
                Color = "Gray",
                MakeId = 2,
                PetName = "Wolf",
                Price = "2000"
            };
            
            int countAdded = _carRepo.Add(car);

            int id = car.Id;
            string? dateBuilt = car.DateBuilt.ToString();
            string? timeStamp = car.TimeStamp.ToString();

            OutputHelper.WriteLine(car.ToString());
            OutputHelper.WriteLine(countAdded.ToString());

            Assert.Equal(1, countAdded);

            // Detached
            _carRepo.Context.ChangeTracker.Clear();
            Assert.Equal(EntityState.Detached, Context.Entry(car).State);

            // Reade from DB
            Car? newCar = _carRepo.Find(id);
            if (newCar != null)
            {
                Assert.Equal(EntityState.Unchanged, Context.Entry(newCar).State);
            }

            Assert.False(object.ReferenceEquals(car, newCar));

            OutputHelper.WriteLine(newCar?.ToString());

            Assert.NotNull(newCar);
            Assert.Equal(id, newCar.Id);
            Assert.Equal(2, newCar.MakeId);
            Assert.Equal("Gray", newCar.Color);
            Assert.Equal("Wolf", newCar.PetName);
            Assert.Equal(dateBuilt, newCar.DateBuilt.ToString());
            Assert.True(newCar.IsDrivable);
            Assert.Equal("2000.00",newCar.Price);
            Assert.Equal("Wolf (Gray)", car.Display);
            Assert.Equal(timeStamp, newCar.TimeStamp.ToString());
        }
    }

    [Fact]
    public void ShouldAddAndReadACarUsingRepoWithOtherContext()
    {
        ExecuteInASharedTransaction(RunTheTest);

        void RunTheTest(IDbContextTransaction transaction)
        {
            //Create in DB
            Car car = new()
            {
                Color = "Gray",
                MakeId = 2,
                PetName = "Wolf",
                Price = "2000"
            };
            OutputHelper.WriteLine(car.ToString());

            int countAdded = _carRepo.Add(car);

            int id = car.Id;
            string? dateBuilt = car.DateBuilt.ToString();
            string? timeStamp = car.TimeStamp.ToString();

            OutputHelper.WriteLine(car.ToString());
            OutputHelper.WriteLine(countAdded.ToString());

            Assert.Equal(1, countAdded);


            // Reade from DB
            var otherContext = TestHelpers.GetSecondContext(Context, transaction);
            Car? otherCar = otherContext.Cars.Find(id);
            OutputHelper.WriteLine(otherCar?.ToString());

            Assert.NotNull(otherCar);
            Assert.Equal(id, otherCar.Id);
            Assert.Equal(2, otherCar.MakeId);
            Assert.Equal("Gray", otherCar.Color);
            Assert.Equal("Wolf", otherCar.PetName);
            Assert.Equal(dateBuilt, otherCar.DateBuilt.ToString());
            Assert.True(otherCar.IsDrivable);
            Assert.Equal("2000.00", otherCar.Price);
            Assert.Equal("Wolf (Gray)", car.Display);
            Assert.Equal(timeStamp, otherCar.TimeStamp.ToString());
        }
    }

    [Fact]
    public void ShouldAddMultipleCars()
    {
        ExecuteInATransaction(RunTheTest);

        void RunTheTest()
        {
            int carCount = Context.Cars.Count();
            OutputHelper.WriteLine(carCount.ToString());

            //Have to add 4 to activate batching
            var cars = new List<Car>
                {
                    new() {Color = "Yellow", MakeId = 1, PetName = "Herbie"},
                    new() {Color = "White", MakeId = 2, PetName = "Mach 5"},
                    new() {Color = "Pink", MakeId = 3, PetName = "Avon"},
                    new() {Color = "Blue", MakeId = 4, PetName = "Blueberry"},
                };
            Context.Cars.AddRange(cars);
            Context.SaveChanges();
            int newCarCount = Context.Cars.Count();
            OutputHelper.WriteLine(newCarCount.ToString());
            Assert.Equal(carCount + 4, newCarCount);

            foreach (var car in Context.Cars)
            {
                OutputHelper.WriteLine(car.ToString());
            }
        }
    }

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
                    RadioId = "KISS FM"
                }
            };

            make.Cars.Add(car);
            Context.Makes.Add(make);
            int carCount = Context.Cars.Count();
            int makeCount = Context.Makes.Count();
            int radioCount = Context.Radios.Count();
            Context.SaveChanges();

            OutputHelper.WriteLine(make.Id.ToString());
            OutputHelper.WriteLine(car.ToString());

            int newCarCount = Context.Cars.Count();
            int newMakeCount = Context.Makes.Count();
            int newRadioCount = Context.Radios.Count();

            Assert.Equal(7, car.MakeId);
            Assert.Equal(7, make.Id);
            Assert.Equal(carCount + 1, newCarCount);
            Assert.Equal(makeCount + 1, newMakeCount);
            Assert.Equal(radioCount + 1, newRadioCount);

            Radio? radio = Context.Radios.Where(r => r.CarId == car.Id).FirstOrDefault();
            OutputHelper.WriteLine(radio?.RadioId);

            Assert.Equal("KISS FM", radio?.RadioId);
        }
    }

    [Fact]
    public void ShouldUpdateACar()
    {
        ExecuteInASharedTransaction(RunTheTest);

        void RunTheTest(IDbContextTransaction transaction)
        {
            Car? car = Context.Cars.Find(1);
            OutputHelper.WriteLine(car?.ToString());

            Assert.Equal("Black", car?.Color);
            if (car != null)
            {
                car.Color = "White";
            }
            OutputHelper.WriteLine(car?.ToString());

            //Calling update is not needed because the entity is tracked
            //Context.Cars.Update(car);
            Context.SaveChanges();

            var otherContext = TestHelpers.GetSecondContext(Context, transaction);
            Car? otherCar = otherContext.Cars.Find(1);
            Assert.Equal("White", otherCar?.Color);
            OutputHelper.WriteLine(otherCar?.ToString());
        }
    }

    [Fact]
    public void ShouldUpdateACarAsNoTracking()
    {
        ExecuteInASharedTransaction(RunTheTest);
        void RunTheTest(IDbContextTransaction transaction)
        {
            Car? car = Context.Cars.AsNoTracking().First(c => c.Id == 1);
            OutputHelper.WriteLine(car.ToString());

            Car updatedCar = new()
                {
                    Id = car.Id,
                    Color = "White",
                    MakeId = car.MakeId,
                    PetName = car.PetName,
                    TimeStamp = car.TimeStamp,
                    DateBuilt = car.DateBuilt,
                    Price = car.Price,
                    IsDrivable = car.IsDrivable
                };


            var context2 = TestHelpers.GetSecondContext(Context, transaction);
            context2.Cars.Update(updatedCar);
            //context2.Entry(updatedCar).State = EntityState.Modified;
            context2.SaveChanges();

            var context3 = TestHelpers.GetSecondContext(Context, transaction);
            Car? otherCar = context3.Cars.Find(1);
            OutputHelper.WriteLine(otherCar?.ToString());
            Assert.Equal("White", otherCar?.Color);
        }
    }

    [Fact]
    public void ShouldThrowConcurrencyException()
    {
        ExecuteInATransaction(RunTheTest);
        void RunTheTest()
        {
            var car = Context.Cars.First();
            OutputHelper.WriteLine(car.ToString());

            //Update the database outside of the context
            FormattableString sql =
                $"Update dbo.Inventory set Color='Pink' where Id = {car.Id}";
            Context.Database.ExecuteSqlInterpolated(sql);

            //update the car record in the change tracker
            car.Color = "Yellow";
            OutputHelper.WriteLine(car.ToString());

            var ex = Assert.Throws<CustomConcurrencyException>(() => Context.SaveChanges());
            OutputHelper.WriteLine(ex.InnerException.Message);

            var entry = ((DbUpdateConcurrencyException)ex.InnerException)?.Entries[0];
            PropertyValues originalProps = entry.OriginalValues;
            PropertyValues currentProps = entry.CurrentValues;
            //This needs another database call
            PropertyValues databaseProps = entry.GetDatabaseValues();
        }
    }

    [Fact]
    public void ShouldRemoveACar()
    {
        ExecuteInATransaction(RunTheTest);
        void RunTheTest()
        {
            int carCount = Context.Cars.Count();
            Car? car = Context.Cars.Find(9);
            if (car != null)
            {
                Context.Cars.Remove(car);
                //Context.Entry(car).State = EntityState.Deleted;
            }
            Context.SaveChanges();
            
            Car? result = Context.Cars.Find(9);
            Assert.Null(result);
            Assert.Equal(carCount - 1, Context.Cars.Count());
            Assert.Equal(EntityState.Detached, Context.Entry(car).State);
            OutputHelper.WriteLine(car?.ToString());
        }
    }

    [Fact]
    public void ShouldRemoveACarAsNoTracking()
    {
        ExecuteInASharedTransaction(RunTheTest);
        void RunTheTest(IDbContextTransaction transacton)
        {
            var context1 = TestHelpers.GetSecondContext(Context, transacton);
            int countCar = context1.Cars.Count();
            OutputHelper.WriteLine(countCar.ToString());
            
            Car? car = context1.Cars.AsNoTracking().First(c => c.Id == 9);
            OutputHelper.WriteLine(car.ToString());

            var context2 = TestHelpers.GetSecondContext(Context, transacton);
            context2.Cars.Remove(car);
            //context2.Entry(car).State = EntityState.Deleted;
            context2.SaveChanges();

            int newCountCar = context2.Cars.Count();
            OutputHelper.WriteLine(newCountCar.ToString());

            Assert.Equal(countCar - 1, newCountCar);
            Assert.Equal(EntityState.Detached, Context.Entry(car).State);
        }
    }

    [Fact]
    public void ShouldFailToRemoveACar()
    {
        ExecuteInATransaction(RunTheTest);
        void RunTheTest()
        {
            Car? car = Context.Cars.Find(1);
            if( car != null)
            {
                Context.Cars.Remove(car);
            }
            var ex = Assert.Throws<CustomDbUpdateException>(() => Context.SaveChanges());
            OutputHelper.WriteLine(ex?.InnerException?.InnerException?.Message);
        }
    }

}
