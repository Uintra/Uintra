namespace Uintra20.Features.Search.Extensions
{
    public static class SearchExtensions
    {
        public static string CropText(this string text, int sizeToCrop)
        {
            if (!string.IsNullOrEmpty(text) && text.Length > sizeToCrop * 2 && !text.Contains(SearchConstants.Global.HighlightPreTag))
            {
                return text.Substring(text.Length - sizeToCrop) + "...";
            }

            return text;
        }
    }
}
