using uIntra.Core.Caching;
using uIntra.Core.Extentions;
using uIntra.Core.TypeProviders;
using uIntra.Users;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Compent.uIntra.Core.Users
{
    public class IntranetUserService<T> : IntranetUserServiceBase<T>
         where T : IntranetUser, new()
    {
        public IntranetUserService(
            IMemberService memberService,
            UmbracoContext umbracoContext,
            UmbracoHelper umbracoHelper,
            IRoleService roleService,
            IIntranetRoleTypeProvider intranetRoleTypeProvider,
            ICacheService cacheService)
            : base(memberService, umbracoContext, umbracoHelper, roleService, intranetRoleTypeProvider, cacheService)
        {
        }

        protected override T Map(IMember member)
        {
            var user = base.Map(member);
            user.FirstName = member.GetValueOrDefault<string>(ProfileConstants.FirstName);
            user.LastName = member.GetValueOrDefault<string>(ProfileConstants.LastName);

            return user;
        }
    }
}
