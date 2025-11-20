using hackernews_api_test.Clients;
using hackernews_api_test.Factories;
using hackernews_api_test.Interfaces;
using hackernews_api_test.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Net.Http;

namespace hackernews_api_test.Tests
{
    public class BaseTest
    {
        protected IHackerNewsService HackerNewsService;
        protected IRandomNumberGenerator RandomNumberGenerator;
        protected ServiceProvider ServiceProvider;

        [TestInitialize]
        public void Setup()
        {
            var services = new ServiceCollection();

            // Configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            // Logging
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Information);
            });

            // HttpClient
            services.AddSingleton<HttpClient>();

            // Register interfaces and implementations
            services.AddTransient<IApiClient, ApiClient>();
            services.AddTransient<IItemFactory, ItemFactory>();
            services.AddTransient<IHackerNewsService, HackerNewsService>();
            services.AddTransient<IRandomNumberGenerator, RandomNumberGenerator>();

            ServiceProvider = services.BuildServiceProvider();

            // Resolve dependencies
            HackerNewsService = ServiceProvider.GetRequiredService<IHackerNewsService>();
            RandomNumberGenerator = ServiceProvider.GetRequiredService<IRandomNumberGenerator>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            ServiceProvider?.Dispose();
        }
    }
}
