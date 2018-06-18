using System.Collections.Generic;
using Umbraco.Core.Models;

namespace uIntra.Navigation
{
    public interface ISystemLinksService
    {
        IEnumerable<IPublishedContent> GetMany(string xPath);
    }
}