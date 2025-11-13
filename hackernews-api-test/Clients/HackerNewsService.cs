using hackernews_api_test.Models;
using hackernews_api_test.Utils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hackernews_api_test.Clients
{
    public class HackerNewsService
    {
        private readonly ApiClient _apiClient;
        private readonly ILogger<ApiClient> _logger;


        private readonly string TOP_STORIES_URL = "/topstories.json";
        private static readonly string GET_ITEM = "/item/{0}.json";
        public HackerNewsService() 
        {
            _logger = LoggerFactoryHelper.CreateLogger<ApiClient>();
            _apiClient = new ApiClient();
        }

        // get top stories
        public async Task<List<int>> GetTopStoriesAsync()
        {
            var response = await _apiClient.GetAsync(TOP_STORIES_URL);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<int>>(content);
        }

        // get item based on type
        public async Task<object> GetItemAsync(int id)
        {
            string itemId = id.ToString();
            var response = await GetItem(itemId);
            
            // Check if the response is successful
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            
            var content = await response.Content.ReadAsStringAsync();
            
            // Check if content is null or empty
            if (string.IsNullOrWhiteSpace(content))
            {
                return null;
            }
            
            // Deserialize to dynamic to check the type
            dynamic dynamicItem = JsonConvert.DeserializeObject<dynamic>(content);
            
            // Check if dynamicItem is null
            if (dynamicItem == null)
            {
                return null;
            }
            
            // Determine the type and convert accordingly
            string type = dynamicItem.type?.ToString().ToLower();
            
            try 
            {
                switch (type)
                {
                    case "story":
                        return JsonConvert.DeserializeObject<Story>(content);
                    case "comment":
                        return JsonConvert.DeserializeObject<Comment>(content);
                    default:
                        return null;
                }
            }
            catch (JsonException)
            {
                // Handle potential deserialization errors
                return null;
            }
        }

        // get item
        public async Task<HttpResponseMessage> GetItem(string id)
        {
            string endpoint = string.Format(GET_ITEM, id);
            var response = await _apiClient.GetAsync(endpoint);
            return response;            
        }
    }
}
