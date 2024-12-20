using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            .IgnoreQueryFilters().Where(c=>c.MakeId ==  makeId);
        OutputHelper.WriteLine(query.ToQueryString());
        var cars = query.ToList();
        Assert.Equal(expectedCount,cars.Count());
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
}
