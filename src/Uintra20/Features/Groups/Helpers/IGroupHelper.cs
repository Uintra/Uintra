using System;
using System.Threading.Tasks;
using Uintra20.Features.Groups.Models;

namespace Uintra20.Features.Groups.Helpers
{
    public interface IGroupHelper
    {
        GroupHeaderViewModel GetHeader(Guid groupId);
        Task<GroupHeaderViewModel> GetHeaderAsync(Guid groupId);
        GroupViewModel GetGroupViewModel(Guid groupId);
        Task<GroupViewModel> GetGroupViewModelAsync(Guid groupId);
        GroupInfoViewModel GetInfoViewModel(Guid groupId);
        Task<GroupInfoViewModel> GetInfoViewModelAsync(Guid groupId);
        GroupLeftNavigationMenuViewModel GroupNavigation();
    }
}
