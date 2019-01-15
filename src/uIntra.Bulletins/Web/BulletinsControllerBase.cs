using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Compent.Extensions;
using Uintra.Core;
using Uintra.Core.Activity;
using Uintra.Core.Attributes;
using Uintra.Core.Context;
using Uintra.Core.Controls.LightboxGallery;
using Uintra.Core.Extensions;
using Uintra.Core.Feed;
using Uintra.Core.Links;
using Uintra.Core.Media;
using Uintra.Core.TypeProviders;
using Uintra.Core.User;
using Uintra.Core.User.Permissions.Web;

namespace Uintra.Bulletins.Web
{
    [ActivityController(ActivityTypeId)]
    public abstract class BulletinsControllerBase : ContextController
    {
        protected virtual string ItemViewPath { get; } = "~/App_Plugins/Bulletins/Item/ItemView.cshtml";
        protected virtual string PreviewItemViewPath { get; } = "~/App_Plugins/Bulletins/PreviewItem/PreviewItem.cshtml";
        protected virtual string CreationFormViewPath { get; } = "~/App_Plugins/Bulletins/Create/CreationForm.cshtml";
        protected virtual string DetailsViewPath { get; } = "~/App_Plugins/Bulletins/Details/DetailsView.cshtml";
        protected virtual string EditViewPath { get; } = "~/App_Plugins/Bulletins/Edit/EditView.cshtml";
        protected virtual string CreationFormItemHeaderViewPath { get; } = "~/App_Plugins/Bulletins/Create/CreationFormItemHeader.cshtml";
        protected virtual string ItemHeaderViewPath { get; } = "~/App_Plugins/Bulletins/Item/ItemHeader.cshtml";

        protected virtual int DisplayedImagesCount { get; } = 3;

        private readonly IBulletinsService<BulletinBase> _bulletinsService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IIntranetUserService<IIntranetUser> _userService;
        private readonly IActivityTypeProvider _activityTypeProvider;

        private const int ActivityTypeId = (int)IntranetActivityTypeEnum.Bulletins;

        public override ContextType ControllerContextType { get; } = ContextType.Bulletins;

        protected BulletinsControllerBase(
            IBulletinsService<BulletinBase> bulletinsService,
            IMediaHelper mediaHelper,
            IIntranetUserService<IIntranetUser> userService,
            IActivityTypeProvider activityTypeProvider,
            IContextTypeProvider contextTypeProvider) :base(contextTypeProvider)
        {
            _bulletinsService = bulletinsService;
            _mediaHelper = mediaHelper;
            _userService = userService;
            _activityTypeProvider = activityTypeProvider;
        }

        public virtual PartialViewResult Create(IActivityCreateLinks links)
        {
            var result = GetCreateFormModel(links);
            return PartialView(CreationFormViewPath, result);
        }

        [NotFoundActivity]
        public virtual ActionResult Details(Guid id, ActivityFeedOptions options)
        {
            var bulletin = _bulletinsService.Get(id);
            var model = GetViewModel(bulletin, options);
            AddEntityIdentityForContext(id);
            return PartialView(DetailsViewPath, model);
        }

        [RestrictedAction(ActivityTypeId, IntranetActivityActionEnum.Edit)]
        public virtual ActionResult Edit(Guid id, ActivityLinks links)
        {
            var bulletin = _bulletinsService.Get(id);
            if (bulletin.IsHidden)
            {
                HttpContext.Response.Redirect(links.Overview);
            }

            var model = GetEditViewModel(bulletin, links);
            return PartialView(EditViewPath, model);
        }

        [HttpPost]
        [RestrictedAction(ActivityTypeId, IntranetActivityActionEnum.Create)]
        public virtual JsonResult Create(BulletinCreateModel model)
        {
            var result = new BulletinCreationResultModel();

            if (!ModelState.IsValid)
            {
                return Json(result);
            }

            var bulletin = MapToBulletin(model);
            var createdBulletinId = _bulletinsService.Create(bulletin);
            bulletin.Id = createdBulletinId;
            OnBulletinCreated(bulletin, model);

            result.Id = createdBulletinId;
            result.IsSuccess = true;

            return Json(result);
        }

        [HttpPost]
        [RestrictedAction(ActivityTypeId, IntranetActivityActionEnum.Edit)]
        public virtual ActionResult Edit(BulletinEditModel editModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToCurrentUmbracoPage(Request.QueryString);
            }

            var bulletin = MapToBulletin(editModel);
            _bulletinsService.Save(bulletin);
            OnBulletinEdited(bulletin, editModel);
            return Redirect(editModel.Links.Details);
        }

