using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indexers;

public class PersonCollectionStringIndexer : IEnumerable
{
    private Dictionary<string, Person> people = new Dictionary<string, Person>();

    // This indexer returns a person based on a string index.
    public Person this[string name]
    {
        get => (Person)people[name];
        set => people[name] = value;
    }

    public int Count => people.Count;

    public IEnumerator GetEnumerator() => people.GetEnumerator();
}
