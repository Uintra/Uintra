using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Caching;
using uIntra.Core.Extentions;
using uIntra.Core.Persistence;
using uIntra.Core.TypeProviders;
using uIntra.Groups.Sql;
using uIntra.Users;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Compent.uIntra.Core.Users
{
    public class IntranetUserService<T> : IntranetUserServiceBase<T>
         where T : IntranetUser, new()
    {
        private readonly ISqlRepository<GroupMember> _groupMemberRepository; //TODO use service instead

        public IntranetUserService(
            IMemberService memberService,
            UmbracoContext umbracoContext,
            UmbracoHelper umbracoHelper,
            IRoleService roleService,
            IIntranetRoleTypeProvider intranetRoleTypeProvider,
            ICacheService cacheService,
            ISqlRepository<GroupMember> groupMemberRepository)
            : base(memberService, umbracoContext, umbracoHelper, roleService, intranetRoleTypeProvider, cacheService)
        {
            _groupMemberRepository = groupMemberRepository;
        }

        protected override T Map(IMember member)
        {
            var user = base.Map(member);
            user.FirstName = member.GetValueOrDefault<string>(ProfileConstants.FirstName);
            user.LastName = member.GetValueOrDefault<string>(ProfileConstants.LastName);
            user.GroupIds = GetMembersGroupIds(user.Id);

            return user;
        }
        
        protected virtual IEnumerable<Guid> GetMembersGroupIds(Guid memberId)
        {
            return _groupMemberRepository.FindAll(gm => gm.MemberId == memberId).Select(gm => gm.GroupId);
        }
    }
}
