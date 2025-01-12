using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp;

class Employee2
{
    // Field data
    private int _id;
    private string _name = null!;
    private float _currentPay;

    // Properties!
    
    // The 'int' represents the type of data this property encapsulates.
    public int Id // Note lack of parentheses.
    {
        get { return _id; }
        set { _id = value; }
    }
    public string Name
    {
        get {  return _name; }
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

    //Constructors
    public Employee2()
    {
    }

    public Employee2(int id, string name, float currentPay)
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
