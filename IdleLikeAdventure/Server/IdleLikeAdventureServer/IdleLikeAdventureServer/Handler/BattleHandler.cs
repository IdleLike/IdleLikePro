using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetData.Message;
using NetData.OpCode;
using Photon.SocketServer;

namespace IdleLikeAdventureServer.Handler
{
    public class BattleHandler : BaseHandler
    {
        public BattleHandler()
        {
            OpCode = OpCodeModule.Battle;
            OpCodeOperation = (byte)OpCodeBattleOperation.BattleRequest;
        }

        public override void OnOperationRequest(BaseMsgData baseMsgData, SendParameters sendParameters, ClientPeer peer)
        {
            
        }
    }
}
