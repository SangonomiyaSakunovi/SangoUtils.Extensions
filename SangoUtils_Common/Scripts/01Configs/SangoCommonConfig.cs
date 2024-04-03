namespace SangoUtils.Common.Config
{
    public class SangoCommonConfig
    {
        public static readonly SceneConfig SceneSunUpF4LobbyConfig = new SceneConfig()
        {
            SceneID = "SunUpF4Lobby",
            SceneName = "4楼办公室大厅",
            SceneBorderX = new int[] { 0, 500 },
            SceneBorderZ = new int[] { 0, 500 },
            AOIConfig = new AOIConfig()
            {
                SceneID = "SunUpF4Lobby_AOI",
                CellSize = 20,
                InitCount = 200
            }
        };

        public static readonly SceneConfig SceneTestMainConfig = new SceneConfig()
        {
            SceneID = "SangoTestMain",
            SceneName = "珊瑚宫测试用主场景",
            SceneBorderX = new int[] { 0, 100 },
            SceneBorderZ = new int[] { 0, 100 },
            AOIConfig = new AOIConfig()
            {
                SceneID = "SangoTestMain_AOI",
                CellSize = 10,
                InitCount = 100
            }
        };
    }
}
