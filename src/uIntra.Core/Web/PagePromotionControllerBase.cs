using uIntra.Core.Activity;
using uIntra.Core.Controls.LightboxGallery;
using uIntra.Core.Extensions;
using uIntra.Core.Links;
using uIntra.Core.PagePromotion;
using uIntra.Core.User;
using Umbraco.Web.Mvc;

namespace uIntra.Core.Web
{
    public abstract class PagePromotionControllerBase : SurfaceController
    {
        protected virtual string ItemViewPath { get; } = "~/App_Plugins/Core/PagePromotion/Item/ItemView.cshtml";
        protected virtual int DisplayedImagesCount { get; } = 3;

        private readonly IIntranetUserService<IIntranetUser> _userService;

        protected PagePromotionControllerBase(IIntranetUserService<IIntranetUser> userService)
        {
            _userService = userService;
        }

        protected virtual PagePromotionItemViewModel GetItemViewModel(PagePromotionBase page, IActivityLinks links)
        {
            var model = page.Map<PagePromotionItemViewModel>();
            model.MediaIds = page.MediaIds;
            model.Links = links;

            model.HeaderInfo = page.Map<IntranetActivityItemHeaderViewModel>();
            model.HeaderInfo.Owner = _userService.Get(page.CreatorId);
            model.HeaderInfo.Links = links;

            model.LightboxGalleryPreviewInfo = new LightboxGalleryPreviewModel
            {
                MediaIds = page.MediaIds,
                DisplayedImagesCount = DisplayedImagesCount,
                ActivityId = page.Id,
                ActivityType = page.Type
            };

            return model;
        }
    }
}
