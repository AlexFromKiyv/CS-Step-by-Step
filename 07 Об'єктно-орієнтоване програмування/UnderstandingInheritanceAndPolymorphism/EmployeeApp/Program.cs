
using EmployeeApp;

void UsingEmployee6Class()
{
    Employee6 employee = new(23, "Marvin", 1000, 35,
        "111 - 11 - 1111", EmployeePayTypeEnum.Salaried);
    employee.DisplayStatus();
    employee.GiveBonus(100);
    employee.DisplayStatus();
}
//UsingEmployee6Class();

void UsingSalesPerson6()
{
    SalesPerson6 salesPerson = new()
    {
        Name = "Fred",
        Age = 31,
        SalesNumber = 50
    };

    salesPerson.DisplayStatus();
}
//UsingSalesPerson6();

void CallingBaseClassConstructors()
{
    Manager6 manager = new Manager6(5, "Alexandr", 1000, 35, "1234-234-32",
        123);
    manager.DisplayStatus();
}
//CallingBaseClassConstructors();

void TheProtectedKeyword()
{
    //Employee7 employee = new();
    // Error! Can't access protected data from client code.
    //employee.name = "John";
}

void ClassUseOtherClass()
{
    Manager7 manager = new Manager7(15, "John", 100000, 50, "333-23-2322", 9000);
    manager.DisplayStatus();
    Console.WriteLine($"Benefit Cost:{manager.GetBenefitCost()}");

    Employee7.BenefitPackage.BenefitPackageLevel packageLevel =
        Employee7.BenefitPackage.BenefitPackageLevel.Platinum;
    Console.WriteLine(packageLevel);
}
//ClassUseOtherClass();

void UsingGiveBonus()
{
    Manager7 manager = new(3, "Jack", 100000, 50, "3256-56-2536", 9000);
    manager.GiveBonus(300);
    manager.DisplayStatus();

    SalesPerson7 salesPerson = new(8, "Olga", 3000, 35, "2342-34-3432", 31);
    salesPerson.GiveBonus(200);
    salesPerson.DisplayStatus();
}
//UsingGiveBonus();

void UsingEmployee()
{
    // Error! Cannot create an instance of an abstract class!
    //Employee7 X = new Employee7();
}

void CastingExample1()
{
    // A Manager 'is-a' System.Object, so we can
    // store a Manager reference in an object variable just fine.
    object frank = new Manager7(9, "Frank Zappa", 40000, 45, "111-11-1111", 5);

    // A Manager 'is-an' Employee too.
    Employee7 moonUnit = new Manager7(10, "MoonUnit Zappa", 30000, 35, "111-12-1234", 3);

    SalesPerson7 jill = new SalesPerson7(12, "Jill", 10000, 25, "234-23-4335", 30);
}

static void GivePromotion(Employee7 employee)
{
    // Increase pay...
    // Give new parking space in company garage...
    Console.WriteLine($"{employee.Name} was promoted!");
}

void CastingExample2()
{
    Employee7 moonUnit = new Manager7(10, "MoonUnit Zappa", 30000, 35, "111-12-1234", 3);
    GivePromotion(moonUnit);

    SalesPerson7 jill = new SalesPerson7(12, "Jill", 10000, 25, "234-23-4335", 30);
    GivePromotion(jill);

    object frank = new Manager7(9, "Frank Zappa", 40000, 45, "111-11-1111", 5);
    //GivePromotion(frank);
    GivePromotion((Employee7)frank);
}
//CastingExample2();

