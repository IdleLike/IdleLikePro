using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetData.Message;
using NetData.OpCode;
using Photon.SocketServer;

namespace IdleLikeAdventureServer.Handler
{
    public class RegisterHandler : BaseHandler
    {
        public RegisterHandler()
        {
            OpCode = OpCodeModule.User;
            OpCodeOperation = (byte)OpCodeUserOperation.Register;
        }

        public override void OnOperationRequest(BaseMsgData baseMsgData, SendParameters sendParameters, ClientPeer peer)
        {
            
        }
    }
}
