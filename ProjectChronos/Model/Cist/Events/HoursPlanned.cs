using Newtonsoft.Json;

namespace ProjectChronos.Model.Cist.Events;

public class HoursPlanned
{
    [JsonProperty("type")]
    public long? EventTypeId { get; set; }

    [JsonProperty("val")]
    public long Hours { get; set; }

    [JsonProperty("teachers")]
    public List<long> TeacherIds { get; set; } = new();
}
