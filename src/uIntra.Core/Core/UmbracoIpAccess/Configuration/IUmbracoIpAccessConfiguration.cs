using System.Collections.Generic;

namespace Uintra.Core.UmbracoIpAccess
{
    public interface IUmbracoIpAccessConfiguration
    {
        int StatusCode { get; }

        IEnumerable<Range<string>> Ips { get; }

        IEnumerable<string> Domains { get; }

        IEnumerable<NetworkMask> NetworkMasks { get; }
    }
}