void CastingExample3()
{
    object frank = new Manager7();
    //Hexagon hexFrank = (Hexagon)frank;
    
    // Catch a possible invalid cast.
    try
    {
        Hexagon hexFrank = (Hexagon)frank;
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
//CastingExample3();

void CastingExample4()
{
    // Use "as" to test compatibility.
    object[] things = new object[4];
    things[0] = false;
    things[1] = new Hexagon();
    things[2] = new Manager7();
    things[3] = "Last thing";

    foreach (object item in things)
    {
        Hexagon? hexagon = item as Hexagon;

        if (hexagon == null)
        {
            Console.WriteLine("Item is not a hexagon");
        }
        else
        {
            hexagon.Draw();
        }
    }
}
//CastingExample4();

static void GivePromotion1(Employee7 employee)
{
    Console.WriteLine($"{employee.Name} was promoted!");

    if (employee is SalesPerson7)
    {
        Console.WriteLine($"{employee.Name} made " +
            $"{((SalesPerson7)employee).SalesNumber} sale(s)!");
    }
    if (employee is Manager7)
    {
        Console.WriteLine($"{employee.Name} had " +
            $"{((Manager7)employee).StockOptions} stock options...");
    }
}

void CastingExample5()
{
    Employee7 moonUnit = new Manager7(10, "MoonUnit Zappa", 30000, 35, "111-12-1234", 3);
    GivePromotion1(moonUnit);

    SalesPerson7 jill = new SalesPerson7(12, "Jill", 10000, 25, "234-23-4335", 30);
    GivePromotion1(jill);

    object frank = new Manager7(9, "Frank Zappa", 40000, 45, "111-11-1111", 5);
    //GivePromotion(frank);
    GivePromotion1((Employee7)frank);
}
//CastingExample5();

static void GivePromotion2(Employee7 employee)
{
    Console.WriteLine($"{employee.Name} was promoted!");

    if (employee is SalesPerson7 salesPerson)
    {
        Console.WriteLine($"{salesPerson.Name} made " +
            $"{salesPerson.SalesNumber} sale(s)!");
    }
    if (employee is Manager7 manager)
    {
        Console.WriteLine($"{manager.Name} had " +
            $"{manager.StockOptions} stock options...");
    }
}

void CastingExample6()
{
    Employee7 moonUnit = new Manager7(10, "MoonUnit Zappa", 30000, 35, "111-12-1234", 3);
    GivePromotion2(moonUnit);

    SalesPerson7 jill = new SalesPerson7(12, "Jill", 10000, 25, "234-23-4335", 30);
    GivePromotion2(jill);

    object frank = new Manager7(9, "Frank Zappa", 40000, 45, "111-11-1111", 5);
    //GivePromotion(frank);
    GivePromotion2((Employee7)frank);
}
//CastingExample6();

static void GivePromotion3(object employee)
{
    if (employee is SalesPerson7 salesPerson)
    {
        Console.WriteLine($"{salesPerson.Name} made " +
            $"{salesPerson.SalesNumber} sale(s)!");
    }
    else if (employee is Manager7 manager)
    {
        Console.WriteLine($"{manager.Name} had " +
            $"{manager.StockOptions} stock options...");
    }
    else if(employee is var _)
    {
        Console.WriteLine($"Unable to promote {employee} Wrong employee type");
    }
}

void CastingExample7()
{
    Employee7 moonUnit = new Manager7(10, "MoonUnit Zappa", 30000, 35, "111-12-1234", 3);
    GivePromotion3(moonUnit);

    SalesPerson7 jill = new SalesPerson7(12, "Jill", 10000, 25, "234-23-4335", 30);
    GivePromotion3(jill);

    object frank = new();
    GivePromotion3(frank);
}
//CastingExample7();

static void GivePromotion4(Employee7 employee)
{
    Console.WriteLine($"{employee.Name} was promoted!");

    switch (employee)
    {
        case SalesPerson7 salesPerson:
            Console.WriteLine($"{salesPerson.Name} made {salesPerson.SalesNumber} sale(s)!");
            break;
        case Manager7 manager:
            Console.WriteLine($"{manager.Name} had {manager.StockOptions} stock options...");
            break;
        case Employee7 _:
            Console.WriteLine($"Unable to promote {employee.Name}. Wrong employee type");
            break;
    }
}
void CastingExample8()
{
    Employee7 moonUnit = new Manager7(10, "MoonUnit Zappa", 30000, 35, "111-12-1234", 3);
    GivePromotion4(moonUnit);

    SalesPerson7 jill = new SalesPerson7(12, "Jill", 10000, 25, "234-23-4335", 30);
    GivePromotion4(jill);

    object frank = new Manager7(9, "Frank Zappa", 40000, 45, "111-11-1111", 5);
    //GivePromotion(frank);
    GivePromotion4((Employee7)frank);
}
//CastingExample8();
