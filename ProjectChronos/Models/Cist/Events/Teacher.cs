using Newtonsoft.Json;

namespace ProjectChronos.Models.Cist.Events;

public class Teacher
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("full_name")]
    public string FullName { get; set; } = string.Empty;

    [JsonProperty("short_name")]
    public string ShortName { get; set; } = string.Empty;
}
