using Newtonsoft.Json;

namespace ProjectChronos.Model.Cist.Events;

public class Group
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
}
