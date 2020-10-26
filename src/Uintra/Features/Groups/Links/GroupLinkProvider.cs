using System;
using Uintra.Features.Groups.ContentServices;
using Uintra.Features.Groups.Models;
using Uintra.Features.Links.Models;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.Groups.Links
{
    public class GroupLinkProvider : IGroupLinkProvider
    {
        private readonly IGroupContentProvider _contentProvider;

        public GroupLinkProvider(IGroupContentProvider contentProvider)
        {
            _contentProvider = contentProvider;
        }

        public UintraLinkModel GetGroupCreateLink()
        {
            return _contentProvider.GetGroupCreatePage()?.Url?.ToLinkModel();
        }

        public UintraLinkModel GetMyGroupsLink()
        {
            return _contentProvider.GetMyGroupsPage()?.Url?.ToLinkModel();
        }

        public UintraLinkModel GetGroupRoomLink(Guid id)
        {
            return _contentProvider.GetGroupRoomPage()?.Url.AddGroupId(id)?.ToLinkModel();
        }

        public UintraLinkModel GetEditLink(Guid id)
        {
            return _contentProvider.GetGroupEditPage()?.Url.AddGroupId(id)?.ToLinkModel();
        }

        public UintraLinkModel GetGroupDocumentsLink(Guid id)
        {
            return _contentProvider.GetGroupDocumentsPage()?.Url.AddGroupId(id)?.ToLinkModel();
        }

        public UintraLinkModel GetGroupMembersLink(Guid id)
        {
            return _contentProvider.GetGroupMembersPage()?.Url.AddGroupId(id)?.ToLinkModel();
        }

        UintraLinkModel IGroupLinkProvider.GetGroupsOverviewLink()
        {
            return _contentProvider.GetGroupsOverviewPage()?.Url?.ToLinkModel();
        }

        public GroupLinksModel GetGroupLinks(Guid id, bool canEdit)
        {
            return new GroupLinksModel
            {
                GroupDocumentsPage = _contentProvider.GetGroupDocumentsPage()?.Url.AddGroupId(id)?.ToLinkModel(),
                GroupRoomPage = _contentProvider.GetGroupRoomPage()?.Url.AddGroupId(id)?.ToLinkModel(),
                GroupMembersPage = _contentProvider.GetGroupMembersPage()?.Url.AddGroupId(id)?.ToLinkModel(),
                GroupEditPage = canEdit ? _contentProvider.GetGroupEditPage()?.Url.AddGroupId(id)?.ToLinkModel() : null
            };
        }
    }
}