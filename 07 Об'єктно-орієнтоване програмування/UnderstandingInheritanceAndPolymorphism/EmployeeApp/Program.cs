
using EmployeeApp;

void UsingEmployee6Class()
{
    Employee6 employee = new(23, "Marvin", 1000, 35,
        "111 - 11 - 1111", EmployeePayTypeEnum.Salaried);
    employee.DisplayStatus();
    employee.GiveBonus(100);
    employee.DisplayStatus();
}
//UsingEmployee6Class();

void UsingSalesPerson6()
{
    SalesPerson6 salesPerson = new()
    {
        Name = "Fred",
        Age = 31,
        SalesNumber = 50
    };

    salesPerson.DisplayStatus();
}
//UsingSalesPerson6();

void CallingBaseClassConstructors()
{
    Manager6 manager = new Manager6(5, "Alexandr", 1000, 35, "1234-234-32",
        123);
    manager.DisplayStatus();
}
//CallingBaseClassConstructors();

void TheProtectedKeyword()
{
    Employee7 employee = new();
    // Error! Can't access protected data from client code.
    //employee.name = "John";
}

void ClassUseOtherClass()
{
    Manager7 manager = new Manager7(15, "John", 100000, 50, "333-23-2322", 9000);
    manager.DisplayStatus();
    Console.WriteLine($"Benefit Cost:{manager.GetBenefitCost()}");

    Employee7.BenefitPackage.BenefitPackageLevel packageLevel =
        Employee7.BenefitPackage.BenefitPackageLevel.Platinum;
    Console.WriteLine(packageLevel);
}
//ClassUseOtherClass();

void UsingGiveBonus()
{
    Manager7 manager = new(3, "Jack", 100000, 50, "3256-56-2536", 9000);
    manager.GiveBonus(300);
    manager.DisplayStatus();

    SalesPerson7 salesPerson = new(8, "Olga", 3000, 35, "2342-34-3432", 31);
    salesPerson.GiveBonus(200);
    salesPerson.DisplayStatus();
}
UsingGiveBonus();