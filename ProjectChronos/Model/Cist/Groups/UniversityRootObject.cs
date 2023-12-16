
using Newtonsoft.Json;

namespace ProjectChronos.Model.Cist.Groups
{
    public class UniversityRootObject
    {
        [JsonProperty("university")]
        public University University { get; set; } = new();
    }
}
