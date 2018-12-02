using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetData.Message
{
    [Serializable]
    public class BattleMsgData
    {
        //战斗初始
        public int TeamID;
        public List<uint> EnemyID;
        //战斗信息
        public List<RoundInfoMsgData> Rounds;
        //战斗结算
        public bool IsWin;
        public List<int> Exps;
        public int GoldRewards;
        public List<uint> ItemRewards;
        public byte RestTime;
    }
}
