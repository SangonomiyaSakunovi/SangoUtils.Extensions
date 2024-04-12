namespace SangoUtils.NetOperation
{
    public abstract class BaseNetHandler : BaseNetOperation
    {
        protected abstract void DefaultOperationResponse(BaseNetClientPeer peer);

        public abstract void OnOperationRequest(string message, BaseNetClientPeer peer);

        public virtual void OnInit(int opCode, NetServerOperationHandler handler)
        {
            OpCode = opCode;
            handler.AddNetHandler(this);
        }

        public virtual void OnDispose(NetServerOperationHandler handler)
        {
            handler.RemoveNetHandler(this);
        }
    }
}
