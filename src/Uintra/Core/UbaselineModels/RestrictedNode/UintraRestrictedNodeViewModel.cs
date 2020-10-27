using System.Net;
using UBaseline.Shared.Node;
using Uintra.Features.Links.Models;

namespace Uintra.Core.UbaselineModels.RestrictedNode
{
    public class UintraRestrictedNodeViewModel : NodeViewModel
    {
        public bool RequiresRedirect => StatusCode != HttpStatusCode.OK;
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public UintraLinkModel ErrorLink { get; set; }
    }
}