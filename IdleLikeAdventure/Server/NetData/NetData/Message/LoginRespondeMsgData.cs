using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetData.Message
{
    [Serializable]
    public class LoginRespondeMsgData : BaseMsgData
    {
        public UserMsgData userData;
    }
}
