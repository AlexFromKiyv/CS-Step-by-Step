using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordInheritance
{
    record MiniVan_v1(string Manufacturer, string Model, string Color, int Seating)
        : Car_v1(Manufacturer, Model, Color);

}
