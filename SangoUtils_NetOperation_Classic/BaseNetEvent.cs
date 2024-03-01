using SangoNetProtocol_Classic;

namespace SangoUtils_NetOperation_Classic
{
    public abstract class BaseNetEvent : BaseNetOperation
    {
        public abstract void OnEventData(string message);

        public virtual void OnInit(int netOperationCode, NetClientOperationHandler handler)
        {
            NetOperationCode = netOperationCode;
            handler.AddNetEvent(this);
        }

        public virtual void OnDispose(NetClientOperationHandler handler)
        {
            handler.RemoveNetEvent(this);
        }
    }
}
