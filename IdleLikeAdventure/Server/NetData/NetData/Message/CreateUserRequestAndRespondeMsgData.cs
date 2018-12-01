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
        public int PlayerID;
        public string TeamName;
        public int TeamID;
        public List<ActorMsgData> Actors;
    }
}
