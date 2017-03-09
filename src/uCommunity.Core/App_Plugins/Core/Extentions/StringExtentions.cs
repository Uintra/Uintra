using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace uCommunity.Core.App_Plugins.Core.Extentions
{
    public static class StringExtentions
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNotNullOrEmpty(this string str)
        {
            return !str.IsNullOrEmpty();
        }

        public static IEnumerable<int> ToIntCollection(this string str)
        {
            return str.IsNullOrEmpty() ? Enumerable.Empty<int>() : str.Split(',').Where(s => s.IsNotNullOrEmpty()).Select(int.Parse);
        }

        public static string GetMedia(this string str, int count)
        {
            if (str.IsNullOrEmpty())
            {
                return string.Empty;
            }

            var fileIds = str.Split(',').ToList();

            return fileIds.Count <= count ? str : fileIds.Take(count).JoinWithComma();
        }

        public static bool Contains(this string[] source, string toCheck, StringComparison comp)
        {
            return source.Any(s => s.IndexOf(toCheck, comp) >= 0);
        }

        public static string JoinToString<T>(this IEnumerable<T> enumerable, string separator = ",")
        {
            return string.Join(separator, enumerable);
        }

        public static string JoinWithComma(this IEnumerable<string> list)
        {
            return list.JoinWithSeparator(", ");
        }

        public static string JoinWithSeparator(this IEnumerable<string> list, string separator)
        {
            return list == null ? "" : string.Join(separator, list);
        }
     
        public static string CropText(this string text, int sizeToCrop)
        {
            if (!string.IsNullOrEmpty(text) && text.Length > sizeToCrop * 2 && !text.Contains(IntranetConstants.SearchConstants.HighlightPreTag))
            {
                return text.Substring(text.Length - sizeToCrop) + "...";
            }

            return text;
        }

        public static string StripHtml(this string input)
        {
            return Regex.Replace(input, "<.*?>", string.Empty);
        }
    }
}