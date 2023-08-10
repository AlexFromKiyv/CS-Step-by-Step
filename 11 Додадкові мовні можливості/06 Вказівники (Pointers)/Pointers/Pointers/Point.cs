using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pointers
{
    struct Point
    {
        public int x;
        public int y;
        public override string? ToString() => $"({x},{y})";

    }

    class PointRef
    {
        public int x;
        public int y;
        public override string? ToString() => $"({x},{y})";
    }

}
