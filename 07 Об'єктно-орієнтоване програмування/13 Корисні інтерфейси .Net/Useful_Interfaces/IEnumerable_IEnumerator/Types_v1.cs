using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEnumerable_IEnumerator
{
    class Car
    {
        public string Name { get; set; } = "";
        public int MaxSpeed { get; set; }

        public Car(string name, int maxSpeed)
        {
            Name = name;
            MaxSpeed = maxSpeed;
        }

        public Car()
        {
        }
    }

    class Garage : IEnumerable
    {
        private Car[] carArray = new Car[4];
        public Garage()
        {
            carArray[0] = new Car("VW Golf 1", 145);
            carArray[1] = new Car("VW Golf 2", 167);
            carArray[2] = new Car("VW Golf 3", 168);
            carArray[3] = new Car("VW Golf 4", 192);
        }

        public IEnumerator GetEnumerator() =>  carArray.GetEnumerator();
    }

    class Garage_v1 : IEnumerable
    {
        private Car[] carArray = new Car[4];
        public Garage_v1()
        {
            carArray[0] = new Car("VW Golf 1", 145);
            carArray[1] = new Car("VW Golf 2", 167);
            carArray[2] = new Car("VW Golf 3", 168);
            carArray[3] = new Car("VW Golf 4", 192);
        }

        IEnumerator IEnumerable.GetEnumerator() => carArray.GetEnumerator();
    }

    class Garage_v2 : IEnumerable
    {
        private Car[] carArray = new Car[4];
        public Garage_v2()
        {
            carArray[0] = new Car("VW Golf 1", 145);
            carArray[1] = new Car("VW Golf 2", 167);
            carArray[2] = new Car("VW Golf 3", 168);
            carArray[3] = new Car("VW Golf 4", 192);
        }

        public IEnumerator GetEnumerator()
        {
            yield return carArray[0];
            yield return carArray[1];
            yield return carArray[2];
            yield return carArray[3];

            foreach (Car car in carArray)
            {
                yield return car; 
            }

            yield return new Car("VW Polo",160);
            yield return new Car("VW Passat",180);

        }
    }

    class Garage_v3 : IEnumerable
    {
        private Car[] carArray = new Car[4];
        public Garage_v3()
        {
            carArray[0] = new Car("VW Golf 1", 145);
            carArray[1] = new Car("VW Golf 2", 167);
            carArray[2] = new Car("VW Golf 3", 168);
            carArray[3] = new Car("VW Golf 4", 192);
        }

        public IEnumerator GetEnumerator()
        {
            Console.WriteLine("Without MoveNext, it does not work.");
            yield return carArray[0];
            yield return carArray[1];
        }
    }

    class Garage_v4 : IEnumerable
    {
        private Car[] carArray = new Car[4];
        public Garage_v4()
        {
            carArray[0] = new Car("VW Golf 1", 145);
            carArray[1] = new Car("VW Golf 2", 167);
            carArray[2] = new Car("VW Golf 3", 168);
            carArray[3] = new Car("VW Golf 4", 192);
        }

        public IEnumerator GetEnumerator()
        {
            Console.WriteLine("Work in GetEnumerator (For example connect to DB)");

            return ActualImplementation();

            IEnumerator ActualImplementation()
            {
                yield return carArray[0];
                yield return carArray[1];
                yield return carArray[2];
            }
        }
    }

    class Garage_v5 
    {
        private Car[] carArray = new Car[4];
        public Garage_v5()
        {
            carArray[0] = new Car("VW Golf 1", 145);
            carArray[1] = new Car("VW Golf 2", 167);
            carArray[2] = new Car("VW Golf 3", 168);
            carArray[3] = new Car("VW Golf 4", 192);
        }

        public IEnumerable GetTheCars(bool returnReversed = false)
        {

            return ActualImplementation();

            IEnumerable ActualImplementation()
            {
                if (returnReversed)
                {
                    for (int i = carArray.Length - 1; i >= 0; i--)
                    {
                        yield return carArray[i];
                    }
                }
                else
                {
                    for (int i = 0; i < carArray.Length; i++)
                    {
                        yield return carArray[i];
                    }
                }
            }
        }
    }

}
