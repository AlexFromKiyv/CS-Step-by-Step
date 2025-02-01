using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectOverrides;
class Person2
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int Age { get; set; }
    public string SSN { get; } = null!;

    public Person2(string firstName, string lastName, int age, string sSN)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        SSN = sSN;
    }
    public Person2()
    {
    }
    public override string? ToString() => base.ToString() + "\t" +
        $"[FirstName:{FirstName};LastName:{LastName};Age:{Age};SSN:{SSN}]";
    
    public override bool Equals(object? obj)
    {
        return obj is Person2 person &&
               FirstName == person.FirstName &&
               LastName == person.LastName &&
               Age == person.Age &&
               SSN == person.SSN;
    }
    public override int GetHashCode() => SSN.GetHashCode();
}
