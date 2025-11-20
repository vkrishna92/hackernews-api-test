using hackernews_api_test.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace hackernews_api_test.Interfaces
{
    public interface IHackerNewsService
    {
        Task<List<int>> GetTopStoriesAsync();
        Task<Item> GetItemAsync(int id);
        Task<HttpResponseMessage> GetItem(string id);
    }
}
