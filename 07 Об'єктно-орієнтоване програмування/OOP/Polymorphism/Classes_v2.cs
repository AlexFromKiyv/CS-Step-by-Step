namespace Polymorphism
{
    class Employee_v2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Pay { get; set; }
        public Employee_v2(int id, string name, decimal pay)
        {
            Id = id;
            Name = name;
            Pay = pay;
        }
        public Employee_v2()
        {
            Name = "Not known";
        }
        public virtual void GiveBonus(decimal amount) => Pay += amount;
        public virtual void ToConsole()
        {
            Console.WriteLine($"\n{Id} {Name}");
            Console.WriteLine($" Pay:{Pay}");
        }
    }

    class SalesPerson_v2 : Employee_v2
    {
        public SalesPerson_v2()
        {
        }
        public SalesPerson_v2(int id, string name, decimal pay, int salesNumber) : base(id, name, pay)
        {
            SalesNumber = salesNumber;
        }
        public int SalesNumber { get; set; }
        public override void GiveBonus(decimal amount)
        {
            int salesBonus = 0;
            if (SalesNumber > 0) 
            { 
                if (SalesNumber <100) 
                {
                    salesBonus = 1;
                }
                else
                {
                    salesBonus = 2;
                }
            }

            base.GiveBonus(amount*salesBonus);
        }
        public override sealed void ToConsole()
        {
            base.ToConsole();
            Console.WriteLine($" SalesNumber:{SalesNumber}");
        }
        
        
    }

    class Manager_v2 : Employee_v2
    {
        public Manager_v2()
        {
        }
        public Manager_v2(int id, string name, decimal pay, int stockOption) : base(id, name, pay)
        {
            StockOptions = stockOption;
        }
        public int StockOptions { get; set; }
        public override void GiveBonus(decimal amount)
        {
            base.GiveBonus(amount);
            StockOptions += new Random().Next(500); 
        }
        public override void ToConsole()
        {
            base.ToConsole();
            Console.WriteLine($" StockOptions:{StockOptions}");
        }
    }
}