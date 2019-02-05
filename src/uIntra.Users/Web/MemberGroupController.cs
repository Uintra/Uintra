using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Uintra.Users.Core.Models;
using Umbraco.Core.Models;
using Umbraco.Web.WebApi;

namespace Uintra.Users.Web
{
    public class MemberGroupController : UmbracoAuthorizedApiController
    {
        public MemberGroupViewModel Get(int id)
        {
            var memberGroup = Services.MemberGroupService.GetById(id);
            return new MemberGroupViewModel() { Id = memberGroup.Id, Name = memberGroup.Name };
        }

        public bool Create(string name)
        {
            Services.MemberGroupService.Save(new MemberGroup() { Name = name });
            return true;
        }

        [HttpPost]
        public bool Save(MemberGroupViewModel model)
        {

            var memberGroup = Services.MemberGroupService.GetById(model.Id);
            memberGroup.Name = model.Name;
            Services.MemberGroupService.Save(memberGroup);
            return true;
        }

        public bool Delete(int id)
        {
            var memberGroup = Services.MemberGroupService.GetById(id);
            Services.MemberGroupService.Delete(memberGroup);
            return true;
        }
    }
}
