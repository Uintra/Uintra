using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Extensions;
using Uintra.Core.Media;
using Uintra.Core.Persistence;
using Uintra.Groups;
using Uintra.Groups.Sql;
using Uintra.Users;
using static LanguageExt.Prelude;

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

        public override void Add(Guid groupId, Guid memberId) =>
            AddMany(groupId, List(memberId));

        public override void AddMany(Guid groupId, IEnumerable<Guid> memberIds)
        {
            var enumeratedMemberIds = memberIds as Guid[] ?? memberIds.ToArray();
            var groupMembers = enumeratedMemberIds
                .Select(memberId => GetNewGroupMember(groupId, memberId))
                .ToList();

            _groupMemberRepository.Add(groupMembers);
            _memberCacheService.UpdateMemberCache(enumeratedMemberIds);
        }

        public override void Remove(Guid groupId, Guid memberId)
        {
            _groupMemberRepository.Delete(gm => gm.GroupId == groupId && gm.MemberId == memberId);
            _memberCacheService.UpdateMemberCache(memberId);
        }

        public override string Create(GroupCreateModel model)
        {
            var group = model.Map<GroupModel>();

            group.GroupTypeId = GroupTypeEnum.Open.ToInt();

            var createdMedias = _mediaHelper.CreateMedia(model).ToList();

            group.ImageId = createdMedias.Any() 
                ? (int?)createdMedias.First() 
                : null;

            var groupId = _groupService.Create(group);

            _groupMediaService.GroupTitleChanged(groupId, group.Title);

            Add(groupId, model.CreatorId);

            return _groupLinkProvider.GetGroupLink(groupId);
        }
    }
}