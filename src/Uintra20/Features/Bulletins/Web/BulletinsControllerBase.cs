using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Compent.Extensions;
using Compent.Shared.Extensions;
using Uintra20.Attributes;
using Uintra20.Core.Activity.Models.Headers;
using Uintra20.Core.Feed;
using Uintra20.Core.Member;
using Uintra20.Core.Member.Models;
using Uintra20.Features.Bulletins.Entities;
using Uintra20.Features.Bulletins.Models;
using Uintra20.Features.Links.Models;
using Uintra20.Features.Media;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.TypeProviders;
using Umbraco.Web.WebApi;

namespace Uintra20.Features.Bulletins.Web
{
    [ValidateModel]
    public abstract class BulletinsControllerBase : UmbracoApiController
    {
        //protected virtual string ItemViewPath { get; } = "~/App_Plugins/Bulletins/Item/ItemView.cshtml";
        //protected virtual string PreviewItemViewPath { get; } = "~/App_Plugins/Bulletins/PreviewItem/PreviewItem.cshtml";
        //protected virtual string CreationFormViewPath { get; } = "~/App_Plugins/Bulletins/Create/CreationForm.cshtml";
        //protected virtual string DetailsViewPath { get; } = "~/App_Plugins/Bulletins/Details/DetailsView.cshtml";
        //protected virtual string EditViewPath { get; } = "~/App_Plugins/Bulletins/Edit/EditView.cshtml";
        //protected virtual string CreationFormItemHeaderViewPath { get; } = "~/App_Plugins/Bulletins/Create/CreationFormItemHeader.cshtml";
        //protected virtual string ItemHeaderViewPath { get; } = "~/App_Plugins/Bulletins/Item/ItemHeader.cshtml";

        //protected virtual int DisplayedImagesCount { get; } = 3;

        private readonly IBulletinsService<Bulletin> _bulletinsService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IIntranetMemberService<IIntranetMember> _memberService;
        //private readonly IActivityTypeProvider _activityTypeProvider;

        //private const int ActivityTypeId = (int)IntranetActivityTypeEnum.Bulletins;
        //private const PermissionResourceTypeEnum ResourceType = PermissionResourceTypeEnum.Bulletins;

        protected BulletinsControllerBase(
            IBulletinsService<Bulletin> bulletinsService,
            IMediaHelper mediaHelper,
            IIntranetMemberService<IIntranetMember> memberService,
            IActivityTypeProvider activityTypeProvider)
        {
            _bulletinsService = bulletinsService;
            _mediaHelper = mediaHelper;
            _memberService = memberService;
            //_activityTypeProvider = activityTypeProvider;
        }

        //public virtual PartialViewResult Create(IActivityCreateLinks links)
        //{
        //    var result = GetCreateFormModel(links);
        //    return PartialView(CreationFormViewPath, result);
        //}

        [HttpGet]
        public virtual BulletinViewModel Details(Guid id, ActivityFeedOptions options)
        {
            var bulletin = _bulletinsService.Get(id);
            var model = GetViewModel(bulletin, options);
            
            return model;
        }

        //public virtual ActionResult Edit(Guid id, ActivityLinks links)
        //{
        //    var bulletin = _bulletinsService.Get(id);
        //    if (bulletin.IsHidden)
        //    {
        //        HttpContext.Current.Response.Redirect(links.Overview);
        //    }

        //    var model = GetEditViewModel(bulletin, links);
        //    return PartialView(EditViewPath, model);
        //}

        [HttpPost]
        public virtual BulletinCreationResultModel Create(BulletinCreateModel model)
        {
            var result = new BulletinCreationResultModel();

            if (!ModelState.IsValid)
            {
                ActionContext.Response = ActionContext.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest, ActionContext.ModelState);
            }

            var bulletin = MapToBulletin(model);
            var createdBulletinId = _bulletinsService.Create(bulletin);
            bulletin.Id = createdBulletinId;
            OnBulletinCreated(bulletin, model);

            result.Id = createdBulletinId;
            result.IsSuccess = true;

            return result;
        }

        [HttpPut]
        public virtual HttpResponseMessage Edit(BulletinEditModel editModel)
        {
            //if (!ModelState.IsValid)
            //{
            //    return RedirectToCurrentUmbracoPage(Request.QueryString);
            //}

            var bulletin = MapToBulletin(editModel);
            _bulletinsService.Save(bulletin);
            OnBulletinEdited(bulletin, editModel);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpDelete]
        public virtual object Delete(Guid id)
        {
            _bulletinsService.Delete(id);
            OnBulletinDeleted(id);

            return new { IsSuccess = true };
        }

