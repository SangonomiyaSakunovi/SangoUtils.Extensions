using SangoScripts_Server.AOI;
using SangoScripts_Server.Cache;
using SangoScripts_Server.Map;
using SangoUtils_Common;
using SangoUtils_Common.Config;
using SharpCompress.Common;

namespace SangoUtils_Server
{
    public class SunUpF4LobbyMap : MapBaseStage
    {
        public override void OnInit()
        {
            base.OnInit();

        }

        public override void OnDispose()
        {

        }

        public override void OnUpdate()
        {
            AOIController?.OnAOIUpdate();
        }

        public override void SetConfig(MapConfig currentMapStageConfig)
        {
            _currentMapStageConfig = currentMapStageConfig;
            AOIController = new AOIController(_currentMapStageConfig.aoiConfig);
            AOIController.OnEntityCellViewChanged = OnEntityCellViewChanged;
            AOIController.OnCellOperationCombined = OnCellOperationCombined;
        }

        private void OnEntityCellViewChanged(AOIEntity entity, AOIPackController packController)
        {
            AOIMessage message = new()
            {
                AOIViewEnterEntitys = new(),
                AOIViewExitEntitys = new()
            };

            if (packController._aoiEntityEnterPacks.Count > 0)
            {
                for (int i = 0; i < packController._aoiEntityEnterPacks.Count; i++)
                {
                    message.AOIViewEnterEntitys.Add(new AOIViewEnterEntity(packController._aoiEntityEnterPacks[i]._entityID, packController._aoiEntityEnterPacks[i]._position));
                }
            }
            if (packController._aoiEntityExitPacks.Count > 0)
            {
                for (int j = 0; j < packController._aoiEntityExitPacks.Count; j++)
                {
                    message.AOIViewExitEntitys.Add(new AOIViewExitEntity(packController._aoiEntityExitPacks[j]._entityID));
                }
            }

            if (_mapEntitysDict.TryGetValue(entity._entityID, out PlayerEntity? playerEntity))
            {
                playerEntity.OnUpdateInMap(message);
            }
        }

        private void OnCellOperationCombined(AOICell cell, AOIPackController packController)
        {
            AOIMessage message = new()
            {
                AOIViewEnterEntitys = new(),
                AOIViewMoveEntitys = new(),
                AOIViewExitEntitys = new()
            };

            if (packController._aoiEntityEnterPacks.Count > 0)
            {
                for (int i = 0; i < packController._aoiEntityEnterPacks.Count; i++)
                {
                    message.AOIViewEnterEntitys.Add(new AOIViewEnterEntity(packController._aoiEntityEnterPacks[i]._entityID, packController._aoiEntityEnterPacks[i]._position));
                }
            }
            if (packController._aoiEntityMovePacks.Count>0)
            {
                for(int j = 0;j< packController._aoiEntityMovePacks.Count; j++)
                {
                    message.AOIViewMoveEntitys.Add(new AOIViewMoveEntity(packController._aoiEntityMovePacks[j]._entityID, packController._aoiEntityMovePacks[j]._position));
                }
            }
            if (packController._aoiEntityExitPacks.Count > 0)
            {
                for (int k = 0; k < packController._aoiEntityExitPacks.Count; k++)
                {
                    message.AOIViewExitEntitys.Add(new AOIViewExitEntity(packController._aoiEntityExitPacks[k]._entityID));
                }
            }

            foreach (AOIEntity entity in cell._holdSet)
            {
                if (_mapEntitysDict.TryGetValue(entity._entityID, out PlayerEntity? playerEntity))
                {
                    playerEntity.OnUpdateInMap(message);
                }
            }
        }

        public void OnEntityMove(SyncTransformInfo syncTransformInfo)
        {
            if (_mapEntitysDict.TryGetValue(syncTransformInfo.EntityID, out PlayerEntity? entity))
            {
                entity.TransformInfo = syncTransformInfo.TransformInfo;
                OnEntityMove(entity);
            }
        }

        public void OnEntityExit(string entityID)
        {
            if (_mapEntitysDict.TryGetValue(entityID, out PlayerEntity? entity))
            {
                OnEntityExit(entity);
            }
        }
    }
}
