using hackernews_api_test.Interfaces;
using hackernews_api_test.Models;
using Newtonsoft.Json;
using System;

namespace hackernews_api_test.Factories
{
    public class ItemFactory : IItemFactory
    {
        public Item CreateItem(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            try
            {
                // Deserialize to dynamic to check the type
                dynamic dynamicItem = JsonConvert.DeserializeObject<dynamic>(json);

                if (dynamicItem == null)
                {
                    return null;
                }

                // Determine the type and convert accordingly
                string type = dynamicItem.type?.ToString().ToLower();

                return type switch
                {
                    "story" => JsonConvert.DeserializeObject<Story>(json),
                    "comment" => JsonConvert.DeserializeObject<Comment>(json),
                    _ => null
                };
            }
            catch (JsonException)
            {
                return null;
            }
        }
    }
}
