using hackernews_api_test.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace hackernews_api_test.Clients
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiClient> _logger;
        private readonly string _baseUrl;

        public ApiClient(HttpClient httpClient, IConfiguration configuration, ILogger<ApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _baseUrl = configuration["BaseUrl"];

            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            _logger.LogInformation("Sending GET request to {Url}", _baseUrl + endpoint);
            return await _httpClient.GetAsync(_baseUrl + endpoint);
        }
    }
}
