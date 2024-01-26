// See https://aka.ms/new-console-template for more information
using SangoUtils_NetAdapter;

Console.WriteLine("Hello, World!");

//NetInterface.ShowNetworkInterfaces();

List<IPv4AddressInfo>? ipv4AddressInfos = NetInterface.GetIPv4AddressInfo();

foreach (IPv4AddressInfo pv4AddressInfo in ipv4AddressInfos)
{
    Console.WriteLine(pv4AddressInfo.Description + " : " + pv4AddressInfo.Address);
}
