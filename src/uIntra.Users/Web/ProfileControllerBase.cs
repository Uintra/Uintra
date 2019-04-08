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

        protected ProfileControllerBase(
            IMediaHelper mediaHelper,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IMemberNotifiersSettingsService memberNotifiersSettingsService,
            IProfileLinkProvider profileLinkProvider)
        {
            _mediaHelper = mediaHelper;
            _intranetMemberService = intranetMemberService;
            _memberNotifiersSettingsService = memberNotifiersSettingsService;
            _profileLinkProvider = profileLinkProvider;
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
            var member = MapToMemberDTO(model);
            _intranetMemberService.Update(member);

            return RedirectToCurrentUmbracoPage();
        }

        [HttpDelete]
        public virtual void DeletePhoto(int? photoId)
        {
            var user = _intranetMemberService.GetCurrentMember();

            if (photoId.HasValue)
                _mediaHelper.DeleteMedia(photoId.Value);

            var updateUser = user.Map<UpdateMemberDto>();
            updateUser.DeleteMedia = true;

            _intranetMemberService.Update(updateUser);
        }

        protected virtual ProfileViewModel MapToViewModel(IIntranetMember member)
        {
            var result = member.Map<ProfileViewModel>();
            return result;
        }

        protected virtual ProfileEditModel MapToEditModel(IIntranetMember member)
        {
            var result = member.Map<ProfileEditModel>();
            result.MemberNotifierSettings = _memberNotifiersSettingsService.GetForMember(member.Id);

            FillEditData(result);

            return result;
        }

        protected virtual UpdateMemberDto MapToMemberDTO(ProfileEditModel model)
        {
            var newMedias = _mediaHelper.CreateMedia(model).ToList();

            var updateMember = model.Map<UpdateMemberDto>();
            updateMember.NewMedia = newMedias.Count > 0 ? newMedias.First() : default(int?);

            return updateMember;
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
