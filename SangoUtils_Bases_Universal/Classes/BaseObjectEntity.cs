using SangoUtils_FixedNum;

namespace SangoUtils_Bases_Universal
{
    public abstract class BaseObjectEntity
    {
        public BaseObjectEntity(string entityID, PlayerState playerState)
        {
            EntityID = entityID;
            PlayerState = playerState;
        }

        public string EntityID { get; private set; } = "";
        public FixedVector3 LogicDirection { get; set; } = FixedVector3.Zero;
        public FixedVector3 LogicPosition { get; set; } = FixedVector3.Zero;
        public FixedVector3 LogicPositionLast { get; set; } = FixedVector3.Zero;
        public PlayerState PlayerState { get; set; } = PlayerState.None;

        public void Update()
        {
            OnUpdate();
        }

        protected virtual void OnUpdate()
        {

        }
    }

    public enum PlayerState
    {
        None = 0,
        Online = 1,
        Offline = 2,
    }
}
