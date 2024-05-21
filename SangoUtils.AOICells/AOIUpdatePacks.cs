using System.Collections.Generic;

namespace SangoUtils.AOICells
{
    public class AOIUpdatePacks
    {
        public List<AOIEntityEnterPack> AOIEntityEnterPacks { get; set; }
        public List<AOIEntityMovePack> AOIEntityMovePacks { get; set; }
        public List<AOIEntityExitPack> AOIEntityExitPacks { get; set; }

        public AOIUpdatePacks(int enterPacksCount, int movePacksCount, int exitPacksCount)
        {
            AOIEntityEnterPacks = new List<AOIEntityEnterPack>(enterPacksCount);
            AOIEntityMovePacks = new List<AOIEntityMovePack>(movePacksCount);
            AOIEntityExitPacks = new List<AOIEntityExitPack>(exitPacksCount);
        }

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

    public struct AOIEntityEnterPack
    {
        public string EntityID { get; private set; }
        public TransformData Transform { get; private set; }

        public AOIEntityEnterPack(string entityID, TransformData transform)
        {
            EntityID = entityID;
            Transform = transform;
        }
    }

    public struct AOIEntityMovePack
    {
        public string EntityID { get; private set; }
        public TransformData Transform { get; private set; }

        public AOIEntityMovePack(string entityID, TransformData transform)
        {
            EntityID = entityID;
            Transform = transform;
        }
    }

    public struct AOIEntityExitPack
    {
        public string EntityID { get; private set; }

        public AOIEntityExitPack(string entityID)
        {
            EntityID = entityID;
        }
    }
}