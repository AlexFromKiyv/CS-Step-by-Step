
namespace EmployeeApp;
// Managers need to know their number of stock options.
class Manager7 :Employee7
{
    public int StockOptions { get; set; }

    public Manager7(int id, string name, float pay, int age, string ssn, 
        int stockOptions) 
        : base(id, name, pay, age, ssn, EmployeePayTypeEnum.Salaried)
    {
        StockOptions = stockOptions;
    }
    public Manager7()
    {
    }

    public override void GiveBonus(float amount)
    {
        base.GiveBonus(amount);
        Random r = new Random();
        StockOptions += r.Next(500);
    }

    public override void DisplayStatus()
    {
        base.DisplayStatus();
        Console.WriteLine($"StockOptions:\t{StockOptions}");
    }
}
