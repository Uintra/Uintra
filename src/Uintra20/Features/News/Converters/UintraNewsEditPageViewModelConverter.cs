using Compent.Shared.Extensions.Bcl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uintra20.Core.Activity.Models.Headers;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Core.UbaselineModels.RestrictedNode;
using Uintra20.Features.Groups.Helpers;
using Uintra20.Features.Links;
using Uintra20.Features.Media;
using Uintra20.Features.Media.Helpers;
using Uintra20.Features.Media.Strategies.Preset;
using Uintra20.Features.News.Models;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Permissions.Models;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.News.Converters
{
    public class UintraNewsEditPageViewModelConverter :
        UintraRestrictedNodeViewModelConverter<UintraNewsEditPageModel, UintraNewsEditPageViewModel>
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IFeedLinkService _feedLinkService;
        private readonly INewsService<Entities.News> _newsService;
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly IUserTagService _userTagService;
        private readonly IUserTagProvider _userTagProvider;
        private readonly ILightboxHelper _lightboxHelper;
        private readonly IGroupHelper _groupHelper;

        public UintraNewsEditPageViewModelConverter(
            IPermissionsService permissionsService,
            IFeedLinkService feedLinkService,
            INewsService<Entities.News> newsService,
            IIntranetMemberService<IntranetMember> memberService,
            IUserTagService userTagService,
            IUserTagProvider userTagProvider,
            ILightboxHelper lightboxHelper,
            IGroupHelper groupHelper,
            IErrorLinksService errorLinksService)
            : base(errorLinksService)
        {
            _permissionsService = permissionsService;
            _feedLinkService = feedLinkService;
            _newsService = newsService;
            _memberService = memberService;
            _userTagService = userTagService;
            _userTagProvider = userTagProvider;
            _lightboxHelper = lightboxHelper;
            _groupHelper = groupHelper;
        }

        public override ConverterResponseModel MapViewModel(UintraNewsEditPageModel node, UintraNewsEditPageViewModel viewModel)
        {
            var requestId = HttpContext.Current.Request.GetRequestQueryValue("id");

            if (!Guid.TryParse(requestId, out var parsedId)) return NotFoundResult();

            var news = _newsService.Get(parsedId);

            if (news == null)
            {
                return NotFoundResult();
            }

            if (!_newsService.CanEdit(parsedId)) return ForbiddenResult();

            viewModel.Details = GetDetails(news);
            viewModel.Links = _feedLinkService.GetLinks(news.Id);
            viewModel.CanEditOwner = _permissionsService.Check(PermissionResourceTypeEnum.News, PermissionActionEnum.EditOwner);
            viewModel.AllowedMediaExtensions = _newsService.GetMediaSettings().AllowedMediaExtensions;
            viewModel.PinAllowed = _permissionsService.Check(PermissionResourceTypeEnum.News, PermissionActionEnum.CanPin);

            if (viewModel.CanEditOwner)
                viewModel.Members = GetUsersWithAccess(new PermissionSettingIdentity(PermissionActionEnum.Create, PermissionResourceTypeEnum.News));

            var requestGroupId = HttpContext.Current.Request["groupId"];

            if (Guid.TryParse(requestGroupId, out var groupId) && news.GroupId == groupId)
            {
                viewModel.GroupHeader = _groupHelper.GetHeader(groupId);
            }

            return OkResult();
        }

        //TODO Refactor this code. Method is duplicated in ActivityCreatePanelConverter
        private IEnumerable<IntranetMember> GetUsersWithAccess(PermissionSettingIdentity permissionSettingIdentity) =>
            _memberService
                .GetAll()
                .Where(member => _permissionsService.Check(member, permissionSettingIdentity))
                .OrderBy(user => user.DisplayedName)
                .ToArray();

        private NewsViewModel GetDetails(Entities.News news)
        {
            var details = news.Map<NewsViewModel>();

            details.Media = MediaHelper.GetMediaUrls(news.MediaIds);
            details.CanEdit = _newsService.CanEdit(news);
            details.Links = _feedLinkService.GetLinks(news.Id);
            details.IsReadOnly = false;
            details.HeaderInfo = news.Map<IntranetActivityDetailsHeaderViewModel>();
            details.HeaderInfo.Dates = news.PublishDate.ToDateTimeFormat().ToEnumerable();
            details.HeaderInfo.Owner = _memberService.Get(news).ToViewModel();
            details.Tags = _userTagService.Get(news.Id);
            details.AvailableTags = _userTagProvider.GetAll();
            details.LightboxPreviewModel = _lightboxHelper.GetGalleryPreviewModel(news.MediaIds, PresetStrategies.ForActivityDetails);

            return details;
        }
    }
}