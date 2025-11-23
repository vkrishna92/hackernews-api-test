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
            var stories = await HackerNewsService.GetTopStoriesAsync();

            StoryAssertions.AssertTopStoriesListIsValid(stories);
        }

        [TestCategory("regression")]
        [TestMethod]
        public async Task verify_top_stories_max_legth()
        {
            var stories = await HackerNewsService.GetTopStoriesAsync();

            StoryAssertions.AssertTopStoriesMaxLength(stories);
        }

        [TestMethod]
        [TestCategory("regression")]
        [TestCategory("smoke")]
        public async Task verify_top_storiy_item()
        {
            var stories = await HackerNewsService.GetTopStoriesAsync();
            StoryAssertions.AssertTopStoriesListIsValid(stories);

            var firstStoryId = stories.First();
            var story = await HackerNewsService.GetItemAsync(firstStoryId) as Story;

            StoryAssertions.AssertStoryIsValid(story, firstStoryId);
        }

        [TestMethod]
        [TestCategory("regression")]
        [TestCategory("smoke")]
        public async Task verify_top_story_comment()
        {
            var stories = await HackerNewsService.GetTopStoriesAsync();
            StoryAssertions.AssertTopStoriesListIsValid(stories);

            int randomIndex = RandomNumberGenerator.GetRandomNumber(0, stories.Count - 1);
            var storyId = stories[randomIndex];

            _logger.LogInformation($"Random StoryId: {storyId}");
            var story = await HackerNewsService.GetItemAsync(storyId) as Story;

            Assert.IsNotNull(story, $"Story with Id {storyId} cannot be null");

            if (story.Kids == null || story.Kids.Count == 0)
            {
                Assert.Inconclusive("This story does not have any comments.");
            }

            var firstCommentId = story.Kids[0];
            var comment = await HackerNewsService.GetItemAsync(firstCommentId) as Comment;

            StoryAssertions.AssertCommentIsValid(comment, firstCommentId, storyId);
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
