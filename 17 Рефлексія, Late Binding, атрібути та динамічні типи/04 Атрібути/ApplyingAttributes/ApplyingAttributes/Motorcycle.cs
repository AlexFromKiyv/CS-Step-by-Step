using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApplyingAttributes;

internal class Motorcycle
{
    [JsonIgnore]
    public float weightOfCurrentPassengers; // This fild is unserializable
    // These fields are still serializable.
    public bool hasRadioSystem;
    public bool hasHeadSet;
    public bool hasSissyBar;
}
