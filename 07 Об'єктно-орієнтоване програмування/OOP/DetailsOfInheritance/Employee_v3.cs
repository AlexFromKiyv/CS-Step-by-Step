using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetailsOfInheritance
{
    internal class Employee_v3
    {
        protected int EmpId;
        protected string EmpName;
        protected float EmpPay;
        protected BenefitPackage EmpBenefits = new BenefitPackage();

        public int Id { get => EmpId; set => EmpId = value; }
        public string Name { get => EmpName ; set => EmpName = value; }
        public float Pay { get => EmpPay; set => EmpPay = value; }
        public BenefitPackage Benefits { get => EmpBenefits; set => EmpBenefits =value; }

        public Employee_v3(int id, string name, float pay) : this()
        {
            Id = id;
            Name = name;
            Pay = pay;
        }
        public Employee_v3()
        {
            EmpName = string.Empty;
        }

        public double GetBenefitCost() => EmpBenefits.ComputePayDeducation();
    }
}
