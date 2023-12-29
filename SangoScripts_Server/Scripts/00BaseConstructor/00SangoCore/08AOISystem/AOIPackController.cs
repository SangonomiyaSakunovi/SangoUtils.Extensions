using System.Numerics;

namespace SangoScripts_Server.AOI
{
    public class AOIPackController : BaseController
    {
        public List<AOIEntityEnterPack> _aoiEntityEnterPacks;
        public List<AOIEntityMovePack> _aoiEntityMovePacks;
        public List<AOIEntityExitPack> _aoiEntityExitPacks;

        public bool IsEmpty
        {
            get
            {
                if (_aoiEntityEnterPacks.Count == 0&& _aoiEntityMovePacks.Count == 0&& _aoiEntityExitPacks.Count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public AOIPackController(int enterPacksCount, int movePacksCount, int exitPacksCount)
        {
            _aoiEntityEnterPacks = new(enterPacksCount);
            _aoiEntityMovePacks = new(movePacksCount);
            _aoiEntityExitPacks = new(exitPacksCount);
        }

        public void Reset()
        {
            _aoiEntityEnterPacks.Clear();
            _aoiEntityMovePacks.Clear();
            _aoiEntityExitPacks.Clear();
        }
    }

    public struct AOIEntityEnterPack
    {
        public string _entityID;
        public Vector3 _position;

        public AOIEntityEnterPack(string entityID, Vector3 position)
        {
            _entityID = entityID;
            _position = position;
        }
    }

    public struct AOIEntityMovePack
    {
        public string _entityID;
        public Vector3 _position;

        public AOIEntityMovePack(string entityID, Vector3 position)
        {
            _entityID = entityID;
            _position = position;
        }
    }

    public struct AOIEntityExitPack
    {
        public string _entityID;

        public AOIEntityExitPack(string entityID)
        {
            _entityID = entityID;
        }
    }
}
