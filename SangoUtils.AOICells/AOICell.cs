﻿using System.Collections.Generic;

namespace SangoUtils.AOICells
{
    public class AOICell
    {
        public AOICellIndex AOICellIndex { get; private set; }
        public AOIController AOIController { get; private set; }
        public AOICell[]? AOICellsAround { get; set; }

        public AOICell[]? AOICellsUp { get; set; }
        public AOICell[]? AOICellsDown { get; set; }
        public AOICell[]? AOICellsLeft { get; set; }
        public AOICell[]? AOICellsRight { get; set; }
        public AOICell[]? AOICellsLeftUp { get; set; }
        public AOICell[]? AOICellsLeftDown { get; set; }
        public AOICell[]? AOICellsRightUp { get; set; }
        public AOICell[]? AOICellsRightDown { get; set; }

        public int ClientEntityConcernCount { get; private set; } = 0;
        public int ServerEntityConcernCount { get; private set; } = 0;

        public HashSet<AOIEntity> AOIEntityHoldSets { get; set; } = new HashSet<AOIEntity>();
        public HashSet<AOIEntity> EnterTODOAOIEntityHoldSets { get; set; } = new HashSet<AOIEntity>();
        public HashSet<AOIEntity> ExitTODOAOIEntityHoldSets { get; set; } = new HashSet<AOIEntity>();

        private AOIUpdatePacks _aoiCellOperationUpdatePacks = new AOIUpdatePacks(10, 10, 10);

        public AOICell(AOICellIndex cellIndex, AOIController aoiController)
        {
            AOICellIndex = cellIndex;
            AOIController = aoiController;
        }


        public void OnEntityEnterCell(AOIEntity entity)
        {
            if (!EnterTODOAOIEntityHoldSets.Add(entity))
            {
                AOISession.Instance.LogErrorFunc?.Invoke($"EntityID: [ {entity.EntityID} ] already exist in EnterTODOHoldSet.");
                return;
            }

            switch (entity.AOIEntityOperationCode)
            {
                case AOIEntityOperationCode.TransferEnterCell:
                    if (AOICellsAround != null)
                    {
                        entity.AddAOIAroundCells(AOICellsAround);
                        for (int i = 0; i < AOICellsAround.Length; i++)
                        {
                            AOICellsAround[i].AddCellOperation(AOICellOperationCode.EntityEnter, entity);
                        }
                    }
                    else
                    {
                        AOISession.Instance.LogErrorFunc?.Invoke($"AOICellIndex: [ {AOICellIndex.XIndex}_{AOICellIndex.ZIndex} ] has no AOIAroundCells.");
                    }
                    break;
                case AOIEntityOperationCode.MoveCrossCell:
                    switch (entity.AOICrossDirectionCode)
                    {
                        case AOICrossDirectionCode.Up:
                            OnStraightCellMove(AOICellsUp, entity);
                            break;
                        case AOICrossDirectionCode.Down:
                            OnStraightCellMove(AOICellsDown, entity);
                            break;
                        case AOICrossDirectionCode.Left:
                            OnStraightCellMove(AOICellsLeft, entity);
                            break;
                        case AOICrossDirectionCode.Right:
                            OnStraightCellMove(AOICellsRight, entity);
                            break;
                        case AOICrossDirectionCode.LeftUp:
                            OnSkewCellMove(AOICellsLeftUp, entity);
                            break;
                        case AOICrossDirectionCode.RightUp:
                            OnSkewCellMove(AOICellsRightUp, entity);
                            break;
                        case AOICrossDirectionCode.LeftDown:
                            OnSkewCellMove(AOICellsLeftDown, entity);
                            break;
                        case AOICrossDirectionCode.RightDown:
                            OnSkewCellMove(AOICellsRightDown, entity);
                            break;
                    }
                    break;
                default:
                    AOISession.Instance.LogErrorFunc?.Invoke($"EntityID: [ {entity.EntityID} ] AOIEntityOperationCode Error: [ {entity.AOIEntityOperationCode} ].");
                    break;
            }
        }

