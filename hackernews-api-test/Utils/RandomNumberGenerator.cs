using hackernews_api_test.Interfaces;
using System;

namespace hackernews_api_test.Utils
{
    public class RandomNumberGenerator : IRandomNumberGenerator
    {
        private readonly Random _random;

        public RandomNumberGenerator()
        {
            _random = new Random();
        }

        public int GetRandomNumber(int min, int max)
        {
            if (min > max)
            {
                throw new ArgumentException("Minimum value must be less than or equal to maximum value.");
            }

            return _random.Next(min, max + 1);
        }
    }
}
