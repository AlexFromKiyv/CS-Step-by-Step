using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetailsOfInheritance
{
    class Employee_v2
    {
        protected int EmpId;
        protected string EmpName;
        protected float EmpPay;
        protected int EmpAge;
        protected string? EmpSSN;
        protected EmployeePayTypeEnum EmpPayType;

        public int Id { get => EmpId; set => EmpId = value; }
        public string Name
        {
            get => EmpName;
            set
            {
                if (value?.Length > 15)
                {
                    Console.WriteLine("Name lenght exceeds 15 characters!");
                }
                else
                {
                    if (value != null)
                    {
                        EmpName = value;
                    }
                }
            }
        }
        public float Pay { get => EmpPay; set => EmpPay = value; }
        public int Age { get => EmpAge; set => EmpAge = value; }
        public string? SSN { get => EmpSSN; private set => EmpSSN = value; }
        public EmployeePayTypeEnum PayType { get => EmpPayType; set => EmpPayType = value; }

        public Employee_v2(int id, string name, float pay, int age, string? sSN, EmployeePayTypeEnum payType):this()
        {
            Id = id;
            Name = name;
            Pay = pay;
            Age = age;
            SSN = sSN;
            PayType = payType;
        }

        public Employee_v2()
        {
            EmpName = string.Empty;
        }
        public void ToConsole()
        {
            Console.WriteLine($"Id:{Id} Name:{Name} Age:{Age} Pay:{Pay} SSN:{SSN} PayType:{PayType} \n");
        }
    }
}
