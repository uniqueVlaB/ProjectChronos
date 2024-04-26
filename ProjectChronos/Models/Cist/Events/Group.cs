using Newtonsoft.Json;

namespace ProjectChronos.Models.Cist.Events;

public class Group
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
}
