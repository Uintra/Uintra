using System;
using System.Linq;
using System.Web.Mvc;
using Uintra.Core.ApplicationSettings;
using Uintra.Core.Extensions;
using Uintra.Core.Links;
using Uintra.Core.Media;
using Uintra.Core.User;
using Uintra.Core.User.DTO;
using Uintra.Notification;
using Umbraco.Web.Mvc;

namespace Uintra.Users.Web
{
    public abstract class ProfileControllerBase : SurfaceController
    {
        protected virtual string ProfileOverViewPath { get; } = "~/App_Plugins/Users/Profile/Overview.cshtml";
        protected virtual string ProfileEditViewPath { get; } = "~/App_Plugins/Users/Profile/Edit.cshtml";

        private readonly IMediaHelper _mediaHelper;
        private readonly IMemberNotifiersSettingsService _memberNotifiersSettingsService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IProfileLinkProvider _profileLinkProvider;

        protected ProfileControllerBase(
            IMediaHelper mediaHelper,
            IApplicationSettings applicationSettings,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IMemberNotifiersSettingsService memberNotifiersSettingsService,
            IProfileLinkProvider profileLinkProvider)
        {
            _mediaHelper = mediaHelper;
            _intranetUserService = intranetUserService;
            _memberNotifiersSettingsService = memberNotifiersSettingsService;
            _profileLinkProvider = profileLinkProvider;
        }

        public virtual ActionResult Overview(Guid? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }

            var user = _intranetUserService.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }

            var result = MapToViewModel(user);

            return View(ProfileOverViewPath, result);
        }

        [HttpGet]
        public virtual ActionResult Edit()
        {
            var user = _intranetUserService.GetCurrentUser();
            var result = MapToEditModel(user);

            return View(ProfileEditViewPath, result);
        }

        [HttpPost]
        public virtual ActionResult Edit(ProfileEditModel model)
        {
            var user = MapToUserDTO(model);
            _intranetUserService.Update(user);

            return RedirectToCurrentUmbracoPage();
        }

        [HttpDelete]
        public virtual void DeletePhoto(string photoPath)
        {
            var user = _intranetUserService.GetCurrentUser();
            _mediaHelper.DeleteMedia(photoPath);

            var updateUser = user.Map<UpdateUserDto>();
            updateUser.DeleteMedia = true;

            _intranetUserService.Update(updateUser);
        }

        protected virtual ProfileViewModel MapToViewModel(IIntranetUser user)
        {
            var result = user.Map<ProfileViewModel>();
            return result;
        }

        protected virtual ProfileEditModel MapToEditModel(IIntranetUser user)
        {
            var result = user.Map<ProfileEditModel>();
            result.MemberNotifierSettings = _memberNotifiersSettingsService.GetForMember(user.Id);

            FillEditData(result);

            return result;
        }

        protected virtual UpdateUserDto MapToUserDTO(ProfileEditModel model)
        {
            var newMedias = _mediaHelper.CreateMedia(model).ToList();

            var updateUser = model.Map<UpdateUserDto>();
            updateUser.NewMedia = newMedias.Count > 0 ? newMedias.First() : default(int?);

            return updateUser;
        }

        protected virtual void FillEditData(ProfileEditModel model)
        {
            var mediaSettings = GetMediaSettings();
            ViewData["AllowedMediaExtensions"] = mediaSettings.AllowedMediaExtensions;
            model.MediaRootId = mediaSettings.MediaRootId;
            model.ProfileUrl = _profileLinkProvider.GetProfileLink(model.Id);
        }

        public abstract MediaSettings GetMediaSettings();
    }
}
