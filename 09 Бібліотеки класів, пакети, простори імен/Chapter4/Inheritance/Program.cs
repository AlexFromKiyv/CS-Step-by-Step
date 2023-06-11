
using Inheritance;
//SimpleInheritance();
void SimpleInheritance()
{
    Employee employee = new Employee();
    employee.Name = "Vika";
    employee.ToConsole();
}

//Upcasting();
void Upcasting()
{
    Employee employee = new Employee("Jo","Antonov");
    
    Person person = employee;

    person.ToConsole();
    
}

//DownCasting();
void DownCasting()
{
    Object employee = new Employee("Viktoria", "Farmak");

    Employee employee2 = (Employee)employee;

    Console.WriteLine(employee2.Company);

    //string bank = ((Client)employee).Bank; 

}
//Casting();
void Casting()
{
    Employee employee = new Employee("Viktoria", "Farmak");

    Person person = employee as Person;

    person.ToConsole();

    if (employee is Person person1)
    {
        person1.ToConsole();
    }
}

VirtualMethod();
void VirtualMethod()
{
    Employee employee = new Employee("Viktory", "KievPasTrans");

    //employee.ToConsole();

    Person person = employee;
    person.ToConsole();
    
}

