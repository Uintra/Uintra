using System;
using System.Linq;
using System.Web.Mvc;
using Compent.Uintra.Core.Users;
using Uintra.Core;
using Uintra.Core.ApplicationSettings;
using Uintra.Core.Extensions;
using Uintra.Core.Links;
using Uintra.Core.Media;
using Uintra.Core.User;
using Uintra.Notification;
using Uintra.Tagging.UserTags;
using Uintra.Users;
using Uintra.Users.Web;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Compent.Uintra.Controllers
{
    public class ProfileController : ProfileControllerBase
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IIntranetUserContentProvider _intranetUserContentProvider;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly UserTagService _userTagService;
        private readonly IMemberService _memberService;

        protected override string ProfileEditViewPath { get; } = "~/Views/Profile/Edit.cshtml";

        public ProfileController(
            IMediaHelper mediaHelper,
            IApplicationSettings applicationSettings,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IMemberNotifiersSettingsService memberNotifiersSettingsService,
            UmbracoHelper umbracoHelper,
            IIntranetUserContentProvider intranetUserContentProvider,
            UserTagService userTagService,
            IProfileLinkProvider profileLinkProvider,
            IMemberService memberService)
            : base(mediaHelper, applicationSettings, intranetUserService, memberNotifiersSettingsService, profileLinkProvider, memberService)
        {
            _umbracoHelper = umbracoHelper;
            _intranetUserContentProvider = intranetUserContentProvider;
            _userTagService = userTagService;
            _memberService = memberService;
            _intranetUserService = intranetUserService;
        }

        public ActionResult EditPage()
        {
            var profilePage = _intranetUserContentProvider.GetEditPage();
            return Redirect(profilePage.Url);
        }

        public override MediaSettings GetMediaSettings()
        {
            var media = _umbracoHelper.TypedMediaAtRoot();
            var id =
                media.Single(m => m.GetPropertyValue<MediaFolderTypeEnum>(FolderConstants.FolderTypePropertyTypeAlias) ==
                                  MediaFolderTypeEnum.MembersContent).Id;

            return new MediaSettings
            {
                MediaRootId = id
            };
        }

        [NonAction]
        public override ActionResult Edit(ProfileEditModel model) => base.Edit(model);

        [HttpPost]
        public ActionResult Edit(ExtendedProfileEditModel model)
        {
            var user = MapToUserDTO(model);
            var tagIds = model.TagIdsData.ParseStringCollection(Guid.Parse);
            _userTagService.Replace(user.Id, tagIds);
            _intranetUserService.Update(user);
            return RedirectToCurrentUmbracoPage();
        }

        [HttpGet]
        public override ActionResult Edit()
        {
            var user = _intranetUserService.GetCurrentUser();
            var result = MapToEditModel(user);

            return PartialView(ProfileEditViewPath, result);
        }

        private new ExtendedProfileEditModel MapToEditModel(IIntranetUser user)
        {
            var baseModel = base.MapToEditModel(user);
            var result = baseModel.Map<ExtendedProfileEditModel>();
            return result;
        }
    }
}