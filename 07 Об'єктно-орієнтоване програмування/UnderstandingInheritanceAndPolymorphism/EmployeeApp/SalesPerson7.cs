using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp;
class SalesPerson7 : Employee7
{
    public int SalesNumber { get; set; }

    public SalesPerson7(int id, string name, float pay, int age, string ssn, 
        int salesNumber) 
        : base(id, name, pay, age, ssn, EmployeePayTypeEnum.Commission)
    {
        SalesNumber = salesNumber;
    }
    public SalesPerson7()
    {
    }

    public override sealed void GiveBonus(float amount)
    {
        int salesBonus = 0;

        if (SalesNumber >= 0 && SalesNumber <= 100)
        {
            salesBonus = 10;
        }
        else
        {
            if (SalesNumber >= 101 && SalesNumber <= 200)
            {
                salesBonus = 15;
            }
            else
            {
                salesBonus = 20;
            }
        }
        base.GiveBonus(amount*salesBonus);
    }

    public override void DisplayStatus()
    {
        base.DisplayStatus();
    }

    public override string? ToString()
    {
        return base.ToString();
    }
}
