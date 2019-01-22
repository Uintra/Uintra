using System;
using System.Collections.Generic;
using Uintra.Core.User;

namespace Uintra.Groups
{
    public interface IGroupService
    {
        Guid Create(GroupModel groupModel);
        void Edit(GroupModel groupModel);
        GroupModel Get(Guid id);        
        IEnumerable<GroupModel> GetAllNotHidden();              
        IEnumerable<GroupModel> GetMany(IEnumerable<Guid> groupIds);
        IEnumerable<GroupModel> GetAllHided();
        IEnumerable<GroupModel> GetAll();

        bool CanEdit(Guid groupId, IIntranetMember member);
        bool CanEdit(GroupModel groupModel, IIntranetMember member);
        bool IsActivityFromActiveGroup(IGroupActivity groupActivity);

        void Hide(Guid id);
        void UpdateGroupUpdateDate(Guid id);
        void Unhide(Guid id);
    }
}
