using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Journey.Models.DTO
{
    public class UserBooks : IEntity, IDTO, IIndexedEntity
    {
        public string ItemName => "book";

        [JsonPropertyName("pk")]
        public string Pk { get; set; }
        public string Sk { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        public string IndexName => "user_book";
    }
}
