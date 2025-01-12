using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPExamples;

class Car
{
    // Car 'has-a' Radio.
    private Radio _radio = new();
    public Car()
    {
        _radio.GetState();
    }
    public void TurnOnRadio()
    {
        // Delegate call to inner object.
        _radio.ChangeState(true);
        _radio.GetState();
    }
}
