namespace SangoUtils_Common.Config
{
    public class SangoCommonConfig
    {
        public static readonly MapConfig SunUpF4LobbyMapConfig = new MapConfig()
        {
            mapID = "SunUpF4Lobby",
            mapName = "4楼办公室大厅",
            mapBorderX = new int[] { 0, 500 },
            mapBorderZ = new int[] { 0, 500 },
            aoiConfig = new AOIConfig()
            {
                mapID = "SunUpF4Lobby_AOI",
                cellSize = 20,
                initCount = 200
            }
        };
    }
}
