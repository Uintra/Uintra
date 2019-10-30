using System;
using Uintra.Core.OpenGraph.Models;
using Umbraco.Core.Models;

namespace Uintra.Core.OpenGraph.Services
{
    public interface IOpenGraphService
    {
        OpenGraphObject GetOpenGraphObject(IPublishedContent content, string defaultUrl = null);
        OpenGraphObject GetOpenGraphObject(Guid activityId, string defaultUrl = null);
        OpenGraphObject GetOpenGraphObject(string url);
    }
}
