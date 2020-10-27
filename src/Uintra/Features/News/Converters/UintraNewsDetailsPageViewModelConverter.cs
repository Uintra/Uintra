using Compent.Extensions;
using System;
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
using Uintra.Features.Media.Helpers;
using Uintra.Features.Media.Strategies.Preset;
using Uintra.Features.News.Models;
using Uintra.Features.Permissions;
using Uintra.Features.Permissions.Interfaces;
using Uintra.Features.Tagging.UserTags.Services;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.News.Converters
{
    public class UintraNewsDetailsPageViewModelConverter :
        UintraRestrictedNodeViewModelConverter<UintraNewsDetailsPageModel, UintraNewsDetailsPageViewModel>
    {
        private readonly IUserTagService _userTagService;
        private readonly IFeedLinkService _feedLinkService;
        private readonly INewsService<Entities.News> _newsService;
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly ILightboxHelper _lightBoxHelper;
        private readonly IPermissionsService _permissionsService;
        private readonly IGroupHelper _groupHelper;
        private readonly IUBaselineRequestContext _context;
        private readonly IGroupService _groupService;

        public UintraNewsDetailsPageViewModelConverter(
            IUserTagService userTagService,
            IFeedLinkService feedLinkService,
            INewsService<Entities.News> newsService,
            IIntranetMemberService<IntranetMember> memberService,
            ILightboxHelper lightBoxHelper,
            IPermissionsService permissionsService,
            IGroupHelper groupHelper,
            IErrorLinksService errorLinksService, 
            IUBaselineRequestContext context,
            IGroupService groupService)
            : base(errorLinksService)
        {
            _userTagService = userTagService;
            _feedLinkService = feedLinkService;
            _newsService = newsService;
            _memberService = memberService;
            _lightBoxHelper = lightBoxHelper;
            _permissionsService = permissionsService;
            _groupHelper = groupHelper;
            _context = context;
            _groupService = groupService;
        }

        public override ConverterResponseModel MapViewModel(UintraNewsDetailsPageModel node, UintraNewsDetailsPageViewModel viewModel)
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

            if (!_permissionsService.Check(PermissionResourceTypeEnum.News, PermissionActionEnum.View))
            {
                return ForbiddenResult();
            }

            var member = _memberService.GetCurrentMember();

            viewModel.Details = GetDetails(news);
            viewModel.Tags = _userTagService.Get(id.Value);
            viewModel.CanEdit = _newsService.CanEdit(id.Value);
            viewModel.IsGroupMember = !news.GroupId.HasValue || member.GroupIds.Contains(news.GroupId.Value);

            var groupIdStr = HttpContext.Current.Request["groupId"];
            if (Guid.TryParse(groupIdStr, out var groupId) && news.GroupId == groupId)
            {
                viewModel.GroupHeader = _groupHelper.GetHeader(groupId);
            }

            return OkResult();
        }

        private NewsViewModel GetDetails(Entities.News news)
        {
            var details = news.Map<NewsViewModel>();

            details.Media = MediaHelper.GetMediaUrls(news.MediaIds);

            details.LightboxPreviewModel = _lightBoxHelper.GetGalleryPreviewModel(news.MediaIds, PresetStrategies.ForActivityDetails);
            details.CanEdit = _newsService.CanEdit(news);
            details.Links = _feedLinkService.GetLinks(news.Id);
            details.IsReadOnly = false;
            details.HeaderInfo = news.Map<IntranetActivityDetailsHeaderViewModel>();
            details.HeaderInfo.Dates = news.PublishDate.ToDateTimeFormat().ToEnumerable();
            details.HeaderInfo.Owner = _memberService.Get(news).ToViewModel();
            details.HeaderInfo.Links = _feedLinkService.GetLinks(news.Id);


            return details;
        }
    }
}