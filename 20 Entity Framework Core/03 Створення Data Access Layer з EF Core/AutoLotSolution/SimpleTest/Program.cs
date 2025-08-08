using AutoLot.Dal.EfStructures;
using AutoLot.Dal.Initialization;
using AutoLot.Dal.Repos;
using AutoLot.Models.Entities;
using AutoLot.Models.Entities.Owned;
using Microsoft.EntityFrameworkCore;

static void Run()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine($"\tWe have methods:\n\n" +
            $"0 All\n" +
            $"1 Test_Make_Car()\n" +
            $"2 Test_Car_Driver()\n" +
            $"3 Test_Car_Radio()\n" +
            $"4 Test_Customer()\n" +
            $"5 Test_Make()\n" +
            $"6 Test_CreditRisk()\n" +
            $"7 Test_Order()\n" +
            $"8 Test_CustomerOrderViewModel()\n" +
            $"9 Test_DB_Functions()\n" +
            $"10 Test_CarRepo()\n" +
            $"11 Test_InitializeData()\n" +
            $"12 Test_ClearAndSeedData()\n"
            );
        Console.Write("\tWhich method to run: ");
        int.TryParse(Console.ReadLine(), out int choice);

        Console.Clear();
        Console.WriteLine("\tResult:\n");
        switch (choice)
        {
            case 0:
                Test_Make_Car(); Test_Make_Car();Test_Car_Driver(); 
                Test_Car_Radio();Test_Customer(); Test_CreditRisk();
                Test_Order(); Test_CustomerOrderViewModel(); Test_DB_Functions(); 
                break;
            case 1: Test_Make_Car(); break;
            case 2: Test_Car_Driver(); break;
            case 3: Test_Car_Radio(); break;
            case 4: Test_Customer(); break;
            case 5: Test_Make_Car(); break;
            case 6: Test_CreditRisk(); break;
            case 7: Test_Order(); break;
            case 8: Test_CustomerOrderViewModel(); break;
            case 9: Test_DB_Functions(); break;
            case 10: Test_CarRepo(); break;
            case 11: Test_InitializeData(); break;
            case 12: Test_ClearAndSeedData(); break;
            default: break;
        }
        Console.Write("\n\tBack to menu");
        Console.ReadKey();
    }
}
Run();



static void Test_Make_Car()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    Make make = new Make() { Name = "VW" };
    context.Makes.Add(make);

    Car car = new() { MakeNavigation = make, Color = "Grey", PetName = "Wolf" };
    context.Cars.Add(car);

    context.SaveChanges();
    Console.WriteLine(car);

    Car? car1 = context.Cars.Find(car.Id);

    Console.WriteLine(car1);
}
Test_Make_Car();

static int Test_Car_Driver_Create()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    Make make = new Make() { Name = "VW" };
    context.Makes.Add(make);
    Car? car = new() { MakeNavigation = make, Color = "Black", PetName = "Wolf" };
    context.Cars.Add(car);

    Driver driver = new Driver()
    {
        PersonInformation =
        new Person { FirstName = "Sara", LastName = "Conor" }
    };

    ((List<Driver>)car.Drivers).Add(driver);
    context.SaveChanges();

    Console.WriteLine(car);
    Console.WriteLine($"{driver.Id} {driver.PersonInformation.FullName}");

    return car.Id;
}

static void Test_Car_Driver()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    int id = Test_Car_Driver_Create();

    Car car = context.Cars.Include(c=>c.MakeNavigation)
        .Include(c => c.CarDrivers)
        .ThenInclude(cd => cd.DriverNavigation)
        .Where(c => c.Id == id)
        .Single();

    Driver? driver = car.Drivers.First();

    Console.WriteLine(car);
    Console.WriteLine($"{driver.Id} {driver.PersonInformation.FullName}");
}
//Test_Car_Driver();

