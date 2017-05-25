using System.Collections.Generic;
using Umbraco.Core.Models;

namespace uIntra.Navigation.Core.Services
{
    public interface ISystemLinksService
    {
        IEnumerable<IPublishedContent> GetMany(string xPath);
    }
}