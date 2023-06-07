using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetailsOfInheritance
{
    sealed class PartSalesPerson_v1 : SalesPerson_v1
    {
        public PartSalesPerson_v1()
        {
        }
        public PartSalesPerson_v1(int id, string name, float pay, int age, string sSN, int salesNumber) : base(id, name, pay, age, sSN, salesNumber)
        {
        }

    }
}
