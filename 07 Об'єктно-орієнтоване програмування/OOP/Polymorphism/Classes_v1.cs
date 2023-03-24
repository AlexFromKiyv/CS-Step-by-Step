namespace Polymorphism
{
    class Employee_v1
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Pay { get; set; }
        public Employee_v1(int id, string name, decimal pay)
        {
            Id = id;
            Name = name;
            Pay = pay;
        }

        public Employee_v1()
        {
            Name = "Not known";
        }
        public void GiveBonus(decimal amount) => Pay += amount;
        public void ToConsole() => Console.WriteLine($"{Id} {Name} {Pay}");

    }

    class SalesPerson_v1 : Employee_v1
    {
        public SalesPerson_v1()
        {
        }
        public SalesPerson_v1(int id, string name, decimal pay, int salesNumber) : base(id, name, pay)
        {
            SalesNumber = salesNumber;
        }
        public int SalesNumber { get; set; }
    }


    class Manager_v1: Employee_v1
    {
        public Manager_v1()
        {
        }
        public Manager_v1(int id, string name, decimal pay, int stockOption ) : base(id, name, pay)
        {
            StockOptions = stockOption;
        }
        public int StockOptions { get; set; }
    }
}