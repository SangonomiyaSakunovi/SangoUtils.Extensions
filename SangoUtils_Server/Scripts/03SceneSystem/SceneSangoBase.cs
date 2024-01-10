using SangoNetProtol;
using SangoScripts_Server;
using SangoScripts_Server.AOI;
using SangoScripts_Server.IOCP;
using SangoScripts_Server.Utils;
using SangoUtils_Common.Config;
using SangoUtils_Common.Infos;
using SangoUtils_Common.Messages;
using System.Numerics;

namespace SangoUtils_Server
{
    public class SceneSangoBase<T> : BaseScene<T> where T : class, new()
    {
        public override void SetConfig(SceneConfig currentMapStageConfig)
        {
            _currentMapStageConfig = currentMapStageConfig;
            AOIController = new AOIController(_currentMapStageConfig.AOIConfig);
            AOIController.OnEntityCellViewChanged = OnEntityCellViewChanged;
            AOIController.OnCellOperationCombined = OnCellOperationCombined;
        }

        private void OnEntityCellViewChanged(AOIEntity entity, AOIUpdatePacks packController)
        {
            AOIEventMessage message = new();

            if (packController.AOIEntityEnterPacks.Count > 0)
            {
                for (int i = 0; i < packController.AOIEntityEnterPacks.Count; i++)
                {
                    Vector3Info position = new(packController.AOIEntityEnterPacks[i].Transform.Position.X, packController.AOIEntityEnterPacks[i].Transform.Position.Y, packController.AOIEntityEnterPacks[i].Transform.Position.Z);
                    QuaternionInfo rotation = new(packController.AOIEntityEnterPacks[i].Transform.Rotation.X, packController.AOIEntityEnterPacks[i].Transform.Rotation.Y, packController.AOIEntityEnterPacks[i].Transform.Rotation.Z, packController.AOIEntityEnterPacks[i].Transform.Rotation.W);
                    Vector3Info scale = new(packController.AOIEntityEnterPacks[i].Transform.Scale.X, packController.AOIEntityEnterPacks[i].Transform.Scale.Y, packController.AOIEntityEnterPacks[i].Transform.Scale.Z);

                    TransformInfo transformInfo = new(position, rotation, scale);
                    AOIViewEnterEntity enterEntity = new(packController.AOIEntityEnterPacks[i].EntityID, transformInfo);
                    message.AOIViewEnterEntitys.Add(enterEntity);
                }
            }
            if (packController.AOIEntityExitPacks.Count > 0)
            {
                for (int j = 0; j < packController.AOIEntityExitPacks.Count; j++)
                {
                    AOIViewExitEntity exitEntity = new(packController.AOIEntityExitPacks[j].EntityID);
                    message.AOIViewExitEntitys.Add(exitEntity);
                }
            }

            if (_mapEntitysDict.TryGetValue(entity.EntityID, out BaseObjectEntity? playerEntity))
            {
                playerEntity.OnMoveInMap(message);
            }
        }

