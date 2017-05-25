namespace uIntra.Core
{
    public static class XPathHelper
    {
        public static string GetXpath(params string[] documentTypesAliases)
        {
            var result = "root/";
            foreach (var alias in documentTypesAliases)
            {
                result += $"{alias}[@isDoc]/";
            }

            return result.TrimEnd('/');
        }
    }
}
