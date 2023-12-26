using SangoUtils_Server.Core;

namespace SangoUtils_Server.DB
{
    public class ServerDBConfig : BaseConfig
    {
        private static readonly ServerDBModuleCode ServerDBModuleCode = ServerDBModuleCode.MongoDB;
        private static readonly ServerDBAddressCode ServerDBAddressCode = ServerDBAddressCode.Local;

        public static List<string> GetDBConnectionInfo()
        {
            List<string> res = new List<string>();
            switch (ServerDBModuleCode)
            {
                case ServerDBModuleCode.MongoDB:
                    switch (ServerDBAddressCode)
                    {
                        case ServerDBAddressCode.Local:
                            res.Add(ServerDBConstant.MongoDBName_Local);
                            res.Add(ServerDBConstant.MongoDBAddress_Local);
                            break;
                        case ServerDBAddressCode.Remote:
                            res.Add(ServerDBConstant.MongoDBName_Remote);
                            res.Add(ServerDBConstant.MongoDBAddress_Remote);
                            break;
                    }
                    break;
            }
            return res;
        }
    }

    public class ServerDBConstant : BaseConstant
    {
        public const string MongoDBName_Remote = "RemoteServerDB";
        public const string MongoDBAddress_Remote = "mongodb://RemoteIP:RemotePort";

        public const string MongoDBName_Local = "MongoDBSango";
        public const string MongoDBAddress_Local = "mongodb://127.0.0.1:27017";
    }

    public enum ServerDBModuleCode
    {
        MongoDB,
        MySQL
    }

    public enum ServerDBAddressCode
    {
        Local,
        Remote
    }
}
