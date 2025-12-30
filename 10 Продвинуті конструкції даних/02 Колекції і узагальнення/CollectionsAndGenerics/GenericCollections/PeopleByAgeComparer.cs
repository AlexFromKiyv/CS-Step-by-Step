namespace GenericCollections;

class PeopleByAgeComparer : IComparer<Person>
{
    public int Compare(Person? first, Person? second)
    {
        if (first!= null && second != null)
        {
            return first.Age.CompareTo(second.Age);
        }
        return 0;
    }
}
