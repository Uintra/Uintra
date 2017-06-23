namespace uIntra.Search.Core
{
    public static class SearchHelper
    {
        public static string CropText(this string text, int sizeToCrop)
        {
            if (!string.IsNullOrEmpty(text) && text.Length > sizeToCrop * 2 && !text.Contains(SearchConstants.HighlightPreTag))
            {
                return text.Substring(text.Length - sizeToCrop) + "...";
            }

            return text;
        }
    }
}
