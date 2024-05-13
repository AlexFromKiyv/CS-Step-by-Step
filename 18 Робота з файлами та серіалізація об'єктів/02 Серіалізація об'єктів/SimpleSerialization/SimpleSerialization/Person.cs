using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SimpleSerialization
{
    public class Person
    {
        // public
        [JsonInclude]
        [JsonPropertyOrder(1)]
        public bool IsAlive = true;
        // private
        private int Age = 21;

        // full property
        private string firstName = string.Empty;
        [JsonPropertyOrder(-1)]
        public string FirstName { get => firstName; set => firstName = value; }

        public override string? ToString()
        {
            return "Object of " + base.ToString() + "\n" +
                $"\tFirstName:{FirstName}\n" +
                $"\tAge:{Age}IsAlive:{IsAlive}";
        }
    }
}
