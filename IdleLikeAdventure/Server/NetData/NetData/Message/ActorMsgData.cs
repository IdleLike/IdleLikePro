using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetData.Message
{
    [Serializable]
    public class ActorMsgData
    {
        public int DataBaseID;
        public string Name;
        public int RaceID;
        public int CareerID;
        public int CareerLevel;
        public int CareerPoint;
        public int TotalExp;
        public DateTime CreateTime;
        public DateTime UpdateTime;
    }
}
