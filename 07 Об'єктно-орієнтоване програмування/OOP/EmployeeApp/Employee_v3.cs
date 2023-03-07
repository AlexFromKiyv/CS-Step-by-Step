using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp
{
    internal class Employee_v3
    {
        private int _employeeId;
        private string? _employeeName;
        private decimal _currentPay;

        public Employee_v3()
        {
        }

        public Employee_v3(int id, string? name):this(id,name,default)
        {
        }

        public Employee_v3(int id, string? name, decimal currentPay)
        {
            Id = id;
            Name = name;
            CurrentPay = currentPay;
        }

        public int Id
        {
            get { return _employeeId; }
            set { _employeeId = value; }
        }

        public string? Name
        {
            get { return _employeeName; }
            set
            {
                if (value?.Length > 17)
                {
                    Console.WriteLine("The name is not set.It must be less than 17 characters");

                }
                else
                {
                    _employeeName = value;
                }
            }
        }

        public decimal CurrentPay
        {
            get => _currentPay;
            set => _currentPay = value;
        }

        public void GiveBonus(decimal amount) => CurrentPay += amount;
        public void ToConsole()
        {
            Console.WriteLine($"Id:{Id}");
            Console.WriteLine($"Name:{Name}");
            Console.WriteLine($"Pay:{CurrentPay}\n\n ");
        }

    }
}
