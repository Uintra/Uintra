using System;
using System.Reflection;
using System.Web;
using System.Xml;
using UBaseline.Core.Node;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Profile.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Core.User;
using Uintra20.Features.Media;
using Uintra20.Features.Media.Strategies.Preset;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Core.Member.Profile.Converters
{
    public class ProfilePageViewModelConverter : INodeViewModelConverter<ProfilePageModel, ProfilePageViewModel>
    {
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly IUserTagService _userTagService;
        private readonly IIntranetUserContentProvider _intranetUserContentProvider;
        private readonly IImageHelper _imageHelper;

        public ProfilePageViewModelConverter(
            IIntranetMemberService<IntranetMember> memberService, 
            IUserTagService userTagService,
            IIntranetUserContentProvider intranetUserContentProvider,
            IImageHelper imageHelper)
        {
            _memberService = memberService;
            _userTagService = userTagService;
            _intranetUserContentProvider = intranetUserContentProvider;
            _imageHelper = imageHelper;
        }

        public void Map(
            ProfilePageModel node, 
            ProfilePageViewModel viewModel)
        {
            var id = HttpContext.Current.Request.GetRequestQueryValue("id");

            if (!Guid.TryParse(id, out var parseId))  return;

            var member = _memberService.Get(parseId);

            var currentMemberId = _memberService.GetCurrentMemberId();

            if (member.Id == currentMemberId)
            {
                viewModel.EditProfileLink = _intranetUserContentProvider.GetEditPage().Url.ToLinkModel();
            }

            viewModel.Profile = member.Map<ProfileViewModel>();
            viewModel.Profile.Photo = _imageHelper.GetImageWithResize(member.Photo, PresetStrategies.ForMemberProfile.ThumbnailPreset);
            viewModel.Tags = _userTagService.Get(member.Id);
        }
    }
}