namespace SangoUtils_Common.Config
{
    public class AOIConfig
    {
        public string MapID { get; set; } = "";
        public int CellSize { get; set; } = 20;
        public int InitCount { get; set; } = 200;

        public int AOICellOperationEnterPacksCount { get; set; } = 10;
        public int AOICellOperationMovePacksCount { get; set; } = 50;
        public int AOICellOperationExitPacksCount { get; set; } = 10;

        public int AOIEntityUpdateEnterPacksCount { get; set; } = 10;
        public int AOIEntityUpdateMovePacksCount { get; set; } = 20;
        public int AOIEntityUpdateExitPacksCount { get; set; } = 10;
    }

    public class MapConfig
    {
        public string MapID { get; set; } = "";
        public string MapName { get; set; } = "";

        public int[] MapBorderX { get; set; } = new int[] { 0, 0 };
        public int[] MapBorderZ { get; set; } = new int[] { 0, 0 };

        public AOIConfig AOIConfig { get; set; } = new AOIConfig();
    }
}
