using System.Collections.Generic;
using Umbraco.Core.Models;

namespace uCommunity.Navigation.Core
{
    public interface ISystemLinksService
    {
        IEnumerable<IPublishedContent> GetMany(string xPath);
    }
}