using SangoScripts_Server;

namespace SangoUtils_Server
{
    public class RegistSystem
    {

    }

    public class RegistRegularConfig : BaseConfig
    {
        public string GuestIDPrefix { get; set; } = "_SangoGuest_";
        public TransformData DefaultTransform { get; set; } = new(new(0, 0, 0), new(0, 0, 0, 0), new(1, 1, 1));
    }
}
