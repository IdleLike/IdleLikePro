using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetData.Message
{
    [Serializable]
    public class CreateUserRequestAndRespondeMsgData : BaseMsgData
    {
        public int AccountID;
        public string PlayerName;
        public string TeamName;
        public List<ActorMsgData> Actors;
    }
}
