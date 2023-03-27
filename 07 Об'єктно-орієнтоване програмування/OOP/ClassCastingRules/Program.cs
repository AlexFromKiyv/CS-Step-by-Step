using ClassCastingRules;

ExploreImplicideCasting();
void ExploreImplicideCasting()
{
    object obj = new Manager(1, "Max", 1000, 200);
    Console.WriteLine(obj);

    Employee employee_1 = new Manager(2, "Bob", 1200, 100);
    employee_1.ToConsole();

    SalesPerson salesPerson_1 = new PartSalesPerson(3,"Jill",500,20);
    salesPerson_1.ToConsole();
}

//UsingImplicideCasting();

void UsingImplicideCasting()
{
    object obj = new Manager(1, "Max", 1000, 200);
    Employee employee_1 = new Manager(2, "Bob", 1200, 100);
    SalesPerson salesPerson_1 = new PartSalesPerson(3, "Jill", 500, 20);

    //GivePromotion(obj); //cannot convert ... to
    GivePromotion((Employee)obj);

    GivePromotion(employee_1);

    GivePromotion(salesPerson_1);

    static void GivePromotion(Employee employee)
    {
        Console.WriteLine($"{employee.Name} was promoted! Increase pay or park space ...");
    }
}
