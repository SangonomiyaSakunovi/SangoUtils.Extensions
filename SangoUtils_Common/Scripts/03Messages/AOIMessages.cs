using System;
using System.Collections.Generic;
using System.Numerics;

namespace SangoUtils_Common
{
#pragma warning disable CS8618

    [Serializable]
    public class AOIMessage
    {
        public int MessageType;
        public List<AOIViewEnterEntity> AOIViewEnterEntitys { get; set; }
        public List<AOIViewMoveEntity> AOIViewMoveEntitys { get; set; }
        public List<AOIViewExitEntity> AOIViewExitEntitys { get; set; }
    }

    [Serializable]
    public class AOIViewEnterEntity
    {
        public AOIViewEnterEntity(string entityID, Vector3 position)
        {
            EntityID = entityID;
            Position = position;
        }

        public string EntityID { get; set; }
        public Vector3 Position { get; set; }
    }

    [Serializable]
    public class AOIViewMoveEntity
    {
        public AOIViewMoveEntity(string entityID, Vector3 position)
        {
            EntityID = entityID;
            Position = position;
        }

        public string EntityID { get; set; }
        public Vector3 Position { get; set; }
    }

    [Serializable]
    public class AOIViewExitEntity
    {
        public AOIViewExitEntity(string entityID)
        {
            EntityID = entityID;
        }

        public string EntityID { get; set; }
    }
#pragma warning restore CS8618
}

