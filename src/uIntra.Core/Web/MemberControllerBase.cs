using System.Web.Mvc;
using Uintra.Core.User;
using Umbraco.Web.Mvc;

namespace Uintra.Core.Web
{
    public abstract class MemberControllerBase : SurfaceController
    {
        protected virtual string UserPhotoViewPath { get; } = "~/App_Plugins/Core/User/Photo.cshtml";

        public virtual ActionResult Photo(MemberViewModel member, string profilePageUrl = null, string cssModificator = null, int photoWidth = 60)
        {
            var result = new MemberPhotoViewModel
            {
                PhotoUrl = member.Photo,
                AltText = member.DisplayedName,
                ProfileUrl = profilePageUrl ?? string.Empty,
                CssModificator = cssModificator ?? string.Empty,
                PhotoWidth = photoWidth
            };

            return PartialView(UserPhotoViewPath, result);
        }
    }
}