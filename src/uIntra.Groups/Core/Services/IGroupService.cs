using System;
using System.Collections.Generic;
using Uintra.Core.User;
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

        bool CanEdit(Guid groupId);
        bool CanEdit(GroupModel groupModel);
        bool ValidatePermission(IPublishedContent content);
        bool IsActivityFromActiveGroup(IGroupActivity groupActivity);

        void Hide(Guid id);
        void Unhide(Guid id);
    }
}
