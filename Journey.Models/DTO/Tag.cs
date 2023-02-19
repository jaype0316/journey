using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Journey.Models.DTO
{
    public class UserTag : IEntity, IDTO
    {
        public string ItemName => "tags";

        [JsonPropertyName("pk")]
        public string Pk { get; set; }
        [JsonPropertyName("sk")]
        public string Sk { get; set; }
        [JsonPropertyName("tags")]
        public IEnumerable<Tag> Tags { get; set; } 

        public class Tag
        {
            [JsonPropertyName("id")]
            public string? Id { get; set; }
            [JsonPropertyName("name")]
            public string Name { get; set; }
            [JsonPropertyName("isDefault")]
            public bool IsDefault { get; set; }
        }
    }
   
}
