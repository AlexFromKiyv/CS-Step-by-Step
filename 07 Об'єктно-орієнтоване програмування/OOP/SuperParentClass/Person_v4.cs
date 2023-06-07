using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperParentClass
{
    internal class Person_v4
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public int Age { get; set; }

        public string SSN { get; } = "";

        public Person_v4(string firstName, string lastName, int age, string sSN)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            SSN = sSN;
        }

        public Person_v4()
        {
        }
        public override string? ToString() =>
            $"[First Name: {FirstName}; Last Name: {LastName}; Age: {Age}]";

        public override bool Equals(object? obj)
        {
            return obj?.ToString() == ToString();
        }

        public override int GetHashCode()
        {
            return SSN.GetHashCode();
        }
    }
}
