using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstAndReadOnly
{
    internal class BodyMassIndex_v1
    {
        public readonly double LESS_THEN_NORM;
        public readonly double OVER_THEN_NORM;

        public BodyMassIndex_v1()
        {
            LESS_THEN_NORM = 18.5;
            OVER_THEN_NORM = 25;
        }
    }
}
