using System;
using System.Collections.Generic;
using System.Text;

namespace SangoUtils.AOICells
{
    public class AOISession
    {
        public Action<string> LogInfoFunc { get; set; }
        public Action<string> LogWarningFunc { get; set; }
        public Action<string> LogErrorFunc { get; set; }
        

        public static AOISession Instance;
    }
}
