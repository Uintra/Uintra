using System.Web.Mvc;
using uIntra.Core.User;
using Umbraco.Web.Mvc;

namespace uIntra.Core.Web
{
    public abstract class UserControllerBase : SurfaceController
    {
        protected virtual string UserPhotoViewPath { get; } = "~/App_Plugins/Core/User/Photo.cshtml";

        public virtual ActionResult Photo(IIntranetUser user, string profilePageUrl = null, string additionalCssModificator = null)
        {
            var result = new UserPhotoViewModel
            {
                PhotoUrl = user.Photo,
                AltText = user.DisplayedName,
                ProfileUrl = profilePageUrl ?? string.Empty,
                AdditionalCssModificator = additionalCssModificator ?? string.Empty
            };

            return PartialView(UserPhotoViewPath, result);
        }
    }
}