using System;
using UBaseline.Shared.Node;
using Uintra.Features.OpenGraph.Models;

namespace Uintra.Features.OpenGraph.Services.Contracts
{
    public interface IOpenGraphService
    {
        OpenGraphObject GetOpenGraphObject(INodeModel nodeModel, string defaultUrl = null);
        OpenGraphObject GetOpenGraphObject(Guid activityId, string defaultUrl = null);
        OpenGraphObject GetOpenGraphObject(string url);
    }
}