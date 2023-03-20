using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetailsOfInheritance
{
    partial class Employee
    {

        public Employee(int id, string name, float pay, int age, string sSN, EmployeePayTypeEnum payType)
        {
            Id = id;
            Name = name;
            Pay = pay;
            Age = age;
            SSN = sSN;
            PayType = payType;
        }

        public Employee() 
        {
        }

        public void ToConsole()
        {
            Console.WriteLine($"Id:{Id} Name:{Name} Age:{Age} Pay:{Pay} SSN:{SSN} PayType:{PayType} \n");
        }

        public void GiveBonus(float amount)
        {
            Pay = this switch
            {
                { PayType: EmployeePayTypeEnum.Hourly } => amount * 0.1F,
                { PayType: EmployeePayTypeEnum.Salaried } => (amount * 40F) / 2080F,
                { PayType: EmployeePayTypeEnum.Commission } => Pay + amount,
                _ => Pay
            }; 
        }
    }
}
