using System;
using System.Collections.Generic;
using System.Linq;
using Compent.uIntra.Core.Search.Entities;
using Compent.uIntra.Core.Search.Indexes;
using uIntra.Core.Caching;
using uIntra.Core.Extensions;
using uIntra.Core.Persistence;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Groups.Sql;
using uIntra.Search;
using uIntra.Tagging.UserTags;
using uIntra.Users;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Compent.uIntra.Core.Users
{
    public class IntranetUserService<T> : IntranetUserServiceBase<T>, IIndexer
        where T : IntranetUser, new()
    {
        private readonly ISqlRepository<GroupMember> _groupMemberRepository; //TODO use service instead
        private readonly IElasticUserIndex _elasticUserIndex;
        private readonly IIntranetUserContentProvider _intranetUserContentProvider;
        private readonly IUserTagService _userTagService;

        public IntranetUserService(
            IMemberService memberService,
            UmbracoContext umbracoContext,
            UmbracoHelper umbracoHelper,
            IRoleService roleService,
            IIntranetRoleTypeProvider intranetRoleTypeProvider,
            ICacheService cacheService,
            ISqlRepository<GroupMember> groupMemberRepository,
            IElasticUserIndex elasticUserIndex,
            IIntranetUserContentProvider intranetUserContentProvider,
            IUserTagService userTagService)
            : base(memberService, umbracoContext, umbracoHelper, roleService, intranetRoleTypeProvider, cacheService)
        {
            _groupMemberRepository = groupMemberRepository;
            _elasticUserIndex = elasticUserIndex;
            _intranetUserContentProvider = intranetUserContentProvider;
            _userTagService = userTagService;
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

        public void FillIndex()
        {
            var searchableUsers = GetAll().Select(MapToSearchableUser);
            _elasticUserIndex.Index(searchableUsers);
        }

        public override void UpdateUserCache(Guid userId)
        {
            base.UpdateUserCache(userId);
            var user = Get(userId);
            _elasticUserIndex.Index(MapToSearchableUser(user));
        }

        private SearchableUser MapToSearchableUser(IntranetUser user)
        {
            var searchableUser = user.Map<SearchableUser>();
            searchableUser.Url = _intranetUserContentProvider.GetProfilePage().Url.AddIdParameter(user.Id);
            searchableUser.UserTagNames = _userTagService.Get(user.Id).Select(t => t.Text).ToList();
            return searchableUser;
        }
    }
}
