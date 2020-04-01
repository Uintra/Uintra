using Compent.Extensions;
using System;
using System.Linq;
using System.Web;
using UBaseline.Core.RequestContext;
using Uintra20.Core.Activity.Models.Headers;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Core.UbaselineModels.RestrictedNode;
using Uintra20.Features.Groups.Helpers;
using Uintra20.Features.Links;
using Uintra20.Features.Media.Helpers;
using Uintra20.Features.Media.Strategies.Preset;
using Uintra20.Features.News.Models;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.News.Converters
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

        public UintraNewsDetailsPageViewModelConverter(
            IUserTagService userTagService,
            IFeedLinkService feedLinkService,
            INewsService<Entities.News> newsService,
            IIntranetMemberService<IntranetMember> memberService,
            ILightboxHelper lightBoxHelper,
            IPermissionsService permissionsService,
            IGroupHelper groupHelper,
            IErrorLinksService errorLinksService, 
            IUBaselineRequestContext context)
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
        }

        public override ConverterResponseModel MapViewModel(UintraNewsDetailsPageModel node, UintraNewsDetailsPageViewModel viewModel)
        {
            var id = _context.ParseQueryString("id").TryParseGuid();

            if (!id.HasValue) return NotFoundResult();

            var news = _newsService.Get(id.Value);

            if (news == null || news.IsHidden) return NotFoundResult();

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