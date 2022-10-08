using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Journey.Models.DTO
{
    public class Quote
    {
        /*
         * 
         * q = quote text
            a = author name
            i = author image (key required)
            c = character count
            h = pre-formatted HTML quote
        ex: "q": "Lack of emotion causes lack of progress and lack of motivation.",
	        "a": "Tony Robbins",
	        "i": "https://zenquotes.io/img/tony-robbins.jpg",
	        "c": "63",
         */
        public string Text { get; set; }
        public string Author { get; set; }
    }

    public class UserQuote :  IEntity, IDTO
    {
        [JsonPropertyName("userId")]
        public string UserId { get; set; }
        [JsonPropertyName("pk")]
        public string Pk { get { return this.UserId; } set { this.UserId = value; } }
        [JsonPropertyName("sk")]
        public string Sk { get { return this.UserId; } set { this.UserId = value; } }

        public string ItemName => "user_quote";
        [JsonPropertyName("quotes")]
        public IList<Quote> Quotes { get; set; } = new List<Quote>();
        
        public class Quote
        {
            [JsonPropertyName("text")]
            public string Text { get; set; }
            [JsonPropertyName("author")]
            public string Author { get; set; }
            [JsonPropertyName("lastShownAt")]
            public DateTime LastShownAt { get; set; } 
        }
}   }
