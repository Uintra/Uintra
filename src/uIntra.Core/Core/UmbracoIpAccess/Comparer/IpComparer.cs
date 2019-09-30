namespace Uintra.Core.UmbracoIpAccess
{
    public class IpComparer
    {
        /// <summary>
        /// Compares two IP addresses for equality. 
        /// </summary>
        /// <param name="ipAddr1">The first IP to compare</param>
        /// <param name="ipAddr2">The second IP to compare</param>
        /// <returns>True if equal, false if not.</returns>
        internal static bool AreEqual(string ipAddr1, string ipAddr2)
        {
            // convert to long in case there is any zero padding in the strings
            return IpAddressToLongBackwards(ipAddr1) == IpAddressToLongBackwards(ipAddr2);
        }

        /// <summary>
        /// Compares two string representations of an Ip address to see if one
        /// is greater than the other
        /// </summary>
        /// <param name="toCompare">The IP address on the left hand side of the greater 
        /// than operator</param>
        /// <param name="compareAgainst">The Ip address on the right hand side of the 
        /// greater than operator</param>
        /// <returns>True if ToCompare is greater than CompareAgainst, else false</returns>
        public static bool IsGreater(string toCompare, string compareAgainst)
        {
            // convert to long in case there is any zero padding in the strings
            return IpAddressToLongBackwards(toCompare) > IpAddressToLongBackwards(compareAgainst);
        }

        /// <summary>
        /// Compares two string representations of an Ip address to see if one
        /// is less than the other
        /// </summary>
        /// <param name="toCompare">The IP address on the left hand side of the less 
        /// than operator</param>
        /// <param name="compareAgainst">The Ip address on the right hand side of the 
        /// less than operator</param>
        /// <returns>True if ToCompare is greater than CompareAgainst, else false</returns>
        public static bool IsLess(string toCompare, string compareAgainst)
        {
            // convert to long in case there is any zero padding in the strings
            return IpAddressToLongBackwards(toCompare) < IpAddressToLongBackwards(compareAgainst);
        }


        /// <summary>
        /// Compares two string representations of an Ip address to see if one
        /// is greater than or equal to the other.
        /// </summary>
        /// <param name="toCompare">The IP address on the left hand side of the greater 
        /// than or equal operator</param>
        /// <param name="compareAgainst">The Ip address on the right hand side of the 
        /// greater than or equal operator</param>
        /// <returns>True if ToCompare is greater than or equal to CompareAgainst, else false</returns>
        public static bool IsGreaterOrEqual(string toCompare, string compareAgainst)
        {
            // convert to long in case there is any zero padding in the strings
            return IpAddressToLongBackwards(toCompare) >= IpAddressToLongBackwards(compareAgainst);
        }

        /// <summary>
        /// Compares two string representations of an Ip address to see if one
        /// is less than or equal to the other.
        /// </summary>
        /// <param name="toCompare">The IP address on the left hand side of the less 
        /// than or equal operator</param>
        /// <param name="compareAgainst">The Ip address on the right hand side of the 
        /// less than or equal operator</param>
        /// <returns>True if ToCompare is greater than or equal to CompareAgainst, else false</returns>
        public static bool IsLessOrEqual(string toCompare, string compareAgainst)
        {
            // convert to long in case there is any zero padding in the strings
            return IpAddressToLongBackwards(toCompare) <= IpAddressToLongBackwards(compareAgainst);
        }

        /// <summary>
        /// Converts a uint representation of an Ip address to a string.
        /// </summary>
        /// <param name="ipAddr">The IP address to convert</param>
        /// <returns>A string representation of the IP address.</returns>
        public static string LongToIpAddress(uint ipAddr)
        {
            return new System.Net.IPAddress(ipAddr).ToString();
        }

        /// <summary>
        /// Converts a string representation of an IP address to a uint. This
        /// encoding is proper and can be used with other networking functions such
        /// as the System.Net.IPAddress class.
        /// </summary>
        /// <param name="ipAddr">The Ip address to convert.</param>
        /// <returns>Returns a uint representation of the IP address.</returns>
        public static uint IpAddressToLong(string ipAddr)
        {
            System.Net.IPAddress oIp = System.Net.IPAddress.Parse(ipAddr);
            byte[] byteIp = oIp.GetAddressBytes();


            uint ip = (uint)byteIp[3] << 24;
            ip += (uint)byteIp[2] << 16;
            ip += (uint)byteIp[1] << 8;
            ip += (uint)byteIp[0];

            return ip;
        }

        /// <summary>
        /// This encodes the string representation of an IP address to a uint, but
        /// backwards so that it can be used to compare addresses. This function is
        /// used internally for comparison and is not valid for valid encoding of
        /// IP address information.
        /// </summary>
        /// <param name="ipAddr">A string representation of the IP address to convert</param>
        /// <returns>Returns a backwards uint representation of the string.</returns>
        private static uint IpAddressToLongBackwards(string ipAddr)
        {
            System.Net.IPAddress oIp = System.Net.IPAddress.Parse(ipAddr);
            byte[] byteIp = oIp.GetAddressBytes();


            uint ip = (uint)byteIp[0] << 24;
            ip += (uint)byteIp[1] << 16;
            ip += (uint)byteIp[2] << 8;
            ip += (uint)byteIp[3];

            return ip;
        }
    }
}