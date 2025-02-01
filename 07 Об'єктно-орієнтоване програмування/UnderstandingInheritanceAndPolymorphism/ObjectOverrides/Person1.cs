using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectOverrides;
class Person1
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int Age { get; set; }

    public Person1(string firstName, string lastName, int age)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
    }

    public Person1()
    {
    }

    //public override string? ToString() =>
    //    $"[FirstName:{FirstName};LastName:{LastName};Age:{Age}]";

    public override string? ToString() => base.ToString()+"\t"+
        $"[FirstName:{FirstName};LastName:{LastName};Age:{Age}]";

    //public override bool Equals(object? obj)
    //{
    //    return obj is Person1 person &&
    //           FirstName == person.FirstName &&
    //           LastName == person.LastName &&
    //           Age == person.Age;
    //}

    //public override int GetHashCode()
    //{
    //    throw new NotImplementedException();
    //}
}
