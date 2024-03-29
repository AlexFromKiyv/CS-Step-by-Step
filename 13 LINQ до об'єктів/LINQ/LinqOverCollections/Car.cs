﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqOverCollections
{
    internal class Car
    {
        public string PetName { get; set; } = "";
        public string Color { get; set; } = "";
        public int Speed { get; set; }
        public string Make { get; set; } = "";

        public override string? ToString()
        {
            return $"{PetName}\t{Color}\t{Speed}\t{Make}\t\t"+base.ToString();
        }
    }
}
