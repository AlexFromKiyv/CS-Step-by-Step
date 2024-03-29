﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace GenericCollections
{

    public class Person 
    {
        public string FirstName { get; set; } = "Undefined";
        public string LastName { get; set; } = "Undefined";
        public int Age { get; set; }

        public Person() { }

        public Person(string firstName, string lastName, int age)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }

        public override string? ToString()
        {
            string? fullData = $"{LastName} {FirstName} {Age}\t";
            return fullData+base.ToString();
        }
    }
}
