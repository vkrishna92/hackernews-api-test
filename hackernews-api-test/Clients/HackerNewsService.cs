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

        // get story 
        public async Task<Story> GetStoryAsync(int id)
        {
            string endpoint = string.Format(GET_ITEM, id);
            var response = await _apiClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Story>(content);
        }

        // get comment 
        public async Task<Comment> GetCommentAsync(int id)
        {
            string endpoint = string.Format(GET_ITEM, id);
            var response = await _apiClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            
            var contnet = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Comment>(contnet);
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
