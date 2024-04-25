using SangoNetProtol;

namespace SangoUtils.NetOperations
{
    public abstract class BaseNetUdpMessage : BaseNetOperation
    {
        public abstract void DefaultOperationUdpMessage();

        public abstract void OnUdpMessage(string message);

        public virtual void OnInit(NetOperationCode netOperationCode, NetClientOperationHandler handler)
        {
            NetOperationCode = netOperationCode;
            handler.AddNetUdpMessage(this);
        }

        public virtual void OnDispose(NetClientOperationHandler handler)
        {
            handler.RemoveNetUdpMessage(this);
        }
    }
}
