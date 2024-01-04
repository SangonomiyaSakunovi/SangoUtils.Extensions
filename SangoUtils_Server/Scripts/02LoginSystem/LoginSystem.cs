using SangoNetProtol;
using SangoScripts_Server;
using SangoScripts_Server.Net;
using SangoUtils_Common.Messages;
using SangoUtils_Server.Config;
using System.Text;

namespace SangoUtils_Server
{
    public class LoginSystem : BaseSystem<LoginSystem>
    {
        private LoginNetHandler? _loginNetHandler;

        private StringBuilder _stringBuilder = new StringBuilder();

        public override void OnInit()
        {
            base.OnInit();
            _loginNetHandler = NetService.Instance.GetNetHandler<LoginNetHandler>(NetOperationCode.Login);
        }

        public LoginResCode GetLoginRes(LoginReqMessage loginReqMessage, ClientPeer peer)
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

        private bool OnClientPeerDisConnected(ClientPeer peer)
        {
            return OnlinePlayerEntityCache.Instance.RemovePlayerEntity(peer);
        }

        private bool AddLoginPlayerEntity(ClientPeer peer, BaseObjectEntity entity)
        {
            return OnlinePlayerEntityCache.Instance.AddLoginPlayerEntity(peer, entity);
        }

        private void OnPlayerEntityEnterSceneTestMain(BaseObjectEntity entity)
        {
            SceneTestMain.Instance.OnPlayerEntityEnter(entity);
        }
    }
}
