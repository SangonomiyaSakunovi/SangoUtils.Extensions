namespace SangoUtils_UDP
{
    public abstract class BaseUdpEvent
    {
        public int UdpEventPortId { get; private set; }

        public abstract void OnEventDataReceived<T>(T data) where T : class;

        public virtual void OnInit(int eventPortId, NetUdpEventHandler handler)
        {
            UdpEventPortId = eventPortId;
            handler.AddUdpEvent(this);
        }

        public virtual void OnDispose(NetUdpEventHandler handler)
        {
            handler.RemoveUdpEvent(this);
        }
    }
}
