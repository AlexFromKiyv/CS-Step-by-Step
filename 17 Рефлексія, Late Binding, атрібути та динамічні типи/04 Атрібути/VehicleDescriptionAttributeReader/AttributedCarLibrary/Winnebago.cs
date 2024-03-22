using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributedCarLibrary;

[VehicleDescription("A very long, slow, but feature-rich auto")]
public  class Winnebago
{
    //[VehicleDescription("My rocking CD player")]

    //public ulong notCompliant; // ... Type is not CLS-compiant
    public void PlayMusic(bool On)
    {
        //...
    }

}
