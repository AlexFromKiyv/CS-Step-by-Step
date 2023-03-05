using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp
{
    internal class Employee
    {
        private int _employeeId;
        private string _employeeName;
        private decimal _currentPay;

        public Employee(int employeeId, string employeeName, decimal currentPay)
        {
            _employeeId = employeeId;
            _employeeName = employeeName;
            _currentPay = currentPay;
        }

        public void GiveBonus(decimal amount) => _currentPay += amount;

        public void ToConsole()
        {
            Console.WriteLine($"Id:{_employeeId}");
            Console.WriteLine($"Name:{_employeeName}");
            Console.WriteLine($"Pay:{_currentPay} ");
        }

    }
}
