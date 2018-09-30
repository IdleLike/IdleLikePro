using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetData.Message
{
    public class UserMsgData : BaseMsgData
    {
        public int DatabaseID;
        public string Name;
        public DateTime CreateTime;
    }
}
