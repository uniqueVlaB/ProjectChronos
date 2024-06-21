
using Newtonsoft.Json;

namespace ProjectChronos.Models.Cist.Groups
{
    public class UniversityRootObject
    {
        [JsonProperty("university")]
        public University University { get; set; } = new();
    }
}
