using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetailsOfInheritance
{
    internal class Manager_v2 : Employee_v2
    {

        public Manager_v2()
        {
            EmpPayType = EmployeePayTypeEnum.Salaried;
        }

        public Manager_v2(int id, string name, float pay, int age, string? sSN) : base(id, name, pay, age, sSN, EmployeePayTypeEnum.Salaried)
        {
        }
    }
}
