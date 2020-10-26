using System.Net;
using UBaseline.Shared.Node;
using Uintra20.Features.Links.Models;

namespace Uintra20.Core.UbaselineModels.RestrictedNode
{
    public class UintraRestrictedNodeViewModel : NodeViewModel
    {
        public bool RequiresRedirect => StatusCode != HttpStatusCode.OK;
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public UintraLinkModel ErrorLink { get; set; }
    }
}