using SangoScripts_Server.Logger;

namespace SangoScripts_Server.AOI
{
    public class AOIEntity(string entityID, AOIController aoiController, AOIEntityType entityDriverCode)
    {
        public string EntityID { get; private set; } = entityID;
        public AOIController AOIController { get; private set; } = aoiController;
        public AOIEntityType AOIEntityType { get; private set; } = entityDriverCode;
        private AOIUpdatePacks _aoiEntityOperationUpdatePacks = new(aoiController.AOIConfig.AOIEntityUpdateEnterPacksCount, aoiController.AOIConfig.AOIEntityUpdateMovePacksCount, aoiController.AOIConfig.AOIEntityUpdateExitPacksCount);


        public TransformData Transform { get; private set; } = new();
        public AOIEntityOperationCode AOIEntityOperationCode { get; private set; }

        public AOICrossDirectionCode AOICrossDirectionCode { get; private set; }

        private AOICellIndex _aoiCellIndexLast = new(0, 0);
        public AOICellIndex AOICellIndex { get; private set; } = new(0, 0);

        private string _aoiCellKeyLast = "";
        public string AOICellKey { get; private set; } = "";

        private AOICell[]? _aoiAroundCells = null;
        private List<AOICell> _aoiRemoveCells = new(5);
        private List<AOICell> _aoiAddCells = new(5);

        public void OnUpdatePosition(TransformData transform, AOIEntityOperationCode operationCode = AOIEntityOperationCode.None)
        {
            Transform = transform;
            AOIEntityOperationCode = operationCode;

            int xIndex = (int)Math.Floor(transform.Position.X / AOIController.CellSize);
            int zIndex = (int)Math.Floor(transform.Position.Z / AOIController.CellSize);

            string aoiCellKeyNew = AOIController.GetAOICellKey(xIndex, zIndex);

            SangoLogger.Log("New AOICell Key in AOIEntity: [ " + aoiCellKeyNew + " ]");

            if (aoiCellKeyNew != AOICellKey)
            {
                _aoiCellIndexLast = AOICellIndex;
                _aoiCellKeyLast = AOICellKey;

                if (AOICellKey != "")
                {
                    AOIController.MarkExitEntityCell(this);
                }

                AOICellIndex = new(xIndex, zIndex);
                AOICellKey = aoiCellKeyNew;

                if (AOIEntityOperationCode != AOIEntityOperationCode.TransferEnterCell && AOIEntityOperationCode != AOIEntityOperationCode.TransferExitCell)
                {
                    AOIEntityOperationCode = AOIEntityOperationCode.MoveCrossCell;
                    if (AOICellIndex.XIndex < _aoiCellIndexLast.XIndex)
                    {
                        if (AOICellIndex.ZIndex == _aoiCellIndexLast.ZIndex)
                        {
                            AOICrossDirectionCode = AOICrossDirectionCode.Left;
                        }
                        else if (AOICellIndex.ZIndex < _aoiCellIndexLast.ZIndex)
                        {
                            AOICrossDirectionCode = AOICrossDirectionCode.LeftDown;
                        }
                        else
                        {
                            AOICrossDirectionCode = AOICrossDirectionCode.LeftUp;
                        }
                    }
                    else if (AOICellIndex.XIndex > _aoiCellIndexLast.XIndex)
                    {
                        if (AOICellIndex.ZIndex == _aoiCellIndexLast.ZIndex)
                        {
                            AOICrossDirectionCode = AOICrossDirectionCode.Right;
                        }
                        else if (AOICellIndex.ZIndex < _aoiCellIndexLast.ZIndex)
                        {
                            AOICrossDirectionCode = AOICrossDirectionCode.RightDown;
                        }
                        else
                        {
                            AOICrossDirectionCode = AOICrossDirectionCode.RightUp;
                        }
                    }
                    else
                    {
                        if (AOICellIndex.ZIndex > _aoiCellIndexLast.ZIndex)
                        {
                            AOICrossDirectionCode = AOICrossDirectionCode.Up;
                        }
                        else
                        {
                            AOICrossDirectionCode = AOICrossDirectionCode.Down;
                        }
                    }
                }
                AOIController.OnEntityMoveCrossAOICell(this);
            }
            else
            {
                AOIEntityOperationCode = AOIEntityOperationCode.MoveInsideCell;
                AOICrossDirectionCode = AOICrossDirectionCode.None;
                AOIController.OnEntityMoveInsideAOICell(this);
            }
        }

        public void AddAOIAroundCells(AOICell[] aoiAroundCells)
        {
            if (AOIEntityType == AOIEntityType.Client)
            {
                _aoiAroundCells = aoiAroundCells;
            }
        }

        public void AddAOIEnterCells(AOICell cell)
        {
            if (AOIEntityType == AOIEntityType.Client)
            {
                _aoiAddCells.Add(cell);
            }
        }

        public void RemoveAOIExitCells(AOICell cell)
        {
            if (AOIEntityType == AOIEntityType.Client)
            {
                _aoiRemoveCells.Add(cell);
            }
        }

        public void CalcEntityCellAndAroundViewChanged()
        {
            AOICell cell = AOIController.GetOrNewAOICell(this);
            if (cell.ClientEntityConcernCount > 0 && AOIEntityType == AOIEntityType.Client)
            {
                if (_aoiAroundCells != null)
                {
                    for (int i = 0; i < _aoiAroundCells.Length; i++)
                    {
                        foreach (AOIEntity entity in _aoiAroundCells[i].AOIEntityHoldSets)
                        {
                            _aoiEntityOperationUpdatePacks.AOIEntityEnterPacks.Add(new AOIEntityEnterPack(entity.EntityID, entity.Transform));
                        }
                    }
                }

                for (int j = 0; j < _aoiAddCells.Count; j++)
                {
                    foreach (AOIEntity entity in _aoiAddCells[j].AOIEntityHoldSets)
                    {
                        _aoiEntityOperationUpdatePacks.AOIEntityEnterPacks.Add(new AOIEntityEnterPack(entity.EntityID, entity.Transform));
                    }
                }

                for (int k = 0; k < _aoiRemoveCells.Count; k++)
                {
                    foreach (AOIEntity entity in _aoiRemoveCells[k].AOIEntityHoldSets)
                    {
                        _aoiEntityOperationUpdatePacks.AOIEntityExitPacks.Add(new AOIEntityExitPack(entity.EntityID));
                    }
                }

                if (!_aoiEntityOperationUpdatePacks.IsEmpty)
                {
                    AOIController.OnEntityCellViewChanged?.Invoke(this, _aoiEntityOperationUpdatePacks);
                    _aoiEntityOperationUpdatePacks.Reset();
                }
            }
            _aoiAroundCells = null;
            _aoiAddCells.Clear();
            _aoiRemoveCells.Clear();
        }
    }

    public enum AOIEntityOperationCode
    {
        None = 0,
        TransferEnterCell,
        TransferExitCell,
        MoveCrossCell,
        MoveInsideCell
    }

    public enum AOICrossDirectionCode
    {
        None = 0,
        Up,
        Down,
        Left,
        Right,
        LeftUp,
        RightUp,
        LeftDown,
        RightDown
    }

    public enum AOIEntityType
    {
        None = 0,
        Client,
        Server
    }
}
