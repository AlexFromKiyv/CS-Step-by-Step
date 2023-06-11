using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicInterface
{

    class Person
    {
        public string Name { get; set; }
        public Person(string name)
        {
            Name = name;
        }
        public virtual void ToConsole() => Console.WriteLine($"Name:{Name}");  
    }

    class Employee_v1 : Person
    {
        public string Company { get; set; }

        public Employee_v1(string name, string company): base(name)
        {
            Company = company;
        }
        public override void ToConsole()
        {
            base.ToConsole();
            Console.WriteLine($"Company:{Company}");
        }
    }


    class Employee_v2 : Person
    {
        public string Company { get; set; }

        public Employee_v2(string name, string company) : base(name)
        {
            Company = company;
        }
        public new void ToConsole() // change only here
        {
            base.ToConsole();
            Console.WriteLine($"Company:{Company}");
        }
    }
}
