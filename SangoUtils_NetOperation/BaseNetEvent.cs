using SangoNetProtol;

namespace SangoUtils.NetOperation
{
    public abstract class BaseNetEvent : BaseNetOperation
    {
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