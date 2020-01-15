using System;

namespace Uintra20.Features.Media.Strategies.ImageResize
{
    public class ActivityDetailsRenderStrategy : IRenderStrategy
    {
        public string Thumbnail { get; } = "width=238&height=158&mode=crop";
        public string Preview { get; } = "width=720&height=478&mode=crop";
        public string PreviewTwo { get; } = "width=359&height=239&mode=crop";
        public int MediaFilesToDisplay { get; } = int.MaxValue; //Means that all files should render
    }
}