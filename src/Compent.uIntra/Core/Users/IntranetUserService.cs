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

namespace Compent.Uintra.Core.Users
{
    public class IntranetUserService<T> : IntranetUserServiceBase<T>, IIndexer
        where T : IntranetUser, new()
    {
        private readonly ISqlRepository<GroupMember> _groupMemberRepository;
        private readonly IElasticUserIndex _elasticUserIndex;
        private readonly IIntranetUserContentProvider _intranetUserContentProvider;
        private readonly IUserTagService _userTagService;
        private readonly IMediaService _mediaService;

        public IntranetUserService(
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
            _mediaService = mediaService;
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
            var actualUsers = GetAll().Where(u => !u.Inactive).ToList();
            var searchableUsers = actualUsers.Select(MapToSearchableUser);

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
