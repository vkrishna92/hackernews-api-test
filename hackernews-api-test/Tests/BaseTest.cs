using hackernews_api_test.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hackernews_api_test.Tests
{
    public class BaseTest
    {
        protected HackerNewsService HackerNewsService;

        [TestInitialize]
        public void Setup()
        {
            HackerNewsService = new HackerNewsService();
        }
    }
}
