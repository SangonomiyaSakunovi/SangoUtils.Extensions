using System.Net;

namespace SangoUtils.NetAdapter
{
    public class IPv4AddressInfo
    {
        public IPv4AddressInfo() { }

        public IPv4AddressInfo(string description, IPAddress address)
        {
            Description = description;
            Address = address;
        }

        public string Description { get; set; } = "Not defined";
        public IPAddress Address { get; set; } = IPAddress.None;
    }

    public class IPv6AddressInfo
    {
        public IPv6AddressInfo() { }

        public IPv6AddressInfo(string description, IPAddress address)
        {
            Description = description;
            Address = address;
        }

        public string Description { get; set; } = "Not defined";
        public IPAddress Address { get; set; } = IPAddress.None;
    }
}