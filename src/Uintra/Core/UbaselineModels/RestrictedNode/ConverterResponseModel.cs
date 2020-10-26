using System.Net;
using Uintra.Features.Links.Models;

namespace Uintra.Core.UbaselineModels.RestrictedNode
{
    public class ConverterResponseModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public UintraLinkModel Link { get; set; }
    }
}