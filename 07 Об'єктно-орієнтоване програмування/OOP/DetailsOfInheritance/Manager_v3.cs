using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetailsOfInheritance
{
    internal class Manager_v3 : Employee_v3
    {
        public Manager_v3()
        {
        }
        public Manager_v3(int id, string name, float pay) : base(id, name, pay)
        {
        }
    }
}
