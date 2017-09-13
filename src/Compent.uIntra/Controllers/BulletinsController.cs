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

namespace Compent.uIntra.Controllers
{
    public class BulletinsController : BulletinsControllerBase
    {
        protected override string DetailsViewPath => "~/Views/Bulletins/DetailsView.cshtml";
        protected override string ItemViewPath => "~/Views/Bulletins/ItemView.cshtml";
        private readonly IBulletinsService<Bulletin> _bulletinsService;
        private readonly IMyLinksService _myLinksService;
        private readonly IGroupService _groupService;

        public BulletinsController(
            IBulletinsService<Bulletin> bulletinsService,
            IMediaHelper mediaHelper,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IIntranetUserContentHelper intranetUserContentHelper,
            IActivityTypeProvider activityTypeProvider, 
            IMyLinksService myLinksService,
            IGroupService groupService)
            : base(bulletinsService, mediaHelper, intranetUserService, intranetUserContentHelper, activityTypeProvider)
        {
            _bulletinsService = bulletinsService;
            _myLinksService = myLinksService;
            _groupService = groupService;
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

            var groupId = _groupService.GetGroupIdFromQuery(Request.QueryString.ToString());
            if (groupId.HasValue)
            {
                _groupService.AddGroupActivityRelation(groupId.Value, bulletin.Id);
                var extendedBulletin = _bulletinsService.Get(bulletin.Id);
                extendedBulletin.GroupIds = extendedBulletin.GroupIds.Concat(groupId.Value.ToEnumerableOfOne());
            }
        }
    }
}