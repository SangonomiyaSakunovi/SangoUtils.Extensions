namespace SangoUtils_Common.Config
{
    public class SangoCommonConfig
    {
        public static readonly MapConfig SunUpF4LobbyMapConfig = new MapConfig()
        {
            MapID = "SunUpF4Lobby",
            MapName = "4楼办公室大厅",
            MapBorderX = new int[] { 0, 500 },
            MapBorderZ = new int[] { 0, 500 },
            AOIConfig = new AOIConfig()
            {
                MapID = "SunUpF4Lobby_AOI",
                CellSize = 20,
                InitCount = 200
            }
        };
    }
}
