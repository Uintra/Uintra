using EmailWorker.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
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
            _groupMemberRepository.Delete(gm => gm.GroupId == groupId && gm.MemberId == memberId);
            _memberCacheService.UpdateMemberCache(memberId);
        }

        public override string Create(GroupCreateModel model)
        {
            var group = AutoMapperExtensions.Map<GroupModel>(model);

            group.GroupTypeId = GroupTypeEnum.Open.ToInt();

            var createdMedias = _mediaHelper.CreateMedia(model).ToList();

            group.ImageId = createdMedias.Any()
                ? (int?) createdMedias.First()
                : null;

            var groupId = _groupService.Create(group);

            _groupMediaService.GroupTitleChanged(groupId, group.Title);

            Add(groupId, model.Creator);

            return _groupLinkProvider.GetGroupLink(groupId);
        }

        public override GroupMember Get(Guid id) =>
            _groupMemberRepository.Get(id);

        public override void Update(GroupMember groupMember) =>
            _groupMemberRepository.Update(groupMember);

        public override GroupMember GetGroupMemberByMemberIdAndGroupId(
            Guid memberId,
            Guid groupId) =>
            _groupMemberRepository.Find(g => g.GroupId == groupId && g.MemberId == memberId);

        public override bool IsMemberAdminOfGroup(Guid memberId, Guid groupId) => 
            GetGroupMemberByMemberIdAndGroupId(memberId, groupId).IsAdmin;
    }
}