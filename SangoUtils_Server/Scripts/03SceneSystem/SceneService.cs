using SangoScripts_Server;

namespace SangoUtils_Server
{
    public class SceneService : BaseService<SceneService>
    {
        protected override void OnUpdate()
        {
            base.OnUpdate();
            SceneTestMain.Instance.Update();
        }
    }
}
