﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetData.Message
{
    [Serializable]
    public class BattleRespondeMsgData : BaseMsgData
    {
        public int FindEnemyTime;
        public BattleMsgData BattleInfo;
    }
}
