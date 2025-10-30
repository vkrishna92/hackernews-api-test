using hackernews_api_test.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hackernews_api_test.Clients
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiClient> _logger;
        private readonly string baseUrl;


        public ApiClient()
        {
            _logger = LoggerFactoryHelper.CreateLogger<ApiClient>();
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            baseUrl = config["BaseUrl"];
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            _logger.LogInformation("Sending GET request to {Url}", baseUrl + endpoint);
            return await _httpClient.GetAsync(baseUrl + endpoint);
        }
    }
}
