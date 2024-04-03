namespace SangoUtils.Bases
{
    public abstract class BaseASyncPackManager
    {
        protected uint _packId = 1;

        protected abstract uint GeneratePackId();

        public abstract bool RemovePack(uint packId);

        public abstract bool RemovePackCallBack(uint packId);
    }
}
