using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroducionToLambdaExpressions
{
    internal class SimpleMath
    {
        public delegate void MathMessage(string message, int result);
        private MathMessage? _mmDelegate;
        public void SetMathMessageHandler(MathMessage target)
        {
            _mmDelegate = target;
        }

        public void Add(int x, int y)
        {
            _mmDelegate?.Invoke("Adding has complited!", x + y);
        }
    }
}
