namespace Uintra20.Features.Media.Strategies.ImageResize
{
    public static class RenderStrategies
    {
        public static CentralFeedRenderStrategy ForCentralFeed = new CentralFeedRenderStrategy();
        public static ActivityDetailsRenderStrategy ForActivityDetails = new ActivityDetailsRenderStrategy();
    }
}