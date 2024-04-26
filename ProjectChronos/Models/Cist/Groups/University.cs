
using Microsoft.Maui.Controls;
using Newtonsoft.Json;

namespace ProjectChronos.Models.Cist.Groups
{
    public class University
    {
        [JsonProperty("short_name")]
        public string ShortName { get; set; } = string.Empty;

        [JsonProperty("full_name")]
        public string FullName { get; set; } = string.Empty;

        [JsonProperty("faculties", NullValueHandling = NullValueHandling.Ignore)]
        public List<Faculty> Faculties { get; set; } = new();

    }
}
