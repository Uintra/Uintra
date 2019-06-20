using Uintra.Core.Activity;
using Uintra.Core.Context;
using Uintra.Core.Controls.LightboxGallery;
using Uintra.Core.Extensions;
using Uintra.Core.Links;
using Uintra.Core.PagePromotion;
using Uintra.Core.User;

namespace Uintra.Core.Web
{
    [TrackContext]
    public abstract class PagePromotionControllerBase : ContextController
    {
        public override ContextType ControllerContextType { get; } = ContextType.PagePromotion;

        protected virtual string ItemViewPath { get; } = "~/App_Plugins/Core/PagePromotion/Item/ItemView.cshtml";
        protected virtual int DisplayedImagesCount { get; } = 3;
        protected virtual string PagePromotionTranslationPrefix { get; } = "PagePromotion.";

        private readonly IIntranetMemberService<IIntranetMember> _memberService;

        protected PagePromotionControllerBase(IIntranetMemberService<IIntranetMember> memberService, IContextTypeProvider contextTypeProvider)
            : base(contextTypeProvider)
        {
            _memberService = memberService;
        }

        protected virtual PagePromotionItemViewModel GetItemViewModel(PagePromotionBase item, IActivityLinks links)
        {
            var model = item.Map<PagePromotionItemViewModel>();
            model.MediaIds = item.MediaIds;
            model.Links = links;

            model.HeaderInfo = item.Map<IntranetActivityItemHeaderViewModel>();
            model.HeaderInfo.Owner = _memberService.Get(item.CreatorId).Map<MemberViewModel>();
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
