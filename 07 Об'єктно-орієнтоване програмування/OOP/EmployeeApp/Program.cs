using EmployeeApp;


UsingEmployee_v1();

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




