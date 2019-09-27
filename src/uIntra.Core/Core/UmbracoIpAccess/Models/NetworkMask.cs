namespace Uintra.Core.UmbracoIpAccess
{
    public class NetworkMask
    {
        private NetworkMask(string network, string mask)
        {
            Network = network;
            Mask = mask;
        }

        public string Network { get; }
        public string Mask { get; }

        public static NetworkMask Of(string network, string mask) =>
            new NetworkMask(network, mask);
    }
}