using System.Collections.Generic;
using System.Threading.Tasks;
using Uintra20.Features.Navigation.Models.MyLinks;

namespace Uintra20.Features.Navigation
{
    public interface IMyLinksHelper
    {
        IEnumerable<MyLinkItemModel> GetMenu();
        Task<IEnumerable<MyLinkItemModel>> GetMenuAsync();
        bool IsActivityLink(int contentId);
        bool IsGroupPage(int contentId);
    }
}