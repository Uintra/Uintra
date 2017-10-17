using System.Web;
using uIntra.Core.ApplicationSettings;
using uIntra.Core.Extensions;

namespace uIntra.Core.User
{
    public static class UserPhotoHelper
    {
        public static string GetUserPhotoOrDefaultAvatar(string userPhoto)
        {
            var applicationSettings = HttpContext.Current.GetService<IApplicationSettings>();
            return !string.IsNullOrEmpty(userPhoto) ? userPhoto : applicationSettings.DefaultAvatarPath;
        }
    }
}
