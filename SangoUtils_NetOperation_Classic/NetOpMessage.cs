using System;

namespace SangoUtils.NetOperations
{
    [Serializable]
    public class NetOpMessage
    {
        public NetOpMessage() { }

        public NetOpMessage(int opCMD, int opCode, string message)
        {
            OpCMD = opCMD;
            OpCode = opCode;
            Message = message;
        }

        /// <summary>
        /// 1-Request 2-Response 3-Event
        /// </summary>
        public int OpCMD { get; set; } = 1;
        /// <summary>
        /// Custom Definition
        /// </summary>
        public int OpCode { get; set; } = 1;
        /// <summary>
        /// The message string
        /// </summary>
        public string Message { get; set; } = "";
    }
}
