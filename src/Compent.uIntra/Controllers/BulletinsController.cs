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
using uIntra.Groups;
using uIntra.Navigation;
using System.Linq;
using Compent.uIntra.Core.Extentions;
using uIntra.CentralFeed;
using uIntra.Groups.Extentions;

namespace Compent.uIntra.Controllers
{
    public class BulletinsController : BulletinsControllerBase
    {
        protected override string DetailsViewPath => "~/Views/Bulletins/DetailsView.cshtml";
        protected override string ItemViewPath => "~/Views/Bulletins/ItemView.cshtml";
        private readonly IBulletinsService<Bulletin> _bulletinsService;
        private readonly IMyLinksService _myLinksService;
        private readonly IGroupActivityService _groupActivityService;

        public BulletinsController(
            IBulletinsService<Bulletin> bulletinsService,
            IMediaHelper mediaHelper,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IActivityTypeProvider activityTypeProvider, 
            IMyLinksService myLinksService,
            IGroupActivityService groupActivityService)
            : base(bulletinsService, mediaHelper, intranetUserService, activityTypeProvider)
        {
            _bulletinsService = bulletinsService;
            _myLinksService = myLinksService;
            _groupActivityService = groupActivityService;
        }

        protected override BulletinViewModel GetViewModel(BulletinBase bulletin, ActivityLinks links)
        {
            var extendedBullet = (Bulletin)bulletin;
            var extendedModel = base.GetViewModel(bulletin, links).Map<BulletinExtendedViewModel>();
            extendedModel = Mapper.Map(extendedBullet, extendedModel);
            return extendedModel;
        }

        public ActionResult CentralFeedItem(Bulletin item, FeedOptions options)
        {
            var activity = item;
            var extendedModel = GetItemViewModel(activity, options.Links).Map<BulletinExtendedItemViewModel>();
            extendedModel.LikesInfo = activity;
            extendedModel.LikesInfo.IsReadOnly = options.IsReadOnly;
            extendedModel.IsReadOnly = options.IsReadOnly;
            return PartialView(ItemViewPath, extendedModel);
        }

        public ActionResult PreviewItem(Bulletin item, ActivityLinks links)
        {
            var viewModel = GetPreviewViewModel(item, links);
            return PartialView(PreviewItemViewPath, viewModel);
        }

        protected override void OnBulletinDeleted(Guid id)
        {
            _myLinksService.DeleteByActivityId(id);
        }

        protected override void OnBulletinCreated(BulletinBase bulletin, BulletinCreateModel model)
        {
            base.OnBulletinCreated(bulletin, model);

            var groupId = Request.QueryString.GetGroupId();
            if (groupId.HasValue)
            {
                _groupActivityService.AddRelation(groupId.Value, bulletin.Id);
                var extendedBulletin = _bulletinsService.Get(bulletin.Id);
                extendedBulletin.GroupId = groupId;
            }
        }
    }
}