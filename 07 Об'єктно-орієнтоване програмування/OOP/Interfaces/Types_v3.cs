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
    interface IDrawable
    {
        void Draw();
    }

    interface IAdvencedDraw : IDrawable
    {
        void DrawInBoundingBox(int top, int left, int bottom, int right);
        void DrawUpsideDown();
    }

    class BitmapImage : IAdvencedDraw
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
