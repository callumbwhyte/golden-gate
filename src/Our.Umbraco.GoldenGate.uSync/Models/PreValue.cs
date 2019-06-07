using Newtonsoft.Json;

namespace Our.Umbraco.GoldenGate.uSync.Models
{
    public class PreValue
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}