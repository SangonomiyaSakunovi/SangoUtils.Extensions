using SangoNetProtol;

namespace SangoUtils.NetOperations
{
    public abstract class BaseNetRequest : BaseNetOperation
    {
        protected abstract void DefaultOperationRequest();

        public abstract void OnOperationResponse(string message);

        public virtual void OnInit(NetOperationCode netOperationCode, NetClientOperationHandler handler)
        {
            NetOperationCode = netOperationCode;
            handler.AddNetRequest(this);
        }

        public virtual void OnDispose(NetClientOperationHandler handler)
        {
            handler.RemoveNetRequest(this);
        }
    }
}