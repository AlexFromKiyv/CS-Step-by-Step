using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp
{
    internal class Employee_v5
    {
        private int _employeeId;
        private string? _employeeName;
        private decimal _currentPay;

        private static decimal _averagePay;

        static Employee_v5()
        {
            _averagePay = 10000; // It's bad style. Here must be reading from configuration.
        }

        public static decimal AveragePay 
        {
            get => _averagePay;
            set => _averagePay = value;
        }
                
        public Employee_v5(int employeeId, string? employeeName, decimal currentPay)
        {
            _employeeId = employeeId;
            _employeeName = employeeName;
            _currentPay = currentPay;
        }

        public bool IsPayMoreThanAverage() => _currentPay > _averagePay; 

    }
}
