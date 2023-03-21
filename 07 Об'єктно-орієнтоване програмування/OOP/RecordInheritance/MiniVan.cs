using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordInheritance
{
    public sealed record MiniVan : Car
    {
        public MiniVan(string manufacturer, string model, string color,int seating ) : base(manufacturer, model, color)
        {
            Seating = seating;
        }

        public int Seating { get; init; }
    }
}
