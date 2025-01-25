
using StaticDataAndMembers;

static void DefiningStaticFieldData()
{
    SavingsAccount s1 = new SavingsAccount(50);
    SavingsAccount s2 = new SavingsAccount(100);
    SavingsAccount s3 = new SavingsAccount(10000.75M);

}
//DefiningStaticFieldData();

static void UsingStaticMethods()
{
    SavingsAccount s1 = new SavingsAccount(50);
    SavingsAccount s2 = new SavingsAccount(100);

    // Print the current interest rate.
    Console.WriteLine($"Interest Rate is:{SavingsAccount.GetInterestRate()}");
    
    // Make new object, this does NOT 'reset' the interest rate.
    SavingsAccount s3 = new SavingsAccount(10000.75M);
    Console.WriteLine($"Interest Rate is:{SavingsAccount.GetInterestRate()}");

    SavingsAccount.SetInterestRate(0.05);
    Console.WriteLine($"Interest Rate is:{SavingsAccount.GetInterestRate()}");

    SavingsAccount s4 = new SavingsAccount(20000);
    Console.WriteLine($"Interest Rate is:{SavingsAccount.GetInterestRate()}");
}
//UsingStaticMethods();

static void StaticDataInConstructor()
{
    SavingsAccount1 s1 = new(50);
    Console.WriteLine($"Interest Rate is: {SavingsAccount1.GetInterestRate()}");

    // Try to change the interest rate via property.
    SavingsAccount1.SetInterestRate(0.08);

    SavingsAccount1 s2 = new(100);
    Console.WriteLine($"Interest Rate is: {SavingsAccount1.GetInterestRate()}");
}
//StaticDataInConstructor();

static void StaticDataInStaticConstructor()
{
    SavingsAccount2 s1 = new(50);
    Console.WriteLine($"Interest Rate is: {SavingsAccount2.GetInterestRate()}");

    // Try to change the interest rate via property.
    SavingsAccount2.SetInterestRate(0.08);

    SavingsAccount2 s2 = new(100);
    Console.WriteLine($"Interest Rate is: {SavingsAccount2.GetInterestRate()}");
}
//StaticDataInStaticConstructor();

static void UsingAStaticClass()
{
    // These compile just fine.
    TimeUtilClass1.PrintDate();
    TimeUtilClass1.PrintTime();
    // Compiler error! Can't create instance of static classes!
    //TimeUtilClass u = new TimeUtilClass();
}
//UsingAStaticClass();

static void UsingStaticProperty()
{
    Console.WriteLine($"Interest Rate is:{SavingsAccount3.CurrentInterestRate}");
    SavingsAccount3.CurrentInterestRate = 0.05;
    SavingsAccount3 account = new(10000);
    Console.WriteLine($"Interest Rate is:{SavingsAccount3.CurrentInterestRate}"  );
}
UsingStaticProperty();