﻿namespace AutoLot.Dal.Tests.IntegrationTests;

[Collection("Integration Tests")]
public class OrderTests : BaseTest, IClassFixture<EnsureAutoLotDatabaseTestFixture>
{
    private readonly IOrderRepo _repo;
    public OrderTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
        _repo = new OrderRepo(Context);
    }

    public override void Dispose()
    {
        _repo.Dispose();
        base.Dispose();
    }

    [Fact]
    public void ShouldGetAllOrdersExceptFiltered()
    {
        var query = Context.Orders;
        OutputHelper.WriteLine(query.ToQueryString());

        var orders = query.ToList();
        Assert.NotEmpty(orders);
        Assert.Equal(4, orders.Count);
    }

    [Fact]
    public void ShouldGetAllOrders()
    {
        var query = Context.Orders.IgnoreQueryFilters();
        OutputHelper.WriteLine(query.ToQueryString());

        var orders = query.ToList();
        Assert.NotEmpty(orders);
        Assert.Equal(5, orders.Count);
    }

}
