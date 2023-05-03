using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    interface IDrawable_v3
    {
        void Draw();
        int TimeToDraw() => 5; 
    }

    interface IAdvencedDraw_v3 : IDrawable_v3
    {
        void DrawInBoundingBox(int top, int left, int bottom, int right);
        void DrawUpsideDown();
        new int TimeToDraw() => 15; // new implementation hide upstream

    }

    class BitmapImage_v3 : IAdvencedDraw_v3
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


    class BitmapImage_v4 : IAdvencedDraw_v3
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

        public int TimeToDraw() => 20; // Hide all default in interface
    }

}