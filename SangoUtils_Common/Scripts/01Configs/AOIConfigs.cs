namespace SangoUtils.Commons.Config
{
    public class AOIConfig
    {
        public string SceneID { get; set; } = "";
        public int CellSize { get; set; } = 20;
        public int InitCount { get; set; } = 200;

        public int AOICellOperationEnterPacksCount { get; set; } = 10;
        public int AOICellOperationMovePacksCount { get; set; } = 50;
        public int AOICellOperationExitPacksCount { get; set; } = 10;

        public int AOIEntityUpdateEnterPacksCount { get; set; } = 10;
        public int AOIEntityUpdateMovePacksCount { get; set; } = 20;
        public int AOIEntityUpdateExitPacksCount { get; set; } = 10;
    }

    public class SceneConfig
    {
        public string SceneID { get; set; } = "";
        public string SceneName { get; set; } = "";

        public int[] SceneBorderX { get; set; } = new int[] { 0, 0 };
        public int[] SceneBorderZ { get; set; } = new int[] { 0, 0 };

        public AOIConfig AOIConfig { get; set; } = new AOIConfig();
    }
}
