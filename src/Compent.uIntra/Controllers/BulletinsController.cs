﻿using System.Web.Mvc;
using AutoMapper;
using Compent.uIntra.Core.Bulletins;
using uIntra.Bulletins;
using uIntra.Bulletins.Web;
using uIntra.CentralFeed;
using uIntra.Core.Extentions;
using uIntra.Core.Media;
using uIntra.Core.User;

namespace Compent.uIntra.Controllers
{
    public class BulletinsController : BulletinsControllerBase
    {
        protected override string DetailsViewPath => "~/Views/Bulletins/DetailsView.cshtml";
        protected override string ItemViewPath => "~/Views/Bulletins/ItemView.cshtml";

        public BulletinsController(
            IBulletinsService<Bulletin> bulletinsService,
            IMediaHelper mediaHelper,
            IIntranetUserService<IIntranetUser> intranetUserService)
            : base(bulletinsService, mediaHelper, intranetUserService)
        {
        }

        protected override BulletinViewModel GetViewModel(BulletinBase bulletin)
        {
            var extendedBullet = (Bulletin) bulletin;
            var extendedModel=base.GetViewModel(bulletin).Map<BulletinExtendedViewModel>();
            extendedModel = Mapper.Map(extendedBullet, extendedModel);
            return extendedModel;
        }

        public ActionResult CentralFeedItem(ICentralFeedItem item)
        {
            var activity = item as Bulletin;
            var extendedModel = GetItemViewModel(activity).Map<BulletinExtendedItemViewModel>();
            extendedModel.LikesInfo = activity;
            return PartialView(ItemViewPath, extendedModel);
        }

    }
}