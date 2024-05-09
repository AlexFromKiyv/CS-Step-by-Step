using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SimpleSerialization;

internal class JsonStringNullToEmptyConverter : JsonConverter<string>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {   
        string? value = reader.GetString();

        if (string.IsNullOrEmpty(value) )
        {
            return null;
        }
        return value;
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        value ??= string.Empty;
        writer.WriteStringValue(value);
    }

    public override bool HandleNull => true;
}
