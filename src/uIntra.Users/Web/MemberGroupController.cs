using System.Web.Http;
using Uintra.Core.Permissions;
using Uintra.Core.User;
using Uintra.Users.Core.Models;
using Umbraco.Web.WebApi;

namespace Uintra.Users.Web
{
    public class MemberGroupController : UmbracoAuthorizedApiController
    {
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly IIntranetMemberGroupService _intranetMemberGroupService;

        public MemberGroupController(IIntranetMemberService<IIntranetMember> intranetMemberService,
            IIntranetMemberGroupService intranetMemberGroupService)
        {
            _intranetMemberService = intranetMemberService;
            _intranetMemberGroupService = intranetMemberGroupService;
        }

        [HttpPost]
        public int Create(MemberGroupCreateModel model)
        {
            return _intranetMemberGroupService.Create(model.Name);
        }

        [HttpPost]
        public bool Save(MemberGroupViewModel model)
        {
            return _intranetMemberGroupService.Save(model.Id, model.Name);
        }

        [HttpPost]
        public bool Delete(MemberGroupDeleteModel model)
        {
            _intranetMemberGroupService.Delete(model.Id);
            return true;
        }
    }
}
