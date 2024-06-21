using Microsoft.AspNetCore.Builder;

namespace SangoServers.Bases_ASPNet.Services
{
    public class ApplicationService
    {
        public static ApplicationService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Activator.CreateInstance<ApplicationService>();
                }
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }

        private static ApplicationService? _instance;

        public static bool IsInitialized
        {
            get
            {
                if (!_isInitlized)
                {
                    Console.WriteLine($"The App is not Initialized.");
                }
                return _isInitlized;
            }
            private set
            {
                _isInitlized = value;
            }
        }

        private static bool _isInitlized = false;

        public static WebApplication? App { get; private set; }

        public void SetWebApplication(WebApplication app)
        {
            App = app;
            _isInitlized = true;
        }
    }
}
