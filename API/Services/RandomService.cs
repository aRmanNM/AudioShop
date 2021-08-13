using System;

namespace API.Services
{
    public class RandomService
    {
        public Random random { get; }

        public RandomService()
        {
            random = new Random();
        }
    }
}