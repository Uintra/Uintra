using System.Collections.Generic;
using System.Linq;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Links.Models
{
    public class UintraLinkModel
    {
        public UintraLinkModel(string originalUrl)
        {
            OriginalUrl = originalUrl;
        }
        public string OriginalUrl { get; set; }
        public string BaseUrl { get; set; }
        public IEnumerable<UintraLinkParamModel> Params { get; set; }

        public override string ToString()
        {
            if (Params.Any())
            {
                return $"{BaseUrl}?" + Params.JoinToString("&");
            }

            return BaseUrl;
        }
    }
}