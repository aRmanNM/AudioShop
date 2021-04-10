using System.Text.RegularExpressions;

namespace API.Helpers
{
    public static class ClearString
    {
        public static string Clear(this string input)
        {
            return Regex.Replace(input.Trim(), @"\t|\n|\r| ", "");
        }
    }
}