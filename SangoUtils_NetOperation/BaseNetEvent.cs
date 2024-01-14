using SangoNetProtol;

namespace SangoUtils_NetOperation
{
    public abstract class BaseNetEvent : BaseNetOperation
    {
        public NetOperationCode NetOperationCode { get; protected set; } = NetOperationCode.Default;

        public abstract void OnEventData(string message);

        public virtual void OnInit(NetOperationCode netOperationCode, NetClientOperationHandler handler)
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