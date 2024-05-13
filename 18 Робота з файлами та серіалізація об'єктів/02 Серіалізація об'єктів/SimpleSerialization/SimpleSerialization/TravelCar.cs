using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SimpleSerialization
{
    [Serializable, XmlRoot(Namespace = "http://www.MyCompany.com")]
    public class TravelCar : Car
    {
        [XmlAttribute]
        [JsonInclude]
        public bool CanFly;
        [XmlAttribute]
        [JsonInclude]
        public bool CanSubmerge;

        public override string? ToString()
        {
            return "Object of " + base.ToString() + "\n" +
                $"\tCanFly:{CanFly}\n" +
                $"\tCanSubmerge:{CanSubmerge}";
        }
    }
}
