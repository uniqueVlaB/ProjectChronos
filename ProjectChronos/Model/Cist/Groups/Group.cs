﻿
using Newtonsoft.Json;

namespace ProjectChronos.Model.Cist.Groups
{
    public class Group
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
    }
}
