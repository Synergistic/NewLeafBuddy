using System;
using System.Text.RegularExpressions;

namespace NewLeaf.Services
{
    public static class Extensions
    {

        private static readonly Regex regex = new Regex(@"\s+");

        public static string StripWhitespace(this string text)
        {
            return regex.Replace(text, String.Empty);
        }
    }
}
