using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hackernews_api_test.Models
{
    public class Story
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("by")]
        public string By { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("kids")]
        public List<int> Kids { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
