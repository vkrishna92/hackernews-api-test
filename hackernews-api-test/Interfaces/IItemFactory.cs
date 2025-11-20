using hackernews_api_test.Models;

namespace hackernews_api_test.Interfaces
{
    public interface IItemFactory
    {
        Item CreateItem(string json);
    }
}
