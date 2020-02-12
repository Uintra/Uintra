using System;

namespace Uintra20.Features.Groups.Links
{
    public interface IGroupLinkProvider
    {
        string GetGroupLink(Guid groupId);
        string GetDeactivatedGroupLink(Guid groupId);

        string GetGroupsOverviewLink();
        string GetCreateGroupLink();
    }
}