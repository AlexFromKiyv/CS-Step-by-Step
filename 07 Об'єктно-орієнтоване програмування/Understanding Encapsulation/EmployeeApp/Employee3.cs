using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp;

class Employee3
{
    // Field data
    private int _id;
    private string _name = null!;
    private float _currentPay;
    private int _age;

    //Properties
    public int Id // Note lack of parentheses.
    {
        get { return _id; }
        set { _id = value; }
    }
    public string Name
    {
        get { return _name; }
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
    public float CurrentPay { get => _currentPay; set => _currentPay = value; }
    public int Age
    {
        get { return _age; }
        set { _age = value; }
    }

    //Constructors
    public Employee3()
    {
    }

    public Employee3(int id, string name, float currentPay)
        : this(id, name, currentPay, 0) { }

    //public Employee3(int id, string name, float currentPay, int age)
    //{
    //    _id = id;
    //    // Humm, this seems like a problem...
    //    if (name.Length > 15)
    //    {
    //        Console.WriteLine("Error! Name length exceeds 15 characters!");
    //    }
    //    else
    //    {
    //        _name = name;
    //    }
    //    _currentPay = currentPay;
    //    _age = age;
    //}

    public Employee3(int id, string name, float currentPay, int age)
    {
        Id = id;
        Name = name;
        CurrentPay = currentPay;
        Age = age;
    }

    //Methods

    public void GiveBonus(float amount)
    {
        _currentPay += amount;
    }
    public void DisplayStatus()
    {
        Console.WriteLine($"{_id}\t{_name}\t{_age}\t{_currentPay}");
    }

}
