using UBaseline.Core.RequestContext;
using UBaseline.Shared.Domain;
using UBaseline.Shared.Node;

namespace Uintra20.Infrastructure.Context
{
    public interface IIntranetRequestContext : IUBaselineRequestContext
    {
        INodeModel RequestedNode { get; }
        INodeModel RequestedOrReferredNode { get; }
        IDomainModel RequestedDomain { get; }
    }
}
