using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticDataAndMembers;

class SavingsAccount
{
    // A static point of data.
    public static double currеntInterestRate = 0.04;
    // Instance-level data.
    public decimal currentBalance;
    public SavingsAccount(decimal currentBalance)
    {
        this.currentBalance = currentBalance;
    }

    public static void SetInterestRate(double newRate)
    {
       currеntInterestRate = newRate;
    }

    public static double GetInterestRate() => currеntInterestRate;
}
