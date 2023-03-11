using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstAndReadOnly
{
    internal class BodyMassIndex_v2
    {
        public static readonly double LESS_THEN_NORM = 18.5;
        public static readonly double OVER_THEN_NORM;

        static BodyMassIndex_v2()
        {
            OVER_THEN_NORM = 25;
        }
    }
}
