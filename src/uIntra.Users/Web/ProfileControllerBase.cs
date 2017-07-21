using System;
using System.Linq;
using System.Web.Mvc;
using uIntra.Core.ApplicationSettings;
using uIntra.Core.Extentions;
using uIntra.Core.Media;
using uIntra.Core.User;
using uIntra.Notification;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace uIntra.Users.Web
{
    public abstract class ProfileControllerBase : SurfaceController
    {
        protected virtual string ProfileOverViewPath { get; } = "~/App_Plugins/Users/Profile/Overview.cshtml";
        protected virtual string ProfileEditViewPath { get; } = "~/App_Plugins/Users/Profile/Edit.cshtml";

        private readonly IMemberService _memberService;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IMediaHelper _mediaHelper;
        private readonly IMemberNotifiersSettingsService _memberNotifiersSettingsService;
        private readonly IIntranetUserService<IntranetUser> _intranetUserService;

        protected ProfileControllerBase(IMemberService memberService,
            UmbracoHelper umbracoHelper,
            IMediaHelper mediaHelper,
            IApplicationSettings applicationSettings,
            IIntranetUserService<IntranetUser> intranetUserService,
            IMemberNotifiersSettingsService memberNotifiersSettingsService)
        {
            _memberService = memberService;
            _umbracoHelper = umbracoHelper;
            _mediaHelper = mediaHelper;
            _intranetUserService = intranetUserService;
            _memberNotifiersSettingsService = memberNotifiersSettingsService;
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

            var result = user.Map<ProfileViewModel>();

            return View(ProfileOverViewPath, result);
        }

        [HttpGet]
        public virtual ActionResult Edit()
        {
            var member = _memberService.GetById(Members.GetCurrentMemberId());
            var result = MapToEditModel(member);

            return View(ProfileEditViewPath, result);
        }

        [HttpPost]
        public virtual ActionResult Edit(ProfileEditModel model)
        {
            var currentMember = _memberService.GetById(Members.GetCurrentMemberId());
            UpdateMember(currentMember, model);
            UpdateMemberPhoto(currentMember, model);

            _memberService.Save(currentMember);

            return RedirectToCurrentUmbracoPage();
        }

        [HttpDelete]
        public virtual void DeletePhoto()
        {
            var currentMember = _memberService.GetById(Members.GetCurrentMemberId());
            _mediaHelper.DeleteMedia(currentMember.GetValue<int>(ProfileConstants.Photo));
            currentMember.SetValue(ProfileConstants.Photo, null);
            _memberService.Save(currentMember);
        }

        protected virtual ProfileEditModel MapToEditModel(IMember member)
        {
            var result = new ProfileEditModel
            {
                FirstName = member.GetValueOrDefault<string>(ProfileConstants.FirstName),
                LastName = member.GetValueOrDefault<string>(ProfileConstants.LastName),
                Photo = GetMemberPhoto(member),
                MemberNotifierSettings = _memberNotifiersSettingsService.GetForMember(member.Key)
            };

            FillEditData(result);

            return result;
        }

        protected virtual void FillEditData(ProfileEditModel model)
        {
            var mediaSettings = GetMediaSettings();
            ViewData["AllowedMediaExtentions"] = mediaSettings.AllowedMediaExtentions;
            model.MediaRootId = mediaSettings.MediaRootId;
        }

        protected virtual void UpdateMember(IMember member, ProfileEditModel profileEditModel)
        {
            member.SetValue(ProfileConstants.FirstName, profileEditModel.FirstName);
            member.SetValue(ProfileConstants.LastName, profileEditModel.LastName);
        }

        protected virtual void UpdateMemberPhoto(IMember member, ProfileEditModel profileEditModel)
        {
            var photo = _mediaHelper.CreateMedia(profileEditModel);

            if (photo.Any())
            {
                // TODO: Some logic for removal of previous photo?
                /*var userPhotoId = member.GetValueOrDefault<int?>(ProfileConstants.Photo);
                if (userPhotoId.HasValue)
                {
                    _mediaHelper.DeleteMedia(userPhotoId.Value);
                }*/

                member.SetValue(ProfileConstants.Photo, photo.First());
            }
        }

        protected virtual string GetMemberPhoto(IMember member)
        {
            var userPhotoId = member.GetValueOrDefault<int?>(ProfileConstants.Photo);
            if (userPhotoId.HasValue)
            {
                var photo = _umbracoHelper.TypedMedia(userPhotoId.Value);
                if (photo != null)
                {
                    return photo.Url;
                }
            }

            return null;
        }

        public abstract MediaSettings GetMediaSettings();
    }
}
