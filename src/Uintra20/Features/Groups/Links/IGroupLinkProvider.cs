using System;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Links.Models;

namespace Uintra20.Features.Groups.Links
{
    public interface IGroupLinkProvider
    {
        UintraLinkModel GetGroupsOverviewLink();
        UintraLinkModel GetGroupCreateLink();
        UintraLinkModel GetMyGroupsLink();
        UintraLinkModel GetGroupRoomLink(Guid id);
        UintraLinkModel GetEditLink(Guid id);
        UintraLinkModel GetGroupDocumentsLink(Guid id);
        UintraLinkModel GetGroupMembersLink(Guid id);
        GroupLinksModel GetGroupLinks(Guid id, bool canEdit);
    }
}