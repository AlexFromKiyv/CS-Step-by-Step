using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkingWithObservableCollection
{
    internal class Person
    {
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public int Age { get; set; }

        public Person(string firstName = "", string lastName = "", int age = 0)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }

        public override string? ToString()
        {
            return $"{FirstName}\t{LastName}\t{Age}\t";
        }
    }
}
