using Compent.Shared.Extensions.Bcl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Models.Headers;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Helpers;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Media;
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
        INodeViewModelConverter<UintraNewsEditPageModel, UintraNewsEditPageViewModel>
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IFeedLinkService _feedLinkService;
        private readonly INewsService<Entities.News> _newsService;
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly IUserTagService _userTagService;
        private readonly IUserTagProvider _userTagProvider;
        private readonly ILightboxHelper _lightboxHelper;
        private readonly IMemberServiceHelper _memberHelper;

        public UintraNewsEditPageViewModelConverter(
            IPermissionsService permissionsService,
            IFeedLinkService feedLinkService,
            INewsService<Entities.News> newsService,
            IIntranetMemberService<IntranetMember> memberService, 
            IUserTagService userTagService, 
            IUserTagProvider userTagProvider, 
            ILightboxHelper lightboxHelper,
            IMemberServiceHelper memberHelper)
        {
            _permissionsService = permissionsService;
            _feedLinkService = feedLinkService;
            _newsService = newsService;
            _memberService = memberService;
            _userTagService = userTagService;
            _userTagProvider = userTagProvider;
            _lightboxHelper = lightboxHelper;
            _memberHelper = memberHelper;
        }
        public void Map(UintraNewsEditPageModel node, UintraNewsEditPageViewModel viewModel)
        {
            var idStr = HttpContext.Current.Request.GetRequestQueryValue("id");

            if (!Guid.TryParse(idStr, out var id))
                return;

            viewModel.CanEdit = _newsService.CanEdit(id);

            if (!viewModel.CanEdit)
            {
                return;
            }

            var news = _newsService.Get(id);

            viewModel.Details = GetDetails(news);
            viewModel.CanEditOwner = _permissionsService.Check(IntranetActivityTypeEnum.News, PermissionActionEnum.EditOwner);
            viewModel.AllowedMediaExtensions = _newsService.GetMediaSettings().AllowedMediaExtensions;
            viewModel.PinAllowed = _permissionsService.Check(PermissionResourceTypeEnum.News, PermissionActionEnum.CanPin);

            if (viewModel.CanEditOwner)
                viewModel.Members = GetUsersWithAccess(PermissionSettingIdentity.Of(PermissionActionEnum.Create, IntranetActivityTypeEnum.News));

            var groupIdStr = HttpContext.Current.Request["groupId"];
            if (!Guid.TryParse(groupIdStr, out var groupId) || news.GroupId != groupId)
                return;

            viewModel.RequiresGroupHeader = true;
            viewModel.GroupId = groupId;
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
            details.HeaderInfo.Owner = _memberHelper.ToViewModel(_memberService.Get(news));
            details.HeaderInfo.Links = _feedLinkService.GetLinks(news.Id);
            details.Tags = _userTagService.Get(news.Id);
            details.AvailableTags = _userTagProvider.GetAll();
            details.LightboxPreviewModel = _lightboxHelper.GetGalleryPreviewModel(news.MediaIds, PresetStrategies.ForActivityDetails);

            return details;
        }
    }
}