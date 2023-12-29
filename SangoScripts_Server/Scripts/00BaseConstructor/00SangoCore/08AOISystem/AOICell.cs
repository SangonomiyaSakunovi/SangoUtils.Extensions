using SangoScripts_Server.Logger;

namespace SangoScripts_Server.AOI
{
    public class AOICell
    {
        public AOICellIndex _aoiCellIndex;
        public AOIController _aoiController;
        public AOICell[]? _aoiAroundCells = null;

        public HashSet<AOIEntity> _holdSet = new();
        public HashSet<AOIEntity> _enterTODOHoldSet = new();

        public AOIPackController _aoiCellOperationPackController;

        public AOICell(AOICellIndex cellIndex, AOIController aoiController)
        {
            _aoiCellIndex = cellIndex;
            _aoiController = aoiController;

            _aoiCellOperationPackController = new(_aoiController.AOIConfig.aoiCellOperationEnterPacksCount, _aoiController.AOIConfig.aoiCellOperationMovePacksCount, _aoiController.AOIConfig.aoiCellOperationExitPacksCount);
        }

        public void OnEntityEnterCell(AOIEntity entity)
        {
            if (!_enterTODOHoldSet.Add(entity))
            {
                SangoLogger.Error($"EntityID: [ {entity._entityID} ] already exist in EnterTODOHoldSet.");
                return;
            }
            
            switch (entity.AOIEntityOperationCode)
            {
                case AOIEntityOperationCode.TransferEnterCell:
                    if (_aoiAroundCells != null)
                    {
                        entity.AddAOIAroundCells(_aoiAroundCells);
                        for (int i = 0; i < _aoiAroundCells.Length; i++)
                        {
                            _aoiAroundCells[i].AddCellOperation(AOICellOperationCode.EntityEnter, entity);
                        }
                    }
                    else
                    {
                        SangoLogger.Error($"AOICellIndex: [ {_aoiCellIndex._xIndex}_{_aoiCellIndex._zIndex} ] has no AOIAroundCells.");
                    }
                    break;
                case AOIEntityOperationCode.MoveCrossCell:

                    break;
                default:
                    SangoLogger.Error($"EntityID: [ {entity._entityID} ] AOIEntityOperationCode Error: [ {entity.AOIEntityOperationCode} ].");
                    break;
            }
        }

        public void OnEntityMoveInsideCell(AOIEntity entity)
        {

        }

        public void OnEntityExitCell(AOIEntity entity)
        {
            if (_aoiAroundCells != null)
            {
                for (int i = 0; i < _aoiAroundCells.Length; i++)
                {
                    _aoiAroundCells[i].AddCellOperation(AOICellOperationCode.EntityExit, entity);
                }
            }
        }

        public void CalcCellOperationCombine()
        {
            if (!_aoiCellOperationPackController.IsEmpty)
            {
                _aoiController.OnCellOperationCombined?.Invoke(this, _aoiCellOperationPackController);
                _aoiCellOperationPackController.Reset();
            }
        }

        private void AddCellOperation(AOICellOperationCode operationCode, AOIEntity entity)
        {
            //TODO ObjectPool???
            switch (operationCode)
            {
                case AOICellOperationCode.EntityEnter:
                    _aoiCellOperationPackController._aoiEntityEnterPacks.Add(new AOIEntityEnterPack(entity._entityID, entity.Position));
                    break;
                case AOICellOperationCode.EntityMove:
                    _aoiCellOperationPackController._aoiEntityMovePacks.Add(new AOIEntityMovePack(entity._entityID, entity.Position));
                    break;
                case AOICellOperationCode.EntityExit:
                    _aoiCellOperationPackController._aoiEntityExitPacks.Add(new AOIEntityExitPack(entity._entityID));
                    break;
            }
        }

        private enum AOICellOperationCode
        {
            EntityEnter,
            EntityExit,
            EntityMove
        }
    }

    public struct AOICellIndex
    {
        public int _xIndex;
        public int _zIndex;

        public AOICellIndex(int xIndex, int zIndex)
        {
            _xIndex = xIndex;
            _zIndex = zIndex;
        }
    }
}
