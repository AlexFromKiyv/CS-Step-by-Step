using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCollections
{
    public class PersonComparerByAge : IComparer<Person>
    {
        public int Compare(Person? x, Person? y)
        {
            if (x != null && y != null)
            {
                return x.Age.CompareTo(y.Age);
            }
            if (x == null && y != null) return -1;
            if (x != null && y == null) return 1;
            return 0;
        }
    }
}
