using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceTypesVsAbstractClasses
{
    public abstract class CloneableType
    {
        public abstract object Clone();
    }

    public class Car
    {

    }
       
    //public class Bus : Car, CloneableType { } // cannot multiple base classes 

    
}
