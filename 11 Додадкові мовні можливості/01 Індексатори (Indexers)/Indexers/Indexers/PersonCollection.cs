using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indexers;

public class PersonCollection 
{
    private ArrayList arrayOfPerson = new();
    public Person this[int index]
    {
        get => (Person)arrayOfPerson[index];
        set => arrayOfPerson.Insert(index,value);
    }

    public int Count => arrayOfPerson.Count;
}

