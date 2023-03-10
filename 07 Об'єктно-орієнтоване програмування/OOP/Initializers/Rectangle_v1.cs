using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Initializers
{
    internal class Rectangle_v1
    {
        public Point TopLeft { get; set; }
        public Point BottomRight { get; set; }

        public void ToConsole()
        {
            Console.WriteLine($"Rectangle [{TopLeft.X},{TopLeft.Y}],[{BottomRight.X},{BottomRight.Y}] \n");
        }
    }
}
