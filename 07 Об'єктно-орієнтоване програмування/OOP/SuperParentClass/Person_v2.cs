using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperParentClass
{
    internal class Person_v2
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public int Age { get; set; }

        public Person_v2(string firstName, string lastName, int age)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }

        public Person_v2()
        {
        }
        public override bool Equals(object? obj)
        {
            if (!(obj is Person_v2 person))
            {
                return false;
            }

            bool comparation =
                FirstName == person.FirstName
                && LastName == person.LastName
                && Age == person.Age;

            if (comparation)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
