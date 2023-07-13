using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomGenericMethods
{
    internal struct Point<T>
    {
        private T? _x;
        private T? _y;

        public Point(T x, T y)
        {
            _x = x;
            _y = y;
        }

        public T? X
        {
            get => _x;
            set => _x = value;
        }

        public T? Y
        {
            get => _y;
            set => _y = value;
        }

        public override readonly string? ToString()
        {
            return $"[ {_x} , {_y} ]";
        }

        public void Reset()
        {
            _x = default;
            _y = default;
        }
    }
}
