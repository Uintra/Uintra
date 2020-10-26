using System;
using Uintra.Features.Groups.Models;
using Uintra.Features.Links.Models;

namespace Uintra.Features.Groups.Links
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