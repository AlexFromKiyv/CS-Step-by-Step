using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SimpleSerialization
{
    public class Car
    {
        [JsonInclude]
        public bool IsHatchBack;
        [JsonInclude]
        public Radio Radio = new();

        public override string? ToString()
        {
            return $"HatchBack:{IsHatchBack}\t {Radio}";
        }
    }
}
