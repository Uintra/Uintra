namespace Uintra20.Features.Media.Strategies.ImageResize
{
    public static class ImageResizeStrategies
    {
        public static CentralFeedImageResizeStrategy ForCentralFeed = new CentralFeedImageResizeStrategy();
        public static ActivityDetailsImageResizeStrategy ForActivityDetails = new ActivityDetailsImageResizeStrategy();
    }
}