using AutoLot.Dal.Tests.Base;

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

    [Fact]
    public void ShouldGetCustomersWithLastNameStartWithW()
    {
        IQueryable<Customer> query = Context.Customers
            .Where(c => c.PersonInformation.LastName.StartsWith("W"));
        OutputHelper.WriteLine(query.ToQueryString());
        List<Customer> customers = query.ToList();

        Assert.Equal(2, customers.Count);

        foreach (var customer in customers)
        {
            Person? person = customer.PersonInformation;
            OutputHelper.WriteLine(customer.Id + "\t" + person.LastName);
            Assert.StartsWith("W", person.LastName, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    public void ShouldGetCustomersWithLastNameStartWithWAndFirstNameStartWithM()
    {
        IQueryable<Customer> query = Context.Customers
            //.Where(c => c.PersonInformation.LastName.StartsWith("W"))
            //.Where(c => c.PersonInformation.FirstName.StartsWith("M"));
            .Where(x => x.PersonInformation.LastName.StartsWith("W") &&
                           x.PersonInformation.FirstName.StartsWith("M"));
        OutputHelper.WriteLine(query.ToQueryString());
        List<Customer> customers = query.ToList();

        Assert.Single(customers);
        foreach (var customer in customers)
        {
            Person? person = customer.PersonInformation;
            Assert.StartsWith("W", person.LastName, StringComparison.OrdinalIgnoreCase);
            Assert.StartsWith("M", person.FirstName, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    public void ShouldGetCustomersWithLastNameStartWithWOrLastNameStartWithH()
    {
        IQueryable<Customer> query = Context.Customers
            .Where(c => c.PersonInformation.LastName.StartsWith("W") ||
                           c.PersonInformation.LastName.StartsWith("H"));
        OutputHelper.WriteLine(query.ToQueryString());
        List<Customer> customers = query.ToList();

        Assert.Equal(3, customers.Count);
        foreach (var customer in customers)
        {
            Person? person = customer.PersonInformation;
            OutputHelper.WriteLine(customer.Id + "\t" + person.LastName + "\t" + person.FirstName);
            Assert.True(
                    person.LastName.StartsWith("W", StringComparison.OrdinalIgnoreCase)
                 || person.LastName.StartsWith("H", StringComparison.OrdinalIgnoreCase)
                );
        }
    }

    [Fact]
    public void ShouldGetCustomersWithLastNameStartWithWOrLastNameStartWithHWithEFFunction()
    {
        IQueryable<Customer> query = Context.Customers
            .Where(c => EF.Functions.Like(c.PersonInformation.LastName, "W%")
            || EF.Functions.Like(c.PersonInformation.LastName, "H%"));
        OutputHelper.WriteLine(query.ToQueryString());
        List<Customer> customers = query.ToList();
        Assert.Equal(3, customers.Count);
    }

}