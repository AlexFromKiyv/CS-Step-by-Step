using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operators
{
    class Point
    {

        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point()
        {
        }

        public override string? ToString() => $"[{X},{Y}]";
    }

    class Point_v1 : Point
    {
        public Point_v1(int x, int y) : base(x, y)
        {
        }

        public static Point_v1 operator + (Point_v1 point1, Point_v1 point2) =>
            new Point_v1(point1.X + point2.X, point1.Y + point2.Y);
        public static Point_v1 operator - (Point_v1 point1, Point_v1 point2) =>
            new Point_v1(point1.X - point2.X, point1.Y - point2.Y);

    }

    class Point_v2 : Point
    {
        public Point_v2(int x, int y) : base(x, y)
        {
        }

        public static Point_v2 operator + (Point_v2 point1, int change) =>
            new Point_v2(point1.X + change, point1.Y + change);
        public static Point_v2 operator - (Point_v2 point1, int change) =>
            new Point_v2(point1.X - change, point1.Y - change);
    }

    class Point_v3 : Point
    {
        public Point_v3(int x, int y) : base(x, y)
        {
        }
        public static Point_v3 operator + (Point_v3 point1, int change) =>
            new Point_v3(point1.X + change, point1.Y + change);
        public static Point_v3 operator + ( int change, Point_v3 point1) =>
            new Point_v3(change + point1.X , change + point1.Y );

    }

    class Point_v4 : Point
    {
        public Point_v4(int x, int y) : base(x, y)
        {
        }

        public static Point_v4 operator ++(Point_v4 point) =>
            new Point_v4(point.X+1, point.Y+1);
        public static Point_v4 operator --(Point_v4 point) =>
            new Point_v4(point.X - 1, point.Y - 1);
    }

    class Point_v5 : Point
    {
        public Point_v5(int x, int y) : base(x, y)
        {
        }

        public override bool Equals(object? obj)
        {
            return (obj?.ToString() == this.ToString());
        }

        public override int GetHashCode() => GetHashCode();

        public static bool operator ==(Point_v5 point1, Point_v5 point2)
        {
            return point1.Equals(point2);
        }

        public static bool operator !=(Point_v5 point1, Point_v5 point2)
        {
            return !point1.Equals(point2);
        }
    }

}
