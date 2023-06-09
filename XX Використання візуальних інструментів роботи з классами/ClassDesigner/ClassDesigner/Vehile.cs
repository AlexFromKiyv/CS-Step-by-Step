using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassDesigner
{
    public class Vehile
    {
        public string Name { get; set; }
        public string Producer { get; set; }

        public Vehile(string name, string producer)
        {
            Name = name;
            Producer = producer;
        }
    }
}
