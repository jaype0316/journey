using Journey.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Journey.Models
{
    public class User : IEntity, IDTO
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("externalUserId")]
        public string ExternalUserId { get; set; }

        public string ItemName => "users";
        [JsonPropertyName("pk")]
        public string Pk { get; set; }
        [JsonPropertyName("sk")]
        public string Sk { get => ExternalUserId; set => value = ExternalUserId; }
    }
}
