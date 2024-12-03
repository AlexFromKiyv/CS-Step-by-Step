
using AutoLot.Dal.EfStructures;
using AutoLot.Dal.EfStructures.Migrations;
using AutoLot.Models.Entities;
using AutoLot.Models.Entities.Owned;
using Microsoft.EntityFrameworkCore;

static void Test_Make_Car()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    Make make = new Make()  {  Name = "VW" };
    context.Makes.Add(make);

    Car car = new() {  MakeNavigation = make, Color = "Navy", PetName = "Wolf" };
    context.Cars.Add(car);

    context.SaveChanges();

    Console.WriteLine($"{make.Id} {make.Name}");
    Console.WriteLine(car);

    context.Cars.Remove(car);
    context.Makes.Remove(make);
    context.SaveChanges();
}
//Test_Make_Car();

static void Test_Car_Driver()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    // Create
    Make make = new Make() { Name = "VW" };
    context.Makes.Add(make);
    Car? car = new() { MakeNavigation = make, Color = "Navy", PetName = "Wolf" };
    context.Cars.Add(car);

    Driver driver = new Driver()
    {
        PersonInformation =
        new Person { FirstName = "John", LastName = "Conor" }
    };

    ((List<Driver>)car.Drivers).Add(driver);
    context.SaveChanges();


    // Read
    Car? car_1 = context.Cars
        .Where(c => c.PetName == "Wolf")
        .Include("Drivers").First();

    Driver? driver_1 = car_1.Drivers.First();

    Console.WriteLine(car_1);
    Console.WriteLine($"{driver_1.Id} {driver_1.PersonInformation.FullName}");

    context.Cars.Remove(car);
    context.Drivers.Remove(driver);
    context.SaveChanges();
}
//Test_Car_Driver();

static void Test_Car_Radio()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    // Create
    Make make = new Make() { Name = "VW" };
    context.Makes.Add(make);
    Car? car = new() { MakeNavigation = make, Color = "Navy", PetName = "Wolf" };

    car.RadioNavigation = new Radio
    {
        HasTweeters = true,
        HasSubWoofers = true,
        RadioId ="RDV23451",
    };

    context.Cars.Add(car);
    context.SaveChanges();

    var radio = context.Radios.First();
    Console.WriteLine($"{radio.Id} {radio.RadioId}");
    Console.WriteLine(radio.CarNavigation);

    context.Radios.Remove(radio);
    context.Cars.Remove(car);
    context.SaveChanges();
}
//Test_Car_Radio();

static void Test_Customer()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    Customer customer = new Customer()
    {
        PersonInformation = new Person { FirstName = "John", LastName = "Conor" }
    };

    context.Customers.Add(customer);
    context.SaveChanges();

    Customer customer_1 = context.Customers.First();
    Console.WriteLine($"{customer_1.Id} {customer_1.PersonInformation.FullName}");

    context.Customers.Remove(customer);
    context.SaveChanges();
}
//Test_Customer();

static void Test_Make()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    Make make = new Make { Name = "BMW" };
    context.Makes.Add(make);
    context.SaveChanges();

    Make make1 = context.Makes.First(m => m.Name == "BMW");
    Console.WriteLine($"{make1.Id} {make1.Name}");
    context.Makes.Remove(make);
    context.SaveChanges();
}
//Test_Make();

static void Test_CreditRisk()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    Customer customer = new Customer()
    {
        PersonInformation = new Person { FirstName = "John", LastName = "Conor" }
    };

    CreditRisk creditRisk = new CreditRisk
    {
        PersonInformation = new Person 
        { 
            FirstName = customer.PersonInformation.FirstName,
            LastName = customer.PersonInformation.LastName            
        },
        CustomerNavigation = customer
    };

    context.CreditRisks.Add(creditRisk);
    context.SaveChanges();

    CreditRisk creditRisk1 = context.CreditRisks.Find(creditRisk.Id);
    Console.WriteLine($"" +
        $"{creditRisk1.Id} " +
        $"{creditRisk1.PersonInformation.FirstName} " +
        $"{creditRisk1.PersonInformation.LastName}");

    context.CreditRisks.Remove(creditRisk);
    context.Customers.Remove(customer);
    context.SaveChanges();
}
//Test_CreditRisk();

static void Test_Order()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    Customer customer = new Customer()
    {
        PersonInformation = new Person { FirstName = "John", LastName = "Conor" }
    };

    Make make = new Make() { Name = "VW" };
    Car car = new() { MakeNavigation = make, Color = "Navy", PetName = "Wolf" };

    Order order = new Order
    {
        CustomerNavigation = customer,
        CarNavigation = car
    };

    context.Orders.Add(order);
    context.SaveChanges();

    Order order1 = context.Orders
        .Include(o=>o.CarNavigation)
        .Include(o=>o.CustomerNavigation)
        .First();

    Console.WriteLine($"" +
        $"{order1.CarNavigation.PetName} " +
        $"{order1.CustomerNavigation.PersonInformation.FirstName}");

    context.Orders.Remove(order);
    context.Cars.Remove(car);
    context.Customers.Remove(customer);
    context.SaveChanges();
}
//Test_Order();