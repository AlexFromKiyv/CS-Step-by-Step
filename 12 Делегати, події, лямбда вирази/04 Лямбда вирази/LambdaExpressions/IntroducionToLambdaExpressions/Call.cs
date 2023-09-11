using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroducionToLambdaExpressions
{
    internal class Call
    {
        public delegate string VerySimpleDelegate();
        private VerySimpleDelegate? _vsDelegate;
        public void SetVSDelegate(VerySimpleDelegate vsDelegate)
        {
            _vsDelegate = vsDelegate;
        }

        public void SaySomething()
        {
            Console.WriteLine(_vsDelegate?.Invoke());
        }
    }
}
