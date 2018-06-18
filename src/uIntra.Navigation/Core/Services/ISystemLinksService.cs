using System.Collections.Generic;
using Umbraco.Core.Models;

namespace Uintra.Navigation
{
    public interface ISystemLinksService
    {
        IEnumerable<IPublishedContent> GetMany(string xPath);
    }
}