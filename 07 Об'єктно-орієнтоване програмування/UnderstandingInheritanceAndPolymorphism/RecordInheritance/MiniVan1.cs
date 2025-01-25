using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordInheritance;

public sealed record MiniVan1 : Car1
{
    public int Seating {  get; set; }

    public MiniVan1(string make, string model, string color, int seating) 
        : base(make, model, color)
    {
        Seating = seating;
    }
}
