using SangoScripts_Server.Logger;
using SangoUtils_Common.Config;
using SangoUtils_Common.Infos;
using System.Text;

namespace SangoScripts_Server.AOI
{
    public class AOIController(AOIConfig config) : BaseController
    {
        public string ControllerName { get; private set; } = "";

        public AOIConfig AOIConfig { get; private set; } = config;
        private Dictionary<string, AOICell> _aoiCellDict = new(config.InitCount);
        private List<AOIEntity> _aoiEntities = new();

        public Action<AOIEntity, AOIUpdatePacks>? OnEntityCellViewChanged { get; set; }
        public Action<AOICell, AOIUpdatePacks>? OnCellOperationCombined { get; set; }

        private StringBuilder _stringBuilder = new();

        public int CellSize
        {
            get
            {
                return AOIConfig.CellSize;
            }
        }

        public AOIEntity OnEntityEnterCell(string entityID, Transform transform, AOIEntityType entityType)
        {
            AOIEntity entity = new AOIEntity(entityID, this, entityType);
            entity.OnUpdatePosition(transform, AOIEntityOperationCode.TransferEnterCell);
            _aoiEntities.Add(entity);
            return entity;
        }

        public void OnEntityMove(AOIEntity entity, Transform transform)
        {
            entity.OnUpdatePosition(transform, AOIEntityOperationCode.MoveCrossCell);
        }

        public void OnEntityExitCell(AOIEntity entity)
        {
            if (_aoiCellDict.TryGetValue(entity.AOICellKey, out AOICell? cell))
            {
                cell.OnEntityExitCell(entity);
            }
            else
            {
                SangoLogger.Error($"AOICellDict can`t find EntityKey: [ {entity.AOICellKey} ].");
            }
            if (!_aoiEntities.Remove(entity))
            {
                SangoLogger.Error($"AOIEntityList can`t find EntityKey: [ {entity.AOICellKey} ].");
            }
        }

        public void OnAOIUpdate()
        {
            for (int i = 0; i < _aoiEntities.Count; i++)
            {
                _aoiEntities[i].CalcEntityCellAndAroundViewChanged();
            }

            foreach (AOICell cell in _aoiCellDict.Values)
            {
                if (cell.ExitTODOAOIEntityHoldSets.Count > 0)
                {
                    cell.AOIEntityHoldSets.ExceptWith(cell.ExitTODOAOIEntityHoldSets);
                    cell.ExitTODOAOIEntityHoldSets.Clear();
                }

                if (cell.EnterTODOAOIEntityHoldSets.Count > 0)
                {
                    cell.AOIEntityHoldSets.UnionWith(cell.EnterTODOAOIEntityHoldSets);
                    cell.EnterTODOAOIEntityHoldSets.Clear();
                }
                cell.CalcCellOperationCombine();
            }
        }

        public void OnEntityMoveCrossAOICell(AOIEntity newEntity)
        {
            AOICell cell = GetOrNewAOICell(newEntity);
            if (cell.AOICellsAround == null)
            {
                GetAOIAroundCell(cell);
            }


            cell.OnEntityEnterCell(newEntity);
        }

        public void OnEntityMoveInsideAOICell(AOIEntity entity)
        {
            if (_aoiCellDict.TryGetValue(entity.AOICellKey, out AOICell? cell))
            {
                cell.OnEntityMoveInsideCell(entity);
            }
            else
            {
                SangoLogger.Warning($"AOICellDict can`t find AOICellKey: [ {entity.AOICellKey} ].");
            }
        }

        public AOICell GetOrNewAOICell(AOIEntity entity)
        {
            if (_aoiCellDict.TryGetValue(entity.AOICellKey, out AOICell? cell))
            {
                return cell;
            }
            else
            {
                AOICell newCell = new(entity.AOICellIndex, this);
                _aoiCellDict.Add(entity.AOICellKey, newCell);
                return newCell;
            }
        }

