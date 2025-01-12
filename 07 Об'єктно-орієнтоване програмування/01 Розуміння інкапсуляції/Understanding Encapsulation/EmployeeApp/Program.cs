
using EmployeeApp;

static void Attempt1()
{
    Employee1 emp = new();
    // Error! Cannot directly access private members
    // from an object!
    //emp._empName = "Marv";
}

void UsingAccessorAndMutator()
{
    Employee1 emp1 = new Employee1(1,"Martin",30_000);

    emp1.GiveBonus(1000);
    emp1.DisplayStatus();

    emp1.SetName("Marvin");
    Console.WriteLine(emp1.GetName());

    // Longer than 15 characters! Error will print to console.
    Employee1 emp2 = new();
    emp2.SetName("Big big and very power Max");
}
//UsingAccessorAndMutator();

void UsingProperties()
{
    Employee2 employee = new Employee2(1, "Martin", 30_000);

    employee.GiveBonus(1000);
    employee.DisplayStatus();

    // Set and then get the Name property.
    employee.Name = "Marvin";
    Console.WriteLine(employee.Name);
}
//UsingProperties();

void UsingProperties1()
{
    Employee3 joe = new Employee3(1, "Joe", 20_000, 25);
    joe.Age++;
    joe.DisplayStatus();
}
//UsingProperties1();