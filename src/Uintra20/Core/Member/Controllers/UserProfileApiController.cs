using System;
using System.Linq;
using System.Web.Http;
using UBaseline.Core.Controllers;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Models.Dto;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Media;
using Uintra20.Features.Notification.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Core.Member.Controllers
{
    public class UserProfileApiController : UBaselineApiController
    {
        private readonly IMediaHelper _mediaHelper;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IMemberNotifiersSettingsService _memberNotifiersSettingsService;

        public UserProfileApiController(
            IMediaHelper mediaHelper,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IMemberNotifiersSettingsService memberNotifiersSettingsService)
        {
            _mediaHelper = mediaHelper;
            _intranetMemberService = intranetMemberService;
            _memberNotifiersSettingsService = memberNotifiersSettingsService;
        }

        [HttpGet]
        public ProfileViewModel GetProfile(Guid id)
        {
            var member = _intranetMemberService.Get(id);

            return member == null
                ? null
                : MapToViewModel(member);
        }

        [HttpGet]
        public ProfileEditModel GetCurrentUserProfile()
        {
            var user = _intranetMemberService.GetCurrentMember();
            return user == null
                ? null
                : MapToEditModel(user);
        }

        [HttpPost]
        public IHttpActionResult Edit(ProfileEditModel model)
        {
            var member = MapToMemberDto(model);
            _intranetMemberService.Update(member);

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeletePhoto(int photoId)
        {
            var user = _intranetMemberService.GetCurrentMember();

            _mediaHelper.DeleteMedia(photoId);

            var updateUser = user.Map<UpdateMemberDto>();
            updateUser.DeleteMedia = true;

            _intranetMemberService.Update(updateUser);

            return Ok();
        }

        private ProfileEditModel MapToEditModel(IIntranetMember member)
        {
            var result = member.Map<ProfileEditModel>();
            result.MemberNotifierSettings = _memberNotifiersSettingsService.GetForMember(member.Id);

            return result;
        }
        private ProfileViewModel MapToViewModel(IIntranetMember member)
        {
            var result = member.Map<ProfileViewModel>();
            return result;
        }
        private UpdateMemberDto MapToMemberDto(ProfileEditModel model)
        {
            var newMedias = _mediaHelper.CreateMedia(model).ToArray();

            var updateMember = model.Map<UpdateMemberDto>();
            updateMember.NewMedia = newMedias.Length > 0 ? newMedias.First() : default(int?);

            return updateMember;
        }
    }
}