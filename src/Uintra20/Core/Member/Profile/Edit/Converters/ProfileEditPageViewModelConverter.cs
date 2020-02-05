using System.Linq;
using UBaseline.Core.Node;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Profile.Edit.Models;
using Uintra20.Core.Member.Profile.Services;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Notification.Services;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;
using Umbraco.Core.Services;

namespace Uintra20.Core.Member.Profile.Edit.Converters
{
    public class ProfileEditPageViewModelConverter :
        INodeViewModelConverter<ProfileEditPageModel, ProfileEditPageViewModel>
    {
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IUserTagService _userTagService;
        private readonly IUserTagProvider _userTagProvider;
        private readonly IMemberNotifiersSettingsService _memberNotifiersSettingsService;
        private readonly IMediaService _mediaService;

        public ProfileEditPageViewModelConverter(
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IUserTagService userTagService,
            IUserTagProvider userTagProvider,
            IMemberNotifiersSettingsService memberNotifiersSettingsService,
            IMediaService mediaService)
        {
            _intranetMemberService = intranetMemberService;
            _userTagService = userTagService;
            _userTagProvider = userTagProvider;
            _memberNotifiersSettingsService = memberNotifiersSettingsService;
            _mediaService = mediaService;
        }

        public void Map(
            ProfileEditPageModel node,
            ProfileEditPageViewModel viewModel)
        {
            var member = _intranetMemberService.GetCurrentMember();

            viewModel.Profile = member.Map<ProfileEditModel>();
            viewModel.Tags = _userTagService.Get(member.Id);
            viewModel.AvailableTags = _userTagProvider.GetAll();
            viewModel.Profile.MemberNotifierSettings = _memberNotifiersSettingsService.GetForMember(member.Id);
            var media = _mediaService.GetRootMedia();

            // TODO: Extract to const strings at 59 line
            var memberContentFolder =
                media.First(m => m.ContentType.Alias == "Folder" && m.Name == "Members Content");

            viewModel.Profile.MediaRootId = memberContentFolder.Id;
        }
    }
}