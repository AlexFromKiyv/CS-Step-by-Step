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
        Assert.NotEmpty(cars);
        Assert.Equal(9, cars.Count());
    }

    [Fact]
    public void ShouldGetAllOfTheCars()
    {
        IQueryable<Car> query = Context.Cars.IgnoreQueryFilters();
        OutputHelper.WriteLine(query.ToQueryString());
        var cars = query.ToList();
        Assert.Equal(10, cars.Count());
    }

    [Fact]
    public void ShouldGetAllOfTheCarsWithMakes()
    {
        IIncludableQueryable<Car, Make> query = Context.Cars
            .Include(c => c.MakeNavigation);
        OutputHelper.WriteLine(query.ToQueryString());
        var cars = query.ToList();
        Assert.Equal(9, cars.Count());
    }

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

    [Fact]
    public void ShouldGetCarsOnOrderWithCustomerIgnoreQueryFilters()
    {
        IIncludableQueryable<Car, Customer?> query = Context.Cars
            .IgnoreQueryFilters()
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
        IQueryable<Car> query = Context.Cars
            .Where(c => c.Orders.Any())
            .Include(c => c.MakeNavigation)
            .Include(c => c.Orders)
            .ThenInclude(o => o.CustomerNavigation)
            .AsSplitQuery();
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

    [Fact]
    public void ShouldGetCarsOnOrderWithCustomerAsSplitQueryIgnoreQueryFilters()
    {
        IQueryable<Car> query = Context.Cars
            .IgnoreQueryFilters()
            .Where(c => c.Orders.Any())
            .Include(c => c.MakeNavigation)
            .Include(c => c.Orders)
            .ThenInclude(o => o.CustomerNavigation)
            .AsSplitQuery();
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
        Car? car = Context.Cars.First(c => c.Id == 1);
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
        Car? car = Context.Cars.First(c => c.Id == 1);
        Assert.Empty(car.Orders);
        var query = Context.Entry(car).Collection(c => c.Orders).Query();
        OutputHelper.WriteLine(query.ToQueryString());
        query.Load();
        Assert.Single(car.Orders);
    }

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
    }

    [Fact]
    public void ShouldNotGetAllCarsUsingFromSqlWithoutFilter()
    {
        var entity = Context.Model.FindEntityType(typeof(Car).FullName!);
        var tableName = entity!.GetTableName();
        var schemaName = entity!.GetSchema();

        string sql = $"Select *,PeriodStart,PeriodEnd from {schemaName}.{tableName}";
        var query = Context.Cars.FromSqlRaw(sql).IgnoreQueryFilters();
        OutputHelper.WriteLine(query.ToQueryString());

        var cars = query.ToList();
        Assert.Equal(10, cars.Count);
    }

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

    [Fact]
    public void ShouldGetTheCountOfCars()
    {
        var count = Context.Cars.Count();
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
            Assert.Equal(0, car.Id);

            int countAdded = Context.SaveChanges();

            int newCarCount = Context.Cars.Count();
            Assert.NotEqual(0, car.Id);
            Assert.Equal(1, countAdded);
            Assert.Equal(carCount + 1, newCarCount);
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
            var carCount = Context.Cars.Count();
            Context.Cars.Attach(car);
            Assert.Equal(EntityState.Added, Context.Entry(car).State);
            Context.SaveChanges();
            var newCarCount = Context.Cars.Count();
            Assert.Equal(carCount + 1, newCarCount);
        }
    }

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

}
