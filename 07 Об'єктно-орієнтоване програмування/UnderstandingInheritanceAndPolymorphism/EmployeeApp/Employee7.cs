using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EmployeeApp;

partial class Employee7
{
    // Mathods

    // Expose certain benefit behaviors of object.
    public double GetBenefitCost() => EmpBenefits.ComputePayDeduction();

    //public void GiveBonus(float amount)
    //{
    //    Pay = this switch
    //    {
    //        { Age: >= 18, PayType: EmployeePayTypeEnum.Commission }
    //        => Pay += 0.10F * amount,
    //        { Age: >= 18, PayType: EmployeePayTypeEnum.Hourly }
    //        => Pay += 40F * amount / 2080F,
    //        { Age: >= 18, PayType: EmployeePayTypeEnum.Salaried }
    //        => Pay += amount,
    //        _ => Pay += 0
    //    };
    //}

    // This method can now be 'overridden' by a derived class.
    public virtual void GiveBonus(float amount)
    {
        Pay += amount;
    }

    //public void DisplayStatus()
    //{
    //    Console.WriteLine($"{Id}\t{Name}\t{Age}\t{Pay}");
    //}

    public virtual void DisplayStatus()
    {
        Console.WriteLine($"\nId:\t{Id}");
        Console.WriteLine($"Name:\t{Name}");
        Console.WriteLine($"Age:\t{Age}");
        Console.WriteLine($"Pay:\t{Pay}");
        Console.WriteLine($"SSN:\t{SSN}");
    }


}
