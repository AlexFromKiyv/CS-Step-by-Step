using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Dal.Tests.IntegrationTests;

[Collection("Integration Tests")]
public class MakeTests : BaseTest, IClassFixture<EnsureAutoLotDatabaseTestFixture>
{
    private readonly IMakeRepo _repo;
    public MakeTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
        _repo = new MakeRepo(Context);
    }

    public override void Dispose()
    {
        _repo.Dispose();
        base.Dispose();
    }


    [Fact]
    public void ShouldGetAllMakesAndCarsThatAreYellow()
    {
        IQueryable<Make> query = Context.Makes
            .IgnoreQueryFilters()
            .Include(m => m.Cars.Where(c => c.Color == "Yellow"));
        OutputHelper.WriteLine(query.ToQueryString());

        List<Make> makes = query.ToList();

        foreach (var make in makes)
        {
            OutputHelper.WriteLine($"{make.Id} {make.Name}");
            foreach (var car in make.Cars)
            {
                OutputHelper.WriteLine($"\t{car.Id} {car.MakeName} {car.PetName}");
            }
        }
        Assert.NotNull(makes);
        Assert.NotEmpty(makes);
        Assert.Contains(makes, m => m.Cars.Any());
        Assert.Empty(makes.First(m => m.Id == 1).Cars);
        Assert.Empty(makes.First(m => m.Id == 2).Cars);
        Assert.Empty(makes.First(m => m.Id == 3).Cars);
        Assert.Empty(makes.First(m => m.Id == 5).Cars);
        Assert.NotEmpty(makes.First(m => m.Id == 4).Cars);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(3, 1)]
    [InlineData(4, 2)]
    [InlineData(5, 3)]
    [InlineData(6, 1)]
    public void ShouldGetAllCarsForAMakeExplicitlyWithQueryFilters(int makeId, int carCount)
    {
        Make? make = Context.Makes.Single(m => m.Id == makeId);
        IQueryable <Car> query = Context.Entry(make).Collection(m => m.Cars).Query();
        OutputHelper.WriteLine(query.ToQueryString());
        query.Load();
        Assert.Equal(carCount, make.Cars.Count);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 1)]
    [InlineData(3, 1)]
    [InlineData(4, 2)]
    [InlineData(5, 3)]
    [InlineData(6, 1)]
    public void ShouldGetAllCarsForAMakeExplicitlyWithoutQueryFilters(int makeId, int carCount)
    {
        Make? make = Context.Makes.Single(m => m.Id == makeId);
        IQueryable<Car> query = Context.Entry(make).Collection(m => m.Cars)
            .Query().IgnoreQueryFilters();
        OutputHelper.WriteLine(query.ToQueryString());
        query.Load();
        Assert.Equal(carCount, make.Cars.Count);
    }

    [Fact]
    public void ShouldGetAllHistoryRows()
    {
        Make make = new Make { Name = "Make" };
        _repo.Add(make);
        Thread.Sleep(1000);
        make.Name = "NewMake";
        _repo.Update(make);
        Thread.Sleep(1000);
        _repo.Delete(make);

        var list = _repo.GetAllHistory().Where(m => m.Entity.Id == make.Id).ToList();
        Assert.Equal(2, list.Count);
        Assert.Equal("Make", list[0].Entity.Name);
        Assert.Equal("NewMake", list[1].Entity.Name);
        Assert.Equal(list[0].PeriodEnd, list[1].PeriodStart);
    }

}
