using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Compent.Shared.Extensions.Bcl;
using UBaseline.Core.Controllers;
using Uintra.Core.Search.Entities;
using Uintra.Core.Search.Helpers;
using Uintra.Core.Member.Abstractions;
using Uintra.Core.Member.Entities;
using Uintra.Core.Member.Services;
using Uintra.Core.Search.Queries;
using Uintra.Core.Search.Repository;
using Uintra.Features.Groups.Models;
using Uintra.Features.Groups.Services;
using Uintra.Features.Links;
using Uintra.Features.Notification;
using Uintra.Features.Notification.Configuration;
using Uintra.Features.Notification.Entities.Base;
using Uintra.Features.Notification.Services;
using Uintra.Features.UserList.Helpers;
using Uintra.Features.UserList.Models;
using Uintra.Infrastructure.Extensions;
using Uintra.Infrastructure.Helpers;
using EnumerableExtensions = Compent.Extensions.EnumerableExtensions;

namespace Uintra.Features.UserList.Controllers
{
    public class UserListController : UBaselineApiController
    {
        private const int AmountPerRequest = 10;

        private readonly IProfileLinkProvider _profileLinkProvider;
        private readonly IGroupService _groupService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IGroupMemberService _groupMemberService;
        private readonly INotificationsService _notificationsService;
        private readonly INotifierDataHelper _notifierDataHelper;
        private readonly IUintraSearchRepository<SearchableMember> _uintraSearchRepository;

        public UserListController(
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IProfileLinkProvider profileLinkProvider,
            IGroupService groupService,
            IGroupMemberService groupMemberService,
            INotificationsService notificationsService,
            INotifierDataHelper notifierDataHelper, 
            IUintraSearchRepository<SearchableMember> uintraSearchRepository)
        {
            _profileLinkProvider = profileLinkProvider;
            _groupService = groupService;
            _intranetMemberService = intranetMemberService;
            _groupMemberService = groupMemberService;
            _notificationsService = notificationsService;
            _notifierDataHelper = notifierDataHelper;
            _uintraSearchRepository = uintraSearchRepository;
        }

        [HttpPost]
        public virtual MembersRowsViewModel GetUsers([FromBody] MembersListSearchModel listSearch)
        {
            var (activeUsers, isLastRequest) =
                GetActiveUsers(listSearch.Map<ActiveMemberSearchQuery>(), listSearch.GroupId);

            var model = GetUsersRowsViewModel(listSearch.GroupId);

            model.SelectedColumns =
                UsersPresentationHelper.ExtendIfGroupMembersPage(listSearch.GroupId,
                    UsersPresentationHelper.GetProfileColumns());
            model.Members = activeUsers;
            model.IsLastRequest = isLastRequest;

            return model;
        }

        [HttpPost]
        public virtual MembersRowsViewModel ForInvitation([FromBody] MembersListSearchModel listSearch)
        {
            var (activeUsers, isLastRequest) =
                GetActiveUsers(listSearch.Map<ActiveMemberSearchQuery>(), listSearch.GroupId);

            var model = GetUsersRowsViewModel(listSearch.GroupId);

            model.SelectedColumns =
                UsersPresentationHelper.AddManagementColumn(UsersPresentationHelper.GetProfileColumns());
            model.Members = activeUsers;
            model.IsLastRequest = isLastRequest;
            model.IsInvite = listSearch.IsInvite;
            return model;
        }

        [HttpDelete]
        public async Task<IHttpActionResult> ExcludeUserFromGroup(Guid groupId, Guid userId)
        {
            var currentMember = _intranetMemberService.GetCurrentMember();

            if (currentMember == null)
            {
                return NotFound();
            }

            var group = _groupService.Get(groupId);
            var isAdmin = _groupMemberService.IsMemberAdminOfGroup(currentMember.Id, groupId);

            if (!isAdmin && currentMember.Id != userId && group.CreatorId != userId)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            await _groupMemberService.RemoveAsync(groupId, userId);
            return Ok();
        }


        [HttpPut]
        public IHttpActionResult Assign(GroupToggleAdminRightsModel rights)
        {
            _groupMemberService.ToggleAdminRights(rights.MemberId, rights.GroupId);

            return Ok();
        }

        [HttpPost]
        public async Task<IHttpActionResult> InviteMember(MemberGroupInviteModel invite)
        {
            await InviteUser(invite);
            SendInvitationToUser(invite);

            return Ok();
        }

        private async Task InviteUser(MemberGroupInviteModel invite) =>
            await _groupMemberService.AddAsync(invite.GroupId, new GroupMemberSubscriptionModel
            {
                MemberId = invite.MemberId
            });

        private void SendInvitationToUser(MemberGroupInviteModel invite) =>
            _notificationsService.ProcessNotification(new NotifierData
            {
                NotificationType = NotificationTypeEnum.GroupInvitation,
                ReceiverIds = EnumerableExtensions.ToEnumerable(invite.MemberId),
                ActivityType = CommunicationTypeEnum.CommunicationSettings,
                Value = _notifierDataHelper.GetGroupInvitationDataModel(NotificationTypeEnum.GroupInvitation,
                    invite.GroupId, invite.MemberId,
                    _intranetMemberService.GetCurrentMember().Id)
            });


        private (IEnumerable<MemberModel> result, bool isLastRequest) GetActiveUsers(ActiveMemberSearchQuery query,
            Guid? groupId)
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

            var searchQuery = new SearchByMemberQuery()//MemberSearchQuery
            {
                Text = query.Text,
                Skip = skip,
                Take = AmountPerRequest,
                OrderingString = ElasticHelpers.FullName,
                SearchableTypeIds = EnumerableExtensions.ToEnumerable(((int) UintraSearchableTypeEnum.Member)),
                GroupId = query.GroupId,
                MembersOfGroup = query.MembersOfGroup
            };

            //var searchResult = _elasticIndex.Search(searchQuery);
            var searchResult = AsyncHelpers.RunSync(() => _uintraSearchRepository.SearchAsync(searchQuery, String.Empty));
            var result = searchResult.Documents.Select(r => Guid.Parse(r.Id.ToString()));

            return (result, searchResult.TotalCount);
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