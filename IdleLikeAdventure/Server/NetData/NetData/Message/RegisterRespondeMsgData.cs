using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetData.Message
{
    [Serializable]
    public class RegisterRespondeMsgData : BaseMsgData
    {
        public UserMsgData userData;
    }
}
