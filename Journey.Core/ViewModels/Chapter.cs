using Journey.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Journey.Core.ViewModels
{
    public class Chapter : IIndexedEntity
    {
        [JsonPropertyName("pk")]
        public string Pk { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("body")]
        public string Body { get; set; }
        [JsonPropertyName("userId")]
        public string UserId { get; set; }
        [JsonPropertyName("tags")]
        public IEnumerable<string> Tags { get; set; }
        [JsonPropertyName("sequence")]
        public int Sequence { get; set; }

        public string IndexName => "user_chapters";

        public string ItemName => "chapters";
    }
}
