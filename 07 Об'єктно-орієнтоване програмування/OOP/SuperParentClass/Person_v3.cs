using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperParentClass
{
    internal class Person_v3
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public int Age { get; set; }

        public Person_v3(string firstName, string lastName, int age)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }
        public Person_v3()
        {
        }

        public override string? ToString() =>
            $"[First Name: {FirstName}; Last Name: {LastName}; Age: {Age}]";

        public override bool Equals(object? obj)
        {
            return obj?.ToString() == ToString();
        }
    }
}
