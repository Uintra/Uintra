using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Compent.CommandBus;
using LanguageExt;
using Uintra20.Core.Commands;
using Uintra20.Core.Member.Entities;
using Uintra20.Features.Groups.Sql;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Infrastructure.Caching;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Persistence.Sql;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Uintra20.Core.Member
{
    public class IntranetMemberService<T> : IntranetMemberServiceBase<T>
         where T : IntranetMember, new()
    {
        private readonly ISqlRepository<GroupMember> _groupMemberRepository;
        private readonly ICommandPublisher _commandPublisher;


        public IntranetMemberService(
            IMediaService mediaService,
            IMemberService memberService,
            UmbracoContext umbracoContext,
            UmbracoHelper umbracoHelper,
            ICacheService cacheService,
            ISqlRepository<GroupMember> groupMemberRepository,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IIntranetMemberGroupService intranetMemberGroupService,
            ICommandPublisher commandPublisher
        )
            : base(mediaService, memberService, umbracoContext, umbracoHelper, cacheService, intranetUserService, intranetMemberGroupService)
        {
            _groupMemberRepository = groupMemberRepository;
            _commandPublisher = commandPublisher;
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

        protected override async Task<T> MapAsync(IMember member)
        {
            var user = base.Map(member);
            user.FirstName = member.GetValueOrDefault<string>(ProfileConstants.FirstName);
            user.LastName = member.GetValueOrDefault<string>(ProfileConstants.LastName);
            user.Phone = member.GetValueOrDefault<string>(ProfileConstants.Phone);
            user.Department = member.GetValueOrDefault<string>(ProfileConstants.Department);
            user.GroupIds = await GetMembersGroupIdsAsync(user.Id);

            return user;
        }


        protected virtual IEnumerable<Guid> GetMembersGroupIds(Guid memberId)
        {
            return _groupMemberRepository.FindAll(gm => gm.MemberId == memberId).Select(gm => gm.GroupId);
        }

        protected virtual async Task<IEnumerable<Guid>> GetMembersGroupIdsAsync(Guid memberId)
        {
            return await _groupMemberRepository.FindAllAsync(gm => gm.MemberId == memberId).Select(x => x.Select(gm => gm.GroupId));
        }

        public override void UpdateMemberCache(Guid memberId)
        {
            base.UpdateMemberCache(memberId);
            var member = Get(memberId);
            _commandPublisher.Publish(new MemberChanged(member));
        }

        public override void UpdateMemberCache(IEnumerable<Guid> memberIds)
        {
            var memberIdsList = memberIds.ToList();
            base.UpdateMemberCache(memberIdsList);
            var members = GetMany(memberIdsList);
            _commandPublisher.Publish(new MembersChanged(members));
        }

        public override async Task UpdateMemberCacheAsync(Guid memberId)
        {
            await base.UpdateMemberCacheAsync(memberId);
            var member = await GetAsync(memberId);
            _commandPublisher.Publish(new MemberChanged(member));
        }

        public override async Task UpdateMemberCacheAsync(IEnumerable<Guid> memberIds)
        {
            var memberIdsList = memberIds.ToList();
            await base.UpdateMemberCacheAsync(memberIdsList);
            var members = await GetManyAsync(memberIdsList);
            _commandPublisher.Publish(new MembersChanged(members));
        }

    }
}