
using Polymorphism;
//ExploreMethodBaseClass();
void ExploreMethodBaseClass()
{
    SalesPerson_v1 salesPerson = new();
    salesPerson.GiveBonus(100);
    salesPerson.ToConsole();

    SalesPerson_v1 salesPerson1 = new(100, "Viktory", 500, 25);
    salesPerson1.GiveBonus(100);
    salesPerson1.ToConsole();

    Manager_v1 manager1 = new(2,"Bob",1000,50);
    manager1.GiveBonus(100);
    manager1.ToConsole();
}

//ExploreVirtualOverridMethods();
void ExploreVirtualOverridMethods()
{
    Employee_v2 employee1 = new();
    employee1.GiveBonus(100);
    employee1.ToConsole();

    Employee_v2 employee2 = new(1, "John", 1000);
    employee2.GiveBonus(100);
    employee2.ToConsole();

    SalesPerson_v2 salesPerson1 = new();
    salesPerson1.GiveBonus(100);
    salesPerson1.ToConsole();

    SalesPerson_v2 salesPerson2 = new(2, "Jak", 700, 120);
    salesPerson2.GiveBonus(100);
    salesPerson2.ToConsole();

    Manager_v2 manager1 = new(3, "Bob", 1000, 300);
    manager1.GiveBonus(100);
    manager1.ToConsole();
}

ExploreAbstractClass();
void ExploreAbstractClass()
{
    //Employee_v3 employee1 = new(); // Cannot create ... abstact type

    SalesPerson_v3 salesPerson1 = new();
    salesPerson1.ToConsole();

    SalesPerson_v3 salesPerson2 = new(1, 1000, 200);
    salesPerson2.ToConsole();    

}