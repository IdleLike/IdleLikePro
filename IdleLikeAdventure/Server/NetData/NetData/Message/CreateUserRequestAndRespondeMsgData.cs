using System;
using System.Collections.Generic;

namespace NetData.Message
{
    [Serializable]
    public class CreateUserRequestAndRespondeMsgData : BaseMsgData
    {
        public int AccountID;
        public int ServerID;
        public string PlayerName;
        public string TeamName;
        public List<ActorMsgData> Actors;
    }
}