static void Test_Car_Radio()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    // Create
    Make make = new() { Name = "BWV" };
    context.Makes.Add(make);
    Car car = new() { MakeNavigation = make, Color = "Black", PetName = "Panter" };

    car.RadioNavigation = new Radio
    {
        HasTweeters = true,
        HasSubWoofers = true,
        RadioId = "RDV23451",
    };
    context.Cars.Add(car);
    context.SaveChanges();

    Car car_1 = context.Cars
        .Include(c => c.RadioNavigation)
        .Where(c => c.Id == car.Id)
        .Single();

    var radio = car_1.RadioNavigation;
    Console.WriteLine($"{radio.Id} {radio.RadioId}");
    Console.WriteLine(radio.CarNavigation);
}
//Test_Car_Radio();

static void Test_Customer()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    Customer customer = new Customer()
    {
        PersonInformation = new Person { FirstName = "Tommy", LastName = "Stark" }
    };

    context.Customers.Add(customer);
    context.SaveChanges();

    Customer customer_1 = context.Customers.Single(c => c.Id == customer.Id);
    Console.WriteLine($"{customer_1.Id} {customer_1.PersonInformation.FullName}");
}
//Test_Customer();

static void Test_Make()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    Make make = new Make { Name = "Toyota" };
    context.Makes.Add(make);
    context.SaveChanges();

    Make make_1 = context.Makes.Single(m => m.Id == make.Id);
    Console.WriteLine($"{make_1.Id} {make_1.Name}");
}
//Test_Make();

static void Test_CreditRisk()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    Customer customer = new Customer()
    {
        PersonInformation = new Person { FirstName = "James", LastName = "Bond" }
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

    CreditRisk? creditRisk1 = context.CreditRisks.Find(creditRisk.Id);
    Console.WriteLine($"" +
        $"{creditRisk1?.Id} " +
        $"{creditRisk1?.PersonInformation.FirstName} " +
        $"{creditRisk1?.PersonInformation.LastName}");
}
//Test_CreditRisk();

static void Test_Order()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    Customer customer = new Customer()
    {
        PersonInformation = new Person { FirstName = "Lara", LastName = "Croft" }
    };

    Make make = new Make() { Name = "Peugeot" };
    Car car = new() { MakeNavigation = make, Color = "Red", PetName = "Fox" };

    Order order = new Order
    {
        CustomerNavigation = customer,
        CarNavigation = car
    };
    context.Orders.Add(order);
    context.SaveChanges();

    Order order_1 = context.Orders
        .Include(o => o.CarNavigation)
        .Include(o => o.CustomerNavigation)
        .Single(o => o.Id == order.Id);

    Console.WriteLine($"" +
        $"{order_1.CarNavigation.PetName} " +
        $"{order_1.CustomerNavigation.PersonInformation.FirstName}");

}
//Test_Order();

static void Test_CustomerOrderViewModel()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    var customerOrders = context.CustomerOrderViewModels.ToList();
    foreach (var customerOrder in customerOrders)
    {
        Console.WriteLine(customerOrder.FullDetail);
    }
}
//Test_Order();
//Test_CustomerOrderViewModel();

static void Test_DB_Functions()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    var query_1 = context.Makes.Where(m => ApplicationDbContext.InventoryCountFor(m.Id) > 1);
    Console.WriteLine(query_1.ToQueryString());
    Console.WriteLine();

    Make make = context.Makes.First();
    var query_2 = context.GetCarsFor(make.Id);
    Console.WriteLine(query_2.ToQueryString());
    Console.WriteLine();

    List<Car>? cars = query_2.ToList();

    foreach (var car in cars)
    {
        Console.WriteLine(car);
    }
}
//Test_DB_Functions();

static void Test_CarRepo()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);

    CarRepo carRepo = new(context);
    ShowCars(carRepo.GetAll()); Console.WriteLine();
    ShowCars(carRepo.GetAllBy(1)); Console.WriteLine();
    Console.WriteLine(carRepo.GetPetName(1)); Console.WriteLine();
    Console.WriteLine(carRepo.Find(1));

    static void ShowCars(IEnumerable<Car> cars)
    {
        foreach (var car in cars)
        {
            Console.WriteLine(car);
        }
    }
}

static void Test_InitializeData()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    SampleDataInitializer.InitializeData(context);
}


static void Test_ClearAndSeedData()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    SampleDataInitializer.ClearAndSeedData(context);
}