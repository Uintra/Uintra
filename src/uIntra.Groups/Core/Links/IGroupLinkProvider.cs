using System;

namespace Uintra.Groups
{
    public interface IGroupLinkProvider
    {
        string GetGroupLink(Guid groupId);
        string GetDeactivatedGroupLink(Guid groupId);

        string GetGroupsOverviewLink();
        string GetCreateGroupLink();
    }
}