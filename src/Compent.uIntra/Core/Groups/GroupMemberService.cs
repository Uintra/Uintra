using EmailWorker.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Uintra.Core.Extensions;
using Uintra.Core.Media;
using Uintra.Core.Persistence;
using Uintra.Groups;
using Uintra.Groups.Sql;
using Uintra.Users;
using AutoMapperExtensions = Uintra.Core.Extensions.AutoMapperExtensions;

namespace Compent.Uintra.Core.Groups
{
	public class GroupMemberService : GroupMemberServiceBase
	{
		private readonly ISqlRepository<GroupMember> _groupMemberRepository;
		private readonly ICacheableIntranetMemberService _memberCacheService;
		private readonly IMediaHelper _mediaHelper;
		private readonly IGroupService _groupService;
		private readonly IGroupMediaService _groupMediaService;
		private readonly IGroupLinkProvider _groupLinkProvider;

		public GroupMemberService(
			ISqlRepository<GroupMember> groupMemberRepository,
			ICacheableIntranetMemberService memberCacheService,
			IMediaHelper mediaHelper,
			IGroupService groupService,
			IGroupMediaService groupMediaService,
			IGroupLinkProvider groupLinkProvider)
			: base(groupMemberRepository)
		{
			_groupMemberRepository = groupMemberRepository;
			_memberCacheService = memberCacheService;
			_mediaHelper = mediaHelper;
			_groupService = groupService;
			_groupMediaService = groupMediaService;
			_groupLinkProvider = groupLinkProvider;
		}

		public override void Add(Guid groupId, GroupMemberSubscriptionModel model) =>
			AddMany(groupId, model.ToEnumerableOfOne());

		public override void AddMany(Guid groupId, IEnumerable<GroupMemberSubscriptionModel> subscriptions)
		{
			var groupMembers = subscriptions
				.Select(memberId => GetNewGroupMember(groupId, memberId))
				.ToList();

			_groupMemberRepository.Add(groupMembers);
			_memberCacheService.UpdateMemberCache(groupMembers.Select(g => g.MemberId));
		}

		public override void Remove(Guid groupId, Guid memberId)
		{
			_groupMemberRepository.Delete(IsGroupAndUserMatch(memberId, groupId));
			_memberCacheService.UpdateMemberCache(memberId);
		}

		public override string Create(GroupCreateModel model)
		{
			var group = AutoMapperExtensions.Map<GroupModel>(model);

			group.GroupTypeId = GroupTypeEnum.Open.ToInt();

			var createdMedias = _mediaHelper.CreateMedia(model).ToList();

			group.ImageId = createdMedias.Any()
				? (int?)createdMedias.First()
				: null;

			var groupId = _groupService.Create(group);
            Add(groupId, model.Creator);

			_groupMediaService.GroupTitleChanged(groupId, group.Title);

			return _groupLinkProvider.GetGroupLink(groupId);
		}

		public override GroupMember GetByMemberId(Guid id) =>
			_groupMemberRepository.Find(gm => gm.MemberId == id);

		public override void Update(GroupMember groupMember)
		{
			_groupMemberRepository.Update(groupMember);
			_memberCacheService.UpdateMemberCache(groupMember.MemberId);
		}

		public override GroupMember GetGroupMemberByMemberIdAndGroupId(
			Guid memberId,
			Guid groupId) =>
			_groupMemberRepository.Find(IsGroupAndUserMatch(memberId, groupId));

		public override bool IsMemberAdminOfGroup(Guid memberId, Guid groupId) =>
			GetGroupMemberByMemberIdAndGroupId(memberId, groupId)?.IsAdmin ?? false;

		public override void ToggleAdminRights(Guid memberId, Guid groupId)
		{
			var groupMember = GetGroupMemberByMemberIdAndGroupId(memberId, groupId);

			groupMember.IsAdmin = !groupMember.IsAdmin;

			Update(groupMember);
			_memberCacheService.UpdateMemberCache(memberId);
		}

		private static Expression<Func<GroupMember, bool>> IsGroupAndUserMatch(Guid memberId, Guid groupId) =>
			g => g.GroupId == groupId && g.MemberId == memberId;
	}
}