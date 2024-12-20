using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Dal.Tests.IntegrationTests;

[Collection("Integration Tests")]
public class CustomerTests(ITestOutputHelper outputHelper) : BaseTest(outputHelper), IClassFixture<EnsureAutoLotDatabaseTestFixture>
{
    [Fact]
    public void SouldGetAllOfTheCustomers()
    {
        var query = Context.Customers;
        OutputHelper.WriteLine(query.ToQueryString());
        var customers = query.ToList();
        Assert.Equal(5, customers.Count);
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
            Assert.True(
                    person.LastName.StartsWith("W",StringComparison.OrdinalIgnoreCase) 
                 || person.LastName.StartsWith("H",StringComparison.OrdinalIgnoreCase)
                );
        }
    }

    [Fact]
    public void ShouldGetCustomersWithLastNameStartWithWOrLastNameStartWithHWithEFFunction()
    {
        IQueryable<Customer> query = Context.Customers
            .Where(c => EF.Functions.Like(c.PersonInformation.LastName,"W%")
            || EF.Functions.Like(c.PersonInformation.LastName, "H%"));
        OutputHelper.WriteLine(query.ToQueryString());
        List<Customer> customers = query.ToList();
        Assert.Equal(3, customers.Count);
    }

    [Fact]
    public void ShouldGetCustomersWithId1()
    {
        Customer? customer = Context.Customers.Find(1);

        Assert.Equal("Dave", customer?.PersonInformation.FirstName);
        Assert.Equal("Brenner", customer?.PersonInformation.LastName);
    }

}


