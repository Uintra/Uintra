using System;
using Uintra20.Features.OpenGraph.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Features.OpenGraph.Services.Contracts
{
    public interface IOpenGraphService
    {
        OpenGraphObject GetOpenGraphObject(IPublishedContent content, string defaultUrl = null);
        OpenGraphObject GetOpenGraphObject(Guid activityId, string defaultUrl = null);
        OpenGraphObject GetOpenGraphObject(string url);
    }
}