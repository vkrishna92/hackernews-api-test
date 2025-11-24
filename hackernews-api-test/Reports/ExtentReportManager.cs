using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;
using System;
using System.IO;

namespace hackernews_api_test.Reports
{
    public class ExtentReportManager
    {
        private static ExtentReports _extentReports;
        private static readonly object _lock = new object();
        private static string _fileName;
        public static ExtentReports GetExtentReports()
        {
            if (_extentReports == null)
            {
                lock (_lock)
                {
                    if (_extentReports == null)
                    {
                        InitializeExtentReports();
                    }
                }
            }
            return _extentReports;
        }

        private static void InitializeExtentReports()
        {
            // Create report directory if it doesn't exist
            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "TestReports");
            if (!Directory.Exists(reportPath))
            {
                Directory.CreateDirectory(reportPath);
            }
            // Filename 
            _fileName = $"TestReport_{DateTime.Now:yyyyMMdd_HHmmss}.html";
            // Create HTML reporter
            var htmlReporter = new ExtentSparkReporter(Path.Combine(reportPath, _fileName));

            // Configure HTML reporter
            htmlReporter.Config.DocumentTitle = "HackerNews API Test Report";
            htmlReporter.Config.ReportName = "API Test Execution Report";
            htmlReporter.Config.Theme = Theme.Standard;
            htmlReporter.Config.Encoding = "UTF-8";
            htmlReporter.Config.TimeStampFormat = "MMM dd, yyyy HH:mm:ss";

            // Initialize ExtentReports
            _extentReports = new ExtentReports();
            _extentReports.AttachReporter(htmlReporter);

            // Add system/environment information
            _extentReports.AddSystemInfo("Application", "HackerNews API");
            _extentReports.AddSystemInfo("Environment", "Test");
            _extentReports.AddSystemInfo("User", Environment.UserName);
            _extentReports.AddSystemInfo("Machine", Environment.MachineName);
            _extentReports.AddSystemInfo("OS", Environment.OSVersion.ToString());
            _extentReports.AddSystemInfo(".NET Version", Environment.Version.ToString());
        }

        public static ExtentTest CreateTest(string testName, string description = "")
        {
            return GetExtentReports().CreateTest(testName, description);
        }

        public static void FlushReports()
        {
            _extentReports?.Flush();
        }

        public static string getFileName()
        {
            return _fileName;
        }
    }
}
