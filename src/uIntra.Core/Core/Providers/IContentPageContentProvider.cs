using System.Collections.Generic;
using Umbraco.Core.Models;

namespace Uintra.Core.Providers
{
    public interface IContentPageContentProvider
    {
        IEnumerable<IPublishedContent> GetAllContentPages();
        IPublishedContent GetFirstUserListContentPage();
    }
}
