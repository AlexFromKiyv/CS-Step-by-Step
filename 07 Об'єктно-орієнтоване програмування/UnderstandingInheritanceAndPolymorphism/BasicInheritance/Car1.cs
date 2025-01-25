using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicInheritance;

class Car1
{
    public readonly int MaxSpeed;
    private int _speed;

    public int Speed
    {
        get => _speed;
        set
        {
            _speed = value;
            if (_speed > MaxSpeed)
            {
                _speed = MaxSpeed;
            }
        }
    }

    public Car1(int maxSpeed)
    {
        MaxSpeed = maxSpeed;
    }
    public Car1()
    {
        MaxSpeed = 55;
    }
}
