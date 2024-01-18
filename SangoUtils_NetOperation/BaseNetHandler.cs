using SangoNetProtol;

namespace SangoUtils_NetOperation
{
    public abstract class BaseNetHandler : BaseNetOperation
    {        
        protected abstract void DefaultOperationResponse(BaseNetClientPeer peer);

        public abstract void OnOperationRequest(string message, BaseNetClientPeer peer);

        public virtual void OnInit(NetOperationCode netOperationCode, NetServerOperationHandler handler)
        {
            NetOperationCode = netOperationCode;
            handler.AddNetHandler(this);
        }

        public virtual void OnDispose(NetServerOperationHandler handler)
        {
            handler.RemoveNetHandler(this);
        }
    }
}