using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp
{
    internal class Employee_v4
    {
        private int _employeeId;
        private string? _employeeName;
        private decimal _currentPay;
        private string? _passportNumber;

        public int Id { set { _employeeId = value; } }
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
        public decimal CurrentPay { get { return _currentPay; } set { _currentPay = value; } }

        public string? PassportNumber 
        { 
            get { return _passportNumber; } 
            private set { _passportNumber = value; }
        }
            
        public Employee_v4(int id, string? name, decimal currentPay, string? passportNumber)
        {
            Id = id;
            Name = name;
            CurrentPay = currentPay;
            // PassportNumber = passportNumber; // it read-only
            // check passportNumber
            _passportNumber = passportNumber;
        }
    }
}
