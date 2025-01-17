using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkWithRecords;

record CarRecord2
{
    public string Make { get; set; }
    public string Model { get; set; }
    public string Color { get; set; }
    public CarRecord2() { }
    public CarRecord2(string make, string model, string color)
    {
        Make = make;
        Model = model;
        Color = color;
    }
}
