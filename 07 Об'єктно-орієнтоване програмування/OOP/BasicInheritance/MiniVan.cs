using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicInheritance
{
    internal class MiniVan : Car
    {
        public void SpeedUp()
        {
            Speed++;
            //_currentSpeed++; //he hasn't access
        }
    }

}