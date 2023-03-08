using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp
{
    internal class Employee_v6
    {
        private int _employeeId;
        private string? _employeeName;
        private decimal _currentPay;
        private DateTime _hireDate;
        private EmployeePayTypeEnum _payType;

        public int Id { get => _employeeId; set => _employeeId = value; }
        public string? Name { get => _employeeName; set => _employeeName = value; }
        public decimal Pay  { get => _currentPay; set => _currentPay = value;}

        public DateTime HireDate { get => _hireDate; set => _hireDate = value; }
        public EmployeePayTypeEnum PayType 
        { 
            get => _payType; set => _payType = value;
        }

        public Employee_v6(int id, string? name, decimal pay)
            :this(id, name, pay, EmployeePayTypeEnum.Salaried)
        {
        }

        public Employee_v6(int id, string? name, decimal pay, EmployeePayTypeEnum payType)
        {
            Id = id;
            Name = name;
            Pay = pay;
            PayType = payType;
        }

        public void GiveBonus(decimal amount)
        {
            Pay = this switch
            {
                { PayType: EmployeePayTypeEnum.Commission } => Pay + amount * 0.1M,
                { PayType:EmployeePayTypeEnum.Hourly} => Pay + amount *40M/2080M,
                { PayType:EmployeePayTypeEnum.Salaried} => Pay + amount,
                _=> Pay
            }; 

        }

        public void GiveBonusWithId(decimal amount)
        {
            Pay = this switch
            {
                { Id: > 100, PayType: EmployeePayTypeEnum.Commission } => Pay + amount * 0.1M,
                { Id: > 100, PayType: EmployeePayTypeEnum.Hourly } => Pay + amount * 40M / 2080M,
                { Id: > 100, PayType: EmployeePayTypeEnum.Salaried } => Pay + amount,
                _ => Pay
            };
        }

        public void GiveBonusWithIdAndHireDate(decimal amount)
        {
            Pay = this switch
            {
                { Id: > 100, PayType: EmployeePayTypeEnum.Commission, HireDate: { Year : > 2020} } => Pay + amount * 0.1M,
                { Id: > 100, PayType: EmployeePayTypeEnum.Hourly, HireDate: { Year: > 2020 } } => Pay + amount * 40M / 2080M,
                { Id: > 100, PayType: EmployeePayTypeEnum.Salaried, HireDate: { Year: > 2020 } } => Pay + amount,
                _ => Pay
            };
        }

        public void GiveBonusWithIdAndHireDateImproved(decimal amount)
        {
            Pay = this switch
            {
                { Id: > 100, PayType: EmployeePayTypeEnum.Commission, HireDate.Year: > 2020  } => Pay + amount * 0.1M,
                { Id: > 100, PayType: EmployeePayTypeEnum.Hourly, HireDate.Year: > 2020 } => Pay + amount * 40M / 2080M,
                { Id: > 100, PayType: EmployeePayTypeEnum.Salaried, HireDate.Year: > 2020 } => Pay + amount,
                _ => Pay
            };
        }

    }
}
