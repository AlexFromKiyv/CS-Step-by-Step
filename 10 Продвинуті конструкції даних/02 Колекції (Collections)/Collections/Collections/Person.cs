using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    public class Person
    {
        public int Age { get; set; }
        public string FirstName { get; set; } = "Undefined";
        public string LastName { get; set; } = "Undefined"; 
        public Person() { }
        public Person(string firstName, string lastName, int age)
        {
            Age = age;
            FirstName = firstName;
            LastName = lastName;
        }

        public override string? ToString()
        {
            string fullNameAge = $"{FirstName} {LastName} {Age}";
            return fullNameAge+"\t"+base.ToString();
        }
    }

    public class PersonCollectiom : IEnumerable
    {
        private ArrayList arrayPeople = new();

        public void Add(Person person)
        {
            arrayPeople.Add(person);
        }

        public void Clear() 
        {
            arrayPeople.Clear();
        }

        public int Count => arrayPeople.Count;

        public Person GetPerson(int index) => (Person)arrayPeople[index]!;

        public IEnumerator GetEnumerator() => arrayPeople.GetEnumerator();
    }
}
