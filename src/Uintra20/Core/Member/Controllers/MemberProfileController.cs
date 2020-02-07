using System.Threading.Tasks;
using System.Web.Http;
using UBaseline.Core.Controllers;
using Uintra20.Core.Member.Profile.Edit.Models;
using Uintra20.Core.Member.Profile.Services;
using Uintra20.Features.Links;

namespace Uintra20.Core.Member.Controllers
{
    public class MemberProfileController : UBaselineApiController
    {
        private readonly IProfileService _profileService;
        private readonly IProfileLinkProvider _profileLinkProvider;

        public MemberProfileController(
            IProfileService profileService, 
            IProfileLinkProvider profileLinkProvider)
        {
            _profileService = profileService;
            _profileLinkProvider = profileLinkProvider;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetCurrentUserProfile()
        {
            var result = await _profileService.GetCurrentUserProfile();

            return Ok(result);
        }

        [HttpPut]
        public async Task<IHttpActionResult> Edit([FromBody] ProfileEditModel editModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            await _profileService.Edit(editModel);

            var result = _profileLinkProvider.GetProfileLink(editModel.Id);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IHttpActionResult> UpdateNotificationSettings([FromBody] ProfileEditNotificationSettings settings)
        {
            await _profileService.UpdateNotificationSettings(settings);

            return Ok();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeletePhoto([FromUri] int photoId)
        {
            await _profileService.Delete(photoId);

            return Ok();
        }
    }
}