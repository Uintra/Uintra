using System;

namespace Uintra.Groups
{
    public interface IGroupActivityService
    {
        void AddRelation(Guid groupId, Guid activityId);
        void RemoveRelation(Guid groupId, Guid activityId);
        Guid? GetGroupId(Guid activityId);
    }
}