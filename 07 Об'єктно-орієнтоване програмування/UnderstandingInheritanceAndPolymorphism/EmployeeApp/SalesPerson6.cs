using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp;
// Salespeople need to know their number of sales.
class SalesPerson6 : Employee6
{
    public int SalesNumber { get; set; }

    public SalesPerson6(int id, string name, float pay, int age, string ssn, 
        int salesNumber) 
        : base(id, name, pay, age, ssn, EmployeePayTypeEnum.Commission)
    {
        SalesNumber = salesNumber;
    }
    public SalesPerson6()
    {
    }
}
