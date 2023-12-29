using SangoScripts_Server.AOI;
using SangoScripts_Server.Cache;
using SangoScripts_Server.Logger;
using SangoUtils_Common.Config;
using System.Collections.Concurrent;

namespace SangoScripts_Server.Map
{
    public class MapBaseStage
    {
        protected ConcurrentDictionary<string, PlayerEntity> _mapEntitysDict = new ConcurrentDictionary<string, PlayerEntity>();

        private ConcurrentQueue<PlayerEntity> _playerEntityEnterQueue = new ConcurrentQueue<PlayerEntity>();
        private ConcurrentQueue<PlayerEntity> _playerEntityExitQueue = new ConcurrentQueue<PlayerEntity>();
        private ConcurrentQueue<PlayerEntity> _playerEntityMoveQueue = new ConcurrentQueue<PlayerEntity>();

        protected MapConfig? _currentMapStageConfig;

        public AOIController? AOIController { get; set; }

        public virtual void SetConfig(MapConfig currentMapStageConfig)
        {

        }

        public virtual void OnInit()
        {

        }

        public virtual void OnUpdate()
        {
            while (_playerEntityExitQueue.TryDequeue(out PlayerEntity? entity))
            {
                if (entity.AOIEntity != null)
                {
                    AOIController?.OnEntityExitCell(entity.AOIEntity);
                    if (_mapEntitysDict.TryRemove(entity.EntityID, out PlayerEntity? e))
                    {
                        entity.OnExitFromMap();
                        entity.AOIEntity = null;
                    }
                    else
                    {
                        SangoLogger.Error($"EntityID: [ {entity.EntityID} ] is not exist in MapID: [ {_currentMapStageConfig?.mapID} ]");
                    }
                }
                else
                {
                    SangoLogger.Error($"EntityID: [ {entity.EntityID} ] has no Entity.");
                }
            }
            while (_playerEntityEnterQueue.TryDequeue(out PlayerEntity? entity))
            {
                entity.AOIEntity = AOIController?.OnEntityEnterCell(entity.EntityID,entity.TransformInfo.Position);
                if (!_mapEntitysDict.ContainsKey(entity.EntityID))
                {
                    if (_mapEntitysDict.TryAdd(entity.EntityID, entity))
                    {
                        entity.OnEnterToMap(this);
                    }
                    else
                    {
                        SangoLogger.Error($"EntityID: [ {entity.EntityID} ] can`t add to MapID: [ {_currentMapStageConfig?.mapID} ]");
                    }
                }
                else
                {
                    SangoLogger.Error($"EntityID: [ {entity.EntityID} ] is already exist in MapID: [ {_currentMapStageConfig?.mapID} ]");
                }              
            }
            while (_playerEntityMoveQueue.TryDequeue(out PlayerEntity? entity))
            {
                if (entity.AOIEntity != null)
                {
                    AOIController?.OnEntityMove(entity.AOIEntity, entity.TransformInfo.Position);                    
                }
                else
                {
                    SangoLogger.Error($"EntityID: [ {entity.EntityID} ] has no Entity.");
                }
            }
        }

        protected virtual void OnEntityEnter(PlayerEntity entity)
        {
            if (!_mapEntitysDict.ContainsKey(entity.EntityID))
            {
                _playerEntityEnterQueue.Enqueue(entity);
                SangoLogger.Processing($"EntityID: [ {entity.EntityID} ] has enter the MapID: [ {_currentMapStageConfig?.mapID} ]");
            }
            else
            {
                SangoLogger.Warning($"EntityID: [ {entity.EntityID} ] has already exist in MapID: [ {_currentMapStageConfig?.mapID} ]");
            }
        }

        protected virtual void OnEntityMove(PlayerEntity entity)
        {
            if (_mapEntitysDict != null)
            {
                if (_mapEntitysDict.ContainsKey(entity.EntityID))
                {
                    _playerEntityMoveQueue.Enqueue(entity);
                    SangoLogger.Processing($"EntityID: [ {entity.EntityID} ] has enter the MapID: [ {_currentMapStageConfig?.mapID} ]");
                }
                else
                {
                    SangoLogger.Warning($"EntityID: [ {entity.EntityID} ] is Not exist in MapID: [ {_currentMapStageConfig?.mapID} ]");
                }
            }           
        }

        protected virtual void OnEntityExit(PlayerEntity entity)
        {
            if (_mapEntitysDict != null)
            {
                if (_mapEntitysDict.ContainsKey(entity.EntityID))
                {
                    _playerEntityExitQueue.Enqueue(entity);
                    SangoLogger.Processing($"EntityID: [ {entity.EntityID} ] has enter the MapID: [ {_currentMapStageConfig?.mapID} ]");
                }
                else
                {
                    SangoLogger.Warning($"EntityID: [ {entity.EntityID} ] is Not exist in MapID: [ {_currentMapStageConfig?.mapID} ]");
                }
            }
        }

        public virtual void OnDispose()
        {
            _mapEntitysDict.Clear();

            _playerEntityEnterQueue.Clear();
            _playerEntityExitQueue.Clear();
            _playerEntityMoveQueue.Clear();

            _currentMapStageConfig = null;
            AOIController = null;
        }
    }
}
