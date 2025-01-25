using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EmployeeApp;

partial class Employee6
{
    // Mathods
    public void GiveBonus(float amount)
    {
        Pay = this switch
        {
            { Age: >= 18, PayType: EmployeePayTypeEnum.Commission }
            => Pay += 0.10F * amount,
            { Age: >= 18, PayType: EmployeePayTypeEnum.Hourly }
            => Pay += 40F * amount / 2080F,
            { Age: >= 18, PayType: EmployeePayTypeEnum.Salaried }
            => Pay += amount,
            _ => Pay += 0
        };
    }

    public void DisplayStatus()
    {
        Console.WriteLine($"{Id}\t{Name}\t{Age}\t{Pay}");
    }
}
