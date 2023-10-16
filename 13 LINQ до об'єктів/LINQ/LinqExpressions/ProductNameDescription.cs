using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqExpressions
{
    internal class ProductNameDescription
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public override string? ToString()
        {
            return string.Format("{0,-30}{1,-30}", Name, Description);
        }
    }
}
