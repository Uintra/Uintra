using System.Threading.Tasks;
using System.Web.Http;
using UBaseline.Core.Controllers;
using Uintra20.Core.Member.Profile.Edit.Models;
using Uintra20.Core.Member.Profile.Services;

namespace Uintra20.Core.Member.Controllers
{
    public class MemberProfileController : UBaselineApiController
    {
        private readonly IProfileService _profileService;

        public MemberProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetCurrentUserProfile()
        {
            var result = await _profileService.GetCurrentUserProfile();

            return Ok(result);
        }

        [HttpPut]
        public async Task<IHttpActionResult> Edit([FromBody] ProfileEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _profileService.Edit(model);
            return Ok();
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