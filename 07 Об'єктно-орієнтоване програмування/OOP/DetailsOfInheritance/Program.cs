using DetailsOfInheritance;

//UsingDescendants();
void UsingDescendants()
{
    Manager manager1 = new();
    Console.WriteLine(manager1);
    manager1.ToConsole();

    Manager manager2 = new()
    {   Id = 1, 
        Name = "Bob", 
        Age = 35, 
        StockOptions = 10 
    };
    Console.WriteLine(manager2);
    Console.WriteLine(manager2.StockOptions);
    manager2.ToConsole();

    SalesPerson salesPerson1 = new();
    Console.WriteLine(salesPerson1);
    salesPerson1.ToConsole();

    SalesPerson salesPerson2 = new()
    {
        Id = 2,
        Name = "Inna",
        SalesNumber = 50
    };
    Console.WriteLine(salesPerson2);
    Console.WriteLine(salesPerson2.SalesNumber);
    salesPerson2.ToConsole();

    Employee employee = new(1, "John", 1000, 25, "123123123", EmployeePayTypeEnum.Hourly);
    //Manager manager = new(2, "Jo", 1000, 25, "233123123", EmployeePayTypeEnum.Hourly);
    //'Manager' does not contain a constructor
}

//UsingBaseConstructor();
void UsingBaseConstructor()
{
    Manager_v1 manager1 = new();
    manager1.ToConsole();

    Manager_v1 manager2 = new(1, "Mikolay", 1000, 35, "233234234234", 10);
    manager2.ToConsole();

    SalesPerson_v1 salesPerson1 = new();
    salesPerson1.ToConsole();

    SalesPerson_v1 salesPerson2 = new(2, "Mark", 700, 27, "421412424", 57);
    salesPerson2.ToConsole();
}

//UsingProtected();
void UsingProtected()
{
    Employee_v2 employee = new();
    employee.ToConsole();
    //employee1.EmpId = 1; // inaccesible 

    Manager_v2 manager = new();
    manager.ToConsole();
}



//ExploreAgregation();
void ExploreAgregation()
{
    Manager_v3 manager = new(1, "bob", 1000);
    Console.WriteLine(manager.GetBenefitCost());
}

UsingNestedClass();
void UsingNestedClass()
{
    Employee_v4.BenefitPackage.BenefitPackageLevel benefitPackageLevel = Employee_v4.BenefitPackage.BenefitPackageLevel.Gold;
    Console.WriteLine(benefitPackageLevel);
}