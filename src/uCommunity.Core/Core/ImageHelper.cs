using System;
using System.IO;
using System.Web;

namespace uCommunity.Core
{
    public static class ImageConstants
    {
        public const int DefaultActivityOverviewImagesCount = 3;
    }

    public static class ImageHelper
    {
        private static string DefaultAvatarPath => AppSettingHelper.GetAppSetting<string>("DefaultAvatarPath");

        public static string GetImageSrcOrDefaultAsBase64(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                string path = HttpContext.Current.Server.MapPath(DefaultAvatarPath);
                return ToBase64(path);
            }

            return imagePath;
        }

        private static string ToBase64(string imagePath)
        {
            var fileBytes = File.ReadAllBytes(imagePath);
            var base64String = Convert.ToBase64String(fileBytes);
            return $"data:image/jpeg;base64,{base64String}";
        }
    }
}