using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp;

class Employee4
{
    // Field data
    private int _id;
    private string _name = null!;
    private float _pay;
    private int _age;
    private string _SSN = null!;

    //Properties
    public int Id { get => _id; set => _id = value; }
    public string Name
    {
        get => _name; 
        set
        {
            if (value.Length > 15)
            {
                Console.WriteLine("Error! Name length exceeds 15 characters!");
            }
            else
            {
                _name = value;
            }
        }
    }
    public float Pay { get => _pay; set => _pay = value; }
    public int Age { get => _age; set => _age = value; }
    public string SSN => _SSN;


    //Constructors
    public Employee4(int id, string name, float pay, int age, string ssn)
    {
        Id = id;
        Name = name;
        Pay = pay;
        Age = age;
        // OOPS! This is no longer possible if the property is read only.
        //SSN = ssn;
        _SSN = ssn;
    }

    public Employee4(int id, string name, float pay) : 
        this(id, name, pay,0,"")
    {}

    public Employee4() {}

    // Mathods
    public void GiveBonus(float amount)
    {
        Pay += amount;
    }
    public void DisplayStatus()
    {
        Console.WriteLine($"{Id}\t{Name}\t{Age}\t{Pay}");
    }
}
