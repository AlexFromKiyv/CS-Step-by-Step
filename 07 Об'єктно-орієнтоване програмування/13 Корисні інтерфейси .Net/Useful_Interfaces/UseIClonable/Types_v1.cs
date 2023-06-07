using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseIClonable
{
    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y) {  X = x; Y = y; }
        public Point() { }
        public override string? ToString()
        {
            return $"{X},{Y}";
        }
    }

    class Point_v1 : Point, ICloneable
    {
        public Point_v1() {}
        public Point_v1(int x, int y) : base(x, y) {}
        public object Clone() => new Point_v1(X, Y);
    }

    class Point_v2 : Point, ICloneable
    {
        public Point_v2() { }
        public Point_v2(int x, int y) : base(x, y) { }
        public object Clone() => MemberwiseClone();

    }

    class PointDiscription
    {
        public Guid PointId { get; set; }
        public string Name { get; set; }
        public PointDiscription(string name = "No-name") 
        {
            PointId = Guid.NewGuid();
            Name = name;
        }
    }

    class Point_v3 :  ICloneable
    {
        public int X { get; set; }
        public int Y { get; set; }
        public PointDiscription Description { get; set; } = new PointDiscription();

        public Point_v3() { }
        public Point_v3(int x, int y) { X = x; Y = y; }

        public Point_v3(int x, int y, PointDiscription description) : this(x, y)
        {
            Description = description;
        }
        public override string? ToString() => $"{X},{Y}\t{Description?.Name}\t{Description?.PointId}";
        public object Clone() => MemberwiseClone();
    }

    class Point_v4 : ICloneable
    {
        public int X { get; set; }
        public int Y { get; set; }
        public PointDiscription Description { get; set; } = new PointDiscription();

        public Point_v4() { }
        public Point_v4(int x, int y) { X = x; Y = y; }

        public Point_v4(int x, int y, PointDiscription description) : this(x, y)
        {
            Description = description;
        }
        public override string? ToString() => $"{X},{Y}\t{Description?.Name}\t{Description?.PointId}";
        public object Clone()
        {
            Point_v4 newPoint = (Point_v4) MemberwiseClone();

            PointDiscription newPointDiscription = new PointDiscription();

            newPointDiscription.Name = Description.Name;

            newPoint.Description = newPointDiscription;

            return newPoint;
        }
    }

}
