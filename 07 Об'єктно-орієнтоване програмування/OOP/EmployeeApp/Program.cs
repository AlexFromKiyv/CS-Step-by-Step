using EmployeeApp;


//UsingEmployee_v1();

void UsingEmployee_v1()
{
    Employee_v1 employee = new(1, "Joseph", 10000);
    employee.ToConsole();

    employee.GiveBonus(1000);
    employee.ToConsole();

    employee.SetName("Joe");
    employee.ToConsole();

    employee.SetName("SoIAttemptToInputTooBigName");
}

//UsingEmployee_v2();

void UsingEmployee_v2()
{
    Employee_v2 employee = new(1, "Joseph", 10000);
    employee.ToConsole();

    employee.GiveBonus(1000);
    employee.ToConsole();

    employee.CurrentPay++;
    employee.ToConsole();

    employee.Name = "Joe";
    employee.ToConsole();

    employee.Name = "SoIAttemptToInputTooBigName";
 
}


//UsingEmployee_v3();

void UsingEmployee_v3()
{
    Employee_v3 employee = new(1, "Joseph", 10000);
    employee.ToConsole();

    employee.GiveBonus(1000);
    employee.ToConsole();

    employee.Name = "Joe";
    employee.ToConsole();

    Employee_v3 badEmployee = new(1, "SoIAttemptToInputTooBigName");
    badEmployee.ToConsole();
}

UsingEmployee_v5();

void UsingEmployee_v5()
{
    Employee_v5 employee = new(1, "Joseph", 11000);

    Console.WriteLine(employee.IsPayMoreThanAverage());
}