using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetData.Message
{
    public class RegisterRequestMsgData : BaseMsgData
    {
        public string Account;
        public string Password;
        public ushort ServerID;
    }
}
