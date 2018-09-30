using NetData.OpCode;
using System;

namespace NetData.Message
{
    [Serializable]
    public class BaseMsgData
    {
        public bool IsError = false;
        public ErrorCode Error = 0;
    }
}
