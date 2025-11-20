using hackernews_api_test.Interfaces;
using hackernews_api_test.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace hackernews_api_test.Clients
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly IApiClient _apiClient;
        private readonly IItemFactory _itemFactory;
        private readonly ILogger<HackerNewsService> _logger;

        private const string TOP_STORIES_URL = "/topstories.json";
        private const string GET_ITEM = "/item/{0}.json";

        public HackerNewsService(IApiClient apiClient, IItemFactory itemFactory, ILogger<HackerNewsService> logger)
        {
            _apiClient = apiClient;
            _itemFactory = itemFactory;
            _logger = logger;
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
        public async Task<Item> GetItemAsync(int id)
        {
            string itemId = id.ToString();
            var response = await GetItem(itemId);

            // Check if the response is successful
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();

            // Use factory to create the item
            return _itemFactory.CreateItem(content);
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
