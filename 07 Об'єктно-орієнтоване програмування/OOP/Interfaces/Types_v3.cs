using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    /// <summary>
    /// Base interface for drawing 
    /// </summary>
    interface IDrawable_v1
    {
        void Draw();
    }

    interface IAdvencedDraw_v1 : IDrawable_v1
    {
        void DrawInBoundingBox(int top, int left, int bottom, int right);
        void DrawUpsideDown();
    }

    class BitmapImage_v1 : IAdvencedDraw_v1
    {
        public void Draw()
        {
            Console.WriteLine("Drawing");
        }

        public void DrawInBoundingBox(int top, int left, int bottom, int right)
        {
            Console.WriteLine("Drawing in box");
        }

        public void DrawUpsideDown()
        {
            Console.WriteLine("Drawing upside down");
        }
    }

}
