using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Polymorphism
{

    abstract class Employee_v3
    {
        public int Id { get; set; }
        public decimal Pay { get; set; }

        protected Employee_v3(int id, decimal pay)
        {
            Id = id;
            Pay = pay;
        }
        protected Employee_v3()
        {
        }
        public virtual void ToConsole()
        {
            Console.WriteLine($"\n{Id}");
            Console.WriteLine($" Pay:{Pay}");
        }

    }

    class SalesPerson_v3 : Employee_v3
    {
        public SalesPerson_v3()
        {
        }

        public SalesPerson_v3(int id, decimal pay,int salesNumber) : base(id, pay)
        {
            SalesNumber = salesNumber;
        }

        public int SalesNumber { get; set; }
    }

}
