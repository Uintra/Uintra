using Uintra.Core.Activity;
using Uintra.Core.Controls.LightboxGallery;
using Uintra.Core.Extensions;
using Uintra.Core.Links;
using Uintra.Core.PagePromotion;
using Uintra.Core.TypeProviders;
using Uintra.Core.User;
using Umbraco.Core;
using Umbraco.Web.Mvc;

namespace Uintra.Core.Web
{
    public abstract class PagePromotionControllerBase : SurfaceController
    {
        protected virtual string ItemViewPath { get; } = "~/App_Plugins/Core/PagePromotion/Item/ItemView.cshtml";
        protected virtual int DisplayedImagesCount { get; } = 3;
        protected virtual string PagePromotionTranslationPrefix { get; } = "PagePromotion.";

        private readonly IIntranetUserService<IIntranetUser> _userService;

        protected PagePromotionControllerBase(IIntranetUserService<IIntranetUser> userService)
        {
            _userService = userService;
        }

        protected virtual PagePromotionItemViewModel GetItemViewModel(PagePromotionBase item, IActivityLinks links)
        {
            var model = item.Map<PagePromotionItemViewModel>();
            model.MediaIds = item.MediaIds;
            model.Links = links;

            model.HeaderInfo = item.Map<IntranetActivityItemHeaderViewModel>();
            model.HeaderInfo.Owner = _userService.Get(item.CreatorId);
            model.HeaderInfo.Links = links;
            model.HeaderInfo.Type = item.Type;

            model.LightboxGalleryPreviewInfo = new LightboxGalleryPreviewModel
            {
                MediaIds = item.MediaIds,
                DisplayedImagesCount = DisplayedImagesCount,
                ActivityId = item.Id,
                ActivityType = item.Type
            };

            return model;
        }
    }
}
