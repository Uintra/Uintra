﻿using System.Web.Mvc;
using Compent.uIntra.Core.Activity.Models;
using Compent.uIntra.Core.PagePromotion.Entities;
using Compent.uIntra.Core.PagePromotion.Models;
using uIntra.Core.Extensions;
using uIntra.Core.Feed;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Core.Web;
using Umbraco.Core;

namespace Compent.uIntra.Controllers
{
    public class PagePromotionController : PagePromotionControllerBase
    {
        protected override string ItemViewPath => "~/Views/PagePromotion/ItemView.cshtml";

        public PagePromotionController(IIntranetUserService<IIntranetUser> userService)
            : base(userService)
        {
        }

        public ActionResult FeedItem(PagePromotion item, ActivityFeedOptions options)
        {
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