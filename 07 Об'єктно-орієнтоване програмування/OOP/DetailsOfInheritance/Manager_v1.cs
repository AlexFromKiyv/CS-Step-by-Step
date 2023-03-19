using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetailsOfInheritance
{
    internal class Manager_v1  :Employee
    {
        public int StockOptions { get; set; }
        public Manager_v1(int id, string name, float pay, int age, string sSN, int stockOptions)
            : base(id, name, pay, age, sSN, EmployeePayTypeEnum.Salaried)
        {
            StockOptions = stockOptions;
        }
        public Manager_v1()
        {
        }
    }
}
