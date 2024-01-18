using SangoUtils_UDP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    public class ListenPortID : UdpEventBaseListenPortID<ListenPortID>
    {
        [UdpEventPortApiKey("SpecialPort")]
        public const int SpecialPort = 52516;

        public int port = ListenPortID.SpecialPort;
    }

    public class test()
    {
        public int prot2 = ListenPortID.SpecialPort;
    }
}