        //public virtual ActionResult ItemHeader(object model)
        //{
        //    return PartialView(ItemHeaderViewPath, model);
        //}

        //protected virtual BulletinCreateModel GetCreateFormModel(IActivityCreateLinks links)
        //{
        //    var currentMember = _memberService.GetCurrentMember();
        //    var mediaSettings = _bulletinsService.GetMediaSettings();

        //    var result = new BulletinCreateModel
        //    {
        //        Title = currentMember.DisplayedName,
        //        ActivityType = _activityTypeProvider[ActivityTypeId],
        //        Dates = DateTime.UtcNow.ToDateFormat().ToEnumerable(),
        //        Creator = currentMember.Map<MemberViewModel>(),
        //        Links = links,
        //        AllowedMediaExtensions = mediaSettings.AllowedMediaExtensions,
        //        MediaRootId = mediaSettings.MediaRootId
        //    };
        //    return result;
        //}

        //protected virtual BulletinEditModel GetEditViewModel(BulletinBase bulletin, ActivityLinks links)
        //{
        //    var model = bulletin.Map<BulletinEditModel>();
        //    var mediaSettings = _bulletinsService.GetMediaSettings();

        //    model.MediaRootId = mediaSettings.MediaRootId;
        //    model.Links = links;

        //    model.CanDelete = _bulletinsService.CanDelete(bulletin);

        //    return model;
        //}

        protected virtual BulletinViewModel GetViewModel(BulletinBase bulletin, ActivityFeedOptions options)
        {
            var model = bulletin.Map<BulletinViewModel>();

            model.CanEdit = _bulletinsService.CanEdit(bulletin);
            model.Links = options.Links;
            model.IsReadOnly = options.IsReadOnly;

            model.HeaderInfo = bulletin.Map<IntranetActivityDetailsHeaderViewModel>();
            model.HeaderInfo.Dates = bulletin.PublishDate.ToDateTimeFormat().ToEnumerable();
            model.HeaderInfo.Owner = _memberService.Get(bulletin).Map<MemberViewModel>();
            model.HeaderInfo.Links = options.Links;

            return model;
        }

        //protected virtual BulletinItemViewModel GetItemViewModel(BulletinBase bulletin, IActivityLinks links)
        //{
        //    var model = bulletin.Map<BulletinItemViewModel>();
        //    var owner = _memberService.Get(bulletin);

        //    model.Links = links;
        //    model.MediaIds = bulletin.MediaIds;

        //    model.HeaderInfo = bulletin.Map<IntranetActivityItemHeaderViewModel>();
        //    model.HeaderInfo.Owner = owner.Map<MemberViewModel>();
        //    model.HeaderInfo.Title = owner.DisplayedName;
        //    model.HeaderInfo.Links = links;

        //    model.LightboxGalleryPreviewInfo = new LightboxGalleryPreviewModel
        //    {
        //        MediaIds = bulletin.MediaIds,
        //        DisplayedImagesCount = DisplayedImagesCount,
        //        ActivityId = bulletin.Id,
        //        ActivityType = bulletin.Type,
        //    };
        //    return model;
        //}

        protected virtual BulletinPreviewViewModel GetPreviewViewModel(BulletinBase bulletin, ActivityLinks links)
        {
            var owner = _memberService.Get(bulletin);
            return new BulletinPreviewViewModel
            {
                Id = bulletin.Id,
                Description = bulletin.Description,
                PublishDate = bulletin.PublishDate,
                Owner = owner.Map<MemberViewModel>(),
                ActivityType = bulletin.Type,
                Links = links
            };
        }

        protected virtual BulletinBase MapToBulletin(BulletinCreateModel model)
        {
            var bulletin = model.Map<BulletinBase>();
            bulletin.PublishDate = DateTime.UtcNow;
            bulletin.CreatorId = bulletin.OwnerId = _memberService.GetCurrentMemberId();

            if (model.NewMedia.HasValue())
            {
                bulletin.MediaIds = _mediaHelper.CreateMedia(model);
            }

            return bulletin;
        }

        protected virtual BulletinBase MapToBulletin(BulletinEditModel editModel)
        {
            var bulletin = _bulletinsService.Get(editModel.Id);
            bulletin = editModel.Map(bulletin);
            bulletin.MediaIds = bulletin.MediaIds.Concat(_mediaHelper.CreateMedia(editModel));

            return bulletin;
        }

        protected virtual void OnBulletinCreated(BulletinBase bulletin, BulletinCreateModel model)
        {

        }

        protected virtual void OnBulletinEdited(BulletinBase bulletin, BulletinEditModel model)
        {
        }

        protected virtual void OnBulletinDeleted(Guid id)
        {
        }
    }
}