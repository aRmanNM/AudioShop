using System;
using System.Linq;

namespace API.Helpers
{
    public static class RandomString
    {
        public static string Generate()
        {
            const string chars = "0123456789abcdefghijklmnopqrstuvxwyz";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
