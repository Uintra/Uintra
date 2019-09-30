using Compent.Shared.Extensions.Bcl;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Uintra.Core.UmbracoIpAccess
{
    public class UmbracoIpAccessConfiguration : ConfigurationSection, IUmbracoIpAccessConfiguration
    {
        private const string StatusCodeKey = "statusCode";
        private const string IpsKey = "ips";
        private const string DomainsKey = "domains";
        private const string NetworkMasksKey = "networkMasks";

        private IEnumerable<Range<string>> ips;
        private IEnumerable<string> domains;
        private IEnumerable<NetworkMask> networkMasks;
        public static UmbracoIpAccessConfiguration Configuration => ConfigurationManager.GetSection("umbracoIpAccessConfiguration") as UmbracoIpAccessConfiguration;

        [ConfigurationProperty(StatusCodeKey, DefaultValue = default)]
        public int StatusCode => (int)base[StatusCodeKey];

        [ConfigurationProperty(IpsKey, DefaultValue = "")]
        public string IpsStr => (string)base[IpsKey];

        [ConfigurationProperty(DomainsKey, DefaultValue = "")]
        public string DomainsStr => (string)base[DomainsKey];

        [ConfigurationProperty(NetworkMasksKey, DefaultValue = "")]
        public string NetworkMasksStr => (string)base[NetworkMasksKey];

        public IEnumerable<Range<string>> Ips => ips ?? (ips = GetAllowedIps().ToList());

        public IEnumerable<string> Domains => domains ?? (domains = Separate(DomainsStr).ToArray());

        public IEnumerable<NetworkMask> NetworkMasks => networkMasks ?? (networkMasks = GetNetworkMasks(NetworkMasksStr).ToArray());

        private static IEnumerable<NetworkMask> GetNetworkMasks(string networkMasksStr)
        {
            return Separate(networkMasksStr).Select(x => x.SplitBy("_"))
                .Select(m => NetworkMask.Of(m.ElementAt(0).Trim(), m.ElementAt(1).Trim()));
        }

        private IEnumerable<Range<string>> GetAllowedIps()
        {
            if (IpsStr.IsEmpty())
            {
                yield break;
            }

            foreach (var allowIpStr in Separate(IpsStr))
            {
                var parts = allowIpStr.SplitBy("-").Select(x => x.Trim()).ToList();
                if (parts.Count == 1)
                {
                    yield return new Range<string>(parts[0], parts[0]);
                    continue;
                }

                yield return new Range<string>(parts[0], parts[1]);
            }
        }

        private static IEnumerable<string> Separate(string str) => str.SplitBy(";").Select(x => x.Trim());
    }
}