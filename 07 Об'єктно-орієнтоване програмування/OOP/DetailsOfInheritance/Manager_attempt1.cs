using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetailsOfInheritance
{
    class Manager_attempt1 : Employee
    {
        public int StockOptions { get; set; }

        public Manager_attempt1(int id, string name, float pay, int age, string sSN, EmployeePayTypeEnum payType, int stockOptions)
        {
            Id = id;
            Name = name;
            Pay = pay;
            Age = age;
            //SSN = sSN; //set acceser is inaccesible SSN is redonly
            PayType = payType;

            StockOptions = stockOptions;
        }
    }
}
