using SangoScripts_Server.AOI;
using SangoScripts_Server.Logger;
using SangoUtils_Common.Config;
using System.Collections.Concurrent;

namespace SangoScripts_Server
{
    public abstract class BaseScene<T> : ServerSingleton<T> where T : class, new()
    {
        protected ConcurrentDictionary<string, BaseObjectEntity> _mapEntitysDict = new();

        private ConcurrentQueue<BaseObjectEntity> _playerEntityEnterQueue = new();
        private ConcurrentQueue<BaseObjectEntity> _playerEntityExitQueue = new();
        private ConcurrentQueue<BaseObjectEntity> _playerEntityMoveQueue = new();

        protected SceneConfig? _currentMapStageConfig;

        public AOIController? AOIController { get; protected set; }

        public virtual void SetConfig(SceneConfig currentSceneConfig)
        {

        }

        public virtual void OnInit()
        {

        }

        public override void Update()
        {
            base.Update();
            OnUpdate();
        }

        protected virtual void OnUpdate()
        {
            AOIController?.OnAOIUpdate();

            while (_playerEntityExitQueue.TryDequeue(out BaseObjectEntity? entity))
            {
                if (entity.AOIEntity != null)
                {
                    AOIController?.OnEntityExitCell(entity.AOIEntity);
                    if (_mapEntitysDict.TryRemove(entity.EntityID, out BaseObjectEntity? e))
                    {
                        entity.OnExitFromScene();
                        entity.AOIEntity = null;
                    }
                    else
                    {
                        SangoLogger.Error($"EntityID: [ {entity.EntityID} ] is not exist in SceneID: [ {_currentMapStageConfig?.SceneID} ]");
                    }
                }
                else
                {
                    SangoLogger.Error($"EntityID: [ {entity.EntityID} ] has no Entity.");
                }
            }
            while (_playerEntityEnterQueue.TryDequeue(out BaseObjectEntity? entity))
            {
                entity.AOIEntity = AOIController?.OnEntityEnterCell(entity.EntityID, entity.Transform, entity.AOIEntityType);
                if (!_mapEntitysDict.ContainsKey(entity.EntityID))
                {
                    if (_mapEntitysDict.TryAdd(entity.EntityID, entity))
                    {
                        entity.OnEnterToScene(this);
                    }
                    else
                    {
                        SangoLogger.Error($"EntityID: [ {entity.EntityID} ] can`t add to SceneID: [ {_currentMapStageConfig?.SceneID} ]");
                    }
                }
                else
                {
                    SangoLogger.Error($"EntityID: [ {entity.EntityID} ] is already exist in SceneID: [ {_currentMapStageConfig?.SceneID} ]");
                }
            }
            while (_playerEntityMoveQueue.TryDequeue(out BaseObjectEntity? entity))
            {
                if (entity.AOIEntity != null)
                {
                    AOIController?.OnEntityMove(entity.AOIEntity, entity.Transform);
                }
                else
                {
                    SangoLogger.Error($"EntityID: [ {entity.EntityID} ] has no Entity.");
                }
            }
        }

        protected virtual void OnEntityEnter(BaseObjectEntity entity)
        {
            if (!_mapEntitysDict.ContainsKey(entity.EntityID))
            {
                _playerEntityEnterQueue.Enqueue(entity);
                SangoLogger.Processing($"EntityID: [ {entity.EntityID} ] has enter the SceneID: [ {_currentMapStageConfig?.SceneID} ]");
            }
            else
            {
                SangoLogger.Warning($"EntityID: [ {entity.EntityID} ] has already exist in SceneID: [ {_currentMapStageConfig?.SceneID} ]");
            }
        }

        protected virtual void OnEntityMove(BaseObjectEntity entity)
        {
            if (_mapEntitysDict != null)
            {
                if (_mapEntitysDict.ContainsKey(entity.EntityID))
                {
                    _playerEntityMoveQueue.Enqueue(entity);                    
                }
                else
                {
                    SangoLogger.Warning($"EntityID: [ {entity.EntityID} ] is Not exist in SceneID: [ {_currentMapStageConfig?.SceneID} ]");
                }
            }
        }

        protected virtual void OnEntityExit(BaseObjectEntity entity)
        {
            if (_mapEntitysDict != null)
            {
                if (_mapEntitysDict.ContainsKey(entity.EntityID))
                {
                    _playerEntityExitQueue.Enqueue(entity);
                    SangoLogger.Processing($"EntityID: [ {entity.EntityID} ] has enter the SceneID: [ {_currentMapStageConfig?.SceneID} ]");
                }
                else
                {
                    SangoLogger.Warning($"EntityID: [ {entity.EntityID} ] is Not exist in SceneID: [ {_currentMapStageConfig?.SceneID} ]");
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
