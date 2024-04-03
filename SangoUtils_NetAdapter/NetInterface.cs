using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace SangoUtils.NetAdapter
{
    public class NetInterface : NetworkInterface
    {
        public static void ShowNetworkInterfaces()
        {
            IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            Console.WriteLine("Interface information for {0}.{1}     ",
                    computerProperties.HostName, computerProperties.DomainName);
            if (nics == null || nics.Length < 1)
            {
                Console.WriteLine("  No network interfaces found.");
                return;
            }

            Console.WriteLine("  Number of interfaces .................... : {0}", nics.Length);
            foreach (NetworkInterface adapter in nics)
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                Console.WriteLine();
                Console.WriteLine(adapter.Description);
                Console.WriteLine(String.Empty.PadLeft(adapter.Description.Length, '='));
                Console.WriteLine("  Interface type .......................... : {0}", adapter.NetworkInterfaceType);
                Console.WriteLine("  Physical Address ........................ : {0}",
                           adapter.GetPhysicalAddress().ToString());
                Console.WriteLine("  Operational status ...................... : {0}",
                    adapter.OperationalStatus);
                string versions = "";

                if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                {
                    versions = "IPv4";
                }
                if (adapter.Supports(NetworkInterfaceComponent.IPv6))
                {
                    if (versions.Length > 0)
                    {
                        versions += " ";
                    }
                    versions += "IPv6";
                }
                Console.WriteLine("  IP version .............................. : {0}", versions);
                ShowIPAddresses(properties);

                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                {
                    continue;
                }
                Console.WriteLine("  DNS suffix .............................. : {0}",
                    properties.DnsSuffix);

                string label;
                if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                {
                    IPv4InterfaceProperties ipv4 = properties.GetIPv4Properties();
                    Console.WriteLine("  MTU...................................... : {0}", ipv4.Mtu);
                    if (ipv4.UsesWins)
                    {

                        IPAddressCollection winsServers = properties.WinsServersAddresses;
                        if (winsServers.Count > 0)
                        {
                            label = "  WINS Servers ............................ :";
                            ShowIPAddresses(label, winsServers);
                        }
                    }
                }

                Console.WriteLine("  DNS enabled ............................. : {0}",
                    properties.IsDnsEnabled);
                Console.WriteLine("  Dynamically configured DNS .............. : {0}",
                    properties.IsDynamicDnsEnabled);
                Console.WriteLine("  Receive Only ............................ : {0}",
                    adapter.IsReceiveOnly);
                Console.WriteLine("  Multicast ............................... : {0}",
                    adapter.SupportsMulticast);
                ShowInterfaceStatistics(adapter);

                Console.WriteLine();
            }
        }

        private static void ShowIPAddresses(IPInterfaceProperties properties)
        {
            Console.WriteLine("  IPv4 Addresses .......................... :");
            foreach (UnicastIPAddressInformation ip in properties.UnicastAddresses)
            {
                if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    Console.WriteLine("    IPv4: {0}", ip.Address);
                }
            }

            Console.WriteLine("  IPv6 Addresses .......................... :");
            foreach (UnicastIPAddressInformation ip in properties.UnicastAddresses)
            {
                if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    Console.WriteLine("    IPv6: {0}", ip.Address);
                }
            }
        }

        private static void ShowIPAddresses(string label, IPAddressCollection addresses)
        {
            Console.WriteLine(label);
            foreach (IPAddress address in addresses)
            {
                Console.WriteLine("    {0}", address);
            }
        }

        private static void ShowInterfaceStatistics(NetworkInterface adapter)
        {
            Console.WriteLine("  Network Interface Statistics:");

            IPv4InterfaceStatistics ipv4Statistics = adapter.GetIPv4Statistics();

            Console.WriteLine("    Bytes Received ...................... : {0}", ipv4Statistics.BytesReceived);
            Console.WriteLine("    Bytes Sent .......................... : {0}", ipv4Statistics.BytesSent);
            Console.WriteLine("    Unicast Packets Received ............ : {0}", ipv4Statistics.UnicastPacketsReceived);
            Console.WriteLine("    Unicast Packets Sent ................ : {0}", ipv4Statistics.UnicastPacketsSent);
            Console.WriteLine("    Non-unicast Packets Received ........ : {0}", ipv4Statistics.NonUnicastPacketsReceived);
            Console.WriteLine("    Non-unicast Packets Sent ............ : {0}", ipv4Statistics.NonUnicastPacketsSent);
            Console.WriteLine("    Incoming Packets Discarded .......... : {0}", ipv4Statistics.IncomingPacketsDiscarded);
            Console.WriteLine("    Outgoing Packets Discarded .......... : {0}", ipv4Statistics.OutgoingPacketsDiscarded);
            Console.WriteLine("    Incoming Packets with Errors ........ : {0}", ipv4Statistics.IncomingPacketsWithErrors);
            Console.WriteLine("    Outgoing Packets with Errors ........ : {0}", ipv4Statistics.OutgoingPacketsWithErrors);
            Console.WriteLine("    Incoming Unknown Protocol Packets ... : {0}", ipv4Statistics.IncomingUnknownProtocolPackets);

            if (adapter.Supports(NetworkInterfaceComponent.IPv6))
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                IPv6InterfaceProperties ipv6Properties = properties.GetIPv6Properties();
                //TODO
            }
        }

        public static List<IPv4AddressInfo>? GetIPv4AddressInfo()
        {
            IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            if (nics == null || nics.Length < 1)
            {
                Console.WriteLine("  No network interfaces found.");
                return null;
            }
            List<IPv4AddressInfo> ipv4AddressInfos = new List<IPv4AddressInfo>();
            foreach (NetworkInterface adapter in nics)
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                {
                    foreach (UnicastIPAddressInformation ip in properties.UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            IPv4AddressInfo addressInfo = new IPv4AddressInfo(adapter.Description, ip.Address);
                            ipv4AddressInfos.Add(addressInfo);
                        }
                    }
                }

            }
            if (ipv4AddressInfos.Count > 0)
            {
                return ipv4AddressInfos;
            }
            return null;
        }
    }
}
