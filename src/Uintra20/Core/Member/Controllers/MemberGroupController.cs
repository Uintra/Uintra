using System.Web.Http;
using Uintra20.Core.Member.Models;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Permissions.Models;
using Umbraco.Web.WebApi;

namespace Uintra20.Core.Member.Controllers
{
    public class MemberGroupController : UmbracoAuthorizedApiController
    {
        private readonly IIntranetMemberGroupService _intranetMemberGroupService;

        public MemberGroupController(IIntranetMemberGroupService intranetMemberGroupService) =>
            _intranetMemberGroupService = intranetMemberGroupService;

        [HttpPost]
        public int Create(MemberGroupCreateModel model) =>
            _intranetMemberGroupService.Create(model.Name);

        [HttpPost]
        public bool Save(MemberGroupViewModel group) =>
            _intranetMemberGroupService.Save(group.Id, group.Name);

        [HttpPost]
        public bool Delete(MemberGroupDeleteModel group)
        {
            _intranetMemberGroupService.Delete(group.Id);
            return true;
        }
    }
}