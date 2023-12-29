using SangoScripts_Server.Logger;
using SangoUtils_Common.Config;
using System.Numerics;
using System.Text;

namespace SangoScripts_Server.AOI
{
    public class AOIController : BaseController
    {
        public string ControllerName { get; set; }

        private AOIConfig _currentAOIConfig;
        private Dictionary<string, AOICell> _aoiCellDict;
        private List<AOIEntity> _aoiEntities;

        public Action<AOIEntity, AOIPackController>? OnEntityCellViewChanged;
        public Action<AOICell, AOIPackController>? OnCellOperationCombined;

        private StringBuilder _stringBuilder = new StringBuilder();

        public AOIController(AOIConfig config)
        {
            _currentAOIConfig = config;
            _aoiCellDict = new Dictionary<string, AOICell>(_currentAOIConfig.initCount);
            _aoiEntities = new List<AOIEntity>();
        }

        public AOIConfig AOIConfig { get { return _currentAOIConfig; } }
        public int CellSize { get { return _currentAOIConfig.cellSize; } }

        public AOIEntity OnEntityEnterCell(string entityID, Vector3 position)
        {
            AOIEntity entity = new AOIEntity(entityID, this);
            entity.OnUpdatePosition(position, AOIEntityOperationCode.TransferEnterCell);
            _aoiEntities.Add(entity);
            return entity;
        }

        public void OnEntityMove(AOIEntity entity, Vector3 position)
        {
            entity.OnUpdatePosition(position, AOIEntityOperationCode.MoveCrossCell);
        }

        public void OnEntityExitCell(AOIEntity entity)
        {
            if (_aoiCellDict.TryGetValue(entity._aoiCellKey, out AOICell? cell))
            {
                cell.OnEntityExitCell(entity);
            }
            else
            {
                SangoLogger.Error($"AOICellDict can`t find EntityKey: [ {entity._aoiCellKey} ].");
            }
            if (!_aoiEntities.Remove(entity))
            {
                SangoLogger.Error($"AOIEntityList can`t find EntityKey: [ {entity._aoiCellKey} ].");
            }
        }

        public void OnAOIUpdate()
        {
            for(int i = 0; i < _aoiEntities.Count; i++)
            {
                _aoiEntities[i].CalcEntityCellAndAroundViewChanged();
            }

            foreach (AOICell cell in _aoiCellDict.Values)
            {
                if (cell._enterTODOHoldSet.Count > 0)
                {
                    cell._holdSet.UnionWith(cell._enterTODOHoldSet);
                    cell._enterTODOHoldSet.Clear();
                }
                cell.CalcCellOperationCombine();
            }
        }

        public void OnEntityMoveCrossAOICell(AOIEntity newEntity)
        {
            AOICell cell = GetOrNewAOICell(newEntity);
            if (cell._aoiAroundCells == null)
            {
                GetAOIAroundCell(cell);
            }


            cell.OnEntityEnterCell(newEntity);
        }

        public void OnEntityMoveInsideAOICell(AOIEntity entity)
        {
            if (_aoiCellDict.TryGetValue(entity._aoiCellKey, out AOICell? cell))
            {
                cell.OnEntityMoveInsideCell(entity);
            }
            else
            {
                SangoLogger.Warning($"AOICellDict can`t find EntityID: [ {entity._entityID} ].");
            }
        }

        public AOICell GetOrNewAOICell(AOIEntity entity)
        {
            if (_aoiCellDict.TryGetValue(entity._aoiCellKey, out AOICell? cell))
            {
                return cell;
            }
            else
            {
                AOICell newCell = new(entity._aoiCellIndex, this);
                _aoiCellDict.Add(entity._aoiCellKey, newCell);
                return newCell;
            }
        }

        private void GetAOIAroundCell(AOICell cell)
        {
            int xIndex = cell._aoiCellIndex._xIndex;
            int zIndex = cell._aoiCellIndex._zIndex;

            cell._aoiAroundCells = new AOICell[9];

            int cellAroundsindex = 0;
            for (int i = xIndex - 2; i < xIndex + 3; i++)
            {
                for (int j = zIndex - 2; j < zIndex + 3; j++)
                {
                    _stringBuilder.Clear();
                    _stringBuilder.Append(i);
                    _stringBuilder.Append('_');
                    _stringBuilder.Append(j);
                    string aoiCellKeyNew = _stringBuilder.ToString();
                    if (!_aoiCellDict.ContainsKey(aoiCellKeyNew))
                    {
                        AOICellIndex newAOICellIndex = new(i, j);
                        AOICell newCell = new AOICell(newAOICellIndex, this);
                        _aoiCellDict.Add(aoiCellKeyNew, newCell);
                        if (i > xIndex - 2 && i < xIndex + 2)
                        {
                            if (j > zIndex - 2 && j < zIndex + 2)
                            {
                                cell._aoiAroundCells[cellAroundsindex] = newCell;
                                cellAroundsindex++;
                            }
                        }
                    }

                }
            }
        }
    }
}
