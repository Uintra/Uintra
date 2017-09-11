using System;
using System.Web.Mvc;
using AutoMapper;
using Compent.uIntra.Core.Bulletins;
using uIntra.Bulletins;
using uIntra.Bulletins.Web;
using uIntra.CentralFeed;
using uIntra.Core.Extentions;
using uIntra.Core.Links;
using uIntra.Core.Media;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Groups;
using uIntra.Navigation;

namespace Compent.uIntra.Controllers
{
    public class BulletinsController : BulletinsControllerBase
    {
        protected override string DetailsViewPath => "~/Views/Bulletins/DetailsView.cshtml";
        protected override string ItemViewPath => "~/Views/Bulletins/ItemView.cshtml";
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
            _myLinksService = myLinksService;
            _groupService = groupService;
        }

        protected override BulletinViewModel GetViewModel(BulletinBase bulletin)
        {
            var extendedBullet = (Bulletin)bulletin;
            var extendedModel = base.GetViewModel(bulletin).Map<BulletinExtendedViewModel>();
            extendedModel = Mapper.Map(extendedBullet, extendedModel);
            return extendedModel;
        }

        [NonAction]
        public override JsonResult Create(BulletinCreateModel model)
        {
            return base.Create(model);
        }

        public virtual JsonResult Create(BulletinExtendedCreateModel model)
        {
            if (model.GroupId.HasValue)
            {
                AddActivityToGroup(model.GroupId.Value);
            }
            return base.Create(model);
        }

        private void AddActivityToGroup(Guid groupId)
        {
            
        }

        public ActionResult CentralFeedItem(Bulletin item, ActivityLinks links)
        {
            FillLinks();
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