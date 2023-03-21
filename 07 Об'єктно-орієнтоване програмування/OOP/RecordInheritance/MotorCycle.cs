using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordInheritance
{
    record MotorCycle(string Manufacturer, string Model);
    record Scooter(string Manufacturer, string Model):MotorCycle(Manufacturer,Model);

}
