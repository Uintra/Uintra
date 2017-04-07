namespace uCommunity.Core
{
    public static class ImageConstants
    {
        public const int DefaultActivityOverviewImagesCount = 3;
    }

    public static class ImageHelper
    {
        private static string DefaultAvatarPath => AppSettingHelper.GetAppSetting<string>("DefaultAvatarPath");

        public static string GetImageSrcOrDefaultAvatar(string imagePath)
        {
            return !string.IsNullOrEmpty(imagePath) ? imagePath : DefaultAvatarPath;
        }
    }
}