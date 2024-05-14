using SangoUtils.Bases_Unity;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SangoUtils.Engines_Unity.Services
{
    public class NetService_Sockets
    {
#pragma warning disable CS8618
        public static NetService_Sockets Instance { get; private set; }

        private NetService_SocketsConfig _netService_SocketsConfig;
#pragma warning restore CS8618
        public Action<string> LogErrorFunc { get; set; } = Debug.LogError;

        public void Initialize(NetService_SocketsConfig config) 
        { 
            if(Instance != null)
            {
                LogErrorFunc("NetService_Sockets is already initialized.");
                return;
            }

            Instance = this;
            _netService_SocketsConfig = config;
        }             
    }
}
