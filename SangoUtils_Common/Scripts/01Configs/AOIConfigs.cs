namespace SangoUtils_Common.Config
{
    public class AOIConfig
    {
        public string mapID = "";
        public int cellSize = 20;
        public int initCount = 200;

        public int aoiCellOperationEnterPacksCount = 10;
        public int aoiCellOperationMovePacksCount = 50;
        public int aoiCellOperationExitPacksCount = 10;

        public int aoiEntityUpdateEnterPacksCount = 10;
        public int aoiEntityUpdateMovePacksCount = 20;
        public int aoiEntityUpdateExitPacksCount = 10;
    }

    public class MapConfig
    {
        public string mapID = "";
        public string mapName = "";

        public int[] mapBorderX = new int[] { 0, 0 };
        public int[] mapBorderZ = new int[] { 0, 0 };

        public AOIConfig aoiConfig = new AOIConfig();
    }
}
