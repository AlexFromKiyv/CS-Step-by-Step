using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnonymousMethods
{
    internal class SomethingEventArgs : EventArgs
    {
        public readonly string message;
        public SomethingEventArgs(string message)
        {
            this.message = message;
        }
    }
}
