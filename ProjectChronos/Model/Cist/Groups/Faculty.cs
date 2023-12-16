
using Newtonsoft.Json;

namespace ProjectChronos.Model.Cist.Groups
{
    public class Faculty
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("short_name")]
        public string ShortName { get; set; } = string.Empty;

        [JsonProperty("full_name")]
        public string FullName { get; set; } = string.Empty;

        [JsonProperty("directions", NullValueHandling = NullValueHandling.Ignore)]
        public List<Direction> Directions { get; set; } = new();
    }
}
