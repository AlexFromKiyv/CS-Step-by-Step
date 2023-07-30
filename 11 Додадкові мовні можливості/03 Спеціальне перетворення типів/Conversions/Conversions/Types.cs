using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conversions
{
    class Base
    {
        public int Id { get; set; }
    }

    class Derived : Base 
    {
        public string? Name { get; set; }
    }

    struct Rectangle
    {
        public double Width { get; set; }
        public double Heigth { get; set; }

        public Rectangle(double width, double heigth)
        {
            Width = width;
            Heigth = heigth;
        }

        public override string? ToString()
        {
            return $"[{Width} x {Heigth}]";
        }
    }

    struct Square
    {
        public double Length { get; set; }

        public Square(double length)
        {
            Length = length;
        }
        public override string? ToString()
        {
            return $"[{Length} x {Length}]";
        }

        public static explicit operator Square(Rectangle rectangle) =>
            new Square(Math.Sqrt(rectangle.Heigth * rectangle.Width)  );        
    }

    struct Square_v1
    {
        public double Length { get; set; }

        public Square_v1(double length)
        {
            Length = length;
        }
        public override string? ToString()
        {
            return $"[{Length} x {Length}]";
        }

        public static explicit operator Square_v1(Rectangle rectangle) =>
           new Square_v1(Math.Sqrt(rectangle.Heigth * rectangle.Width));

        public static explicit operator Square_v1(double volume)
        {
            return new Square_v1(Math.Sqrt(volume));
        }

        public static explicit operator double(Square_v1 square)
        {
            return square.Length * square.Length;
        }
    }

    struct Rectangle_v1
    {
        public double Width { get; set; }
        public double Heigth { get; set; }

        public Rectangle_v1(double width, double heigth)
        {
            Width = width;
            Heigth = heigth;
        }

        public override string? ToString()
        {
            return $"[{Width} x {Heigth}]";
        }

        public static implicit operator Rectangle_v1(Square square) => new Rectangle_v1(square.Length, square.Length); 
 
    }

}
