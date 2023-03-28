using ClassCastingRules;

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

UsingKeywordAs();
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