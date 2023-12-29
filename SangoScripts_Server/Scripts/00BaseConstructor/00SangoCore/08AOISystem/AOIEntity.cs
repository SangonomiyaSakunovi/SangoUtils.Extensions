using System.Numerics;
using System.Text;

namespace SangoScripts_Server.AOI
{
    public class AOIEntity
    {
        public string _entityID;
        public AOIController _aoiController;

        public AOICellIndex _aoiCellIndexHistory = new(0, 0);
        public AOICellIndex _aoiCellIndex = new(0, 0);

        public string _aoiCellKeyHistory = "";
        public string _aoiCellKey = "";

        private Vector3 _position;
        private AOIEntityOperationCode _aoiEntityOperationCode;
        private StringBuilder _stringBuilder = new StringBuilder();

        private AOICell[]? _aoiAroundCells = null;

        private AOIPackController _aoiEntityUpdatePackController;

        public AOIEntity(string entityID, AOIController aoiController)
        {
            _entityID = entityID;
            _aoiController = aoiController;

            _aoiEntityUpdatePackController = new(_aoiController.AOIConfig.aoiEntityUpdateEnterPacksCount, _aoiController.AOIConfig.aoiEntityUpdateMovePacksCount, _aoiController.AOIConfig.aoiEntityUpdateExitPacksCount);
        }

        public Vector3 Position { get { return _position; } }
        public AOIEntityOperationCode AOIEntityOperationCode { get { return _aoiEntityOperationCode; } }

        public void OnUpdatePosition(Vector3 position, AOIEntityOperationCode operationCode = AOIEntityOperationCode.None)
        {
            _position = position;
            _aoiEntityOperationCode = operationCode;

            int xIndex = (int)Math.Floor(position.X / _aoiController.CellSize);
            int zIndex = (int)Math.Floor(position.Z / _aoiController.CellSize);

            _stringBuilder.Clear();
            _stringBuilder.Append(xIndex);
            _stringBuilder.Append('_');
            _stringBuilder.Append(zIndex);
            string aoiCellKeyNew = _stringBuilder.ToString();
            if (aoiCellKeyNew != _aoiCellKey)
            {
                _aoiCellIndexHistory = _aoiCellIndex;
                _aoiCellIndex = new(xIndex, zIndex);
                _aoiCellKeyHistory = _aoiCellKey;
                _aoiCellKey = aoiCellKeyNew;

                if (_aoiEntityOperationCode != AOIEntityOperationCode.TransferEnterCell && _aoiEntityOperationCode != AOIEntityOperationCode.TransferExitCell)
                {
                    _aoiEntityOperationCode = AOIEntityOperationCode.MoveCrossCell;
                }
                _aoiController.OnEntityMoveCrossAOICell(this);
            }
            else
            {
                _aoiEntityOperationCode = AOIEntityOperationCode.MoveInsideCell;
                _aoiController.OnEntityMoveInsideAOICell(this);
            }
        }

        public void AddAOIAroundCells(AOICell[] aoiAroundCells)
        {
            _aoiAroundCells = aoiAroundCells;
        }

        public void CalcEntityCellAndAroundViewChanged()
        {
            if (_aoiAroundCells != null)
            {
                for (int i = 0; i < _aoiAroundCells.Length; i++)
                {
                    HashSet<AOIEntity> set = _aoiAroundCells[i]._holdSet;
                    foreach (AOIEntity entity in set)
                    {
                        _aoiEntityUpdatePackController._aoiEntityEnterPacks.Add(new AOIEntityEnterPack(entity._entityID, entity.Position));
                    }
                }

                if (!_aoiEntityUpdatePackController.IsEmpty)
                {
                    _aoiController.OnEntityCellViewChanged?.Invoke(this, _aoiEntityUpdatePackController);
                    _aoiEntityUpdatePackController.Reset();
                }

                _aoiAroundCells = null;
            }
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
}
