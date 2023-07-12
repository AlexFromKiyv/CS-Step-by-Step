using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomGenericMethods
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "Undefined";
        public string LastName { get; set; } = "Undefined";
        public int Age { get; set; } 
        public Person() { }

        public Person(int id, string firstName)
        {
            Id = id;
            FirstName = firstName;
        }

        public override string? ToString()
        {
            return $"{Id} {FirstName} {LastName} {Age}\t";
        }
    }
}
