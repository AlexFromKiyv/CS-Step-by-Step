using ClassCastingRules;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.InteropServices;

//ExploreImplicideCasting();
void ExploreImplicideCasting()
{
    object obj = new Manager(1, "Max", 1000, 200);
    Console.WriteLine(obj);

    Employee employee_1 = new Manager(2, "Bob", 1200, 100);
    employee_1.ToConsole();

    Employee employee_2 = new PartSalesPerson(3, "Jim", 700, 30);
    employee_2.ToConsole();

    SalesPerson salesPerson_1 = new PartSalesPerson(4,"Jill",500,20);
    salesPerson_1.ToConsole();
}

//UsingImplicideCasting();

void UsingImplicideCasting()
{
    object obj = new Manager(1, "Max", 1000, 200);
    //GivePromotion(obj); //cannot convert ... to

    Employee employee_1 = new Manager(2, "Bil", 1200, 100);
    GivePromotion(employee_1);

    Employee employee_2 = new PartSalesPerson(3, "Jim", 700, 30);
    GivePromotion(employee_2);

    SalesPerson salesPerson_1 = new PartSalesPerson(3, "Jill", 500, 20);
    GivePromotion(salesPerson_1);

    
    void GivePromotion(Employee employee)
    {
        employee.Pay++;
        Console.WriteLine($"{employee.Name} was promoted! Pay: {employee.Pay}");
        Console.WriteLine(employee.GetType());
        Console.WriteLine();
    }
}

//UsingExplicitCasting();
void UsingExplicitCasting()
{
    object obj = new Manager(1, "Max", 1000, 200);
    //GivePromotion(obj); //cannot convert ... to

    GivePromotion((Employee)obj);

    void GivePromotion(Employee employee)
    {
        employee.Pay++;
        Console.WriteLine($"{employee.Name} was promoted! Pay: {employee.Pay}");
        Console.WriteLine(employee.GetType());
        Console.WriteLine();
    }
}

//ExploreExplicitCasting();
void ExploreExplicitCasting()
{
    object meneger = new Manager(1, "Bill", 1000, 100);

    Hexagon hexagon = (Hexagon)meneger;

    Console.WriteLine(hexagon.Name);
}

//ExploreExplicitCastingWithTry();
void ExploreExplicitCastingWithTry()
{
    object meneger = new Manager(1, "Bill", 1000, 100);

    try
    {
        Hexagon hexagon = (Hexagon)meneger;
        Console.WriteLine(hexagon.Name);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

//UsingKeywordAs();
void UsingKeywordAs()
{
    object[] things = new object[4];
    things[0] = "Hi girl";
    things[1] = new Manager(1, "Bill", 1000, 100);
    things[2] = new Hexagon("Hex");
    things[3] = new PartSalesPerson(3, "Jill", 500, 20);

    foreach( object thing in things)
    {
        Employee? employee = thing as Employee;
        if (employee != null )
        {
            Console.WriteLine(employee.Name);
        }
    }
    Console.WriteLine();

    foreach (object thing in things)
    {
        Shape? shape = thing as Shape;
        if (shape != null)
        {
            Console.WriteLine(shape.Name);
        }
    }
}

//UsingKeywordIs();
void UsingKeywordIs()
{

    SalesPerson salesPerson_1 = new(1, "Max", 800, 60);
    EmployeeDetails(salesPerson_1);

    Manager manager = new(2, "Bill", 1100, 100);
    EmployeeDetails(manager);

    Employee employee = new PartSalesPerson(3, "Julia", 300, 10);
    EmployeeDetails(employee);


    static void EmployeeDetails(Employee employee)
    {
        string details = string.Empty;

        if (employee is SalesPerson)
        {
            details = "  Number of sales: " + ((SalesPerson)employee).SalesNumber.ToString();
        }

        if (employee is Manager)
        {
            details = "  Stock options: " + ((Manager)employee).StockOptions.ToString();
        }

        Console.WriteLine(employee.Name + details);

    }
}



//UsingKeywordIsWithAssign();
void UsingKeywordIsWithAssign()
{

    SalesPerson salesPerson_1 = new(1, "Max", 800, 60);
    EmployeeDetails(salesPerson_1);

    Manager manager = new(2, "Bill", 1100, 100);
    EmployeeDetails(manager);

    Employee employee = new PartSalesPerson(3, "Julia", 300, 10);
    EmployeeDetails(employee);


    static void EmployeeDetails(Employee employee)
    {
        string details = string.Empty;

        if (employee is SalesPerson saleser)
        {
            details = "  Number of sales: " + saleser.SalesNumber.ToString();
        }

        if (employee is Manager _manager)
        {
            details = "  Stock options: " + _manager.StockOptions.ToString();
        }

        Console.WriteLine(employee.Name + details);

    }
}

//UsingKeywordIsNot();
void UsingKeywordIsNot()
{
    object[] things = new object[4];
    things[0] = "Hi girl";
    things[1] = new Manager(1, "Bill", 1000, 100);
    things[2] = new Hexagon("Hex");
    things[3] = new PartSalesPerson(3, "Jill", 500, 20);

    foreach (object thing in things)
    {
        if (thing is not Employee)
        {
            Console.WriteLine("No know object");
        }
        else
        {
            Console.WriteLine(((Employee)thing).Id);
        }
    }
}



UsingSwithWithPatternMatching();

void UsingSwithWithPatternMatching()
{

    SalesPerson salesPerson_1 = new(1, "Max", 800, 60);
    EmployeeDetails(salesPerson_1);

    Manager manager = new(2, "Bill", 1100, 100);
    EmployeeDetails(manager);

    Employee employee = new PartSalesPerson(3, "Julia", 300, 10);
    EmployeeDetails(employee);


    static void EmployeeDetails(Employee employee)
    {
        string details = string.Empty;

        switch (employee)
        {
            case Manager manager:
                details = "  Stock options: " + manager.StockOptions.ToString();
                break;
            case PartSalesPerson _:
                details = " No promote.";
                break;
            case SalesPerson salesPerson when salesPerson.SalesNumber >30 :
                details = "  Number of sales: " + salesPerson.SalesNumber.ToString();
                break;
            default:
                break;
        }
        Console.WriteLine(employee.Name + details);
    }
}


