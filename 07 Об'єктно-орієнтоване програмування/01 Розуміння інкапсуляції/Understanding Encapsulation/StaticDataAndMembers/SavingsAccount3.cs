using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticDataAndMembers;

class SavingsAccount3
{
    public static double currеntInterestRate;
    public decimal currentBalance;
    public static double CurrentInterestRate
    {
        get => currеntInterestRate;
        set => currеntInterestRate = value;
    }
    static SavingsAccount3()
    {
        currеntInterestRate = 0.04;
    }
    public SavingsAccount3(decimal currentBalance)
    {
        this.currentBalance = currentBalance;
    }
}
