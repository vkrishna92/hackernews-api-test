using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hackernews_api_test.Utils
{
    public class LoggerFactoryHelper
    {
        private static readonly ILoggerFactory _factory = LoggerFactory.Create(builder =>
        {
            builder
                .AddConsole()
                .SetMinimumLevel(LogLevel.Information);
        });

        public static ILogger<T> CreateLogger<T>() => _factory.CreateLogger<T>();
    }
}
