namespace SangoUtils.NetOperation
{
    public abstract class BaseNetController : BaseNetOperation
    {
        protected abstract void DefaultOperationEvent(BaseNetClientPeer peer);

        public virtual void OnInit(int opCode, NetServerOperationHandler handler)
        {
            OpCode = opCode;
            handler.AddNetController(this);
        }

        public virtual void OnDispose(NetServerOperationHandler handler)
        {
            handler.RemoveNetController(this);
        }
    }
}
