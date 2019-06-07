using Newtonsoft.Json;
using System.Collections.Generic;

namespace Our.Umbraco.GoldenGate.uSync.Models
{
    public class PreValues
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Filter { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? MaxNumber { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? MinNumber { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? ShowOpen { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public dynamic TreeSource { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? Multiple { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public dynamic Rte { get; set; }

        public dynamic Items { get; set; }
    }
}