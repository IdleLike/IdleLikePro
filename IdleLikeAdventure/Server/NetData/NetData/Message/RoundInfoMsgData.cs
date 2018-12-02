using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetData.Message
{
    [Serializable]
    public class RoundInfoMsgData
    {
        public List<AttackInfoMsgData> AttackInfos;
    }
}
