using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp;

sealed class PtSalesPerson6 : SalesPerson6
{
    public PtSalesPerson6(int id, string name, float pay, int age, string ssn, int salesNumber) 
        : base(id, name, pay, age, ssn, salesNumber)
    {
    }

    //...

}
