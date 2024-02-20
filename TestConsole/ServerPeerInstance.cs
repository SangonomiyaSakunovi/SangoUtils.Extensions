using SangoUtils_Socket.TCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    internal class ServerPeerInstance : Socket_TCP_Peer_Server<TokenPeer>
    {

    }

    internal class TokenPeer : IClientPeer_Socket_TCP
    {
        protected override void OnClosed()
        {
            Console.WriteLine("DisConnect");
        }

        protected override void OnConnected()
        {
            Console.WriteLine("Connect");
        }

        protected override void OnMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
