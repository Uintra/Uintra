using System.Web;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Media.Strategies.ImageResize
{
    public class CentralFeedRenderStrategy : IRenderStrategy
    {
        public string Thumbnail { get; } = "width=238&height=158&mode=crop";
        public string Preview { get; } = "width=745&height=495&mode=crop";
        public string PreviewTwo { get; } = "width=359&height=239&mode=crop";
        public int MediaFilesToDisplay { get; } = HttpContext.Current.Request.IsMobileBrowser() ? 2 : 3;
    }
}