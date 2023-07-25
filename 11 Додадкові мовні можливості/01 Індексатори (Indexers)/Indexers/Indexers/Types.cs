using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Indexers
{
    class Person
    {
        public string Name { get; set; } 
        public Person(string name = "")
        {
            Name = name;
        }
        public override string? ToString()
        {
            return $"{Name}\t"+base.ToString();
        }
    }


    class PersonCollection_v1
    {
        private Person[] arrayOfPerson = new Person[100];
        public int Count => arrayOfPerson.Length;

        //Indexer
        public Person this[int index] 
        { 
            get => arrayOfPerson[index];
            set => arrayOfPerson[index] = value;
        }
    }


    class PersonCollectionWithStringIndex : IEnumerable
    {
        private Dictionary<string, Person> dictionaryOfPerson = new();

        public Person this[string index]
        {
            get => dictionaryOfPerson[index];
            set => dictionaryOfPerson[index] = value;
        }

        public int Count => dictionaryOfPerson.Count;
        public void Clear()
        {
            dictionaryOfPerson.Clear();
        }
        IEnumerator IEnumerable.GetEnumerator() => dictionaryOfPerson.GetEnumerator();
    }


}