        public void MarkExitEntityCell(AOIEntity entity)
        {
            if (_aoiCellDict.TryGetValue(entity.AOICellKey, out AOICell? cell))
            {
                cell.ExitTODOAOIEntityHoldSets.Add(entity);
            }
            else
            {
                SangoLogger.Warning($"AOICellDict can`t find AOICellKey when MarkExitEntityCell: [ {entity.AOICellKey} ].");
            }
        }

        private void GetAOIAroundCell(AOICell cell)
        {
            int xIndex = cell.AOICellIndex.XIndex;
            int zIndex = cell.AOICellIndex.ZIndex;

            cell.AOICellsAround = new AOICell[9];

            int cellAroundsindex = 0;
            for (int i = xIndex - 2; i < xIndex + 3; i++)
            {
                for (int j = zIndex - 2; j < zIndex + 3; j++)
                {
                    string aoiCellKeyNew = GetAOICellKey(i, j);
                    if (!_aoiCellDict.TryGetValue(aoiCellKeyNew, out AOICell? aoiCell))
                    {
                        AOICellIndex newAOICellIndex = new(i, j);
                        aoiCell = new AOICell(newAOICellIndex, this);
                        _aoiCellDict.Add(aoiCellKeyNew, aoiCell);                       
                    }
                    if (i > xIndex - 2 && i < xIndex + 2)
                    {
                        if (j > zIndex - 2 && j < zIndex + 2)
                        {
                            cell.AOICellsAround[cellAroundsindex] = aoiCell;
                            cellAroundsindex++;
                        }
                    }
                }
            }

            {
                //Protocol: 0-2: ExitCells, 3-5: EnterCells, 6-11:MoveEventCells
                {
                    cell.AOICellsUp = new AOICell[12];
                    cell.AOICellsUp[0] = _aoiCellDict[GetAOICellKey(xIndex - 1, zIndex - 2)];
                    cell.AOICellsUp[1] = _aoiCellDict[GetAOICellKey(xIndex, zIndex - 2)];
                    cell.AOICellsUp[2] = _aoiCellDict[GetAOICellKey(xIndex + 1, zIndex - 2)];

                    cell.AOICellsUp[3] = _aoiCellDict[GetAOICellKey(xIndex - 1, zIndex + 1)];
                    cell.AOICellsUp[4] = _aoiCellDict[GetAOICellKey(xIndex, zIndex + 1)];
                    cell.AOICellsUp[5] = _aoiCellDict[GetAOICellKey(xIndex + 1, zIndex + 1)];

                    cell.AOICellsUp[6] = _aoiCellDict[GetAOICellKey(xIndex - 1, zIndex)];
                    cell.AOICellsUp[7] = _aoiCellDict[GetAOICellKey(xIndex, zIndex)];
                    cell.AOICellsUp[8] = _aoiCellDict[GetAOICellKey(xIndex + 1, zIndex)];
                    cell.AOICellsUp[9] = _aoiCellDict[GetAOICellKey(xIndex - 1, zIndex - 1)];
                    cell.AOICellsUp[10] = _aoiCellDict[GetAOICellKey(xIndex, zIndex - 1)];
                    cell.AOICellsUp[11] = _aoiCellDict[GetAOICellKey(xIndex + 1, zIndex - 1)];
                }
                {
                    cell.AOICellsDown = new AOICell[12];
                    cell.AOICellsDown[0] = _aoiCellDict[GetAOICellKey(xIndex - 1, zIndex + 2)];
                    cell.AOICellsDown[1] = _aoiCellDict[GetAOICellKey(xIndex, zIndex + 2)];
                    cell.AOICellsDown[2] = _aoiCellDict[GetAOICellKey(xIndex + 1, zIndex + 2)];

                    cell.AOICellsDown[3] = _aoiCellDict[GetAOICellKey(xIndex - 1, zIndex - 1)];
                    cell.AOICellsDown[4] = _aoiCellDict[GetAOICellKey(xIndex, zIndex - 1)];
                    cell.AOICellsDown[5] = _aoiCellDict[GetAOICellKey(xIndex + 1, zIndex - 1)];

                    cell.AOICellsDown[6] = _aoiCellDict[GetAOICellKey(xIndex - 1, zIndex + 1)];
                    cell.AOICellsDown[7] = _aoiCellDict[GetAOICellKey(xIndex, zIndex + 1)];
                    cell.AOICellsDown[8] = _aoiCellDict[GetAOICellKey(xIndex + 1, zIndex + 1)];
                    cell.AOICellsDown[9] = _aoiCellDict[GetAOICellKey(xIndex - 1, zIndex)];
                    cell.AOICellsDown[10] = _aoiCellDict[GetAOICellKey(xIndex, zIndex)];
                    cell.AOICellsDown[11] = _aoiCellDict[GetAOICellKey(xIndex + 1, zIndex)];
                }
                {
                    cell.AOICellsLeft = new AOICell[12];
                    cell.AOICellsLeft[0] = _aoiCellDict[GetAOICellKey(xIndex + 2, zIndex + 1)];
                    cell.AOICellsLeft[1] = _aoiCellDict[GetAOICellKey(xIndex + 2, zIndex)];
                    cell.AOICellsLeft[2] = _aoiCellDict[GetAOICellKey(xIndex + 2, zIndex - 1)];

                    cell.AOICellsLeft[3] = _aoiCellDict[GetAOICellKey(xIndex - 1, zIndex + 1)];
                    cell.AOICellsLeft[4] = _aoiCellDict[GetAOICellKey(xIndex - 1, zIndex)];
                    cell.AOICellsLeft[5] = _aoiCellDict[GetAOICellKey(xIndex - 1, zIndex - 1)];

                    cell.AOICellsLeft[6] = _aoiCellDict[GetAOICellKey(xIndex, zIndex + 1)];
                    cell.AOICellsLeft[7] = _aoiCellDict[GetAOICellKey(xIndex, zIndex)];
                    cell.AOICellsLeft[8] = _aoiCellDict[GetAOICellKey(xIndex, zIndex - 1)];
                    cell.AOICellsLeft[9] = _aoiCellDict[GetAOICellKey(xIndex + 1, zIndex + 1)];
                    cell.AOICellsLeft[10] = _aoiCellDict[GetAOICellKey(xIndex + 1, zIndex)];
                    cell.AOICellsLeft[11] = _aoiCellDict[GetAOICellKey(xIndex + 1, zIndex - 1)];
                }
                {
                    cell.AOICellsRight = new AOICell[12];
                    cell.AOICellsRight[0] = _aoiCellDict[GetAOICellKey(xIndex - 2, zIndex + 1)];
                    cell.AOICellsRight[1] = _aoiCellDict[GetAOICellKey(xIndex - 2, zIndex)];
                    cell.AOICellsRight[2] = _aoiCellDict[GetAOICellKey(xIndex - 2, zIndex - 1)];

                    cell.AOICellsRight[3] = _aoiCellDict[GetAOICellKey(xIndex + 1, zIndex + 1)];
                    cell.AOICellsRight[4] = _aoiCellDict[GetAOICellKey(xIndex + 1, zIndex)];
                    cell.AOICellsRight[5] = _aoiCellDict[GetAOICellKey(xIndex + 1, zIndex - 1)];

                    cell.AOICellsRight[6] = _aoiCellDict[GetAOICellKey(xIndex - 1, zIndex + 1)];
                    cell.AOICellsRight[7] = _aoiCellDict[GetAOICellKey(xIndex - 1, zIndex)];
                    cell.AOICellsRight[8] = _aoiCellDict[GetAOICellKey(xIndex - 1, zIndex - 1)];
                    cell.AOICellsRight[9] = _aoiCellDict[GetAOICellKey(xIndex, zIndex + 1)];
                    cell.AOICellsRight[10] = _aoiCellDict[GetAOICellKey(xIndex, zIndex)];
                    cell.AOICellsRight[11] = _aoiCellDict[GetAOICellKey(xIndex, zIndex - 1)];
                }
                //Protocol: 0-4: ExitCells, 5-9: EnterCells, 10-13:MoveEventCells
                {
                    cell.AOICellsLeftUp = new AOICell[14];
                    cell.AOICellsLeftUp[0] = _aoiCellDict[GetAOICellKey(xIndex, zIndex - 2)];
                    cell.AOICellsLeftUp[1] = _aoiCellDict[GetAOICellKey(xIndex + 1, zIndex - 2)];
                    cell.AOICellsLeftUp[2] = _aoiCellDict[GetAOICellKey(xIndex + 2, zIndex - 2)];
                    cell.AOICellsLeftUp[3] = _aoiCellDict[GetAOICellKey(xIndex + 2, zIndex - 1)];
                    cell.AOICellsLeftUp[4] = _aoiCellDict[GetAOICellKey(xIndex + 2, zIndex)];

                    cell.AOICellsLeftUp[5] = _aoiCellDict[GetAOICellKey(xIndex - 1, zIndex - 1)];
                    cell.AOICellsLeftUp[6] = _aoiCellDict[GetAOICellKey(xIndex - 1, zIndex)];
                    cell.AOICellsLeftUp[7] = _aoiCellDict[GetAOICellKey(xIndex - 1, zIndex + 1)];
                    cell.AOICellsLeftUp[8] = _aoiCellDict[GetAOICellKey(xIndex, zIndex + 1)];
                    cell.AOICellsLeftUp[9] = _aoiCellDict[GetAOICellKey(xIndex + 1, zIndex + 1)];

                    cell.AOICellsLeftUp[10] = _aoiCellDict[GetAOICellKey(xIndex, zIndex)];
                    cell.AOICellsLeftUp[11] = _aoiCellDict[GetAOICellKey(xIndex + 1, zIndex)];
                    cell.AOICellsLeftUp[12] = _aoiCellDict[GetAOICellKey(xIndex, zIndex - 1)];
                    cell.AOICellsLeftUp[13] = _aoiCellDict[GetAOICellKey(xIndex + 1, zIndex - 1)];
                }
                {
                    cell.AOICellsRightUp = new AOICell[14];
                    cell.AOICellsRightUp[0] = _aoiCellDict[GetAOICellKey(xIndex - 2, zIndex)];
                    cell.AOICellsRightUp[1] = _aoiCellDict[GetAOICellKey(xIndex - 2, zIndex - 1)];
                    cell.AOICellsRightUp[2] = _aoiCellDict[GetAOICellKey(xIndex - 2, zIndex - 2)];
                    cell.AOICellsRightUp[3] = _aoiCellDict[GetAOICellKey(xIndex - 1, zIndex - 2)];
                    cell.AOICellsRightUp[4] = _aoiCellDict[GetAOICellKey(xIndex, zIndex - 2)];

                    cell.AOICellsRightUp[5] = _aoiCellDict[GetAOICellKey(xIndex - 1, zIndex + 1)];
                    cell.AOICellsRightUp[6] = _aoiCellDict[GetAOICellKey(xIndex, zIndex + 1)];
                    cell.AOICellsRightUp[7] = _aoiCellDict[GetAOICellKey(xIndex + 1, zIndex + 1)];
                    cell.AOICellsRightUp[8] = _aoiCellDict[GetAOICellKey(xIndex + 1, zIndex)];
                    cell.AOICellsRightUp[9] = _aoiCellDict[GetAOICellKey(xIndex + 1, zIndex - 1)];

                    cell.AOICellsRightUp[10] = _aoiCellDict[GetAOICellKey(xIndex - 1, zIndex)];
                    cell.AOICellsRightUp[11] = _aoiCellDict[GetAOICellKey(xIndex, zIndex)];
                    cell.AOICellsRightUp[12] = _aoiCellDict[GetAOICellKey(xIndex - 1, zIndex - 1)];
                    cell.AOICellsRightUp[13] = _aoiCellDict[GetAOICellKey(xIndex, zIndex - 1)];
                }
                {
                    cell.AOICellsLeftDown = new AOICell[14];
                    cell.AOICellsLeftDown[0] = _aoiCellDict[GetAOICellKey(xIndex, zIndex + 2)];
                    cell.AOICellsLeftDown[1] = _aoiCellDict[GetAOICellKey(xIndex + 1, zIndex + 2)];
                    cell.AOICellsLeftDown[2] = _aoiCellDict[GetAOICellKey(xIndex + 2, zIndex + 2)];
                    cell.AOICellsLeftDown[3] = _aoiCellDict[GetAOICellKey(xIndex + 2, zIndex + 1)];
                    cell.AOICellsLeftDown[4] = _aoiCellDict[GetAOICellKey(xIndex + 2, zIndex)];

                    cell.AOICellsLeftDown[5] = _aoiCellDict[GetAOICellKey(xIndex - 1, zIndex + 1)];
                    cell.AOICellsLeftDown[6] = _aoiCellDict[GetAOICellKey(xIndex - 1, zIndex)];
                    cell.AOICellsLeftDown[7] = _aoiCellDict[GetAOICellKey(xIndex - 1, zIndex - 1)];
                    cell.AOICellsLeftDown[8] = _aoiCellDict[GetAOICellKey(xIndex, zIndex - 1)];
                    cell.AOICellsLeftDown[9] = _aoiCellDict[GetAOICellKey(xIndex + 1, zIndex - 1)];

                    cell.AOICellsLeftDown[10] = _aoiCellDict[GetAOICellKey(xIndex, zIndex + 1)];
                    cell.AOICellsLeftDown[11] = _aoiCellDict[GetAOICellKey(xIndex + 1, zIndex + 1)];
                    cell.AOICellsLeftDown[12] = _aoiCellDict[GetAOICellKey(xIndex, zIndex)];
                    cell.AOICellsLeftDown[13] = _aoiCellDict[GetAOICellKey(xIndex + 1, zIndex)];
                }
                {
                    cell.AOICellsRightDown = new AOICell[14];
                    cell.AOICellsRightDown[0] = _aoiCellDict[GetAOICellKey(xIndex - 2, zIndex + 2)];
                    cell.AOICellsRightDown[1] = _aoiCellDict[GetAOICellKey(xIndex - 2, zIndex + 1)];
                    cell.AOICellsRightDown[2] = _aoiCellDict[GetAOICellKey(xIndex - 2, zIndex)];
                    cell.AOICellsRightDown[3] = _aoiCellDict[GetAOICellKey(xIndex - 1, zIndex + 2)];
                    cell.AOICellsRightDown[4] = _aoiCellDict[GetAOICellKey(xIndex, zIndex + 2)];

                    cell.AOICellsRightDown[5] = _aoiCellDict[GetAOICellKey(xIndex - 1, zIndex - 1)];
                    cell.AOICellsRightDown[6] = _aoiCellDict[GetAOICellKey(xIndex, zIndex - 1)];
                    cell.AOICellsRightDown[7] = _aoiCellDict[GetAOICellKey(xIndex + 1, zIndex - 1)];
                    cell.AOICellsRightDown[8] = _aoiCellDict[GetAOICellKey(xIndex + 1, zIndex)];
                    cell.AOICellsRightDown[9] = _aoiCellDict[GetAOICellKey(xIndex + 1, zIndex + 1)];

                    cell.AOICellsRightDown[10] = _aoiCellDict[GetAOICellKey(xIndex - 1, zIndex + 1)];
                    cell.AOICellsRightDown[11] = _aoiCellDict[GetAOICellKey(xIndex, zIndex + 1)];
                    cell.AOICellsRightDown[12] = _aoiCellDict[GetAOICellKey(xIndex - 1, zIndex)];
                    cell.AOICellsRightDown[13] = _aoiCellDict[GetAOICellKey(xIndex, zIndex)];
                }
            }
        }

        public string GetAOICellKey(int key1, int key2)
        {
            _stringBuilder.Clear();
            _stringBuilder.Append(key1);
            _stringBuilder.Append('_');
            _stringBuilder.Append(key2);
            return _stringBuilder.ToString();
        }

        public Dictionary<string, AOICell> GetExistCellDict()
        {
            return _aoiCellDict;
        }
    }
}
