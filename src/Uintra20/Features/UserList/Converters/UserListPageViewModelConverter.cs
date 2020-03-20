using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Compent.Extensions;
using UBaseline.Core.Node;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Core.Search.Entities;
using Uintra20.Core.Search.Indexes;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Search.Queries;
using Uintra20.Features.UserList.Helpers;
using Uintra20.Features.UserList.Models;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.UserList.Converters
{
    public class UserListPageViewModelConverter : INodeViewModelConverter<UserListPageModel, UserListPageViewModel>
    {
        private const int AmountPerRequest = 10;

        private readonly IElasticMemberIndex<SearchableMember> _elasticIndex;
        private readonly IProfileLinkProvider _profileLinkProvider;
        private readonly IGroupService _groupService;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly IGroupMemberService _groupMemberService;

        public UserListPageViewModelConverter(
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IElasticMemberIndex<SearchableMember> elasticIndex,
            IProfileLinkProvider profileLinkProvider,
            IGroupService groupService,
            IGroupMemberService groupMemberService)
        {
            _elasticIndex = elasticIndex;
            _profileLinkProvider = profileLinkProvider;
            _groupService = groupService;
            _intranetMemberService = intranetMemberService;
            _groupMemberService = groupMemberService;
        }

        public void Map(UserListPageModel node, UserListPageViewModel viewModel)
        {
            Guid? groupId = null;

            var groupIdStr = HttpContext.Current.Request["groupId"];
            if (Guid.TryParse(groupIdStr, out var parsedGroupId))
                groupId = parsedGroupId;

            viewModel.Details = GetUsers(groupId);
        }

        public virtual MembersRowsViewModel GetUsers(Guid? groupId)
        {
            var listSearch = new ActiveMemberSearchQuery
            {
                GroupId = groupId,
                OrderingString = string.Empty,
                Text = string.Empty,
                Page = 1
            };

            var (activeUsers, isLastRequest) = GetActiveUsers(listSearch, groupId);

            var model = GetUsersRowsViewModel(groupId);

            model.SelectedColumns = UsersPresentationHelper.ExtendIfGroupMembersPage(listSearch.GroupId, UsersPresentationHelper.GetProfileColumns());
            model.Members = activeUsers;
            model.IsLastRequest = isLastRequest;

            return model;
        }

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