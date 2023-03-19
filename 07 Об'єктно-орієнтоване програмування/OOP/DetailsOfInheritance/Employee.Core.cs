using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DetailsOfInheritance
{
    partial class Employee
    {
        private int _id;
        private string _name;
        private float _pay;
        private int _age;
        private string _ssn;
        private EmployeePayTypeEnum _payType;


        public int Id { get => _id; set => _id = value; }
        public string Name 
        { 
            get => _name;
            set 
            {
                if (value.Length > 15) 
                {
                    Console.WriteLine("Name lenght exceeds 15 characters!");
                }
                else
                {
                    _name = value;
                }
            } 
        }
        public float Pay { get => _pay; set => _pay = value; }
        public int Age { get => _age; set => _age = value; }
        public string SSN { get => _ssn; private set => _ssn = value; }
        public EmployeePayTypeEnum PayType { get => _payType ; set => _payType = value ; }
    }
}