        [HttpPost]
        [RestrictedAction(ActivityTypeId, IntranetActivityActionEnum.Delete)]
        public virtual JsonResult Delete(Guid id)
        {
            _bulletinsService.Delete(id);
            OnBulletinDeleted(id);

            return Json(new { IsSuccess = true });
        }

        public virtual ActionResult ItemHeader(object model)
        {
            return PartialView(ItemHeaderViewPath, model);
        }

        protected virtual BulletinCreateModel GetCreateFormModel(IActivityCreateLinks links)
        {
            var currentUser = _userService.GetCurrentUser();
            var mediaSettings = _bulletinsService.GetMediaSettings();

            var result = new BulletinCreateModel
            {
                Title = currentUser.DisplayedName,
                ActivityType = _activityTypeProvider[ActivityTypeId],
                Dates = DateTime.UtcNow.ToDateFormat().ToEnumerable(),
                Creator = currentUser.Map<UserViewModel>(),
                Links = links,
                AllowedMediaExtensions = mediaSettings.AllowedMediaExtensions,
                MediaRootId = mediaSettings.MediaRootId
            };
            return result;
        }

        protected virtual BulletinEditModel GetEditViewModel(BulletinBase bulletin, ActivityLinks links)
        {
            var model = bulletin.Map<BulletinEditModel>();
            var mediaSettings = _bulletinsService.GetMediaSettings();
            FillMediaSettingsData(mediaSettings);

            model.MediaRootId = mediaSettings.MediaRootId;
            model.Links = links;

            return model;
        }

        protected virtual BulletinViewModel GetViewModel(BulletinBase bulletin, ActivityFeedOptions options)
        {
            var model = bulletin.Map<BulletinViewModel>();

            model.CanEdit = _bulletinsService.CanEdit(bulletin);
            model.Links = options.Links;
            model.IsReadOnly = options.IsReadOnly;

            model.HeaderInfo = bulletin.Map<IntranetActivityDetailsHeaderViewModel>();
            model.HeaderInfo.Dates = bulletin.PublishDate.ToDateTimeFormat().ToEnumerable();
            model.HeaderInfo.Owner = _userService.Get(bulletin).Map<UserViewModel>();
            model.HeaderInfo.Links = options.Links;

            return model;
        }

        protected virtual BulletinItemViewModel GetItemViewModel(BulletinBase bulletin, IActivityLinks links)
        {
            var model = bulletin.Map<BulletinItemViewModel>();
            var owner = _userService.Get(bulletin);

            model.Links = links;
            model.MediaIds = bulletin.MediaIds;

            model.HeaderInfo = bulletin.Map<IntranetActivityItemHeaderViewModel>();
            model.HeaderInfo.Owner = owner.Map<UserViewModel>();
            model.HeaderInfo.Title = owner.DisplayedName;
            model.HeaderInfo.Links = links;

            model.LightboxGalleryPreviewInfo = new LightboxGalleryPreviewModel
            {
                MediaIds = bulletin.MediaIds,
                DisplayedImagesCount = DisplayedImagesCount,
                ActivityId = bulletin.Id,
                ActivityType = bulletin.Type,
            };
            return model;
        }

        protected virtual BulletinPreviewViewModel GetPreviewViewModel(BulletinBase bulletin, ActivityLinks links)
        {
            var owner = _userService.Get(bulletin);
            return new BulletinPreviewViewModel
            {
                Id = bulletin.Id,
                Description = bulletin.Description,
                PublishDate = bulletin.PublishDate,
                Owner = owner.Map<UserViewModel>(),
                ActivityType = bulletin.Type,
                Links = links
            };
        }

        protected virtual BulletinBase MapToBulletin(BulletinCreateModel model)
        {
            var bulletin = model.Map<BulletinBase>();
            bulletin.PublishDate = DateTime.UtcNow;
            bulletin.CreatorId = bulletin.OwnerId = _userService.GetCurrentUserId();

            if (model.NewMedia.HasValue())
            {
                bulletin.MediaIds = _mediaHelper.CreateMedia(model);
            }

            return bulletin;
        }

        protected virtual BulletinBase MapToBulletin(BulletinEditModel editModel)
        {
            var bulletin = _bulletinsService.Get(editModel.Id);
            bulletin = Mapper.Map(editModel, bulletin);
            bulletin.MediaIds = bulletin.MediaIds.Concat(_mediaHelper.CreateMedia(editModel));

            return bulletin;
        }

        protected virtual void FillMediaSettingsData(MediaSettings settings)
        {
            ViewData["AllowedMediaExtensions"] = settings.AllowedMediaExtensions;
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