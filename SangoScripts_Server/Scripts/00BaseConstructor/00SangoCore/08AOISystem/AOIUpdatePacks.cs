namespace SangoUtils_Server_Scripts.AOI
{
    public class AOIUpdatePacks(int enterPacksCount, int movePacksCount, int exitPacksCount) : BaseController
    {
        public List<AOIEntityEnterPack> AOIEntityEnterPacks { get; set; } = new(enterPacksCount);
        public List<AOIEntityMovePack> AOIEntityMovePacks { get; set; } = new(movePacksCount);
        public List<AOIEntityExitPack> AOIEntityExitPacks { get; set; } = new(exitPacksCount);

        public bool IsEmpty
        {
            get
            {
                if (AOIEntityEnterPacks.Count == 0 && AOIEntityMovePacks.Count == 0 && AOIEntityExitPacks.Count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void Reset()
        {
            AOIEntityEnterPacks.Clear();
            AOIEntityMovePacks.Clear();
            AOIEntityExitPacks.Clear();
        }
    }

    public struct AOIEntityEnterPack(string entityID, TransformData transform)
    {
        public string EntityID { get; private set; } = entityID;
        public TransformData Transform { get; private set; } = transform;
    }

    public struct AOIEntityMovePack(string entityID, TransformData transform)
    {
        public string EntityID { get; private set; } = entityID;
        public TransformData Transform { get; private set; } = transform;
    }

    public struct AOIEntityExitPack(string entityID)
    {
        public string EntityID { get; private set; } = entityID;
    }
}