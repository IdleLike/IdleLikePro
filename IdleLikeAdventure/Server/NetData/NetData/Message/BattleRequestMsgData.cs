using NetData.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetData.Message
{
    [Serializable]
    public class BattleRequestMsgData : BaseMsgData
    {
        public int TeamID;
        public EnumBattleKind BattleType;
    }
}
