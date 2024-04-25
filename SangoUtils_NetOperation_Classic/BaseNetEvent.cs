namespace SangoUtils.NetOperations
{
    public abstract class BaseNetEvent : BaseNetOperation
    {
        public abstract void OnEventData(string message);

        public virtual void OnInit(int opCode, NetClientOperationHandler handler)
        {
            OpCode = opCode;
            handler.AddNetEvent(this);
        }

        public virtual void OnDispose(NetClientOperationHandler handler)
        {
            handler.RemoveNetEvent(this);
        }
    }
}
