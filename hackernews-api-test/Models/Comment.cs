using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hackernews_api_test.Models
{
    public class Comment
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("by")]
        public string By { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
