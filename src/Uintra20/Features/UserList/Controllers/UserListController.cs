using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Compent.Extensions;
using UBaseline.Core.Controllers;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Core.Search.Entities;
using Uintra20.Core.Search.Indexes;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Notification;
using Uintra20.Features.Notification.Configuration;
using Uintra20.Features.Notification.Entities.Base;
using Uintra20.Features.Notification.Services;
using Uintra20.Features.Search.Queries;
using Uintra20.Features.UserList.Helpers;
using Uintra20.Features.UserList.Models;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.Helpers;

namespace Uintra20.Features.UserList.Controllers
{
    public class UserListController : UBaselineApiController
    {
        private const int AmountPerRequest = 10;

        private readonly IElasticMemberIndex<SearchableMember> _elasticIndex;
        private readonly IProfileLinkProvider _profileLinkProvider;
        private readonly IGroupService _groupService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IGroupMemberService _groupMemberService;
        private readonly INotificationsService _notificationsService;
        private readonly INotifierDataHelper _notifierDataHelper;

        public UserListController(
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IElasticMemberIndex<SearchableMember> elasticIndex,
            IProfileLinkProvider profileLinkProvider,
            IGroupService groupService,
            IGroupMemberService groupMemberService,
            INotificationsService notificationsService,
            INotifierDataHelper notifierDataHelper)
        {
            _elasticIndex = elasticIndex;
            _profileLinkProvider = profileLinkProvider;
            _groupService = groupService;
            _intranetMemberService = intranetMemberService;
            _groupMemberService = groupMemberService;
            _notificationsService = notificationsService;
            _notifierDataHelper = notifierDataHelper;
        }

        [HttpPost]
        public virtual MembersRowsViewModel GetUsers([FromBody]MembersListSearchModel listSearch)
        {
            var (activeUsers, isLastRequest) = GetActiveUsers(listSearch.Map<ActiveMemberSearchQuery>(), listSearch.GroupId);

            var model = GetUsersRowsViewModel(listSearch.GroupId);

            model.SelectedColumns = UsersPresentationHelper.ExtendIfGroupMembersPage(listSearch.GroupId, UsersPresentationHelper.GetProfileColumns());
            model.Members = activeUsers;
            model.IsLastRequest = isLastRequest;

            return model;
        }

        public bool ExcludeUserFromGroup(Guid groupId, Guid userId)
        {
            var currentMember = _intranetMemberService.GetCurrentMember();

            if (currentMember == null)
            {
                return false;
            }

            var isAdmin = _groupMemberService.IsMemberAdminOfGroup(currentMember.Id, groupId);

            if (!isAdmin)
            {
                return false;
            }

            var group = _groupService.Get(groupId);

            if (userId == group.CreatorId)
            {
                return false;
            }

            _groupMemberService.Remove(groupId, userId);
            return true;
        }


        [HttpPut]
        public IHttpActionResult Assign(GroupToggleAdminRightsModel rights)
        {
            _groupMemberService.ToggleAdminRights(rights.MemberId, rights.GroupId);

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult InviteMember(MemberGroupInviteModel invite)
        {
            InviteUser(invite);
            SendInvitationToUser(invite);

            return Ok();
        }

        private void InviteUser(MemberGroupInviteModel invite) =>
            _groupMemberService.Add(invite.GroupId, new GroupMemberSubscriptionModel
            {
                MemberId = invite.MemberId
            });

        private void SendInvitationToUser(MemberGroupInviteModel invite) =>
            _notificationsService.ProcessNotification(new NotifierData
            {
                NotificationType = NotificationTypeEnum.GroupInvitation,
                ReceiverIds = invite.MemberId.ToEnumerable(),
                ActivityType = CommunicationTypeEnum.CommunicationSettings,
                Value = _notifierDataHelper.GetGroupInvitationDataModel(NotificationTypeEnum.GroupInvitation, invite.GroupId, invite.MemberId,
                    _intranetMemberService.GetCurrentMember().Id)
            });


        private (IEnumerable<MemberModel> result, bool isLastRequest) GetActiveUsers(ActiveMemberSearchQuery query, Guid? groupId)
        {
            var (searchResult, totalHits) = GetActiveUserIds(query);

            var result = _intranetMemberService.GetMany(searchResult)
                .Select(x => MapToViewModel(x, groupId));

            var skip = (query.Page - 1) * AmountPerRequest;
            
            var isLastRequest = skip + AmountPerRequest >= totalHits;

            return (result, isLastRequest);
        }

        private (IEnumerable<Guid> searchResult, long totalHits) GetActiveUserIds(
            ActiveMemberSearchQuery query)
        {
            var skip = (query.Page - 1) * AmountPerRequest;

            var searchQuery = new MemberSearchQuery
            {
                Text = query.Text,
                Skip = skip,
                Take = AmountPerRequest,
                OrderingString = query.OrderingString,
                SearchableTypeIds = ((int)UintraSearchableTypeEnum.Member).ToEnumerable(),
                GroupId = query.GroupId,
                MembersOfGroup = query.MembersOfGroup
            };

            var searchResult = _elasticIndex.Search(searchQuery);
            var result = searchResult.Documents.Select(r => Guid.Parse(r.Id.ToString()));

            return (result, searchResult.TotalHits);
        }

        private MembersRowsViewModel GetUsersRowsViewModel(Guid? groupId)
        {
            var model = new MembersRowsViewModel
            {
                SelectedColumns = UsersPresentationHelper.GetProfileColumns(),
                CurrentMember = _intranetMemberService.GetCurrentMember().ToViewModel(),
            };

            model.IsCurrentMemberGroupAdmin = groupId.HasValue && _groupMemberService
                .IsMemberAdminOfGroup(model.CurrentMember.Id, groupId.Value);

            model.GroupId = groupId;

            return model;
        }

        private MemberModel MapToViewModel(IIntranetMember user, Guid? groupId)
        {
            var model = user.Map<MemberModel>();
            model.ProfileUrl = _profileLinkProvider.GetProfileLink(user.Id);

            var isAdmin = groupId.HasValue && _groupMemberService
                .IsMemberAdminOfGroup(user.Id, groupId.Value);

            model.IsGroupAdmin = isAdmin;
            model.IsCreator = groupId.HasValue && _groupService.IsMemberCreator(user.Id, groupId.Value);

            return model;
        }
    }
}