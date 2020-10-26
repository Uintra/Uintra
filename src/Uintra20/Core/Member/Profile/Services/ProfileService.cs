using System.Linq;
using System.Threading.Tasks;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models.Dto;
using Uintra20.Core.Member.Profile.Edit.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Media;
using Uintra20.Features.Media.Enums;
using Uintra20.Features.Media.Helpers;
using Uintra20.Features.Notification.Services;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Core.Member.Profile.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IMemberNotifiersSettingsService _memberNotifiersSettingsService;
        private readonly IUserTagService _userTagService;

        public ProfileService(
            IIntranetMemberService<IntranetMember> intranetMemberService, 
            IMediaHelper mediaHelper, 
            IMemberNotifiersSettingsService memberNotifiersSettingsService, 
            IUserTagService userTagService)
        {
            _intranetMemberService = intranetMemberService;
            _mediaHelper = mediaHelper;
            _memberNotifiersSettingsService = memberNotifiersSettingsService;
            _userTagService = userTagService;
        }

        public async Task Delete(int photoId)
        {
            var member = _intranetMemberService.GetCurrentMember();

            _mediaHelper.DeleteMedia(photoId);

            var updateUser = member.Map<UpdateMemberDto>();

            updateUser.DeleteMedia = true;

            await _intranetMemberService.UpdateAsync(updateUser);
        }

        public async Task UpdateNotificationSettings(ProfileEditNotificationSettings settings)
        {
            var member = await _intranetMemberService.GetCurrentMemberAsync();

            await _memberNotifiersSettingsService.SetForMemberAsync(member.Id, settings.NotifierTypeEnum, settings.IsEnabled);
        }

        public async Task Edit(ProfileEditModel editModel)
        {
            var newMedias = _mediaHelper.CreateMedia(editModel, MediaFolderTypeEnum.MembersContent).ToArray();

            var member = editModel.Map<UpdateMemberDto>();

            member.NewMedia = newMedias.Length > 0
                ? newMedias.First()
                : default(int?);

            await _userTagService.ReplaceAsync(editModel.Id, editModel.TagIdsData);
            
            await _intranetMemberService.UpdateAsync(member);
        }

        public async Task<ProfileEditViewModel> GetCurrentUserProfile()
        {
            var member = await _intranetMemberService.GetCurrentMemberAsync();

            var result = member.Map<ProfileEditViewModel>();

            result.MemberNotifierSettings = _memberNotifiersSettingsService.GetForMember(member.Id);

            return result;
        }
    }
}