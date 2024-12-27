
namespace AutoLot.Dal.Tests.IntegrationTests;

[Collection("Integration Tests")]
public class CustomerOrderViewModelTests : BaseTest, IClassFixture<EnsureAutoLotDatabaseTestFixture>
{
    private readonly ICustomerOrderViewModelRepo _repo;
    public CustomerOrderViewModelTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
        _repo = new CustomerOrderViewModelRepo(Context);
    }

    public override void Dispose()
    {
        _repo.Dispose();
        base.Dispose();
    }
    [Fact]
    public void ShouldGetAllViewModels()
    {
        var query = Context.CustomerOrderViewModels;
        OutputHelper.WriteLine(query.ToQueryString());
        var list = query.ToList();
        Assert.NotEmpty(list);
        Assert.Equal(5, list.Count);
    }

}
