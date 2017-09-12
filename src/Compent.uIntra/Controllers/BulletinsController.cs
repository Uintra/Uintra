using System;
using System.Web.Mvc;
using AutoMapper;
using Compent.uIntra.Core.Bulletins;
using uIntra.Bulletins;
using uIntra.Bulletins.Web;
using uIntra.Core.Extentions;
using uIntra.Core.Links;
using uIntra.Core.Media;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Navigation;

namespace Compent.uIntra.Controllers
{
    public class BulletinsController : BulletinsControllerBase
    {
        protected override string DetailsViewPath => "~/Views/Bulletins/DetailsView.cshtml";
        protected override string ItemViewPath => "~/Views/Bulletins/ItemView.cshtml";
        private readonly IMyLinksService _myLinksService;

        public BulletinsController(
            IBulletinsService<Bulletin> bulletinsService,
            IMediaHelper mediaHelper,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IIntranetUserContentHelper intranetUserContentHelper,
            IActivityTypeProvider activityTypeProvider, 
            IMyLinksService myLinksService)
            : base(bulletinsService, mediaHelper, intranetUserService, intranetUserContentHelper, activityTypeProvider)
        {
            _myLinksService = myLinksService;
        }

        protected override BulletinViewModel GetViewModel(BulletinBase bulletin, ActivityLinks links)
        {
            var extendedBullet = (Bulletin)bulletin;
            var extendedModel = base.GetViewModel(bulletin, links).Map<BulletinExtendedViewModel>();
            extendedModel = Mapper.Map(extendedBullet, extendedModel);
            return extendedModel;
        }

        public ActionResult CentralFeedItem(Bulletin item, ActivityLinks links)
        {
            var activity = item;
            var extendedModel = GetItemViewModel(activity, links).Map<BulletinExtendedItemViewModel>();
            extendedModel.LikesInfo = activity;
            return PartialView(ItemViewPath, extendedModel);
        }

        public ActionResult PreviewItem(Bulletin item)
        {
            FillLinks();

            var activity = item;
            BulletinPreviewViewModel viewModel = GetPreviewViewModel(activity);
            return PartialView(PreviewItemViewPath, viewModel);
        }

        protected override void OnBulletinDeleted(Guid id)
        {
            _myLinksService.DeleteByActivityId(id);
        }
    }
}