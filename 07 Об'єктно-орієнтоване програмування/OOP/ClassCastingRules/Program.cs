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

UsingExplicitCasting();
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