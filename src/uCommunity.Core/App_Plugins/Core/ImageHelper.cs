using System;
using System.IO;
using System.Web;

namespace uCommunity.Core.App_Plugins.Core
{
    public static class ImageConstants
    {
        public const int DefaultActivityOverviewImagesCount = 3;

        public const string DefaultAvatarPath = "~/Content/images/default-avatar.png";
    }

    public static class ImageHelper
    {
        public static string GetImageSrcAsBase64(string imagePath)
        {
            var result = "";
            if (File.Exists(imagePath))
            {
                result = ToBase64(imagePath);
            }

            return result;
        }

        public static string GetImageSrcOrDefaultAsBase64(string imagePath)
        {
            var image = GetImageSrcAsBase64(imagePath);
            if (string.IsNullOrEmpty(image))
            {
                string path = HttpContext.Current.Server.MapPath(ImageConstants.DefaultAvatarPath);
                return ToBase64(path);
            }

            return image;
        }

        private static string ToBase64(string imagePath)
        {
            var fileBytes = File.ReadAllBytes(imagePath);
            var base64String = Convert.ToBase64String(fileBytes);
            return $"data:image/jpeg;base64,{base64String}";
        }
    }
}