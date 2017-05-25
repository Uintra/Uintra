using System.Linq;
using System.Web.Mvc;
using uIntra.Core.ApplicationSettings;
using uIntra.Core.Extentions;
using uIntra.Core.Media;
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
        private readonly IApplicationSettings _applicationSettings;

        protected ProfileControllerBase(IMemberService memberService,
            UmbracoHelper umbracoHelper,
            IMediaHelper mediaHelper,
            IApplicationSettings applicationSettings)
        {
            _memberService = memberService;
            _umbracoHelper = umbracoHelper;
            _mediaHelper = mediaHelper;
            _applicationSettings = applicationSettings;
        }

        public virtual ActionResult Overview()
        {
            var member = _memberService.GetById(Members.GetCurrentMemberId());
            var result = MapToViewModel(member);

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
        public virtual ActionResult Edit(ProfileEditModelBase model)
        {
            var currentMember = _memberService.GetById(Members.GetCurrentMemberId());
            UpdateMember(currentMember, model);
            UpdateMemberPhoto(currentMember, model);

            _memberService.Save(currentMember);

            return Overview();
        }
        
        protected virtual ProfileViewModelBase MapToViewModel(IMember member)
        {
            var result = new ProfileViewModelBase
            {
                FirstName = member.GetValueOrDefault<string>(ProfileConstants.FirstName),
                LastName = member.GetValueOrDefault<string>(ProfileConstants.LastName),
                Email = member.Email,
                Photo = GetMemberPhoto(member)
            };

            return result;
        }

        protected virtual ProfileEditModelBase MapToEditModel(IMember member)
        {
            var result = new ProfileEditModelBase
            {
                FirstName = member.GetValueOrDefault<string>(ProfileConstants.FirstName),
                LastName = member.GetValueOrDefault<string>(ProfileConstants.LastName),
                Photo = GetMemberPhoto(member)
            };

            FillEditData(result);

            return result;
        }

        protected virtual void FillEditData(ProfileEditModelBase model)
        {
            var mediaSettings = GetMediaSettings();
            ViewData["AllowedMediaExtentions"] = mediaSettings.AllowedMediaExtentions;
            model.MediaRootId = mediaSettings.MediaRootId;
        }

        protected virtual void UpdateMember(IMember member, ProfileEditModelBase profileEditModel)
        {
            member.SetValue(ProfileConstants.FirstName, profileEditModel.FirstName);
            member.SetValue(ProfileConstants.LastName, profileEditModel.LastName);
        }

        protected virtual void UpdateMemberPhoto(IMember member, ProfileEditModelBase profileEditModel)
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

            return _applicationSettings.DefaultAvatarPath;
        }

        public abstract MediaSettings GetMediaSettings();
    }
}
