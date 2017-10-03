using System.Linq;
using uIntra.Core.Caching;
using uIntra.Core.Extentions;
using uIntra.Core.TypeProviders;
using uIntra.Groups;
using uIntra.Users;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Compent.uIntra.Core.Users
{
    public class IntranetUserService<T> : IntranetUserServiceBase<T>
         where T : IntranetUser, new()
    {
        private readonly IGroupMemberService _groupMemberService;
        //TODO use service instead

        public IntranetUserService(
            IMemberService memberService,
            UmbracoContext umbracoContext,
            UmbracoHelper umbracoHelper,
            IRoleService roleService,
            IIntranetRoleTypeProvider intranetRoleTypeProvider,
            ICacheService cacheService,
            IGroupMemberService groupMemberService)
            : base(memberService, umbracoContext, umbracoHelper, roleService, intranetRoleTypeProvider, cacheService)
        {
            _groupMemberService = groupMemberService;
        }

        protected override T Map(IMember member)
        {
            var user = base.Map(member);
            user.FirstName = member.GetValueOrDefault<string>(ProfileConstants.FirstName);
            user.LastName = member.GetValueOrDefault<string>(ProfileConstants.LastName);
            user.GroupIds = _groupMemberService.GetGroupMemberByMember(user.Id).Select(m => m.GroupId);
            return user;
        }       
    }
}
