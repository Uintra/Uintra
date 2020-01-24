using System.Collections.Generic;
using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Features.Navigation.Services
{
    public interface ISystemLinksService
    {
        IEnumerable<IPublishedContent> GetMany(IEnumerable<string> aliasPath);
    }
}