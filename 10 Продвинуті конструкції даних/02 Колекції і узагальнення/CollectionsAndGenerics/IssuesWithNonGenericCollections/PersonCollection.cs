using System.Collections;

namespace IssuesWithNonGenericCollections;

public class PersonCollection : IEnumerable
{
    private ArrayList arPeople = new ArrayList();
    public IEnumerator GetEnumerator() =>
        arPeople.GetEnumerator();

    // Cast for caller.
    public Person GetPerson(int pos) => (Person)arPeople[pos];
    
    // Insert only Person objects.
    public void AddPerson(Person p)
    {
        arPeople.Add(p);
    }
    // Other methods
    public void ClearPeople()
    {
        arPeople.Clear();
    }
    public int Count => arPeople.Count;
}
