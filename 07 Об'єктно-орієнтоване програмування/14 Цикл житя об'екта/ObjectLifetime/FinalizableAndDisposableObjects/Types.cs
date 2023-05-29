using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalizableAndDisposableObjects
{

    class MyResourceWrapper : IDisposable
    {
        ~MyResourceWrapper()
        {
            //Clean up any internal unmanaged resources
            // Don't call Dispose on any managed objects
        }

        public void Dispose()
        {
            //Clean up unmanaged resources here
            //Call Dispose on other contained disposable objects
            GC.SuppressFinalize(this);
        }
    }

    class goodResourseWrapper : IDisposable 
    {
        // Used to determine if Dispose() has already been called.
        private bool disposed = false;

        public string? Resource  { get; set; }

        public void Dispose()
        {
            // Call our helper method
            // Object user triggered cleanup
            CleanUp(true);

            GC.SuppressFinalize(this);
        }

        private void CleanUp(bool disposing)
        {
            if (!disposed) 
            { 
                if (disposing)
                {
                    // Dispose managed resources.
                }
                // Clean up unmanaged resources here.
                disposed = true;
            }
        }

        ~goodResourseWrapper()
        {            
            // Call our helper method
            // GC triggered cleanup
            CleanUp(false);
        }
    }
}
