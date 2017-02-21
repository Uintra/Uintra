using System;
using System.Collections.Generic;
using System.Linq;
using TeamDenmark.Common.ApiClient.Utils;

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
    }
}