        public void OnEntityMoveInsideCell(AOIEntity entity)
        {
            if (AOICellsAround != null)
            {
                for (int i = 0; i < AOICellsAround.Length; i++)
                {
                    AOICellsAround[i].AddCellOperation(AOICellOperationCode.EntityMove, entity);
                }
            }
        }

        public void OnEntityExitCell(AOIEntity entity)
        {
            ExitTODOAOIEntityHoldSets.Add(entity);
            if (AOICellsAround != null)
            {
                for (int i = 0; i < AOICellsAround.Length; i++)
                {
                    AOICellsAround[i].AddCellOperation(AOICellOperationCode.EntityExit, entity);
                }
            }
        }

        public void CalcCellOperationCombine()
        {
            if (!_aoiCellOperationUpdatePacks.IsEmpty)
            {
                if (ClientEntityConcernCount > 0 && AOIEntityHoldSets.Count > 0)
                {
                    AOIController.OnCellOperationCombined?.Invoke(this, _aoiCellOperationUpdatePacks);
                }
                _aoiCellOperationUpdatePacks.Reset();
            }
        }

        private void OnStraightCellMove(AOICell[]? cells, AOIEntity entity)
        {
            if (cells != null)
            {
                for (int i = 0; i < 3; i++)
                {
                    entity.RemoveAOIExitCells(cells[i]);
                    cells[i].AddCellOperation(AOICellOperationCode.EntityExit, entity);
                }
                for (int j = 3; j < 6; j++)
                {
                    entity.AddAOIEnterCells(cells[j]);
                    cells[j].AddCellOperation(AOICellOperationCode.EntityEnter, entity);
                }
                for (int k = 6; k < cells.Length; k++)
                {
                    cells[k].AddCellOperation(AOICellOperationCode.EntityMove, entity);
                }
            }
        }

        private void OnSkewCellMove(AOICell[]? cells, AOIEntity entity)
        {
            if (cells != null)
            {
                for (int i = 0; i < 5; i++)
                {
                    entity.RemoveAOIExitCells(cells[i]);
                    cells[i].AddCellOperation(AOICellOperationCode.EntityExit, entity);
                }
                for (int j = 5; j < 10; j++)
                {
                    entity.AddAOIEnterCells(cells[j]);
                    cells[j].AddCellOperation(AOICellOperationCode.EntityEnter, entity);
                }
                for (int k = 10; k < cells.Length; k++)
                {
                    cells[k].AddCellOperation(AOICellOperationCode.EntityMove, entity);
                }
            }
        }

        private void AddCellOperation(AOICellOperationCode operationCode, AOIEntity entity)
        {
            //TODO ObjectPool???
            switch (operationCode)
            {
                case AOICellOperationCode.EntityEnter:
                    if (entity.AOIEntityType == AOIEntityType.Client)
                    {
                        ClientEntityConcernCount++;
                    }
                    else
                    {
                        ServerEntityConcernCount++;
                    }

                    _aoiCellOperationUpdatePacks.AOIEntityEnterPacks.Add(new AOIEntityEnterPack(entity.EntityID, entity.Transform));
                    break;
                case AOICellOperationCode.EntityMove:
                    _aoiCellOperationUpdatePacks.AOIEntityMovePacks.Add(new AOIEntityMovePack(entity.EntityID, entity.Transform));
                    break;
                case AOICellOperationCode.EntityExit:
                    if (entity.AOIEntityType == AOIEntityType.Client)
                    {
                        ClientEntityConcernCount--;
                    }
                    else
                    {
                        ServerEntityConcernCount--;
                    }


                    _aoiCellOperationUpdatePacks.AOIEntityExitPacks.Add(new AOIEntityExitPack(entity.EntityID));
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
        public int XIndex { get; set; }
        public int ZIndex { get; set; }

        public AOICellIndex(int xIndex, int zIndex)
        {
            XIndex = xIndex;
            ZIndex = zIndex;
        }
    }
}
