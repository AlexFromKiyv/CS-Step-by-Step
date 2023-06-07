using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Partial
{
    partial class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }

        public Person(int id, string name, double height, double weight)
        {
            Id = id;
            Name = name;
            Height = height;
            Weight = weight;
        }
        public Person() 
        {
            Name = "Name is no known";
        }
    }
}
