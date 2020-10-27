using Compent.Shared.Extensions.Bcl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UBaseline.Core.RequestContext;
using Uintra.Core.Activity.Models.Headers;
using Uintra.Core.Controls.LightboxGallery;
using Uintra.Core.Member.Entities;
using Uintra.Core.Member.Services;
using Uintra.Core.UbaselineModels.RestrictedNode;
using Uintra.Features.Groups.Helpers;
using Uintra.Features.Groups.Services;
using Uintra.Features.Links;
using Uintra.Features.Media;
using Uintra.Features.Media.Helpers;
using Uintra.Features.Media.Strategies.Preset;
using Uintra.Features.News.Models;
using Uintra.Features.Permissions;
using Uintra.Features.Permissions.Interfaces;
using Uintra.Features.Permissions.Models;
using Uintra.Features.Tagging.UserTags.Services;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.News.Converters
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
        private readonly IUBaselineRequestContext _context;
        private readonly IGroupService _groupService;

        public UintraNewsEditPageViewModelConverter(
            IPermissionsService permissionsService,
            IFeedLinkService feedLinkService,
            INewsService<Entities.News> newsService,
            IIntranetMemberService<IntranetMember> memberService,
            IUserTagService userTagService,
            IUserTagProvider userTagProvider,
            ILightboxHelper lightboxHelper,
            IGroupHelper groupHelper,
            IErrorLinksService errorLinksService, 
            IUBaselineRequestContext context,
            IGroupService groupService)
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
            _context = context;
            _groupService = groupService;
        }

        public override ConverterResponseModel MapViewModel(UintraNewsEditPageModel node, UintraNewsEditPageViewModel viewModel)
        {
            var id = _context.ParseQueryString("id").TryParseGuid();

            if (!id.HasValue) return NotFoundResult();

            var news = _newsService.Get(id.Value);

            if (news == null || news.IsHidden) return NotFoundResult();

            if (news.GroupId.HasValue)
            {
                var group = _groupService.Get(news.GroupId.Value);
                if (group != null && group.IsHidden)
                    return NotFoundResult();
            }

            if (!_newsService.CanEdit(id.Value)) return ForbiddenResult();

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