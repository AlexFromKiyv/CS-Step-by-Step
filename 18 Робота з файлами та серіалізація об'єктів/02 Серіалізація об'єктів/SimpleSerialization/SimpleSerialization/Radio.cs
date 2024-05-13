using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SimpleSerialization
{
    public class Radio
    {
        [JsonInclude]
        public string? RadioId = "Sony-236";
        [JsonInclude]
        public bool HasTweeters;
        [JsonInclude]
        public bool HasSubWoofers;
        public List<double>? StationPresets { get; set; }

        public override string? ToString()
        {
            string? presets = StationPresets == null ? "" : string.Join(", ", 
                StationPresets.Select(i => i.ToString()).ToList());

            return "Object of "+base.ToString()+"\n"+ 
                $"\tRadioId: {RadioId}\n" +
                $"\tHasTweeters: {HasTweeters}\n" +
                $"\tHasSubWoofers:{HasSubWoofers}\n" +
                $"\tStation Presets:{presets}";
        }
    }
}