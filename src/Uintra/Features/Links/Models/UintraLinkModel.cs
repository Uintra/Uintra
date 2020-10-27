using System.Collections.Generic;
using System.Linq;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.Links.Models
{
    public class UintraLinkModel
    {
        public UintraLinkModel(string originalUrl)
        {
            OriginalUrl = originalUrl;
        }
        public string OriginalUrl { get; set; }
        public string BaseUrl { get; set; }
        public IEnumerable<UintraLinkParamModel> Params { get; set; } = Enumerable.Empty<UintraLinkParamModel>();

        public override string ToString()
        {
            if (Params.Any())
            {
                return $"{BaseUrl.TrimEnd('/', '?')}?" + Params.JoinToString("&");
            }

            return BaseUrl;
        }
    }
}