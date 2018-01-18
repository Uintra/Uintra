using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Extensions;

namespace uIntra.Core.Extensions
{
    public static class StringExtensions
    {
        public const string GroupIdQueryParam = "groupId";

        public static string Take(this string str, int n)
        {
            var substring = Enumerable.Take(str, n).ToArray();
            return new string(substring);
        }

        public static string SubstringAfter(this string str, string substring)
        {
            return str.Substring(str.IndexOf(substring) + substring.Length);
        }

        public static string AddQueryParameter(this string url, string query)
        {
            return url.AddParameter("query", query);
        }

        public static IEnumerable<int> ToIntCollection(this string str)
        {
            return str.IsNullOrEmpty() ? Enumerable.Empty<int>() : str.Split(',').Where(s => s.HasValue()).Select(int.Parse);
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

        public static bool Contains(this IEnumerable<string> source, string toCheck, StringComparison comp)
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

        public static string StripHtml(this string input)
        {
            if (input.IsNullOrEmpty())
            {
                return string.Empty;
            }

            return Regex.Replace(input, "<.*?>", string.Empty);
        }

        public static string AddIdParameter(this string url, object paramValue)
        {
            return AddParameter(url, "id", paramValue);
        }

        public static string AddParameter(this string url, string paramName, object paramValue)
        {
            var queryString = string.Empty;
            if (url.Contains("?"))
            {
                var urlSplit = url.Split('?');
                url = urlSplit[0];
                queryString = urlSplit.Length > 1 ? urlSplit[1] : string.Empty;
            }

            var queryCollection = HttpUtility.ParseQueryString(queryString);
            queryCollection.Add(paramName, paramValue.ToString());
            return $"{url.TrimEnd('/')}?{queryCollection}";
        }

        public static string RemoveHtmlTags(this string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public static int? ToNullableInt(this string str)
        {
            return int.TryParse(str, out var result) ? result : new int?();
        }

        public static string AddGroupId(this string url, Guid groupId)
        {
            return url.AddParameter(GroupIdQueryParam, groupId);
        }

        public static string ToAbsoluteUrl(this string source)
        {
            if (source == null)
                return null;

            return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + source;
        }

        public static string TrimByWordEnd(this string str, int maxLength)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            if (str.Length <= maxLength)
            {
                return str;
            }

            if (str[maxLength] == ' ')
            {
                return str.Substring(0, maxLength);
            }

            var lastIndex = str.Substring(0, maxLength).LastIndexOf(' ');
            return str.Substring(0, lastIndex).Trim();
        }

        public static string SplitOnUpperCaseLetters(this string str) =>
             str.IsNullOrEmpty() ? string.Empty : Regex.Split(str, @"(?<!^)(?=[A-Z])").JoinWithSeparator(" ");

        public static string ReplaceLineBreaksForHtml(this string src)
            => src.IsNullOrEmpty() ? string.Empty : src.Replace("\r\n", "<br />").Replace("\n", "<br />").Replace("\r", "<br />");

        public static IEnumerable<TResult> ParseStringCollection<TResult>(this string collection, Func<string, TResult> parserFunc, char separator = ',')
        {
            return collection.SplitBySeparator(separator).Select(parserFunc);
        }

        public static IEnumerable<string> SplitBySeparator(this string str, char separator)
        {
            if (str.IsNullOrEmpty())
            {
                return Enumerable.Empty<string>();
            }

            return str.Split(separator);
        }

        public static IEnumerable<TResult> ParseStringCollection<TResult>(this IEnumerable<string> collection, Func<string, TResult> parserFunc, char separator = ',')
        {
            return collection.SelectMany(col => col.ParseStringCollection(parserFunc, separator));
        }
    }
}