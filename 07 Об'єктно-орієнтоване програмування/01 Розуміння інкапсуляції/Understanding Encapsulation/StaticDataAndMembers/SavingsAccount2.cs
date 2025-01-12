using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticDataAndMembers;

class SavingsAccount2
{
    public static double currеntInterestRate;
    public decimal currentBalance;
    // A static constructor!
    static SavingsAccount2()
    {
        Console.WriteLine("In static constructor!");
        currеntInterestRate = 0.04;
    }
    public SavingsAccount2(decimal currentBalance)
    {

        this.currentBalance = currentBalance;
    }
    public static double GetInterestRate() => currеntInterestRate;

    public static void SetInterestRate(double newRate)
    {
        currеntInterestRate = newRate;
    }
}
