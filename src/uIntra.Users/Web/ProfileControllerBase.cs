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
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;

namespace Uintra.Users.Web
{
    public abstract class ProfileControllerBase : SurfaceController
    {
        protected virtual string ProfileOverViewPath { get; } = "~/App_Plugins/Users/Profile/Overview.cshtml";
        protected virtual string ProfileEditViewPath { get; } = "~/App_Plugins/Users/Profile/Edit.cshtml";

        private readonly IMediaHelper _mediaHelper;
        private readonly IMemberNotifiersSettingsService _memberNotifiersSettingsService;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly IProfileLinkProvider _profileLinkProvider;
        private readonly IMemberService _memberService;

        protected ProfileControllerBase(
            IMediaHelper mediaHelper,
            IApplicationSettings applicationSettings,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IMemberNotifiersSettingsService memberNotifiersSettingsService,
            IProfileLinkProvider profileLinkProvider,
            IMemberService memberService)
        {
            _mediaHelper = mediaHelper;
            _intranetMemberService = intranetMemberService;
            _memberNotifiersSettingsService = memberNotifiersSettingsService;
            _profileLinkProvider = profileLinkProvider;
            _memberService = memberService;
        }

        public virtual ActionResult Overview(Guid? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }

            var user = _intranetMemberService.Get(id.Value);
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
            var user = _intranetMemberService.GetCurrentMember();
            var result = MapToEditModel(user);

            return View(ProfileEditViewPath, result);
        }

        [HttpPost]
        public virtual ActionResult Edit(ProfileEditModel model)
        {
            var user = MapToUserDTO(model);
            _intranetMemberService.Update(user);

            return RedirectToCurrentUmbracoPage();
        }

        [HttpDelete]
        public virtual void DeletePhoto(Guid memberId)
        {
            var user = _intranetMemberService.GetCurrentMember();
            //todo store photoId in user profile instead of full path and fill it on view models
            var member = _memberService.GetByKey(memberId);

            var userPhotoId = member.GetValueOrDefault<int?>(ProfileConstants.Photo) ?? member.GetMemberImageId(ProfileConstants.Photo);

            if (userPhotoId.HasValue)
            {
                _mediaHelper.DeleteMedia(userPhotoId.Value);
            }

            var updateUser = user.Map<UpdateMemberDto>();
            updateUser.DeleteMedia = true;

            _intranetMemberService.Update(updateUser);
        }

        protected virtual ProfileViewModel MapToViewModel(IIntranetMember user)
        {
            var result = user.Map<ProfileViewModel>();
            return result;
        }

        protected virtual ProfileEditModel MapToEditModel(IIntranetMember user)
        {
            var result = user.Map<ProfileEditModel>();
            result.MemberNotifierSettings = _memberNotifiersSettingsService.GetForMember(user.Id);

            FillEditData(result);

            return result;
        }

        protected virtual UpdateMemberDto MapToUserDTO(ProfileEditModel model)
        {
            var newMedias = _mediaHelper.CreateMedia(model).ToList();

            var updateUser = model.Map<UpdateMemberDto>();
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
