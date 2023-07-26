using System.Collections;


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

    public interface IStringContainer
    {
        string this[int index] { get; set; }
    }

    class SomeStrings : IStringContainer
    {
        private List<string> myString = new();

        public SomeStrings(List<string> myString)
        {
            this.myString = myString;
        }
        public int Count => myString.Count;
        public string this[int index]
        {
            get => myString[index];
            set => myString[index] = value;
        }
    }


}