        private void OnCellOperationCombined(AOICell cell, AOIUpdatePacks aoiCellOperationUpdatePacks)
        {
            AOIEventMessage message = new();

            if (aoiCellOperationUpdatePacks.AOIEntityEnterPacks.Count > 0)
            {
                for (int i = 0; i < aoiCellOperationUpdatePacks.AOIEntityEnterPacks.Count; i++)
                {
                    Vector3Info position = new(aoiCellOperationUpdatePacks.AOIEntityEnterPacks[i].Transform.Position.X, aoiCellOperationUpdatePacks.AOIEntityEnterPacks[i].Transform.Position.Y, aoiCellOperationUpdatePacks.AOIEntityEnterPacks[i].Transform.Position.Z);
                    QuaternionInfo rotation = new(aoiCellOperationUpdatePacks.AOIEntityEnterPacks[i].Transform.Rotation.X, aoiCellOperationUpdatePacks.AOIEntityEnterPacks[i].Transform.Rotation.Y, aoiCellOperationUpdatePacks.AOIEntityEnterPacks[i].Transform.Rotation.Z, aoiCellOperationUpdatePacks.AOIEntityEnterPacks[i].Transform.Rotation.W);
                    Vector3Info scale = new(aoiCellOperationUpdatePacks.AOIEntityEnterPacks[i].Transform.Scale.X, aoiCellOperationUpdatePacks.AOIEntityEnterPacks[i].Transform.Scale.Y, aoiCellOperationUpdatePacks.AOIEntityEnterPacks[i].Transform.Scale.Z);

                    TransformInfo transformInfo = new(position, rotation, scale);
                    AOIViewEnterEntity enterEntity = new(aoiCellOperationUpdatePacks.AOIEntityEnterPacks[i].EntityID, transformInfo);
                    message.AOIViewEnterEntitys.Add(enterEntity);
                }
            }
            if (aoiCellOperationUpdatePacks.AOIEntityMovePacks.Count > 0)
            {
                for (int j = 0; j < aoiCellOperationUpdatePacks.AOIEntityMovePacks.Count; j++)
                {
                    Vector3Info position = new(aoiCellOperationUpdatePacks.AOIEntityMovePacks[j].Transform.Position.X, aoiCellOperationUpdatePacks.AOIEntityMovePacks[j].Transform.Position.Y, aoiCellOperationUpdatePacks.AOIEntityMovePacks[j].Transform.Position.Z);
                    QuaternionInfo rotation = new(aoiCellOperationUpdatePacks.AOIEntityMovePacks[j].Transform.Rotation.X, aoiCellOperationUpdatePacks.AOIEntityMovePacks[j].Transform.Rotation.Y, aoiCellOperationUpdatePacks.AOIEntityMovePacks[j].Transform.Rotation.Z, aoiCellOperationUpdatePacks.AOIEntityMovePacks[j].Transform.Rotation.W);
                    Vector3Info scale = new(aoiCellOperationUpdatePacks.AOIEntityMovePacks[j].Transform.Scale.X, aoiCellOperationUpdatePacks.AOIEntityMovePacks[j].Transform.Scale.Y, aoiCellOperationUpdatePacks.AOIEntityMovePacks[j].Transform.Scale.Z);

                    TransformInfo transformInfo = new(position, rotation, scale);
                    AOIViewMoveEntity moveEntity = new(aoiCellOperationUpdatePacks.AOIEntityMovePacks[j].EntityID, transformInfo);
                    message.AOIViewMoveEntitys.Add(moveEntity);
                }
            }
            if (aoiCellOperationUpdatePacks.AOIEntityExitPacks.Count > 0)
            {
                for (int k = 0; k < aoiCellOperationUpdatePacks.AOIEntityExitPacks.Count; k++)
                {
                    AOIViewExitEntity exitEntity = new(aoiCellOperationUpdatePacks.AOIEntityExitPacks[k].EntityID);
                    message.AOIViewExitEntitys.Add(exitEntity);
                }
            }

            string messageJson = JsonUtils.SetJsonString(message);
            byte[] bytes = IOCPUtils.ConvertNetEventDataPackMessageBytes(NetOperationCode.Aoi, messageJson);

            foreach (AOIEntity entity in cell.AOIEntityHoldSets)
            {
                if (_mapEntitysDict.TryGetValue(entity.EntityID, out BaseObjectEntity? playerEntity))
                {
                    AOISystem.Instance.SendAOIEventMessage(playerEntity, bytes);
                }
            }
        }

        public void OnPlayerEntityEnter(BaseObjectEntity entity)
        {
            OnEntityEnter(entity);
        }

        public void OnEntityMove(AOIActiveMoveEntity activeMoveEntity)
        {
            if (_mapEntitysDict.TryGetValue(activeMoveEntity.EntityID, out BaseObjectEntity? entity))
            {
                Vector3 position = new(activeMoveEntity.TransformInfo.Position.X, activeMoveEntity.TransformInfo.Position.Y, activeMoveEntity.TransformInfo.Position.Z);
                Quaternion rotation = new(activeMoveEntity.TransformInfo.Rotation.X, activeMoveEntity.TransformInfo.Rotation.Y, activeMoveEntity.TransformInfo.Rotation.Z, activeMoveEntity.TransformInfo.Rotation.W);
                Vector3 scale = new(activeMoveEntity.TransformInfo.Scale.X, activeMoveEntity.TransformInfo.Scale.Y, activeMoveEntity.TransformInfo.Scale.Z);

                TransformData newTrans = new(position, rotation, scale);
                if(entity.Transform != newTrans)
                {
                    entity.Transform = newTrans;
                    OnEntityMove(entity);
                }
            }
        }

        public void OnEntityExit(string entityID)
        {
            if (_mapEntitysDict.TryGetValue(entityID, out BaseObjectEntity? entity))
            {
                OnEntityExit(entity);
            }
        }
    }
}
