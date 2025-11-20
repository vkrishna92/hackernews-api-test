using System.Net.Http;
using System.Threading.Tasks;

namespace hackernews_api_test.Interfaces
{
    public interface IApiClient
    {
        Task<HttpResponseMessage> GetAsync(string endpoint);
    }
}
