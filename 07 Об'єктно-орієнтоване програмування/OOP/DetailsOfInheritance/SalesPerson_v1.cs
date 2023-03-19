using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetailsOfInheritance
{
    internal class SalesPerson_v1: Employee
    {
        public int SalesNumber { get; set; }
        public SalesPerson_v1(int id, string name, float pay, int age, string sSN, int salesNumber) : base(id, name, pay, age, sSN,EmployeePayTypeEnum.Commission)
        {
            SalesNumber = salesNumber;
        }

        public SalesPerson_v1()
        {
        }
    }
}
