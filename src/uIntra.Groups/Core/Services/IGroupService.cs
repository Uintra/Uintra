using System;
using System.Collections.Generic;
using Umbraco.Core.Models;

namespace Uintra.Groups
{
    public interface IGroupService
    {
        Guid Create(GroupModel groupModel);
        void Edit(GroupModel groupModel);
        GroupModel Get(Guid id);
        IEnumerable<GroupModel> GetAllNotHidden();
        IEnumerable<GroupModel> GetMany(IEnumerable<Guid> groupIds);
        IEnumerable<GroupModel> GetAll();
        bool CanHide(Guid id);
        bool CanHide(GroupModel group);
        bool CanEdit(Guid id);
        bool CanEdit(GroupModel group);
        bool ValidatePermission(IPublishedContent content);
        bool IsActivityFromActiveGroup(IGroupActivity groupActivity);
        bool IsMemberCreator(Guid memberId, Guid groupId);
        void Hide(Guid id);
        void Unhide(Guid id);
    }
}
