using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomTypeConversions;

public struct Rectangle
{
    public int Width { get; set; }
    public int Height { get; set; }
    public Rectangle(int w, int h)
    {
        Width = w;
        Height = h;
    }
    public void Draw()
    {
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                Console.Write("*");
            }
            Console.WriteLine();
        }
    }
    public override string ToString()
      => $"[Width = {Width}; Height = {Height}]";

    public static implicit operator Rectangle(Square s)
    {
        Rectangle r = new Rectangle
        {
            Height = s.Length,
            Width = s.Length * 2 // Assume the length of the new Rectangle with (Length x 2).
        };
        return r;
    }
}

public struct Square
{
    public int Length { get; set; }
    public Square(int l) : this()
    {
        Length = l;
    }
    public void Draw()
    {
        for (int i = 0; i < Length; i++)
        {
            for (int j = 0; j < Length; j++)
            {
                Console.Write("*");
            }
            Console.WriteLine();
        }
    }
    public override string ToString() => $"[Length = {Length}]";
    // Rectangles can be explicitly converted into Squares.
    public static explicit operator Square(Rectangle r)
    {
        Square s = new Square { Length = r.Height };
        return s;
    }

    public static explicit operator Square(int sideLength)
    {
        Square newSq = new Square { Length = sideLength };
        return newSq;
    }
    public static explicit operator int(Square s) => s.Length;
}