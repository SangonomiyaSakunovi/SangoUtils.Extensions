using SangoUtils_Server_Scripts;

namespace SangoUtils_Server_App
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
