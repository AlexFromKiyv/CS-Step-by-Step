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
    public void ShouldSortByLastNameThenByDescendingFirstName()
    {
        var query = Context.Customers
            .OrderBy(c => c.PersonInformation.LastName)
            .ThenByDescending(c => c.PersonInformation.FirstName);
        OutputHelper.WriteLine(query.ToQueryString()+"\n");

        var customers = query.ToList();

        foreach (var customer in customers)
        {
            OutputHelper.WriteLine(
                $"{customer.PersonInformation.LastName} " +
                $"{customer.PersonInformation.FirstName}");
        }

        for (int i = 0; i < customers.Count-1; i++)
        {
            Compare(customers[i].PersonInformation, customers[i+1].PersonInformation);
        }
        
        static void Compare(Person person1, Person person2)
        {
            var compareResult = string.Compare(person1.LastName, person2.LastName,
                StringComparison.CurrentCultureIgnoreCase);
            Assert.True(compareResult <= 0);
            if (compareResult == 0)
            {
                Assert.True(string.Compare(person1.FirstName, person2.FirstName,
                    StringComparison.CurrentCultureIgnoreCase) >= 0);
            }
        }
    }

    [Fact]
    public void ShouldSortByFirstNameThenLastNameUsingReverse()
    {
        var query = Context.Customers
    .OrderBy(c => c.PersonInformation.LastName)
    .ThenByDescending(c => c.PersonInformation.FirstName)
    .Reverse();
        OutputHelper.WriteLine(query.ToQueryString() + "\n");

        var customers = query.ToList();

        foreach (var customer in customers)
        {
            OutputHelper.WriteLine(
                $"{customer.PersonInformation.LastName} " +
                $"{customer.PersonInformation.FirstName}");
        }

        //if only one customer, nothing to test
        if (customers.Count <= 1) { return; }
        for (int x = 0; x < customers.Count - 1; x++)
        {
            var pi1 = customers[x].PersonInformation;
            var pi2 = customers[x + 1].PersonInformation;
            var compareLastName = string.Compare(pi1.LastName,
            pi2.LastName, StringComparison.CurrentCultureIgnoreCase);
            Assert.True(compareLastName >= 0);
            if (compareLastName != 0) continue;
            var compareFirstName = string.Compare(pi1.FirstName,
            pi2.FirstName, StringComparison.CurrentCultureIgnoreCase);
            Assert.True(compareFirstName <= 0);
        }
    }

    [Fact]
    public void GetFirstMatchingRecordDatabaseOrder()
    {
        var customer = Context.Customers.First();
        OutputHelper.WriteLine($"{customer.Id}");
        Assert.Equal(1, customer.Id);
    }

    [Fact]
    public void GetFirstMatchingRecordNameOrder()
    {
        var customer = Context.Customers
            .OrderBy(c=>c.PersonInformation.LastName)
            .ThenBy(c=>c.PersonInformation.FirstName)
            .First();
        OutputHelper.WriteLine($"{customer.Id}");
        Assert.Equal(1, customer.Id);
    }

    [Fact]
    public void FirstShouldThrowExceptionIfNoneMatch()
    {
        //Filters based on Id. Throws due to no match
        Assert.Throws<InvalidOperationException>(() => Context.Customers.First(c => c.Id == 10)); 
    }

    [Fact]
    public void FirstOrDefaultShouldReturnDefaultIfNoneMatch()
    {
        Expression<Func<Customer, bool>> expression = c => c.Id == 10;
        var customer = Context.Customers.FirstOrDefault(expression);
        Assert.Null(customer);
    }

    [Fact]
    public void GetLastMatchingRecordNameOrder()
    {
        var customer = Context.Customers
          .OrderBy(c => c.PersonInformation.LastName)
          .ThenBy(c => c.PersonInformation.FirstName)
          .Last();
        Assert.Equal(4, customer.Id);
    }

    [Fact]
    public void LastShouldThrowIfNoSortSpecified()
    {
        Assert.Throws<InvalidOperationException>(() => Context.Customers.Last());
    }

    [Fact]
    public void GetOneMatchingRecordWithSingle()
    {
        var customer = Context.Customers.Single(x => x.Id == 1);
        Assert.Equal(1, customer.Id);
    }

    [Fact]
    public void SingleShouldThrowExceptionIfNoneMatch()
    {
        //Filters based on Id. Throws due to no match
        Assert.Throws<InvalidOperationException>(() => Context.Customers.Single(x => x.Id == 10));
    }

    [Fact]
    public void SingleShouldThrowExceptionIfMoreThenOneMatch()
    {
        // Throws due to more than one match
        Assert.Throws<InvalidOperationException>(() => Context.Customers.Single());
    }
    [Fact]
    public void SingleOrDefaultShouldThrowExceptionIfMoreThenOneMatch()
    {
        // Throws due to more than one match
        Assert.Throws<InvalidOperationException>(() => Context.Customers.SingleOrDefault());
    }

    [Fact]
    public void SingleOrDefaultShouldReturnDefaultIfNoneMatch()
    {
        //Expression<Func<Customer>> is a lambda expression
        Expression<Func<Customer, bool>> expression = x => x.Id == 10;
        //Returns null when nothing is found
        var customer = Context.Customers.SingleOrDefault(expression);
        Assert.Null(customer);
    }

}


