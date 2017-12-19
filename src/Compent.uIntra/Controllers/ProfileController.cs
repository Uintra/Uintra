using System;
using System.Linq;
using System.Web.Mvc;
using Compent.uIntra.Core.Users;
using uIntra.Core;
using uIntra.Core.ApplicationSettings;
using uIntra.Core.Extensions;
using uIntra.Core.Media;
using uIntra.Core.User;
using uIntra.Notification;
using uIntra.Tagging.UserTags;
using uIntra.Users;
using uIntra.Users.Web;
using Umbraco.Web;

namespace Compent.uIntra.Controllers
{
    public class ProfileController : ProfileControllerBase
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IIntranetUserContentProvider _intranetUserContentProvider;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly UserTagService _userTagService;

        protected override string ProfileEditViewPath { get; } = "~/Views/Profile/Edit.cshtml";

        public ProfileController(
            IMediaHelper mediaHelper,
            IApplicationSettings applicationSettings,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IMemberNotifiersSettingsService memberNotifiersSettingsService,
            UmbracoHelper umbracoHelper,
            IIntranetUserContentProvider intranetUserContentProvider, UserTagService userTagService)
            : base(mediaHelper, applicationSettings, intranetUserService, memberNotifiersSettingsService)
        {
            _umbracoHelper = umbracoHelper;
            _intranetUserContentProvider = intranetUserContentProvider;
            _userTagService = userTagService;
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
            _intranetUserService.Save(user);
            var tagIds = model.TagIdsData.ParseStringCollection(Guid.Parse);
            _userTagService.ReplaceRelations(user.Id, tagIds);
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