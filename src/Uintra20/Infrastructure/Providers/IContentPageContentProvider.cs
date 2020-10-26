using System.Collections.Generic;
using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Infrastructure.Providers
{
    public interface IContentPageContentProvider
    {
        IEnumerable<IPublishedContent> GetAllContentPages();
        IPublishedContent GetFirstUserListContentPage();
        IPublishedContent GetUserListContentPageFromPicker();
    }
}
