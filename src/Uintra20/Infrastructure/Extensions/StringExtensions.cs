using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Compent.Extensions;
using UBaseline.Shared.Link;
using Uintra20.Features.Links.Models;

namespace Uintra20.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        private const string GroupIdQueryParam = "groupId";

        //private const string ToExtractAchorsTagsPattern = "</?(a|A).*?>";
        private const string ToExtractHtmlTagsPattern = "<.*?>";
        //private const string defaultType = "misc";
        //private const int maxViewedLength = 4;

        //public static string Take(this string str, int n)
        //{
        //    var substring = Enumerable.Take(str, n).ToArray();

        //    return new string(substring);
        //}

        //public static string SubstringAfter(this string src, string substring) =>
        //    src.Substring(src.IndexOf(substring) + substring.Length);

        //public static string AddQueryParameter(this string src, string query) =>
        //    src.AddParameter("query", query);

        public static IEnumerable<int> ToIntCollection(this string src) =>
            src.IsNullOrEmpty()
                ? Enumerable.Empty<int>()
                : src.Split(',').Where(s => s.HasValue()).Select(int.Parse);

        //public static string GetMedia(this string str, int count)
        //{
        //    if (str.IsNullOrEmpty()) return string.Empty;

        //    var fileIds = str.Split(',').ToList();

        //    return fileIds.Count <= count
        //        ? str
        //        : fileIds.Take(count).JoinWithComma();
        //}

        public static bool Contains(this IEnumerable<string> src, string toCheck, StringComparison comp) =>
            src.Any(s => s.IndexOf(toCheck, comp) >= 0);

        public static string JoinToString<T>(this IEnumerable<T> enumerable, string separator = ",") =>
            enumerable.Select(e => e.ToString()).JoinWith(separator);

        //public static string JoinWithComma(this IEnumerable<string> list) =>
        //    list.JoinWithSeparator(", ");

        //public static string JoinWithSeparator(this IEnumerable<string> list, string separator) =>
        //    list == null
        //    ? string.Empty
        //    : string.Join(separator, list);

        public static string StripHtml(this string src)
        {
            if (src.IsNullOrEmpty()) return string.Empty;

            return Regex.Replace(src, ToExtractHtmlTagsPattern, string.Empty);
        }

        public static string AddIdParameter(this string url, object paramValue) =>
            AddParameter(url, "id", paramValue);

        public static string AddParameter(this string url, string paramName, object paramValue)
        {
            if (url == null)
            {
                return null;
            }

            var queryString = string.Empty;
            if (url.Contains("?"))
            {
                var urlSplit = url.Split('?');
                url = urlSplit[0];
                queryString = urlSplit.Length > 1
                    ? urlSplit[1]
                    : string.Empty;
            }

            var queryCollection = HttpUtility.ParseQueryString(queryString);

            queryCollection.Add(paramName, paramValue.ToString());

            return $"{url.TrimEnd('/')}?{queryCollection}";
        }

        //public static int? ToNullableInt(this string src) =>
        //    int.TryParse(src, out var result)
        //    ? result
        //    : new int?();

        public static string AddGroupId(this string url, Guid groupId) =>
            url.AddParameter(GroupIdQueryParam, groupId);

        public static UintraLinkModel AddGroupId(this UintraLinkModel linkModel, Guid groupId)
        {
            var param = new UintraLinkParamModel()
            {
                Name = "groupId",
                Value = groupId.ToString()
            };

            if (!linkModel.Params.Any())
            {
                linkModel.BaseUrl += "?";
            }

            linkModel.Params = linkModel.Params.Append(param);
            linkModel.OriginalUrl = linkModel.ToString();

            return linkModel;
        }


        public static string ToAbsoluteUrl(this string src)
        {
            if (src == null) return null;

            return $"{HttpContext.Current.Request.Url.Scheme}{Uri.SchemeDelimiter}{HttpContext.Current.Request.Url.Host}{src}";
        }

        public static string TrimByWordEnd(this string src, int maxLength)
        {
            if (string.IsNullOrEmpty(src)) return string.Empty;

            if (src.Length <= maxLength) return src;

            if (src[maxLength] == ' ') return src.Substring(0, maxLength);

            var lastIndex = src.Substring(0, maxLength).LastIndexOf(' ');

            return lastIndex > 0
                ? src.Substring(0, lastIndex).Trim()
                : src;
        }

        public static string SplitOnUpperCaseLetters(this string src) =>
             src.IsNullOrEmpty()
            ? string.Empty
            : Regex.Split(src, @"(?<!^)(?=[A-Z])").JoinWith(" ");

        //public static string ReplaceLineBreaksForHtml(this string src)
        //    => src.IsNullOrEmpty()
        //    ? string.Empty
        //    : src.Replace("\r\n", "<br />").Replace("\n", "<br />").Replace("\r", "<br />");

        //public static string StripHtmlAnchors(this string src) =>
        //    Regex.Replace(src, ToExtractAchorsTagsPattern, string.Empty);

        public static IEnumerable<TResult> ParseStringCollection<TResult>(
            this string collection,
            Func<string, TResult> parserFunc,
            char separator = ',') =>
            collection.SplitBy(separator.ToString()).Select(parserFunc);



        //public static string ToExtensionViewString(this string src) =>
        //    src?.Length <= maxViewedLength
        //    ? src
        //    : defaultType;

        public static string GetEventDateTimeString(this DateTime from, DateTime to)
        {
            var result = from.Date == to.Date
                ? $"{from.ToDateTimeFormat()} - {to.ToTimeFormat()}"
                : $"{from.ToDateTimeFormat()} - {to.ToDateTimeFormat()}";

            return result;
        }

        public static UintraLinkModel ToLinkModel(this string url)
        {
            if (!url.HasValue())
            {
                return null;
            }

            var linkModel = new UintraLinkModel(url);

            if (url.Contains('?'))
            {
                var splitedUrl = url.Split('?');
                linkModel.BaseUrl = splitedUrl[0];
                linkModel.Params = splitedUrl[1].ToParams().ToList();
                return linkModel;
            }

            linkModel.BaseUrl = url;
            return linkModel;
        }

        public static IEnumerable<UintraLinkParamModel> ToParams(this string @params)
        {
            var splitedParams = @params.Split('&').ToList();

            foreach (var nameAndValue in splitedParams.Select(param => param.Split('=')))
            {
                yield return new UintraLinkParamModel()
                {
                    Name = nameAndValue[0],
                    Value = nameAndValue[1]
                };
            }
        }
    }
}