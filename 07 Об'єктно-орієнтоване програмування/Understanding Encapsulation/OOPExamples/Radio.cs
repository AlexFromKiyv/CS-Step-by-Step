using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPExamples;

class Radio
{
    bool state;
    public void ChangeState(bool state)
    {
        this.state = state;
    }
    public void GetState()
    {
        string result = state ? "Radio is on" : "Radio is off";
        
        Console.WriteLine(result);
    }
}
