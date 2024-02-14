using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkWithConfiguration
{
    internal class Car
    {
        private string? war_code { get; set; }
        public string? Make { get; set; }
        public string? Color { get; set; }
        public string? EngineType { get; set; }
        public string? GetWarCode() => war_code;
    }
}
