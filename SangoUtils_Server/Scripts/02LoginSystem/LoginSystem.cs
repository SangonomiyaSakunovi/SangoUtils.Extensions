using SangoNetProtol;
using SangoUtils_Server_Scripts;
using SangoUtils_Server_Scripts.Net;
using SangoUtils_Common.Messages;
using SangoUtils_Server_App.Config;
using System.Text;

namespace SangoUtils_Server_App
{
    public class LoginSystem : BaseSystem<LoginSystem>
    {
        private LoginIOCPHandler? _loginNetHandler;

        private StringBuilder _stringBuilder = new StringBuilder();

        public override void OnInit()
        {
            base.OnInit();
            _loginNetHandler = IOCPService.Instance.GetNetHandler<LoginIOCPHandler>(NetOperationCode.Login);
        }

        public LoginResCode GetLoginRes(LoginReqMessage loginReqMessage, IOCPClientPeer peer)
        {
            LoginResCode loginResCode = LoginResCode.None;
            switch (loginReqMessage.LoginMode)
            {
                case LoginMode.Guest:
                    string entityID = GenerateGuestPlayerEntityID(peer.PeerId);
                    peer.SetEntityID(entityID);
                    peer.OnClientPeerDisconnected = OnClientPeerDisConnected;
                    PlayerEntity playerEntity = new(entityID, SangoSystemConfig.RegistRegularConfig.DefaultTransform, peer);
                    AddLoginPlayerEntity(peer, playerEntity);
                    OnPlayerEntityEnterSceneTestMain(playerEntity);
                    loginResCode = LoginResCode.LoginSuccess;
                    break;
                case LoginMode.UIDAndPassword:
                    //TODO
                    break;
            }
            return loginResCode;
        }

        private string GenerateGuestPlayerEntityID(int peerID)
        {
            _stringBuilder.Clear();
            _stringBuilder.Append(SangoSystemConfig.RegistRegularConfig.GuestIDPrefix);
            _stringBuilder.Append('_');
            _stringBuilder.Append(peerID);
            return _stringBuilder.ToString();
        }

        private bool OnClientPeerDisConnected(IOCPClientPeer peer)
        {
            return OnlinePlayerEntityCache.Instance.RemovePlayerEntity(peer);
        }

        private bool AddLoginPlayerEntity(IOCPClientPeer peer, BaseObjectEntity entity)
        {
            return OnlinePlayerEntityCache.Instance.AddLoginPlayerEntity(peer, entity);
        }

        private void OnPlayerEntityEnterSceneTestMain(BaseObjectEntity entity)
        {
            AOISystem.Instance.OnPlayerEntityEnterInSceneTestMain(entity);
        }
    }
}
