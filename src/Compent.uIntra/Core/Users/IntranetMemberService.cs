using System;
using System.Collections.Generic;
using System.Linq;
using Compent.CommandBus;
using Uintra.Core.Caching;
using Uintra.Core.Extensions;
using Uintra.Core.Permissions;
using Uintra.Core.Persistence;
using Uintra.Core.User;
using Uintra.Groups.Sql;
using Uintra.Users;
using Uintra.Users.Commands;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Compent.Uintra.Core.Users
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

		protected virtual IEnumerable<Guid> GetMembersGroupIds(Guid memberId)
		{
			return _groupMemberRepository.FindAll(gm => gm.MemberId == memberId).Select(gm => gm.GroupId);
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

	}
}
