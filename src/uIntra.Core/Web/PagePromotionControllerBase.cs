using uIntra.Core.Activity;
using uIntra.Core.Controls.LightboxGallery;
using uIntra.Core.Extensions;
using uIntra.Core.Links;
using uIntra.Core.PagePromotion;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using Umbraco.Core;
using Umbraco.Web.Mvc;

namespace uIntra.Core.Web
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

            model.HeaderInfo.Type = new IntranetType
            {
                Id = item.Type.Id,
                Name = $"{PagePromotionTranslationPrefix}{item.PageAlias.ToFirstUpper()}"
            };

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
