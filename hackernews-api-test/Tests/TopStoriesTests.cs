using hackernews_api_test.Helpers;
using hackernews_api_test.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace hackernews_api_test.Tests
{
    [TestClass]
    public class TopStoriesTests : BaseTest
    {
        private ILogger<TopStoriesTests> _logger;

        [TestInitialize]
        public new void Setup()
        {
            base.Setup();
            _logger = ServiceProvider.GetRequiredService<ILogger<TopStoriesTests>>();
        }

        [TestCategory("regression")]
        [TestCategory("smoke")]
        [TestMethod]
        public async Task verify_top_stories()
        {
            ExtentTest.AssignCategory("regression", "smoke");
            ExtentTest.Info("Fetching top stories from HackerNews API");

            var stories = await HackerNewsService.GetTopStoriesAsync();
            ExtentTest.Info($"Retrieved {stories.Count} top stories");

            StoryAssertions.AssertTopStoriesListIsValid(stories);
            ExtentTest.Pass("Top stories list validation successful");
        }

        [TestCategory("regression")]
        [TestMethod]
        public async Task verify_top_stories_max_legth()
        {
            ExtentTest.AssignCategory("regression");
            ExtentTest.Info("Verifying top stories list does not exceed maximum length");

            var stories = await HackerNewsService.GetTopStoriesAsync();
            ExtentTest.Info($"Story count: {stories.Count}");

            StoryAssertions.AssertTopStoriesMaxLength(stories);
            ExtentTest.Pass("Maximum length validation passed");
        }

        [TestMethod]
        [TestCategory("regression")]
        [TestCategory("smoke")]
        public async Task verify_top_storiy_item()
        {
            ExtentTest.AssignCategory("regression", "smoke");
            ExtentTest.Info("Verifying the first top story item details");

            var stories = await HackerNewsService.GetTopStoriesAsync();
            StoryAssertions.AssertTopStoriesListIsValid(stories);

            var firstStoryId = stories.First();
            ExtentTest.Info($"Testing story with ID: {firstStoryId}");

            var story = await HackerNewsService.GetItemAsync(firstStoryId) as Story;
            ExtentTest.Info($"Story title: {story?.Title}");

            StoryAssertions.AssertStoryIsValid(story, firstStoryId);
            ExtentTest.Pass("Story item validation successful");
        }

        [TestMethod]
        [TestCategory("regression")]
        [TestCategory("smoke")]
        public async Task verify_top_story_comment()
        {
            ExtentTest.AssignCategory("regression", "smoke");
            ExtentTest.Info("Verifying a random top story's comment");

            var stories = await HackerNewsService.GetTopStoriesAsync();
            StoryAssertions.AssertTopStoriesListIsValid(stories);

            int randomIndex = RandomNumberGenerator.GetRandomNumber(0, stories.Count - 1);
            var storyId = stories[randomIndex];

            _logger.LogInformation($"Random StoryId: {storyId}");
            ExtentTest.Info($"Selected random story ID: {storyId} (index: {randomIndex})");

            var story = await HackerNewsService.GetItemAsync(storyId) as Story;

            Assert.IsNotNull(story, $"Story with Id {storyId} cannot be null");

            if (story.Kids == null || story.Kids.Count == 0)
            {
                ExtentTest.Skip("This story does not have any comments");
                Assert.Inconclusive("This story does not have any comments.");
            }

            var firstCommentId = story.Kids[0];
            ExtentTest.Info($"Testing first comment with ID: {firstCommentId}");

            var comment = await HackerNewsService.GetItemAsync(firstCommentId) as Comment;

            StoryAssertions.AssertCommentIsValid(comment, firstCommentId, storyId);
            ExtentTest.Pass("Comment validation successful");
        }

        [TestMethod]
        [TestCategory("regression")]
        public async Task verify_non_existent_item_returns_null()
        {
            var item = await HackerNewsService.GetItemAsync(int.MaxValue);
            Assert.IsNull(item, "Item with Id: " + int.MaxValue + " must be null.");
        }

        [TestMethod]
        [TestCategory("regression")]
        public async Task verify_null_item_id_returns_unauthorized()
        {
            var itemResponse = await HackerNewsService.GetItem(null);
            Assert.AreEqual(itemResponse.StatusCode, HttpStatusCode.Unauthorized, "Status code should be 401.");
        }

        [TestMethod]
        [TestCategory("regression")]
        public async Task verify_empty_item_id_returns_unauthorized()
        {
            var response = await HackerNewsService.GetItem(string.Empty);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Unauthorized, "Status code should be 401.");
        }
    }
}
