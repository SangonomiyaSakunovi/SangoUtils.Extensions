namespace SangoUtils.NetOperations
{
    public abstract class BaseNetRequest : BaseNetOperation
    {
        protected abstract void DefaultOperationRequest();

        public abstract void OnOperationResponse(string message);

        public virtual void OnInit(int opCode, NetClientOperationHandler handler)
        {
            OpCode = opCode;
            handler.AddNetRequest(this);
        }

        public virtual void OnDispose(NetClientOperationHandler handler)
        {
            handler.RemoveNetRequest(this);
        }
    }
}
