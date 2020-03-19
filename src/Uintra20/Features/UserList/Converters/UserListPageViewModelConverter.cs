using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Compent.Extensions;
using Localization.Core;
using UBaseline.Core.Node;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Core.Search.Entities;
using Uintra20.Core.Search.Indexes;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Notification.Services;
using Uintra20.Features.Search.Queries;
using Uintra20.Features.UserList.Helpers;
using Uintra20.Features.UserList.Models;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.Helpers;

namespace Uintra20.Features.UserList.Converters
{
    public class UserListPageViewModelConverter : INodeViewModelConverter<UserListPageModel, UserListPageViewModel>
    {
        private const int AmountPerRequest = 10;

        private readonly IElasticMemberIndex<SearchableMember> _elasticIndex;
        private readonly ILocalizationCoreService _localizationCoreService;
        private readonly IProfileLinkProvider _profileLinkProvider;
        private readonly IGroupService _groupService;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly IGroupMemberService _groupMemberService;
        private readonly INotificationsService _notificationsService;
        private readonly INotifierDataHelper _notifierDataHelper;

        public UserListPageViewModelConverter(
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IElasticMemberIndex<SearchableMember> elasticIndex,
            ILocalizationCoreService localizationCoreService,
            IProfileLinkProvider profileLinkProvider,
            IGroupService groupService,
            IGroupMemberService groupMemberService,
            INotificationsService notificationsService,
            INotifierDataHelper notifierDataHelper)
        {
            _elasticIndex = elasticIndex;
            _localizationCoreService = localizationCoreService;
            _profileLinkProvider = profileLinkProvider;
            _groupService = groupService;
            _intranetMemberService = intranetMemberService;
            _groupMemberService = groupMemberService;
            _notificationsService = notificationsService;
            _notifierDataHelper = notifierDataHelper;
        }

        public void Map(UserListPageModel node, UserListPageViewModel viewModel)
        {
            Guid? groupId = null;

            var groupIdStr = HttpContext.Current.Request["groupId"];
            if (Guid.TryParse(groupIdStr, out var parsedGroupId))
                groupId = parsedGroupId;

            viewModel.Details = Render(groupId);
        }

        public virtual UserListViewModel Render(Guid? groupId)
        {
            var selectedColumns = UsersPresentationHelper.GetProfileColumns().ToArray();

            var orderByColumn = selectedColumns.FirstOrDefault(i => i.SupportSorting);

            var query = new ActiveMemberSearchQuery
            {
                Text = string.Empty,
                Skip = 0,
                Take = AmountPerRequest,
                OrderingString = orderByColumn?.PropertyName,
                GroupId = groupId,
                MembersOfGroup = groupId.HasValue
            };

            var (activeUsers, isLastRequest) = GetActiveUsers(query, groupId);

            var viewModel = new UserListViewModel
            {
                AmountPerRequest = AmountPerRequest,
                Title = null,
                MembersRows = GetUsersRowsViewModel(groupId),
                OrderByColumn = orderByColumn
            };
            viewModel.MembersRows.SelectedColumns = UsersPresentationHelper.ExtendIfGroupMembersPage(groupId, selectedColumns);
            viewModel.MembersRows.Members = activeUsers;
            viewModel.IsLastRequest = isLastRequest;

            return viewModel;
        }

        private (IEnumerable<MemberModel> result, bool isLastRequest) GetActiveUsers(ActiveMemberSearchQuery query, Guid? groupId)
        {
            var (searchResult, totalHits) = GetActiveUserIds(query);

            var result = _intranetMemberService.GetMany(searchResult)
                .Select(x => MapToViewModel(x, groupId));

            var isLastRequest = query.Skip + query.Take >= totalHits;

            return (result, isLastRequest);
        }

        private (IEnumerable<Guid> searchResult, long totalHits) GetActiveUserIds(
            ActiveMemberSearchQuery query)
        {
            var searchQuery = new MemberSearchQuery
            {
                Text = query.Text,
                Skip = query.Skip,
                Take = query.Take,
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
                CurrentMember = _intranetMemberService.GetCurrentMember().Map<MemberViewModel>(),
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