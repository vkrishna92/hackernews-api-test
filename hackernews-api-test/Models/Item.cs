using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hackernews_api_test.Models
{
    public class Item
    {
        public string By { get; set; }
        public int Id { get; set; }
        public long Time { get; set; }
        public string Type { get; set; }

        [JsonProperty("kids")]
        public List<int>? Kids { get; set; }

        [JsonProperty("parent")]
        public int? Parent { get; set; }
    }
}
