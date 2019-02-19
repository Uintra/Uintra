using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Uintra.Core.User;
using Uintra.Users.Core.Models;
using Umbraco.Core.Models;
using Umbraco.Web.Editors;
using Umbraco.Web.WebApi;

namespace Uintra.Users.Web
{
    public class MemberGroupController : UmbracoAuthorizedApiController
    {
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;

        public MemberGroupController(IIntranetMemberService<IIntranetMember> intranetMemberService)
        {
            _intranetMemberService = intranetMemberService;
        }

        [HttpPost]
        public int Create(MemberGroupCreateModel model)
        {
            Services.MemberGroupService.Save(new MemberGroup() { Name = model.Name });
            return Services.MemberGroupService.GetByName(model.Name).Id;
        }

        [HttpPost]
        public bool Save(MemberGroupViewModel model)
        {

            var memberGroup = Services.MemberGroupService.GetById(model.Id);
            memberGroup.Name = model.Name;
            Services.MemberGroupService.Save(memberGroup);
            return true;
        }

        [HttpPost]
        public bool Delete(MemberGroupDeleteModel model)
        {
            var memberGroup = Services.MemberGroupService.GetById(model.Id);
            Services.MemberGroupService.Delete(memberGroup);
            return true;
        }
    }
}
