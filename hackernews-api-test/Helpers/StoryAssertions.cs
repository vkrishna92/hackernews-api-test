using hackernews_api_test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace hackernews_api_test.Helpers
{
    public static class StoryAssertions
    {
        public static void AssertStoryIsValid(Story story, int expectedId)
        {
            Assert.IsNotNull(story, $"Story with Id {expectedId} could not be retrieved.");
            Assert.IsTrue(story.Id > 0, "Story Id cannot be less than or equal to zero");
            Assert.AreEqual("story", story.Type, "Item type is not story.");
            Assert.AreEqual(expectedId, story.Id, "Story Id not matching with the Id from topstories endpoint.");
        }

        public static void AssertCommentIsValid(Comment comment, int expectedId, int? expectedParentId = null)
        {
            Assert.IsNotNull(comment, $"Comment with Id: {expectedId} does not exist.");
            Assert.AreEqual("comment", comment.Type, "Item type is not comment.");

            if (expectedParentId.HasValue)
            {
                Assert.IsNotNull(comment.Parent, $"Comment with id {expectedId} should have a parentId");
                Assert.AreEqual(expectedParentId.Value, comment.Parent, "Parent Id not matching on the comment.");
            }
        }

        public static void AssertTopStoriesListIsValid(List<int> stories)
        {
            Assert.IsTrue(stories.Count > 0, "Top stories list should not be empty.");
        }

        public static void AssertTopStoriesMaxLength(List<int> stories, int maxLength = 500)
        {
            Assert.IsTrue(stories.Count <= maxLength, $"Top stories list exceed {maxLength} items.");
        }
    }
}
