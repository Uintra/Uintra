using System;
using System.Web.Mvc;
using AutoMapper;
using Compent.uIntra.Core.Bulletins;
using uIntra.Bulletins;
using uIntra.Bulletins.Web;
using uIntra.CentralFeed;
using uIntra.Core.Extentions;
using uIntra.Core.Media;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Navigation;

namespace Compent.uIntra.Controllers
{
    public class BulletinsController : BulletinsControllerBase
    {
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        protected override string DetailsViewPath => "~/Views/Bulletins/DetailsView.cshtml";
        protected override string ItemViewPath => "~/Views/Bulletins/ItemView.cshtml";
        private readonly IMyLinksService _myLinksService;

        public BulletinsController(
            IBulletinsService<Bulletin> bulletinsService,
            IMediaHelper mediaHelper,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IIntranetUserContentHelper intranetUserContentHelper,
            IActivityTypeProvider activityTypeProvider, IMyLinksService myLinksService)
            : base(bulletinsService, mediaHelper, intranetUserService, intranetUserContentHelper, activityTypeProvider)
        {
            _intranetUserService = intranetUserService;
            _myLinksService = myLinksService;
        }

        protected override BulletinViewModel GetViewModel(BulletinBase bulletin)
        {
            var extendedBullet = (Bulletin)bulletin;
            var extendedModel = base.GetViewModel(bulletin).Map<BulletinExtendedViewModel>();
            extendedModel = Mapper.Map(extendedBullet, extendedModel);
            return extendedModel;
        }

        public ActionResult CentralFeedItem(ICentralFeedItem item)
        {
            FillLinks();
            var activity = item as Bulletin;
            var extendedModel = GetItemViewModel(activity).Map<BulletinExtendedItemViewModel>();
            extendedModel.LikesInfo = activity;
            return PartialView(ItemViewPath, extendedModel);
        }

        public ActionResult PreviewItem(ICentralFeedItem item)
        {
            FillLinks();

            var activity = item as Bulletin;
            BulletinPreviewViewModel viewModel = GetPreviewViewModel(activity);
            return PartialView(PreviewItemViewPath, viewModel);
        }

        protected override void OnBulletinDeleted(Guid id)
        {
            _myLinksService.DeleteByActivityId(id);
        }
    }
}