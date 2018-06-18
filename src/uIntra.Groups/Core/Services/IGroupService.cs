using System;
using System.Collections.Generic;
using uIntra.Core.User;

namespace uIntra.Groups
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

        bool CanEdit(Guid groupId, IIntranetUser user);
        bool CanEdit(GroupModel groupModel, IIntranetUser user);

        void Hide(Guid id);
        void UpdateGroupUpdateDate(Guid id);
        void Unhide(Guid id);
    }
}
