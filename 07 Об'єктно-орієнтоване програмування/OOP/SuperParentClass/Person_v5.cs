using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperParentClass
{
    internal class Person_v5
    {
        public int Id { get; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public int Age { get; set; }

        public Person_v5(int id, string firstName, string lastName, int age)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }
        public Person_v5()
        {
        }

        public override string? ToString() =>
            $"[Id: {Id}; First Name: {FirstName}; Last Name: {LastName}; Age: {Age}]";

        public override bool Equals(object? obj)
        {
            Console.Write("Work in method public override bool Equals(object? obj)\t");
            return obj?.ToString() == ToString();
        }

        public override int GetHashCode()
        {
            //Console.Write("Work in method public override int GetHashCode()\t");
            return HashCode.Combine(Id, FirstName, LastName, Age);
        }
    }
}
