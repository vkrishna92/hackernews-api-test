using AventStack.ExtentReports;
using hackernews_api_test.Clients;
using hackernews_api_test.Factories;
using hackernews_api_test.Interfaces;
using hackernews_api_test.Reports;
using hackernews_api_test.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Net.Http;
using Amazon.S3;
using Amazon.S3.Transfer;
using System;
using System.Threading.Tasks;

namespace hackernews_api_test.Tests
{
    [TestClass]
    public class BaseTest
    {
        protected IHackerNewsService HackerNewsService;
        protected IRandomNumberGenerator RandomNumberGenerator;
        protected ServiceProvider ServiceProvider;
        protected ExtentTest ExtentTest;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Setup()
        {
            // Create ExtentTest instance for this test
            var testName = TestContext.TestName;
            ExtentTest = ExtentReportManager.CreateTest(testName);
            ExtentTest.Info($"Starting test: {testName}");

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
            // Log test result to ExtentReports
            var outcome = TestContext.CurrentTestOutcome;
            switch (outcome)
            {
                case UnitTestOutcome.Passed:
                    ExtentTest.Pass("Test passed successfully");
                    break;
                case UnitTestOutcome.Failed:
                    ExtentTest.Fail("Test failed - check console output for details");
                    break;
                case UnitTestOutcome.Inconclusive:
                    ExtentTest.Skip("Test was inconclusive");
                    break;
                default:
                    ExtentTest.Warning($"Test outcome: {outcome}");
                    break;
            }

            ServiceProvider?.Dispose();
        }

        [AssemblyCleanup]
        public static async Task AssemblyCleanups()
        {
            // Flush all reports at the end of test execution
            ExtentReportManager.FlushReports();

            // Optionally save reports to S3
            await saveReportsToS3();            
        }

        private static async Task saveReportsToS3()
        {
            try
            {
                // Initialize S3 client
                IAmazonS3 s3Client = new AmazonS3Client();

                // Configure S3 bucket and report file details
                string bucketName = Environment.GetEnvironmentVariable("RESULT_BUCKET_NAME") ?? "test-reports";
                string reportFileName = ExtentReportManager.getFileName();
                string reportFilePath = Path.Combine(Directory.GetCurrentDirectory(), "TestReports", reportFileName);

                // Check if report file exists
                if (!File.Exists(reportFilePath))
                {
                    Console.WriteLine($"Report file not found at: {reportFilePath}");
                    return;
                }

                // Create S3 key with timestamp for versioning
                string s3Key = $"test-reports/{DateTime.UtcNow:yyyy-MM-dd}/{DateTime.UtcNow:HHmmss}_{reportFileName}";

                // Upload file to S3
                using (var fileTransferUtility = new TransferUtility(s3Client))
                {
                    await fileTransferUtility.UploadAsync(reportFilePath, bucketName, s3Key);
                    Console.WriteLine($"Successfully uploaded report to S3: s3://{bucketName}/{s3Key}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading reports to S3: {ex.Message}");
                // Don't throw - we don't want S3 upload failures to break tests
            }
        }
    }
}
