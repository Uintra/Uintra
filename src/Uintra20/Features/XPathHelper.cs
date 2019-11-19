using System.Collections.Generic;
using System.Linq;

namespace Uintra20.Features
{
    public static class XPathHelper
    {
        public static string GetXpath(IEnumerable<string> documentTypesAliases) =>
            GetXpath(documentTypesAliases.ToArray());

        public static string GetXpath(params string[] documentTypesAliases)
        {
            var result = "root/";
            foreach (var alias in documentTypesAliases)
            {
                result += $"{alias}[@isDoc]/";
            }

            return result.TrimEnd('/');
        }

        public static string GetDescendantsXpath(IEnumerable<string> xPath)
        {
            if (!xPath.Any())
                return "root/";
            var last = xPath.Last();
            var tempArray = xPath.Take(xPath.Count() - 1);
            return GetXpath(tempArray) + "//" + last;
        }
    }
}