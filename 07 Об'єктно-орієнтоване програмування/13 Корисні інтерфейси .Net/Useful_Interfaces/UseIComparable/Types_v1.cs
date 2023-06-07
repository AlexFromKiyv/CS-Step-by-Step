using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseIComparable
{
    class Car
    {
        public int Id { get; set; } 
        public string Name { get; set; } = "";
        public int MaxSpeed { get; }

        public Car(int id, string name, int maxSpeed)
        {
            Id = id;
            Name = name;
            MaxSpeed = maxSpeed;
        }
        public Car() {}

        public override string? ToString() => $"{Id}\t{Name}\tMax.Spped:{MaxSpeed}";

    }

    class Car_v1 : Car, IComparable
    {
        public Car_v1() {}
        public Car_v1(int id, string name, int maxSpeed) : base(id, name, maxSpeed)
        {
        }

        public int CompareTo(object? obj)
        {
            if (obj is Car_v1 otherCar)
            {
                if (Id > otherCar.Id)
                {
                    return 1;
                }

                if (Id < otherCar.Id)
                {
                    return -1;
                }

                return 0;
            }
            else
            {
                throw new ArgumentException("The parameter is not a compatible type.");
            }
        }
    }

    class Car_v2 : Car, IComparable
    {
        public Car_v2() {}

        public Car_v2(int id, string name, int maxSpeed) : base(id, name, maxSpeed) {}

        public int CompareTo(object? obj)
        {
            if (obj is Car_v2 otherCar)
            {
                return Id.CompareTo(otherCar.Id);
            }

            throw new ArgumentException("The parameter is not a compatible type.");
        }
    }

    class Car_v3 : Car, IComparable
    {
        public Car_v3() { }

        public Car_v3(int id, string name, int maxSpeed) : base(id, name, maxSpeed) { }

        public int CompareTo(object? obj)
        {
            if (obj is Car_v3 otherCar)
            {
                return MaxSpeed.CompareTo(otherCar.MaxSpeed);
            }
            throw new ArgumentException("The parameter is not a compatible type.");
        }
    }

    public class Car_v3NameComparer : IComparer
    {
        public int Compare(object? x, object? y)
        {
            if (x is Car_v3 car1 && y is Car_v3 car2)
            {
                return string.Compare(car1.Name, car2.Name, StringComparison.OrdinalIgnoreCase);
            }
            throw new ArgumentException("The parameter is not a compatible type.");
        }
    }

    public class CarNameComparer : IComparer
    {
        public int Compare(object? x, object? y)
        {
            if (x is Car car1 && y is Car car2)
            {
                return string.Compare(car1.Name, car2.Name, StringComparison.OrdinalIgnoreCase);
            }
            throw new ArgumentException("The parameter is not a compatible type.");
        }
    }

    public class CarIdComparer : IComparer
    {
        public int Compare(object? x, object? y)
        {
            if (x is Car car1 && y is Car car2)
            {
                return car1.Id.CompareTo(car2.Id);
            }
            throw new ArgumentException("The parameter is not a compatible type.");
        }
    }

    public class CarMaxSppedComparer : IComparer
    {
        public int Compare(object? x, object? y)
        {
            if (x is Car car1 && y is Car car2)
            {
                return car1.MaxSpeed.CompareTo(car2.MaxSpeed);
            }
            throw new ArgumentException("The parameter is not a compatible type.");
        }
    }

    class Car_v4 : Car
    {
        public Car_v4() { }

        public Car_v4(int id, string name, int maxSpeed) : base(id, name, maxSpeed) { }

        public static IComparer SortByName => new Car_v4NameComparer();
    }

    public class Car_v4NameComparer : IComparer
    {
        public int Compare(object? x, object? y)
        {
            if (x is Car_v4 car1 && y is Car_v4 car2)
            {
                return string.Compare(car1.Name, car2.Name, StringComparison.OrdinalIgnoreCase);
            }
            throw new ArgumentException("The parameter is not a compatible type.");
        }
    }

}
