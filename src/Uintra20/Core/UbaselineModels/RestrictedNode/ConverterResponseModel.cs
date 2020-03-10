using System.Net;
using Uintra20.Features.Links.Models;

namespace Uintra20.Core.UbaselineModels.RestrictedNode
{
    public class ConverterResponseModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public UintraLinkModel Link { get; set; }
    }
}