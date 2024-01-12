using SangoUtils_Common.Messages;
using System;
using System.Collections.Generic;

namespace SangoUtils_Common.Messages
{
    [Serializable]
    public class AOIReqMessage
    {
        public AOIReqMessage() { }
        
        public AOIReqMessage(List<AOIActiveMoveEntity> aOIActiveMoveEntitys)
        {
            AOIActiveMoveEntitys = aOIActiveMoveEntitys;
        }

        public List<AOIActiveMoveEntity> AOIActiveMoveEntitys { get; set; } = new List<AOIActiveMoveEntity>();
    }

    [Serializable]
    public class AOIEventMessage
    {
        public AOIEventMessage() { }
        
        public AOIEventMessage(List<AOIViewEnterEntity> aOIViewEnterEntitys, List<AOIViewMoveEntity> aOIViewMoveEntitys, List<AOIViewExitEntity> aOIViewExitEntitys)
        {
            AOIViewEnterEntitys = aOIViewEnterEntitys;
            AOIViewMoveEntitys = aOIViewMoveEntitys;
            AOIViewExitEntitys = aOIViewExitEntitys;
        }

        public List<AOIViewEnterEntity> AOIViewEnterEntitys { get; set; } = new List<AOIViewEnterEntity>();
        public List<AOIViewMoveEntity> AOIViewMoveEntitys { get; set; } = new List<AOIViewMoveEntity>();
        public List<AOIViewExitEntity> AOIViewExitEntitys { get; set; } = new List<AOIViewExitEntity>();
    }

    [Serializable]
    public class AOIActiveMoveEntity
    {
        public AOIActiveMoveEntity() { }

        public AOIActiveMoveEntity(string entityID, TransformMessage transformInfo)
        {
            EntityID = entityID;
            TransformInfo = transformInfo;
        }

        public string EntityID { get; set; } = "";
        public TransformMessage TransformInfo { get; set; } = new TransformMessage();
    }

    [Serializable]
    public class AOIViewEnterEntity
    {
        public AOIViewEnterEntity() { }
        
        public AOIViewEnterEntity(string entityID, TransformMessage transformInfo)
        {
            EntityID = entityID;
            TransformInfo = transformInfo;
        }

        public string EntityID { get; set; } = "";
        public TransformMessage TransformInfo { get; set; } = new TransformMessage();
    }

    [Serializable]
    public class AOIViewMoveEntity
    {
        public AOIViewMoveEntity() { }

        public AOIViewMoveEntity(string entityID, TransformMessage transformInfo)
        {
            EntityID = entityID;
            TransformInfo = transformInfo;
        }

        public string EntityID { get; set; } = "";
        public TransformMessage TransformInfo { get; set; } = new TransformMessage();
    }

    [Serializable]
    public class AOIViewExitEntity
    {
        public AOIViewExitEntity() { }

        public AOIViewExitEntity(string entityID)
        {
            EntityID = entityID;
        }

        public string EntityID { get; set; } = "";
    }
}

