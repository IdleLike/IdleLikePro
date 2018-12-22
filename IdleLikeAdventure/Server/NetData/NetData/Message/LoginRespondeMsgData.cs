using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetData.Message
{
    [Serializable]
    public class LoginRespondeMsgData : BaseMsgData
    {
        public bool IsNewPlayer;
        public int AccountID;
        public PlayerMsgData Player;
        public List<TeamMsgData> Teams;
        public List<ActorMsgData> Actors;
    }
}
