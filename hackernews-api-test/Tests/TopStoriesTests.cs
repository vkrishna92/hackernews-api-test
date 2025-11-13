using hackernews_api_test.Clients;
using hackernews_api_test.Models;
using hackernews_api_test.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace hackernews_api_test.Tests
{
    [TestClass]
    public class TopStoriesTests : BaseTest
    {
        private readonly ILogger<TopStoriesTests> _logger = LoggerFactoryHelper.CreateLogger<TopStoriesTests>();
        [TestMethod]
        public async Task verify_top_stories()
        {
            var stories = await HackerNewsService.GetTopStoriesAsync();

            Assert.IsTrue(stories.Count > 0, "Top stories list should not empty.");
            Assert.IsTrue(stories.Count <= 500, "Top stories list not exceed 500 items.");

        }

        [TestMethod]
        public async Task verify_top_storiy_item()
        {
            var stories = await HackerNewsService.GetTopStoriesAsync();
            Assert.IsTrue(stories.Count > 0, "Top stories list should not be empty.");

            var firstStoryId = stories.First();
            var story = await HackerNewsService.GetItemAsync(firstStoryId) as Story;            

            Assert.IsNotNull(story, $"Story with Id {firstStoryId} could not be retrieved.");
            Assert.IsTrue(story.Id > 0, "Story Id cannot be less than or equal to zero");
            Assert.IsTrue(story.Type == "story", "Item type is not story.");
            Assert.IsTrue(story.Id == firstStoryId, "Story Id not matching with the Id from topstories end point.");
        }

        [TestMethod]
        public async Task verify_top_story_comment()
        {
            var stories = await HackerNewsService.GetTopStoriesAsync();
            Assert.IsTrue(stories.Count > 0, "Top stories list should not be empty.");

            int randomIndex = GetRandonNumber(0, stories.Count - 1);
            
            var firstStoryId = stories[randomIndex];

            _logger.LogInformation("Random StoryId " + firstStoryId);
            var story = await HackerNewsService.GetItemAsync(firstStoryId) as Story;
            
            Assert.IsNotNull(story, $"Story with Id {firstStoryId} cannot be null");
            
            if (story.Kids == null || story.Kids.Count == 0)
            {
                Assert.Inconclusive("This story does not have any comments.");
            }

            var firstCommentId = story.Kids[0];
            var comment = await HackerNewsService.GetItemAsync(firstCommentId) as Comment;

            Assert.IsNotNull(comment.Parent, "Comment with id "+firstCommentId+"  should have a parentId");
            Assert.IsTrue(comment.Parent == firstStoryId, "Parent Id not matching on the comment.");
            Assert.IsNotNull(comment, $"Comment with Id: {firstCommentId} does not exist.");
            Assert.IsTrue(comment.Type == "comment", "Item type is not comment.");
        }

        [TestMethod]
        public async Task verify_top_story_edge_cases()
        {
            var item = await HackerNewsService.GetItemAsync(int.MaxValue);
            Assert.IsNull(item, "Item with Id: " + int.MaxValue + " has be null.");

            var itemResponse = await HackerNewsService.GetItem(null);
            Assert.AreEqual(itemResponse.StatusCode, HttpStatusCode.Unauthorized, "Status coude should be 401");

            var response = await HackerNewsService.GetItem(string.Empty);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Unauthorized);

        }

        private int GetRandonNumber(int min, int max)
        {
            Random _random = new Random();

            // Validate input
            if (min > max)
            {
                throw new ArgumentException("Minimum value must be less than or equal to maximum value.");
            }

            // Use Random.Next() with min and max values
            return _random.Next(min, max + 1);
        }
    }
}
