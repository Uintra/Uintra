using System.Web.Mvc;
using Compent.Uintra.Core.Activity.Models;
using Compent.Uintra.Core.PagePromotion.Entities;
using Compent.Uintra.Core.PagePromotion.Models;
using Uintra.Core;
using Uintra.Core.Context;
using Uintra.Core.Extensions;
using Uintra.Core.Feed;
using Uintra.Core.User;
using Uintra.Core.Web;

namespace Compent.Uintra.Controllers
{
    [TrackContext]
    public class PagePromotionController : PagePromotionControllerBase
    {
        protected override string ItemViewPath => "~/Views/PagePromotion/ItemView.cshtml";

        public PagePromotionController(IIntranetUserService<IIntranetUser> userService, IContextTypeProvider contextTypeProvider)
            : base(userService, contextTypeProvider)
        {
        }

        public ActionResult FeedItem(PagePromotion item, ActivityFeedOptions options)
        {
            AddEntityIdentityForContext(item.Id);

            var viewModel = GetItemViewModel(item, options);
            return PartialView(ItemViewPath, viewModel);
        }

        private PagePromotionExtendedItemViewModel GetItemViewModel(PagePromotion item, ActivityFeedOptions options)
        {
            var model = GetItemViewModel(item, options.Links);
            var extendedModel = model.Map<PagePromotionExtendedItemViewModel>();

            extendedModel.HeaderInfo = model.HeaderInfo.Map<ExtendedItemHeaderViewModel>();
            model.HeaderInfo.Type = item.Type;

            extendedModel.LikesInfo = item;
            extendedModel.LikesInfo.IsReadOnly = options.IsReadOnly;
            extendedModel.IsReadOnly = options.IsReadOnly;
            return extendedModel;
        }
    }
}