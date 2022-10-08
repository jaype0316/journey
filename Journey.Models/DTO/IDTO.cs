using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Journey.Models.DTO
{
    public interface IDTO
    {
        [JsonPropertyName("pk")]
        public string Pk { get; set; }
        [JsonPropertyName("sk")]
        public string Sk { get; set; }
    }
}
