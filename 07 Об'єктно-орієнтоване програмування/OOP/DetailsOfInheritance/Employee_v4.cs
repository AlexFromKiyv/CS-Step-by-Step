using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetailsOfInheritance
{
    class Employee_v4
    {
        protected int EmpId;
        protected string EmpName;
        protected float EmpPay;
        public int Id { get => EmpId; set => EmpId = value; }
        public string Name { get => EmpName; set => EmpName = value; }
        public float Pay { get => EmpPay; set => EmpPay = value; }

        public class BenefitPackage
        {
            public enum BenefitPackageLevel
            {
                Standard, Gold, Platinum
            }
        }

        public Employee_v4(int id, string name, float pay) : this()
        {
            Id = id;
            Name = name;
            Pay = pay;
        }
        public Employee_v4()
        {
            EmpName = string.Empty;
        }

    }
}
