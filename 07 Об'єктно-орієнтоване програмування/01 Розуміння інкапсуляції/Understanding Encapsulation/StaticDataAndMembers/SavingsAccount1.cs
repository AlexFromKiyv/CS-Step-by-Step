using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticDataAndMembers;

class SavingsAccount1
{
    public static double currеntInterestRate;
    public decimal currentBalance;

    public SavingsAccount1(decimal currentBalance)
    {
        this.currentBalance = currentBalance;
        currеntInterestRate = 0.04; // This is static data!
    }
    public static double GetInterestRate() => currеntInterestRate;

    public static void SetInterestRate(double newRate)
    {
        currеntInterestRate = newRate;
    }
}
