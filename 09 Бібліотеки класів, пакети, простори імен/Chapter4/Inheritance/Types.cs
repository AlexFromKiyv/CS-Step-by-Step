using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance
{

    class Person
    {
        public string Name { get; set; }

        public Person(string name = "Undefined")
        {
            Name = name;
        }

        public void ToConsole() => Console.WriteLine($"Name: {Name}");
    }

    class Employee : Person
    {
        public string Company { get; set; }

        public Employee(string name = "Undefined", string company = "Undefined") : base(name)
        {
            Company = company;
        }

        public new void ToConsole()
        {
            base.ToConsole();
            Console.WriteLine($"Company: {Company}");
        }
    }

    class Client : Person
    {
        public string Bank { get; set; }

        public Client(string name = "Undefined", string bank = "Undefined") : base(name) 
        {
            Bank = bank;
        }

    }



}
