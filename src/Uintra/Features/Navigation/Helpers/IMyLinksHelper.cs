using System.Collections.Generic;
using System.Threading.Tasks;
using Uintra.Features.Navigation.Models.MyLinks;

namespace Uintra.Features.Navigation.Helpers
{
    public interface IMyLinksHelper
    {
        IEnumerable<MyLinkItemModel> GetMenu();
        Task<IEnumerable<MyLinkItemModel>> GetMenuAsync();
        bool IsActivityLink(int contentId);
        bool IsGroupPage(int contentId);
    }
}