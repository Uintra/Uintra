using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Infrastructure.Helpers
{
    public class HtmlHelper
    {
        public static string CreateLink(string title, string url, bool openInNewTab = false)
        {
            var link = $"<a href=\"{url}\" target=\"{(openInNewTab ? "_blank " : "_self")}\">{title}</a>";
            return link;
        }
    }
}