using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisposableObjects
{
    class MyResourceWrapper : IDisposable
    {

        public string? Resource { get; set; }

        // When you are finished using the object, you must call this method.
        public void Dispose()
        {
            //Clean up unmanaged resource ...
            //Dispose other contained objects...
            Console.WriteLine("In dispose."  );
        }
    }

}
