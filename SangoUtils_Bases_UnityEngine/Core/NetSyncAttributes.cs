﻿using System;

namespace SangoUtils.Bases_Unity
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class NetSyncComponentAttribute : Attribute
    {
        public int NetSyncGroupID { get; private set; }

        public NetSyncComponentAttribute(int netSyncGroupID = 0)
        {
            NetSyncGroupID = netSyncGroupID;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class NetSyncSubspaceObjectAttribute : NetSyncComponentAttribute
    {
        public NetSyncSubspaceObjectAttribute(int netSyncGroupID = 0) : base(netSyncGroupID)
        {

        }
    }

}
