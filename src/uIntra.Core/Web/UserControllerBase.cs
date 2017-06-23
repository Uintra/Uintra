using System.Web.Mvc;
using uIntra.Core.User;
using Umbraco.Web.Mvc;

namespace uIntra.Core.Web
{
    public abstract class UserControllerBase : SurfaceController
    {
        protected virtual string UserPhotoViewPath { get; } = "~/App_Plugins/Core/User/Photo.cshtml";

        public virtual ActionResult Photo(IIntranetUser user)
        {
            var result = new UserPhotoViewModel
            {
                Id = user.Id,
                Photo = user.Photo
            };

            return PartialView(UserPhotoViewPath, result);
        }
    }
}
