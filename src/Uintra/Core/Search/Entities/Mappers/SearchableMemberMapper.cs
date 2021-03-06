﻿using System.Collections.Generic;
using System.Linq;
using Uintra.Core.User;
using Uintra.Features.Groups;
using Uintra.Features.Groups.Services;
using Uintra.Features.Tagging.UserTags.Services;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Core.Search.Entities.Mappers
{
	public class SearchableMemberMapper<T> : ISearchableMemberMapper<T> where T : SearchableMember
	{
		private readonly IIntranetUserContentProvider _intranetUserContentProvider;
		private readonly IUserTagService _userTagService;
		private readonly IGroupService _groupService;
		private readonly IGroupMemberService _groupMemberService;

		public SearchableMemberMapper(
			IIntranetUserContentProvider intranetUserContentProvider,
			IUserTagService userTagService,
			IGroupService groupService,
			IGroupMemberService groupMemberService)
		{
			_intranetUserContentProvider = intranetUserContentProvider;
			_userTagService = userTagService;
			_groupService = groupService;
			_groupMemberService = groupMemberService;
		} 
		public virtual T Map(IGroupMember member)
		{
			var searchableUser = member.Map<T>();
			searchableUser.Url = _intranetUserContentProvider.GetProfilePage().Url.AddIdParameter(member.Id).ToLinkModel();
			searchableUser.UserTagNames = _userTagService.Get(member.Id).Select(t => t.Text).ToList();
			searchableUser.Groups = FillGroupInfo(member);

			return searchableUser;
		}
		protected virtual IEnumerable<SearchableMemberGroupInfo> FillGroupInfo(IGroupMember user)
		{
			var userGroups = _groupService.GetMany(user.GroupIds);

			return userGroups.Select(ug =>
			{
				var isCreator = ug.CreatorId == user.Id;
				var groupInfo = new SearchableMemberGroupInfo()
				{
					GroupId = ug.Id,
					IsCreator = isCreator,
					IsAdmin = isCreator || _groupMemberService.IsMemberAdminOfGroup(user.Id, ug.Id)
				};

				return groupInfo;
			});
		}
	}
}