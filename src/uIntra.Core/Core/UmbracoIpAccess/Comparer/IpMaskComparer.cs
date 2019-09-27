using System;
using System.Net;

namespace Uintra.Core.UmbracoIpAccess
{
    public static class IpMaskComparer
    {
        public static bool IsInSameSubnet(string ip, string network, string subnetMask)
        {
            var mask = IPAddress.Parse(subnetMask);

            var network1 = GetNetworkAddress(IPAddress.Parse(network), mask);
            var network2 = GetNetworkAddress(IPAddress.Parse(ip), mask);

            return network1.Equals(network2);
        }

        private static IPAddress GetNetworkAddress(IPAddress address, IPAddress subnetMask)
        {
            var addressBytes = address.GetAddressBytes();
            var subnetMaskBytes = subnetMask.GetAddressBytes();

            if (addressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            var broadcastAddress = new byte[addressBytes.Length];
            for (var i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(addressBytes[i] & (subnetMaskBytes[i]));
            }

            return new IPAddress(broadcastAddress);
        }
    }
}