using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetData.Message
{
    [Serializable]
    public class CreateUserRequestAndRespondeMsgData : BaseMsgData
    {
        public string UserName;
        public string TeamName;
        public List<ActorMsgData> Actors;
    }
}
