using AutoLot.Dal.Tests.Base;

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