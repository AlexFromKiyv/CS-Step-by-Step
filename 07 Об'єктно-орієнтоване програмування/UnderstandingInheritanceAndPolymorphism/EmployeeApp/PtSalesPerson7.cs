using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp;

sealed class PtSalesPerson7 : SalesPerson7
{
    //// Compiler error! Can't override this method
    //// in the PTSalesPerson class, as it was sealed.
    //public override void GiveBonus(float amount)
    //{
    //    base.GiveBonus(amount);
    //}
}
