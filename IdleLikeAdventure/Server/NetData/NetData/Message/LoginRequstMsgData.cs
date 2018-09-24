using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetData.Message
{
    public class LoginRequstMsgData : BaseMsgData
    {
        public string Account;
        public string Password;
        public ushort ServerID; 
    }
}
