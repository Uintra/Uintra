using System;
using System.Web.Mvc;
using AutoMapper;
using Compent.uIntra.Core.Bulletins;
using uIntra.Bulletins;
using uIntra.Bulletins.Web;
using uIntra.Core.Extensions;
using uIntra.Core.Links;
using uIntra.Core.Media;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Groups;
using uIntra.Navigation;
using Compent.uIntra.Core.Activity.Models;
using Compent.uIntra.Core.Feed;
using uIntra.Core.Feed;
using uIntra.Groups.Extensions;
using uIntra.Tagging.UserTags;

namespace Compent.uIntra.Controllers
{
    public class BulletinsController : BulletinsControllerBase
    {
        protected override string DetailsViewPath => "~/Views/Bulletins/DetailsView.cshtml";
        protected override string ItemViewPath => "~/Views/Bulletins/ItemView.cshtml";
        protected override string EditViewPath => "~/Views/Bulletins/EditView.cshtml";
        protected override string CreationFormViewPath { get; } = "~/Views/Bulletins/CreationForm.cshtml";
        protected override string ItemHeaderViewPath { get; } = "~/Views/Bulletins/ItemHeader.cshtml";

        private readonly IBulletinsService<Bulletin> _bulletinsService;
        private readonly IMyLinksService _myLinksService;
        private readonly IGroupActivityService _groupActivityService;
        private readonly UserTagService _userTagService;

        public BulletinsController(
            IBulletinsService<Bulletin> bulletinsService,
            IMediaHelper mediaHelper,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IActivityTypeProvider activityTypeProvider, 
            IMyLinksService myLinksService,
            IGroupActivityService groupActivityService,
            UserTagService userTagService)
            : base(bulletinsService, mediaHelper, intranetUserService, activityTypeProvider)
        {
            _bulletinsService = bulletinsService;
            _myLinksService = myLinksService;
            _groupActivityService = groupActivityService;
            _userTagService = userTagService;
        }

        [HttpPost]
        public JsonResult CreateExtended(BulletinExtendedCreateModel model)
        {
            return Create(model);
        }

        protected override BulletinCreateModel GetCreateFormModel(IActivityCreateLinks links)
        {
            var model = base.GetCreateFormModel(links);
            var extendedModel = model.Map<BulletinExtendedCreateModel>();
            return extendedModel;
        }

        [HttpPost]
        public ActionResult EditExtended(BulletinExtendedEditModel model)
        {
            return Edit(model);
        }

        protected override BulletinEditModel GetEditViewModel(BulletinBase bulletin, ActivityLinks links)
        {
            var baseModel = base.GetEditViewModel(bulletin, links);
            var extendedModel = baseModel.Map<BulletinExtendedEditModel>();
            return extendedModel;
        }

        protected override void OnBulletinEdited(BulletinBase bulletin, BulletinEditModel model)
        {
            if (model is BulletinExtendedEditModel extendedModel)
            {
                ReplaceTags(bulletin.Id, extendedModel.TagIdsData);
            }
        }

        protected override BulletinViewModel GetViewModel(BulletinBase bulletin, ActivityFeedOptions options)
        {
            var extendedBullet = (Bulletin)bulletin;
            var extendedModel = base.GetViewModel(bulletin, options).Map<BulletinExtendedViewModel>();
            extendedModel = Mapper.Map(extendedBullet, extendedModel);
            return extendedModel;
        }

        public ActionResult FeedItem(Bulletin item, ActivityFeedOptionsWithGroups options)
        {
            BulletinExtendedItemViewModel extendedModel = GetItemViewModel(item, options);
            return PartialView(ItemViewPath, extendedModel);
        }

        private BulletinExtendedItemViewModel GetItemViewModel(Bulletin item, ActivityFeedOptionsWithGroups options)
        {
            var model = GetItemViewModel(item, options.Links);
            var extendedModel = model.Map<BulletinExtendedItemViewModel>();

            extendedModel.HeaderInfo = model.HeaderInfo.Map<ExtendedItemHeaderViewModel>();
            extendedModel.HeaderInfo.GroupInfo = options.GroupInfo;

            extendedModel.LikesInfo = item;
            extendedModel.LikesInfo.IsReadOnly = options.IsReadOnly;
            extendedModel.IsReadOnly = options.IsReadOnly;
            return extendedModel;
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

            if (model is BulletinExtendedCreateModel extendedModel)
            {
                ReplaceTags(bulletin.Id, extendedModel.TagIdsData);
            }
        }

        private void ReplaceTags(Guid entityId, string collectionString)
        {
            var tagIds = collectionString.ParseStringCollection(Guid.Parse);
            _userTagService.ReplaceRelations(entityId, tagIds);
        }
    }
}