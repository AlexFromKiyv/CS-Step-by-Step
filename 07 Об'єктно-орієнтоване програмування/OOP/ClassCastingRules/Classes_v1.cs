using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassCastingRules
{
    abstract class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Pay { get; set; }
        public Employee(int id, string name, decimal pay)
        {
            Id = id;
            Name = name;
            Pay = pay;
        }
        public Employee() 
        {
            Name = string.Empty;
        }
        public virtual void ToConsole()
        {
            Console.WriteLine($"\n{Id}");
            Console.WriteLine($" Pay:{Pay}");
        }
    }

    class SalesPerson : Employee 
    {
        public SalesPerson()
        {
        }
        public SalesPerson(int id, string name, decimal pay, int salesNumber ) : base(id, name, pay)
        {
            SalesNumber = salesNumber;
        }
        public int SalesNumber { get; set; }

    }

    sealed class PartSalesPerson : SalesPerson
    {
        public PartSalesPerson(int id, string name, decimal pay, int salesNumber) : base(id, name, pay, salesNumber)
        {
        }
    }


    class Manager : Employee
    {
        public Manager(int id, string name, decimal pay, int stockOptions) : base(id, name, pay)
        {
            StockOptions = stockOptions;
        }
        public int StockOptions { get; set; }

        public override void ToConsole()
        {
            base.ToConsole();
            Console.WriteLine($" StockOptions:{StockOptions}");
        }
    }
}
