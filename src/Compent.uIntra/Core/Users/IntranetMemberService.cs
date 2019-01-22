using System;
using System.Collections.Generic;
using System.Linq;
using Compent.Uintra.Core.Search.Entities;
using Compent.Uintra.Core.Search.Indexes;
using Uintra.Core.Caching;
using Uintra.Core.Extensions;
using Uintra.Core.Persistence;
using Uintra.Core.User;
using Uintra.Groups.Sql;
using Uintra.Search;
using Uintra.Tagging.UserTags;
using Uintra.Users;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using static LanguageExt.Prelude;

namespace Compent.Uintra.Core.Users
{
    public class IntranetMemberService<T> : IntranetMemberServiceBase<T>, IIndexer
        where T : IntranetUser, new()
    {
        private readonly ISqlRepository<GroupMember> _groupMemberRepository;
        private readonly IElasticUserIndex _elasticUserIndex;
        private readonly IIntranetUserContentProvider _intranetUserContentProvider;
        private readonly IUserTagService _userTagService;

        public IntranetMemberService(
            IMediaService mediaService,
            IMemberService memberService,
            UmbracoContext umbracoContext,
            UmbracoHelper umbracoHelper,
            IRoleService roleService,
            ICacheService cacheService,
            ISqlRepository<GroupMember> groupMemberRepository,
            IElasticUserIndex elasticUserIndex,
            IIntranetUserContentProvider intranetUserContentProvider,
            IUserTagService userTagService
            )
            : base(mediaService, memberService, umbracoContext, umbracoHelper, roleService, cacheService)
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
            user.Phone = member.GetValueOrDefault<string>(ProfileConstants.Phone);
            user.Department = member.GetValueOrDefault<string>(ProfileConstants.Department);
            user.GroupIds = GetMembersGroupIds(user.Id);

            return user;
        }

        protected virtual IEnumerable<Guid> GetMembersGroupIds(Guid memberId)
        {
            return _groupMemberRepository.FindAll(gm => gm.MemberId == memberId).Select(gm => gm.GroupId);
        }

        public void FillIndex()
        {
            var actualUsers = GetAll().Where(u => !u.Inactive).ToList();
            var searchableUsers = actualUsers.Select(MapToSearchableUser);

            _elasticUserIndex.Index(searchableUsers);
        }

        public override void UpdateMemberCache(Guid memberId)
        {
            base.UpdateMemberCache(memberId);
            var user = Get(memberId);
            _elasticUserIndex.Index(MapToSearchableUser(user));
        }

        public override void UpdateMemberCache(IEnumerable<Guid> memberIds)
        {
            base.UpdateMemberCache(memberIds);
            var users = GetMany(memberIds).Select(MapToSearchableUser);
            _elasticUserIndex.Index(users);
        }

        private SearchableUser MapToSearchableUser(IntranetUser user)
        {
            var searchableUser = user.Map<SearchableUser>();
            searchableUser.Url = _intranetUserContentProvider.GetProfilePage().Url.AddIdParameter(user.Id);
            searchableUser.UserTagNames = _userTagService.Get(user.Id).Select(t => t.Text).ToList();
            searchableUser.GroupIds = user.GroupIds;
            return searchableUser;
        }
    }
}
