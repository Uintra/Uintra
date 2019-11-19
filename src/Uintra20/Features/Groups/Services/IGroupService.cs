using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uintra20.Features.Groups.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Features.Groups.Services
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

        Task<Guid> CreateAsync(GroupModel groupModel);
        Task EditAsync(GroupModel groupModel);
        Task<GroupModel> GetAsync(Guid id);
        Task<IEnumerable<GroupModel>> GetAllNotHiddenAsync();
        Task<IEnumerable<GroupModel>> GetManyAsync(IEnumerable<Guid> groupIds);
        Task<IEnumerable<GroupModel>> GetAllAsync();
        Task<bool> CanHideAsync(Guid id);
        Task<bool> CanHideAsync(GroupModel group);
        Task<bool> CanEditAsync(Guid id);
        Task<bool> CanEditAsync(GroupModel group);
        Task<bool> ValidatePermissionAsync(IPublishedContent content);
        Task<bool> IsActivityFromActiveGroupAsync(IGroupActivity groupActivity);
        Task<bool> IsMemberCreatorAsync(Guid memberId, Guid groupId);
        Task HideAsync(Guid id);
        Task UnhideAsync(Guid id);
    }
}
