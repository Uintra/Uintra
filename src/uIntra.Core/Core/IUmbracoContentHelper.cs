using System;
using Umbraco.Core.Models;

namespace uIntra.Core
{
    public interface IUmbracoContentHelper
    {
        bool IsContentAvailable(IPublishedContent publishedContent);
        bool IsForContentPage(Guid id);
    }
}