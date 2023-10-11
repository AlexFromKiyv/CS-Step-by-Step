using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqExpressions
{
    internal class ProductInfo
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int NumberInStock { get; set; } = 0;
        public override string? ToString()
        {
            return string.Format("{0,-30}{1,-30}{2,-20}", Name, Description, NumberInStock);
        }
    }
}
