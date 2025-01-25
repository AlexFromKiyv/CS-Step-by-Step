using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp;

class Employee1
{
    // Field data
    private int _id;
    private string _name = null!;
    private float _currentPay;

    // Accessor (get method).
    public string GetName() => _name;

    // Mutator (set method).

    public void SetName(string name)
    {
        // Do a check on incoming value
        // before making assignment.
        if (name.Length > 15)
        {
            Console.WriteLine("Error! Name length exceeds 15 characters!");
        }
        else
        {
            _name = name;
        }
    }

    // Constructors
    public Employee1()
    {
    }

    public Employee1(int id, string name, float currentPay)
    {
        _id = id;
        _name = name;
        _currentPay = currentPay;
    }

    //Methods
    public void GiveBonus(float amount)
    {
        _currentPay += amount;
    }

    public void DisplayStatus()
    {
        Console.WriteLine($"{_id}\t{_name}\t{_currentPay}");
    }
}
