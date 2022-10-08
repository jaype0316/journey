using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Journey.Models.DTO
{
    public class Book : IDTO, IEntity
    {
        [JsonPropertyName("userId")]
        public string UserId { get; set; }
        [JsonPropertyName("pk")]
        public string Pk { get; set; }
        [JsonPropertyName("sk")]
        public string Sk { get => UserId; set => value = UserId; }

        [JsonPropertyName("about")]
        public string About { get; set; }
        [JsonPropertyName("createdAt")]
        public DateTime? CreatedAt { get; set; }
        [JsonPropertyName("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
        public IList<ChapterHeader> Chapters { get; set; }
        public string ItemName { get => "book"; }
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("logoKey")]
        public string LogoKey { get; set; }
    }

    public class ChapterHeader 
    {
        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("updatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [JsonPropertyName("sequence")]
        public int Sequence { get; set; }
        [JsonPropertyName("pk")]
        public string Pk { get; set; }
        public IEnumerable<string>? Tags { get; set; } = new List<string>();
    }

    public class Chapter : IDTO, IEntity
    {
        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; }
        [JsonPropertyName("sequence")]
        public int Sequence { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; } = default!;
        [JsonPropertyName("body")]
        public string Body { get; set; } = default!;
        [JsonPropertyName("userId")]
        public string UserId { get; set; } = default!;
        [JsonPropertyName("createdAt")]
        public DateTime? CreatedAt { get; set; }
        [JsonPropertyName("pk")]
        public string Pk { get; set; }
        [JsonPropertyName("sk")]
        public string Sk { get => UserId; set => value = UserId; }

        public string ItemName => "chapters";

        [JsonPropertyName("logoKey")]
        public string LogoKey { get; set; }
        [JsonPropertyName("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
        [JsonPropertyName("tags")]
        public IEnumerable<string>? Tags { get; set; } = new List<string>();
    }
}
