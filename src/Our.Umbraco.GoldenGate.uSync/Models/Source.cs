using Newtonsoft.Json;

namespace Our.Umbraco.GoldenGate.uSync.Models
{
    public class Source
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}